using UnityEngine;

[CreateAssetMenu(fileName = "TaskResult", menuName = "ScriptableObject/TaskResult", order = 1)]
/// <summary>
/// タスクの開始時間、終了時間、所要時間を記録するScriptableObject
/// Editor上でタスクの結果を確認できるようにするために作成
/// </summary>s
public class TaskResult_ScriptableObject : ScriptableObject
{
    public float[] taskStartTime = new float[4];
    public float[] taskEndTime = new float[4];
    public float[] taskDurationTime => new float[4] {
        taskEndTime[0] - taskStartTime[0],
        taskEndTime[1] - taskStartTime[1],
        taskEndTime[2] - taskStartTime[2],
        taskEndTime[3] - taskStartTime[3]
    };
}
