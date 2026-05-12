using UnityEngine;
using System.Collections;

public class Coin_Spawn : MonoBehaviour
{
    public GameObject coinPrefab;
    public float minY = -4f;
    public float maxY = 4f;

    public float baseSpawnRate = 3f;
    public float minSpawnRate = 1.2f;
    public float speedIncreasePerRound = 0.3f;

    void Start()
    {
        // float currentSpawnRate = baseSpawnRate - (GameManager.currentRound * speedIncreasePerRound);
        // currentSpawnRate = Mathf.Max(currentSpawnRate, minSpawnRate);
        // InvokeRepeating(nameof(SpawnCoin), 1f, currentSpawnRate);

        StartCoroutine(SpawnLoop()); // used to wait spawns dynamically
    }

    IEnumerator SpawnLoop() {
        while (true) {
            if (GameManager.instance!= null && GameManager.instance.isGameActive) {
                SpawnCoin();
            }

            float currentSpawnRate = baseSpawnRate - (GameManager.currentRound * speedIncreasePerRound); // calculates current spawn
            currentSpawnRate = Mathf.Max(currentSpawnRate, minSpawnRate); // clamp the spawn rate 
            Debug.Log("Coin Spawn Rate: " + currentSpawnRate); 
            yield return new WaitForSeconds(currentSpawnRate); // controls coin spawn pauses courotine
        }
    }

    void SpawnCoin()
    {
        if (GameManager.instance.isGameActive == false) return;

        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }
}
