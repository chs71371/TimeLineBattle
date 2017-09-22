// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/channel-clamper.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Channel Clamper")]
	public class ChannelClamper : BaseEffect
	{
		public Vector2 Red = new Vector2(0f, 1f);
		public Vector2 Green = new Vector2(0f, 1f);
		public Vector2 Blue = new Vector2(0f, 1f);

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_RedClamp", Red);
			Material.SetVector("_GreenClamp", Green);
			Material.SetVector("_BlueClamp", Blue);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Channel Clamper";
		}
	}
}
