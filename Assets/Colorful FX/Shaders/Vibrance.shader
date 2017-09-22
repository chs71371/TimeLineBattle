// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Vibrance"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Amount ("Amount", Float) = 0
		_Channels ("Channels", Vector) = (1.0, 1.0, 1.0, 0.0)
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half _Amount;
		half4 _Channels;

		half4 frag_simple(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);

			half cMax = max(max(color.r, color.g), color.b);
			half amount = (cMax - luminance(color.rgb)) * (-3.0 * _Amount);
			color.rgb = lerp(color.rgb, half3(cMax, cMax, cMax), amount);

			return color;
		}

		half4 frag_advanced(v2f_img i) : SV_Target
		{
			half4 oc = tex2D(_MainTex, i.uv);
			half3 color = oc.rgb;

			half3 coeff = _Channels.rgb * _Amount;
			half lum = luminance(color);
			half sat = max(color.r, max(color.g, color.b)) - min(color.r, min(color.g, color.b));
			color = lerp(half3(lum, lum, lum), color, (1.0 + (coeff * (1.0 - (sign(coeff) * sat)))));

			return half4(color, oc.a);
		}

	ENDCG

	SubShader
	{
		// (0) Simple
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_simple
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}

		// (1) Advanced
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_advanced
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}
	}

	FallBack off
}
