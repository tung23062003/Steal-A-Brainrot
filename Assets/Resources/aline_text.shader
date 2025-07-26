Shader "Hidden/ALINE/Font" {
	Properties {
		_Color ("Main Color", Vector) = (1,1,1,0.5)
		_FadeColor ("Fade Color", Vector) = (1,1,1,0.3)
		_MainTex ("Texture", 2D) = "white" {}
		_FallbackTex ("Fallback Texture", 2D) = "white" {}
		_FallbackAmount ("Fallback Amount", Range(0, 1)) = 1
		_TransitionPoint ("Transition Point", Range(0, 5)) = 0.6
		_MipBias ("Mip Bias", Range(-2, 0)) = -1
		_GammaCorrection ("Gamma Correction", Range(0, 2)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_MatrixMVP;

			struct Vertex_Stage_Input
			{
				float3 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixMVP, float4(input.pos, 1.0));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState _MainTex_sampler;
			fixed4 _Color;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(_MainTex_sampler, float2(input.uv.x, input.uv.y)) * _Color;
			}

			ENDHLSL
		}
	}
}