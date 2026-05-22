using UnityEngine;
using Fusion;

public class ParentObjSpawned : NetworkBehaviour
{
    [SerializeField] EffectLists.EffectType effectType;

    public override void Spawned()
    {
        base.Spawned();

        // 詳細ログ
        Debug.Log($"=== ParentObjSpawned Spawned ===");
        Debug.Log($"Name: {name}");
        Debug.Log($"EffectType: {effectType}");
        Debug.Log($"ChildCount: {transform.childCount}");

        var allBehaviours = GetComponents<NetworkBehaviour>();
        Debug.Log($"Direct NetworkBehaviours: {allBehaviours.Length}");

        var childBehaviours = GetComponentsInChildren<NetworkBehaviour>();
        Debug.Log($"All NetworkBehaviours (including children): {childBehaviours.Length}");

        foreach (var child in childBehaviours)
        {
            Debug.Log($"  → {child.GetType().Name} on {child.gameObject.name}");
        }
        //---ログここまで---

        var effectSpawner = GameObject.Find("EffectSpawners");
        if (effectSpawner == null)
        {
            Debug.LogError("ParentObjSpawned: EffectSpawner not found!");
        }
        // FindSpawner(effectSpawner);
        RegistParentObject();
        Debug.Log("ParentObjSpawned Spawned(): " + effectType);
    }

    /// <summary>
    /// 自身を親オブジェクトとして登録する
    /// </summary>
    void RegistParentObject()
    {
        EffectLists.Instance.SetEffectParentObj(effectType, Object);
    }
}