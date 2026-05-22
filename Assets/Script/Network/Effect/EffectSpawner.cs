using Fusion;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkObject))]
public abstract class EffectSpawner : NetworkBehaviour
{
    [SerializeField] EffectLists.EffectType _effectType; //エフェクトの種類

    [Header("References")]
    [SerializeField] protected EffectMeshManager effectMeshManager;

    [Header("Other Objects")]
    [SerializeField] protected NetworkObject parentObjectPrefab;
    [SerializeField] protected GameObject effectPrefab;
    [SerializeField] protected Button visibleButton;

    [Header("parameters")]
    bool _isVisible = false;
    public bool IsVisible { get { return _isVisible; } }

    public override void Spawned()
    {
        base.Spawned();
        SpawnParentObj();
    }

    /// <summary>
    /// 起動時にエフェクトの親オブジェクトをSpwanする
    /// </summary>
    void SpawnParentObj()
    {
        if (!HasStateAuthority) return;
        if (parentObjectPrefab == null) return;
        Runner.Spawn(parentObjectPrefab);//親オブジェクトをスポーン
        // effectLists.SetEffectParentObj(_effectType, _parentObject);//EffectAddListに親オブジェクトをセットする
    }

    /// <summary>
    /// エフェクトをスポーンしリストに追加
    /// </summary>
    /// <param name="pos">スポーン位置</param>
    /// <param name="rot">スポーン時のrotaion</param>
    public NetworkObject SpawnEffects(Vector3 pos, Quaternion rot)
    {
        var effect = Runner.Spawn(effectPrefab, pos, rot, PlayerRef.None);
        // effectLists.EffectToList(effect, _effectType);//SpanwしたEffectをリストに追加する
        Rpc_VisibleEffect(true);//生成したときエフェクトを表示状態にする

        string debugMsg = $"[EffectSpawner] SpawnEffects called name={effect.name}";
        debugMsg += $" parent={(effect.transform.parent ? effect.transform.parent.name : "null")}";
        Debug.Log(debugMsg, this);

        return effect;
    }

    /// <summary>
    /// 非同期にエフェクトをスポーンしリストに追加
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <returns></returns>
    public /*async*/ NetworkObject SpawnEffectsAsync(int num, Vector3 pos, Quaternion rot)
    {
        var effect = Runner.SpawnAsync(effectPrefab, pos, rot, PlayerRef.None).Object;
        // effectLists.EffectToList(effect, _effectType);//SpanwしたEffectをリストに追加する
        Rpc_VisibleEffect(true);//生成したときエフェクトを表示状態にする

        string debugMsg = $"[EffectSpawner] SpawnEffectsAsync called name={effect.name}";
        debugMsg += $" parent={(effect.transform.parent ? effect.transform.parent.name : "null")}";
        Debug.Log(debugMsg, this);
        return effect;
    }

    /// <summary>
    /// Effectの表示状態を反転
    /// </summary>
    public void ToggleVisibility()
    {
        Rpc_VisibleEffect(!_isVisible);
    }

    /// <summary>
    /// エフェクトの表示・非表示を同期する
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_VisibleEffect(bool val)
    {
        Debug.Log("Rpc_VisibleEffect:" + val);
        _isVisible = val;//トグル切り替え
        var parentObj = EffectLists.Instance.GetParentObj(_effectType);
        parentObj.gameObject.SetActive(val);

        if (!HasStateAuthority) return;
        Rpc_SyncButtonVisibility(val);//ホストの状態を全員に同期
    }

    /// <summary>
    /// エフェクトの表示・非表示を同期する
    /// </summary>
    /// <param name="val">表示状態</param>
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_SyncButtonVisibility(bool val)
    {
        //ボタンの見た目を変更(PCのみ)
        ButtonStateVisualizer buttonStateVisualizer = new ButtonStateVisualizer();//ボタンの見た目を変更するクラスのインスタンス化
        buttonStateVisualizer.SetButtonState_Outline(visibleButton, val);
        Debug.Log("Rpc_SyncButtonVisibility called: " + visibleButton.name + " isVisible:" + val);
    }
}
