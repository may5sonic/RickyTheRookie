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

        // Mathf.Repeat creates a perfect, unbroken loop from 0 to 'width'
        float loopPosition = Mathf.Repeat(Time.time * speed, width);

        // Apply that loop to the start position. No drifting allowed!
        transform.position = startPosition + Vector3.left * loopPosition;
        //transform.position += Vector3.left * speed * Time.deltaTime;

        // when completely off screen move to the right
       // if (transform.position.x < -width) {
        //    transform.position += new Vector3(width * 2f, 0, 0);
        //}

        // Mathf.Repeat creates a continuous loop from 0 to 'width'
        // Time.time ensures the movement is perfectly smooth
        // float newPosition = Mathf.Repeat(Time.time * speed, width);

        // // Move the background left based on that looping number
        // transform.position = startPosition + Vector3.left * newPosition;
    }
}