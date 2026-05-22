using UnityEngine;

/// <summary>
/// タスク0
/// </summary>
public class Task0 : TaskContentMq
{
    [SerializeField] TaskCubeHitChecker taskCubeHitChecker;

    [Header("Properties")]
    const int TASK_NUM = 1;
    Vector3[] targetCubePos =
    {
        new Vector3(2f, 1f, 2f / 2),//left
        new Vector3(0, 1f, 4f / 2),//center
        new Vector3(-2f, 1f, 2f / 2)//right
    };
    GameObject _leftCube;
    GameObject _centerCube;
    GameObject _rightCube;

    public override void StartTask(float time)
    {
        if (taskResultRecoder.StartTime[TASK_NUM] > 0)
        {
            Debug.LogWarning("Task " + TASK_NUM + " has already been started.");
            return; //すでに開始している場合は無視
        }


        //開始時刻を記録
        taskResultRecoder.RecordStartTime(TASK_NUM, time);
        _isTaskStarted = true;

        //目標(キューブ)の配置
        PlaceCubes();

        Debug.Log("Task " + TASK_NUM + " Started!");
        taskUIManager.SetTaskText("Task " + TASK_NUM + "Start!");
    }

    /// <summary>
    /// Sphereがキューブにヒットしたか
    /// </summary>
    public override void DoTask(float time)
    {
        if (!_isTaskStarted) return;

        _isLeftHit = taskCubeHitChecker.CheckHit(taskManager.LeftSphereMesh.transform.position, EffectLists.EffectType.leftLaser);
        _isCenterHit = taskCubeHitChecker.CheckHit(taskManager.CenterSphereMesh.transform.position, EffectLists.EffectType.centerLaser);
        _isRightHit = taskCubeHitChecker.CheckHit(taskManager.RightSphereMesh.transform.position, EffectLists.EffectType.rightLaser);

        //ヒットしたらキューブを非表示にする
        _leftCube.GetComponent<TaskCube>().VisibleCube(!_isLeftHit);
        _centerCube.GetComponent<TaskCube>().VisibleCube(!_isCenterHit);
        _rightCube.GetComponent<TaskCube>().VisibleCube(!_isRightHit);

        if (endRequirement())
        {
            EndTask(time);
        }
    }

    protected override void EndTask(float time)
    {
        if (taskResultRecoder.EndTime[TASK_NUM] > 0)
        {
            Debug.LogWarning("Task " + TASK_NUM + " has already been completed.");
            return; //すでに終了している場合は無視
        }

        taskRpcSender.Rpc_EndTask(TASK_NUM);
        taskUIManager.SetTaskText("Task " + TASK_NUM + "\nCompleted!");
    }

    /// <summary>
    /// 目標であるCubeの配置
    /// </summary>
    void PlaceCubes()
    {
        _leftCube = taskManager.LeftCube;
        _centerCube = taskManager.CenterCube;
        _rightCube = taskManager.RightCube;

        _leftCube.transform.position = targetCubePos[0];
        _centerCube.transform.position = targetCubePos[1];
        _rightCube.transform.position = targetCubePos[2];

        _leftCube.GetComponent<TaskCube>().VisibleCube(true);
        _centerCube.GetComponent<TaskCube>().VisibleCube(true);
        _rightCube.GetComponent<TaskCube>().VisibleCube(true);
    }

    bool _isLeftHit, _isCenterHit, _isRightHit = false;
    /// <summary>
    /// 3つのキューブすべてにヒットしたらタスク終了
    /// </summary>
    /// <returns></returns>
    protected override bool endRequirement()
    {
        return _isLeftHit && _isCenterHit && _isRightHit;
    }
}
