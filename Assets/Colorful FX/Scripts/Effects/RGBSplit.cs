// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/camera-effects/rgb-split.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Camera Effects/RGB Split")]
	public class RGBSplit : BaseEffect
	{
		[Tooltip("RGB shifting amount.")]
		public float Amount = 0f;

		[Tooltip("Shift direction in radians.")]
		public float Angle = 0f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Amount == 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_Params", new Vector3(
					Amount * 0.001f,
					Mathf.Sin(Angle),
					Mathf.Cos(Angle)
				));

			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/RGB Split";
		}
	}
}
