using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f; // Fires every 2 seconds

    void Start()
    {
        InvokeRepeating(nameof(FireLaser), fireRate, fireRate);
    }

    void FireLaser()
    {
        // If the enemy is off-screen to the right, don't shoot yet
        if (transform.position.x > 10f) return;

        Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
    }
}