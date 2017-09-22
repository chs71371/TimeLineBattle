// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Threshold"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Threshold ("Threshold", Range(0.0, 1.0)) = 0.5
		_Range ("Noise Range", Range(0.0, 0.5)) = 0.2
	}

	CGINCLUDE
	
		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half _Threshold;
		half _Range;

		half4 frag(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half s = step(luminance(color.rgb), _Threshold);
			color = lerp(half4(1.0, 1.0, 1.0, 1.0), half4(0.0, 0.0, 0.0, 0.0), s);
			return color;
		}

		half4 frag_noise(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half r = frac(sin(dot(i.uv, half2(12.9898, 78.233))) * 43758.5453) * _Range - _Range / 2.0;
			half s = step(luminance(color.rgb), _Threshold + r);
			color = lerp(half4(1.0, 1.0, 1.0, 1.0), half4(0.0, 0.0, 0.0, 0.0), s);
			return color;
		}

	ENDCG

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

			ENDCG
		}

		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_noise
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}
	}

	FallBack off
}
