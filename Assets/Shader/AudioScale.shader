// AudioLinkでオブジェクトの大きさを変えるシェーダー
Shader "Try/AudioScale"
{
    Properties
    {
        // マテリアルインスペクターに表示される設定項目
        _AudioLink ("AudioLink Texture", 2D) = "black" {}
        _ScaleMultiplier ("Scale Multiplier", Range(0, 5)) = 1.0
        _Band ("AudioLink Band (0-3)", Range(0, 3)) = 0
        _BaseColor ("Base Color", Color) = (0.5, 0.7, 1.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.llealloo.audiolink/Runtime/Shaders/AudioLink.cginc"

            // --- プロパティで定義した変数 ---
            CBUFFER_START(UnityPerMaterial)
                float _ScaleMultiplier;
                float _Band;
                half4 _BaseColor;
            CBUFFER_END

            // --- 構造体の定義 ---
            struct appdata
            {
                float4 vertex : POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;

                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            // --- 頂点シェーダー ---
            v2f vert (appdata v)
            {
                v2f o;
                
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                //UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert
                
                // どの帯域のデータを取得するかを決定 (0:Bass, 1:LowMid, 2:HighMid, 3:Treble)
                int band = (int)_Band;
                
                // AudioLinkテクスチャから指定した帯域の音の強さを取得 (0.0～1.0)
                float audioValue = AudioLinkData(uint2(0, band)).r;

         
                // 音の強さに応じて拡大率を計算
                // 基本の大きさ1.0に、(音の強さ * 倍率) を加算
                float scale = 1.0 + audioValue * _ScaleMultiplier;

                // 頂点座標のY成分を-0.5～0.5の範囲から0～1の範囲に変換する
                // これにより、オブジェクトの底面(Y=-0.5)で0、天面(Y=0.5)で1になる係数を作る
                float up_ratio = v.vertex.y + 0.5;

                // 音量に応じた「追加の高さ」を計算する
                // (例: scaleが1.2なら、追加の高さは0.2になる)
                float extra_height = (scale - 1.0); 

                // Y座標に「追加の高さ」を、up_ratioに応じて加算する
                // 底面(up_ratio=0)では移動せず、天面(up_ratio=1)で最大量移動する
                v.vertex.y += extra_height * up_ratio;

                // 最終的な頂点位置を計算
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                
                return o;
            }

            // --- フラグメントシェーダー ---
            half4 frag (v2f i) : SV_Target
            {
                // ここでは単純に色を付けるだけ
                // 応用として、音の強さで色を明るくすることも可能
                int band = (int)_Band;
                float audioValue = AudioLinkData(uint2(0, band)).r;

                // 音の強さに応じて基本の色を明るくする
                return _BaseColor * (0.5 + audioValue);
            }
            ENDHLSL
        }
    }
}