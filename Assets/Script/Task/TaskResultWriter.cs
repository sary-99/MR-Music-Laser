using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TaskResultWriter : MonoBehaviour
{
    [SerializeField] StartGameInfo startGameInfo;
    [SerializeField] TaskResult_ScriptableObject taskResult_SO;
    string _fileName;
    string _filePath = "";
    void Awake()
    {
        string day = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        _fileName = "TaskResult_" + day + ".txt";
        _filePath = Application.dataPath + "/TaskResult/" + _fileName;
        Debug.Log("File Path: " + _filePath);
    }


    void CheckFile()
    {
        if (startGameInfo.isPc == false)
        {
            //PCでない場合は書き込みしない
            return;
        }
        if (File.Exists(_filePath))
        {
            Debug.Log("File already exists: " + _filePath);
        }
        else
        {
            Debug.Log("Creating new file: " + _filePath);
            // ファイルが存在しない場合、新規作成
            StreamWriter sw = new StreamWriter(_filePath, false);
            sw.WriteLine("Task Result Data");
            sw.Flush();
            sw.Close();
        }
    }

    /// <summary>
    /// タスク完遂の所要時間をファイルに書き込む
    /// </summary>
    /// <param name="taskNum"></param>
    public void WriteTaskDurationResult(int taskNum)
    {
        CheckFile();
        float startTime = taskResult_SO.taskStartTime[taskNum];
        float endTime = taskResult_SO.taskEndTime[taskNum];
        float durationTime = taskResult_SO.taskDurationTime[taskNum];
        if (durationTime <= 0f)
        {
            Debug.LogError("Duration time is zero or negative for task " + taskNum + ". Check start and end times.");
        }

        //00:00:00にフォーマット
        int minutes = Mathf.FloorToInt(durationTime / 60f);
        int seconds = Mathf.FloorToInt(durationTime % 60f);
        int milliseconds = Mathf.FloorToInt((durationTime * 1000f) % 1000f);
        string timeResult = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);

        string txt = "";
        txt += $"\ntask{taskNum + 1}  result\n";
        txt += " Start Time, End Time, Duration Time\n";
        txt += $"{startTime},{endTime},{timeResult}";

        StreamWriter sw = new StreamWriter(_filePath, true);// TextData.txtというファイルを新規で用意,第２引数がtrueだと末尾に追記
        sw.WriteLine(txt);// ファイルに書き出したあと改行
        sw.Flush();// StreamWriterのバッファに書き出し残しがないか確認
        sw.Close();// ファイルを閉じる

        Debug.Log("Task Result Written: " + txt);
    }

    /// <summary>
    /// CubeとSphereがどれだけ離れていないかを検証するタスクの結果を記録する
    /// </summary>
    /// <param name="taskNum">タスク番号</param>
    /// <param name="addTime">計測間隔</param>
    /// <param name="targetPos">ターゲットの位置リスト</param>
    /// <param name="chaserPos">追従オブジェクトの位置リスト</param>
    public void Write_DistanceTaskResult(int taskNum, float addTime, List<Vector3> targetPos, List<Vector3> chaserPos)
    {
        CheckFile();
        StreamWriter sw = new StreamWriter(_filePath, true);// TextData.txtというファイルを新規で用意,第２引数がtrueだと末尾に追記
        string txt = "Task " + taskNum + " Distance Results\n num, time(s), targetPos, chaserPos, distance(m)";
        sw.WriteLine(txt);// ファイルに書き出したあと改行

        // List<Vector3> distances = new List<Vector3>();

        for (int i = 0; i < targetPos.Count; i++)
        {
            float distance = Vector3.Distance(targetPos[i], chaserPos[i]);
            string targetPosStr = $"{targetPos[i].x},{targetPos[i].y},{targetPos[i].z}";
            string chaserPosStr = $"{chaserPos[i].x},{chaserPos[i].y},{chaserPos[i].z}";
            // 計測ごとの経過時間
            float t = i * addTime;
            //00:00:00にフォーマット
            int minutes = Mathf.FloorToInt(t / 60f);
            int seconds = Mathf.FloorToInt(t % 60f);
            int milliseconds = Mathf.FloorToInt((t * 1000f) % 1000f);
            string timeResult = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);

            sw.WriteLine($"{i}, {timeResult}, {targetPosStr},{chaserPosStr}, {distance}");
        }

        // float avgDistance = 0f;

        sw.Flush();// StreamWriterのバッファに書き出し残しがないか確認
        sw.Close();// ファイルを閉じる
    }
}
