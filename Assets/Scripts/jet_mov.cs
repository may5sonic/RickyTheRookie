using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jet_mov : MonoBehaviour
{



    //     void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Boundary"))
    //     {
    //         // Bounce slightly inward
    //         Vector2 bounceDir = collision.contacts[0].normal;
    //         GetComponent<Rigidbody2D>().AddForce(-bounceDir * 5f, ForceMode2D.Impulse);
    //     }
    // }   

    [Header("Movement Settings")]
    public float speed = 5f;
    public float rotationSpeed = 720f; // How fast it turns (degrees per second)

    [Header("Shooting Settings")]
    public GameObject bulletPrefab; // Drag your Bullet Prefab here
    public Transform firePoint;     // Drag your FirePoint object here
    
    [Header("Effects")]
    public GameObject deathEffect; // Drag your explosion prefab here

    void Update()
    {


        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3 (moveX, moveY, 0f);
        Vector3 movement = new Vector3(moveX, moveY, 0f).normalized;
        //transform.Translate (movement * speed * Time.deltaTime);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        //     body.linearVelocity = new Vector2(horizontalInput, body.linearVelocity.y);

        //     if(Input.GetKey(KeyCode.Space))
        //         body.linearVelocity = new Vector2(body.linearVelocity.x, speed);
        // }

        if (movement != Vector3.zero)
        {
            // Option A: Instant Snap (Good for very twitchy games)
            // transform.up = movement; 

            // Option B: Smooth Turn (Feels more like a vehicle)
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Spawn the bullet at the FirePoint's position and rotation
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    // This catches "Trigger" hits (passing through objects)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check for the tag "missile" (lowercase, just like you have it)
        if (other.CompareTag("missile") || other.name.Contains("missile"))
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.instance.GameOver();

        // --- NEW CODE ---
        // Spawn the explosion at the PLAYER'S position
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        // ----------------

        Destroy(gameObject);
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8.5f, 8.5f);  // horizontal screen limits
        pos.y = Mathf.Clamp(pos.y, -4.5f, 4.5f);  // vertical screen limits
        transform.position = pos;
    }

}