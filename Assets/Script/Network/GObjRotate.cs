using Fusion;
using UnityEngine;

/// <summary>
/// Renderでオブジェクトを回転させ通信を確認するためのクラス
/// </summary>
public class GObjRotate : NetworkBehaviour
{
    [SerializeField] StartGameInfo startGameInfo;
    [SerializeField] float _rotateSpeed = 10f;
    GameObject _targetObj;
    Vector3 _mqPos = new Vector3(-0.5f, 1.2f, 0.5f);
    Vector3 _pcPos = new Vector3(-0.15f, -0.38f, -9.02f);
    void Start()
    {
        _targetObj = gameObject;
        if (startGameInfo.isMetaQuest)
        {
            _targetObj.transform.position = _mqPos;
        }
        else
        {
            _targetObj.transform.position = _pcPos;
        }
    }

    public override void Render()
    {
        if (_targetObj == null) return;

        _targetObj.transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
    }
}