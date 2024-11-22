using System;
using Fusion;
using UnityEngine;
using TMPro; // For UI text
using System.Collections;

public class GameManager : NetworkBehaviour
{
    public static event Action<GameManager> OnLobbyDetailsUpdated;

    // Layers
    [SerializeField, Layer] private int groundLayer;
    public static int GroundLayer => Instance.groundLayer;
    [SerializeField, Layer] private int kartLayer;
    public static int KartLayer => Instance.kartLayer;

    // Race Management
    public bool raceStart = false;
    public int totalLaps = 3; // Set the total number of laps
    [Networked] private int CurrentLap { get; set; }
    [Networked] private float RaceTime { get; set; }
    private bool isCountdownActive = false;

    // Camera Management
    public new Camera camera;
    private ICameraController cameraController;

    public GameType GameType => ResourceManager.Instance.gameTypes[GameTypeId];
    public static Track CurrentTrack { get; private set; }
    public static bool IsPlaying => CurrentTrack != null;

    public static GameManager Instance { get; private set; }

    public string TrackName => ResourceManager.Instance.tracks[TrackId].trackName;
    public string ModeName => ResourceManager.Instance.gameTypes[GameTypeId].modeName;

    // Networking
    [Networked] public NetworkString<_32> LobbyName { get; set; }
    [Networked] public int TrackId { get; set; }
    [Networked] public int GameTypeId { get; set; }
    [Networked] public int MaxUsers { get; set; }

    // UI Elements
    public TMP_Text timerText; // Assign in Unity editor
    public TMP_Text lapText;   // Assign in Unity editor
    public TMP_Text countdownText; // For countdown (assign in editor)

    private static void OnLobbyDetailsChangedCallback(GameManager changed)
    {
        OnLobbyDetailsUpdated?.Invoke(changed);
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasStateAuthority)
        {
            LobbyName = ServerInfo.LobbyName;
            TrackId = ServerInfo.TrackId;
            GameTypeId = ServerInfo.GameMode;
            MaxUsers = ServerInfo.MaxUsers;
        }
    }

    private void LateUpdate()
    {
        // Camera controller logic
        if (cameraController == null) return;
        if (cameraController.Equals(null))
        {
            Debug.LogWarning("Phantom object detected");
            cameraController = null;
            return;
        }

        if (!cameraController.ControlCamera(camera))
            cameraController = null;

        // Update the race timer
        if (raceStart && Object.HasInputAuthority)
        {
            RaceTime += Time.deltaTime;
            UpdateTimerDisplay(RaceTime);
        }
    }

    // Countdown and Race Start
    public void StartRaceSequence()
    {
        if (isCountdownActive) return; // Prevent multiple countdowns
        StartCoroutine(StartCountdownAndRace());
    }

    private IEnumerator StartCountdownAndRace()
    {
        isCountdownActive = true;
        int countdown = 3;

        // Disable all karts before countdown starts
        DisableKarts();

        while (countdown > 0)
        {
            UpdateCountdownDisplay(countdown);
            Debug.Log($"{countdown}...");
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        UpdateCountdownDisplay(0); // Display "GO!"
        StartRace();
        isCountdownActive = false;
    }

    private void StartRace()
    {
        raceStart = true;
        RaceTime = 0f;
        CurrentLap = 0;
        UpdateLapDisplay();
        EnableKarts();
        Debug.Log("Race started!");
    }

    public void EndRace()
    {
        raceStart = false;
        Debug.Log($"Race ended! Final time: {RaceTime:F2} seconds");
    }

    private void EnableKarts()
    {
        foreach (var player in RoomPlayer.Players)
        {
            if (player.Kart != null)
            {
                player.Kart.CanDrive = true; // Allow karts to drive
            }
        }
    }

    private void DisableKarts()
    {
        foreach (var player in RoomPlayer.Players)
        {
            if (player.Kart != null)
            {
                player.Kart.CanDrive = false; // Prevent karts from moving
            }
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    private void UpdateLapDisplay()
    {
        lapText.text = $"Lap: {CurrentLap}/{totalLaps}";
    }

    private void UpdateCountdownDisplay(int countdown)
    {
        if (countdownText == null) return;

        countdownText.text = countdown > 0 ? countdown.ToString() : "GO!";
        if (countdown == 0) Debug.Log("Go!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            if (raceStart && CurrentLap < totalLaps)
            {
                CurrentLap++;
                UpdateLapDisplay();

                if (CurrentLap == totalLaps)
                {
                    Debug.Log("Race completed!");
                    EndRace();
                }
            }
        }
    }

    public static void GetCameraControl(ICameraController controller)
    {
        Instance.cameraController = controller;
    }

    public static bool IsCameraControlled => Instance.cameraController != null;

    public static void SetTrack(Track track)
    {
        CurrentTrack = track;
    }
}
