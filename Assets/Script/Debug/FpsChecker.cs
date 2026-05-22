using UnityEngine;
using TMPro;

public class FpsChecker : MonoBehaviour
{
    public StartGameInfo startGameInfo;
    public TextMeshProUGUI fpsText;
    float fps = 0f;

    float t = 0;
    void Update()
    {
        if (!startGameInfo.isMetaQuest) return;
        t += Time.unscaledDeltaTime;
        if (t < 1f) return;
        t = 0f;

        fps = 1f / Time.unscaledDeltaTime;
        fpsText.text = "FPS: " + fps.ToString("F1") + "\n";
    }
}
