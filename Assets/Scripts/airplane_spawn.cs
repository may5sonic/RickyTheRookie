using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane_Spawn : MonoBehaviour
{
    public GameObject airplanePrefab;
    //public float spawnRate = 4f; // Spawns a cloud every 5 seconds
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Difficulty Settings")]
    public float baseSpawnRate = 4f;
    public float minSpawnRate = 1.5f;
    public float speedIncreasePerRound = 0.5f;

    void Start()
    {
        // Start the repeating timer
        //InvokeRepeating(nameof(SpawnCloud), 2f, spawnRate);
        // float currentSpawnRate = baseSpawnRate - (GameManager.currentRound * speedIncreasePerRound);
        // currentSpawnRate = Mathf.Max(currentSpawnRate, minSpawnRate);
        // Debug.Log("Round " + GameManager.currentRound + " | Airplane Spawn Rate: " + currentSpawnRate);
        // InvokeRepeating(nameof(SpawnAirplane), 1f, currentSpawnRate);

         StartCoroutine(SpawnLoop()); // used to wait spawns dynamically
    }

    IEnumerator SpawnLoop() {
        while (true) {
            if (GameManager.instance.isGameActive) {
                SpawnAirplane();
            }

            float currentSpawnRate = baseSpawnRate - (GameManager.currentRound * speedIncreasePerRound); // calculates current spawn
            currentSpawnRate = Mathf.Max(currentSpawnRate, minSpawnRate); // clamp the spawn rate 
            yield return new WaitForSeconds(currentSpawnRate); // controls enemy plane spawn pauses courotine
        }
    }

    void SpawnAirplane() 
    {
        // 1. Check if game is active
        if (GameManager.instance.isGameActive == false)
        {
            //CancelInvoke(nameof(SpawnAirplane)); 
            return;
        }

        // Pick a random height
        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);

        Instantiate(airplanePrefab, spawnPos, Quaternion.identity);
    }
}