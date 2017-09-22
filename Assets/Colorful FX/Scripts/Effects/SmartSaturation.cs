// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/smart-saturation.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Smart Saturation")]
	public class SmartSaturation : BaseEffect
	{
		[Range(0f, 2f), Tooltip("Saturation boost. Default: 1 (no boost).")]
		public float Boost = 1f;

		public AnimationCurve Curve;

		Texture2D _CurveTexture;
		protected Texture2D m_CurveTexture
		{
			get
			{
				if (_CurveTexture == null)
					UpdateCurve();

				return _CurveTexture;
			}
		}

		protected virtual void Reset()
		{
			Curve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 0f), new Keyframe(1f, 0.5f, 0f, 0f));
		}

		protected virtual void OnEnable()
		{
			if (Curve == null)
				Reset();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (_CurveTexture != null)
				DestroyImmediate(_CurveTexture);
		}

		public virtual void UpdateCurve()
		{
			if (_CurveTexture == null)
			{
				_CurveTexture = new Texture2D(256, 1, TextureFormat.Alpha8, false);
				_CurveTexture.name = "Saturation Curve Texture";
				_CurveTexture.wrapMode = TextureWrapMode.Clamp;
				_CurveTexture.anisoLevel = 0;
				_CurveTexture.filterMode = FilterMode.Bilinear;
				_CurveTexture.hideFlags = HideFlags.DontSave;
			}

			Color[] pixels = _CurveTexture.GetPixels();

			for (int i = 0; i < 256; i++)
			{
				float z = Mathf.Clamp01(Curve.Evaluate((float)i / 255f));
				pixels[i] = new Color(z, z, z, z);
			}

			_CurveTexture.SetPixels(pixels);
			_CurveTexture.Apply();
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetTexture("_Curve", m_CurveTexture);
			Material.SetFloat("_Boost", Boost);
			Graphics.Blit(source, destination, Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Smart Saturation";
		}
	}
}
