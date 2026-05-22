using UnityEngine;
using System.Collections.Generic;
using Fusion;
public class LaserBandChanger : NetworkBehaviour
{
    [SerializeField] SyncLaser syncLaser;
    public enum AudioLinkBandType
    {
        Bass = 0,
        LowMid = 1,
        HighMid = 2,
        Treble = 3
    }

    public override void Spawned()
    {
        InitBand();//Laserの反応音域を初期化,UIは削除
    }

    /// <summary>
    /// レーザーの反応音域を初期化する
    /// </summary>
    void InitBand()
    {
        Rpc_ChangeBandtype_EffectType(EffectLists.EffectType.leftLaser, AudioLinkBandType.HighMid);
        Rpc_ChangeBandtype_EffectType(EffectLists.EffectType.centerLaser, AudioLinkBandType.Treble);
        Rpc_ChangeBandtype_EffectType(EffectLists.EffectType.rightLaser, AudioLinkBandType.HighMid);
        Debug.Log("InitBand called");
    }

    /// <summary>
    /// マテリアルのAudioLinkTypeプロパティを変更する
    /// </summary>
    /// <param name="objList"></param>
    /// <param name="bandType"></param>
    void ChangeBandtype_MateriaPropaty(List<NetworkObject> objList, AudioLinkBandType bandType)
    {
        syncLaser.BandType = (SyncLaser.Band)bandType;
        for (int i = 0; i < objList.Count; i++)
        {
            Material mat = objList[i].GetComponent<Renderer>().material;
            mat.SetFloat("_AudioLinkType", (int)bandType);
        }
    }


    /// <summary>
    /// UIから呼び出す用
    /// </summary>
    // public void OnChangeBandtype(EffectAddList.EffectType effectType, AudioLinkBandType bandType)
    // {

    // }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_ChangeBandtype_EffectType(EffectLists.EffectType effectType, AudioLinkBandType bandType)
    {
        switch (effectType)
        {
            case EffectLists.EffectType.leftLaser:
                ChangeBandtype_MateriaPropaty(EffectLists.Instance.LeftLaserObjectList, bandType);
                syncLaser.LeftBand = (SyncLaser.Band)bandType;
                break;
            case EffectLists.EffectType.centerLaser:
                ChangeBandtype_MateriaPropaty(EffectLists.Instance.CenterLaserObjectList, bandType);
                syncLaser.CenterBand = (SyncLaser.Band)bandType;
                break;
            case EffectLists.EffectType.rightLaser:
                ChangeBandtype_MateriaPropaty(EffectLists.Instance.RightLaserObjectList, bandType);
                syncLaser.RightBand = (SyncLaser.Band)bandType;
                break;

            default:
                break;
        }
    }
}
