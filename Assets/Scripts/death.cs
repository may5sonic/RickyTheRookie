using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour

{
    public GameObject rocket_fired;

    void OnTriggerEnter2D(Collider2D collide) {
        // if hit by missile die 
        // || collide.CompareTag("storm")
        
        if (collide.CompareTag("missile")) {
            rocket_fired = collide.gameObject;
            Die();
        }
    }

    void Die() {
        // destorys the jet
        Destroy(gameObject);
        if (rocket_fired != null) {
            Destroy(rocket_fired);
        }
    }

}

