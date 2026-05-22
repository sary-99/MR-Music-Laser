using UnityEngine;
using Fusion;
using System.Collections.Generic;

/// <summary>
/// タスクの開始時間、終了時間、所要時間などを記録するクラス
/// Host(Quest3)でTaskが開始/終了したときにpcに指示を出す
/// </summary>
public class TaskResultRecoder : NetworkBehaviour
{
    [SerializeField] TaskResult_ScriptableObject taskResulte_SO;
    [SerializeField] TaskResultWriter taskResultWriter;
    [SerializeField] StartGameInfo startGameInfo;
    public float[] StartTime => taskResulte_SO.taskStartTime;
    public float[] EndTime => taskResulte_SO.taskEndTime;
    public float[] DurationTime => taskResulte_SO.taskDurationTime;

    void Start()
    {
        InitializeResult_ScriptableObject();
    }

    /// <summary>
    /// Task結果をリセットする
    /// </summary>
    void InitializeResult_ScriptableObject()
    {
        for (int i = 0; i < StartTime.Length; i++)
        {
            StartTime[i] = 0f;
            EndTime[i] = 0f;
            DurationTime[i] = 0f;
        }
    }

    /// <summary>
    /// タスク開始時間を記録するメソッド
    /// </summary>
    /// <param name="taskNum">タスク番号</param>
    /// <param name="startTime">開始時間</param>
    public void RecordStartTime(int taskNum, float startTime)
    {
        if (startGameInfo.isObserver == false) return;
        StartTime[taskNum] = startTime;
        Debug.Log("Task " + taskNum + " Started at time: " + startTime);
    }

    /// <summary>
    /// タスク終了時間を記録するメソッド
    /// </summary>
    /// <param name="taskNum">タスク番号</param>
    /// <param name="endTime">終了時間</param>
    public void RecordEndTime(int taskNum, float endTime)
    {
        if (startGameInfo.isObserver == false) return;
        float duration = endTime - StartTime[taskNum];
        EndTime[taskNum] = endTime;
        Debug.Log("Task " + taskNum + " Completed! Time taken: " + duration + " seconds");
    }

    /// <summary>
    /// タスクの所要時間をファイルに書き込む
    /// </summary>
    /// <param name="taskNum">Task番号</param>
    /// <param name="startTime">開始時間</param>
    /// <param name="endTime">終了時間</param>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_WriteDurationTaskResult(int taskNum, float startTime, float endTime)
    {
        if (startGameInfo.isObserver == false) return;
        Debug.Log("Writing Task" + taskNum + 1 + " Result: " + taskNum);

        if (startGameInfo.isPc == false)
        {
            //PCでない場合は書き込みしない
            return;
        }
        taskResultWriter.WriteTaskDurationResult(taskNum);
    }

    List<Vector3> _targetPosList = new List<Vector3>();
    List<Vector3> _chaserPosList = new List<Vector3>();

    /// <summary>
    /// Rpcでリストを引数にするとエラーになるため、距離データを個別に追加するための関数
    /// QuestからPCへ距離データを送信する際に使用
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_AddDistanceList(Vector3 targetPos, Vector3 chaserPos)
    {
        if (startGameInfo.isObserver == false) return;

        _targetPosList.Add(targetPos);
        _chaserPosList.Add(chaserPos);
        // _cubeDistanceList.Add(distance);
    }

    /// <summary>
    /// Task4での使用を想定
    /// </summary>
    /// <param name="taskNum">記入用</param>
    /// <param name="addTime">記入間隔</param>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_WriteDistanceTaskResult(int taskNum, float addTime)
    {
        if (startGameInfo.isObserver == false) return;
        Debug.Log("Writing Distance Task Result: " + taskNum + 1);

        taskResultWriter.Write_DistanceTaskResult(taskNum, addTime, _targetPosList, _chaserPosList);

        //リストの中身を空にする
        _targetPosList.Clear();
        _chaserPosList.Clear();
    }
}
