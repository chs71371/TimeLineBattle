// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Shadows Midtones Highlights"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Shadows ("Shadows (RGB)", Vector) = (1, 1, 1, 1)
		_Midtones ("Midtones (RGB)", Vector) = (1, 1, 1, 1)
		_Highlights ("Highlights (RGB)", Vector) = (1, 1, 1, 1)
		_Amount ("Amount (Float)", Range(0, 1)) = 1
	}

	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half4 _Shadows;			// Lift | Offset
		half4 _Midtones;		// Gamma
		half4 _Highlights;		// Gain | Slope
		half _Amount;

		half4 frag_lgg(v2f_img i) : SV_Target
		{
			half3 oc = tex2D(_MainTex, i.uv).rgb;
			half3 color = oc + (_Shadows.rgb * 0.5 - 0.5) * (1.0 - oc);
			color = saturate(color);
			color *= _Highlights.rgb;
			color = pow(color, 1.0 / _Midtones.rgb);
			color = saturate(color);
			return half4(lerp(oc, color, _Amount), 1.0);
		}

		half4 frag_cdl(v2f_img i) : SV_Target
		{
			half3 oc = tex2D(_MainTex, i.uv).rgb;
			half3 color = oc * _Highlights.rgb;
			color = saturate(color);
			color = color + (_Shadows.rgb * 0.5 - 0.5);
			color = saturate(color);
			color = pow(color, 1.0 / _Midtones.rgb);
			color = saturate(color);
			return half4(lerp(oc, color, _Amount), 1.0);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) LGG
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_lgg
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}

		// (1) CDL
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_cdl
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}
	}

	FallBack off
}
