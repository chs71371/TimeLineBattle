// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Hue Focus"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Range ("Hue Range (X) Min (Y) Max", Vector) = (-0.16, 0.16, 0.0, 0.0)
		_Params ("Hue (X) Boost (Y) Amount (Z)", Vector) = (0.0, 0.5, 1.0, 0.0)
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
				#pragma target 3.0
				#include "UnityCG.cginc"
				#include "./Colorful.cginc"

				sampler2D _MainTex;
				half2 _Range;
				half3 _Params;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = saturate(tex2D(_MainTex, i.uv));
					
					half3 target = color.rgb;
					half lum = luminance(target);
					half hue = RGBtoHUE(target);

					if (_Range.y > 1.0 && hue < _Range.y - 1.0) hue += 1.0;
					if (_Range.x < 0.0 && hue > _Range.x + 1.0) hue -= 1.0;
						
					target = (hue < _Params.x) ?
							 lerp(lum.xxx, target, smoothstep(_Range.x, _Params.x, hue) * _Params.y) :
							 lerp(lum.xxx, target, (1.0 - smoothstep(_Params.x, _Range.y, hue)) * _Params.y);

					color.rgb = lerp(color.rgb, target, _Params.z);
					return color;
				}

			ENDCG
		}
	}

	FallBack off
}
