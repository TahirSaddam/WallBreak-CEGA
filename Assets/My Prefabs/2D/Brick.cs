using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brick : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isDetached = false;
    private SpriteRenderer spriteRenderer;

    [Header("Physics Settings")]
    public float brickMass = 8f;              // Heavier mass for realistic feel
    public float detachRadius = 1.2f;         // Smaller radius for controlled breaking
    public float impactForce = 400f;          // Strong initial force
    public float gravityScale = 2.5f;         // Stronger gravity
    public float rotationResistance = 0.8f;   // Resist rotation for brick-like behavior

    [Header("Connection Settings")]
    public float connectionStrength = 15f;     // Stronger connections
    public List<Brick> connectedBricks = new List<Brick>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.mass = brickMass;
        rb.linearDamping = 0.5f;                       // Add some air resistance
        rb.angularDamping = 0.8f;                // Resist spinning
        FindConnectedBricks();
    }

    void FindConnectedBricks()
    {
        float brickSize = spriteRenderer.bounds.size.x;
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, brickSize * 1.1f);

        foreach (Collider2D col in nearbyColliders)
        {
            Brick otherBrick = col.GetComponent<Brick>();
            if (otherBrick != null && otherBrick != this)
            {
                connectedBricks.Add(otherBrick);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                DetachNearbyBricks(mousePos);
            }
        }
    }

    void DetachNearbyBricks(Vector2 impactPoint)
    {
        Collider2D[] affectedColliders = Physics2D.OverlapCircleAll(impactPoint, detachRadius);

        foreach (Collider2D col in affectedColliders)
        {
            Brick brick = col.GetComponent<Brick>();
            if (brick != null)
            {
                brick.DetachFromWall(impactPoint);
            }
        }
    }

    public void DetachFromWall(Vector2 impactPoint)
    {
        if (isDetached) return;
        isDetached = true;

        // Make brick dynamic with realistic properties
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScale;

        // Calculate impact force with more realistic physics
        Vector2 direction = ((Vector2)transform.position - impactPoint).normalized;
        float distance = Vector2.Distance(impactPoint, transform.position);
        float forceMagnitude = impactForce * (1f - Mathf.Clamp01(distance / detachRadius));

        // Apply forces with more downward tendency
        Vector2 finalForce = direction * forceMagnitude;
        finalForce.y *= 0.5f; // Reduce upward force
        rb.AddForce(finalForce, ForceMode2D.Impulse);

        // Add minimal rotation
        float torque = Random.Range(-50f, 50f) * rotationResistance;
        rb.AddTorque(torque);

        StartCoroutine(DetachConnectedBricksWithDelay(impactPoint));
    }

    IEnumerator DetachConnectedBricksWithDelay(Vector2 impactPoint)
    {
        foreach (Brick brick in connectedBricks)
        {
            if (brick != null && !brick.isDetached)
            {
                yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

                float distance = Vector2.Distance(impactPoint, brick.transform.position);
                if (Random.value < (1f - connectionStrength / 20f))
                {
                    brick.DetachFromWall(impactPoint);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDetached && collision.relativeVelocity.magnitude > 1f)
        {
            // Reduced bounce and rotation on collision
            float collisionForce = collision.relativeVelocity.magnitude * 0.3f;
            rb.AddForce(-collision.relativeVelocity * 0.2f, ForceMode2D.Impulse);
            rb.AddTorque(collisionForce * Random.Range(-5f, 5f));
        }
    }
}