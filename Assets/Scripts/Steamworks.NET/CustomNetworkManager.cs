using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using Steamworks;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private NetworkPlayerController GamePlayerPrefabs;

    public List<NetworkPlayerController> GamePlayer { get; } = new List<NetworkPlayerController>();

    private void OnServerInitialized(ConnectedPlayer con)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LobbyScene")
        {
            NetworkPlayerController gamePlayerInstance = Instantiate(GamePlayerPrefabs);

            gamePlayerInstance.connectionID = con.playerId;
            gamePlayerInstance.playerIdNumber = GamePlayer.Count + 1;
            gamePlayerInstance.playerSteamID =
                (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.currentLobbyID,
                    GamePlayer.Count);
        }
    }
}
