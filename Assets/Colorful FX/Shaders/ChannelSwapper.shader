// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Channel Swapper"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Red ("Red Source Channel (RGB)", Vector) = (1, 0, 0, 0)
		_Green ("Green Source Channel (RGB)", Vector) = (0, 1, 0, 0)
		_Blue ("Blue Source Channel (RGB)", Vector) = (0, 0, 1, 0)
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
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half4 _Red;
				half4 _Green;
				half4 _Blue;

				half4 frag(v2f_img i) : SV_Target
				{
					half3 color = tex2D(_MainTex, i.uv).rgb;
					
					half3 red = color * _Red.rgb;
					half3 green = color * _Green.rgb;
					half3 blue = color * _Blue.rgb;

					half3 result = half3(
						red.x + red.y + red.z,
						green.x + green.y + green.z,
						blue.x + blue.y + blue.z
					);

					return half4(result, 1.0);
				}

			ENDCG
		}
	}

	FallBack off
}
