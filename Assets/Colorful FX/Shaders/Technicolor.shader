// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Technicolor"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Exposure ("Exposure (Float)", Range(0.0, 8.0)) = 4.0
		_Balance ("Channel Balance (RGB)", Vector) = (0.75, 0.75, 0.75, 1.0)
		_Amount ("Amount (Float)", Range(0.0, 1.0)) = 0.5
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
				half _Exposure;
				half4 _Balance;
				half _Amount;

				half4 frag(v2f_img i) : SV_Target
				{
					half3 color = tex2D(_MainTex, i.uv).rgb;

					half3 balance = 1.0 / (_Balance.rgb * _Exposure);
					half2 rmul = color.rg * balance.r;
					half2 gmul = color.rg * balance.g;
					half2 bmul = color.rb * balance.b;
	
					half rneg = dot(half2(1.05, 0.62), rmul);
					half gneg = dot(half2(0.30, 1.0), gmul);
					half bneg = dot(half2(1.0, 1.05), bmul);
	
					half3 rout = rneg.rrr + half3(0.0, 1.3, 1.0);
					half3 gout = gneg.rrr + half3(1.0, 0.0, 1.05);
					half3 bout = bneg.rrr + half3(1.6, 1.6, 0.05);
	
					half3 result = rout * gout * bout;
					return half4(lerp(color, result, _Amount), 1.0);
				}

			ENDCG
		}
	}

	FallBack off
}
