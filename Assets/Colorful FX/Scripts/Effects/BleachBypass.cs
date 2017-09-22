// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/bleach-bypass.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Bleach Bypass")]
	public class BleachBypass : BaseEffect
	{
		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetFloat("_Amount", Amount);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Bleach Bypass";
		}
	}
}
