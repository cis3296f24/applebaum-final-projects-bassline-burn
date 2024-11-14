using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public Text timerText;
    public Text countdownText;
    public int countdownTime = 3;

    private float elapsedTime;
    public bool raceOngoing = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartCountdown", RpcTarget.AllBuffered);
        }
    }

    private void Update()
    {
        // Update the timer only if the race is ongoing
        if (raceOngoing)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = FormatTime(elapsedTime);
        }
    }

    [PunRPC]
    IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        photonView.RPC("StartRace", RpcTarget.AllBuffered);  // Sync race start
    }

    [PunRPC]
    public void StartRace()
    {
        raceOngoing = true;
        elapsedTime = 0f;
        Debug.Log("Race Started!");
    }

    [PunRPC]
    public void EndRace()
    {
        raceOngoing = false;
        Debug.Log("Race Ended! Final Time: " + elapsedTime);
    }

    public void ResetGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Reset countdown and timer, then sync countdown to all players
            countdownTime = 3;
            countdownText.gameObject.SetActive(true);
            elapsedTime = 0f;
            raceOngoing = false;
            timerText.text = "00:00";

            photonView.RPC("StartCountdown", RpcTarget.AllBuffered);  // Sync countdown reset
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
