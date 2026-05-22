using UnityEngine;

/// <summary>
/// タスクを完了する時間を計測するためのクラス
/// PCからタスク開始指示->Hostでタスク実行&タイマースタート->Hostでタスク完了&タイマーストップ
/// タイマーは記録用PCで動かす。
/// </summary>
public class Timer : MonoBehaviour
{
    [Header("Properties")]
    public static Timer Instance { get; private set; }
    bool _isRunning = false; // タイマーが動いているかどうか
    float _time = 0; //経過時間

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Start the timer 
    /// </summary>
    public void StartTimer()
    {
        _isRunning = true;
    }

    /// <summary>
    /// Pause the timer
    /// </summary>
    public void PauseTimer()
    {
        _isRunning = false;
    }

    /// <summary>
    /// Reset the timer
    /// Do not Stop
    /// </summary>
    public void ResetTimer()
    {
        _time = 0;
    }

    /// <summary>
    /// Stop the timer and reset time to 0
    /// </summary>
    public void StopTimer()
    {
        _isRunning = false;
        _time = 0;
    }

    /// <summary>
    /// タイマースタートからの経過時間を取得する
    /// </summary>
    public float GetTime()
    {
        return _time;
    }

    void CountTimer()
    {
        if (!_isRunning) return;
        _time += Time.deltaTime;
    }
    void Update()
    {
        CountTimer();
    }

}
