using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class RoadHipspos : MonoBehaviour
{
    public DanceData danceData;
    List<Vector3> _hipsPosList = new List<Vector3>();
    string _filePath = Application.dataPath + "/TaskResult/";
    [SerializeField] string _fileName;
    void Awake()
    {
        _filePath += _fileName + ".txt";
    }

    public List<Vector3> GetHipPoses()
    {
        _hipsPosList = LoadHipsPosDataByTxt(_filePath);
        // CopyDanceDataToScriptableObject(_hipsPosList, danceData);

        return _hipsPosList;
    }

    void CopyDanceDataToScriptableObject(List<Vector3> positions, DanceData danceData_SO)
    {
        if (danceData_SO == null)
        {
            Debug.LogError("DanceData ScriptableObject is not assigned.");
            return;
        }

        danceData_SO.dancePositions = new List<Vector3>(positions);
        Debug.Log("Copied " + positions.Count + " positions to DanceData ScriptableObject.");
    }


    /// <summary>
    /// ファイルから座標データを読み込む
    /// </summary>
    List<Vector3> LoadHipsPosDataByTxt(string filePath)
    {
        List<Vector3> positionData = new List<Vector3>();

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return positionData;
        }
        try
        {
            StreamReader sr = new StreamReader(filePath);
            string line = sr.ReadLine(); // ヘッダー行をスキップ

            while ((line = sr.ReadLine()) != null)
            {
                // 空行をスキップ
                if (string.IsNullOrEmpty(line))
                    continue;

                // カンマで分割
                string[] values = line.Split(',');

                if (values.Length == 3)
                {
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);
                    float z = float.Parse(values[2]);

                    positionData.Add(new Vector3(x, y, z));
                }
            }

            sr.Close();
            Debug.Log("Loaded " + positionData.Count + " position data points");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error reading file: " + e.Message);
        }

        return positionData;
    }
}
