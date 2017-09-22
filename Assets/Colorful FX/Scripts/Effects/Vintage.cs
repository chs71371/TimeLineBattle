// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/vintage.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Vintage (Deprecated)")]
	public class Vintage : LookupFilter
	{
		public enum InstragramFilter
		{
			None,
			F1977,
			Aden,
			Amaro,
			Brannan,
			Crema,
			Earlybird,
			Hefe,
			Hudson,
			Inkwell,
			Juno,
			Kelvin,
			Lark,
			LoFi,
			Ludwig,
			Mayfair,
			Nashville,
			Perpetua,
			Reyes,
			Rise,
			Sierra,
			Slumber,
			Sutro,
			Toaster,
			Valencia,
			Walden,
			Willow,
			XProII
		}

		public InstragramFilter Filter = InstragramFilter.None;

		protected InstragramFilter m_CurrentFilter = InstragramFilter.None;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Filter != m_CurrentFilter)
			{
				m_CurrentFilter = Filter;

				if (Filter == InstragramFilter.None)
					LookupTexture = null;
				else
					LookupTexture = Resources.Load<Texture2D>("Instagram/" + Filter.ToString());
			}

			base.OnRenderImage(source, destination);
		}
	}
}
