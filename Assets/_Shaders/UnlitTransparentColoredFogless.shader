Shader "Unlit/Transparent Colored Fogless" {
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
        _InvertBase("Invert Base", float) = 0.0
    }

    SubShader {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 100

        //ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float _InvertBase;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.texcoord);
                    col = fixed4(_InvertBase * (_InvertBase - col.rgb) + (1.0 - _InvertBase) * col.rgb, col.a) * _Color;
                    return col;
                }
            ENDCG
        }
    }
}