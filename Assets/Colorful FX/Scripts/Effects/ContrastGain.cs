// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/contrast-gain.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Contrast Gain")]
	public class ContrastGain : BaseEffect
	{
		[Range(0.001f, 2f), Tooltip("Steepness of the contrast curve. 1 is linear, no contrast change.")]
		public float Gain = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetFloat("_Gain", Gain);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Contrast Gain";
		}
	}
}
