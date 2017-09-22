// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Channel Clamper"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RedClamp ("Red Clamp (XY)", Vector) = (0.0, 1.0, 0.0, 0.0)
		_GreenClamp ("Green Clamp (XY)", Vector) = (0.0, 1.0, 0.0, 0.0)
		_BlueClamp ("Blue Clamp (XY)", Vector) = (0.0, 1.0, 0.0, 0.0)
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
				half2 _RedClamp;
				half2 _GreenClamp;
				half2 _BlueClamp;

				half4 frag(v2f_img i) : SV_Target
				{
					half3 color = tex2D(_MainTex, i.uv).rgb;
					color.r = clamp(color.r, _RedClamp.x, _RedClamp.y);
					color.g = clamp(color.g, _GreenClamp.x, _GreenClamp.y);
					color.b = clamp(color.b, _BlueClamp.x, _BlueClamp.y);
					return half4(color, 1.0);
				}

			ENDCG
		}
	}

	FallBack off
}
