// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;
	using InstragramFilter = Vintage.InstragramFilter;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/vintage-fast.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Vintage")]
	public class VintageFast : LookupFilter3D
	{
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
					LookupTexture = Resources.Load<Texture2D>("InstagramFast/" + Filter.ToString());
			}

			base.OnRenderImage(source, destination);
		}
	}
}
