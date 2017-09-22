// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Glitch"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// Interference
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"
				#pragma target 3.0

				sampler2D _MainTex;
				float3 _Params; // x: speed, y: density, z: maxDisplace

				inline float rand(float2 seed)
				{
					return frac(sin(dot(seed * floor(_Time.y * _Params.x), float2(127.1, 311.7))) * 43758.5453123);
				}

				inline float rand(float seed)
				{
					return rand(float2(seed, 1.0));
				}

				half4 frag(v2f_img i) : SV_Target
				{
					float2 rblock = rand(floor(i.uv * _Params.y));
					float displaceNoise = pow(rblock.x, 8.0) * pow(rblock.x, 3.0) - pow(rand(7.2341), 17.0) * _Params.z;

					float r = tex2D(_MainTex, i.uv).r;
					float g = tex2D(_MainTex, i.uv + half2(displaceNoise * 0.05 * rand(7.0), 0.0)).g;
					float b = tex2D(_MainTex, i.uv - half2(displaceNoise * 0.05 * rand(13.0), 0.0)).b;

					return half4(r, g, b, 1.0);
				}

			ENDCG
		}

		// Tearing
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_tearing
				#pragma fragmentoption ARB_precision_hint_fastest 
				#pragma target 3.0

				#include "./Glitch.cginc"

			ENDCG
		}

		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_tearing
				#pragma fragmentoption ARB_precision_hint_fastest 
				#pragma target 3.0

				#define ALLOW_FLIPPING
				#include "./Glitch.cginc"

			ENDCG
		}

		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_tearing
				#pragma fragmentoption ARB_precision_hint_fastest 
				#pragma target 3.0

				#define YUV_COLOR_BLEEDING
				#include "./Glitch.cginc"

			ENDCG
		}

		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_tearing
				#pragma fragmentoption ARB_precision_hint_fastest 
				#pragma target 3.0
				
				#define ALLOW_FLIPPING
				#define YUV_COLOR_BLEEDING
				#include "./Glitch.cginc"

			ENDCG
		}
	}

	FallBack off
}
