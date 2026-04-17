using UnityEngine;

public class Cloud_Mov : MonoBehaviour
{
    public float speed = 3f; // Clouds usually move slower than missiles

    void Update()
    {
        // Move to the left 
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        // Destroy the cloud if it passes the left edge of the screen
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}