// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/dithering.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/Dithering")]
	public class Dithering : BaseEffect
	{
		[Tooltip("Show the original picture under the dithering pass.")]
		public bool ShowOriginal = false;

		[Tooltip("Convert the original render to black & white.")]
		public bool ConvertToGrayscale = false;

		[Range(0f, 1f), Tooltip("Amount of red to contribute to the luminosity.")]
		public float RedLuminance = 0.299f;

		[Range(0f, 1f), Tooltip("Amount of green to contribute to the luminosity.")]
		public float GreenLuminance = 0.587f;

		[Range(0f, 1f), Tooltip("Amount of blue to contribute to the luminosity.")]
		public float BlueLuminance = 0.114f;

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1f;

		protected Texture2D m_DitherPattern;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			if (m_DitherPattern == null)
				m_DitherPattern = Resources.Load<Texture2D>("Misc/DitherPattern");

			Material.SetTexture("_Pattern", m_DitherPattern);
			Material.SetVector("_Params", new Vector4(RedLuminance, GreenLuminance, BlueLuminance, Amount));

			int pass = ShowOriginal ? 4 : 0;
			pass += ConvertToGrayscale ? 2 : 0;
			pass += CLib.IsLinearColorSpace() ? 1 : 0;

			Graphics.Blit(source, destination, Material, pass);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Dithering";
		}
	}
}
