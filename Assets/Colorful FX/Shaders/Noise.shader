// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Noise"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Seed (X) Strength (Y) Lum Contribution (Z)", Vector) = (0, 0, 0, 0)
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half3 _Params;

		half4 frag_mono(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			float n = simpleNoise(i.uv + _Params.x) * 2.0;
			return lerp(color, color * n, _Params.y);
		}

		half4 frag_colored(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			float n = simpleNoise_fracLess(i.uv + _Params.x);
			float nr = frac(n) * 2.0;
			float ng = frac(n * 1.2154) * 2.0;
			float nb = frac(n * 1.3453) * 2.0;
			float na = frac(n * 1.3647) * 2.0;
			return lerp(color, color * half4(nr, ng, nb, na), _Params.y);
		}

		half4 frag_mono_lum(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			float n = simpleNoise(i.uv + _Params.x) * 2.0;
			half lum = luminance(color.rgb);
			return lerp(color, color * n, _Params.y * (1.0 - lerp(0.0, lum, _Params.z)));
		}

		half4 frag_colored_lum(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			float n = simpleNoise_fracLess(i.uv + _Params.x);
			float nr = frac(n) * 2.0;
			float ng = frac(n * 1.2154) * 2.0;
			float nb = frac(n * 1.3453) * 2.0;
			float na = frac(n * 1.3647) * 2.0;
			half lum = luminance(color.rgb);
			return lerp(color, color * half4(nr, ng, nb, na), _Params.y * (1.0 - lerp(0.0, lum, _Params.z)));
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Monochrome
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_mono
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (1) Colored
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_colored
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (2) Monochrome - Lum Contrib
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_mono_lum
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (3) Colored - Lum Contrib
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_colored_lum
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}
	}

	FallBack off
}
