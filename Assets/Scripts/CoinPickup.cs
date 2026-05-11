using UnityEngine;
using System.Collections.Generic;

public class CoinPickup : MonoBehaviour
{
    public int points = 100;
    static readonly HashSet<int> pickedUpCoinIds = new HashSet<int>();

    void OnTriggerEnter2D(Collider2D other)
    {
        int id = gameObject.GetInstanceID();
        if (pickedUpCoinIds.Contains(id)) return;
        if (other.GetComponent<jet_mov>() == null) return;

        pickedUpCoinIds.Add(id);
        GameManager.instance.AddScore(points);
        Destroy(gameObject);
    }
}
