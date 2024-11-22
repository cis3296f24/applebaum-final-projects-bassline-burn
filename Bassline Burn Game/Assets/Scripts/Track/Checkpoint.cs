using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkPointNumber = -1; // Unique number for each checkpoint
    public int index; // Add this property to store the index

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) // Assuming kart has "Player" tag
        {
            KartLapController lapController = collider.GetComponent<KartLapController>();
            if (lapController != null)
            {
                lapController.ProcessCheckpoint(this);
            }
        }
    }
}
