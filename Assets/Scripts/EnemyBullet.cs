using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Cleans up memory
    }

    void Update()
    {
        // Flies to the LEFT
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }
}