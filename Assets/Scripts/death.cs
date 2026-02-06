using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour

{

    void OnTriggerEnter2D(Collider2D collide) {
        // if hit by missile die 
        // || collide.CompareTag("storm")
        
        if (collide.CompareTag("missile")) { 
            Die();
        }
    }

    void Die() {
        // destorys the jet
        Destroy(gameObject);
    }

}

