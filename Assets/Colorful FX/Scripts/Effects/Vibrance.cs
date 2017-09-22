// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/vibrance.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Vibrance")]
	public class Vibrance : BaseEffect
	{
		[Range(-100f, 100f), Tooltip("Adjusts the saturation so that clipping is minimized as colors approach full saturation.")]
		public float Amount = 0.0f;

		[Range(-5f, 5f)]
		public float RedChannel = 1.0f;
		[Range(-5f, 5f)]
		public float GreenChannel = 1.0f;
		[Range(-5f, 5f)]
		public float BlueChannel = 1.0f;

		public bool AdvancedMode = false;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			if (AdvancedMode)
			{
				Material.SetFloat("_Amount", Amount * 0.01f);
				Material.SetVector("_Channels", new Vector3(RedChannel, GreenChannel, BlueChannel));
				Graphics.Blit(source, destination, Material, 1);
			}
			else
			{
				Material.SetFloat("_Amount", Amount * 0.02f);
				Graphics.Blit(source, destination, Material, 0);
			}
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Vibrance";
		}
	}
}
