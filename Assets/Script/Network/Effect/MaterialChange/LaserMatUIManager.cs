using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LaserMatUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LaserBandChanger materialMsgSender;
    [SerializeField] LaserColorManager laserColorManager;

    // [Header("Dropdowns")]
    // [SerializeField] TMP_Dropdown centerLaser_BandType_Dropdown;
    // [SerializeField] TMP_Dropdown leftLaser_BandType_Dropdown;
    // [SerializeField] TMP_Dropdown rightLaser_BandType_Dropdown;

    [Header("Color Change Buttons")]
    [SerializeField] UnityEngine.UI.Button allButton;
    [SerializeField] UnityEngine.UI.Button leftButton;
    [SerializeField] UnityEngine.UI.Button centerButton;
    [SerializeField] UnityEngine.UI.Button rightButton;

    [Header("Color Buttons")]
    [SerializeField] UnityEngine.UI.Button redButton;
    [SerializeField] UnityEngine.UI.Button greenButton;
    [SerializeField] UnityEngine.UI.Button blueButton;
    [SerializeField] UnityEngine.UI.Button yellowButton;

    [Header("properties")]
    List<UnityEngine.UI.Button> _onButtons = new List<UnityEngine.UI.Button>(3);

    // #region 4Band Change Methods 未使用
    // public void OnChange4BandType_CenterLaser()
    // {
    //     int band = centerLaser_BandType_Dropdown.value;
    //     materialMsgSender.Rpc_ChangeBandtype_EffectType(EffectLists.EffectType.centerLaser, (LaserBandChanger.AudioLinkBandType)band);
    //     Debug.Log("OnChange4BandType_CenterLaser: " + band);
    // }
    // public void OnChange4BandType_LeftLaser()
    // {
    //     int band = leftLaser_BandType_Dropdown.value;
    //     materialMsgSender.Rpc_ChangeBandtype_EffectType(EffectLists.EffectType.leftLaser, (LaserBandChanger.AudioLinkBandType)band);
    //     Debug.Log("OnChange4BandType_LeftLaser: " + band);
    // }
    // public void OnChange4BandType_RightLaser()
    // {
    //     int band = rightLaser_BandType_Dropdown.value;
    //     materialMsgSender.Rpc_ChangeBandtype_EffectType(EffectLists.EffectType.rightLaser, (LaserBandChanger.AudioLinkBandType)band);
    //     Debug.Log("OnChange4BandType_RightLaser: " + band);
    // }
    // #endregion

    #region laser Color Change Methods

    /// <summary>
    /// 色変更を適用するレーザーの種類を切り替える
    /// </summary>
    /// <param name="laserPosType"></param>
    public void ToggleLaserPosType(LaserColorManager.LaserPositionType laserPosType)
    {
        ButtonStateVisualizer buttonVisualizer = new ButtonStateVisualizer();//ボタンの見た目を変更するクラスのインスタンス化
        var buttons = _GetButtons(laserPosType);
        Debug.Log("buttons.Length: " + buttons.Length);

        laserColorManager.Rpc_ClearLaserList();//一旦リストをクリアしてから再設定する
        foreach (var btn in buttons)
        {
            if (btn == null) continue;
            Debug.Log("ToggleLaserPosType button: " + btn.name + " alpha:" + btn.image.color.a);
            if (_onButtons.Contains(btn))
            {
                //すでにOnの場合
                buttonVisualizer.SetButtonState_Outline(btn, false);//ボタンの色ををオフにする
                _onButtons.Remove(btn);

                Debug.Log("Removed button: " + btn.name, this);
            }
            else
            {
                //未選択の場合
                buttonVisualizer.SetButtonState_Outline(btn, true);//ボタンの色ををオンにする
                _onButtons.Add(btn);

                Debug.Log("Added button: " + btn.name, this);
            }
        }

        //選択されているボタンに応じてレーザーリストを更新
        if (_onButtons.Contains(leftButton))
        {
            laserColorManager.Rpc_AddLaserToList(LaserColorManager.LaserPositionType.Left);
        }
        if (_onButtons.Contains(centerButton))
        {
            laserColorManager.Rpc_AddLaserToList(LaserColorManager.LaserPositionType.Center);
        }
        if (_onButtons.Contains(rightButton))
        {
            laserColorManager.Rpc_AddLaserToList(LaserColorManager.LaserPositionType.Right);
        }


        UnityEngine.UI.Button[] _GetButtons(LaserColorManager.LaserPositionType posType)
        {
            UnityEngine.UI.Button[] buttons = new UnityEngine.UI.Button[3];
            switch (posType)
            {
                case LaserColorManager.LaserPositionType.All:
                    buttons[0] = leftButton;
                    buttons[1] = centerButton;
                    buttons[2] = rightButton;

                    break;
                case LaserColorManager.LaserPositionType.Left:
                    buttons[0] = leftButton;
                    break;
                case LaserColorManager.LaserPositionType.Center:
                    buttons[0] = centerButton;
                    break;
                case LaserColorManager.LaserPositionType.Right:
                    buttons[0] = rightButton;
                    break;
                default:
                    break;
            }
            return buttons;
        }

        // Debug.Log("ChangeLaserPosType: " + laserPosType.ToString() + " Buttons count: " + buttons.Length);
    }


    /// <summary>
    /// UIのボタンから呼び出す、レーザーの色を変更するメソッド
    /// </summary>
    public void On_ChangeLaserColor(Color color)
    {
        laserColorManager.Rpc_ChangeLaserColor(color);
        Debug.Log("On_ChangeLaserColor: " + " " + color.ToString());

        ChangeLaserButtonColor(_onButtons, color);//ボタンの色も変更
    }

    /// <summary>
    /// レーザーの色をランダムに変更するメソッド(リスト内は全て同じ色)
    /// </summary>
    public void On_RandomChange_LaserColorInList()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        laserColorManager.Rpc_ChangeLaserColor(randomColor);
        ChangeLaserButtonColor(_onButtons, randomColor);
    }

    public void On_RandomChange_AllLaserColor()
    {
        laserColorManager.Rpc_RandomChange_AllLaserColor();
        Debug.Log("On_RandomChangeLaserColor: ");

        //ボタンを白に変更(alphaは維持)
        ChangeLaserButtonColor(_onButtons, Color.white);
    }

    /// <summary>
    /// 変更するレーザーボタンの色を変える(アルファ値は維持)
    /// </summary>
    /// <param name="buttons"></param>
    /// <param name="newColor"></param>
    void ChangeLaserButtonColor(List<UnityEngine.UI.Button> buttons, Color newColor)
    {
        foreach (var btn in buttons)
        {
            if (btn == null) continue;
            Color buttonColor = btn.image.color;
            newColor = new Color(newColor.r, newColor.g, newColor.b, buttonColor.a);
            btn.image.color = newColor;
            Debug.Log("ChangeLaserButtonColor: " + btn.name + " Color:" + newColor.ToString());
        }
    }
    #endregion

}
