using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [Header("Missile Stats")]
    public float speed = 5f;
    
    [Tooltip("How fast it turns. 200 is a very aggressive turn rate!")]
    public float rotationSpeed = 200f; 

    private Transform player;

    void Start()
    {
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }

        if (player != null)
        {
            // 1. Find where the player is
            Vector2 direction = player.position - transform.position;
            
            // 2. MATH FIX: Add 180 degrees because our missile sprite's nose points Left!
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;

            // 3. Turn the steering wheel
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 4. Hit the gas pedal (Space.Self ensures it flies wherever the nose is pointing)
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.Self);
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("RADAR LOCK: I found the player!");
        }
        else
        {
            Debug.LogWarning("RADAR BROKEN: I can't find anything tagged 'Player'!");
        }
    }
}
