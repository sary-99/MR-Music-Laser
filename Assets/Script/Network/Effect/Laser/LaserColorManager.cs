using UnityEngine;
using Fusion;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// レーザーの色を変更するクラス
/// </summary>
public class LaserColorManager : NetworkBehaviour
{
    public enum LaserPositionType
    {
        All,
        Left,
        Center,
        Right
    }
    List<NetworkObject> _laserList = new List<NetworkObject>(); // 色を変更するレーザーのリスト
    public List<NetworkObject> LaserList
    {
        get { return _laserList; }
        set => _laserList = value;
    }

    /// <summary>
    /// レーザーの色を同期させる
    /// </summary>
    /// <param name="laserPosType"></param>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_AddLaserToList(LaserPositionType laserPosType)
    {
        var addList = GetLaserList(laserPosType);
        _laserList = _laserList.Union(addList).ToList();//重複を除いて追加
    }

    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_ClearLaserList()
    {
        _laserList.Clear();
    }

    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_ChangeLaserColor(Color color)
    {
        ChangeLaserColor_InList(_laserList, color);
        Debug.Log("Rpc_ChangeLaserColor: " + " " + color.ToString());
    }
    /// <summary>
    /// レーザーオブジェクトリスト内の全てのレーザーの色を変更するメソッド
    /// </summary>
    public void ChangeLaserColor_InList(List<NetworkObject> laserObjectList, Color color1, Color color2 = default)
    {
        if (color2 == default)
        {
            // color2が指定されていない場合、color1と同じ色にする
            color2 = color1;
        }
        foreach (var laserObj in laserObjectList)
        {
            ChangeLaserColor(laserObj, color1, color2);
        }
    }

    /// <summary>
    /// レーザーの色を変更するメソッド
    /// </summary>
    void ChangeLaserColor(NetworkObject laserObj, Color color1, Color color2 = default)
    {
        if (color2 == default)
        {
            // color2が指定されていない場合、color1と同じ色にする
            color2 = color1;
        }
        var mat = laserObj.GetComponent<Renderer>().material;
        if (mat != null)
        {
            mat.SetColor("_Color1", color1);
            mat.SetColor("_Color2", color2);
        }
    }


    /// <summary>
    /// レーザーの色をランダムに変更するメソッド(リスト内は全て同じ色)
    /// </summary>
    /// <param name="laserPosType"></param>
    // [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    // public void Rpc_RandomChange_LaserColorInList(LaserPositionType laserPosType)
    // {
    //     var laserList = GetLaserList(laserPosType);
    //     Color randomColor = new Color(Random.value, Random.value, Random.value);
    //     ChangeLaserColor_InList(laserList, randomColor);
    //     // foreach (var laser in laserList)
    //     // {
    //     //     // Color randomColor2 = new Color(Random.value, Random.value, Random.value);

    //     //     ChangeLaserColor(laser, randomColor);
    //     // }
    // }

    /// <summary>
    /// レーザーの色をランダムに変更するメソッド(一つずつ異なる色)
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_RandomChange_AllLaserColor()
    {

        foreach (var laser in _laserList)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            // Color randomColor2 = new Color(Random.value, Random.value, Random.value);

            ChangeLaserColor(laser, randomColor);
        }
    }

    /// <summary>
    /// レーザーの種類に応じたレーザーオブジェクトリストを取得する
    /// </summary>
    List<NetworkObject> GetLaserList(LaserPositionType laserPostype)
    {
        List<NetworkObject> laserList = null;
        switch (laserPostype)
        {
            case LaserPositionType.All:
                laserList = EffectLists.Instance.LeftLaserObjectList;
                laserList.AddRange(EffectLists.Instance.CenterLaserObjectList);
                laserList.AddRange(EffectLists.Instance.RightLaserObjectList);
                break;
            case LaserPositionType.Left:
                laserList = EffectLists.Instance.LeftLaserObjectList;
                break;
            case LaserPositionType.Center:
                laserList = EffectLists.Instance.CenterLaserObjectList;
                break;
            case LaserPositionType.Right:
                laserList = EffectLists.Instance.RightLaserObjectList;
                break;
            default:
                break;
        }
        return laserList;
    }



}
