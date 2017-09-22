// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Bleach Bypass"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Amount ("Amount", Range(0.0, 1.0)) = 1.0
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
				half _Amount;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tex2D(_MainTex, i.uv);

					half lum = luminance(color.rgb);
					half3 blend = half3(lum, lum, lum);
					half L = min(1.0, max(0.0, 10.0 * (lum - 0.45)));
					half3 nc = lerp(2.0 * color.rgb * blend,
									1.0 - 2.0 * (1.0 - blend) * (1.0 - color.rgb),
									L);

					return lerp(color, half4(nc, color.a), _Amount);
				}

			ENDCG
		}
	}

	FallBack off
}
