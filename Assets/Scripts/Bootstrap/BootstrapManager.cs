using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    public string menuName = "OutdoorsScene";
    public NetworkManager _networkManager;
    public FishySteamworks.FishySteamworks _FishySteamworks;

    ///<summary>
    /// Callbacks
    /// Lobby created, request and entered
    ///</summary>
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    private void Start()
    {
        ///<summary> Steam is Online and Start load the menuscene </summary>
        if(SteamManager.Initialized)
            SceneManager.LoadScene(menuName,LoadSceneMode.Additive);
    }
}
