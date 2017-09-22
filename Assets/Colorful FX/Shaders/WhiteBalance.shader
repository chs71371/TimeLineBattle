// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/White Balance"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_White ("White Color (RGB)", Color) = (0.5, 0.5, 0.5, 1.0)
	}

	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half4 _White;

		#define EPSILON 0.000001

		half4 frag_simple(v2f_img i) : SV_Target
		{
			half3 color = tex2D(_MainTex, i.uv).rgb;

			half3x3 scale = half3x3(
				0.5 / (_White.r + EPSILON), 0, 0,
				0, 0.5 / (_White.g + EPSILON), 0,
				0, 0, 0.5 / (_White.b + EPSILON)
			);
			
			color = mul(scale, color);
			return half4(saturate(color), 1.0);
		}

		half4 frag_complex(v2f_img i) : SV_Target
		{
			half3 color = tex2D(_MainTex, i.uv).rgb;

			half3x3 RGBtoLMS = half3x3(
				0.3811,0.5783,0.0402,
				0.1967,0.7244,0.0782,
				0.0241,0.1288,0.8444
			);

			half3x3 LMStoRGB = half3x3(
				4.4679, -3.5873, 0.1193,
				-1.2186, 2.3809, -0.1624,
				0.0497, -0.2439, 1.2045
			);

			half3 white = mul(RGBtoLMS, _White.rgb);

			half3x3 scale = half3x3(
				0.5 / (white.r + EPSILON), 0, 0,
				0, 0.5 / (white.g + EPSILON), 0,
				0, 0, 0.5 / (white.b + EPSILON)
			);

			half3 original = mul(RGBtoLMS, color);
			half3 balanced = mul(scale, original);
			color.rgb = mul(LMStoRGB, balanced);

			return half4(saturate(color), 1.0);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Simple
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_simple
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}

		// (1) Complex
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_complex
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}
	}

	FallBack off
}
