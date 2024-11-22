using Fusion;
using UnityEngine;

public class Track : NetworkBehaviour, ICameraController
{
    public static Track Current { get; private set; }

    [Networked] public TickTimer StartRaceTimer { get; set; }

    public CameraTrack[] introTracks;
    public Checkpoint[] checkpoints;
    public Transform[] spawnpoints;
    public FinishLine finishLine;

    public TrackDefinition definition;
    public TrackStartSequence sequence;

    public string music = "";
    public float introSpeed = 0.5f;

    private int _currentIntroTrack;
    private float _introIntervalProgress;

    private void Awake()
    {
        Current = this;

        // Ensure checkpoints array is properly initialized
        if (checkpoints == null || checkpoints.Length == 0)
        {
            Debug.LogWarning("Checkpoints array is empty. Attempting to auto-populate from children...");
            checkpoints = GetComponentsInChildren<Checkpoint>();

            if (checkpoints.Length == 0)
            {
                Debug.LogError("No checkpoints found! Ensure they are added as children of the Track GameObject.");
                return;
            }
        }

        InitCheckpoints();

        // Initialize cutscene
        AudioManager.StopMusic();
        GameManager.SetTrack(this);
        GameManager.Instance.camera = Camera.main;
        StartIntro();
    }

    public override void Spawned()
    {
        base.Spawned();

        if (RoomPlayer.Local.IsLeader)
        {
            StartRaceTimer = TickTimer.CreateFromSeconds(Runner, sequence.duration + 4f);
        }

        sequence.StartSequence();
    }

    private void OnDestroy()
    {
        GameManager.SetTrack(null);
    }

    public void SpawnPlayer(NetworkRunner runner, RoomPlayer player)
    {
        var index = RoomPlayer.Players.IndexOf(player);

        // Ensure spawnpoints are valid
        if (spawnpoints == null || spawnpoints.Length == 0)
        {
            Debug.LogError("No spawnpoints defined in the Inspector!");
            return;
        }

        if (index >= spawnpoints.Length)
        {
            Debug.LogError($"Invalid spawnpoint index: {index}. Check spawnpoints array.");
            return;
        }

        var point = spawnpoints[index];
        var prefabId = player.KartId;
        var prefab = ResourceManager.Instance.kartDefinitions[prefabId].prefab;

        // Spawn player
        var entity = runner.Spawn(
            prefab,
            point.position,
            point.rotation,
            player.Object.InputAuthority
        );

        entity.Controller.RoomUser = player;
        player.GameState = RoomPlayer.EGameState.GameCutscene;
        player.Kart = entity.Controller;

        Debug.Log($"Spawning kart for {player.Username} as {entity.name}");
        entity.transform.name = $"Kart ({player.Username})";
    }

    private void InitCheckpoints()
    {
        if (checkpoints == null || checkpoints.Length == 0)
        {
            Debug.LogError("Checkpoints are not set up in the Inspector or found in children!");
            return;
        }

        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i] == null)
            {
                Debug.LogError($"Checkpoint at index {i} is null. Ensure all checkpoints are assigned.");
                continue;
            }

            checkpoints[i].checkPointNumber = i; // Assign unique number to each checkpoint
            Debug.Log($"Checkpoint {i} initialized with index {checkpoints[i].checkPointNumber}.");
        }
    }

    public bool ControlCamera(Camera cam)
    {
        if (introTracks == null || introTracks.Length == 0)
        {
            Debug.LogError("Intro tracks are not defined!");
            return false;
        }

        cam.transform.position = Vector3.Lerp(
            introTracks[_currentIntroTrack].startPoint.position,
            introTracks[_currentIntroTrack].endPoint.position,
            _introIntervalProgress);

        cam.transform.rotation = Quaternion.Slerp(
            introTracks[_currentIntroTrack].startPoint.rotation,
            introTracks[_currentIntroTrack].endPoint.rotation,
            _introIntervalProgress);

        _introIntervalProgress += Time.deltaTime * introSpeed;
        if (_introIntervalProgress > 1)
        {
            _introIntervalProgress -= 1;
            _currentIntroTrack++;
            if (_currentIntroTrack == introTracks.Length)
            {
                _currentIntroTrack = 0;
                _introIntervalProgress = 0;
                return false;
            }
        }

        return true;
    }

    public void StartIntro()
    {
        _currentIntroTrack = 0;
        _introIntervalProgress = 0;
        AudioManager.PlayMusic("intro");
        GameManager.GetCameraControl(this);
    }
}
