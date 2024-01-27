using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class BootstrapNeyworkManager : NetworkBehaviour
{
    public static BootstrapNeyworkManager Instance;

    private void Awake() => Instance = this;

    public static void ChangeNetworkScene(string sceneName, string[] scenesToClose)
    {
        Instance.CloseScenes(scenesToClose);

        SceneLoadData sld = new SceneLoadData(sceneName);

        NetworkConnection[] conns = Instance.ServerManager.Clients.Values.ToArray();
        
        Instance.SceneManager.LoadConnectionScenes(conns,sld);
    }

    [ServerRpc(RequireOwnership = false)]
    void CloseScenes(string[] scenesToClose)
    {
        CloseScenesObServer(scenesToClose);
    }

    [ObserversRpc]
    void CloseScenesObServer(string[] scenesToClose)
    {
        foreach (var sceneName in scenesToClose)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
