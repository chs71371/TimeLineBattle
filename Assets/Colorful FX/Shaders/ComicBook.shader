// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Comic Book"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half4 _StripParams;
		half3 _StripParams2;
		half3 _StripInnerColor;
		half3 _StripOuterColor;

		half3 _FillColor;
		half3 _BackgroundColor;
		
		half _EdgeThreshold;
		half3 _EdgeColor;

		struct fInput
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 uvs[4] : TEXCOORD1;
		};

		fInput vert_edge(appdata_img v)
		{
			fInput o;
			o.pos = UnityObjectToClipPos(v.vertex);
			float2 uv = v.texcoord.xy;
			o.uv = uv;
			o.uvs[0] = float4(uv.x - _EdgeThreshold, uv.y + _EdgeThreshold, uv.x - _EdgeThreshold, uv.y                 );
			o.uvs[1] = float4(uv.x - _EdgeThreshold, uv.y - _EdgeThreshold, uv.x                 , uv.y + _EdgeThreshold);
			o.uvs[2] = float4(uv.x                 , uv.y - _EdgeThreshold, uv.x + _EdgeThreshold, uv.y + _EdgeThreshold);
			o.uvs[3] = float4(uv.x + _EdgeThreshold, uv.y                 , uv.x + _EdgeThreshold, uv.y - _EdgeThreshold);
			return o;
		}

		half3 strip_color(half2 uv)
		{
			half2 p = (uv - 0.5) * _StripParams2.x;
			half brightness = cos(dot(p, _StripParams.xy));
			half lum_strip = luminance(1.0 - brightness);
			return lerp(_StripOuterColor, _StripInnerColor, step(lum_strip, _StripParams2.y));
		}

		half4 frag_edge(fInput i) : SV_Target
		{
			half3 color = _BackgroundColor;

			half3 sample0 = tex2D(_MainTex, i.uvs[0].xy).rgb;
			half3 sample1 = tex2D(_MainTex, i.uvs[0].zw).rgb;
			half3 sample2 = tex2D(_MainTex, i.uvs[1].xy).rgb;
			half3 sample3 = tex2D(_MainTex, i.uvs[1].zw).rgb;
			half3 sample4 = tex2D(_MainTex, i.uv).rgb;
			half3 sample5 = tex2D(_MainTex, i.uvs[2].xy).rgb;
			half3 sample6 = tex2D(_MainTex, i.uvs[2].zw).rgb;
			half3 sample7 = tex2D(_MainTex, i.uvs[3].xy).rgb;
			half3 sample8 = tex2D(_MainTex, i.uvs[3].zw).rgb;

			half3 hEdge = sample2 + sample5 + sample8 - (sample0 + sample3 + sample6);
			half3 vEdge = sample0 + sample1 + sample2 - (sample6 + sample7 + sample8);
			half3 edge = sqrt(hEdge * hEdge + vEdge * vEdge);
	
			if (edge.r > 0.5 || edge.g > 0.5 || edge.b > 0.5) // Flattened by Unity's shader compiler on DX9 and GL
			{
				color = _EdgeColor;
			}
			else
			{
				half lum = luminance(tex2D(_MainTex, i.uv).rgb);
				half s1 = step(lum, _StripParams.z);
				half s2 = step(_StripParams.z, lum) * step(lum, _StripParams.w);
				color = lerp(lerp(_BackgroundColor, strip_color(i.uv), s2), _FillColor, s1);
			}
    
			return half4(lerp(sample4, color, _StripParams2.z), 1.0);
		}

		half4 frag(v2f_img i) : SV_Target
		{
			half3 color = _BackgroundColor;

			half lum = luminance(tex2D(_MainTex, i.uv).rgb);
			half s1 = step(lum, _StripParams.z);
			half s2 = step(_StripParams.z, lum) * step(lum, _StripParams.w);
			color = lerp(lerp(_BackgroundColor, strip_color(i.uv), s2), _FillColor, s1);
	
			return half4(lerp(tex2D(_MainTex, i.uv).rgb, color, _StripParams2.z), 1.0);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) No edge detection
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		// (1) Edge detection
		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_edge
				#pragma fragment frag_edge
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0

			ENDCG
		}
	}

	FallBack off
}
