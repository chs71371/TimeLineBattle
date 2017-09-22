// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Photo Filter"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RGB ("Levels", Color) = (1, 0.5, 0.2)
		_Density ("Density", Range(0.0, 1.0)) = 0.35
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"
				#include "./Colorful.cginc"

				sampler2D _MainTex;
				half4 _RGB;
				half _Density;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tex2D(_MainTex, i.uv);
					
					half lum = luminance(color.rgb);
					half4 filter = _RGB;
					filter = lerp(half4(0.0, 0.0, 0.0, 0.0), filter, saturate(lum * 2.0));
					filter = lerp(filter, half4(1.0, 1.0, 1.0, 1.0), saturate(lum - 0.5) * 2.0);
					filter = lerp(color, filter, saturate(lum * _Density));
					filter.a = color.a;

					return filter;
				}

			ENDCG
		}
	}

	FallBack off
}
