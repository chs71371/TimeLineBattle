// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/DirectionalBlur"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Direction (XY) Samples (Z)", Vector) = (0, 0, 0, 0)
	}

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
				#pragma target 3.0
				#pragma glsl
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half3 _Params;

				half4 frag(v2f_img i) : SV_Target
				{
					half2 dir = _Params.xy;
					half4 color = half4(0.0, 0.0, 0.0, 0.0);

					for (int k = -_Params.z; k < _Params.z; k++)
						color += tex2Dlod(_MainTex, half4(i.uv - dir * k, 0.0, 0.0));

					return color / (_Params.z * 2.0);
				}

			ENDCG
		}
	}

	FallBack off
}
