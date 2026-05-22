using UnityEngine;
using Fusion;
using UnityEngine.UI;

/// <summary>
/// Centerlaserを生成し、その他のレーザー生成クラスも管理するクラス
/// </summary>
public class CenterLaserSpawner : EffectSpawner
{
    [Header("References")]
    [SerializeField] LaserManager laserManager;
    [SerializeField] TargetSphereController targetSphereController;

    [Header("Other Objects")]
    [SerializeField] GameObject laserTarget;

    public override void Spawned()
    {
        base.Spawned();
        Debug.Log("LaserSpawner Spawned");
        if (!HasStateAuthority)
        {
            //Hostでなければターゲット球体を非表示にする
            targetSphereController.HideTargetSpheres();
            Debug.Log("Not Host: TargetSphereController is not visible");
        }
    }

    Vector3 _target = Vector3.zero;
    /// <summary>
    /// レーザーが常に球体の方を向くようにする
    /// </summary>
    void LasersLookAtTarget()
    {
        if (_target == laserTarget.transform.position) return;
        _target = laserTarget.transform.position;
        var laserList = EffectLists.Instance.CenterLaserObjectList;
        for (int i = 0; i < laserList.Count; i++)
        {
            laserList[i].transform.LookAt(_target);
        }
    }

    public override void FixedUpdateNetwork()
    {
        //FixedUpdateNetworkはHostでしか動かない
        //Cliant側は、NetworkTransformで位置や回転が同期される
        LasersLookAtTarget();
    }
}



