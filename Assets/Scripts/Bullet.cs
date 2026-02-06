using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f; // Bullet destroys itself after 2 seconds

    [Header("Effects")]
    public GameObject deathEffect; // Drag your explosion prefab here

    void Start()
    {
        // 1. Destroy the bullet automatically so memory doesn't fill up
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 2. Move the bullet "Up" (which is Forward for your 2D game)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    
    // 3. Detect hits (We will use this later for enemies)
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if the thing we hit is a Missile
        // We check the Tag OR if the name contains "missile"
        if (hitInfo.CompareTag("missile") || hitInfo.gameObject.name.Contains("missile") || hitInfo.gameObject.name.Contains("Missile(Clone)"))
        {
            // This part tells our GameManager the missile was destoryed
            GameManager.instance.MissileDestroyed();

            if (deathEffect != null)
            {
                Instantiate(deathEffect, hitInfo.transform.position, Quaternion.identity);
            }

            // Destroy the Enemy
            Destroy(hitInfo.gameObject);
            
            // Destroy the Bullet (so it doesn't keep flying)
            Destroy(gameObject);
            
            // Optional: This is where you would play an explosion sound or spawn an effect
            // Instantiate(explosionPrefab, transform.position, transform.rotation);
        }
        // Debug.Log("Hit " + hitInfo.name);
        // Destroy(gameObject); // Destroy bullet on impact
    }
}