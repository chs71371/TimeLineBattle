// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/blur-effects/directional-blur.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Blur Effects/Directional Blur")]
	public class DirectionalBlur : BaseEffect
	{
		public enum QualityPreset
		{
			Low = 2,
			Medium = 4,
			High = 6,
			Custom
		}

		[Tooltip("Quality preset. Higher means better quality but slower processing.")]
		public QualityPreset Quality = QualityPreset.Medium;

		[Range(1, 16), Tooltip("Sample count. Higher means better quality but slower processing.")]
		public int Samples = 5;

		[Range(0f, 5f), Tooltip("Blur strength (distance).")]
		public float Strength = 1f;

		[Tooltip("Blur direction in radians.")]
		public float Angle = 0f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int samples = Quality == QualityPreset.Custom ? Samples : (int)Quality;
			float s = (Mathf.Sin(Angle) * Strength * 0.05f) / samples;
			float c = (Mathf.Cos(Angle) * Strength * 0.05f) / samples;
			Material.SetVector("_Params", new Vector3(s, c, samples));
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/DirectionalBlur";
		}
	}
}
