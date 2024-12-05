using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;
    public Transform leaderboardContent;
    public GameObject leaderboardRowPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowResults()
    {
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var result in RaceResults.results)
        {
            var row = Instantiate(leaderboardRowPrefab, leaderboardContent);
            var texts = row.GetComponentsInChildren<UnityEngine.UI.Text>();
            texts[0].text = result.position.ToString();
            texts[1].text = result.playerName;
            texts[2].text = result.result.ToString("F2");
        }
    }
}
