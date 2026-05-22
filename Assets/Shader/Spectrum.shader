// AudioLinkのスペクトラムを中央基準・上下対称で描画するシェーダー
Shader "AudioLink/AudioLinkSpectrum_Centered"
{
    Properties
    {
        _AudioLink("AudioLink Texture", 2D) = "black" {}
        _SpectrumColor("Spectrum Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _BackgroundColor("Background Color", Color) = (0.0, 0.0, 0.0, 0.0)
        _Intensity("Intensity", Range(0, 5)) = 1.0
        _Smoothing("Smoothing", Range(0.01, 1.0)) = 0.1
        _Center("Center", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" "LightMode"="UniversalForward"}

        Pass
        {
            // 透過設定
            Blend SrcAlpha OneMinusSrcAlpha 
            ZWrite Off 
            Cull Off // ← 追加: カリングを無効にし、両面を描画する

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.llealloo.audiolink/Runtime/Shaders/AudioLink.cginc"

            CBUFFER_START(UnityPerMaterial)
                half4 _SpectrumColor;
                half4 _BackgroundColor;
                float _Intensity;
                float _Smoothing;
                float _Center;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                
                 UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                //UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            // --- ここから下が変更されたフラグメントシェーダー ---
            half4 frag (v2f i) : SV_Target
            {
                
                // UVのX座標からサンプリングする周波数帯域を決定
                float frequencyBin = i.uv.x * AUDIOLINK_ETOTALBINS;
                float4 spectrum_value = AudioLinkLerpMultiline(ALPASS_DFT + float2(frequencyBin, 0.0));

                // 波形の「全長」を計算
                float waveHeight = spectrum_value.g * _Intensity;

                    
                // --- ロジックの変更点 ---
                // 1. 中心線をY=0.5に設定
               // float centerY = 0.5;

                // 2. 波形の「振幅（中心からの距離）」を計算 (全長の半分)
                float amplitude = waveHeight / 2.0;

                // 3. ピクセルの中心からの「絶対距離」を計算
                float pixelDistFromCenter = abs(i.uv.y - _Center);

                // 4. ピクセルの距離が振幅の内側にあるかを判定
                if (pixelDistFromCenter < amplitude)
                {
                    // 滑らかな境界線を作る
                    float diff = amplitude - pixelDistFromCenter;
                    float line_width = _Smoothing * 0.1;

                    if (diff < line_width)
                    {
                        float t = diff / line_width;
                        return _SpectrumColor * t;
                    }
                    return _SpectrumColor;
                }
                
                // それ以外は背景色を描画
                return _BackgroundColor;
            }
            ENDHLSL
        }
    }
}