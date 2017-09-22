// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/strokes.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/Strokes")]
	public class Strokes : BaseEffect
	{
		public enum ColorMode
		{
			BlackAndWhite,
			WhiteAndBlack,
			ColorAndWhite,
			ColorAndBlack,
			WhiteAndColor,
			BlackAndColor
		}

		public ColorMode Mode = ColorMode.BlackAndWhite;

		[Range(0f, 0.04f), Tooltip("Stroke rotation, or wave pattern amplitude.")]
		public float Amplitude = 0.025f;

		[Range(0f, 20f), Tooltip("Wave pattern frequency (higher means more waves).")]
		public float Frequency = 10f;

		[Range(4f, 12f), Tooltip("Global scaling.")]
		public float Scaling = 7.5f;

		[Range(0.1f, 0.5f), Tooltip("Stroke maximum thickness.")]
		public float MaxThickness = 0.2f;

		[Range(0f, 1f), Tooltip("Contribution threshold (higher means more continous strokes).")]
		public float Threshold = 0.7f;

		[Range(-0.3f, 0.3f), Tooltip("Stroke pressure.")]
		public float Harshness = 0f;

		[Range(0f, 1f), Tooltip("Amount of red to contribute to the strokes.")]
		public float RedLuminance = 0.299f;

		[Range(0f, 1f), Tooltip("Amount of green to contribute to the strokes.")]
		public float GreenLuminance = 0.587f;

		[Range(0f, 1f), Tooltip("Amount of blue to contribute to the strokes.")]
		public float BlueLuminance = 0.114f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float scale = Scaling / (float)source.height;
			Material.SetVector("_Params1", new Vector4(Amplitude, Frequency, scale, MaxThickness * scale));
			Material.SetVector("_Params2", new Vector3(RedLuminance, GreenLuminance, BlueLuminance));
			Material.SetVector("_Params3", new Vector2(Threshold, Harshness));
			Graphics.Blit(source, destination, Material, (int)Mode);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Strokes";
		}
	}
}
