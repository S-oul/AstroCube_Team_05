// Made with Amplify Shader Editor v1.9.8.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Reset_shader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _TextureSample0("Texture Sample 0", 2D) = "white" {}
        _HDR("HDR", Float) = 0
        _Min("Min", Float) = 0.09
        _Max("Max", Float) = 0.62
        _TextureSample2("Texture Sample 2", 2D) = "white" {}
        _Float2("Float 2", Range( 0 , 1)) = 0
        _TextureSample1("Texture Sample 1", 2D) = "white" {}
        _Float3("Float 3", Float) = 0
        _TextureSample3("Texture Sample 3", 2D) = "white" {}
        _Float4("Float 4", Float) = 0
        _TextureSample4("Texture Sample 4", 2D) = "white" {}
        _TextureSample5("Texture Sample 5", 2D) = "white" {}
        _Slider1("Slider", Range( 0 , 1)) = 0
        [Toggle(_BOUTON_ON)] _Bouton("Bouton", Float) = 0
        [HideInInspector] _texcoord( "", 2D ) = "white" {}

    }

    SubShader
    {
		LOD 0

        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

        Stencil
        {
        	Ref [_Stencil]
        	ReadMask [_StencilReadMask]
        	WriteMask [_StencilWriteMask]
        	Comp [_StencilComp]
        	Pass [_StencilOp]
        }


        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        
        Pass
        {
            Name "Default"
        CGPROGRAM
            #define ASE_VERSION 19801

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityShaderVariables.cginc"
            #pragma shader_feature_local _BOUTON_ON


            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4  mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
                float4 ase_texcoord3 : TEXCOORD3;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

            uniform float _Slider1;
            uniform float _Min;
            uniform float _Max;
            uniform sampler2D _TextureSample0;
            uniform float _Float3;
            uniform float _Float4;
            uniform sampler2D _TextureSample2;
            uniform float _Float2;
            uniform float _HDR;
            uniform sampler2D _TextureSample5;
            uniform float4 _TextureSample5_ST;
            uniform sampler2D _TextureSample1;
            uniform float4 _TextureSample1_ST;
            uniform sampler2D _TextureSample4;
            uniform float4 _TextureSample4_ST;
            uniform sampler2D _TextureSample3;
            uniform float4 _TextureSample3_ST;
            struct Gradient
            {
            	int type;
            	int colorsLength;
            	int alphasLength;
            	float4 colors[8];
            	float2 alphas[8];
            };
            
            Gradient NewGradient(int type, int colorsLength, int alphasLength, 
            float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
            float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
            {
            	Gradient g;
            	g.type = type;
            	g.colorsLength = colorsLength;
            	g.alphasLength = alphasLength;
            	g.colors[ 0 ] = colors0;
            	g.colors[ 1 ] = colors1;
            	g.colors[ 2 ] = colors2;
            	g.colors[ 3 ] = colors3;
            	g.colors[ 4 ] = colors4;
            	g.colors[ 5 ] = colors5;
            	g.colors[ 6 ] = colors6;
            	g.colors[ 7 ] = colors7;
            	g.alphas[ 0 ] = alphas0;
            	g.alphas[ 1 ] = alphas1;
            	g.alphas[ 2 ] = alphas2;
            	g.alphas[ 3 ] = alphas3;
            	g.alphas[ 4 ] = alphas4;
            	g.alphas[ 5 ] = alphas5;
            	g.alphas[ 6 ] = alphas6;
            	g.alphas[ 7 ] = alphas7;
            	return g;
            }
            
            float4 SampleGradient( Gradient gradient, float time )
            {
            	float3 color = gradient.colors[0].rgb;
            	UNITY_UNROLL
            	for (int c = 1; c < 8; c++)
            	{
            	float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
            	color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
            	}
            	#ifndef UNITY_COLORSPACE_GAMMA
            	color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
            	#endif
            	float alpha = gradient.alphas[0].x;
            	UNITY_UNROLL
            	for (int a = 1; a < 8; a++)
            	{
            	float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
            	alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
            	}
            	return float4(color, alpha);
            }
            


            v2f vert(appdata_t v )
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float4 ase_positionCS = UnityObjectToClipPos( v.vertex );
                float4 screenPos = ComputeScreenPos( ase_positionCS );
                OUT.ase_texcoord3 = screenPos;
                

                v.vertex.xyz +=  float3( 0, 0, 0 ) ;

                float4 vPosition = UnityObjectToClipPos(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;

                float2 pixelSize = vPosition.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                OUT.texcoord = v.texcoord;
                OUT.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN ) : SV_Target
            {
                //Round up the alpha color coming from the interpolator (to 1.0/256.0 steps)
                //The incoming alpha could have numerical instability, which makes it very sensible to
                //HDR color transparency blend, when it blends with the world's texture.
                const half alphaPrecision = half(0xff);
                const half invAlphaPrecision = half(1.0/alphaPrecision);
                IN.color.a = round(IN.color.a * alphaPrecision)*invAlphaPrecision;

                float2 texCoord46 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
                float smoothstepResult53 = smoothstep( 0.0 , 0.0 , ( ( 1.0 - texCoord46.y ) - ( 1.0 - _Slider1 ) ));
                Gradient gradient20 = NewGradient( 0, 2, 2, float4( 0.5503581, 0.2152153, 0.6830188, 0.2717632 ), float4( 0.7809727, 0, 0.8943396, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
                float4 screenPos = IN.ase_texcoord3;
                float4 ase_positionSSNorm = screenPos / screenPos.w;
                ase_positionSSNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_positionSSNorm.z : ase_positionSSNorm.z * 0.5 + 0.5;
                float2 appendResult21 = (float2(ase_positionSSNorm.x , ase_positionSSNorm.y));
                float2 appendResult23 = (float2(ase_positionSSNorm.x , ase_positionSSNorm.y));
                float2 panner32 = ( 1.0 * _Time.y * float2( -0.03,-0.03 ) + (appendResult23*1.0 + 0.0));
                float smoothstepResult30 = smoothstep( _Float3 , _Float4 , tex2D( _TextureSample2, panner32 ).r);
                float2 panner37 = ( 1.0 * _Time.y * float2( 0.03,0.03 ) + ( appendResult21 + ( smoothstepResult30 * _Float2 ) ));
                float smoothstepResult27 = smoothstep( _Min , _Max , tex2D( _TextureSample0, panner37 ).r);
                float4 temp_output_18_0 = ( saturate( SampleGradient( gradient20, saturate( smoothstepResult27 ) ) ) * _HDR );
                float2 uv_TextureSample5 = IN.texcoord.xy * _TextureSample5_ST.xy + _TextureSample5_ST.zw;
                float2 uv_TextureSample1 = IN.texcoord.xy * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
                float2 uv_TextureSample4 = IN.texcoord.xy * _TextureSample4_ST.xy + _TextureSample4_ST.zw;
                #ifdef _BOUTON_ON
                float4 staticSwitch74 = tex2D( _TextureSample4, uv_TextureSample4 );
                #else
                float4 staticSwitch74 = tex2D( _TextureSample1, uv_TextureSample1 );
                #endif
                float2 uv_TextureSample3 = IN.texcoord.xy * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
                float2 texCoord56 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
                float smoothstepResult59 = smoothstep( 0.0 , 0.0 , ( texCoord56.y - _Slider1 ));
                float4 temp_output_72_0 = ( ( smoothstepResult53 * ( ( temp_output_18_0 * tex2D( _TextureSample5, uv_TextureSample5 ) ) + ( temp_output_18_0 * staticSwitch74 ) ) ) + ( saturate( ( ( ( staticSwitch74 + float4( 0,0,0,0 ) ) + tex2D( _TextureSample3, uv_TextureSample3 ) ) * smoothstepResult59 ) ) * 1.0 ) );
                

                half4 color = temp_output_72_0;

                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color.rgb *= color.a;

                return color;
            }
        ENDCG
        }
    }
    CustomEditor "AmplifyShaderEditor.MaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19801
Node;AmplifyShaderEditor.ScreenPosInputsNode;83;-5568,-528;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;23;-5328,-448;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;24;-5136,-432;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;32;-4880,-416;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.03,-0.03;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-4528,-128;Inherit;False;Property;_Float4;Float 4;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-4592,-256;Inherit;False;Property;_Float3;Float 3;7;0;Create;True;0;0;0;False;0;False;0;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;40;-4672,-528;Inherit;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;0;False;0;False;-1;None;e5b234c8823b14d469d368b1938e96ac;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SmoothstepOpNode;30;-4288,-544;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.07;False;2;FLOAT;-0.17;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-4304,-256;Inherit;False;Property;_Float2;Float 2;5;0;Create;True;0;0;0;False;0;False;0;0.4330312;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;22;-4464,-752;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;21;-4144,-720;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-4032,-480;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-3904,-720;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;37;-3728,-720;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.03,0.03;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-3328,48;Inherit;False;Property;_Max;Max;3;0;Create;True;0;0;0;False;0;False;0.62;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-3408,-112;Inherit;False;Property;_Min;Min;2;0;Create;True;0;0;0;False;0;False;0.09;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;31;-3456,-480;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;a1df82c5c992a5143bbbb2ba6030c4e7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SmoothstepOpNode;27;-3088,-272;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;26;-2736,-304;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;43;-2992,416;Inherit;True;Property;_TextureSample4;Texture Sample 4;10;0;Create;True;0;0;0;False;0;False;-1;None;36d4c17feaa43a34690d1eec215f91d3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;41;-2928,144;Inherit;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;0;False;0;False;-1;None;2f1cd7978af24ef4e8d76c66b76bdd5b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.GradientNode;20;-2736,-624;Inherit;False;0;2;2;0.5503581,0.2152153,0.6830188,0.2717632;0.7809727,0,0.8943396,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.GradientSampleNode;19;-2432,-464;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;-2048,624;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-2320,0;Inherit;False;Property;_Slider1;Slider;12;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;74;-2368,336;Inherit;False;Property;_Bouton;Bouton;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;X;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;46;-1952,-704;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;38;-2032,-448;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1504,880;Inherit;False;Constant;_Float6;Float 1;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1568,752;Inherit;False;Constant;_Float5;Float 0;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;42;-1360,432;Inherit;True;Property;_TextureSample3;Texture Sample 3;8;0;Create;True;0;0;0;False;0;False;-1;None;82c2122dc270c004ebc1c0892654894b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleSubtractOpNode;60;-1680,512;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1920,-48;Inherit;False;Property;_HDR;HDR;1;0;Create;True;0;0;0;False;0;False;0;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;-1328,112;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;67;-1680,-800;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;71;-1616,-608;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-1024,224;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;59;-1264,784;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1760,-368;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;44;-1472,-224;Inherit;True;Property;_TextureSample5;Texture Sample 5;11;0;Create;True;0;0;0;False;0;False;-1;None;f07e3051224aaf44baa9a6360550b9ce;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;54;-1328,-544;Inherit;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-1264,-432;Inherit;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;47;-1424,-832;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-1680,-32;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1104,-368;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-768,432;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;53;-1088,-800;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-848,-176;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;51;-464,240;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-368,464;Inherit;False;Constant;_Float7;Float 7;14;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-528,-480;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-224,192;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-48,-16;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;73;224,144;Inherit;False;Alpha Split;-1;;1;07dab7960105b86429ac8eebd729ed6d;0;1;2;COLOR;0,0,0,0;False;2;FLOAT3;0;FLOAT;6
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;86;512,0;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;0;3;Reset_shader;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;True;_ColorMask;False;False;False;False;False;False;False;True;True;0;True;_Stencil;255;True;_StencilReadMask;255;True;_StencilWriteMask;0;True;_StencilComp;0;True;_StencilOp;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;0;True;unity_GUIZTestMode;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;23;0;83;1
WireConnection;23;1;83;2
WireConnection;24;0;23;0
WireConnection;32;0;24;0
WireConnection;40;1;32;0
WireConnection;30;0;40;1
WireConnection;30;1;28;0
WireConnection;30;2;25;0
WireConnection;21;0;22;1
WireConnection;21;1;22;2
WireConnection;33;0;30;0
WireConnection;33;1;29;0
WireConnection;34;0;21;0
WireConnection;34;1;33;0
WireConnection;37;0;34;0
WireConnection;31;1;37;0
WireConnection;27;0;31;1
WireConnection;27;1;36;0
WireConnection;27;2;35;0
WireConnection;26;0;27;0
WireConnection;19;0;20;0
WireConnection;19;1;26;0
WireConnection;74;1;41;0
WireConnection;74;0;43;0
WireConnection;38;0;19;0
WireConnection;60;0;56;2
WireConnection;60;1;57;0
WireConnection;81;0;74;0
WireConnection;67;0;46;2
WireConnection;71;0;57;0
WireConnection;82;0;81;0
WireConnection;82;1;42;0
WireConnection;59;0;60;0
WireConnection;59;1;61;0
WireConnection;59;2;62;0
WireConnection;18;0;38;0
WireConnection;18;1;10;0
WireConnection;47;0;67;0
WireConnection;47;1;71;0
WireConnection;78;0;18;0
WireConnection;78;1;74;0
WireConnection;45;0;18;0
WireConnection;45;1;44;0
WireConnection;50;0;82;0
WireConnection;50;1;59;0
WireConnection;53;0;47;0
WireConnection;53;1;54;0
WireConnection;53;2;55;0
WireConnection;79;0;45;0
WireConnection;79;1;78;0
WireConnection;51;0;50;0
WireConnection;63;0;53;0
WireConnection;63;1;79;0
WireConnection;84;0;51;0
WireConnection;84;1;85;0
WireConnection;72;0;63;0
WireConnection;72;1;84;0
WireConnection;73;2;72;0
WireConnection;86;0;72;0
ASEEND*/
//CHKSM=5EB285E153E27B2F03C7C80A04FDF837F199D8BA