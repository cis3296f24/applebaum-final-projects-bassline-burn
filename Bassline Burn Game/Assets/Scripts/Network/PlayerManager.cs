using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void CreatePlayer(string name)
    {
        PhotonNetwork.NickName = name;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // Connect to the Photon server
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " connected to Photon master server.");
    }
}
