// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/shadows-midtones-highlights.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Shadows, Midtones, Highlights")]
	public class ShadowsMidtonesHighlights : BaseEffect
	{
		public enum ColorMode
		{
			LiftGammaGain = 0,
			OffsetGammaSlope = 1
		}

		[Tooltip("Color mode. The difference between these two modes is the way shadows are handled.")]
		public ColorMode Mode = ColorMode.LiftGammaGain;

		[Tooltip("Adds density or darkness, raises or lowers the shadow levels with its alpha value and offset the color balance in the dark regions with the hue point.")]
		public Color Shadows = new Color(1f, 1f, 1f, 0.5f);

		[Tooltip("Shifts the middle tones to be brighter or darker. For instance, to make your render more warm, just move the midtone color toward the yellow/red range. The more saturated the color is, the warmer the render becomes.")]
		public Color Midtones = new Color(1f, 1f, 1f, 0.5f);

		[Tooltip("Brightens and tints the entire render but mostly affects the highlights.")]
		public Color Highlights = new Color(1f, 1f, 1f, 0.5f);

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			float multiplier;
			Material.SetVector("_Shadows", Shadows * (Shadows.a * 2));
			multiplier = 1f + (1f - (Midtones.r * 0.299f + Midtones.g * 0.587f + Midtones.b * 0.114f));
			Material.SetVector("_Midtones", (Midtones * multiplier) * (Midtones.a * 2f));
			multiplier = 1f + (1f - (Highlights.r * 0.299f + Highlights.g * 0.587f + Highlights.b * 0.114f));
			Material.SetVector("_Highlights", (Highlights * multiplier) * (Highlights.a * 2f));

			Material.SetFloat("_Amount", Amount);

			Graphics.Blit(source, destination, Material, (int)Mode);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Shadows Midtones Highlights";
		}
	}
}
