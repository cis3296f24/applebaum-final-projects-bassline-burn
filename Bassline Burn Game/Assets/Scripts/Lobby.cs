using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;

public class Lobby : MonoBehaviour
{
    // UI Elements
    public GameObject roomPanel;        // Panel for create/join room buttons
    public GameObject lobbyPanel;       // Panel for displaying the player list
    public GameObject playerListPanel;       // The panel where the player list will be displayed
    public GameObject playerListItemPrefab;  // Prefab for each player entry (can be a Text element or custom UI)
    public Transform playerListContainer;   // Container where the player list items will be instantiated (e.g., a Vertical Layout Group)
    public TMP_InputField username;     // InputField for User's nickname/username
    public TMP_InputField roomCode;     // InputField for the room code (if joining a game only!)
    public TMP_Text codeLabel;          // Text field that displays the room code on the lobby screen once game is created
    public Button joinButton;
    public Button createButton;

    // Network Objects
    private PlayerManager playerManager; // Reference to PlayerManager
    private LobbyManager lobbyManager; // Reference to LobbyManager

    // Coroutine Stuff
    bool isPhotonReady = false;
    private Action pendingAction;

    private void Start()
    {
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);

        joinButton.onClick.AddListener(OnJoinButtonClicked);
        createButton.onClick.AddListener(OnCreateButtonClicked);
        playerManager = FindObjectOfType<PlayerManager>();
        lobbyManager = FindObjectOfType<LobbyManager>();

        playerManager.OnPhotonConnected += OnPhotonConnected;
        lobbyManager.OnJoinSuccess += OnJoinSuccess;
        lobbyManager.OnPlayerJoin += UpdatePlayerList;
    }

    // Callback method when Photon is connected
    private void OnPhotonConnected()
    {
        // Mark Photon as ready
        isPhotonReady = true;

        // If there's a pending action, execute it
        pendingAction?.Invoke();
        pendingAction = null; // Reset the action after executing
    }

    // Callback method when Photon is connected
    private void OnJoinSuccess()
    {
        Transition();
    }

    public void OnJoinButtonClicked()
    {
        // Create the player first
        playerManager.CreatePlayer(getUsername());

        // If Photon is not ready, queue the join action
        if (!isPhotonReady)
        {
            Debug.Log("Waiting for Photon to connect...");
            pendingAction = () => {
                lobbyManager.JoinRoom(roomCode.text);
            };
            return;  // Skip performing the action until Photon is ready
        }

        // If Photon is already ready, join the room directly
        lobbyManager.JoinRoom(roomCode.text);
    }

    public void OnCreateButtonClicked()
    {
        // Create the player first
        playerManager.CreatePlayer(getUsername());

        // If Photon is not ready, queue the create action
        if (!isPhotonReady)
        {
            Debug.Log("Waiting for Photon to connect...");
            pendingAction = () => {
                string generatedRoomCode = genRoomCode();
                roomCode.text = generatedRoomCode;
                Debug.Log("Generated Room Code: " + generatedRoomCode);
                lobbyManager.CreateRoom(generatedRoomCode);
            };
            return;  // Skip performing the action until Photon is ready
        }

        // If Photon is already ready, create the room directly
        string generatedRoomCode = genRoomCode();
        roomCode.text = generatedRoomCode;
        Debug.Log("Generated Room Code: " + generatedRoomCode);
        lobbyManager.CreateRoom(generatedRoomCode);
    }
    private void Transition()
    {
        codeLabel.text = "Code: " + roomCode.text;
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    private void ReverseTransition()
    {
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }

    // Update the player list UI
    void UpdatePlayerList()
    {
        // Clear the previous player list (if any)
        ClearPlayerList();

        // Loop through all players in the room and create a list item for each
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log("Found Player: " + player.NickName);
            // Instantiate a new player list item (e.g., a Text element)
            GameObject playerListItem = Instantiate(playerListItemPrefab, playerListContainer);

            // Get the Text component (or any other component you want to modify)
            TMP_Text playerNameText = playerListItem.GetComponent<TMP_Text>();

            // Set the player’s name
            playerNameText.text = player.NickName;

            // Optionally, you can set other information (e.g., player status, avatar)
        }
    }

    // Clear the current player list (before updating)
    void ClearPlayerList()
    {
        // Destroy all child objects under the player list container
        foreach (Transform child in playerListContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private string getUsername()
    {
        if (username.text == "")
        {
            username.text = "Player#" + UnityEngine.Random.Range(0, 10000).ToString("D4");
        }
        return username.text;
    }

    private string genRoomCode()
    {
        string roomCode = "";
        for (int i = 0; i < 4; i++)
        {
            int randNum = UnityEngine.Random.Range(0, 36);
            char character;
            if (randNum < 10)
            {
                character = (char)('0' + randNum);
            }
            else
            {
                character = (char)('A' + (randNum - 10));
            }
            roomCode += character;
        }
        return roomCode;
    }
}
