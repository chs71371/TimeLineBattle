// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Dithering"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Pattern ("Dithering Pattern (A)", 2D) = "white" {}
		_Params ("Luminance (RGB) Amount (A)", Vector) = (0.30, 0.59, 0.11, 1.0)
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		sampler2D _Pattern;
		half4 _Params;

		half4 frag(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half4 dither = pow(tex2D(_Pattern, (i.uv * _ScreenParams.xy) / 8.0).aaaa, 0.454545);
			return lerp(color, step(dither, color), _Params.w);
		}

		half4 frag_linear(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half4 dither = tex2D(_Pattern, (i.uv * _ScreenParams.xy) / 8.0).aaaa;
			return lerp(color, step(dither, color), _Params.w);
		}

		half4 frag_bw(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half lum = dot(color.rgb, _Params.xyz);
			half4 grey = half4(lum.xxx, color.a);
			half4 dither = pow(tex2D(_Pattern, (i.uv * _ScreenParams.xy) / 8.0).aaaa, 0.454545);
			return lerp(color, step(dither, grey), _Params.w);
		}

		half4 frag_linear_bw(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half lum = dot(color.rgb, _Params.xyz);
			half4 grey = half4(lum.xxx, color.a);
			half4 dither = tex2D(_Pattern, (i.uv * _ScreenParams.xy) / 8.0).aaaa;
			return lerp(color, step(dither, grey), _Params.w);
		}

		half4 frag_orig(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half4 dither = pow(tex2D(_Pattern, (i.uv * _ScreenParams.xy) / 8.0).aaaa, 0.454545);
			return lerp(color, color + step(dither, color), _Params.w);
		}

		half4 frag_linear_orig(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half4 dither = tex2D(_Pattern, (i.uv * _ScreenParams.xy) / 8.0).aaaa;
			return lerp(color, color + step(dither, color), _Params.w);
		}

		half4 frag_bw_orig(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half lum = dot(color.rgb, _Params.xyz);
			half4 grey = half4(lum.xxx, color.a);
			half4 dither = pow(tex2D(_Pattern, (i.uv * _ScreenParams.xy) / 8.0).aaaa, 0.454545);
			return lerp(color, grey + step(dither, grey), _Params.w);
		}

		half4 frag_linear_bw_orig(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half lum = dot(color.rgb, _Params.xyz);
			half4 grey = half4(lum.xxx, color.a);
			half4 dither = tex2D(_Pattern, (i.uv * _ScreenParams.xy) / 8.0).aaaa;
			return lerp(color, grey + step(dither, grey), _Params.w);
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

		// (2) Gamma B&W
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_bw
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (3) Linear B&W
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_linear_bw
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (4) Gamma + original
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_orig
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (5) Linear + original
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_linear_orig
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (6) Gamma B&W + original
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_bw_orig
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (7) Linear B&W + original
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_linear_bw_orig
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}
	}

	FallBack off
}
