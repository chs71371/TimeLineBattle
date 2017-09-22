// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/letterbox.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Letterbox")]
	public class Letterbox : BaseEffect
	{
		[Min(0f), Tooltip("Crop the screen to the given aspect ratio.")]
		public float Aspect = 21f / 9f;

		[Tooltip("Letter/Pillar box color. Alpha is transparency.")]
		public Color FillColor = Color.black;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float w = (float)source.width;
			float h = (float)source.height;
			float currentAspect = w / h;
			float offset = 0f;
			int pass = 0;

			Material.SetColor("_FillColor", FillColor);

			if (currentAspect < Aspect)
			{
				offset = (h - w / Aspect) * 0.5f / h;
			}
			else if (currentAspect > Aspect)
			{
				offset = (w - h * Aspect) * 0.5f / w;
				pass = 1;
			}
			else
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_Offsets", new Vector2(offset, 1f - offset));
			Graphics.Blit(source, destination, Material, pass);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Letterbox";
		}
	}
}
