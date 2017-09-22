// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Analog TV"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params1 ("Noise Intensity (X) Scanlines Intensity (Y) Scanlines Count (Z) Scanlines Offset (W)", Vector) = (0.5, 2.0, 768, 0.0)
		_Params2 ("Phase (X) Distortion (Y) Cubic Distortion (Z) Scale (W)", Vector) = (0.05, 0.2, 0.6, 0.8)
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half4 _Params1;
		half4 _Params2;

		half4 filter_pass_h(half2 uv)
		{
			half2 coord = barrelDistortion(uv, _Params2.y, _Params2.z, _Params2.w);
			half4 color = tex2D(_MainTex, coord);

			float n = simpleNoise(coord.xy * _Params2.x);
			float dx = mod(n, 0.01);

			half3 result = color.rgb + color.rgb * saturate(0.1 + dx * 100.0);
			half2 sc = half2(sin(coord.y * _Params1.z + _Params1.w), cos(coord.y * _Params1.z + _Params1.w));
			result += color.rgb * sc.xyx * _Params1.y;
			result = color.rgb + saturate(_Params1.x) * (result - color.rgb);

			return half4(result, color.a);
		}

		half4 filter_pass_v(half2 uv)
		{
			half2 coord = barrelDistortion(uv, _Params2.y, _Params2.z, _Params2.w);
			half4 color = tex2D(_MainTex, coord);

			float n = simpleNoise(coord.xy * _Params2.x);
			float dx = mod(n, 0.01);

			half3 result = color.rgb + color.rgb * saturate(0.1 + dx * 100.0);
			half2 sc = half2(sin(coord.x * _Params1.z + _Params1.w), cos(coord.x * _Params1.z + _Params1.w));
			result += color.rgb * sc.xyx * _Params1.y;
			result = color.rgb + saturate(_Params1.x) * (result - color.rgb);

			return half4(result, color.a);
		}

		half4 frag(v2f_img i) : SV_Target
		{
			return filter_pass_h(i.uv);
		}

		half4 frag_grayscale(v2f_img i) : SV_Target
		{
			half4 result = filter_pass_h(i.uv);
			half lum = luminance(result.rgb);
			result = half4(lum, lum, lum, result.a);
			return result;
		}

		half4 frag_vertical(v2f_img i) : SV_Target
		{
			return filter_pass_v(i.uv);
		}

		half4 frag_grayscale_vertical(v2f_img i) : SV_Target
		{
			half4 result = filter_pass_v(i.uv);
			half lum = luminance(result.rgb);
			result = half4(lum, lum, lum, result.a);
			return result;
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Horizontal scanlines
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#pragma glsl

			ENDCG
		}

		// (1) Horizontal scanlines + grayscale
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_grayscale
				#pragma fragmentoption ARB_precision_hint_fastest 
				#pragma glsl

			ENDCG
		}

		// (2) Vertical scanlines
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_vertical
				#pragma fragmentoption ARB_precision_hint_fastest 
				#pragma glsl

			ENDCG
		}

		// (3) Vertical scanlines + grayscale
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_grayscale_vertical
				#pragma fragmentoption ARB_precision_hint_fastest 
				#pragma glsl

			ENDCG
		}
	}

	FallBack off
}
