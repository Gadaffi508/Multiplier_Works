using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance;
    
    //UI Elements
    public Text LobbyNameText;
    
    //Player Data
    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    
    //Other Data
    public ulong CurrentLobbyID;
    public bool PlayerItemCreated = false;
    private List<PlayerLıstItem> _playerItems = new List<PlayerLıstItem>();
    
    //Ready
    public Button StartGameButton;
    public Text ReadyButtonText;

    public Text LobbyId;

    private CustomNetworkManager customNetworkManager;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        customNetworkManager = GetComponent<CustomNetworkManager>();
    }

    
    //Lobi İsim güncelleme
    public void UpdateLobbyName()
    {
        CurrentLobbyID = SteamLobby.Instance.currentLobbyID;
        LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
        
    }

    public void UpdatePlayerList()
    {
        if (!PlayerItemCreated) CreateHostPlayerItem(); // Host
        //işlemcinin bağlanıp bağlanmadığını kontrol ediyoruz
        if (_playerItems.Count < SteamMatchmaking.GetNumLobbyMembers((CSteamID)CurrentLobbyID)) CreateClientPlayerItem();
        //Oyuncu bağlantı koması yaşarsa
        if (_playerItems.Count > SteamMatchmaking.GetNumLobbyMembers((CSteamID)CurrentLobbyID)) RemovePlayerItem();
        //Lobideki insanları güncelleme
        if (_playerItems.Count == SteamMatchmaking.GetNumLobbyMembers((CSteamID)CurrentLobbyID)) UpdatePlayerItem();
    }

    public void CreateHostPlayerItem()
    {
        //Ana bilgisauar öğemizde her döngü için bir döngüye sahip olma 
        foreach (NetworkPlayerController player in customNetworkManager.GamePlayer)
        {
            GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
            PlayerLıstItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerLıstItem>();

            NewPlayerItemScript.PlayerName = player.playerName;
            NewPlayerItemScript.ConnectionId = player.connectionID;
            NewPlayerItemScript.PlayerSteamID = player.playerSteamID;
            NewPlayerItemScript.SetPlayerValues();
            NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            NewPlayerItem.transform.localScale = Vector3.one;
            
            _playerItems.Add(NewPlayerItemScript);
        }

        PlayerItemCreated = true;
    }
    
    public void CreateClientPlayerItem()
    {
        foreach (NetworkPlayerController player in customNetworkManager.GamePlayer)
        {
            //listede olup olmadığını kontrol ediyoruz
            if (!_playerItems.Any(b => b.ConnectionId == player.connectionID))
            {
                GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                PlayerLıstItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerLıstItem>();

                NewPlayerItemScript.PlayerName = player.playerName;
                NewPlayerItemScript.ConnectionId = player.connectionID;
                NewPlayerItemScript.PlayerSteamID = player.playerSteamID;
                NewPlayerItemScript.SetPlayerValues();
            
                NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
                NewPlayerItem.transform.localScale = Vector3.one;
            
                _playerItems.Add(NewPlayerItemScript);
            }
        }
    }
    
    public void UpdatePlayerItem()
    {
        foreach (NetworkPlayerController player in customNetworkManager.GamePlayer)
        {
            foreach (PlayerLıstItem PlayerListItemScript in _playerItems)
            {
                if (PlayerListItemScript.ConnectionId == player.connectionID)
                {
                    PlayerListItemScript.PlayerName = player.playerName;
                    PlayerListItemScript.SetPlayerValues();
                }
            }
        }
    }
    
    public void RemovePlayerItem()
    {
        List<PlayerLıstItem> playerListItemToRemove = new List<PlayerLıstItem>();

        foreach (PlayerLıstItem playerListItem in _playerItems)
        {
            //içinde olduğumuzu kontrol ediyoruz
            if (customNetworkManager.GamePlayer.Any(b => b.connectionID == playerListItem.ConnectionId))
            {
                playerListItemToRemove.Add(playerListItem);
            }
        }

        if (playerListItemToRemove.Count > 0)
        {
            foreach (PlayerLıstItem playerlistItemToRemove in playerListItemToRemove)
            {
                GameObject ObjectToRemove = playerlistItemToRemove.gameObject;
                _playerItems.Remove(playerlistItemToRemove);
                Destroy(ObjectToRemove);
                ObjectToRemove = null;
            }
        }
    }

    public void StartGame(string SceneName)
    {
        if (SteamMatchmaking.GetNumLobbyMembers((CSteamID)SteamLobby.Instance.currentLobbyID) == 2)
        {
            Debug.Log("Connection.");
        }
        else
        {
            Debug.Log("You need 1 person.");
        }
        //LocalPlayerController.CanStartGame(SceneName);
    }
}
