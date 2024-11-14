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
        if (PhotonNetwork.IsConnected)
        {
            // Only the host/master client starts the countdown in multiplayer mode
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("StartCountdown", RpcTarget.AllBuffered);
            }
        }
        else
        {
            // Start countdown directly for offline mode
            StartCoroutine(StartCountdown());
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
    private IEnumerator StartCountdown()
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

        StartRace(); // Start the race locally or through RPC
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
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            countdownTime = 3;
            countdownText.gameObject.SetActive(true);
            elapsedTime = 0f;
            raceOngoing = false;
            timerText.text = "00:00";

            photonView.RPC("StartCountdown", RpcTarget.AllBuffered);  // Sync countdown reset in multiplayer mode
        }
        else if (!PhotonNetwork.IsConnected)
        {
            // Reset for offline mode
            countdownTime = 3;
            countdownText.gameObject.SetActive(true);
            elapsedTime = 0f;
            raceOngoing = false;
            timerText.text = "00:00";

            StartCoroutine(StartCountdown());
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
