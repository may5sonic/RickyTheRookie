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
        float difficulty = GameManager.instance != null ? GameManager.instance.DifficultyMultiplier : 1f;
        float currentSpawnRate = Mathf.Max(baseSpawnRate / Mathf.Max(0.01f, difficulty), minSpawnRate);
        Debug.Log("Round " + GameManager.currentRound + " | Powerup Spawn Rate: " + currentSpawnRate);
        InvokeRepeating(nameof(SpawnPowerup), 1f, currentSpawnRate);
    }

    void SpawnPowerup() 
    {
        // 1. Check if game is active
        if (GameManager.instance == null || !GameManager.instance.spawningEnabled || !GameManager.instance.IsPlaying)
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
