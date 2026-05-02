using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile_spawn : MonoBehaviour
{

[Header("Missile Types")]
    public GameObject regularMissilePrefab; 
    public GameObject homingMissilePrefab;
    //public float spawnRate = 2f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Difficulty")]
    [Range(0, 100)]
    public int homingChance = 20; // 20% chance to spawn a homing missile

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
        //InvokeRepeating(nameof(SpawnMissile), 1f, spawnRate);
        float currentSpawnRate = baseSpawnRate - (GameManager.currentRound * speedIncreasePerRound);
        currentSpawnRate = Mathf.Max(currentSpawnRate, minSpawnRate);
        Debug.Log("Round " + GameManager.currentRound + " | Missile Spawn Rate: " + currentSpawnRate);
        InvokeRepeating(nameof(SpawnMissile), 1f, currentSpawnRate);
    }


    void SpawnMissile() {

        if (GameManager.instance.isGameActive == false)
        {
            return;
        }

        //CancelInvoke(nameof(SpawnMissile));
        GameObject missileToSpawn;
        int roll = Random.Range(0, 100); 

        if (roll < homingChance)
        {
            missileToSpawn = homingMissilePrefab; // We rolled low! Spawn the deadly one!
        }
        else
        {
            missileToSpawn = regularMissilePrefab; // Normal spawn
        }

        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);
        float randomZ = Random.Range(-maxRotationAngle, maxRotationAngle);
        Quaternion spawnRotation = Quaternion.Euler(0, 0, randomZ);
        Instantiate(missileToSpawn, spawnPos, spawnRotation);
    }
}
