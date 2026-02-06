using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour

{

    public GameObject deathEffectPrefab;

    void OnTriggerEnter2D(Collider2D collide) {
        // if hit by missile die 
        // || collide.CompareTag("storm")
        
        if (collide.CompareTag("missile")) { 
            Die();
        }
    }

    void Die() {

        // spawns explosion death effect
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        // destorys the jet
        Destroy(gameObject);
    }

}

