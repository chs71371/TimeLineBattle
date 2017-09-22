// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/DynamicLookup"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_White ("White point (RGB)", Color) = (1, 1, 1, 0)
		_Black ("Black point (RGB)", Color) = (0, 0, 0, 0)
		_Red ("Red point (RGB)", Color) = (1, 0, 0, 0)
		_Green ("Green point (RGB)", Color) = (0, 1, 0, 0)
		_Blue ("Blue point (RGB)", Color) = (0, 0, 1, 0)
		_Yellow ("Yellow point (RGB)", Color) = (1, 1, 0, 0)
		_Magenta ("Magenta point (RGB)", Color) = (1, 0, 1, 0)
		_Cyan ("Cyan point (RGB)", Color) = (0, 1, 1, 0)
		_Amount ("Amount (Float)", Float) = 1
	}

	CGINCLUDE
	
		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half3 _White;
		half3 _Black;
		half3 _Red;
		half3 _Green;
		half3 _Blue;
		half3 _Yellow;
		half3 _Magenta;
		half3 _Cyan;
		half _Amount;

		half3 get_abcd(half4 color)
		{
			half3 ta = lerp(_Black, _Red, color.r);
			half3 tb = lerp(_Green, _Yellow, color.r);
			half3 tc = lerp(_Blue, _Magenta, color.r);
			half3 td = lerp(_Cyan, _White, color.r);
			half3 ab = lerp(ta, tb, color.g);
			half3 cd = lerp(tc, td, color.g);
			return lerp(ab, cd, color.b);
		}

		half4 frag(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half3 abcd = get_abcd(color);
			return lerp(color, half4(abcd, 1.0), _Amount);
		}

		half4 frag_linear(v2f_img i) : SV_Target
		{
			half4 color = sRGB(tex2D(_MainTex, i.uv));
			half3 abcd = get_abcd(color);
			return Linear(lerp(color, half4(abcd, 1.0), _Amount));
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

			ENDCG
		}

		// (1) Linear
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_linear
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}
	}

	FallBack off
}
