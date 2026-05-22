using Fusion;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 生成したEffectのリストを管理する
/// Effectの親オブジェクトも管理する
/// </summary>
public class EffectLists : MonoBehaviour
{
    public static EffectLists Instance { get; private set; }

    [Header("Effect Parent")]
    NetworkObject _leftLaserParent;
    NetworkObject _centerLaserParent;
    NetworkObject _rightLaserParent;

    [Header("Effect Object Lists")]
    List<NetworkObject> _centerLaserObjectList = new List<NetworkObject>();
    List<NetworkObject> _leftLaserObjectList = new List<NetworkObject>();
    List<NetworkObject> _rightLaserObjectList = new List<NetworkObject>();

    [Header("List getter")]
    public List<NetworkObject> CenterLaserObjectList => _centerLaserObjectList;
    public List<NetworkObject> LeftLaserObjectList => _leftLaserObjectList;
    public List<NetworkObject> RightLaserObjectList => _rightLaserObjectList;

    public enum EffectType
    {
        centerLaser,
        leftLaser,
        rightLaser
    }

    void Awake()
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
    /// エフェクトがSpawnされたときにエフェクトをリストに追加し、親子関係を設定する関数
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="type"></param>
    public void EffectToList(NetworkObject effect, EffectType type)
    {
        switch (type)
        {
            case EffectType.centerLaser:
                _AddList(effect, _centerLaserParent, _centerLaserObjectList);
                break;
            case EffectType.leftLaser:
                _AddList(effect, _leftLaserParent, _leftLaserObjectList);
                break;
            case EffectType.rightLaser:
                _AddList(effect, _rightLaserParent, _rightLaserObjectList);
                break;
            default:
                break;
        }

        /// <summary>
        /// エフェクトを指定のリストに追加し、親子関係を設定する
        /// </summary>
        void _AddList(NetworkObject effect, NetworkObject parent, List<NetworkObject> list)
        {
            list.Add(effect);
            if (parent == null)
            {
                Debug.LogError($"EffectToList: Parent is null for effect {effect.name} of type {type}");
            }
            else
            {
                effect.transform.parent = parent.transform;
            }
        }
        string debugMsg = $"[EffectAddList] EffectToList called name={effect.name}";
        debugMsg += $"\nparent={(effect.transform.parent ? effect.transform.parent.name : "null")}";
        Debug.Log(debugMsg, this);
        DebugTxtManager.Instance.DebugTxtPlusR(debugMsg);
    }

    /// <summary>
    /// エフェクトの親オブジェクトを設定する
    /// </summary>
    /// <param name="type">Effectの種類</param>
    /// <param name="obj">親オブジェクト</param>
    public void SetEffectParentObj(EffectType type, NetworkObject obj)
    {
        switch (type)
        {
            case EffectType.centerLaser:
                _centerLaserParent = obj;
                break;
            case EffectType.leftLaser:
                _leftLaserParent = obj;
                break;
            case EffectType.rightLaser:
                _rightLaserParent = obj;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Effectの親オブジェクトを返す
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public NetworkObject GetParentObj(EffectType type)
    {
        switch (type)
        {
            case EffectType.centerLaser:
                return _centerLaserParent;
            case EffectType.leftLaser:
                return _leftLaserParent;
            case EffectType.rightLaser:
                return _rightLaserParent;
            default:
                Debug.LogError("EffectLists GetParentObj: Invalid EffectType " + type);
                return null;
        }
    }
}

