using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{

    public float speed = 6f;

    // Update is called once per frame
    void Update()
    {
    
    // moves left across screen
     transform.Translate(Vector3.left * speed * Time.deltaTime);

    // destroy off screen
    if (transform.position.x < -15f) {
        Destroy(gameObject);
    }

    }
}
