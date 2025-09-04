Shader "LowPolyWater/WaterShadedURP"
{
    Properties
    {
        _BaseColor ("Base color", COLOR) = (0.54, 0.95, 0.99, 0.5)
        _SpecColor ("Specular Material Color", Color) = (1,1,1,1)
        _Shininess ("Shininess", Float) = 10
        _ShoreTex ("Shore & Foam texture", 2D) = "black" {}

        _InvFadeParemeter ("Auto blend parameter (Edge, Shore, Distance scale)", Vector) = (0.2, 0.39, 0.5, 1.0)

        _BumpTiling ("Foam Tiling", Vector) = (1.0 ,1.0, -2.0, 3.0)
        _BumpDirection ("Foam movement", Vector) = (1.0 ,1.0, -1.0, 1.0)

        _Foam ("Foam (intensity, cutoff)", Vector) = (0.1, 0.375, 0.0, 0.0)
        [MaterialToggle] _isInnerAlphaBlendOrColor("Fade inner to color or alpha?", Float) = 0
    }

    SubShader
    {
        Tags {"RenderType"="Transparent" "Queue"="Transparent"}
        LOD 500
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fragment _ _FOG_LINEAR _FOG_EXP _FOG_EXP2
            #pragma multi_compile _ WATER_EDGEBLEND_ON WATER_EDGEBLEND_OFF

            // URP include’lar
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_ShoreTex);
            SAMPLER(sampler_ShoreTex);

            TEXTURE2D_X_FLOAT(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            float4 _BaseColor;
            float4 _SpecColor;
            float _Shininess;
            float4 _InvFadeParemeter;
            float4 _BumpTiling;
            float4 _BumpDirection;
            float4 _Foam;
            float _isInnerAlphaBlendOrColor;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float4 bumpCoords : TEXCOORD3;
            };

            inline half4 Foam(float4 coords)
            {
                half4 foam = (SAMPLE_TEXTURE2D(_ShoreTex, sampler_ShoreTex, coords.xy) *
                              SAMPLE_TEXTURE2D(_ShoreTex, sampler_ShoreTex, coords.zw)) - 0.125;
                return foam;
            }

            Varyings vert (Attributes v)
            {
                Varyings o;
                float3 worldPos = TransformObjectToWorld(v.positionOS.xyz);
                o.worldPos = worldPos;
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);

                o.positionCS = TransformWorldToHClip(worldPos);
                o.screenPos = ComputeScreenPos(o.positionCS);

                float2 tileUV = worldPos.xz;
                o.bumpCoords.xyzw = (tileUV.xyxy + _Time.xxxx * _BumpDirection.xyzw) * _BumpTiling.xyzw;

                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // edge blend
                half4 edgeBlendFactors = half4(1,0,0,0);

                #if defined(WATER_EDGEBLEND_ON)
                    float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, i.screenPos.xy / i.screenPos.w).r;
                    depth = LinearEyeDepth(depth, _ZBufferParams);
                    float sceneZ = i.screenPos.w;
                    edgeBlendFactors = saturate(_InvFadeParemeter * (depth - sceneZ));
                    edgeBlendFactors.y = 1.0 - edgeBlendFactors.y;
                #endif

                // Lighting
                Light mainLight = GetMainLight();
                float3 normal = normalize(i.normalWS);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

                float3 lightDir = normalize(mainLight.direction);
                float NdotL = saturate(dot(normal, lightDir));

                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _BaseColor.rgb;
                float3 diffuse = mainLight.color.rgb * _BaseColor.rgb * NdotL;
                float3 specular = pow(saturate(dot(reflect(-lightDir, normal), viewDir)), _Shininess) * _SpecColor.rgb * mainLight.color.rgb;

                half4 baseColor = half4(ambient + diffuse + specular, _BaseColor.a);

                // foam
                half4 foam = Foam(i.bumpCoords * 2.0);
                baseColor.rgb += foam.rgb * _Foam.x * (edgeBlendFactors.y);

                if (_isInnerAlphaBlendOrColor == 0)
                    baseColor.rgb += 1.0 - edgeBlendFactors.x;
                else
                    baseColor.a = edgeBlendFactors.x;

                return baseColor;
            }
            ENDHLSL
        }
    }

    FallBack Off
}
