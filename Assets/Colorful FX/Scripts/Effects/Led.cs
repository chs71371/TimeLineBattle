// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/led.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/LED")]
	public class Led : BaseEffect
	{
		public enum SizeMode
		{
			ResolutionIndependent,
			PixelPerfect
		}

		[Range(1f, 255f), Tooltip("Scale of an individual LED. Depends on the Mode used.")]
		public float Scale = 80.0f;

		[Range(0f, 10f), Tooltip("LED brightness booster.")]
		public float Brightness = 1.0f;

		[Range(1f, 3f), Tooltip("LED shape, from softer to harsher.")]
		public float Shape = 1.5f;

		[Tooltip("Turn this on to automatically compute the aspect ratio needed for squared LED.")]
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

			Material.SetVector("_Params", new Vector4(
					scale,
					AutomaticRatio ? ((float)source.width / (float)source.height) : Ratio,
					Brightness,
					Shape
				));

			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Led";
		}
	}
}
