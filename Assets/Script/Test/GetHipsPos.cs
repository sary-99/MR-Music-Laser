using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetHipsPos : MonoBehaviour
{
    public GameObject hips;
    float _timer = 0f;
    [SerializeField] float _recordDuration = 0.5f;
    List<Vector3> _hipsPosList = new List<Vector3>();
    [Header("File Info")]
    string _fileName = "";
    string _filePath = Application.dataPath;

    void Awake()
    {
        string day = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        _fileName = "HipsDistance_" + day + ".txt";
        _filePath += "/TaskResult/" + _fileName;
        Debug.Log("File Path: " + _filePath, this);
    }
    void Update()
    {
        // RecordHipsPos();
    }

    void RecordHipsPos()
    {
        _timer += Time.deltaTime;
        if (_timer >= _recordDuration)
        {
            Vector3 hipsPos = hips.transform.position;
            Debug.Log("Hips Position: " + hipsPos.ToString("F3"));
            _hipsPosList.Add(hipsPos);
            _timer = 0f;
        }
    }
    void WriteHipsPosData()
    {
        StreamWriter sw = new StreamWriter(_filePath, true);
        sw.WriteLine("Hips Position Data");
        for (int i = 0; i < _hipsPosList.Count; i++)
        {
            Vector3 pos = _hipsPosList[i];
            string posStr = string.Format("{0},{1},{2}", pos.x.ToString("F4"), pos.y.ToString("F4"), pos.z.ToString("F4"));
            sw.WriteLine(posStr);
        }
        sw.Flush();
        sw.Close();
        Debug.Log("Hips Position Data Written to " + _filePath, this);
    }

    void OnApplicationQuit()
    {
        // WriteHipsPosData();
    }


}
