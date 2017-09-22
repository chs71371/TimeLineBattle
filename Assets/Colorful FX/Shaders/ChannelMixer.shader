// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Channel Mixer"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Red ("Red Channel", Vector) = (1, 0, 0, 1)
		_Green ("Green Channel", Vector) = (0, 1, 0, 1)
		_Blue ("Blue Channel", Vector) = (0, 0, 1, 1)
		_Constant ("Constant", Vector) = (0, 0, 0, 1)
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
				half4 _Constant;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = tex2D(_MainTex, i.uv);

					half3 result = (color.rrr * _Red)
								 + (color.ggg * _Green)
								 + (color.bbb * _Blue)
								 + _Constant;
					
					return half4(result, color.a);
				}

			ENDCG
		}
	}

	FallBack off
}
