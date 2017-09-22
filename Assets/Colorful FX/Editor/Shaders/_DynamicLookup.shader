// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Editor/_DynamicLookup"
{
	SubShader
	{
		Pass
		{
			CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag

				struct vInput
				{
					float4 pos : POSITION;
					float4 color : COLOR;
				};

				struct fInput
				{
					float4 pos : SV_POSITION;
					float4 color : COLOR;
				};

				fInput vert(vInput i)
				{
					fInput o;
					o.pos = UnityObjectToClipPos(i.pos);
					o.color = i.color;
					return o;
				}

				half4 frag(fInput i) : SV_Target
				{
					return half4(i.color.rgb, 1.0);
				}

			ENDCG
		}
	}
}
