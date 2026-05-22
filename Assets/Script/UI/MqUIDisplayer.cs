using UnityEngine;

public class MqUIDisplayer : MonoBehaviour
{
    [SerializeField] GameObject uiParent;
    [SerializeField] GameObject rotateCube;
    void Start()
    {
        if (uiParent == null)
        {
            uiParent = this.gameObject;
        }
    }

    bool _isMqUIVisible = true;
    void VisibleMqUI(bool visible)
    {
        _isMqUIVisible = visible;
        for (int i = 0; i < uiParent.transform.childCount; i++)
        {
            GameObject child = uiParent.transform.GetChild(i).gameObject;

            child.SetActive(visible);
        }
        VisibleRotateCube(visible);
    }

    /// <summary>
    /// 通信成功確認用Cubeの表示・非表示切り替え(デバッグ用)
    /// </summary>
    void VisibleRotateCube(bool visible)
    {
        if (rotateCube != null)
        {
            rotateCube.SetActive(visible);
        }
    }
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            VisibleMqUI(!_isMqUIVisible);
        }
    }
}
