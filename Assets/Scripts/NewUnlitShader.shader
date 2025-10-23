Shader "Unlit/HologramCircle"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0, 0.5, 1, 0.5) // Default: Blue, semi-transparent
        _Pulse("Pulse", Float) = 1.0
        _RimPower("Rim Power", Range(0.1, 10.0)) = 2.0
        _ScanlineSpeed("Scanline Speed", Float) = 5.0
        _ScanlineDensity("Scanline Density", Float) = 20.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Cull Off // Render both sides of the plane
        ZWrite Off // Don't write to depth buffer for transparency
        Blend SrcAlpha OneMinusSrcAlpha // Standard transparency blending

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            fixed4 _BaseColor;
            float _Pulse;
            float _RimPower;
            float _ScanlineSpeed;
            float _ScanlineDensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 1. Calculate Rim Effect (Fresnel)
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
                float fresnel = 1.0 - saturate(dot(i.worldNormal, viewDir));
                fresnel = pow(fresnel, _RimPower);

                // 2. Blend from transparent to base color using Fresnel
                // lerp(a, b, t) -> blends from a to b based on t
                fixed4 fresnelColor = lerp(fixed4(0,0,0,0), _BaseColor, fresnel);
                
                // 3. Calculate Scan Lines
                float scanline = sin((i.uv.y * _ScanlineDensity) + (_Time.y * _ScanlineSpeed));
                scanline = step(0.5, scanline); // Make the lines sharp
                fixed4 scanlineColor = fixed4(scanline, scanline, scanline, scanline) * _BaseColor.a * 0.5;

                // 4. Combine effects
                fixed4 finalColor = fresnelColor + scanlineColor;

                // 5. Apply Pulse
                finalColor *= _Pulse;

                return finalColor;
            }
            ENDCG
        }
    }
}
