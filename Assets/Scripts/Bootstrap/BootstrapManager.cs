using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    public static BootstrapManager Instance;
    /// <summary>
    /// Grant access
    /// </summary>
    private void Awake() => Instance = this;
    
    //public string menuName = "LobbyScene";
    public NetworkManager _networkManager;
    public FishySteamworks.FishySteamworks _FishySteamworks;
    //public GameObject C_BG;

    ///<summary>
    /// Callbacks
    /// Lobby created, request and entered
    ///</summary>
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    public static ulong CurrentLobbyID;

    private void Start()
    {
        ///<summary> if Steam is Offline return </summary>
        if(!SteamManager.Initialized) return;
        
        //C_BG.SetActive(false);
        //SceneManager.LoadScene(menuName,LoadSceneMode.Additive);
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public static void CreateLobby()
    {
        //Lobby type and max members entered
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic,2);
    }
    
    /// <summary>
    /// Lobby created func
    /// </summary>
    /// <param name="callback"></param>
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        //Debug.Log("Starting lobby creation : " + callback.m_eResult.ToString());
        //if lobby is true working
        if(callback.m_eResult != EResult.k_EResultOK) return;

        CurrentLobbyID = callback.m_ulSteamIDLobby;
        //make host server to HostAddress.
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID),"HostAddres",SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID),"name",SteamFriends.GetPersonaName().ToString() + "'s Lobby");
        
        _FishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        _FishySteamworks.StartConnection(true);
        Debug.Log("Lobby created was successful");
    }
    
    /// <summary>
    /// Send a request to join the lobby.
    /// </summary>
    /// <param name="callback"></param>
    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    
    /// <summary>
    /// Join the lobby.
    /// </summary>
    /// <param name="callback"></param>
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        
        MainMenuManager.LobbyEntered(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID),"name"),_networkManager.IsServer);
        
        _FishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID),"HostAddres"));
        _FishySteamworks.StartConnection(false);
    }

    public static void JoinByID(CSteamID steamID)
    {
        Debug.Log("Attempting to join lobby with ID : " + steamID.m_SteamID);
        if (SteamMatchmaking.RequestLobbyData(steamID)) SteamMatchmaking.JoinLobby(steamID);
        else Debug.Log("Failed to join lobby with ID : " + steamID.m_SteamID);
    }

    public static void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyID));
        CurrentLobbyID = 0;

        Instance._FishySteamworks.StopConnection(false);
        if(Instance._networkManager.IsServer) Instance._FishySteamworks.StopConnection(true);
    }
}
