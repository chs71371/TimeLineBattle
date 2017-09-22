// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/double-vision.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/Double Vision")]
	public class DoubleVision : BaseEffect
	{
		[Tooltip("Diploplia strength.")]
		public Vector2 Displace = new Vector2(0.7f, 0.0f);

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1.0f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Amount <= 0f || Displace == Vector2.zero)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_Displace", new Vector2(Displace.x / (float)source.width, Displace.y / (float)source.height));
			Material.SetFloat("_Amount", Amount);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Double Vision";
		}
	}
}
