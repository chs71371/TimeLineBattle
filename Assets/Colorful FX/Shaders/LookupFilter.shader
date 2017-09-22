// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Lookup Filter (Deprecated)"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LookupTex ("Lookup (RGB)", 2D) = "white" {}
		_Amount ("Amount (Float)", Range(0.0, 1.0)) = 1.0
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"
		
		sampler2D _MainTex;
		sampler2D _LookupTex;
		half _Amount;

		half4 LUT(half4 color)
		{
			half blue = color.b * 63.0;

			half2 quad1 = half2(0.0, 0.0);
			quad1.y = floor(floor(blue) * 0.125);
			quad1.x = floor(blue) - quad1.y * 8.0;

			half2 quad2 = half2(0.0, 0.0);
			quad2.y = floor(ceil(blue) * 0.125);
			quad2.x = ceil(blue) - quad2.y * 8.0;

			half c1 = 0.0009765625 + (0.123046875 * color.r);
			half c2 = 0.0009765625 + (0.123046875 * color.g);

			half2 texPos1 = half2(0.0, 0.0);
			texPos1.x = quad1.x * 0.125 + c1;
			texPos1.y = -(quad1.y * 0.125 + c2);

			half2 texPos2 = half2(0.0, 0.0);
			texPos2.x = quad2.x * 0.125 + c1;
			texPos2.y = -(quad2.y * 0.125 + c2);

			half4 newColor = lerp(tex2D(_LookupTex, texPos1),
								  tex2D(_LookupTex, texPos2),
								  frac(blue));
			newColor.a = color.a;
			return lerp(color, newColor, _Amount);
		}

		half4 frag(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			return LUT(saturate(color));
		}

		half4 frag_linear(v2f_img i) : SV_Target
		{
			half4 color = sRGB(tex2D(_MainTex, i.uv));
			return Linear(LUT(saturate(color)));
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Gamma
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma exclude_renderers flash

			ENDCG
		}

		// (1) Linear
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_linear
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma exclude_renderers flash

			ENDCG
		}
	}

	FallBack off
}
