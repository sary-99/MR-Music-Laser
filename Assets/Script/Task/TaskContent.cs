using UnityEngine;

/// <summary>
/// タスク実行時MetaQuest3で動く処理を定義する抽象クラス
/// </summary>
public abstract class TaskContentMq : MonoBehaviour
{
    [SerializeField] protected TaskManager taskManager;
    [SerializeField] protected TaskResultRecoder taskResultRecoder;
    [SerializeField] protected TaskRpcSender taskRpcSender;
    [SerializeField] protected TaskUIManager taskUIManager;
    protected bool _isTaskStarted = false; // タスクが開始されたかどうか
    public bool IsTaskStarted => _isTaskStarted;

    /// <summary>
    /// タスク開始時の処理
    /// PCで実行される
    /// ここでRPCを呼び出しQuest3でタスクを実行する
    /// </summary>
    /// <param name="time">タスク開始時刻</param>
    public abstract void StartTask(float time);

    /// <summary>
    /// タスク実行中の処理
    /// Quest3で毎フレーム呼び出される
    /// </summary>
    /// <param name="time">タスク開始からの経過時間</param>
    public abstract void DoTask(float time);

    /// <summary>
    /// タスク終了時の処理
    /// Quest3で呼び出される
    /// ここでRPCを呼び出しPCに記録処理をさせる
    /// </summary>
    protected abstract void EndTask(float time);

    /// <summary>
    /// タスク終了の条件
    /// DoTask()内で毎フレーム呼ばれ、trueを返したらEndTask()が呼ばれる
    /// </summary>
    /// <returns></returns>
    protected abstract bool endRequirement();
}