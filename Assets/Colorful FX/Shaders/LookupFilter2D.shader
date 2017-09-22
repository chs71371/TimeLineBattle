// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Lookup Filter 2D"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LookupTex ("Lookup (RGB)", 2D) = "white" {}
		_Params1 ("Scale (XY) Offset (Z)", Vector) = (0, 0, 0, 0)
		_Params2 ("Amount (Z) PixelSize (W)", Vector) = (1, 0, 0, 0)
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"
			
		sampler2D _MainTex;
		sampler2D _LookupTex;
		
		half3 _Params1;
		half2 _Params2;
		
		half3 internal_tex3d(sampler2D tex, half3 uv)
		{
			uv.y = 1.0 - uv.y;
			uv.z *= _Params1.z;
			float shift = floor(uv.z);
			uv.xy = uv.xy * _Params1.z * _Params1.xy + 0.5 * _Params1.xy;
			uv.x += shift * _Params1.y;
			uv.xyz = lerp(tex2D(tex, uv.xy).rgb, tex2D(tex, uv.xy + float2(_Params1.y, 0)).rgb, uv.z - shift);
			return uv;
		}
		
		half3 internal_tex3d_nn(sampler2D tex, half3 uv)
		{
			uv.y = 1.0 - uv.y;
			uv.z *= _Params1.z;
			float shift = floor(uv.z);
			uv.xy = uv.xy * _Params1.z * _Params1.xy + 0.5 * _Params1.xy;
			uv.x += shift * _Params1.y;
			uv.xyz = lerp(tex2D(tex, uv.xy).rgb, tex2D(tex, uv.xy + float2(_Params1.y, 0)).rgb, step(0.5, uv.z - shift));
			return uv;
		}

		inline half4 lookup_gamma(half2 uv)
		{
			half4 c = saturate(tex2D(_MainTex, uv));
			half4 o = c;
			o.rgb = internal_tex3d(_LookupTex, c.rgb);
			return lerp(c, o, _Params2.x);
		}

		inline half4 lookup_linear(half2 uv)
		{
			half4 c = saturate(tex2D(_MainTex, uv));
			half4 o = c;
			o.rgb = sRGB(c.rgb);
			o.rgb = internal_tex3d(_LookupTex, o.rgb);
			return lerp(c, Linear(o), _Params2.x);
		}

		inline half4 lookup_gamma_nn(half2 uv)
		{
			half4 c = saturate(tex2D(_MainTex, uv));
			half4 o = c;
			o.rgb = internal_tex3d_nn(_LookupTex, c.rgb);
			return lerp(c, o, _Params2.x);
		}

		inline half4 lookup_linear_nn(half2 uv)
		{
			half4 c = saturate(tex2D(_MainTex, uv));
			half4 o = c;
			o.rgb = sRGB(c.rgb);
			o.rgb = internal_tex3d_nn(_LookupTex, o.rgb);
			return lerp(c, Linear(o), _Params2.x);
		}

		inline half2 px(half2 uv)
		{
			half2 div = half2(_ScreenParams.x * _Params2.y / _ScreenParams.y, _Params2.y);
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

		half4 frag_nn(v2f_img i) : SV_Target 
		{
			return lookup_gamma_nn(i.uv);
		}

		half4 frag_linear_nn(v2f_img i) : SV_Target
		{
			return lookup_linear_nn(i.uv);
		}

		half4 frag_px_nn(v2f_img i) : SV_Target 
		{
			return lookup_gamma_nn(px(i.uv));
		}

		half4 frag_linear_px_nn(v2f_img i) : SV_Target
		{
			return lookup_linear_nn(px(i.uv));
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// ------------------------------------------------------------------
		// Bilinear

		// (0) Gamma
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragment frag
			ENDCG
		}

		// (1) Linear
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragment frag_linear
			ENDCG
		}

		// (2) Gamma pixelized
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragment frag_px
			ENDCG
		}

		// (3) Linear pixelized
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragment frag_linear_px
				#pragma target 3.0
			ENDCG
		}

		// ------------------------------------------------------------------
		// Nearest Neighbor

		// (4) Gamma
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragment frag_nn
			ENDCG
		}

		// (5) Linear
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragment frag_linear_nn
			ENDCG
		}

		// (6) Gamma pixelized
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragment frag_px_nn
			ENDCG
		}

		// (7) Linear pixelized
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragment frag_linear_px_nn
				#pragma target 3.0
			ENDCG
		}
	}

	FallBack off
}
