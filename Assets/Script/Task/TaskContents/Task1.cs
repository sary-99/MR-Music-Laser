using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class Task1 : TaskContentMq
{
    [SerializeField]
    Colors[] targetColors ={
        new Colors { colorName = "Red", colorValue = Color.red },//(left)
        new Colors { colorName = "Green", colorValue = Color.green },//(center)
        new Colors { colorName = "Blue", colorValue = Color.blue }//(right)
    };

    [Header("References")]
    [SerializeField] EffectLists effectLists;
    [SerializeField] LaserColorChecker colorChecker;
    [SerializeField] LaserColorManager laserColorManager;
    [SerializeField] LaserMatUIManager laserMatUIManager;
    [SerializeField] LaserColorResetter laserColorResetter;//タスク開始時、全レーザーを白くするために使用

    [Header("Properties")]
    const int TASK_NUM = 2;

    [System.Serializable]
    public struct Colors
    {
        public string colorName;
        public Color colorValue;
    }

    string _leftText, _centerText, _rightText;
    void SetTaskText()
    {
        _leftText = $"LEFT to <color=#{ColorUtility.ToHtmlStringRGB(targetColors[0].colorValue)}> {targetColors[0].colorName}</color>\n";
        _centerText = $"CENTER to <color=#{ColorUtility.ToHtmlStringRGB(targetColors[1].colorValue)}> {targetColors[1].colorName}</color>\n";
        _rightText = $"RIGHT to <color=#{ColorUtility.ToHtmlStringRGB(targetColors[2].colorValue)}> {targetColors[2].colorName}</color>\n";
        Debug.Log(_leftText + _centerText + _rightText);
        Debug.Log($"ColorUtility.ToHtmlStringRGB(0): {ColorUtility.ToHtmlStringRGB(targetColors[0].colorValue)}:{targetColors[0].colorName}");
        Debug.Log($"ColorUtility.ToHtmlStringRGB(1): {ColorUtility.ToHtmlStringRGB(targetColors[1].colorValue)}:{targetColors[1].colorName}");
        Debug.Log($"ColorUtility.ToHtmlStringRGB(2): {ColorUtility.ToHtmlStringRGB(targetColors[2].colorValue)}:{targetColors[2].colorName}");
    }
    public override void StartTask(float time)
    {
        int left = EffectLists.Instance.LeftLaserObjectList.Count();
        int center = EffectLists.Instance.CenterLaserObjectList.Count();
        int right = EffectLists.Instance.RightLaserObjectList.Count();
        if (left <= 0 || center <= 0 || right <= 0)
        {
            Debug.LogError("Lasers not found!");
            return;
        }
        int taskNum = TASK_NUM - 1;//0始まりのため-1,配列用
        // Getlasers();
        if (taskResultRecoder.StartTime[taskNum] > 0)
        {
            return; //すでに開始している場合は無視
        }

        taskResultRecoder.RecordStartTime(taskNum, time);
        SetTaskText();//タスクUIに表示するテキスト設定

        //全てのレーザーの色を白にする
        laserColorResetter.Rpc_ResetLasersColor(Color.white);

        Debug.Log("Task " + TASK_NUM + " Started!");
        taskUIManager.SetTaskText("Task " + TASK_NUM + "\nChange the colors of the lasers!");
    }

    protected override void EndTask(float time)
    {
        int taskNum = TASK_NUM - 1;//0始まりのため-1
        if (taskResultRecoder.EndTime[taskNum] > 0)
        {
            return; //すでに終了している場合は無視
        }

        taskRpcSender.Rpc_EndTask(TASK_NUM);
        taskUIManager.SetTaskText("Task " + TASK_NUM + "\nCompleted!");
    }
    /// <summary>
    /// レーザーの色が指定の色と一致したか
    /// </summary>
    bool _isLeftColorChanged = false, _isCenterColorChanged = false, _isRightColorChanged = false;
    public override void DoTask(float time)
    {
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

        //終了判定
        if (endRequirement())
        {
            EndTask(time);
        }
        else
        {
            string txt = _leftText + _centerText + _rightText;
            taskUIManager.SetTaskText(txt);
        }
    }

    protected override bool endRequirement()
    {
        _isLeftColorChanged = colorChecker.CheckLaserColor(EffectLists.EffectType.leftLaser, targetColors[0].colorValue);
        _isCenterColorChanged = colorChecker.CheckLaserColor(EffectLists.EffectType.centerLaser, targetColors[1].colorValue);
        _isRightColorChanged = colorChecker.CheckLaserColor(EffectLists.EffectType.rightLaser, targetColors[2].colorValue);

        return _isLeftColorChanged && _isCenterColorChanged && _isRightColorChanged;
    }
}
