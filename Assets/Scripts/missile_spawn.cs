using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile_spawn : MonoBehaviour
{

    public GameObject missilePrefab;
    //public float spawnRate = 2f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Missile Trajectory")]
    // Set this to 10 in the Inspector to get your 0-10 degree variance
    public float maxRotationAngle = 10f;

    [Header("Difficulty Settings")]
    public float baseSpawnRate = 2f;
    public float minSpawnRate = 0.4f;
    public float speedIncreasePerRound = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float difficulty = GameManager.instance != null ? GameManager.instance.DifficultyMultiplier : 1f;
        float currentSpawnRate = Mathf.Max(baseSpawnRate / Mathf.Max(0.01f, difficulty), minSpawnRate);
        Debug.Log("Round " + GameManager.currentRound + " | Missile Spawn Rate: " + currentSpawnRate);
        InvokeRepeating(nameof(SpawnMissile), 1f, currentSpawnRate);
    }


    void SpawnMissile() {

        if (GameManager.instance == null || !GameManager.instance.spawningEnabled || !GameManager.instance.IsPlaying)
        {
            CancelInvoke(nameof(SpawnMissile));
            return;
        }

        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);

        float randomZ = Random.Range(-maxRotationAngle, maxRotationAngle);

        Quaternion spawnRotation = Quaternion.Euler(0, 0, randomZ);

        Instantiate(missilePrefab, spawnPos, spawnRotation);
    }
}
