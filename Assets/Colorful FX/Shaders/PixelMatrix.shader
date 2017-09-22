// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/PixelMatrix"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Size (X) Red Offset (Y) Green Offset (Z) Brightness (W)", Vector) = (9, 3, 3, 1.4)
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half4 _Params;

		half4 compute(v2f_img i)
		{
			half2 coord = i.uv * _ScreenParams.xy;
			half2 p = floor(coord.xy / _Params.x) * _Params.x;
			int2 offset = int2(mod(coord, _Params.x));

			half3 pixel = tex2D(_MainTex, p / _ScreenParams.xy).rgb;
			half3 color = half3(0.0, 0.0, 0.0);

			if (offset.x < _Params.y)
				color.r = pixel.r;
			else if (offset.x < _Params.z)
				color.g = pixel.g;
			else
				color.b = pixel.b;

			return half4(color, offset.y);
		}

		half4 frag(v2f_img i) : SV_Target
		{
			half4 c = compute(i);
			return half4(c.rgb * _Params.w, 1.0);
		}

		half4 frag_border(v2f_img i) : SV_Target
		{
			half4 c = compute(i);
			c.rgb *= 1.0 - step(_Params.x - 1.0, c.w);
			return half4(c.rgb * _Params.w, 1.0);
		}

	ENDCG

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

			ENDCG
		}

		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_border
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}
	}

	FallBack off
}
