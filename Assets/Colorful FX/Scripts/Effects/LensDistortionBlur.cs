// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/blur-effects/lens-distortion-blur.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Blur Effects/Lens Distortion Blur")]
	public class LensDistortionBlur : BaseEffect
	{
		public enum QualityPreset
		{
			Low = 4,
			Medium = 8,
			High = 12,
			Custom
		}

		[Tooltip("Quality preset. Higher means better quality but slower processing.")]
		public QualityPreset Quality = QualityPreset.Medium;

		[Range(2, 32), Tooltip("Sample count. Higher means better quality but slower processing.")]
		public int Samples = 10;

		[Range(-2f, 2f), Tooltip("Spherical distortion factor.")]
		public float Distortion = 0.2f;

		[Range(-2f, 2f), Tooltip("Cubic distortion factor.")]
		public float CubicDistortion = 0.6f;

		[Range(0.01f, 2f), Tooltip("Helps avoid screen streching on borders when working with heavy distortions.")]
		public float Scale = 0.8f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int samples = Quality == QualityPreset.Custom ? Samples : (int)Quality;
			Material.SetVector("_Params", new Vector4(samples, Distortion / samples, CubicDistortion / samples, Scale));
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/LensDistortionBlur";
		}
	}
}
