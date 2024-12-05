using System.Collections.Generic;
using UnityEngine;

public class PopulateLeaderboard : MonoBehaviour
{
    [Header("Leaderboard Settings")]
    public Transform leaderboardContent;    
    public GameObject leaderboardRowPrefab; 

    public void Populate(List<PlayerResult> playerResults)
    {
        // Clear existing rows
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        // Add a row for each player's result
        foreach (var player in playerResults)
        {
            var row = Instantiate(leaderboardRowPrefab, leaderboardContent);
            var resultItem = row.GetComponent<PlayerResultItem>();
            resultItem.SetResult(player.playerName, player.result, player.position);
        }
    }
}
