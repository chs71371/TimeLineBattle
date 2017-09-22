// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/posterize.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Posterize")]
	public class Posterize : BaseEffect
	{
		[Range(2, 255), Tooltip("Number of tonal levels (brightness values) for each channel.")]
		public int Levels = 16;

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1f;

		[Tooltip("Only affects luminosity. Use this if you don't want any hue shifting or color changes.")]
		public bool LuminosityOnly = false;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Params", new Vector2((float)Levels, Amount));
			Graphics.Blit(source, destination, Material, LuminosityOnly ? 1 : 0);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Posterize";
		}
	}
}
