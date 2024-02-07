using System;
using System.Collections.Generic;
using FishNet.Managing;
using Steamworks;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private NetworkPlayerController GamePlayerPrefabs;

    public List<NetworkPlayerController> GamePlayer = new List<NetworkPlayerController>();

    private void OnServerInitialized()
    {
        ConnectedPlayer conn;
        NetworkPlayerController GamePlayerInstance = Instantiate(GamePlayerPrefabs);
        //verileri imzalama, nokta bağlantı kimliğne geçiyoruz oyuncu numarasınıda ayarlıyoruz
        //GamePlayerInstance.connectionID = conn.playerId;
        GamePlayerInstance.playerIdNumber = GamePlayer.Count + 1;
        GamePlayerInstance.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex(
            (CSteamID)SteamLobby.Instance.currentLobbyID,
            GamePlayer.Count
        );
    }

    private void OnConnectedToServer()
    {
        ConnectedPlayer conn;
        NetworkPlayerController GamePlayerInstance = Instantiate(GamePlayerPrefabs);
        //verileri imzalama, nokta bağlantı kimliğne geçiyoruz oyuncu numarasınıda ayarlıyoruz
        //GamePlayerInstance.connectionID = conn.playerId;
        GamePlayerInstance.playerIdNumber = GamePlayer.Count + 1;
        GamePlayerInstance.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex(
            (CSteamID)SteamLobby.Instance.currentLobbyID,
            GamePlayer.Count
        );
    }

    public void StartGame(string SceneName)
    {
        //Sahne değiştirme
        if (SteamMatchmaking.GetNumLobbyMembers((CSteamID)SteamLobby.Instance.currentLobbyID) != 2)
        {
            Debug.Log("You are the only person in the match..");
        }
    }
}
