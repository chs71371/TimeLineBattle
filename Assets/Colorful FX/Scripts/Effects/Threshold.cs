// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/threshold.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Threshold")]
	public class Threshold : BaseEffect
	{
		[Range(1f, 255f), Tooltip("Luminosity threshold.")]
		public float Value = 128f;

		[Range(0f, 128f), Tooltip("Aomunt of randomization.")]
		public float NoiseRange = 24f;

		[Tooltip("Adds some randomization to the threshold value.")]
		public bool UseNoise = false;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetFloat("_Threshold", Value / 255f);
			Material.SetFloat("_Range", NoiseRange / 255f);
			Graphics.Blit(source, destination, Material, UseNoise ? 1 : 0);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Threshold";
		}
	}
}
