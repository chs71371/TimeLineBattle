// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/pixelate.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/Pixelate")]
	public class Pixelate : BaseEffect
	{
		public enum SizeMode
		{
			ResolutionIndependent,
			PixelPerfect
		}

		[Range(1f, 1024f), Tooltip("Scale of an individual pixel. Depends on the Mode used.")]
		public float Scale = 80.0f;

		[Tooltip("Turn this on to automatically compute the aspect ratio needed for squared pixels.")]
		public bool AutomaticRatio = true;

		[Tooltip("Custom aspect ratio.")]
		public float Ratio = 1.0f;

		[Tooltip("Used for the Scale field.")]
		public SizeMode Mode = SizeMode.ResolutionIndependent;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float scale = Scale;

			if (Mode == SizeMode.PixelPerfect)
				scale = (float)source.width / Scale;

			Material.SetVector("_Params", new Vector2(
					scale,
					AutomaticRatio ? ((float)source.width / (float)source.height) : Ratio
				));

			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Pixelate";
		}
	}
}
