// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/noise.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/Noise")]
	public class Noise : BaseEffect
	{
		public enum ColorMode
		{
			Monochrome,
			RGB
		}

		[Tooltip("Black & white or colored noise.")]
		public ColorMode Mode = ColorMode.Monochrome;

		[Tooltip("Automatically increment the seed to animate the noise.")]
		public bool Animate = true;

		[Tooltip("A number used to initialize the noise generator.")]
		public float Seed = 0.5f;

		[Range(0f, 1f), Tooltip("Strength used to apply the noise. 0 means no noise at all, 1 is full noise.")]
		public float Strength = 0.12f;

		[Range(0f, 1f), Tooltip("Reduce the noise visibility in luminous areas.")]
		public float LumContribution = 0f;

		protected virtual void Update()
		{
			if (Animate)
			{
				// Reset the Seed after a while, some GPUs don't like big numbers
				if (Seed > 1000f)
					Seed = 0.5f;

				Seed += Time.deltaTime * 0.25f;
			}
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Params", new Vector3(Seed, Strength, LumContribution));
			int pass = (Mode == ColorMode.Monochrome) ? 0 : 1;
			pass += (LumContribution > 0f) ? 2 : 0;
			Graphics.Blit(source, destination, Material, pass);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Noise";
		}
	}
}
