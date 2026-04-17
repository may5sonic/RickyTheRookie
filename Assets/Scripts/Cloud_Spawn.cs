using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Spawn : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float spawnRate = 4f; // Spawns a cloud every 5 seconds
    public float minY = -4f;
    public float maxY = 4f;

    void Start()
    {
        // Start the repeating timer
        InvokeRepeating(nameof(SpawnCloud), 2f, spawnRate);
    }

    void SpawnCloud() 
    {
        // 1. Check if game is active
        if (GameManager.instance.isGameActive == false)
        {
            CancelInvoke(nameof(SpawnCloud)); 
            return;
        }

        // Pick a random height
        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);

        Instantiate(cloudPrefab, spawnPos, Quaternion.identity);
    }
}