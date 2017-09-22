// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;
	using System;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/levels.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Levels")]
	public class Levels : BaseEffect
	{
		public enum ColorMode
		{
			Monochrome,
			RGB
		}

		public ColorMode Mode = ColorMode.Monochrome;

		public Vector3 InputL = new Vector3(0f, 255f, 1f);
		public Vector3 InputR = new Vector3(0f, 255f, 1f);
		public Vector3 InputG = new Vector3(0f, 255f, 1f);
		public Vector3 InputB = new Vector3(0f, 255f, 1f);

		public Vector2 OutputL = new Vector2(0f, 255f);
		public Vector2 OutputR = new Vector2(0f, 255f);
		public Vector2 OutputG = new Vector2(0f, 255f);
		public Vector2 OutputB = new Vector2(0f, 255f);

#if UNITY_EDITOR
		public enum Channel
		{
			Red,
			Green,
			Blue
		}

		public Channel e_CurrentChannel = 0;
		public bool e_Logarithmic = false;
		public bool e_AutoRefresh = false;
		public Action<RenderTexture> e_OnFrameEnd;

		bool e_ForceRefresh = false;

		public void InternalForceRefresh()
		{
			e_ForceRefresh = true;
		}
#endif

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
#if UNITY_EDITOR
			if (e_OnFrameEnd != null && (e_AutoRefresh || e_ForceRefresh))
			{
				RenderTexture rt = RenderTexture.GetTemporary(160, Mathf.FloorToInt(160f * ((float)source.height / (float)source.height)), 0, RenderTextureFormat.ARGB32);
				Graphics.Blit(source, rt);
				e_OnFrameEnd(rt);
				RenderTexture.ReleaseTemporary(rt);
				e_ForceRefresh = false;
			}
#endif

			if (Mode == ColorMode.Monochrome)
			{
				Material.SetVector("_InputMin", new Vector4(InputL.x / 255f, InputL.x / 255f, InputL.x / 255f, 1.0f));
				Material.SetVector("_InputMax", new Vector4(InputL.y / 255f, InputL.y / 255f, InputL.y / 255f, 1.0f));
				Material.SetVector("_InputGamma", new Vector4(InputL.z, InputL.z, InputL.z, 1.0f));
				Material.SetVector("_OutputMin", new Vector4(OutputL.x / 255f, OutputL.x / 255f, OutputL.x / 255f, 1.0f));
				Material.SetVector("_OutputMax", new Vector4(OutputL.y / 255f, OutputL.y / 255f, OutputL.y / 255f, 1.0f));
			}
			else
			{
				Material.SetVector("_InputMin", new Vector4(InputR.x / 255f, InputG.x / 255f, InputB.x / 255f, 1.0f));
				Material.SetVector("_InputMax", new Vector4(InputR.y / 255f, InputG.y / 255f, InputB.y / 255f, 1.0f));
				Material.SetVector("_InputGamma", new Vector4(InputR.z, InputG.z, InputB.z, 1.0f));
				Material.SetVector("_OutputMin", new Vector4(OutputR.x / 255f, OutputG.x / 255f, OutputB.x / 255f, 1.0f));
				Material.SetVector("_OutputMax", new Vector4(OutputR.y / 255f, OutputG.y / 255f, OutputB.y / 255f, 1.0f));
			}

			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Levels";
		}
	}
}
