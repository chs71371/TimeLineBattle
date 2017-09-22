// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/channel-swapper.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Channel Swapper")]
	public class ChannelSwapper : BaseEffect
	{
		public enum Channel
		{
			Red,
			Green,
			Blue
		}

		[Tooltip("Source channel to use for the output red channel.")]
		public Channel RedSource = Channel.Red;

		[Tooltip("Source channel to use for the output green channel.")]
		public Channel GreenSource = Channel.Green;

		[Tooltip("Source channel to use for the output blue channel.")]
		public Channel BlueSource = Channel.Blue;

		static Vector4[] m_Channels = new Vector4[]
		{
			new Vector4(1f, 0f, 0f, 0f),
			new Vector4(0f, 1f, 0f, 0f),
			new Vector4(0f, 0f, 1f, 0f)
		};

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Red", m_Channels[(int)RedSource]);
			Material.SetVector("_Green", m_Channels[(int)GreenSource]);
			Material.SetVector("_Blue", m_Channels[(int)BlueSource]);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Channel Swapper";
		}
	}
}
