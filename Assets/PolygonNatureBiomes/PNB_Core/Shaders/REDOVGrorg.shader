Shader "SyntyStudios/Basic_LOD_Shader_URP"
{
    Properties
    {
        _Albedo("Albedo", 2D) = "white" {}
        _AlbedoColour("AlbedoColour", Color) = (1,1,1,0)
        _Metallic("Metallic", Float) = 0
        _Smoothness("Smoothness", Float) = 0.2
        _NormalMap("NormalMap", 2D) = "bump" {}
        _NormalAmount("NormalAmount", Float) = 0
        [Header(LOD Crossfade)]_LOD_DitheringMap("LOD_DitheringMap", 2D) = "white" {}
        [Toggle(LOD_FADE_CROSSFADE)] _LOD_FADE_CROSSFADE_out("LOD_FADE_CROSSFADE_out", Float) = 1
        [Toggle(LOD_FADE_CROSSFADE)] _LOD_FADE_CROSSFADE_in("LOD_FADE_CROSSFADE_in", Float) = 1
        _Cutoff( "Mask Clip Value", Float ) = 0.5
        [HideInInspector] _texcoord( "", 2D ) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            Cull Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local LOD_FADE_CROSSFADE
            #pragma multi_compile_instancing
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD1;
                float4 tangentWS : TEXCOORD2;
                float3 positionWS : TEXCOORD3;
                float4 screenPosition : TEXCOORD4;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            TEXTURE2D(_Albedo);
            SAMPLER(sampler_Albedo);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            TEXTURE2D(_LOD_DitheringMap);
            SAMPLER(sampler_LOD_DitheringMap);
            
            CBUFFER_START(UnityPerMaterial)
            float4 _Albedo_ST;
            float4 _NormalMap_ST;
            float4 _AlbedoColour;
            float _Metallic;
            float _Smoothness;
            float _NormalAmount;
            float _Cutoff;
            float4 _LOD_DitheringMap_TexelSize;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.tangentWS = float4(normalInput.tangentWS, input.tangentOS.w);
                output.uv = TRANSFORM_TEX(input.uv, _Albedo);
                
                output.screenPosition = ComputeScreenPos(output.positionCS);
                return output;
            }

            float DitherNoiseTex(float4 screenPos, TEXTURE2D_PARAM(noiseTexture, sampler_noiseTexture), float4 noiseTexelSize)
            {
                float2 screenUV = screenPos.xy * _ScreenParams.xy * noiseTexelSize.xy;
                float dither = SAMPLE_TEXTURE2D_LOD(noiseTexture, sampler_noiseTexture, screenUV, 0).g;
                float ditherRate = noiseTexelSize.x * noiseTexelSize.y;
                dither = (1 - ditherRate) * dither + ditherRate;
                return dither;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                
                // Sample textures
                half4 albedo = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, input.uv) * _AlbedoColour;
                half3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, input.uv), _NormalAmount);
                
                // Transform normal to world space
                float sgn = input.tangentWS.w;
                float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
                half3 normalWS = TransformTangentToWorld(normalTS, tangentToWorld);
                
                // LOD dithering
                float4 screenPosNorm = input.screenPosition / input.screenPosition.w;
                screenPosNorm.z = (UNITY_NEAR_CLIP_VALUE >= 0) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
                
                float dither = DitherNoiseTex(screenPosNorm, TEXTURE2D_ARGS(_LOD_DitheringMap, sampler_LOD_DitheringMap), _LOD_DitheringMap_TexelSize);
                
                #ifdef LOD_FADE_CROSSFADE
                    float fadeFactor = 1.0;
                #else
                    float fadeFactor = unity_LODFade.x < 0.0 ? 
                        lerp(0.0, 1.0, sqrt(1.0 - abs(unity_LODFade.x)) / 0.7) : 
                        lerp(0.0, 1.0, sqrt(unity_LODFade.x) / 0.7);
                #endif
                
                dither = step(dither, fadeFactor);
                clip(dither - _Cutoff);
                
                // Surface data
                InputData inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                inputData.normalWS = NormalizeNormalPerPixel(normalWS);
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                inputData.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                
                SurfaceData surfaceData;
                surfaceData.albedo = albedo.rgb;
                surfaceData.metallic = _Metallic;
                surfaceData.specular = half3(0.0h, 0.0h, 0.0h);
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = normalTS;
                surfaceData.occlusion = 1.0;
                surfaceData.emission = 0.0;
                surfaceData.alpha = 1.0;
                surfaceData.clearCoatMask = 0.0;
                surfaceData.clearCoatSmoothness = 0.0;
                
                half4 color = UniversalFragmentPBR(inputData, surfaceData);
                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "UnityEditor.ShaderGraphLitGUI"
}