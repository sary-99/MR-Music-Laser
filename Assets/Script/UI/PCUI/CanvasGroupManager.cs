using UnityEngine;
using System.Linq;
/// <summary>
/// 指定したCanvasGroupだけを表示するためのクラス
/// 管理したいCanvasGroupはCanvasGroup_Listに追加する
/// </summary>
public class CanvasGroupManager : MonoBehaviour
{
    [SerializeField] CanvasGroup[] canvasGroup_Array = new CanvasGroup[0]; // 管理したいCanvasGroupをインスペクターで追加する

    void Start()
    {
        HideAllCanvasGroups();
    }

    /// <summary>
    /// 外部から呼び出す場合
    /// </summary>
    /// <param name="canvasGroupName"></param>
    public void ShowJustOneCanvasGroup(string canvasGroupName)
    {
        ShowJustOneCanvasGroup(canvasGroup_Array, canvasGroupName);
    }
    /// <summary>
    /// canvasGroupNameで指定したCanvasGroupだけを表示する
    /// </summary>
    /// <param name="canvasGroupName"></param>
    void ShowJustOneCanvasGroup(CanvasGroup[] canvasGroupArray, string canvasGroupName)
    {
        Debug.Log("ShowCanvasGroup: " + canvasGroupName);
        int index = CanvasGroupNameToListIndex(canvasGroupArray, canvasGroupName);

        HideAllCanvasGroups();
        ShowCanvasGroup(canvasGroupArray[index]);
    }

    /// <summary>
    /// CanvasGroupのオブジェクト名からCanvasGroup_Arrayのインデックスを返す
    /// </summary>
    /// <param name="canvasGroupArray">探す配列</param>
    /// <param name="canvasGroupName">探すCanvasGroupの名前</param>
    /// <returns></returns>
    public int CanvasGroupNameToListIndex(CanvasGroup[] canvasGroupArray, string canvasGroupName)
    {
        if (!canvasGroupArray.Any(c => c.name == canvasGroupName))
        {
            Debug.LogError("CanvasGroupDisplayer: CanvasGroup name not found: " + canvasGroupName);
            return -1;
        }
        return System.Array.IndexOf(canvasGroupArray.Select(c => c.name).ToArray(), canvasGroupName);
    }

    /// <summary>
    /// すべてのCanvasGroupを非表示にする
    /// </summary>
    public void HideAllCanvasGroups()
    {
        foreach (var canvas in canvasGroup_Array)
        {
            HideCanvasGroup(canvas);
        }
    }

    /// <summary>
    /// CanvasGroupを表示する
    /// </summary>
    /// <param name="canvas"></param>
    void ShowCanvasGroup(CanvasGroup canvas)
    {
        canvas.alpha = 1f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    /// <summary>
    /// CanvasGroupを非表示にする
    /// </summary>
    /// <param name="canvas"></param>
    void HideCanvasGroup(CanvasGroup canvas)
    {
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }

}
