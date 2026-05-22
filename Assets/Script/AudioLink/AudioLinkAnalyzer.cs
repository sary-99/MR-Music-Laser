using Fusion;
using UnityEngine;

public class AudioLinkAnalyzer : MonoBehaviour
{
    [Header("References")]
    public StartGameInfo startGameInfo;
    public AudioLinkSync audioLinkSync;
    public RenderTexture audioLinkTex;
    Texture2D _audioLinkTexPrev;

    void Start()
    {
        if (!startGameInfo.isPc) return;
        //PCのみで実行
        _audioLinkTexPrev = new Texture2D(
            audioLinkTex.width,
            audioLinkTex.height,
            TextureFormat.RGBA32, false
        );
        Debug.Log($"AudioLinkAnalyzer: Initialized Texture2D with size {audioLinkTex.width}x{audioLinkTex.height}");
    }

    void OnDestroy()
    {
        if (!startGameInfo.isPc) return;
        //PCのみで実行
        Destroy(_audioLinkTexPrev);
        _audioLinkTexPrev = null;
    }
    void Update()
    {
        if (!startGameInfo.isPc) return; //PCのみで実行
        if (startGameInfo.isObserver) return;//実験者のPCからは同期しない

        // Debug.Log("AudioLinkAnalyzer: Render called.")
        if (audioLinkSync == null)
        {
            Debug.LogWarning("AudioLinkSync Instance is null in AudioLinkAnalyzer.");

            return;
        }

        Vector4 val = Analyze_alpass_audioLink();
        audioLinkSync.SetAllpass_audioLink(val);//HasStateAuthorityに送信
    }

    /// <summary>
    /// AudioLinkの値を反映
    /// </summary>
    Vector4 Analyze_alpass_audioLink()
    {
        if (!startGameInfo.isPc) return Vector4.zero;

        // Sample the pixel at (0,0) from the RenderTexture and use its red channel
        RenderTexture.active = audioLinkTex;
        _audioLinkTexPrev.ReadPixels(new Rect(0, 0, audioLinkTex.width, audioLinkTex.height), 0, 0);
        _audioLinkTexPrev.Apply();
        // _audioSync = _audioLinkTexPrev.GetPixel(0, 0).r;

        var bass = (Color32)_audioLinkTexPrev.GetPixel(0, 0);
        var low_mid = (Color32)_audioLinkTexPrev.GetPixel(0, 1);
        var high_mid = (Color32)_audioLinkTexPrev.GetPixel(0, 2);
        var treble = (Color32)_audioLinkTexPrev.GetPixel(0, 3);

        const float inv255 = 1f / 255f;
        float[] pixelData = { bass.r * inv255, low_mid.r * inv255, high_mid.r * inv255, treble.r * inv255 };
        Vector4 alpass_audioLink = new Vector4(pixelData[0], pixelData[1], pixelData[2], pixelData[3]);

        RenderTexture.active = null;
        return alpass_audioLink;
    }

    // float[] Analyze_alpass_dft()
    // {
    //     if (!startGameInfo.isPc) return new float[0];

    //     //https://docs.google.com/spreadsheets/d/1PkA98uI_zslpTr6ARBVGOBSq5Yna0rKPe_RWbdtbERM/edit?gid=0#gid=0
    //     int texX = 127;//0-127
    //     // int xOffset = 0;
    //     int texY = 15;//6-21
    //     int yOffset = 6;

    //     // Sample the pixel at (0,0) from the RenderTexture and use its red channel
    //     RenderTexture.active = audioLinkTex;
    //     _audioLinkTexPrev.ReadPixels(new Rect(0, 0, audioLinkTex.width, audioLinkTex.height), 0, 0);
    //     _audioLinkTexPrev.Apply();

    //     float[] dftData = new float[(texX * texY)];//2048
    //     const float inv255 = 1f / 255f;
    //     for (int i = 0; i < texY; i++)
    //     {
    //         for (int j = 0; j < texX; j++)
    //         {
    //             var pixel = (Color32)_audioLinkTexPrev.GetPixel(j, i + yOffset);
    //             dftData[(i + 1) * j] = pixel.r * inv255;
    //         }
    //     }

    //     RenderTexture.active = null;
    //     return dftData;
    // }


}
