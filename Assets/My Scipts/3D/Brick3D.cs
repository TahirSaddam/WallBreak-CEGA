using UnityEngine;
using System.Collections;
using System;
using Unity.Mathematics;

public class Brick3D : MonoBehaviour, IOption
{
    [Header("Impact Settings")]
    public float impactRadius = 1f;
    public float impactForce = 1000f;
    public float fallForce = 300f;

    [Header("Other Settings")]
    public float hitDelay = 0.2f;
    public bool isDetached = false;
    public Texture2D directHitTex;
    public Texture2D[] impactedTex;

    [Header("Brick Materials")]
    public Material detachedMat;

    [Header("Hit Effects")]
    public GameObject[] hitEffectPrefabs;
    public GameObject lastBrickEffect;
    public GameObject groundedEffect;
    public GameObject crackEffectPrefab;
    public GameObject normalHitEffect;

    private bool hasWallSetUp = false;
    private bool hasScoredHit = false;
    private bool hasScoredGrounded = false;
    private bool hasBeenHitRecently = false;

    private float movementThreshold = 1f;

    private Rigidbody rb;
    private Renderer brickRenderer;
    private Material brickMat;

    private Vector3 initialPosition;
    private Vector3 currentImpactPoint;

    public static Action<Brick3D> OnAnyBrickDetached;
    public static Action<int> OnScoredPoints;

    private int normalHitPoints = 10;
    private int groundedHitPoints = 50;

    private void OnEnable()
    {
        WallGenerator3D.OnWallSetUp += HandleWallSetUp;
    }

    private void OnDisable()
    {
        WallGenerator3D.OnWallSetUp -= HandleWallSetUp;
    }

    private void HandleWallSetUp()
    {
        initialPosition = transform.position;
        hasWallSetUp = true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        brickRenderer = GetComponent<Renderer>();
        brickMat = brickRenderer.material;
    }

    private void Start()
    {
        initialPosition = transform.position; // Store initial position
    }

    public void OnClicked(Vector3 impactPoint)
    {
        if(!hasBeenHitRecently)
            CreateImpact(impactPoint);
    }

    private void FixedUpdate()
    {
        if (!isDetached && hasWallSetUp)
        {
            // Check for movement from initial position
            float distanceMoved = Vector3.Distance(transform.position, initialPosition);
            if (distanceMoved > movementThreshold)
            {
                DetachBrick(transform.position);
            }
            
            // Check if there's no support below
            if(rb.isKinematic)
                CheckForSupportBelow();
        }
    }

    private void CheckForSupportBelow()
    {
        // Skip if already detached
        if (isDetached) return;
        
        // Ray starts slightly below the current brick's center
        Vector3 rayStart = transform.position - new Vector3(0, 0.1f, 0);
        
        // Cast a short ray downward to check for support
        bool hasSupportBelow = Physics.Raycast(rayStart, Vector3.down, 0.5f);
        
        // If no support below, make non-kinematic
        if (!hasSupportBelow)
        {
            rb.isKinematic = false;
        }
    }

    private void CreateImpact(Vector3 impactPoint)
    {
        hasBeenHitRecently = true;
        StartCoroutine(HitDelayRoutine());

        SoundManager.instance.BrickHit();
        currentImpactPoint = impactPoint;

        if (crackEffectPrefab)
        {
            Instantiate(crackEffectPrefab, transform.position, Quaternion.identity);
        }

        if(hitEffectPrefabs.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, hitEffectPrefabs.Length);
            Instantiate(hitEffectPrefabs[randomIndex], impactPoint, Quaternion.identity);
        }

        // First detach the directly hit brick with more force
        DetachBrick(impactPoint, true);

