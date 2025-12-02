using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPiece : MonoBehaviour
{
    public float destroyDelay = 2f; // Destroy after 2 seconds

    void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}
