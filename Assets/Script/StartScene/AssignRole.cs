using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 初期シーンでマッチングするためのクラス
/// </summary>
public class AssignRole : MonoBehaviour
{
    [SerializeField] StartGameInfo startGameInfo;
    public void AssignMQHost()
    {
        startGameInfo.isAuthority = true;
        startGameInfo.isMetaQuest = true;
        startGameInfo.isPc = false;
        Debug.Log("AssignRole: Assigned Host Role");
        SceneManager.LoadScene("MQ_PC Experiment");
    }

    public void AssignMQClient()
    {
        startGameInfo.isAuthority = false;
        startGameInfo.isMetaQuest = true;
        startGameInfo.isPc = false;
        Debug.Log("AssignRole: Assigned Client Role");
        SceneManager.LoadScene("MQ_PC Experiment");
    }

    public void AssignPCController()
    {
        startGameInfo.isAuthority = false;
        startGameInfo.isMetaQuest = false;
        startGameInfo.isPc = true;
        Debug.Log("AssignRole: Assigned PC Controller Role");
        SceneManager.LoadScene("MQ_PC Experiment");
    }
}
