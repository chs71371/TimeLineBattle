// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/grayscale.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Grayscale")]
	public class Grayscale : BaseEffect
	{
		[Range(0f, 1f), Tooltip("Amount of red to contribute to the luminosity.")]
		public float RedLuminance = 0.299f;

		[Range(0f, 1f), Tooltip("Amount of green to contribute to the luminosity.")]
		public float GreenLuminance = 0.587f;

		[Range(0f, 1f), Tooltip("Amount of blue to contribute to the luminosity.")]
		public float BlueLuminance = 0.114f;

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_Params", new Vector4(RedLuminance, GreenLuminance, BlueLuminance, Amount));
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Grayscale";
		}
	}
}
