// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Sharpen"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Strength (X) Clamp (Y) Pixel Size (ZW)", Vector) = (0.60, 0.05, 1, 1)
	}

	CGINCLUDE
	
		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half4 _Params;

		half4 frag_typeA(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);

			half2 p = _Params.zw;
			half2 p_h = p * 0.5;
			half4 blur = tex2D(_MainTex, i.uv + half2( p_h.x,   -p.y));
			blur += tex2D(_MainTex, i.uv + half2(  -p.x, -p_h.y));
			blur += tex2D(_MainTex, i.uv + half2(   p.x,  p_h.y));
			blur += tex2D(_MainTex, i.uv + half2(-p_h.x,    p.y));
			blur *= 0.25;

			half4 lumaStrength = half4(0.222, 0.707, 0.071, 0.0) * _Params.x * 0.666;
			half4 sharp = color - blur;
			color += clamp(dot(sharp, lumaStrength), -_Params.y, _Params.y);

			return color;
		}

		half4 frag_typeB(v2f_img i) : SV_Target
		{
			half2 p = _Params.zw;
			half4 blur = tex2D( _MainTex, i.uv + half2(-p.x, -p.y) * 1.5);
			blur += tex2D(_MainTex, i.uv + half2( p.x, -p.y) * 1.5);
			blur += tex2D(_MainTex, i.uv + half2(-p.x,  p.y) * 1.5);
			blur += tex2D(_MainTex, i.uv + half2( p.x,  p.y) * 1.5);
			blur *= 0.25;

			half4 center = tex2D(_MainTex, i.uv);
			return center + (center - blur) * _Params.x;
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Type A
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_typeA
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (1) Type B
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_typeB
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}
	}

	FallBack off
}
