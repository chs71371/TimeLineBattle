// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Halftone"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Scale (X) DotSize (Y) Smoothness (Z)", Vector) = (12, 1.35, 0.08)
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half4 _Params;
		half2 _Center;
		half4x4 _MatRot;

		half4 halftone(half2 fc, half2x2 m)
		{
			half2 g = floor(mul(m, fc) / _Params.x) * _Params.x;
			half2 smp = mul(g + 0.5 * _Params.x, m);
			half s = min(length(fc - smp) / (_Params.y * 0.5 * _Params.x), 1.0);
			half4 c = RGBtoCMYK(tex2D(_MainTex, (smp + _Center) / _ScreenParams.xy));
			return c + s;
		}

		half3 compute(v2f_img i)
		{
			half2 coord = (i.uv * _ScreenParams.xy) - _Center;
	
			half4 cmyk = half4(
					halftone(coord, half2x2(_MatRot[0])).x, // C
					halftone(coord, half2x2(_MatRot[1])).y, // M
					halftone(coord, half2x2(_MatRot[2])).z, // Y
					halftone(coord, half2x2(_MatRot[3])).w  // K
				);

			return CMYKtoRGB(smoothstep(0.8 - _Params.z, 0.8 + _Params.z, cmyk));
		}

		half4 frag(v2f_img i) : SV_Target
		{
			return half4(compute(i), 1.0);
		}

		half4 frag_desaturate(v2f_img i) : SV_Target
		{
			half3 rgb = compute(i);
			half lum = luminance(rgb);
			return half4(lum, lum, lum, 1.0);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0)
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				
			ENDCG
		}

		// (1) Desaturate
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_desaturate
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				
			ENDCG
		}
	}

	FallBack off
}
