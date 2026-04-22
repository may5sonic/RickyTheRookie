using UnityEngine;

public class LevelContentConfig : MonoBehaviour
{
    public bool enableMissiles = true;
    public bool enableClouds = false;
    public bool enablePowerups = false;
    public bool enableEnemyJets = false;

    [Header("Spawner Roots (optional)")]
    public GameObject missileSpawnerRoot;
    public GameObject cloudSpawnerRoot;
    public GameObject powerupSpawnerRoot;
    public GameObject enemyJetSpawnerRoot;

    public void Apply()
    {
        if (missileSpawnerRoot != null) missileSpawnerRoot.SetActive(enableMissiles);
        if (cloudSpawnerRoot != null) cloudSpawnerRoot.SetActive(enableClouds);
        if (powerupSpawnerRoot != null) powerupSpawnerRoot.SetActive(enablePowerups);
        if (enemyJetSpawnerRoot != null) enemyJetSpawnerRoot.SetActive(enableEnemyJets);
    }
}
