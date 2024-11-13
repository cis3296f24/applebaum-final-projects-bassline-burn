using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    // UI Panels
    public GameObject roomPanel;        // Panel for create/join room buttons
    public GameObject playerListPanel;  // Panel for displaying the player list
    private PlayerManager playerManager; // Reference to PlayerManager

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        DontDestroyOnLoad(this);
    }

    private void Transition()
    {
        roomPanel.SetActive(false);
        playerListPanel.SetActive(true);
    }
    public void JoinButton()
    {
        Transition();
        playerManager.CreatePlayer("Test");
    }

    public void CreateButton()
    {
        Transition();
        playerManager.CreatePlayer("Test");
    }
}
