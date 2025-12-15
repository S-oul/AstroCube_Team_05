Shader "UI/MaskCycleShader6_OrganicAnimated"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}

        _Mask1("Mask 1", 2D) = "white" {}
        _Mask2("Mask 2", 2D) = "white" {}
        _Mask3("Mask 3", 2D) = "white" {}
        _Mask4("Mask 4", 2D) = "white" {}
        _Mask5("Mask 5", 2D) = "white" {}
        _Mask6("Mask 6", 2D) = "white" {}

        _Speed("Cycle Speed", Range(0.1, 10)) = 1
        _MaskStrength("Mask Strength", Range(0,1)) = 1
        _Opacity("Opacity", Range(0,1)) = 1

        _Fade("Fade Amount", Range(0,1)) = 0.25

        _NoiseStrength("Noise Influence", Range(0,1)) = 0.6
        _NoiseScale("Noise Scale", Range(1,200)) = 40

        _DistortStrength("Distortion Strength", Range(0,0.2)) = 0.03
        _DistortSpeed("Distortion Speed", Range(0,5)) = 1.5

        _MaskScale("Mask UV Scale", Vector) = (1,1,0,0)
        _MainScale("Main Texture Scale", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex; float4 _MainTex_ST;

            sampler2D _Mask1;
            sampler2D _Mask2;
            sampler2D _Mask3;
            sampler2D _Mask4;
            sampler2D _Mask5;
            sampler2D _Mask6;

            float _Speed;
            float _MaskStrength;
            float _Opacity;

            float _Fade;

            float _NoiseStrength;
            float _NoiseScale;

            float _DistortStrength;
            float _DistortSpeed;

            float4 _MaskScale;
            float4 _MainScale;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            float2 hash(float2 p) {
                p = float2(dot(p, float2(127.1, 311.7)),
                           dot(p, float2(269.5, 183.3)));
                return frac(sin(p) * 43758.5453);
            }

            float perlin(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float2 u = f*f*(3.0-2.0*f);

                return lerp(
                    lerp(dot(hash(i + float2(0,0)), f - float2(0,0)),
                         dot(hash(i + float2(1,0)), f - float2(1,0)), u.x),
                    lerp(dot(hash(i + float2(0,1)), f - float2(0,1)),
                         dot(hash(i + float2(1,1)), f - float2(1,1)), u.x),
                    u.y
                );
            }


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }


            float GetMask(int index, float2 uv)
            {
                uv = (uv - 0.5) * _MaskScale.xy + 0.5;

                if (index == 0) return tex2D(_Mask1, uv).r;
                if (index == 1) return tex2D(_Mask2, uv).r;
                if (index == 2) return tex2D(_Mask3, uv).r;
                if (index == 3) return tex2D(_Mask4, uv).r;
                if (index == 4) return tex2D(_Mask5, uv).r;
                return tex2D(_Mask6, uv).r;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float time = _Time.y * _DistortSpeed;

                float2 distortUV = i.uv;
                float d = perlin(i.uv * _NoiseScale + time);
                distortUV += (d - 0.5) * _DistortStrength;



                float2 mainUV = (distortUV - 0.5) * _MainScale.xy + 0.5;
                mainUV = saturate(mainUV); // NO REPEAT

                float4 col = tex2D(_MainTex, mainUV);



                float t = _Time.y * _Speed;
                float phase = fmod(t, 6);

                int current = (int)floor(phase);
                float blend = frac(phase);
                int next = (current + 1) % 6;

                float fadeBlend = smoothstep(0.5 - _Fade, 0.5 + _Fade, blend);

                float n = perlin(i.uv * _NoiseScale + float2(time * 0.7, time * 1.3));
                fadeBlend = lerp(fadeBlend, n, _NoiseStrength);

                float maskA = GetMask(current, distortUV);
                float maskB = GetMask(next, distortUV);

                float mask = lerp(maskA, maskB, fadeBlend);

                float alpha = col.a * mask * _MaskStrength;
                alpha *= _Opacity;

                return float4(col.rgb, alpha);
            }
            ENDCG
        }
    }
}
