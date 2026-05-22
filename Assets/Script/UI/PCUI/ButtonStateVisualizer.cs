using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボタンの縁取りによってON/OFFを視覚的にわかりやすくするためのクラス
/// </summary>
public class ButtonStateVisualizer
{
    [Header("Visualize Outline")]
    float _onOutlineWidth = 10f;
    float _offOutlineWidth = 0f;
    Color _onCol = new Color(10f / 255f, 10f / 255f, 10f / 255f, 1);
    Color _offCol = new Color(0, 0, 0, 0);

    public void SetButtonState_Outline(Button button, bool val)
    {
        Outline outline = button.gameObject.GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogWarning("Outline component not found on button: " + button.name);
            return;
        }

        if (val == true)
        {
            outline.effectColor = _onCol;
            outline.effectDistance = new Vector2(_onOutlineWidth, -_onOutlineWidth);
        }
        else
        {
            outline.effectColor = _offCol;
            outline.effectDistance = new Vector2(_offOutlineWidth, -_offOutlineWidth);
        }
        Debug.Log("SetButtonState_Outline called: " + val, button.gameObject);
    }
}
