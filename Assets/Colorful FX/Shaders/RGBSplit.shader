// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/RGB Split"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Params ("Amount (X) Angle Sin (Y) Angle Cos (Z)", Vector) = (0, 0, 0, 0)
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
				half3 _Params;

				half4 frag(v2f_img i) : SV_Target
				{
					half2 coords = i.uv;
					half  d = distance(coords, half2(0.5, 0.5));
					half  amount = _Params.x * d * 2;
					half2 offset = amount * half2(_Params.z, _Params.y);
					half  cr  = tex2D(_MainTex, coords + offset).r;
					half2 cga = tex2D(_MainTex, coords).ga;
					half  cb  = tex2D(_MainTex, coords - offset).b;

					// Stupid hack to make it work with d3d9 (CG compiler bug ?)
					return half4(cr + 0.0000001, cga.x + 0.0000002, cb + 0.0000003, cga.y);
				}

			ENDCG
		}
	}

	FallBack off
}
