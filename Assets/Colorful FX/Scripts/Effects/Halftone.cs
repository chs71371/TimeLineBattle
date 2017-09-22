// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/halftone.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/Halftone")]
	public class Halftone : BaseEffect
	{
		[Min(0f), Tooltip("Global haltfoning scale.")]
		public float Scale = 12f;

		[Min(0f), Tooltip("Individual dot size.")]
		public float DotSize = 1.35f;

		[Tooltip("Rotates the dot placement according to the Center point.")]
		public float Angle = 1.2f;

		[Range(0f, 1f), Tooltip("Dots antialiasing")]
		public float Smoothness = 0.080f;

		[Tooltip("Center point to use for the rotation.")]
		public Vector2 Center = new Vector2(0.5f, 0.5f);

		[Tooltip("Turns the effect black & white.")]
		public bool Desaturate = false;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Center", new Vector2(Center.x * (float)source.width, Center.y * (float)source.height));
			Material.SetVector("_Params", new Vector3(Scale, DotSize, Smoothness));

			// Precompute rotation matrices
			Matrix4x4 m = new Matrix4x4();
			m.SetRow(0, CMYKRot(Angle + 0.261799388f)); // C
			m.SetRow(1, CMYKRot(Angle + 1.30899694f));  // M
			m.SetRow(2, CMYKRot(Angle));                // Y
			m.SetRow(3, CMYKRot(Angle + 0.785398163f)); // K
			Material.SetMatrix("_MatRot", m);

			Graphics.Blit(source, destination, Material, Desaturate ? 1 : 0);
		}

		Vector4 CMYKRot(float angle)
		{
			float ca = Mathf.Cos(angle);
			float sa = Mathf.Sin(angle);
			return new Vector4(ca, -sa, sa, ca);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Halftone";
		}
	}
}
