using UnityEngine;

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
        float currentSpawnRate = baseSpawnRate - (GameManager.currentRound * speedIncreasePerRound);
        currentSpawnRate = Mathf.Max(currentSpawnRate, minSpawnRate);
        InvokeRepeating(nameof(SpawnCoin), 1f, currentSpawnRate);
    }

    void SpawnCoin()
    {
        if (GameManager.instance.isGameActive == false) return;

        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }
}
