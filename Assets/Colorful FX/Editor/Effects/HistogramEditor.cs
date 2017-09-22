// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;
	using UnityEditorInternal;
	using System;
	using Channel = Histogram.Channel;

	[CustomEditor(typeof(Histogram))]
	public class HistogramEditor : BaseEffectEditor
	{
		SerializedProperty p_CurrentChannel;
		SerializedProperty p_Logarithmic;
		SerializedProperty p_AutoRefresh;

		Histogram m_Target;
		int[] m_Histogram = new int[256];
		int[] m_HistogramRGB = new int[256 * 3];
		Rect m_HistogramRect = new Rect(0f, 0f, 1f, 1f);
		Texture2D m_TempTexture;

		Color MasterColor = new Color(1f, 1f, 1f, 2f);
		Color RedColor = new Color(1f, 0f, 0f, 2f);
		Color GreenColor = new Color(0f, 1f, 0f, 2f);
		Color BlueColor = new Color(0f, 1f, 1f, 2f);

		void OnEnable()
		{
			p_CurrentChannel = serializedObject.FindProperty("e_CurrentChannel");
			p_Logarithmic = serializedObject.FindProperty("e_Logarithmic");
			p_AutoRefresh = serializedObject.FindProperty("e_AutoRefresh");

			m_Target = target as Histogram;
			m_Target.e_OnFrameEnd = UpdateHistogram;
			m_Target.InternalForceRefresh();

			InternalEditorUtility.RepaintAllViews();
		}

		void OnDisable()
		{
			m_Target.e_OnFrameEnd = null;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Sizing
			if (!EditorGUIUtility.isProSkin)
				EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			{
				Rect rect = GUILayoutUtility.GetRect(256f, 128f);
				float width = Mathf.Min(512f, rect.width);
				float height = Mathf.Min(128f, rect.height);

				if (Event.current.type == EventType.Repaint)
				{
					m_HistogramRect = new Rect(
						Mathf.Floor(rect.x + rect.width / 2f - width / 2f),
						Mathf.Floor(rect.y + rect.height / 2f - height / 2f),
						width, height
					);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (!EditorGUIUtility.isProSkin)
				EditorGUILayout.Space();

			// Histogram
			if (!EditorGUIUtility.isProSkin)
			{
				Vector3[] verts = new Vector3[] {
					new Vector3(m_HistogramRect.x - 4, m_HistogramRect.y - 4),
					new Vector3(m_HistogramRect.xMax + 4, m_HistogramRect.y - 4),
					new Vector3(m_HistogramRect.xMax + 4, m_HistogramRect.yMax + 4),
					new Vector3(m_HistogramRect.x - 4, m_HistogramRect.yMax + 4)
				};
				Handles.DrawSolidRectangleWithOutline(verts, new Color(0.21f, 0.21f, 0.21f, 2.0f), new Color(0.21f, 0.21f, 0.21f, 2.0f));
			}

			if (p_CurrentChannel.intValue == (int)Channel.RGB)
				DrawHistogramRGB(m_HistogramRect);
			else
				DrawHistogram(m_HistogramRect);

			// UI
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginHorizontal(GUILayout.Width(m_HistogramRect.width));
				{
					Channel currentChannel = (Channel)p_CurrentChannel.intValue;
					currentChannel = (Channel)EditorGUILayout.EnumPopup(currentChannel);
					p_CurrentChannel.intValue = (int)currentChannel;

					GUILayout.FlexibleSpace();

					p_Logarithmic.boolValue = GUILayout.Toggle(p_Logarithmic.boolValue, GetContent("Log"), EditorStyles.miniButtonLeft);
					p_AutoRefresh.boolValue = GUILayout.Toggle(p_AutoRefresh.boolValue, GetContent("Auto Refresh"), EditorStyles.miniButtonMid);

					EditorGUI.BeginDisabledGroup(p_AutoRefresh.boolValue);

					if (GUILayout.Button(GetContent("Refresh"), EditorStyles.miniButtonRight))
						Refresh();

					EditorGUI.EndDisabledGroup();
				}
				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			EditorGUILayout.EndHorizontal();

			if (EditorGUI.EndChangeCheck())
				Refresh();

			serializedObject.ApplyModifiedProperties();
		}

		void Refresh()
		{
			m_Target.InternalForceRefresh();
			InternalEditorUtility.RepaintAllViews();
		}

		void UpdateHistogram(RenderTexture source)
		{
			if (m_TempTexture == null || m_TempTexture.width != source.width || m_TempTexture.height != source.height)
			{
				if (m_TempTexture != null)
					DestroyImmediate(m_TempTexture);

				m_TempTexture = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);
				m_TempTexture.anisoLevel = 0;
				m_TempTexture.wrapMode = TextureWrapMode.Clamp;
				m_TempTexture.filterMode = FilterMode.Bilinear;
				m_TempTexture.hideFlags = HideFlags.HideAndDontSave;
			}

			// Grab the screen content for the camera
			RenderTexture.active = source;
			m_TempTexture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0, false);
			m_TempTexture.Apply();
			RenderTexture.active = null;

			// Raw histogram data
			Color[] pixels = m_TempTexture.GetPixels();

			switch (p_CurrentChannel.intValue)
			{
				case (int)Channel.RGB:
					Array.Clear(m_HistogramRGB, 0, 256 * 3);
					for (int i = 0; i < pixels.Length; i++)
					{
						Color c = pixels[i];
						m_HistogramRGB[(int)(c.r * 255)]++;
						m_HistogramRGB[(int)(c.g * 255) + 256]++;
						m_HistogramRGB[(int)(c.b * 255) + 512]++;
					}
					break;
				case (int)Channel.Luminance:
					Array.Clear(m_Histogram, 0, 256);
					for (int i = 0; i < pixels.Length; i++)
					{
						Color c = pixels[i];
						m_Histogram[(int)((c.r * 0.2125f + c.g * 0.7154f + c.b * 0.0721f) * 255)]++;
					}
					break;
				case (int)Channel.Red:
					Array.Clear(m_Histogram, 0, 256);
					for (int i = 0; i < pixels.Length; i++)
						m_Histogram[(int)(pixels[i].r * 255)]++;
					break;
				case (int)Channel.Green:
					Array.Clear(m_Histogram, 0, 256);
					for (int i = 0; i < pixels.Length; i++)
						m_Histogram[(int)(pixels[i].g * 255)]++;
					break;
				case (int)Channel.Blue:
					Array.Clear(m_Histogram, 0, 256);
					for (int i = 0; i < pixels.Length; i++)
						m_Histogram[(int)(pixels[i].b * 255)]++;
					break;
			}
		}

		void DrawHistogram(Rect rect)
		{
			// Scale histogram values
			int[] scaledHistogram = new int[256];

			int max = 0;
			for (int i = 0; i < 256; i++)
				max = (max < m_Histogram[i]) ? m_Histogram[i] : max;

			scaledHistogram = new int[256];

			if (p_Logarithmic.boolValue)
			{
				float factor = rect.height / Mathf.Log10(max);

				for (int i = 0; i < 256; i++)
					scaledHistogram[i] = (m_Histogram[i] == 0) ? 0 : Mathf.Max(Mathf.RoundToInt(Mathf.Log10(m_Histogram[i]) * factor), 1);
			}
			else
			{
				float factor = rect.height / max;

				for (int i = 0; i < 256; i++)
					scaledHistogram[i] = Mathf.Max(Mathf.RoundToInt(m_Histogram[i] * factor), 1);
			}

			// Color
			if (p_CurrentChannel.intValue == (int)Channel.Luminance)
				Handles.color = MasterColor;
			else if (p_CurrentChannel.intValue == (int)Channel.Red)
				Handles.color = RedColor;
			else if (p_CurrentChannel.intValue == (int)Channel.Green)
				Handles.color = GreenColor;
			else
				Handles.color = BlueColor;

			// Base line
			Vector2 p1 = new Vector2(rect.x - 1, rect.yMax);
			Vector2 p2 = new Vector2(rect.xMax - 1, rect.yMax);
			Handles.DrawLine(p1, p2);

			// Histogram
			for (int i = 0; i < (int)rect.width; i++)
			{
				float remapI = (float)i / rect.width * 255f;
				int index = Mathf.FloorToInt(remapI);
				float fract = remapI - (float)index;
				float v1 = scaledHistogram[index];
				float v2 = scaledHistogram[Mathf.Min(index + 1, 255)];
				float h = v1 * (1.0f - fract) + v2 * fract;
				Handles.DrawLine(
						new Vector2(rect.x + i, rect.yMax),
						new Vector2(rect.x + i, rect.yMin + (rect.height - h))
					);
			}
		}

		void DrawHistogramRGB(Rect rect)
		{
			// Scale histogram values
			Vector3 max = Vector3.zero;
			for (int i = 0; i < 256; i++)
			{
				max.x = (max.x < m_HistogramRGB[i]) ? m_HistogramRGB[i] : max.x;
				max.y = (max.y < m_HistogramRGB[i + 256]) ? m_HistogramRGB[i + 256] : max.y;
				max.z = (max.z < m_HistogramRGB[i + 512]) ? m_HistogramRGB[i + 512] : max.z;
			}

			Vector3[] scaledHistogramRGB = new Vector3[256];

			if (p_Logarithmic.boolValue)
			{
				Vector3 factor = new Vector3(
					rect.height / Mathf.Log10(max.x),
					rect.height / Mathf.Log10(max.y),
					rect.height / Mathf.Log10(max.z)
				);

				for (int i = 0; i < 256; i++)
				{
					scaledHistogramRGB[i] = new Vector3(
						(m_HistogramRGB[i] == 0) ? 0 : Mathf.Max(Mathf.RoundToInt(Mathf.Log10(m_HistogramRGB[i]) * factor.x), 1),
						(m_HistogramRGB[i + 256] == 0) ? 0 : Mathf.Max(Mathf.RoundToInt(Mathf.Log10(m_HistogramRGB[i + 256]) * factor.y), 1),
						(m_HistogramRGB[i + 512] == 0) ? 0 : Mathf.Max(Mathf.RoundToInt(Mathf.Log10(m_HistogramRGB[i + 512]) * factor.z), 1)
					);
				}
			}
			else
			{
				Vector3 factor = new Vector3(rect.height / max.x, rect.height / max.y, rect.height / max.z);

				for (int i = 0; i < 256; i++)
				{
					scaledHistogramRGB[i] = new Vector3(
						Mathf.Max(Mathf.RoundToInt(m_HistogramRGB[i] * factor.x), 1),
						Mathf.Max(Mathf.RoundToInt(m_HistogramRGB[i + 256] * factor.y), 1),
						Mathf.Max(Mathf.RoundToInt(m_HistogramRGB[i + 512] * factor.z), 1)
					);
				}
			}

			// Base line
			Handles.color = MasterColor;
			Vector2 p1 = new Vector2(rect.x - 1, rect.yMax);
			Vector2 p2 = new Vector2(rect.xMax - 1, rect.yMax);
			Handles.DrawLine(p1, p2);
			Color[] colors = { RedColor, GreenColor, BlueColor };

			// Histogram
			for (int i = 0; i < (int)rect.width; i++)
			{
				int[] heights = new int[3];

				for (int j = 0; j < 3; j++)
				{
					float remapI = (float)i / rect.width * 255f;
					int index = Mathf.FloorToInt(remapI);
					float fract = remapI - (float)index;
					float v1 = scaledHistogramRGB[index][j];
					float v2 = scaledHistogramRGB[Mathf.Min(index + 1, 255)][j];
					heights[j] = (int)(v1 * (1.0f - fract) + v2 * fract);
				}

				int[] indices = { 0, 1, 2 };
				Array.Sort<int>(indices, (a, b) => heights[a].CompareTo(heights[b]));

				Handles.color = MasterColor;
				Handles.DrawLine(
						new Vector2(rect.x + i, rect.yMax),
						new Vector2(rect.x + i, rect.yMin + (rect.height - heights[indices[0]]))
					);

				Handles.color = colors[indices[2]] + colors[indices[1]];
				Handles.DrawLine(
						new Vector2(rect.x + i, rect.yMin + (rect.height - heights[indices[0]])),
						new Vector2(rect.x + i, rect.yMin + (rect.height - heights[indices[1]]))
					);

				Handles.color = colors[indices[2]];
				Handles.DrawLine(
						new Vector2(rect.x + i, rect.yMin + (rect.height - heights[indices[1]])),
						new Vector2(rect.x + i, rect.yMin + (rect.height - heights[indices[2]]))
					);
			}
		}
	}
}
