using UnityEngine;
using Fusion;
using UnityEngine.UI;
public class RightLaserSpawner : EffectSpawner
{
    [Header("References")]
    [SerializeField] LaserManager laserManager;
    [Header("Other Objects")]
    [SerializeField] GameObject laserTarget;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        LasersLookAtTarget();
    }

    Vector3 _target = Vector3.zero;
    void LasersLookAtTarget()
    {
        if (_target == laserTarget.transform.position) return;
        _target = laserTarget.transform.position;
        var laserList = EffectLists.Instance.RightLaserObjectList;
        for (int i = 0; i < laserList.Count; i++)
        {
            laserList[i].transform.LookAt(_target);
        }
    }
}