// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Strokes"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params1 ("Amplitude (X) Frequency (Y) Scaling (Z) Max Thickness (W)", Vector) = (0, 0, 0, 0)
		_Params2 ("Red Luminance (X) Green Luminance (Y) Blue Luminance (Z)", Vector) = (0, 0, 0, 0)
		_Params3 ("Threshold (X) Harshness (Y)", Vector) = (0, 0, 0, 0)
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half4 _Params1;
		half3 _Params2;
		half2 _Params3;

		void get_stroke(half2 uv, out half4 color, out half stroke)
		{
			half2 screenUV = uv * _ScreenParams.xy;
			half2 xy = screenUV / _ScreenParams.yy;

			half3 pattern[6] = {
				half3(-0.707, 0.707, 3.0),
				half3(   0.0,   1.0, 0.6),
				half3(   0.0,   1.0, 0.5),
				half3(   1.0,   0.0, 0.4),
				half3(   1.0,   0.0, 0.3),
				half3(   0.0,   1.0, 0.2)
			};

			color = tex2D(_MainTex, half2(screenUV.x / _ScreenParams.x, xy.y));
			stroke = 1.0;
			
			for(int i = 0; i < 6; i++)
			{
				half2 pt = half2(
					xy.x * pattern[i].x - xy.y * pattern[i].y,
					xy.x * pattern[i].y + xy.y * pattern[i].x
				);

				half thickness = _Params1.w * half(i + 1.0);
				half dist = mod(pt.y + thickness * 0.5 - sin(pt.x * _Params1.y) * _Params1.x, _Params1.z);
				half lum = dot(color.rgb, _Params2.xyz);

				if(dist < thickness && lum < _Params3.x - 0.12 * half(i))
				{
					half k = pattern[i].z - _Params3.y;
					half x = (thickness - dist) / thickness;
					stroke = min(1.0 - 0.5 / k + abs((x - 0.5) / k), stroke);
				}
			}
		}

		half4 frag_black_white(v2f_img i) : SV_Target
		{
			half4 color; half stroke;
			get_stroke(i.uv, color, stroke);
			return half4(stroke.xxx, 1.0);
		}

		half4 frag_white_black(v2f_img i) : SV_Target
		{
			half4 color; half stroke;
			get_stroke(i.uv, color, stroke);
			stroke = 1.0 - stroke;
			return half4(stroke.xxx, 1.0);
		}

		half4 frag_color_white(v2f_img i) : SV_Target
		{
			half4 color; half stroke;
			get_stroke(i.uv, color, stroke);
			half one_minus_stroke = 1.0 - stroke;
			return half4(stroke.xxx, 1.0) + half4(one_minus_stroke.xxx, 1.0) * color;
		}

		half4 frag_color_black(v2f_img i) : SV_Target
		{
			half4 color; half stroke;
			get_stroke(i.uv, color, stroke);
			stroke = 1.0 - stroke;
			return half4(stroke.xxx, 1.0) * color;
		}

		half4 frag_white_color(v2f_img i) : SV_Target
		{
			half4 color; half stroke;
			get_stroke(i.uv, color, stroke);
			half one_minus_stroke = 1.0 - stroke;
			return half4(one_minus_stroke.xxx, 1.0) + half4(stroke.xxx, 1.0) * color;
		}

		half4 frag_black_color(v2f_img i) : SV_Target
		{
			half4 color; half stroke;
			get_stroke(i.uv, color, stroke);
			return half4(stroke.xxx, 1.0) * color;
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Black and white
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_black_white
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				
			ENDCG
		}

		// (1) White and black
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_white_black
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				
			ENDCG
		}

		// (2) Color and white
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_color_white
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				
			ENDCG
		}

		// (3) Color and black
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_color_black
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				
			ENDCG
		}

		// (4) White and color
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_white_color
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				
			ENDCG
		}

		// (5) Black and color
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_black_color
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				
			ENDCG
		}
	}

	FallBack off
}
