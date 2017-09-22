// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Blend"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_OverlayTex ("Overlay (RGB)", 2D) = "white" {}
		_Amount ("Amount", Range(0.0, 1.0)) = 1.0
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		#define half4_one half4(1.0, 1.0, 1.0, 1.0)

		sampler2D _MainTex;
		sampler2D _OverlayTex;
		half _Amount;

		// Darken
		half4 frag_darken(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, min(c, o), _Amount);
		}

		// Multiply
		half4 frag_multiply(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, c * o, _Amount);
		}

		// Color Burn
		half4 frag_colorburn(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, half4_one - (half4_one - c) / o, _Amount);
		}

		// Linear Burn
		half4 frag_linearburn(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, o + c - half4_one, _Amount);
		}

		// Darker Color
		half4 frag_darkercolor(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			half4 result = luminance(c.rgb) < luminance(o.rgb) ? c : o;
			return lerp(c, result, _Amount);
		}

		// Lighten
		half4 frag_lighten(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, max(c, o), _Amount);
		}

		// Screen
		half4 frag_screen(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, half4_one - ((half4_one - o) * (half4_one - c)), _Amount);
		}

		// Color Dodge
		half4 frag_colordodge(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, c / (half4_one - o), _Amount); // Should check for div by 0 but GPU drivers apparently don't care
		}

		// Linear Dodge (Add)
		half4 frag_add(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, c + o, _Amount);
		}

		// Lighter Color
		half4 frag_lightercolor(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			half4 result = luminance(c.rgb) > luminance(o.rgb) ? c : o;
			return lerp(c, result, _Amount);
		}

		// Overlay
		half4 frag_overlay(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			half4 check = step(0.5, c);
			half4 result = check * (half4_one - ((half4_one - 2.0 * (c - 0.5)) * (half4_one - o))); 
			result += (half4_one - check) * (2.0 * c * o);
			return lerp(c, result, _Amount);
		}

		// Soft Light
		half4 frag_softlight(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			half4 check = step(0.5, o);
			half4 result = check * (2.0 * c * o + c * c - 2.0 * c * c * o); 
			result += (half4_one - check) * (2.0 * sqrt(c) * o - sqrt(c) + 2.0 * c - 2.0 * c * o);
			return lerp(c, result, _Amount);
		}

		// Hard Light
		half4 frag_hardlight(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			half4 check = step(0.5, o);
			half4 result = check * (half4_one - ((half4_one - 2.0 * (c - 0.5)) * (half4_one - o))); 
			result += (half4_one - check) * (2.0 * c * o);
			return lerp(c, result, _Amount);
		}

		// Vivid Light
		half4 frag_vividlight(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			half4 check = step(0.5, o);
			half4 result = check * (c / (half4_one - 2.0 * (o - 0.5)));
			result += (half4_one - check) * (half4_one - (half4_one - c) / (2.0 * o));
			return lerp(c, result, _Amount);
		}

		// Linear Light
		half4 frag_linearlight(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			half4 check = step(0.5, o);
			half4 result = check * (c + (2.0 * (o - 0.5)));
			result += (half4_one - check) * (c + 2.0 * o - half4_one);
			return lerp(c, result, _Amount);
		}

		// Pin Light
		half4 frag_pinlight(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			half4 check = step(0.5, o);
			half4 result = check * max(2.0 * (o - 0.5), c);
			result += (half4_one - check) * min(2 * o, c);
			return lerp(c, result, _Amount);
		}

		// Hard Mix
		half4 frag_hardmix(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			half4 result = half4(0.0, 0.0, 0.0, 0.0);
			result.r = o.r > 1.0 - c.r ? 1.0 : 0.0;
			result.g = o.g > 1.0 - c.g ? 1.0 : 0.0;
			result.b = o.b > 1.0 - c.b ? 1.0 : 0.0;
			result.a = o.a > 1.0 - c.a ? 1.0 : 0.0;
			return lerp(c, result, _Amount);
		}

		// Difference
		half4 frag_difference(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, abs(c - o), _Amount);
		}

		// Exclusion
		half4 frag_exclusion(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, c + o - (2 * c * o), _Amount);
		}

		// Subtract
		half4 frag_subtract(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, saturate(c - o), _Amount);
		}

		// Divide
		half4 frag_divide(v2f_img i) : SV_Target
		{
			half4 o = tex2D(_OverlayTex, i.uv);
			half4 c = tex2D(_MainTex, i.uv);
			return lerp(c, c / o, _Amount);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Darken
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_darken
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (1) Multiply
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_multiply
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (2) Color Burn
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_colorburn
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (3) Linear Burn
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_linearburn
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (4) Darker Color
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_darkercolor
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// Separator
		Pass {}

		// (6) Lighten
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_lighten
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (7) Screen
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_screen
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (8) Color Dodge
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_colordodge
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (9) Linear Dodge (add)
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_add
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (10) Lighter Color
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_lightercolor
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// Separator
		Pass {}

		// (12) Overlay
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_overlay
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (13) Soft Light
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_softlight
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma exclude_renderers flash

			ENDCG
		}

		// (14) Hard Light
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_hardlight
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (15) Vivid Light
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_vividlight
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (16) Linear Light
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_linearlight
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (17) Pin Light
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_pinlight
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (18) Hard Mix
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_hardmix
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// Separator
		Pass {}

		// (20) Difference
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_difference
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (21) Exclusion
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_exclusion
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (22) Subtract
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_subtract
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (23) Divide
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_divide
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}
	}

	FallBack off
}
