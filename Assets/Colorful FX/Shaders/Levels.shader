// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Levels"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_InputMin ("Input Black", Vector) = (0, 0, 0, 1)
		_InputMax ("Input White", Vector) = (1, 1, 1, 1)
		_InputGamma ("Input Gamma", Vector) = (1, 1, 1, 1)
		_OutputMin ("Output Black", Vector) = (0, 0, 0, 1)
		_OutputMax ("Output White", Vector) = (1, 1, 1, 1)
	}

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half4 _InputMin;
				half4 _InputMax;
				half4 _InputGamma;
				half4 _OutputMin;
				half4 _OutputMax;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tex2D(_MainTex, i.uv);
					color = lerp(_OutputMin, _OutputMax, pow(min(max(color - _InputMin, half4(0.0, 0.0, 0.0, 0.0)) / (_InputMax - _InputMin), half4(1.0, 1.0, 1.0, 1.0)), 1.0 / _InputGamma));
					return color;
				}

			ENDCG
		}
	}

	FallBack off
}
