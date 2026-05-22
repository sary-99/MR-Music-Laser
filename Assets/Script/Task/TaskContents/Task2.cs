using UnityEngine;

public class Task2 : TaskContentMq
{
    [Header("References")]
    [SerializeField] TaskCubeHitChecker taskCubeHitChecker;
    [SerializeField] LaserColorChecker colorChecker;
    [SerializeField] LaserColorResetter laserColorResetter;

    [Header("Properties")]
    const int TASK_NUM = 3;
    [SerializeField]
    Vector3[] targetCubePos =
    {
        new Vector3(3f, 0f, 3f),//left
        new Vector3(0, 3f, -2f),//center
        new Vector3(-3f, 0f, 3f)//right
    };
    [SerializeField]
    Task1.Colors[] targetColors ={
        new Task1.Colors { colorName = "Blue", colorValue = Color.blue },//(left)
        new Task1.Colors { colorName = "Red", colorValue = Color.red },//(center)
        new Task1.Colors { colorName = "Blue", colorValue = Color.blue }//(right)
    };


    GameObject _leftCube;
    GameObject _centerCube;
    GameObject _rightCube;
    void GetCube()
    {
        _leftCube = taskManager.LeftCube;
        _centerCube = taskManager.CenterCube;
        _rightCube = taskManager.RightCube;
    }
    public override void StartTask(float time)
    {
        int taskNum = TASK_NUM - 1;//配列用
        if (taskResultRecoder.StartTime[taskNum] > 0)
        {
            return; //すでに開始している場合は無視
        }

        //全てのレーザーの色を白にする
        laserColorResetter.Rpc_ResetLasersColor(Color.white);

        GetCube();
        // float taskStartTime = _time;//タスク開始時間を記録
        taskResultRecoder.RecordStartTime(taskNum, time);


        _leftCube.transform.position = targetCubePos[0];
        _centerCube.transform.position = targetCubePos[1];
        _rightCube.transform.position = targetCubePos[2];

        _leftCube.GetComponent<TaskCube>().VisibleCube(true);
        _centerCube.GetComponent<TaskCube>().VisibleCube(true);
        _rightCube.GetComponent<TaskCube>().VisibleCube(true);

        Debug.Log("Task " + TASK_NUM + " Started!");
        taskUIManager.SetTaskText("Task " + TASK_NUM + "\nHit all the cubes with the laser!");
    }

    /// <summary>
    /// タスクUIに表示するカラータスク用テキスト
    /// </summary>
    string _leftText, _centerText, _rightText;
    public override void DoTask(float time)
    {
        if (taskResultRecoder.StartTime[TASK_NUM] < 0) return;

        //Task3完了判定
        if (endRequirement())
        {
            EndTask(time);
        }
        else
        {
            taskUIManager.SetTaskText("Task " + TASK_NUM + "\n" +
            _leftText + _centerText + _rightText);
        }

        //ヒットしたらキューブを非表示にする
        _leftCube.GetComponent<TaskCube>().VisibleCube(!_isLeftHit);
        _centerCube.GetComponent<TaskCube>().VisibleCube(!_isCenterHit);
        _rightCube.GetComponent<TaskCube>().VisibleCube(!_isRightHit);

        //ｔTaskTextの更新
        if (_isLeftColorChanged)
        {
            _leftText = "";
        }
        else
        {
            _leftText = $"Left to <color=#{ColorUtility.ToHtmlStringRGB(targetColors[0].colorValue)}> {targetColors[0].colorName}</color>\n";
        }
        if (_isCenterColorChanged)
        {
            _centerText = "";
        }
        else
        {
            _centerText = $"Center to <color=#{ColorUtility.ToHtmlStringRGB(targetColors[1].colorValue)}> {targetColors[1].colorName}</color>\n";
        }
        if (_isRightColorChanged)
        {
            _rightText = "";
        }
        else
        {
            _rightText = $"Right to <color=#{ColorUtility.ToHtmlStringRGB(targetColors[2].colorValue)}> {targetColors[2].colorName}</color>\n";
        }
    }

    protected override void EndTask(float time)
    {
        int taskNum = TASK_NUM - 1;//配列用
        if (taskResultRecoder.EndTime[taskNum] > 0)
        {
            return; //すでに終了している場合は無視
        }

        taskRpcSender.Rpc_EndTask(TASK_NUM);
        taskUIManager.SetTaskText("Task " + TASK_NUM + "\nCompleted!");
    }

    bool _isLeftColorChanged, _isCenterColorChanged, _isRightColorChanged = false;
    bool _isLeftHit, _isCenterHit, _isRightHit = false;
    bool _endHitTask, _endColorTask = false;
    protected override bool endRequirement()
    {
        //ヒットタスク
        _isLeftHit = taskCubeHitChecker.CheckHit(taskManager.LeftSphereMesh.transform.position, EffectLists.EffectType.leftLaser);
        _isCenterHit = taskCubeHitChecker.CheckHit(taskManager.CenterSphereMesh.transform.position, EffectLists.EffectType.centerLaser);
        _isRightHit = taskCubeHitChecker.CheckHit(taskManager.RightSphereMesh.transform.position, EffectLists.EffectType.rightLaser);

        if (_isLeftHit && _isCenterHit && _isRightHit)
        {   //ヒットタスク完了
            _endHitTask = true;
        }

        //カラータスク
        _isLeftColorChanged = colorChecker.CheckLaserColor(EffectLists.EffectType.leftLaser, targetColors[0].colorValue);
        _isCenterColorChanged = colorChecker.CheckLaserColor(EffectLists.EffectType.centerLaser, targetColors[1].colorValue);
        _isRightColorChanged = colorChecker.CheckLaserColor(EffectLists.EffectType.rightLaser, targetColors[2].colorValue);
        if (_isLeftColorChanged && _isCenterColorChanged && _isRightColorChanged)
        {
            _endColorTask = true;
        }

        //終了判定
        return _endHitTask && _endColorTask;
    }
}
