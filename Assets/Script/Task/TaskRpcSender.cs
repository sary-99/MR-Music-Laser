using Fusion;
using UnityEngine;

/// <summary>
/// PCのUIからQuest3(Host)へタスクに関するRpcリクエストを送信するためのクラス
/// </summary>
public class TaskRpcSender : NetworkBehaviour
{
    [SerializeField] TaskManager taskManager;
    [SerializeField] Task3 task3;
    [SerializeField] GameObject danceAvatar;
    [SerializeField] TimelinePlayer danceTimelinePlayer;

    /// <summary>
    /// PC->Host
    /// タスク開始を指示するRPC
    /// </summary>
    /// <param name="taskNum"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_StartTask(int taskNum)
    {
        taskManager?.StartTask(taskNum);
    }

    /// <summary>
    /// Host->PC
    /// タスクが終了したことを伝えるRPC
    /// </summary>
    /// <param name="taskNum"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_EndTask(int taskNum)
    {
        taskManager?.EndTask(taskNum);
    }

    #region Task3
    /// <summary>
    /// Task4で使用
    /// アバターがダンスを開始する初期位置を表示
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_VisibleDanceInitPos()
    {
        task3?.VisibleInitPos();
    }

    /// <summary>
    /// アバターのダンスアニメーションを開始する
    /// Task4開始と同時に呼ばれる
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_StartDance()
    {
        danceTimelinePlayer?.PlayTimeline();
        Debug.Log("Rpc_StartDance called: Dance timeline started.");
    }

    /// <summary>
    /// アバターのダンスアニメーションを停止する
    /// </summary>
    // [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    // public void Rpc_StopDance()
    // {
    //     danceTimelinePlayer?.StopTimeline();
    // }

    /// <summary>
    /// アバターの表示/非表示を切り替えるRPC
    /// </summary>
    /// <param name="isVisible"></param>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_VisibleAvatar(bool isVisible)
    {
        danceAvatar.SetActive(isVisible);
        Debug.Log("Rpc_VisibleAvatar called: Avatar visibility set to " + isVisible);
    }
    #endregion
}
