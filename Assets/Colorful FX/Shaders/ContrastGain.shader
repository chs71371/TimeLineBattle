// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Contrast Gain"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Gain ("Contrast Gain (Float)", Float) = 1.0
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
				half _Gain;

				half4 frag(v2f_img i) : SV_Target
				{
					half4 color = saturate(tex2D(_MainTex, i.uv));
					half g = pow(2.0, _Gain) * 0.5;

					// The following code fails for some reason on Unity 4.6/DX9
					//color.rgb = (color.rgb < 0.5) ? pow(color.rgb, _Gain) * g : 1.0 - pow(1.0 - color.rgb, _Gain) * g;
					// So use a classic lerp/step combo instead
					half3 c1 = pow(color.rgb, _Gain) * g;
					half3 c2 = 1.0 - pow(1.0 - color.rgb, _Gain) * g;
					color.rgb = lerp(c1, c2, step(0.5, color.rgb));

					return color;
				}

			ENDCG
		}
	}

	FallBack off
}
