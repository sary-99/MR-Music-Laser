using UnityEngine;

/// <summary>
/// Scene0でシステムが起動した際に、MetaQuestかPCかを判定して
/// </summary>
public class SystemLaunched : MonoBehaviour
{
        // public bool isMQ;
        [SerializeField] GameObject mqRoot;
        [SerializeField] GameObject pcRoot;

        [SerializeField] StartGameInfo startGameInfo;
        void Start()
        {
#if UNITY_EDITOR
                //UnityEditorの場合
                //StartGameInfoの値はそのまま
#elif UNITY_STANDALONE_WIN
//Winspwsにビルドした場合
                startGameInfo.isPc = true;
                startGameInfo.isMetaQuest = false;
#else
//MetaQuestの場合を判定するものはないので、Win以外ならMQと判定することとする
                startGameInfo.isPc = false;
                startGameInfo.isMetaQuest = true;
                startGameInfo.isObserver = false;
#endif

                mqRoot.SetActive(startGameInfo.isMetaQuest);
                pcRoot.SetActive(startGameInfo.isPc);
        }
}
