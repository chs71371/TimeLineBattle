// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Lookup Filter 3D"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LookupTex ("Lookup (RGB 3D)", 3D) = "white" {}
		_Params ("Scale (X) Offset (Y) Amount (Z) PixelSize (W)", Vector) = (0, 0, 1, 0)
	}

	CGINCLUDE
	
		#pragma exclude_renderers gles flash
		#include "UnityCG.cginc"
		#include "./Colorful.cginc"
			
		sampler2D _MainTex;
		sampler3D _LookupTex;

		half4 _Params;

		inline half4 lookup_gamma(half2 uv)
		{
			half4 c = saturate(tex2D(_MainTex, uv));
			half4 o = c;
			o.rgb = tex3D(_LookupTex, c.rgb * _Params.x + _Params.y).rgb;
			return lerp(c, o, _Params.z);
		}

		inline half4 lookup_linear(half2 uv)
		{
			half4 c = saturate(tex2D(_MainTex, uv));
			half4 o = c;
			o.rgb = sRGB(c.rgb);
			o.rgb = tex3D(_LookupTex, o.rgb * _Params.x + _Params.y).rgb;
			return lerp(c, Linear(o), _Params.z);
		}

		inline half2 px(half2 uv)
		{
			half2 div = half2(_ScreenParams.x * _Params.w / _ScreenParams.y, _Params.w);
			return floor(uv * div) / div;
		}

		half4 frag(v2f_img i) : SV_Target 
		{
			return lookup_gamma(i.uv);
		}

		half4 frag_linear(v2f_img i) : SV_Target
		{
			return lookup_linear(i.uv);
		}

		half4 frag_px(v2f_img i) : SV_Target 
		{
			return lookup_gamma(px(i.uv));
		}

		half4 frag_linear_px(v2f_img i) : SV_Target
		{
			return lookup_linear(px(i.uv));
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
				#pragma target 3.0
			ENDCG
		}

		// (1) Linear
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag_linear
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
			ENDCG
		}

		// (2) Gamma pixelized
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag_px
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
			ENDCG
		}

		// (3) Linear pixelized
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag_linear_px
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
			ENDCG
		}
	}

	FallBack off
}
