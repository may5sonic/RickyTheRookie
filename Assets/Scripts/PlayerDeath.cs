using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Header("Death Settings")]
    public GameObject deathEffect;

    private bool isDead = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("missile"))
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Tell GameManager the player died
        GameManager.instance.PlayerDied();

        // Spawn explosion
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}