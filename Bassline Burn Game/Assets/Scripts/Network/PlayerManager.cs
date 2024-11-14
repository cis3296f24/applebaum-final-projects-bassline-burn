using Photon.Pun;
using System;
using UnityEngine;
public class PlayerManager : MonoBehaviourPunCallbacks
{
    public event Action OnPhotonConnected;
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void CreatePlayer(string username)
    {
        PhotonNetwork.NickName = username;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // Connect to the Photon server
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " connected to Photon master server.");
        OnPhotonConnected?.Invoke();
    }
}
