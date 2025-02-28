Shader "Custom/PortalShader"
{
    Properties
    {
        _MainTex ("360° Texture", 2D) = "white" {}
        _PortalMask ("Portal Mask", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _PortalMask;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos); // Inverser la direction
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Définir le masque de portail
                float portalEffect = smoothstep(0.4, _PortalMask, dot(normalize(i.viewDir), float3(0,0,-1)));
                
                // Projeter l'environnement 360° sur toute la sphère
                float3 dir = normalize(i.viewDir);
                float2 sphericalUV = float2(0.5 + atan2(dir.z, dir.x) / (2 * 3.14159), 0.5 - asin(dir.y) / 3.14159);
                fixed4 col = tex2D(_MainTex, sphericalUV);
                
                return lerp(fixed4(0,0,0,0), col, portalEffect);
            }
            
            ENDCG
        }
    }
}