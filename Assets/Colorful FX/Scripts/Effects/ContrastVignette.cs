// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/contrast-vignette.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Contrast Vignette")]
	public class ContrastVignette : BaseEffect
	{
		[Tooltip("Center point.")]
		public Vector2 Center = new Vector2(0.5f, 0.5f);

		[Range(-100f, 100f), Tooltip("Smoothness of the vignette effect.")]
		public float Sharpness = 32f;

		[Range(0f, 100f), Tooltip("Amount of vignetting on screen.")]
		public float Darkness = 28f;

		[Range(0f, 200f), Tooltip("Expands or shrinks the overall range of tonal values in the vignette area.")]
		public float Contrast = 20.0f;

		public Vector3 ContrastCoeff = new Vector3(0.5f, 0.5f, 0.5f);

		[Range(0f, 200f), Tooltip("Blends the contrast change toward the edges of the vignette effect.")]
		public float EdgeBlending = 0f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Params", new Vector4(Sharpness * 0.01f, Darkness * 0.02f, Contrast * 0.01f, EdgeBlending * 0.01f));
			Material.SetVector("_Coeffs", ContrastCoeff);
			Material.SetVector("_Center", Center);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Contrast Vignette";
		}
	}
}
