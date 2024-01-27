using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    
    private void Awake() => Instance = this;

    public GameObject menuScreen, lobbyScreen;

    public InputField lobbyInput;

    public Text lobbyTitle, lobbyIDText;

    public Button startGameButton;

    private void Start()
    {
        OpenMainMenu();
    }

    public void CreateLobby()
    {
        BootstrapManager.CreateLobby();
    }

    public void OpenMainMenu()
    {
        CloseAllScreen();
        menuScreen.SetActive(true);
    }

    public void OpenLobby()
    {
        CloseAllScreen();
        lobbyScreen.SetActive(true);
    }

    public static void LobbyEntered(string lobbyName, bool isHost)
    {
        Instance.lobbyTitle.text = lobbyName;
        Instance.startGameButton.gameObject.SetActive(isHost);
        Instance.lobbyIDText.text = BootstrapManager.CurrentLobbyID.ToString();
        Instance.OpenLobby();
    }
    
    void CloseAllScreen()
    {
        menuScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }

    public void JoinLobby()
    {
        CSteamID steamID = new CSteamID(Convert.ToUInt64(lobbyInput.text));
        BootstrapManager.JoinByID(steamID);
    }

    public void LeaveLobby()
    {
        BootstrapManager.LeaveLobby();
        OpenMainMenu();
    }

    public void StartGame()
    {
        string[] scenesToClose = new string[] { "LobbyScene" };
        BootstrapNeyworkManager.ChangeNetworkScene("GameScene",scenesToClose);
    }
}
