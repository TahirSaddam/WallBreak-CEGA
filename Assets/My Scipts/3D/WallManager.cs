using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class WallManager : MonoBehaviour
{
    public GameObject wallPrefab;
    public int totalWalls = 20;
    public float wallOffsetZ = 1.65f;
    public float wallSpacing = 20f;
    public float cameraSpeed = 5f;
    public float minMass;
    public float maxMass;

    [Header("Camera Settings")]
    public float cameraMoveTime = 1f;
    public Ease cameraEaseType = Ease.InOutQuad;

    [Header("Wall Pattern")]
    [SerializeField] private WallPattern wallPattern;
    [SerializeField] private bool useVerticalBricks = false;
    [SerializeField] private bool useAdjacentVerticalBricks = false;

    [Range(0f, 1f)]
    [SerializeField] private float verticalBrickChance = 0.1f;

    [Header("Wall Attributes")]
    [SerializeField] private int rows = 1;
    [SerializeField] private int columns = 1;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private bool alternateRows = false;

    private int currentWallIndex = 0;
    private int remainingBricks;

    private float initialCameraZ;
    private float nextCameraZ;

    private bool canInteract = true;
    private bool isMovingCamera = false;

    private List<WallGenerator3D> walls = new();

    private void OnEnable()
    {
        Brick3D.OnAnyBrickDetached += HandleBrickDetached;
    }
    private void OnDisable()
    {
        Brick3D.OnAnyBrickDetached -= HandleBrickDetached;
    }
    void Start()
    {
        initialCameraZ = Camera.main.transform.position.z;
        nextCameraZ = initialCameraZ;
    }

    public void SpawnNewWall()
    {
        SpawnNextSection();
    }

    void SpawnNextSection()
    {
        float zPosition = (currentWallIndex * wallSpacing) + wallOffsetZ;
        Vector3 wallPosition = new(0, 0, zPosition);
        GameObject wall = Instantiate(wallPrefab, wallPosition, Quaternion.identity);

        if (wall.TryGetComponent<WallGenerator3D>(out var wallGen))
        {
            // Get brick mass based on the current wall index
            float brickMass = GetBrickMassByWallIndex(currentWallIndex);
            wallGen.SetBrickMass(brickMass);  // Pass the mass value
            wallGen.SetWallPattern(useVerticalBricks, useAdjacentVerticalBricks, verticalBrickChance); // Set the vertical and adjacent mode
            wallGen.SetWallAttributes(rows, columns, spacing, alternateRows); // Set the wall attributes
            wallGen.Initialize(currentWallIndex, wallPattern);
            remainingBricks = wallGen.TotalBricksCreated(); 

        }

        walls.Add(wallGen);
        nextCameraZ = initialCameraZ + (currentWallIndex * wallSpacing);
    }
    void HandleBrickDetached(Brick3D brick)
    {
        remainingBricks--;
        if (remainingBricks <= 0)
        {
            brick.SpawnWallEndEffect();
            canInteract = false;
            StartCoroutine(MoveToNextWall());
        }
    }
    float GetBrickMassByWallIndex(int index)
    {

        float startMass = minMass;
        float endMass = maxMass;

        float incrementPerLevel = (endMass - startMass) / (totalWalls - 1);
        return startMass + index * incrementPerLevel;
    }

    void Update()
    {
        if (canInteract && IsCurrentWallDestroyed())
        {
            canInteract = false;
            StartCoroutine(MoveToNextWall());
        }
    }

    bool IsCurrentWallDestroyed()
    {
        if (walls.Count <= currentWallIndex) return false;
        return remainingBricks <= 0;

        //Brick3D[] bricks = walls[currentWallIndex].GetComponentsInChildren<Brick3D>();
        //foreach (Brick3D brick in bricks)
        //{
        //    if (!brick.IsDetached()) return false;
        //}
        //return true;
    }

    IEnumerator MoveToNextWall()
    {
        yield return new WaitForSeconds(4f);

        currentWallIndex++;
        if (currentWallIndex < totalWalls)
        {
            SpawnNextSection();
            MoveCameraToNextPosition();
        }
        else
        {
            Debug.Log("Game Complete!");
        }
    }

    void MoveCameraToNextPosition()
    {
        isMovingCamera = true;
        Camera.main.transform.DOMoveZ(nextCameraZ, cameraMoveTime)
            .SetEase(cameraEaseType)
            .OnComplete(() =>
            {
                isMovingCamera = false;
                canInteract = true;

                if (currentWallIndex > 0)
                {
                    Destroy(walls[currentWallIndex - 1].gameObject);
                    walls[currentWallIndex - 1] = null;
                }
            });
    }

    public void GameOver()
    {
        canInteract = false;
        currentWallIndex = 0;
        nextCameraZ = initialCameraZ;
        float cameraX = Camera.main.transform.position.x;
        float cameraY = Camera.main.transform.position.y;
        Camera.main.transform.position = new(cameraX, cameraY, nextCameraZ);
        walls[^1].DestroyThisWall();
        walls.Clear();
    }
}
