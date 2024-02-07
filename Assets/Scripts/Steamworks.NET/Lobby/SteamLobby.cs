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
    
    protected Callback<LobbyMatchList_t> LobbyList;
    protected Callback<LobbyDataUpdate_t> LobbyDataUpdated; 
    
    //variables
    public ulong currentLobbyID;
    private const string HostAddressKey = "HostAddress";
    public Text lobbyNameText;
    public Text lobbyIDText;
    
    public FishySteamworks.FishySteamworks _FishySteamworks;
    
    public List<CSteamID> lobbyIDs = new List<CSteamID>();

    public InputField EnterIDField;

    public GameObject[] LobbiesObjects;

    private void Start()
    {
        if(!SteamManager.Initialized) return;
        
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        
        LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 2);

        foreach (GameObject obj in LobbiesObjects)
        {
            obj.SetActive(false);
        }
    }
    
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK) return;
        
        currentLobbyID = callback.m_ulSteamIDLobby;
        
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyID),HostAddressKey,SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyID),"name",SteamFriends.GetPersonaName().ToString() + "'s Lobby");
        
        _FishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        _FishySteamworks.StartConnection(true);

        lobbyIDText.text = currentLobbyID.ToString();
        
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
    
    public void JoinLobby(CSteamID lobbyID)
    {
        SteamMatchmaking.JoinLobby(lobbyID);
    }
    
    public void GetLobbiesList()
    {
        if(lobbyIDs.Count > 0) lobbyIDs.Clear();
        
        SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
        SteamMatchmaking.RequestLobbyList();
    }
    
    private void OnGetLobbyData(LobbyDataUpdate_t result)
    {
        LobbiesListManager.Instance.DisaplayLobbies(lobbyIDs,result);
    }
    
    private void OnGetLobbyList(LobbyMatchList_t result)
    {
        if(LobbiesListManager.Instance.listOfLobbies.Count > 0) LobbiesListManager.Instance.DestroyLobbies();
        //Tüm listeye ulaşma
        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDs.Add(lobbyId);
            SteamMatchmaking.RequestLobbyData(lobbyId);
        }
    }

    public void EnteredLobby()
    {
        string steamIDText = EnterIDField.text;
        
        CSteamID steamID;
        if (ulong.TryParse(steamIDText, out ulong steamIDValue))
        {
            steamID = (CSteamID)steamIDValue;
            JoinLobby(steamID);
        }
        
        foreach (GameObject obj in LobbiesObjects)
        {
            obj.SetActive(false);
        }
    }
    
    public void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(currentLobbyID));
        currentLobbyID = 0;
        
        foreach (GameObject obj in LobbiesObjects)
        {
            obj.SetActive(true);
        }
        
        Debug.Log("Disconnected");
    }
}
