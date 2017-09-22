// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Bilateral Gaussian Blur"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Direction ("Direction (XY)", Vector) = (0, 0, 0, 0)
		_Threshold ("Threshold", Float) = 0
		_Amount ("Blend factor (Float)", Float) = 1.0
	}

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Blur
		Pass
		{

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#pragma exclude_renderers flash
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _CameraDepthTexture;
				half2 _Direction;
				half _Threshold;

				struct fInput
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float4 uv1 : TEXCOORD1;
					float4 uv2 : TEXCOORD2;
				};

				fInput vert(appdata_img v)
				{
					fInput o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord.xy;
					float2 d1 = 1.3846153846 * _Direction;
					float2 d2 = 3.2307692308 * _Direction;
					o.uv1 = float4(o.uv + d1, o.uv - d1);
					o.uv2 = float4(o.uv + d2, o.uv - d2);
					return o;
				}

				half4 frag(fInput i) : SV_Target
				{
					const half2 uvs[4] = { i.uv1.xy, i.uv1.zw, i.uv2.xy, i.uv2.zw };

					half depth = Linear01Depth(tex2D(_CameraDepthTexture, i.uv).x);
					half4 accum = tex2D(_MainTex, i.uv) * 0.2270270270;
					half accumWeight = 0.2270270270;

					half4 depthTmp;
					depthTmp.x = Linear01Depth(tex2D(_CameraDepthTexture, uvs[0]).x);
					depthTmp.y = Linear01Depth(tex2D(_CameraDepthTexture, uvs[1]).x);
					depthTmp.z = Linear01Depth(tex2D(_CameraDepthTexture, uvs[2]).x);
					depthTmp.w = Linear01Depth(tex2D(_CameraDepthTexture, uvs[3]).x);
					half4 diff = abs(depth - depthTmp);
					half4 weight = (1.0 - step(_Threshold, diff)) * half4(0.3162162162, 0.3162162162, 0.0702702703, 0.0702702703);

					for (int i = 0; i < 4; i++)
					{
						accum += weight[i] * tex2D(_MainTex, uvs[i]);
						accumWeight += weight[i];
					}

					return accum / accumWeight;
				}

			ENDCG
		}

		// (1) Composite if _Amount is in ]0;1[
		Pass
		{
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _MainTex_TexelSize;
				sampler2D _Blurred;
				half _Amount;

				half4 frag(v2f_img i) : SV_Target
				{
					half2 uv = i.uv;
					half4 oc = tex2D(_MainTex, uv);
					#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						uv.y = 1.0 - uv.y;
					#endif
					half4 bc = tex2D(_Blurred, uv);
					return lerp(oc, bc, _Amount);
				}

			ENDCG
		}
	}

	FallBack off
}
