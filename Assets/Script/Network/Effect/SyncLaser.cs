using UnityEngine;
using System.Collections.Generic;
using Fusion;
using TMPro;
public class SyncLaser : NetworkBehaviour
{
    [SerializeField] AudioLinkSync audioLinkSync;
    [SerializeField] EffectLists effectLists;
    [SerializeField] TextMeshProUGUI debugTextL;
    [SerializeField] StartGameInfo startGameInfo;

    [Header("Band Type")]
    Band _leftBand = Band.HighMid;
    Band _centerband = Band.Treble;
    Band _rightBand = Band.HighMid;
    public Band LeftBand
    {
        get { return _leftBand; }
        set { _leftBand = value; }
    }
    public Band CenterBand
    {
        get { return _centerband; }
        set { _centerband = value; }
    }
    public Band RightBand
    {
        get { return _rightBand; }
        set { _rightBand = value; }
    }
    public enum Band
    {
        Bass,
        LowMid,
        HighMid,
        Treble
    }

    Band _bandType = Band.Bass;
    public Band BandType
    {
        get { return _bandType; }
        set { _bandType = value; }
    }
    public override void FixedUpdateNetwork()
    {
        //        Debug.Log("LaserSync FixedUpdateNetwork");
        //Debug.Log("SyncLaser FixedUpdateNetwork bandType:" + (int)bandType);
        Rpc_LaserSync();
    }

    /// <summary>
    /// Client側でNetworked変数の値を各エフェクトマテリアルに反映
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_LaserSync()
    {

        Vector4 alVal = audioLinkSync.alpass_audioLink;
        debugTextL.text = "Rpc_LaserALSync alVal:" + alVal;
        float[] alpass = new float[4] { alVal.x, alVal.y, alVal.z, alVal.w };

        SyncLaserVal(effectLists.LeftLaserObjectList, alpass[(int)_leftBand]);
        SyncLaserVal(effectLists.CenterLaserObjectList, alpass[(int)_centerband]);
        SyncLaserVal(effectLists.RightLaserObjectList, alpass[(int)_rightBand]);

    }
    void SyncLaserVal(List<NetworkObject> laserEffectList, float val)
    {
        for (int i = 0; i < laserEffectList.Count; i++)
        {
            Material mat = laserEffectList[i].GetComponent<MeshRenderer>().material;
            if (!mat.HasProperty("_AudioLinkSync"))
            {
                Debug.LogWarning("Material does not have _AudioLinkSync property.");
                return;
            }
            mat.SetFloat("_AudioLinkSync", val);
        }
    }
}
