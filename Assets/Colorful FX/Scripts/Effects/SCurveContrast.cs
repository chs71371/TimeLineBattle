// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/s-curve-contrast.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/S-Curve Contrast")]
	public class SCurveContrast : BaseEffect
	{
		public float RedSteepness = 1f;
		public float RedGamma = 1f;
		public float GreenSteepness = 1f;
		public float GreenGamma = 1f;
		public float BlueSteepness = 1f;
		public float BlueGamma = 1f;

#if UNITY_EDITOR
		public bool e_ShowCurves = true;
#endif

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Red", new Vector2(RedSteepness, RedGamma));
			Material.SetVector("_Green", new Vector2(GreenSteepness, GreenGamma));
			Material.SetVector("_Blue", new Vector2(BlueSteepness, BlueGamma));
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/SCurveContrast";
		}
	}
}
