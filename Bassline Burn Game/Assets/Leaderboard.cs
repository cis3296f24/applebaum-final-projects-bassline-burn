using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public Transform leaderboardContent; 
    public GameObject leaderboardRowPrefab; 

    public void PopulateLeaderboard()
    {
        // Clear the leaderboard
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        // Sort results and populate rows
        RaceResults.SortResults();
        foreach (var result in RaceResults.results)
        {
            var row = Instantiate(leaderboardRowPrefab, leaderboardContent);
            var texts = row.GetComponentsInChildren<Text>();

            texts[0].text = result.position.ToString();      
            texts[1].text = result.playerName;              
            texts[2].text = $"{result.result:F2}s";        
        }
    }
}
