// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/blur-effects/grainy-blur.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Blur Effects/Grainy Blur")]
	public class GrainyBlur : BaseEffect
	{
		[Min(0f), Tooltip("Blur radius.")]
		public float Radius = 32f;

		[Range(1, 32), Tooltip("Sample count. Higher means better quality but slower processing.")]
		public int Samples = 16;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (CLib.Approximately(Radius, 0f, 0.001f))
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_Params", new Vector2(Radius, Samples));
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/GrainyBlur";
		}
	}
}
