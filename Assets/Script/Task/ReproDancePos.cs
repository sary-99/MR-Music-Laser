using UnityEngine;
using System.Collections.Generic;
public class ReproDancePos : MonoBehaviour
{
    public GameObject moveObjectPrefab;
    GameObject _moveObject;
    public RoadHipspos roadHipspos;
    public DanceData danceData;
    [SerializeField] float _frameInterval = 0.5f; // 記録間隔
    public float FrameInterval => _frameInterval;
    [SerializeField] Vector3 _initPos = new Vector3(0, 0, 0);//ダンス開始位置
    // List<Vector3> _positions = new List<Vector3>();
    bool _isMoving = false;
    public bool IsMoving { get { return _isMoving; } }
    float _totalTime = 0f;//経過時間
    int _moveIndex = 0;//指定時間毎の移動インデックス

    public void StartMove(GameObject moveObj = null, Vector3? initPos = null)
    {
        GameObject obj;
        if (moveObj == null)
        {
            obj = Instantiate(moveObjectPrefab, _initPos, Quaternion.identity);
        }
        else
        {
            obj = moveObj;
        }

        GetPositionListData();
        Debug.Log("positions count: " + danceData.dancePositions.Count);
        _isMoving = true;
        _moveObject = obj;
        if (initPos != null)
        {
            _initPos = (Vector3)initPos;
        }
    }
    public List<Vector3> GetPositionListData()
    {
        if (danceData == null)
        {
            Debug.LogError("DanceData ScriptableObject is not assigned.");
            return default;
        }
        else if (danceData.dancePositions.Count == 0)
        {
            //ScriptableObjectにデータが無い場合、ファイルから読み込む
            danceData.dancePositions = roadHipspos.GetHipPoses();
        }

        //ScriptableObjectにデータがある場合
        return danceData.dancePositions;
    }

    #region Move Object
    public void UpdateObj()
    {
        if (_isMoving == false) return;
        if (danceData.dancePositions.Count == 0) return;
        _totalTime += Time.deltaTime;

        _moveIndex = UpdateMoveIndex(_moveIndex);
        MoveObj(_moveObject.transform);
    }
    int UpdateMoveIndex(int index)
    {
        if (index >= danceData.dancePositions.Count)
        {//indexが範囲外の場合
            return index;
        }
        index = (int)(_totalTime / _frameInterval);//index更新の試行
        Debug.Log("moveIndex: " + index);

        return index;
    }
    void MoveObj(Transform chaserTrs)
    {
        if (_isMoving == false) return;

        //停止判定
        if (_moveIndex > danceData.dancePositions.Count)
        {
            _isMoving = false;
            Debug.Log("ReproDancePos: Movement Ended.\nEnd of Positions Reached.");
        }
        chaserTrs.position = UpdateChaserPos(chaserTrs.position);

        // if (danceData.dancePositions[_moveIndex - 1] == danceData.dancePositions[_moveIndex])
        // {
        //     _isMoving = false;
        //     Debug.Log("ReproDancePos: Movement Ended.\nSame Position Reached.");
        // }

    }
    Vector3 UpdateChaserPos(Vector3 chaserPos)
    {
        float t = _totalTime - (_moveIndex * _frameInterval);//現座標から目標座標までの補間用時間
        Vector3 pos = Vector3.Lerp(
            chaserPos,
            danceData.dancePositions[_moveIndex],
            t
        ); //(現座標,目標座標,進行度(%))      
        return _initPos + pos;
    }


    #endregion

}
