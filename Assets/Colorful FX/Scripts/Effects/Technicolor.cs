// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/technicolor.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Technicolor")]
	public class Technicolor : BaseEffect
	{
		[Range(0f, 8f)]
		public float Exposure = 4f;

		public Vector3 Balance = new Vector3(0.25f, 0.25f, 0.25f);

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 0.5f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetFloat("_Exposure", 8f - Exposure);
			Material.SetVector("_Balance", Vector3.one - Balance);
			Material.SetFloat("_Amount", Amount);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Technicolor";
		}
	}
}
