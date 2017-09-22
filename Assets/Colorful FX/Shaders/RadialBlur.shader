// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Radial Blur"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Center ("Center Point", Vector) = (0.5, 0.5, 0.0, 0.0)
		_Params ("Strength (X) Samples (Y) Sharpness (Z) Darkness (W)", Vector) = (0.1, 10, 0.4, 0.35)
	}

	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half2 _Center;
		half4 _Params;

		half4 blur(half2 uv, half samples, half amount)
		{
			half2 coord = uv - _Center;
			half4 color = half4(0.0, 0.0, 0.0, 0.0);
			half scale;
			half factor = samples - 1;

			for (int i = 0; i < samples; i++)
			{
				scale = 1.0 + amount * (i / factor);
				color += tex2Dlod(_MainTex, half4(coord * scale + _Center, 0.0, 0.0));
			}

			color /= samples;
			return color;
		}

		half vignette(half2 uv)
		{
			half v = 1.0;
			half d = distance(uv, _Center);
			v *= smoothstep(0.8, _Params.z * 0.799, d * (_Params.w + _Params.z));
			return 1.0 - v;
		}

		half4 frag(v2f_img i) : SV_Target
		{
			return blur(i.uv, _Params.y, _Params.x);
		}

		half4 frag_vignette(v2f_img i) : SV_Target
		{
			return blur(i.uv, _Params.y, _Params.x * vignette(i.uv));
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) No vignette
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				#pragma glsl

			ENDCG
		}

		// (1) Vignette
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_vignette
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				#pragma glsl

			ENDCG
		}
	}

	FallBack off
}
