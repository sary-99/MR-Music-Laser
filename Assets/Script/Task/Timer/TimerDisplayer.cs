using UnityEngine;
using TMPro;

/// <summary>
/// UI操作を受け付け、タイマーを操作するクラス
/// </summary>
public class TimeDisplayer : MonoBehaviour
{
    // [SerializeField] TimerOperater timerOperater;
    Timer timer => Timer.Instance;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] StartGameInfo startGameInfo;
    public void StartTimer()
    {
        timer.StartTimer();
        Debug.Log("StartTimer called");
    }

    public void PauseTimer()
    {
        timer.PauseTimer();
        Debug.Log("PauseTimer called");
    }

    public void ResetTimer()
    {
        timer.ResetTimer();
        Debug.Log("ResetTimer called");
    }

    void DisplayTime()
    {
        float currentTime = Timer.Instance.GetTime();
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000f) % 1000f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    void Update()
    {
        if (startGameInfo.isPc == false) return;
        DisplayTime();
    }

}