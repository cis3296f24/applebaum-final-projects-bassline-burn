using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public event Action OnJoinSuccess;
    public event Action OnPlayerJoin;

    private void Start()
    {
        // Ensure all players are synced to the same scene
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateRoom(string roomCode)
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 20 };
        PhotonNetwork.CreateRoom(roomCode, options);
    }

    public void JoinRoom(string roomCode)
    {
        PhotonNetwork.JoinRoom(roomCode);
    }

    public override void OnJoinedRoom()
    {
        OnJoinSuccess?.Invoke();
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);

        // Optionally, start loading the game scene after joining
        StartGame();

        OnPlayerJoin?.Invoke();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);

        if (returnCode == ErrorCode.GameDoesNotExist)
        {
            Debug.Log("The room does not exist. Maybe it's closed or the name is incorrect.");
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player joined: " + newPlayer.NickName);
        OnPlayerJoin?.Invoke();
    }

    private void StartGame()
    {
        // Load the shared game scene for all players
        PhotonNetwork.LoadLevel("GameScene");
    }
}
