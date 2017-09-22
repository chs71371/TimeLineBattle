// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Kuwahara"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_PSize ("Pixel Size (XY)", Vector) = (0,0,0,0)
	}

	CGINCLUDE
		#pragma vertex vert_img
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		#pragma target 3.0
		#pragma glsl
		#include "UnityCG.cginc"
	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		Pass
		{			
			CGPROGRAM
				#define RADIUS 1
				#include "./Kuwahara.cginc"
			ENDCG
		}

		Pass
		{			
			CGPROGRAM
				#define RADIUS 2
				#include "./Kuwahara.cginc"
			ENDCG
		}

		Pass
		{			
			CGPROGRAM
				#define RADIUS 3
				#include "./Kuwahara.cginc"
			ENDCG
		}

		Pass
		{			
			CGPROGRAM
				#define RADIUS 4
				#include "./Kuwahara.cginc"
			ENDCG
		}

		Pass
		{			
			CGPROGRAM
				#define RADIUS 5
				#include "./Kuwahara.cginc"
			ENDCG
		}

		Pass
		{			
			CGPROGRAM
				#define RADIUS 6
				#include "./Kuwahara.cginc"
			ENDCG
		}
	}

	FallBack off
}
