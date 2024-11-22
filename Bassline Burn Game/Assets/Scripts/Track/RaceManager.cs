using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    private Dictionary<GameObject, int> playerProgress = new Dictionary<GameObject, int>();

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerHitCheckpoint(GameObject player, int checkpointNumber)
    {
        if (!playerProgress.ContainsKey(player))
        {
            playerProgress[player] = 0; // Initialize progress
        }

        // Ensure players hit checkpoints in order
        if (playerProgress[player] + 1 == checkpointNumber)
        {
            playerProgress[player] = checkpointNumber;
            Debug.Log($"Player {player.name} reached checkpoint {checkpointNumber}");

            // Handle lap completion
            if (IsFinalCheckpoint(checkpointNumber))
            {
                Debug.Log($"Player {player.name} completed a lap!");
            }
        }
        else
        {
            Debug.Log($"Player {player.name} hit an invalid checkpoint!");
        }
    }

    private bool IsFinalCheckpoint(int checkpointNumber)
    {
        // Replace with your logic to determine if this is the final checkpoint
        return checkpointNumber == 5; // Example: 5 is the last checkpoint
    }
}
