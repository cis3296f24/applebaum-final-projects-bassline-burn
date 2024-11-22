
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public bool debug = false; // If true, bypass normal checks for testing purposes.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player {other.name} reached the finish line!");
            // Add race completion logic here
        }
    }
}
