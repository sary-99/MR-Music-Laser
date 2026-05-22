using UnityEngine;
using System.Collections.Generic;
public class ReproMotion : MonoBehaviour
{
    GameObject _cube;
    public RoadHipspos roadHipspos;
    public DanceData danceData;
    public GameObject danceCubePrefab;
    [SerializeField] float _frameInterval = 0.5f; // 記録間隔
    [SerializeField] Vector3 _initPos = new Vector3(0, 0, 0);//ダンス開始位置
    List<Vector3> _positions = new List<Vector3>();
    bool _isMoving = false;
    float _totalTime = 0f;
    int _moveIndex = 0;


    void Start()
    {
        // StartMove();
    }
    void StartMove()
    {
        _isMoving = true;
        _positions = GetPositions();
        _cube = Instantiate(danceCubePrefab, _initPos, Quaternion.identity);
    }
    void Update()
    {
        if (_isMoving == false) return;
        if (_positions.Count == 0) return;

        _cube.transform.position = CalcPos(_cube.transform.position);
    }
    public List<Vector3> GetPositions()
    {
        if (danceData == null || danceData.dancePositions.Count == 0)
        {
            //ScriptableObjectにデータが無い場合、ファイルから読み込む
            _positions = roadHipspos.GetHipPoses();
        }
        else
        {
            //ScriptableObjectにデータがある場合
            _positions = danceData.dancePositions;
        }
        return _positions;
    }
    Vector3 CalcPos(Vector3 nowPos)
    {
        if (_moveIndex >= _positions.Count)
        {
            return nowPos;
        }
        _totalTime += Time.deltaTime;
        _moveIndex = (int)(_totalTime / _frameInterval);

        float t = _totalTime - (_moveIndex * _frameInterval);//開始座標から目標座標までの補間用時間
        Vector3 pos = Vector3.Lerp(nowPos, _positions[_moveIndex], t);        //(開始座標,目標座標,進行度(%))

        return _initPos + pos;
    }



}
