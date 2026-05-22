using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 色変更ボタンが押されたときの動作
/// </summary>
public class ColorButtonOnPointerDown : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] LaserMatUIManager laserMatUIManager;
    Color _color;
    void Awake()
    {
        _color = GetComponent<Image>().color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        laserMatUIManager.On_ChangeLaserColor(_color);
    }
}
