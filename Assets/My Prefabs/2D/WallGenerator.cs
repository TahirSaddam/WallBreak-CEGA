using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create new script: WallGenerator.cs
public class WallGenerator : MonoBehaviour
{
    public GameObject brickPrefab;
    public int rows = 5;
    public int columns = 8;
    public float spacing = 0.1f;

    void Start()
    {
        GenerateWall();
    }

    void GenerateWall()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 pos = new Vector3(
                    x * (brickPrefab.transform.localScale.x + spacing),
                    y * (brickPrefab.transform.localScale.y + spacing),
                    0
                );
                Instantiate(brickPrefab, pos, Quaternion.identity);
            }
        }
    }
}
