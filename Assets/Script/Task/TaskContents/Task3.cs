using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class Task3 : TaskContentMq
{
    [Header("References")]
    [SerializeField] ReproDancePos reproDancePos;
    [SerializeField] TimelinePlayer timelinePlayer;

    [Header("Other GameObjects")]
    [SerializeField] GameObject danceInitPosObj;//ダンス開始位置を示すGameObject
    [SerializeField] GameObject laserSphereMesh;

    [Header("Dance Avatar")]
    [SerializeField] GameObject avatar;
    [SerializeField] Transform hips;

    [Header("Properties")]
    const int TASK_NUM = 4;
    void Start()
    {
        danceInitPosObj.SetActive(false);
    }

    public override void StartTask(float time)
    {
        if (taskResultRecoder.StartTime[TASK_NUM] > 0)
        {
            Debug.LogError("Task " + TASK_NUM + " has already been started.");
            return; //すでに開始している場合は無視
        }

        //位置合わせ
        var initPos = danceInitPosObj.transform.position;
        avatar.transform.position = initPos;

        //回転合わせ
        var initRot = new Vector3(
            avatar.transform.eulerAngles.x,
            danceInitPosObj.transform.eulerAngles.y,
            avatar.transform.eulerAngles.z
            );
        avatar.transform.rotation = Quaternion.Euler(initRot);//回転合わせ

        taskResultRecoder.RecordStartTime(TASK_NUM, time);

        //アバターを表示
        taskRpcSender.Rpc_VisibleAvatar(true);
        taskRpcSender.Rpc_StartDance();//ダンス開始を全員に通知

        Debug.Log("Task " + TASK_NUM + " Started!");
        taskUIManager.SetTaskText("Task " + TASK_NUM + "\nStart!");
    }

    float _recordInterval = 0.5f;
    float _timeSinceLastRecord = 0f;
    public override void DoTask(float time)
    {
        if (!IsTaskStarted) return;

        //タスク完了判定
        if (timelinePlayer.IsTimelineFinished())
        {
            EndTask(time);
        }
        _timeSinceLastRecord += Time.deltaTime;
        if (_timeSinceLastRecord >= _recordInterval)
        {
            //ここでデータを記録
            taskResultRecoder.Rpc_AddDistanceList(hips.position, laserSphereMesh.transform.position);
            _timeSinceLastRecord = 0f;
        }
    }

    protected override void EndTask(float time)
    {
        int taskNum = TASK_NUM - 1;//0始まりのため-1
        if (taskResultRecoder.EndTime[taskNum] > 0)
        {
            return; //すでに終了している場合は無視
        }

        taskRpcSender.Rpc_VisibleAvatar(false);//アバターを非表示
        taskRpcSender.Rpc_EndTask(TASK_NUM);
        taskUIManager.SetTaskText("Task " + TASK_NUM + "\nCompleted!");
    }

    public void StartTask_Avatar(float time)
    {
        // int taskNum = TASK_NUM - 1;//0始まりのため-1,配列用
        if (taskResultRecoder.StartTime[TASK_NUM] > 0)
        {
            Debug.LogError("Task " + TASK_NUM + " has already been started.");
            return; //すでに開始している場合は無視
        }

        //位置合わせ
        // Transform avatarPos = avatar.transform;
        var initPos = danceInitPosObj.transform.position;
        avatar.transform.position = initPos;
        //回転合わせ
        var rot = new Vector3(
            avatar.transform.eulerAngles.x,
            danceInitPosObj.transform.eulerAngles.y,
            avatar.transform.eulerAngles.z
            );
        avatar.transform.rotation = Quaternion.Euler(rot);//回転合わせ

        //タスク開始
        taskResultRecoder.RecordStartTime(TASK_NUM, time);
        taskRpcSender.Rpc_VisibleAvatar(true); //アバターを表示
        taskRpcSender.Rpc_StartDance(); //ダンス開始を全員に通知

        Debug.Log("Task " + TASK_NUM + " Started!");
        taskUIManager.SetTaskText("Task " + TASK_NUM + "\nStart!");
    }

    protected override bool endRequirement()
    {
        return timelinePlayer.IsTimelineFinished();
    }

    /// <summary>
    /// タスク開始位置を示すオブジェクトの表示切替
    /// </summary>
    public void VisibleInitPos()
    {
        danceInitPosObj.SetActive(!danceInitPosObj.activeSelf);
    }

    // public void TaskDo_Avatar(float time)
    // {
    //     if (!IsTaskStarted) return;

    //     //タスク完了判定
    //     if (timelinePlayer.IsTimelineFinished())
    //     {
    //         EndTask(time);
    //     }
    //     _timeSinceLastRecord += Time.deltaTime;
    //     if (_timeSinceLastRecord >= _recordInterval)
    //     {
    //         //ここでデータを記録
    //         taskResultRecoder.Rpc_AddDistanceList(hips.position, laserSphereMesh.transform.position);
    //         _timeSinceLastRecord = 0f;
    //     }
    // }
}