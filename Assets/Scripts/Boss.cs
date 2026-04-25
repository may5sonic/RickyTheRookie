using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health = 20;
    public float speed = 1.5f;
    public GameObject deathEffect;

    [Header("Boss Attacks")]
    public GameObject airplanePrefab;
    public Transform topHangar;
    public Transform bottomHangar;

    [Header("Hover Movement")]
    public float hoverSpeed = 2f;      // How fast it bobs up and down
    public float hoverDistance = 4.5f; // How high and low it reaches

    private bool inPosition = false;

    void Start()
    {
        InvokeRepeating(nameof(DeployAirplanes), 3f, 4f);
    }

    void Update()
    {
        // Slowly enter the screen, then stop at X = 6
        if (inPosition == false)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            if (transform.position.x <= 6f)
            {
                inPosition = true;
            }
        }
        else
        {
            // Smooth wave
            float newY = Mathf.Sin(Time.time * hoverSpeed) * hoverDistance;

            // apply the Y position while keeping the X and Z the same
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    void DeployAirplanes()
    {
        // Don't spawn them until the boss is actually on screen
        if (transform.position.x > 8f) return;

        if (airplanePrefab != null)
        {
            Instantiate(airplanePrefab, topHangar.position, Quaternion.identity);
            Instantiate(airplanePrefab, bottomHangar.position, Quaternion.identity);
        }
    }

    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        GameManager.instance.BossDefeated();
        Destroy(gameObject);
    }
}