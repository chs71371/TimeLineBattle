// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/GrainyBlur"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Radius (X) Samples (Y)", Vector) = (32, 16, 0, 0)
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
				half2 _Params;

				half4 frag(v2f_img i) : SV_Target
				{
					half p = _Params.x / _ScreenParams.y;
					half4 c = half4(0.0, 0.0, 0.0, 0.0);
					float r = sin(dot(i.uv, half2(1233.224, 1743.335)));
					half2 rv = half2(0.0, 0.0);
	
					for(int k = 0; k < int(_Params.y); k++)
					{
						r = frac(3712.65 * r + 0.61432);
						rv.x = (r - 0.5) * 2.0;
						r = frac(3712.65 * r + 0.61432);
						rv.y = (r - 0.5) * 2.0;
						c += tex2Dlod(_MainTex, half4(i.uv + rv * p, 0.0, 0.0));
					}

					return c / _Params.y;
				}

			ENDCG
		}
	}

	FallBack off
}
