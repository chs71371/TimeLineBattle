// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/channel-mixer.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Channel Mixer")]
	public class ChannelMixer : BaseEffect
	{
		public Vector3 Red = new Vector3(100f, 0f, 0f);
		public Vector3 Green = new Vector3(0f, 100f, 0f);
		public Vector3 Blue = new Vector3(0f, 0f, 100f);
		public Vector3 Constant = new Vector3(0f, 0f, 0f);

#if UNITY_EDITOR
		public int e_CurrentChannel = 0;
#endif

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Red", new Vector4(Red.x * 0.01f, Green.x * 0.01f, Blue.x * 0.01f));
			Material.SetVector("_Green", new Vector4(Red.y * 0.01f, Green.y * 0.01f, Blue.y * 0.01f));
			Material.SetVector("_Blue", new Vector4(Red.z * 0.01f, Green.z * 0.01f, Blue.z * 0.01f));
			Material.SetVector("_Constant", new Vector4(Constant.x * 0.01f, Constant.y * 0.01f, Constant.z * 0.01f));

			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Channel Mixer";
		}
	}
}
