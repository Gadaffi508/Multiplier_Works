using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance;

    private void Awake() => Instance = this;

    //Calbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;
    
    //variables
    public ulong currentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private CustomNetworkManager _manager;
    public Text lobbyNameText;
    
    public FishySteamworks.FishySteamworks _FishySteamworks;

    private void Start()
    {
        if(!SteamManager.Initialized) return;
        
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        _manager = GetComponent<CustomNetworkManager>();
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 2);
    }
    
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK) return;
        
        currentLobbyID = callback.m_ulSteamIDLobby;
        
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyID),HostAddressKey,SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyID),"name",SteamFriends.GetPersonaName().ToString() + "'s Lobby");
        
        _FishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        _FishySteamworks.StartConnection(true);
        
        Debug.Log("Lobby Created Succesfully");
    }
    
    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        
        Debug.Log("Join lobby");
    }
    
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        currentLobbyID = callback.m_ulSteamIDLobby;

        lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");
        
        _FishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID),HostAddressKey));
        _FishySteamworks.StartConnection(false);
    }
}
