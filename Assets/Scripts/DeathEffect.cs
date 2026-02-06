using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    void Start()
    {
        // Destroy this effect object after 0.5 seconds so it doesn't clutter the game
        Destroy(gameObject, 0.5f);
    }
}