using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("References")]
    [SerializeField] StartGameInfo startGameInfo;
    [SerializeField] GameObject metaQuestRoot;
    [SerializeField] GameObject pcRoot;
    [SerializeField] NetworkRunner networkRunner;
    [SerializeField] float actionTime = 1f;

    void Awake()
    {
        Application.targetFrameRate = 72;
    }

    void Start()
    {
        DeviceSetup();
        if (startGameInfo.isAuthority)
        {
            AssignHost();
        }
        else
        {
            AssignClient();
        }
    }

    void DeviceSetup()
    {
        metaQuestRoot?.SetActive(startGameInfo.isMetaQuest);
        pcRoot?.SetActive(startGameInfo.isPc);
        Debug.Log($"GameLauncher DeviceSetup: isPc={startGameInfo.isPc}, isMetaQuest={startGameInfo.isMetaQuest}");
    }

    public async void StartGame(GameMode mode)
    {
        networkRunner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"現在のシーン名: {sceneName}, buildIndex: {buildIndex}");

        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        DebugTxtManager.Instance.DebugTxtPlusL($"StartGame Result: {result}\n");
        Debug.Log($"StartGame Result: {result}");
    }
    public void AssignHost()
    {
        StartGame(GameMode.Host);

        DebugTxtManager.Instance.DebugTxtPlusL("Assigned as Host\n");
        Debug.Log("Assigned as Host");
    }
    public void AssignClient()
    {
        StartGame(GameMode.Client);


        DebugTxtManager.Instance.DebugTxtPlusL("Assigned as Client\n");
        Debug.Log("Assigned as Client");
    }

    // NetworkRunner.StartGame() でセッションに入れなかった場合、
    // NetworkRunnerが削除されてしまう仕様みたいなので入室の度に生成する
    // private NetworkRunner GetOrInstantiateNetworkRunnerIfNeed()
    // {
    //     var runner = FindFirstObjectByType<NetworkRunner>();
    //     if (runner == null)
    //     {
    //         runner = Instantiate(networkRunner);
    //         Debug.Log("Instantiated NetworkRunner");
    //     }
    //     return runner;
    // }

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined called");
        if (player == runner.LocalPlayer)
        {
            Debug.Log("You joined");
            DebugTxtManager.Instance.DebugTxtPlusL($"you joined your {player}\n");
            DebugTxtManager.Instance.DebugTxtPlusL($"your PlayerID: {player.PlayerId}\n");
        }
        else
        {
            Debug.Log($"Player Joined: {player} ID: {player.PlayerId}");
            DebugTxtManager.Instance.DebugTxtPlusL($"Player Joined: {player} ID: {player.PlayerId}\n");
        }
    }
    // NetworkRunner関連のコールバック
    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        var behaviours = obj.GetComponents<NetworkBehaviour>();
        Debug.Log($"[OnObjectEnterAOI] {obj.name}, Behaviours: {behaviours.Length}, Active: {obj.gameObject.activeSelf}");
        DebugTxtManager.Instance.DebugTxtPlusL($"Spawned: {obj.name}, Behaviours: {behaviours.Length}\n");
    }
    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
}