Shader "Custom/MovingFog"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (1,1,1,1)
        _Speed ("Speed", Range(0.1, 10.0)) = 1.0
        _Scale ("Scale", Range(0.1, 10.0)) = 1.0
        _Intensity ("Intensity", Range(0.0, 1.0)) = 0.5
        _Alpha ("Alpha", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FogColor;
            float _Speed;
            float _Scale;
            float _Intensity;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Access the built-in time variable
                float time = _Time.y;

                // Calculate the moving UV coordinates
                float2 movingUV = i.uv * _Scale + float2(time * _Speed, time * _Speed);

                // Sample the noise texture
                fixed4 noiseSample = tex2D(_MainTex, movingUV);

                // Calculate the final color
                fixed4 finalColor = lerp(_FogColor, noiseSample, _Intensity);
                finalColor.a *= _Alpha; // Apply the alpha transparency

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
