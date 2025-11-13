Shader "SeeExit/Stencil/Invisible Mask"
{
    Properties {}
    
    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "HDRenderPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry-1"
        }
        
        Pass
        {
            Name "InvisibleStencil"
            
            // Rendering settings
            Blend Off
            ZWrite Off
            ZTest LEqual
            Cull Back
            ColorMask 0
            AlphaToMask Off
            
            Stencil
            {
                Ref 1
                Comp Equal
                Pass Replace
            }
            /*
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            // Required for HDRP
            #pragma target 4.5
            #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
            
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                uint vertexID : SV_VertexID;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }
            
            float4 frag(Varyings input) : SV_Target
            {
                return float4(0, 0, 0, 0);
            }
            
            ENDHLSL*/
        }
    }
    
    //CustomEditor "UnityEditor.Rendering.HighDefinition.HDShaderGUI"
}