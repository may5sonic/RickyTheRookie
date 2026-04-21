using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Spawn : MonoBehaviour
{
    public GameObject PowerupPrefab;
    //public float spawnRate = 4f; // Spawns a Powerup every 5 seconds
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Difficulty Settings")]
    public float baseSpawnRate = 4f;
    public float minSpawnRate = 1.5f;
    public float speedIncreasePerRound = 0.5f;

    void Start()
    {
        // Start the repeating timer
        //InvokeRepeating(nameof(SpawnPowerup), 2f, spawnRate);
        float currentSpawnRate = baseSpawnRate - (GameManager.currentRound * speedIncreasePerRound);
        currentSpawnRate = Mathf.Max(currentSpawnRate, minSpawnRate);
        Debug.Log("Round " + GameManager.currentRound + " | Powerup Spawn Rate: " + currentSpawnRate);
        InvokeRepeating(nameof(SpawnPowerup), 1f, currentSpawnRate);
    }

    void SpawnPowerup() 
    {
        // 1. Check if game is active
        if (GameManager.instance.isGameActive == false)
        {
            CancelInvoke(nameof(SpawnPowerup)); 
            return;
        }

        // Pick a random height
        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);

        Instantiate(PowerupPrefab, spawnPos, Quaternion.identity);
    }
}