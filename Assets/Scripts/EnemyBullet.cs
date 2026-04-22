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
        float difficulty = GameManager.instance != null ? GameManager.instance.DifficultyMultiplier : 1f;

        // Flies to the LEFT
        transform.Translate(Vector3.left * (speed * difficulty) * Time.deltaTime, Space.World);
    }
}
