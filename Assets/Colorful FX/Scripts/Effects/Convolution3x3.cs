// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/other-effects/convolution-3x3.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Other Effects/Convolution Matrix 3x3")]
	public class Convolution3x3 : BaseEffect
	{
		public Vector3 KernelTop = Vector3.zero;
		public Vector3 KernelMiddle = Vector3.up;
		public Vector3 KernelBottom = Vector3.zero;

		[Tooltip("Used to normalize the kernel.")]
		public float Divisor = 1f;

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_PSize", new Vector2(1f / (float)source.width, 1f / (float)source.height));
			Material.SetVector("_KernelT", KernelTop / Divisor);
			Material.SetVector("_KernelM", KernelMiddle / Divisor);
			Material.SetVector("_KernelB", KernelBottom / Divisor);
			Material.SetFloat("_Amount", Amount);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Convolution 3x3";
		}
	}
}
