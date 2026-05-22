using UnityEngine;
using Fusion;
using Unity.VisualScripting;

public class TaskManager : MonoBehaviour
{
    [Header("Refaerences")]
    [SerializeField] StartGameInfo startGameInfo;
    [SerializeField] TaskResultRecoder taskResultRecoder;
    // [SerializeField] TimerOperater timerOperater;
    // [SerializeField] TaskResultRecoder taskResultRecoder;
    [SerializeField] TaskCubeGenerator taskCubeGenerator;
    [SerializeField] TaskContentMq[] taskContents = new TaskContentMq[4];//タスクの内容が書かれたクラスを入れる配列

    [Header("Task Start Boolean")]
    /// <summary>
    /// true:タスク開始 false:タスク未開始
    /// </summary>
    bool[] _isTaskStarted_Array => new bool[4] {
            taskContents[0].IsTaskStarted,
            taskContents[1].IsTaskStarted,
            taskContents[2].IsTaskStarted,
            taskContents[3].IsTaskStarted
        };

    [Header("Target spheres")]
    [SerializeField] GameObject leftSphere;
    [SerializeField] GameObject centerSphere;
    [SerializeField] GameObject rightSphere;
    public GameObject LeftSphereMesh => leftSphere.transform.GetChild(0).gameObject;
    public GameObject CenterSphereMesh => centerSphere.transform.GetChild(0).gameObject;
    public GameObject RightSphereMesh => rightSphere.transform.GetChild(0).gameObject;

    [Header("Task Cubes")]
    GameObject _leftCube;
    GameObject _centerCube;
    GameObject _rightCube;
    public GameObject LeftCube => _leftCube;
    public GameObject CenterCube => _centerCube;
    public GameObject RightCube => _rightCube;

    void Awake()
    {
        if (!startGameInfo.isAuthority || !startGameInfo.isMetaQuest) return;

        taskCubeGenerator.GenerateCubes();
        _centerCube = taskCubeGenerator.CenterCube;
        _leftCube = taskCubeGenerator.LeftCube;
        _rightCube = taskCubeGenerator.RightCube;
    }

    void Start()
    {
        // Debug.Log("LeftSpheremesh", LeftSphereMesh.gameObject);
        // Debug.Log("CenterSpheremesh", CenterSphereMesh.gameObject);
        // Debug.Log("RightSpheremesh", RightSphereMesh.gameObject);
    }

    /// <summary>
    /// タスクを開始する
    /// </summary>
    /// <param name="taskNum">開始するタスク番号</param>
    public void StartTask(int taskNum)
    {
        if (!startGameInfo.isAuthority || !startGameInfo.isMetaQuest) return;

        float startTime;//タスク開始時刻

        //タスク開始時刻を取得  
        Timer.Instance.StartTimer();
        startTime = Timer.Instance.GetTime();

        //タスクを開始
        if (taskContents[taskNum] != null)
        {
            taskContents[taskNum].StartTask(startTime);
        }
        else
        {
            Debug.LogError("TaskManager: Task content for " + taskNum + " is not assigned.");
            return;
        }
    }

    /// <summary>
    /// タスクの内容を実行する
    /// タスク開始後、毎フレームUpdateで呼ばれる
    /// </summary>
    /// <param name="taskIndex"></param>
    void DoTask(int taskIndex)
    {
        if (!startGameInfo.isAuthority || !startGameInfo.isMetaQuest) return;

        float currentTime = Timer.Instance.GetTime();

        if (taskContents[taskIndex] != null)
        {
            taskContents[taskIndex].DoTask(currentTime);
        }
        else
        {
            Debug.LogError("TaskManager: Task content for " + taskIndex + " is not assigned.");
            return;
        }
    }

    /// <summary>
    /// タスクを終了し、タスク完遂に要した時間を記録する
    /// </summary>
    /// <param name="taskNum"></param>
    public void EndTask(int taskNum)
    {
        if (!startGameInfo.isAuthority || !startGameInfo.isMetaQuest) return;

        float endTime = Timer.Instance.GetTime();
        taskResultRecoder.RecordEndTime(taskNum, endTime);
        taskResultRecoder.Rpc_WriteDurationTaskResult(taskNum, taskResultRecoder.StartTime[taskNum], endTime);
    }

    void Update()
    {
        for (int i = 0; i < _isTaskStarted_Array.Length; i++)
        {
            if (_isTaskStarted_Array[i])
            {
                DoTask(i);
            }
        }
    }
}
