// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/hue-saturation-value.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Hue, Saturation, Value")]
	public class HueSaturationValue : BaseEffect
	{
		[Range(-180f, 180f)]
		public float MasterHue = 0.0f;
		[Range(-100f, 100f)]
		public float MasterSaturation = 0.0f;
		[Range(-100f, 100f)]
		public float MasterValue = 0.0f;

		[Range(-180f, 180f)]
		public float RedsHue = 0.0f;
		[Range(-100f, 100f)]
		public float RedsSaturation = 0.0f;
		[Range(-100f, 100f)]
		public float RedsValue = 0.0f;

		[Range(-180f, 180f)]
		public float YellowsHue = 0.0f;
		[Range(-100f, 100f)]
		public float YellowsSaturation = 0.0f;
		[Range(-100f, 100f)]
		public float YellowsValue = 0.0f;

		[Range(-180f, 180f)]
		public float GreensHue = 0.0f;
		[Range(-100f, 100f)]
		public float GreensSaturation = 0.0f;
		[Range(-100f, 100f)]
		public float GreensValue = 0.0f;

		[Range(-180f, 180f)]
		public float CyansHue = 0.0f;
		[Range(-100f, 100f)]
		public float CyansSaturation = 0.0f;
		[Range(-100f, 100f)]
		public float CyansValue = 0.0f;

		[Range(-180f, 180f)]
		public float BluesHue = 0.0f;
		[Range(-100f, 100f)]
		public float BluesSaturation = 0.0f;
		[Range(-100f, 100f)]
		public float BluesValue = 0.0f;

		[Range(-180f, 180f)]
		public float MagentasHue = 0.0f;
		[Range(-100f, 100f)]
		public float MagentasSaturation = 0.0f;
		[Range(-100f, 100f)]
		public float MagentasValue = 0.0f;

		public bool AdvancedMode = false;

#if UNITY_EDITOR
		public int e_CurrentChannel = 0;
#endif

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Master", new Vector3(MasterHue / 360f, (MasterSaturation + 100f) * 0.01f, (MasterValue + 100f) * 0.01f));

			if (AdvancedMode)
			{
				Material.SetVector("_Reds", new Vector3(RedsHue / 360f, (RedsSaturation + 100f) * 0.01f, (RedsValue + 100f) * 0.01f));
				Material.SetVector("_Yellows", new Vector3(YellowsHue / 360f, (YellowsSaturation + 100f) * 0.01f, (YellowsValue + 100f) * 0.01f));
				Material.SetVector("_Greens", new Vector3(GreensHue / 360f, (GreensSaturation + 100f) * 0.01f, (GreensValue + 100f) * 0.01f));
				Material.SetVector("_Cyans", new Vector3(CyansHue / 360f, (CyansSaturation + 100f) * 0.01f, (CyansValue + 100f) * 0.01f));
				Material.SetVector("_Blues", new Vector3(BluesHue / 360f, (BluesSaturation + 100f) * 0.01f, (BluesValue + 100f) * 0.01f));
				Material.SetVector("_Magentas", new Vector3(MagentasHue / 360f, (MagentasSaturation + 100f) * 0.01f, (MagentasValue + 100f) * 0.01f));
				Graphics.Blit(source, destination, Material, 1);
			}
			else
			{
				Graphics.Blit(source, destination, Material, 0);
			}
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Hue Saturation Value";
		}
	}
}
