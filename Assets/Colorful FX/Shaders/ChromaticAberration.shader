// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Chromatic Aberration"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Refraction ("Refraction Index (RGB)", Vector) = (1.0, 1.005, 1.01, 0.0)
	}

	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half3 _Refraction;

		half3 compute(half2 uv)
		{
			half3 incidence = half3(2.0 * uv - 1.0, 1.0);
			half3 normal = half3(0.0, 0.0, -1.0);
			half3 refract_r = refract(incidence, normal, _Refraction.r);
			half3 refract_g = refract(incidence, normal, _Refraction.g);
			half3 refract_b = refract(incidence, normal, _Refraction.b);
			half2 uv_r = ((refract_r / refract_r.z).xy + 1.0) / 2.0;
			half2 uv_g = ((refract_g / refract_g.z).xy + 1.0) / 2.0;
			half2 uv_b = ((refract_b / refract_b.z).xy + 1.0) / 2.0;
			return half3(tex2D(_MainTex, uv_r).r, tex2D(_MainTex, uv_g).g, tex2D(_MainTex, uv_b).b);
		}

		half4 frag(v2f_img i) : SV_Target
		{
			return half4(compute(i.uv), 1.0);
		}

		half4 frag_alpha(v2f_img i) : SV_Target
		{
			return half4(compute(i.uv), tex2D(_MainTex, i.uv).a);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Preserve alpha = false
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}

		// (1) Preserve alpha = true
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_alpha
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}
	}

	FallBack off
}
