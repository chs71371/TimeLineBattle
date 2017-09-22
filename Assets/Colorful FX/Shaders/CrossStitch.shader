// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Cross Stitch"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_StitchSize ("Stitch Size (Int)", Float) = 8
		_Brightness ("Brightness (Float)", Float) = 1.5
		_Scale ("Scale (Float)", Float) = 1.0
		_Ratio ("Ratio (Float)", Float) = 1.0
	}

	CGINCLUDE
	
		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half _StitchSize;
		half _Brightness;
		half _Scale;
		half _Ratio;

		half2 stitch(half2 uv)
		{
			half2 pixelUV = uv * _ScreenParams.xy;
			half2 offset = floor(pixelUV);
			offset.x = offset.x - offset.y;
			offset.y = offset.x + offset.y * 2.0;
			return fmod(offset, half2(_StitchSize, _StitchSize));
		}

		half4 frag(v2f_img i) : SV_Target
		{
			half2 reminder = stitch(i.uv);
			half4 color = (reminder.x == 0 || reminder.y == 0) ? tex2D(_MainTex, i.uv) * _Brightness : half4(0.0, 0.0, 0.0, 1.0);
			return color;
		}

		half4 frag_invert(v2f_img i) : SV_Target
		{
			half2 reminder = stitch(i.uv);
			half4 color = (reminder.x == 0 || reminder.y == 0) ? half4(0.0, 0.0, 0.0, 1.0) : tex2D(_MainTex, i.uv) * _Brightness;
			return color;
		}

		half4 frag_px(v2f_img i) : SV_Target
		{
			half2 reminder = stitch(i.uv);
			half4 color = (reminder.x == 0 || reminder.y == 0) ? pixelate(_MainTex, i.uv, _Scale, _Ratio) * _Brightness : half4(0.0, 0.0, 0.0, 1.0);
			return color;
		}

		half4 frag_invert_px(v2f_img i) : SV_Target
		{
			half2 reminder = stitch(i.uv);
			half4 color = (reminder.x == 0 || reminder.y == 0) ? half4(0.0, 0.0, 0.0, 1.0) : pixelate(_MainTex, i.uv, _Scale, _Ratio) * _Brightness;
			return color;
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}

		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_invert
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}

		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_px
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}

		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_invert_px
				#pragma fragmentoption ARB_precision_hint_fastest 

			ENDCG
		}
	}

	FallBack off
}
