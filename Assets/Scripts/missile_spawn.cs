using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile_spawn : MonoBehaviour
{

    public GameObject missilePrefab;
    public float spawnRate = 2f;
    public float minY = -4f;
    public float maxY = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(SpawnMissile), 1f, spawnRate);
    }


    void SpawnMissile() {
        float spawnY = Random.Range(minY, maxY);

        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);
        Instantiate(missilePrefab, spawnPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
