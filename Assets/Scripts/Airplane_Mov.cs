using UnityEngine;

public class Airplane_Mov : MonoBehaviour
{
    public float speed = 4f;
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            if (transform.position.x < -15f)
            {
                Destroy(gameObject);
            }
    }
}
