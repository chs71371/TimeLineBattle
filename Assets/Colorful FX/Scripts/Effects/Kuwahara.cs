// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/kuwahara.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/Kuwahara")]
	public class Kuwahara : BaseEffect
	{
		[Range(1, 6), Tooltip("Larger radius will give a more abstract look but will lower performances.")]
		public int Radius = 3;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Radius = Mathf.Clamp(Radius, 1, 6);
			Material.SetVector("_PSize", new Vector2(1f / (float)source.width, 1f / (float)source.height));
			Graphics.Blit(source, destination, Material, Radius - 1);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Kuwahara";
		}
	}
}
