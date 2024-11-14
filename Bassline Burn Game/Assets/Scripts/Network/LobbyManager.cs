using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public event Action OnJoinSuccess;
    public event Action OnPlayerJoin;
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
        // Display player names in the room
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

}
