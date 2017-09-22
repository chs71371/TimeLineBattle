// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/hue-focus.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Hue Focus")]
	public class HueFocus : BaseEffect
	{
		[Range(0f, 360f), Tooltip("Center hue.")]
		public float Hue = 0f;

		[Range(1f, 180f), Tooltip("Hue range to focus on.")]
		public float Range = 30f;

		[Range(0f, 1f), Tooltip("Makes the colored pixels more vibrant.")]
		public float Boost = 0.5f;

		[Range(0f, 1f), Tooltip("Blending Factor.")]
		public float Amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float h = Hue / 360f;
			float r = Range / 180f;
			Material.SetVector("_Range", new Vector2(h - r, h + r));
			Material.SetVector("_Params", new Vector3(h, Boost + 1f, Amount));
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Hue Focus";
		}
	}
}
