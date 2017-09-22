// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Brightness Contrast Gamma"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BCG ("Brightness (X) Contrast (Y) Gamma (Z)", Vector) = (0.0, 1.0, 1.0, 1.0)
		_Coeffs ("Contrast coeffs (RGB)", Vector) = (0.5, 0.5, 0.5, 1.0)
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

				sampler2D _MainTex;
				half4 _BCG;
				half3 _Coeffs;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tex2D(_MainTex, i.uv);
					half4 factor = half4(_Coeffs, color.a);
					
					color *= _BCG.x;
					color = (color - factor) * _BCG.y + factor;
					color = clamp(color, 0.0, 1.0);
					color = pow(color, _BCG.z);

					return color;
				}

			ENDCG
		}
	}

	FallBack off
}
