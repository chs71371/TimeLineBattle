// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/SCurveContrast"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Red ("Steepness (X) Gamma (Y)", Vector) = (0, 0, 0, 0)
		_Green ("Steepness (X) Gamma (Y)", Vector) = (0, 0, 0, 0)
		_Blue ("Steepness (X) Gamma (Y)", Vector) = (0, 0, 0, 0)
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
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half2 _Red;
				half2 _Green;
				half2 _Blue;

				half curve(half o, half2 params)
				{
					half g = pow(2.0, params.x) * 0.5;
					half c = (o < 0.5) ? pow(o, params.x) * g : 1.0 - pow(1.0 - o, params.x) * g;
					return pow(c, params.y);
				}

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = saturate(tex2D(_MainTex, i.uv));
					half r = curve(color.r, _Red);
					half g = curve(color.g, _Green);
					half b = curve(color.b, _Blue);
					return half4(r, g, b, color.a);
				}

			ENDCG
		}
	}

	FallBack off
}
