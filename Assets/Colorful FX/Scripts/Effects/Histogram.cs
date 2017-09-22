// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;
	using System;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/utilities/histogram.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Utilities/Histogram")]
	public class Histogram : MonoBehaviour
	{
#if UNITY_EDITOR
		public enum Channel
		{
			Luminance,
			RGB,
			Red,
			Green,
			Blue
		}

		public Channel e_CurrentChannel = Channel.RGB;
		public bool e_Logarithmic = false;
		public bool e_AutoRefresh = false;
		public Action<RenderTexture> e_OnFrameEnd;

		bool e_ForceRefresh = false;

		public void InternalForceRefresh()
		{
			e_ForceRefresh = true;
		}

		protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (e_OnFrameEnd != null && (e_AutoRefresh || e_ForceRefresh))
			{
				RenderTexture rt = RenderTexture.GetTemporary(160, Mathf.FloorToInt(160f * ((float)source.height / (float)source.height)), 0, RenderTextureFormat.ARGB32);
				Graphics.Blit(source, rt);
				e_OnFrameEnd(rt);
				RenderTexture.ReleaseTemporary(rt);
				e_ForceRefresh = false;
			}

			Graphics.Blit(source, destination);
		}
#endif
	}
}
