using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object.Synchronizing;
using FishNet.Object;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour
{
    private CustomNetworkManager _manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (_manager != null)
            {
                return _manager;
            }

            return _manager = CustomNetworkManager.Instances.Single() as CustomNetworkManager;
        }
    }
    
    [SyncVar] public int connectionID;
    [SyncVar] public int playerIdNumber;
    [SyncVar] public ulong playerSteamID;

    [SyncVar(OnChange = nameof(PlayerNameUpdate))]
    public string playerName;

    public override void OnStartClient()
    {
        _manager.GamePlayer.Add(this);
        LobbyController.Instance.UpdateLobbyName();
        LobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        _manager.GamePlayer.Remove(this);
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
