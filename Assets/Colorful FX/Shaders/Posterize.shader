// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Posterize"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Levels (X) Amount (Y)", Vector) = (4, 1, 0, 0)
	}

	CGINCLUDE

		#pragma vertex vert_img
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		#include "UnityCG.cginc"
		#include "Colorful.cginc"

		sampler2D _MainTex;
		half2 _Params;

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Classic
		Pass
		{
			CGPROGRAM

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tex2D(_MainTex, i.uv);
					half4 posterized = floor(color * _Params.x) / _Params.x;
					return lerp(color, posterized, _Params.y);
				}

			ENDCG
		}

		// (1) Luminosity only
		Pass
		{
			CGPROGRAM

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tex2D(_MainTex, i.uv);
					half4 output = color;
					output.rgb = RGBtoYUV(output.rgb);
					output.x = floor(output.x * _Params.x) / _Params.x;
					output.rgb = YUVtoRGB(output.rgb);
					return lerp(color, output, _Params.y);
				}

			ENDCG
		}
	}

	FallBack off
}

