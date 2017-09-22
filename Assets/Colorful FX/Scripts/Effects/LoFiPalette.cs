// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/lofi-palette.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/LoFi Palette")]
	public class LoFiPalette : LookupFilter3D
	{
		public enum Preset
		{
			None = 0,
			
			AmstradCPC = 2,
			CGA = 3,
			Commodore64 = 4,
			CommodorePlus = 5,
			EGA = 6,
			GameBoy = 7,
			MacOS16 = 8,
			MacOS256 = 9,
			MasterSystem = 10,
			RiscOS16 = 11,
			Teletex = 12,
			Windows16 = 13,
			Windows256 = 14,
			ZXSpectrum = 15,

			Andrae = 17,
			Anodomani = 18,
			Crayolo = 19,
			DB16 = 20,
			DB32 = 21,
			DJinn = 22,
			DrazileA = 23,
			DrazileB = 24,
			DrazileC = 25,
			Eggy = 26,
			FinlalA = 27,
			FinlalB = 28,
			Hapiel = 29,
			PavanzA = 30,
			PavanzB = 31,
			Peyton = 32,
			SpeedyCube = 33
		}

		public Preset Palette = Preset.None;

		[Tooltip("Pixelize the display.")]
		public bool Pixelize = true;

		[Tooltip("The display height in pixels.")]
		public float PixelSize = 128f;

		protected Preset m_CurrentPreset = Preset.None;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Palette != m_CurrentPreset)
			{
				m_CurrentPreset = Palette;

				if (Palette == Preset.None)
					LookupTexture = null;
				else
					LookupTexture = Resources.Load<Texture2D>("LoFiPalettes/" + Palette.ToString());
			}

			if (LookupTexture == null || Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			if (m_Use2DLut || ForceCompatibility)
				RenderLut2D(source, destination);
			else
				RenderLut3D(source, destination);
		}

		protected override void RenderLut2D(RenderTexture source, RenderTexture destination)
		{
			float tileSize = Mathf.Sqrt((float)LookupTexture.width);
			Material.SetTexture("_LookupTex", LookupTexture);
			Material.SetVector("_Params1", new Vector3(1f / (float)LookupTexture.width, 1f / (float)LookupTexture.height, tileSize - 1f));
			Material.SetVector("_Params2", new Vector2(Amount, PixelSize));

			int pass = (Pixelize ? 6 : 4) + (CLib.IsLinearColorSpace() ? 1 : 0);
			Graphics.Blit(source, destination, Material, pass);
		}

		protected override void RenderLut3D(RenderTexture source, RenderTexture destination)
		{
			if (LookupTexture.name != m_BaseTextureName)
				ConvertBaseTexture();

			if (m_Lut3D == null)
				SetIdentityLut();

			m_Lut3D.filterMode = FilterMode.Point;
			Material.SetTexture("_LookupTex", m_Lut3D);
			float lutSize = (float)m_Lut3D.width;
			Material.SetVector("_Params", new Vector4(
					(lutSize - 1f) / (1f * lutSize),
					1f / (2f * lutSize),
					Amount,
					PixelSize
				));

			int pass = (Pixelize ? 2 : 0) + (CLib.IsLinearColorSpace() ? 1 : 0);
			Graphics.Blit(source, destination, Material, pass);
		}
	}
}
