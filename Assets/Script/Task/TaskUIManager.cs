using UnityEngine;
using TMPro;

public class TaskUIManager : MonoBehaviour
{
    public TaskUIPos taskUIPos;
    public StartGameInfo startGameInfo;
    public GameObject taskTextParent;
    TextMeshProUGUI _taskTMPro;
    public GameObject centerEyeAnchor;
    // [SerializeField] Vector3 _offset = new Vector3(0.3f, -0.3f, 0.5f);

    void Start()
    {
        _taskTMPro = taskTextParent.GetComponentInChildren<TextMeshProUGUI>();
        if (_taskTMPro == null)
        {
            Debug.LogError("TaskUIManager: TextMeshProUGUI component not found in taskTextParent.");
            return;
        }

        //演出者以外は非表示にする
        if (!startGameInfo.isAuthority || !startGameInfo.isMetaQuest)
        {
            VisibleTaskUI(false);
            return;
        }

        // centerEyeAnchor の子にする
        taskTextParent.transform.SetParent(centerEyeAnchor.transform, worldPositionStays: false);
        // centerEyeAnchor から見たローカル位置を設定
        taskTextParent.transform.localPosition = taskUIPos.pos;
        // 向きもヘッドセットと同じでよければ
        // taskTextParent.transform.localRotation = Quaternion.identity;
        _taskTMPro.fontSize = taskUIPos.fontSize;

        Debug.Log("TaskUIManager: TextMeshProUGUI component found.", this);

    }

    /// <summary>
    /// PCからMQへタスクに関するテキストを送信
    /// </summary>
    /// <param name="text"></param>
    public void SetTaskText(string text)
    {
        _taskTMPro.SetText(text);
    }

    // public void PlusTaskText(string text)
    // {
    //     _taskTMPro.text += text;
    // }

    bool _isTaskUIVisible = true;
    /// <summary>
    ///taskTextParentの子オブジェクトの表示・非表示切り替え
    /// </summary>
    public void VisibleTaskUI(bool visible)
    {
        _isTaskUIVisible = visible;
        for (int i = 0; i < taskTextParent.transform.childCount; i++)
        {
            GameObject child = taskTextParent.transform.GetChild(i).gameObject;
            child.SetActive(visible);
        }
    }

    void Update()
    {
        //TaskUIの表示・非表示切り替え
        // if (OVRInput.GetDown(OVRInput.Button.Two))
        // {
        //     VisibleTaskUI(!_isTaskUIVisible);
        // }
    }
}
