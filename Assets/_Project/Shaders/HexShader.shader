Shader "Hex/UnlitAtlasRandom"
{
    Properties {
        _BaseMap ("Base (2x3 Atlas)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Pass {
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            
            UNITY_INSTANCING_BUFFER_START(PerInstance)
                UNITY_DEFINE_INSTANCED_PROP(float, _Seed)
            UNITY_INSTANCING_BUFFER_END(PerInstance)

            struct appdata {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            float hash21(float2 p){
                p = frac(p * float2(123.34, 345.45));
                p += dot(p, p + 34.345);
                return frac(p.x * p.y);
            }

            v2f vert (appdata v) {
                UNITY_SETUP_INSTANCE_ID(v);
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                
                float seed = UNITY_ACCESS_INSTANCED_PROP(PerInstance, _Seed);
                float faceId = round(saturate(v.color.r) * 5.0);
                
                float rnd = hash21(float2(seed, faceId));
                int cell = (int)floor(rnd * 6.0 + 1e-4);
                
                const float3 colX = float3(0.0, 1.0/3.0, 2.0/3.0);
                const float2 rowY = float2(0.0, 1.0/2.0);

                int cx = cell % 3;
                int cy = cell / 3;

                float2 scale = float2(1.0/3.0, 1.0/2.0);
                float2 offset = float2(colX[cx], rowY[cy]);

                o.uv = v.uv * scale + offset;
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
            }
            ENDHLSL
        }
    }
}