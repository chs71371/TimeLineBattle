// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/LensDistortionBlur"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Samples (X) Distortion (Y) Cubic Distortion (Z) Scale (W)", Vector) = (0, 0, 0, 0)
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
				#include "./Colorful.cginc"

				sampler2D _MainTex;
				half4 _Params;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = half4(0.0, 0.0, 0.0, 0.0);

					for (int k = 0; k < _Params.x; k++)
						color += tex2Dlod(_MainTex, half4(barrelDistortion(i.uv, k * _Params.y, k * _Params.z, _Params.w), 0.0, 0.0));

					return color / _Params.x;
				}

			ENDCG
		}
	}

	FallBack off
}
