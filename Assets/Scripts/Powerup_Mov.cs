using UnityEngine;

public class PowerUp_Mov : MonoBehaviour
{
    public float speed = 4f;
    void Update()
    {
        float difficulty = GameManager.instance != null ? GameManager.instance.DifficultyMultiplier : 1f;
        transform.Translate(Vector3.left * (speed * difficulty) * Time.deltaTime, Space.World);
            if (transform.position.x < -15f)
            {
                Destroy(gameObject);
            }
    }
}
