using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object.Synchronizing;
using FishNet.Object;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour
{
    [SyncVar] public int connectionID;
    [SyncVar] public int playerIdNumber;
    [SyncVar] public ulong playerSteamID;

    [SyncVar(OnChange = nameof(PlayerNameUpdate))]
    public string playerName;

    public override void OnStartClient()
    {
        LobbyController.Instance.UpdateLobbyName();
        LobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        LobbyController.Instance.UpdatePlayerList();
    }

    public void PlayerNameUpdate(string oldValue, string newValue,bool asServer)
    {
        if (IsServer)
        {
            playerName = newValue;
        }

        if (IsClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }
    
    private void CmdSetPlayerName(string _playerName)
    {
        this.PlayerNameUpdate(this.playerName,_playerName,true);
    }
}
