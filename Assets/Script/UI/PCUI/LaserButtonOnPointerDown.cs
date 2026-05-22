using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// レーザーボタンが押されたときの動作(生成,表示,非表示)
/// </summary>
public class LaserButtonOnPointerDown : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] LaserManager.LaserPosType laserType;
    [SerializeField] LaserManager laserManager;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("LaserButtonOnPointerDown: " + laserType);
        laserManager.Rpc_OnSetEffect(laserType);
    }


}
