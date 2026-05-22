using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 色を変えるレーザーの種類を選ぶボタンが押されたときの動作
/// </summary>
public class ChangeColorButtonOnPointerDown : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] LaserMatUIManager laserMatUIManager;
    [SerializeField] LaserColorManager.LaserPositionType laserPos;
    public void OnPointerDown(PointerEventData eventData)
    {
        // LaserMatUIManager laserMatUIManager = new LaserMatUIManager();
        laserMatUIManager.ToggleLaserPosType(laserPos);
    }
}
