using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject leaderboardUI; // Reference to your leaderboard UI
    public bool debug;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out KartLapController kart))
        {
            SavePlayerFinishData(kart);
        }
    }

    private void SavePlayerFinishData(KartLapController kart)
    {
        if (kart.HasFinished) return;

        kart.EndRaceTick = kart.Runner.Tick;

        float finishTime = kart.GetTotalRaceTime();
        int position = RaceResults.results.Count + 1;

        RaceResults.results.Add(new PlayerResult
        {
            playerName = kart.Controller.RoomUser.Username.Value, // Ensure this property is accessible
            result = finishTime,
            position = position
        });

        if (debug)
        {
            Debug.Log($"{kart.Controller.RoomUser.Username.Value} finished in position {position} with time {finishTime:F2}s");
        }

        if (RaceResults.results.Count == RoomPlayer.Players.Count)
        {
            leaderboardUI.SetActive(true);
            leaderboardUI.GetComponent<Leaderboard>().PopulateLeaderboard();
        }
    }
}
