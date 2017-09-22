// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/lookup-filter-3d.html")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Colorful FX/Color Correction/Lookup Filter 3D")]
	public class LookupFilter3D : MonoBehaviour
	{
		[Tooltip("The lookup texture to apply. Read the documentation to learn how to create one.")]
		public Texture2D LookupTexture;

		[Range(0f, 1f), Tooltip("Blending factor.")]
		public float Amount = 1f;

		[Tooltip("The effect will automatically detect the correct shader to use for the device but you can force it to only use the compatibility shader.")]
		public bool ForceCompatibility = false;

		protected Texture3D m_Lut3D;
		protected string m_BaseTextureName;
		protected bool m_Use2DLut = false;

		public Shader Shader2D;
		public Shader Shader2DSafe
		{
			get
			{
				if (Shader2D == null)
					Shader2D = Shader.Find("Hidden/Colorful/Lookup Filter 2D");

				return Shader2D;
			}
		}

		public Shader Shader3D;
		public Shader Shader3DSafe
		{
			get
			{
				if (Shader3D == null)
					Shader3D = Shader.Find("Hidden/Colorful/Lookup Filter 3D");

				return Shader3D;
			}
		}

		protected Material m_Material2D;
		protected Material m_Material3D;
		public Material Material
		{
			get
			{
				if (m_Use2DLut || ForceCompatibility)
				{
					if (m_Material2D == null)
					{
						m_Material2D = new Material(Shader2DSafe);
						m_Material2D.hideFlags = HideFlags.HideAndDontSave;
					}

					return m_Material2D;
				}
				else
				{
					if (m_Material3D == null)
					{
						m_Material3D = new Material(Shader3DSafe);
						m_Material3D.hideFlags = HideFlags.HideAndDontSave;
					}

					return m_Material3D;
				}
			}
		}

		protected virtual void Start()
		{
			// Disable if we don't support image effects
			if (!SystemInfo.supportsImageEffects)
			{
				Debug.LogWarning("Image effects aren't supported on this device");
				enabled = false;
				return;
			}

			// Switch to the 2D lut if the platform doesn't support 3D textures
			if (!SystemInfo.supports3DTextures)
				m_Use2DLut = true;

			// Disable the image effect if the shader can't run on the users graphics card
			if (!Shader2DSafe || !Shader2D.isSupported)
			{
				Debug.LogWarning("The shader is null or unsupported on this device");
				enabled = false;
				return;
			}

			if (!m_Use2DLut && !ForceCompatibility && (!Shader3DSafe || !Shader3D.isSupported))
			{
				// Falling back on the supported 2D shader
				m_Use2DLut = true;
			}
		}

		protected virtual void OnDisable()
		{
			if (m_Material2D)
				DestroyImmediate(m_Material2D);

			if (m_Material3D)
				DestroyImmediate(m_Material3D);

			if (m_Lut3D)
				DestroyImmediate(m_Lut3D);

			m_BaseTextureName = "";
		}

		protected virtual void Reset()
		{
			m_BaseTextureName = "";
		}

		protected void SetIdentityLut()
		{
			int dim = 16;
			Color[] newC = new Color[dim * dim * dim];
			float oneOverDim = 1.0f / (1.0f * dim - 1.0f);

			for (int i = 0; i < dim; i++)
			{
				for (int j = 0; j < dim; j++)
				{
					for (int k = 0; k < dim; k++)
					{
						newC[i + (j * dim) + (k * dim * dim)] = new Color((i * 1.0f) * oneOverDim, (j * 1.0f) * oneOverDim, (k * 1.0f) * oneOverDim, 1.0f);
					}
				}
			}

			if (m_Lut3D)
				DestroyImmediate(m_Lut3D);

			m_Lut3D = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
			m_Lut3D.hideFlags = HideFlags.HideAndDontSave;
			m_Lut3D.SetPixels(newC);
			m_Lut3D.Apply();
			m_BaseTextureName = "";
		}

		public bool ValidDimensions(Texture2D tex2D)
		{
			if (tex2D == null || tex2D.height != Mathf.FloorToInt(Mathf.Sqrt(tex2D.width)))
				return false;

			return true;
		}

		protected void ConvertBaseTexture()
		{
			if (!ValidDimensions(LookupTexture))
			{
				Debug.LogWarning("The given 2D texture " + LookupTexture.name + " cannot be used as a 3D LUT. Pick another texture or adjust dimension to e.g. 256x16.");
				return;
			}

			m_BaseTextureName = LookupTexture.name;

			int dim = LookupTexture.height;

			Color[] c = LookupTexture.GetPixels();
			Color[] newC = new Color[c.Length];

			for (int i = 0; i < dim; i++)
			{
				for (int j = 0; j < dim; j++)
				{
					for (int k = 0; k < dim; k++)
					{
						int j_ = dim - j - 1;
						newC[i + (j * dim) + (k * dim * dim)] = c[k * dim + i + j_ * dim * dim];
					}
				}
			}

			if (m_Lut3D)
				DestroyImmediate(m_Lut3D);

			m_Lut3D = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
			m_Lut3D.hideFlags = HideFlags.HideAndDontSave;
			m_Lut3D.wrapMode = TextureWrapMode.Clamp;
			m_Lut3D.SetPixels(newC);
			m_Lut3D.Apply();
		}

		public void Apply(Texture source, RenderTexture destination)
		{
			if (source is RenderTexture)
			{
				OnRenderImage(source as RenderTexture, destination);
				return;
			}

			RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(source, rt);
			OnRenderImage(rt, destination);
			RenderTexture.ReleaseTemporary(rt);
		}

		protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
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

		protected virtual void RenderLut2D(RenderTexture source, RenderTexture destination)
		{
			float tileSize = Mathf.Sqrt((float)LookupTexture.width);
			Material.SetTexture("_LookupTex", LookupTexture);
			Material.SetVector("_Params1", new Vector3(1f / (float)LookupTexture.width, 1f / (float)LookupTexture.height, tileSize - 1f));
			Material.SetVector("_Params2", new Vector2(Amount, 0f));

			Graphics.Blit(source, destination, Material, CLib.IsLinearColorSpace() ? 1 : 0);
		}

		protected virtual void RenderLut3D(RenderTexture source, RenderTexture destination)
		{
			if (LookupTexture.name != m_BaseTextureName)
				ConvertBaseTexture();

			if (m_Lut3D == null)
				SetIdentityLut();

			Material.SetTexture("_LookupTex", m_Lut3D);
			float lutSize = (float)m_Lut3D.width;
			Material.SetVector("_Params", new Vector3(
					(lutSize - 1f) / (1f * lutSize),
					1f / (2f * lutSize),
					Amount
				));

			Graphics.Blit(source, destination, Material, CLib.IsLinearColorSpace() ? 1 : 0);
		}
	}
}
