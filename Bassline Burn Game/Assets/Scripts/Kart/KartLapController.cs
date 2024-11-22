using System;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;


public class KartLapController : KartComponent
{
    public static event Action<KartLapController> OnRaceCompleted;

    [Networked]
    public int Lap { get; set; } = 1;

    [Networked, Capacity(5)]
    public NetworkArray<int> LapTicks { get; }

    [Networked]
    public int StartRaceTick { get; set; }

    [Networked]
    public int EndRaceTick { get; set; }

    [Networked]
    private int CheckpointIndex { get; set; } = -1;

    public event Action<int, int> OnLapChanged;
    public bool HasFinished => EndRaceTick != 0;

    private KartController Controller => Kart.Controller;
    private GameUI Hud => Kart.Hud;

    private NetworkRigidbody3D _nrb;

    private void Awake()
    {
        _nrb = GetComponent<NetworkRigidbody3D>();
    }

    public override void Spawned()
    {
        base.Spawned();

        if (GameManager.Instance.GameType.IsPracticeMode())
        {
            enabled = false;
        }
        else
        {
            Lap = 1;
        }
    }

    public override void OnRaceStart()
    {
        base.OnRaceStart();
        StartRaceTick = Runner.Tick;
    }

    public override void OnLapCompleted(int lap, bool isFinish)
    {
        base.OnLapCompleted(lap, isFinish);

        if (isFinish)
        {
            if (Object.HasInputAuthority)
            {
                // Finished race
                AudioManager.Play("raceFinishedSFX", AudioManager.MixerTarget.SFX);
                Hud.ShowEndRaceScreen();
            }

            Kart.Controller.RoomUser.HasFinished = true;
            EndRaceTick = Runner.Tick;
        }
        else
        {
            if (Object.HasInputAuthority)
            {
                AudioManager.Play("newLapSFX", AudioManager.MixerTarget.SFX);
            }
        }

        OnRaceCompleted?.Invoke(this);
    }

    public void ProcessCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint.checkPointNumber == CheckpointIndex + 1) // Check if the checkpoint is in order
        {
            CheckpointIndex = checkpoint.checkPointNumber; // Update checkpoint index
            Debug.Log($"Checkpoint {CheckpointIndex} passed!");

            if (CheckpointIndex == GameManager.CurrentTrack.checkpoints.Length - 1)
            {
                // Last checkpoint, update the lap
                ProcessFinishLine(GameManager.CurrentTrack.finishLine);
            }
        }
        else
        {
            Debug.LogWarning("Checkpoints must be passed in order!");
        }
    }




    public void ProcessFinishLine(FinishLine finishLine)
    {
        var gameType = GameManager.Instance.GameType;
        var checkpoints = GameManager.CurrentTrack.checkpoints;

        if (gameType.IsPracticeMode())
        {
            CheckpointIndex = -1;
            return;
        }

        if (CheckpointIndex == checkpoints.Length - 1 || finishLine.debug)
        {
            if (Lap == 0) return;

            LapTicks.Set(Lap - 1, Runner.Tick);
            Lap++;
            CheckpointIndex = -1;
        }
    }

    public void ResetToCheckpoint()
    {
        var tgt = CheckpointIndex == -1
            ? GameManager.CurrentTrack.finishLine.transform
            : GameManager.CurrentTrack.checkpoints[CheckpointIndex].transform;

        _nrb.Teleport(tgt.position, tgt.rotation);
        Controller.ResetState();
    }

    public float GetTotalRaceTime()
    {
        if (!Runner.IsRunning || StartRaceTick == 0)
            return 0f;

        var endTick = EndRaceTick == 0 ? Runner.Tick.Raw : EndRaceTick;
        return TickHelper.TickToSeconds(Runner, endTick - StartRaceTick);
    }
}
