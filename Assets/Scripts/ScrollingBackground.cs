using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    public float speed = 2f;
    
    private float width;
    private Vector3 startPosition;

    void Start()
    {
        // Remember where this specific image started
        startPosition = transform.position;
        
        // Automatically measure how wide the image is
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Mathf.Repeat creates a continuous loop from 0 to 'width'
        // Time.time ensures the movement is perfectly smooth
        float newPosition = Mathf.Repeat(Time.time * speed, width);

        // Move the background left based on that looping number
        transform.position = startPosition + Vector3.left * newPosition;
    }
}