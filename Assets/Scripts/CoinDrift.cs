using UnityEngine;

public class CoinDrift : MonoBehaviour
{
    public float leftSpeed = 3f;
    public float driftSpeed = 1.5f;
    public float driftAmount = 0.6f;

    float seed;

    void Awake()
    {
        seed = Random.value * 1000f;
    }

    void Update()
    {
        transform.Translate(Vector3.left * leftSpeed * Time.deltaTime, Space.World);

        float yOffset = Mathf.Sin((Time.time + seed) * driftSpeed) * driftAmount;
        transform.position = new Vector3(transform.position.x, transform.position.y + yOffset * Time.deltaTime, transform.position.z);

        if (transform.position.x < -15f) Destroy(gameObject);
    }
}
