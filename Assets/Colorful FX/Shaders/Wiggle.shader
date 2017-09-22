// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Wiggle"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Timer ("Timer", Float) = 0.0
		_Scale ("Scale", Float) = 12.0
		_Params ("Frequency (X) Amplitude (Y) Timer (Z)", Vector) = (0, 0, 0, 0)
	}

	CGINCLUDE
	
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half3 _Params;

		half4 frag_simple(v2f_img i) : SV_Target
		{
			half2 t = i.uv;
			t.x += sin(_Params.z + t.x * _Params.x) * _Params.y;
			t.y += cos(_Params.z + t.y * _Params.x) * _Params.y - _Params.y;
			return tex2D(_MainTex, t);
		}

		half2 shift(half2 uv)
		{
			half2 freq = _Params.x * (uv + _Params.z);
			return cos(half2(cos(freq.x - freq.y) * cos(freq.y), sin(freq.x + freq.y) * sin(freq.y)));
		}

		half4 frag_complex(v2f_img i) : SV_Target
		{
			half2 p = shift(i.uv);
			half2 q = shift(i.uv + 1.0);
			half2 t = i.uv + _Params.y * (p - q);
			return tex2D(_MainTex, t);
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
				#pragma target 3.0

			ENDCG
		}
	}

	FallBack off
}
