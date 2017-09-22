// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/dynamic-lookup.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Dynamic Lookup")]
	public class DynamicLookup : BaseEffect
	{
		[ColorUsage(false)]
		public Color White = new Color(1f, 1f, 1f);

		[ColorUsage(false)]
		public Color Black = new Color(0f, 0f, 0f);

		[ColorUsage(false)]
		public Color Red = new Color(1f, 0f, 0f);

		[ColorUsage(false)]
		public Color Green = new Color(0f, 1f, 0f);

		[ColorUsage(false)]
		public Color Blue = new Color(0f, 0f, 1f);

		[ColorUsage(false)]
		public Color Yellow = new Color(1f, 1f, 0f);

		[ColorUsage(false)]
		public Color Magenta = new Color(1f, 0f, 1f);

		[ColorUsage(false)]
		public Color Cyan = new Color(0f, 1f, 1f);

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetColor("_White", White);
			Material.SetColor("_Black", Black);
			Material.SetColor("_Red", Red);
			Material.SetColor("_Green", Green);
			Material.SetColor("_Blue", Blue);
			Material.SetColor("_Yellow", Yellow);
			Material.SetColor("_Magenta", Magenta);
			Material.SetColor("_Cyan", Cyan);
			Material.SetFloat("_Amount", Amount);
			Graphics.Blit(source, destination, Material, CLib.IsLinearColorSpace() ? 1 : 0);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/DynamicLookup";
		}
	}
}
