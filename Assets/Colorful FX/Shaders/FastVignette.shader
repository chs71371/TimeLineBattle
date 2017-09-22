// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Fast Vignette"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Center (XY) Sharpness (Z) Darkness (W)", Vector) = (0.5, 0.5, 0.1, 0.3)
		_Color ("Vignette Color (RGB)", Color) = (0, 0, 0, 0)
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half4 _Params;
		half3 _Color;

		half4 frag(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);

			half d = distance(i.uv, _Params.xy);
			half multiplier = smoothstep(0.8, _Params.z * 0.799, d * (_Params.w + _Params.z));
			color.rgb *= multiplier;

			return color;
		}

		half4 frag_desat(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);

			half d = distance(i.uv, _Params.xy);
			half multiplier = smoothstep(0.8, _Params.z * 0.799, d * (_Params.w + _Params.z));
			color.rgb *= multiplier;

			half lum = luminance(color.rgb);
			half4 grayscale = half4(lum, lum, lum, color.a);

			return lerp(grayscale, color, multiplier);
		}

		half4 frag_rgb(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);

			half d = distance(i.uv, _Params.xy);
			half multiplier = smoothstep(0.8, _Params.z * 0.799, d * (_Params.w + _Params.z));
			half3 c = lerp(_Color, color.rgb, multiplier);

			return half4(c, color.a);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Classic
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}

		// (1) Desaturate
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_desat
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}

		// (2) Colored
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_rgb
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}
	}

	FallBack off
}
