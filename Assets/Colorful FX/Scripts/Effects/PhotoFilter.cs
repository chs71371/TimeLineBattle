// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/photo-filter.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Photo Filter")]
	public class PhotoFilter : BaseEffect
	{
		[ColorUsage(false), Tooltip("Lens filter color.")]
		public Color Color = new Color(1.0f, 0.5f, 0.2f, 1.0f);

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Density = 0.35f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Density <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetColor("_RGB", Color);
			Material.SetFloat("_Density", Density);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Photo Filter";
		}
	}
}