        // Then handle the surrounding bricks with normal force
        Collider[] affectedBricks = Physics.OverlapSphere(impactPoint, impactRadius);
        foreach (Collider brick in affectedBricks)
        {
            Brick3D brickScript = brick.GetComponent<Brick3D>();
            if (brickScript != null && brickScript != this) // Skip the directly hit brick
            {
                brickScript.DetachBrick(impactPoint, false);
            }
        }
    }

    private void DetachBrick(Vector3 impactPoint, bool isDirectHit = false)
    {
        rb.isKinematic = false;
        bool wasDetached = isDetached;
        isDetached = true;

        // Check for bricks above this one and detach them
        CheckAndDetachBricksAbove();

        Vector3 direction = (transform.position - impactPoint).normalized;
        float distance = Vector3.Distance(transform.position, impactPoint);
        float impactMultiplier = 1f - (distance / impactRadius);

        // Apply more force for direct hits
        float finalImpactForce = isDirectHit ? impactForce * 3f : impactForce;
        float finalFallForce = isDirectHit ? fallForce * 2f : fallForce;

        Vector3 totalForce = direction * finalImpactForce * impactMultiplier;
        totalForce += direction * finalFallForce;

        rb.AddForce(totalForce, ForceMode.Impulse);

        // More torque for direct hits
        float finalTorqueStrength = isDirectHit ?
            UnityEngine.Random.Range(200f, 400f) :
            UnityEngine.Random.Range(100f, 300f);

        Vector3 randomTorqueAxis = UnityEngine.Random.onUnitSphere;
        rb.AddTorque(randomTorqueAxis * finalTorqueStrength, ForceMode.Impulse);

        SpawnCracks(isDirectHit);

        if (!hasScoredHit)
        {
            hasScoredHit = true;
            OnScoredPoints?.Invoke(normalHitPoints);
            Instantiate(normalHitEffect, GetNormalHitEffectPosition(), Quaternion.identity);
        }

        if (!wasDetached)
        {
            OnAnyBrickDetached?.Invoke(this);
        }
    }

    private Vector3 GetNormalHitEffectPosition()
    {
        float x = transform.position.x;
        float z = transform.position.z - 0.25f;
        float y = transform.position.y;
        return new Vector3(x, y, z);
    }
    private void CheckAndDetachBricksAbove()
    {
        Vector3 rayStart = transform.position + new Vector3(0, 0.1f, 0);

        if (Physics.Raycast(rayStart, Vector3.up, out RaycastHit hit, 1.0f))
        {
            // Skip self
            if (hit.collider.gameObject == gameObject)
                return;
                
            Brick3D brickAbove = hit.collider.GetComponent<Brick3D>();
            if (brickAbove != null && !brickAbove.isDetached)
            {
                // Simply set isKinematic to false for the brick above
                if (brickAbove.TryGetComponent<Rigidbody>(out var brickRb))
                {
                    brickRb.isKinematic = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDetached)
        {
            float collisionForce = collision.relativeVelocity.magnitude;
            rb.linearVelocity *= 0.8f;
            rb.AddForce(0.2f * collisionForce * UnityEngine.Random.insideUnitSphere, ForceMode.Impulse);

            if (collision.gameObject.CompareTag("Ground"))
            {
                ApplyTransparentMat();

                if (!hasScoredGrounded)
                {
                    hasScoredGrounded = true;
                    Instantiate(groundedEffect, transform.position, Quaternion.identity);
                    OnScoredPoints?.Invoke(groundedHitPoints);
                }

                if (!HasBricksAbove())
                    StartCoroutine(DestroyAfterDelay(UnityEngine.Random.Range(1, 3)));
                else
                    InvokeRepeating(nameof(CheckIfCanBeDestroyed), 0f, 3f);
            }
        }
    }

    private bool HasBricksAbove()
    {
        Vector3 rayStart = transform.position + new Vector3(0, 1f, 0);
        return Physics.Raycast(rayStart, Vector3.up, 1.5f);
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void CheckIfCanBeDestroyed()
    {
        if (!HasBricksAbove())
            StartCoroutine(DestroyAfterDelay(UnityEngine.Random.Range(2, 5)));
    }

    public void SetColor(Color color)
    {
        if(brickRenderer != null)
            brickMat.SetColor("_baseColor", color);
        else
            Debug.LogError("Brick Renderer is not assigned.");
    }
    
    private void ApplyTransparentMat()
    {
        Material[] materials = new Material[1];
        materials[0] = detachedMat;
        brickRenderer.materials = materials;
    }

    private void SpawnCracks(bool isDirectHit)
    {
        if (!isDirectHit)
        {
            int randTex = UnityEngine.Random.Range(0, impactedTex.Length);
            brickMat.SetTexture("_crack", impactedTex[randTex]);
        }
        else
        {
            brickMat.SetTexture("_crack", directHitTex);
        }

        brickMat.SetFloat("_crackAlpha", 1f);

        float vectorX = UnityEngine.Random.Range(0f, 1f);
        float vectorY = UnityEngine.Random.Range(0f, 1f);
        brickMat.SetVector("_offset", new(vectorX, vectorY));
    }

    public void SpawnWallEndEffect()
    {
        if (lastBrickEffect)
        {
            Instantiate(lastBrickEffect, transform.position, Quaternion.identity);
        }
    }

    IEnumerator HitDelayRoutine()
    {
        yield return new WaitForSeconds(hitDelay);
        hasBeenHitRecently = false;
    }

}
