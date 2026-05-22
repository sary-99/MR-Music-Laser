using UnityEngine;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Fusion;

/// <summary>
/// レーザーの生成,表示状態を管理するクラス
/// </summary>
public class LaserManager : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] LeftLaserSpawner leftLaserSpawner;
    [SerializeField] CenterLaserSpawner centerLaserSpawner;
    [SerializeField] RightLaserSpawner rightLaserSpawner;
    [SerializeField] EffectMeshManager effectMeshManager;
    [SerializeField] TargetSphereController targetSphereController;
    [SerializeField] EffectLists effectLists;

    [Header("Properties")]
    [SerializeField] float _edgeLengthThreshold = 1f;//レーザーを配置する辺の長さの閾値
    public enum LaserPosType
    {
        left = EffectLists.EffectType.leftLaser,
        center = EffectLists.EffectType.centerLaser,
        right = EffectLists.EffectType.rightLaser
    }

    /// <summary>
    /// "AllLaser"ボタンが押されたときHost側で実行される
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_OnSetAllLasers()
    {
        Debug.Log("LaserManager Rpc_OnSetAllLasers");
        if (!HasStateAuthority) return;
        Rpc_OnSetEffect(LaserPosType.left);
        Rpc_OnSetEffect(LaserPosType.center);
        Rpc_OnSetEffect(LaserPosType.right);
    }

    /// <summary>
    /// SpwanerのRpc_OnSetEffectを呼び出す
    /// </summary>
    /// <param name="laserType"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_OnSetEffect(LaserPosType laserType)
    {
        Debug.Log("LaserManager Rpc_OnSetEffect laserType:" + laserType);
        if (!HasStateAuthority) return;
        switch (laserType)
        {
            case LaserPosType.left:
                if (effectLists.LeftLaserObjectList.Count <= 0)//まだレーザーがセットされていないなら生成
                {//Effectが生成されていなければSpwan
                    SpawnLasers(LaserPosType.left);
                }
                else
                {//Effectが生成されて入れば表示状態を切り替える
                    leftLaserSpawner.ToggleVisibility();
                }
                break;
            case LaserPosType.center:
                if (effectLists.CenterLaserObjectList.Count <= 0)//まだレーザーがセットされていないなら生成
                {
                    SpawnLasers(LaserPosType.center);
                }
                else
                {
                    centerLaserSpawner.ToggleVisibility();
                }
                break;
            case LaserPosType.right:
                if (effectLists.RightLaserObjectList.Count <= 0)//まだレーザーがセットされていないなら生成
                {
                    SpawnLasers(LaserPosType.right);
                }
                else
                {
                    rightLaserSpawner.ToggleVisibility();
                }
                break;
            default:
                Debug.LogError("Invalid laser type: " + laserType);
                break;
        }
    }

    /// <summary>
    /// 位置や回転を求めてレーザーを生成する
    /// </summary>
    /// <param name="laserPosType"></param>
    public void SpawnLasers(LaserPosType laserPosType)
    {
        List<Vector3> leftEdgeList = GetEdges(LaserPosType.left);
        List<Vector3> centerEdgeList = GetEdges(LaserPosType.center);
        List<Vector3> rightEdgeList = GetEdges(LaserPosType.right);
        if (leftEdgeList == null || centerEdgeList == null || rightEdgeList == null)
        {
            Debug.LogError("No valid edges found for one or more laser spawning positions.");
            return;
        }

        for (int i = 0; i < leftEdgeList.Count; i++)
        {
            Vector3 leftEdge = leftEdgeList[i];
            Vector3 centerEdge = centerEdgeList[i];
            Vector3 rightEdge = rightEdgeList[i];

            float edgeLength = (leftEdge - rightEdge).magnitude;//辺の長さ

            Vector3 aim;
            Quaternion rot;
            Fusion.NetworkObject laser = default;
            switch (laserPosType)
            {
                case LaserPosType.left:
                    aim = targetSphereController.LeftTarget.transform.position - leftEdge;
                    rot = Quaternion.LookRotation(aim);
                    laser = leftLaserSpawner.SpawnEffects(leftEdge, rot);
                    break;
                case LaserPosType.center:
                    aim = targetSphereController.CenterTarget.transform.position - centerEdge;
                    rot = Quaternion.LookRotation(aim);
                    laser = centerLaserSpawner.SpawnEffects(centerEdge, rot);
                    break;
                case LaserPosType.right:
                    aim = targetSphereController.RightTarget.transform.position - rightEdge;
                    rot = Quaternion.LookRotation(aim);
                    laser = rightLaserSpawner.SpawnEffects(rightEdge, rot);
                    break;
            }

            //辺の長さに合わせてスケールを変更
            Vector3 scale = new Vector3(
                edgeLength,
                laser.transform.localScale.y,
                laser.transform.localScale.z
                );

            laser.transform.localScale = scale;

            Debug.Log("LaserManager SpawnLasers()", this);
        }
    }

    /// <summary>
    /// 天井の頂点を取得し、レーザーの配置位置を返す
    /// </summary>
    public List<Vector3> GetEdges(LaserPosType laserPos)
    {
        if (effectMeshManager == null)
        {
            Debug.LogError("effectMeshManager is null");
            return null;
        }
        List<Vector3> edgePos = new List<Vector3>();
        EffectMeshManager.RoomSurface ceiling = effectMeshManager.FindRoomSurface(MRUKAnchor.SceneLabels.CEILING);//天井
        Debug.Log("Ceiling Edge Count:" + ceiling.edgeList.Count);

        Vector3 leftPos;
        Vector3 centerPos;
        Vector3 rightPos;
        for (int i = 0; i < ceiling.edgeList.Count; i++)
        {
            //辺の長さを求める
            var edge = ceiling.edgeList[i]; // (Vector3, Vector3)頂点と頂点を結んだ辺
            Vector3 edgeVec = edge.Item2 - edge.Item1;
            float edgeLength = edgeVec.magnitude;

            //辺の中点にlaserを配置
            // Debug.Log("c:" + edgeLength);
            if (edgeLength < _edgeLengthThreshold)
            {
                Debug.Log("ceiling line too short, skipping." + edgeLength);
                continue;//短すぎる辺は無視
            }

            centerPos = (edge.Item1 + edge.Item2) / 2; //辺の中点

            switch (laserPos)
            {
                case LaserPosType.left:
                    leftPos = (edge.Item2 + centerPos) / 2;
                    edgePos.Add(leftPos);
                    Debug.Log("leftPos:" + leftPos);
                    break;
                case LaserPosType.center:
                    edgePos.Add(centerPos);
                    Debug.Log("centerPos:" + centerPos);
                    break;
                case LaserPosType.right:
                    rightPos = (edge.Item1 + centerPos) / 2;
                    edgePos.Add(rightPos);
                    Debug.Log("rightPos:" + rightPos);
                    break;
            }
        }

        Debug.Log(laserPos + "GetEdges Count:" + edgePos.Count);
        return edgePos;
    }
}
