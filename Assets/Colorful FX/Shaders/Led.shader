// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Led"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Scale (X) Ratio (Y) Brightness (Z) Shape (W)", Vector) = (80, 1, 1, 1.5)
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
				half4 _Params;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = pixelate(_MainTex, i.uv, _Params.x, _Params.y) * _Params.z;
					half2 coord = i.uv * half2(_Params.x, _Params.x / _Params.y);
					half2 mv = abs(sin(coord * PI)) * _Params.w;
					half s = mv.x * mv.y;
					half c = step(s, 1.0);
					color = ((1 - c) * color) + ((color * s) * c);
					return color;
				}

			ENDCG
		}
	}

	FallBack off
}
