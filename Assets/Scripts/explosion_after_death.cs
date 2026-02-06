using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion_after_time : MonoBehaviour
{


    public float lifetime = 0.7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
