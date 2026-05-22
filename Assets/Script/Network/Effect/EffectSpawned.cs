using Fusion;

/// <summary>
/// EffectがSpawnされたときにEffectListsに追加するクラス
/// Spawnしたときに全端末で実行させる
/// </summary>
public class EffectSpawned : NetworkBehaviour
{
    public EffectLists.EffectType effectType;
    public override void Spawned()
    {
        base.Spawned();
        EffectLists.Instance.EffectToList(Object, effectType);
    }
}