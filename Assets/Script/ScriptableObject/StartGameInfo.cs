using UnityEngine;

[CreateAssetMenu(fileName = "StartGameInfo", menuName = "ScriptableObject/StartGameInfo", order = 1)]
public class StartGameInfo : ScriptableObject
{
    public bool isAuthority = true;
    public bool isMetaQuest = true;
    public bool isPc = false;
    // public bool isPerformer = false;//実験用、演奏者
    public bool isObserver = false;//実験者,タスク指示者
    public bool hideEffectMesh = true;
}
