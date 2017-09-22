// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/blend.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/Blend")]
	public class Blend : BaseEffect
	{
		public enum BlendingMode
		{
			Darken = 0,
			Multiply = 1,
			ColorBurn = 2,
			LinearBurn = 3,
			DarkerColor = 4,

			Lighten = 6,
			Screen = 7,
			ColorDodge = 8,
			LinearDodge = 9,
			LighterColor = 10,

			Overlay = 12,
			SoftLight = 13,
			HardLight = 14,
			VividLight = 15,
			LinearLight = 16,
			PinLight = 17,
			HardMix = 18,

			Difference = 20,
			Exclusion = 21,
			Subtract = 22,
			Divide = 23
		}
		
		[Tooltip("The Texture2D, RenderTexture or MovieTexture to blend.")]
		public Texture Texture;

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1f;

		[Tooltip("Blending mode.")]
		public BlendingMode Mode = 0;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Texture == null || Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetTexture("_OverlayTex", Texture);
			Material.SetFloat("_Amount", Amount);
			Graphics.Blit(source, destination, Material, (int)Mode);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Blend";
		}
	}
}
