using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon cloud
    }

    public void CreateRoom(string roomCode)
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 10 };
        PhotonNetwork.CreateRoom(roomCode, options);
    }

    public void JoinRoom(string roomCode)
    {
        PhotonNetwork.JoinRoom(roomCode);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        // Display player names in the room
        //UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player joined: " + newPlayer.NickName);
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log("Player in room: " + player.NickName);
            // Update your UI here to show player names in the lobby
        }
    }
}
