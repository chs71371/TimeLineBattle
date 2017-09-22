// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/white-balance.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/White Balance")]
	public class WhiteBalance : BaseEffect
	{
		public enum BalanceMode
		{
			Simple,
			Complex
		}

		[ColorUsage(false), Tooltip("Reference white point or midtone value.")]
		public Color White = new Color(0.5f, 0.5f, 0.5f);

		[Tooltip("Algorithm used.")]
		public BalanceMode Mode = BalanceMode.Complex;

		protected virtual void Reset()
		{
			White = CLib.IsLinearColorSpace() ?
				new Color(0.72974005284f, 0.72974005284f, 0.72974005284f) :
				new Color(0.5f, 0.5f, 0.5f);
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetColor("_White", White);
			Graphics.Blit(source, destination, Material, (int)Mode);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/White Balance";
		}
	}
}
