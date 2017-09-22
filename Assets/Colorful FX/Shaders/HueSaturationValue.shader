// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

Shader "Hidden/Colorful/Hue Saturation Value"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Master ("Master (HSV)", Vector) = (0, 0, 0, 0)
		_Reds ("Reds (HSV)", Vector) = (0, 0, 0, 0)
		_Yellows ("Yellows (HSV)", Vector) = (0, 0, 0, 0)
		_Greens ("Greens (HSV)", Vector) = (0, 0, 0, 0)
		_Cyans ("Cyans (HSV)", Vector) = (0, 0, 0, 0)
		_Blues ("Blues (HSV)", Vector) = (0, 0, 0, 0)
		_Magentas ("Magentas (HSV)", Vector) = (0, 0, 0, 0)
	}

	CGINCLUDE
	
		#include "UnityCG.cginc"
		#include "./Colorful.cginc"

		sampler2D _MainTex;
		half4 _Master;
		half4 _Reds;
		half4 _Yellows;
		half4 _Greens;
		half4 _Cyans;
		half4 _Blues;
		half4 _Magentas;

		half4 frag_simple(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);

			half3 hsv = RGBtoHSV(color.rgb);
			hsv.x = rot10(hsv.x + _Master.x);
			hsv.yz *= _Master.yz;

			return half4(HSVtoRGB(hsv), color.a);
		}

		half4 frag_advanced(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);

			// Master
			half3 hsv = RGBtoHSV(color.rgb);
			hsv.x = rot10(hsv.x + _Master.x);
			hsv.yz *= _Master.yz;

			half ts = 1.0 / 360.0;
			half ts60 = ts * 60.0;
			half4 c15_45_75_105 = half4(15.0, 45.0, 75.0, 105.0) * ts;
			half4 c135_165_195_225 = half4(135.0, 165.0, 195.0, 225.0) * ts;
			half4 c255_285_315_345 = half4(255.0, 285.0, 315.0, 345.0) * ts;
			
			half dr, dy, dg, dc, db, dm;

			// Reds
			hsv.x = rot10(hsv + ts60);
			dr = saturate(invlerp(c15_45_75_105.x, c15_45_75_105.y, hsv.x)) * (1.0 - saturate(invlerp(c15_45_75_105.z, c15_45_75_105.w, hsv.x)));
			hsv.x = rot10(hsv - ts60);
			
			// Yellow
			dy = saturate(invlerp(c15_45_75_105.x, c15_45_75_105.y, hsv.x)) * (1.0 - saturate(invlerp(c15_45_75_105.z, c15_45_75_105.w, hsv.x)));

			// Greens
			dg = saturate(invlerp(c15_45_75_105.z, c15_45_75_105.w, hsv.x)) * (1.0 - saturate(invlerp(c135_165_195_225.x, c135_165_195_225.y, hsv.x)));

			// Cyans
			dc = saturate(invlerp(c135_165_195_225.x, c135_165_195_225.y, hsv.x)) * (1.0 - saturate(invlerp(c135_165_195_225.z, c135_165_195_225.w, hsv.x)));

			// Blues
			db = saturate(invlerp(c135_165_195_225.z, c135_165_195_225.w, hsv.x)) * (1.0 - saturate(invlerp(c255_285_315_345.x, c255_285_315_345.y, hsv.x)));

			// Magentas
			dm = saturate(invlerp(c255_285_315_345.x, c255_285_315_345.y, hsv.x)) * (1.0 - saturate(invlerp(c255_285_315_345.z, c255_285_315_345.w, hsv.x)));

			hsv.x = rot10(hsv.x + dr * _Reds.x + dy * _Yellows.x + dg * _Greens.x + dc * _Cyans.x + db * _Blues.x + dm * _Magentas.x);
			hsv.y *= dr * _Reds.y + dy * _Yellows.y + dg * _Greens.y + dc * _Cyans.y + db * _Blues.y + dm * _Magentas.y;
			hsv.z *= dr * _Reds.z + dy * _Yellows.z + dg * _Greens.z + dc * _Cyans.z + db * _Blues.z + dm * _Magentas.z;

			return half4(HSVtoRGB(hsv), color.a);
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		// (0) Simple
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_simple
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma glsl
				#pragma exclude_renderers flash

			ENDCG
		}

		// (1) Advanced
		Pass
		{
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_advanced
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				#pragma glsl
				#pragma exclude_renderers flash

			ENDCG
		}
	}

	FallBack off
}
