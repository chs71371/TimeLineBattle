// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Smart Saturation"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Curve ("Curve Texture (A)", 2D) = "white" {}
		_Boost ("Saturation Boost (Float)", Float) = 1.0
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
				#include "./Colorful.cginc"

				sampler2D _MainTex;
				sampler2D _Curve;
				half _Boost;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tex2D(_MainTex, i.uv);
					half3 hsv = RGBtoHSV(color.rgb);
					half s = tex2D(_Curve, half2(hsv.y, 0.5)).a * 2.0 * hsv.y;
					color.rgb = HSVtoRGB(half3(hsv.x, s * _Boost, hsv.z));
					return color;
				}

			ENDCG
		}
	}

	FallBack off
}
