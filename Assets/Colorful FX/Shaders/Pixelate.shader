// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Pixelate"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Scale (X) Ratio (Y)", Vector) = (80, 1, 0, 0)
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
				half2 _Params;

				half4 frag(v2f_img i) : SV_Target
				{
					return pixelate(_MainTex, i.uv, _Params.x, _Params.y);
				}

			ENDCG
		}
	}

	FallBack off
}
