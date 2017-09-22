// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class BaseEffect : MonoBehaviour
	{
		public Shader Shader;
		public Shader ShaderSafe
		{
			get
			{
				if (Shader == null)
					Shader = Shader.Find(GetShaderName());

				return Shader;
			}
		}

		protected Material m_Material;
		public Material Material
		{
			get
			{
				if (m_Material == null)
				{
					m_Material = new Material(ShaderSafe);
					m_Material.hideFlags = HideFlags.HideAndDontSave;
				}

				return m_Material;
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

			// Disable the image effect if the shader can't run on the users graphics card
			if (!ShaderSafe || !Shader.isSupported)
			{
				Debug.LogWarning("The shader is null or unsupported on this device");
				enabled = false;
			}
		}

		protected virtual void OnDisable()
		{
			if (m_Material)
				DestroyImmediate(m_Material);

			m_Material = null;
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

		protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination) { }

		protected virtual string GetShaderName() { return "null"; }
	}
}
