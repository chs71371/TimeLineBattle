// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/TV Vignette"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Size (X) Offset (Y)", Vector) = (0, 0, 0, 0)
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
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half2 _Params;

				half4 frag(v2f_img i) : SV_Target
				{
					half2 uv = -i.uv * i.uv + i.uv;
					half v = saturate(uv.x * uv.y * _Params.x + _Params.y);
					return v * tex2D(_MainTex, i.uv);
				}

			ENDCG
		}
	}

	FallBack off
}
