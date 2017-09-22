// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Contrast Vignette"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Sharpness (X) Darkness (Y) Contrast (Z) Edge (W)", Vector) = (0.1, 0.3, 0.25, 0.5)
		_Coeffs ("Luminance coeffs (RGB)", Vector) = (0.5, 0.5, 0.5, 1.0)
		_Center ("Center point (XY)", Vector) = (0.5, 0.5, 1.0, 1.0)
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
				half4 _Params;
				half3 _Coeffs;
				half4 _Center;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tex2D(_MainTex, i.uv);

					half d = distance(i.uv, _Center.xy);
					half multiplier = smoothstep(0.8, _Params.x * 0.799, d * (_Params.y + _Params.x));
					color.rgb = (color.rgb - _Coeffs) * max((1.0 - _Params.z * (multiplier - 1.0) - _Params.w), 1.0) + _Coeffs;
					color.rgb *= multiplier;

					return color;
				}

			ENDCG
		}
	}

	FallBack off
}
