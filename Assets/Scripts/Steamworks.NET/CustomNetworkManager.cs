using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing;
using Steamworks;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private NetworkPlayerController GamePlayerPrefabs;

    public List<NetworkPlayerController> GamePlayer = new List<NetworkPlayerController>();

    public void StartGame(string SceneName)
    {
        //Sahne değiştirme
        if (SteamMatchmaking.GetNumLobbyMembers((CSteamID)SteamLobby.Instance.currentLobbyID) != 2)
        {
            Debug.Log("You are the only person in the match..");
        }
    }
    public void SpawnPlayers()
    {
        if (GamePlayerPrefabs == null)
        {
            Debug.LogError("GamePlayerPrefab is not assigned!");
            return;
        }
        for (int i = 0; i< SteamMatchmaking.GetNumLobbyMembers((CSteamID)SteamLobby.Instance.currentLobbyID) + 1;i++)
        {
            GameObject playerObject = Instantiate(GamePlayerPrefabs.gameObject);

            NetworkPlayerController playerController = playerObject.GetComponent<NetworkPlayerController>();

            if (playerController == null)
            {
                Debug.LogError("NetworkPlayerController component is missing on the GamePlayerPrefab!");
                Destroy(playerObject);
                continue;
            }

            GamePlayer.Add(playerController);
            return;
        }
    }
}
