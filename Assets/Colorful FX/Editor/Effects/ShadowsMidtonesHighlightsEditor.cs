// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

#if !(UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0)
#define UNITY_5_1_PLUS
#endif

#if !(UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
#define UNITY_5_3_PLUS
#endif

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(ShadowsMidtonesHighlights))]
	public class ShadowsMidtonesHighlightsEditor : BaseEffectEditor
	{
		SerializedProperty p_Mode;
		SerializedProperty p_Shadows;
		SerializedProperty p_Midtones;
		SerializedProperty p_Highlights;
		SerializedProperty p_Amount;

		ColorWheel m_ShadowsWheel;
		ColorWheel m_MidtonesWheel;
		ColorWheel m_HighlightsWheel;

		void OnEnable()
		{
			p_Mode = serializedObject.FindProperty("Mode");
			p_Shadows = serializedObject.FindProperty("Shadows");
			p_Midtones = serializedObject.FindProperty("Midtones");
			p_Highlights = serializedObject.FindProperty("Highlights");
			p_Amount = serializedObject.FindProperty("Amount");

			m_ShadowsWheel = new ColorWheel("Shadows");
			m_MidtonesWheel = new ColorWheel("Midtones");
			m_HighlightsWheel = new ColorWheel("Highlights");
		}

		void OnDisable()
		{
			m_ShadowsWheel.Destroy();
			m_MidtonesWheel.Destroy();
			m_HighlightsWheel.Destroy();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);

			int size = Mathf.FloorToInt((EditorGUIUtility.currentViewWidth - 100f) / 3);

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				p_Shadows.colorValue = m_ShadowsWheel.DoGUI(p_Shadows.colorValue, size);
				EditorGUILayout.Space();
				p_Midtones.colorValue = m_MidtonesWheel.DoGUI(p_Midtones.colorValue, size);
				EditorGUILayout.Space();
				p_Highlights.colorValue = m_HighlightsWheel.DoGUI(p_Highlights.colorValue, size);
				GUILayout.FlexibleSpace();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(p_Shadows);
			EditorGUILayout.PropertyField(p_Midtones);
			EditorGUILayout.PropertyField(p_Highlights);
			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}

		class ColorWheel
		{
			// Constants
			const int kMinSize = 60;
			const int kMaxSize = 150;

			// Hue Wheel
			Texture2D m_WheelTexture;
			int m_Diameter;
			float m_Radius;
			GUIContent m_Title;

			// UI
			Texture2D m_ThumbTexture;

			// Input utils
			Vector2 m_Cursor;
			ColorWheel m_Dragging;

			internal ColorWheel(string title)
			{
				m_Title = GetContent(title);
				m_Diameter = -1;
				m_ThumbTexture = Resources.Load<Texture2D>("UI/WheelThumb");
			}

			// Cleanup
			internal void Destroy()
			{
				CleanTexture(m_WheelTexture);
			}

			internal Color DoGUI(Color color, int diameter)
			{
				float alpha = color.a;
				diameter = Mathf.Clamp(diameter, kMinSize, kMaxSize);
				Vector3 hsv;

#if UNITY_5_3_PLUS
				Color.RGBToHSV(color, out hsv.x, out hsv.y, out hsv.z);
#else
				EditorGUIUtility.RGBToHSV(color, out hsv.x, out hsv.y, out hsv.z);
#endif

				if (diameter != m_Diameter)
				{
					m_Diameter = diameter;
					m_Radius = diameter / 2f;
					UpdateHueWheel(true);
				}

				EditorGUILayout.BeginVertical();
				{
					// Title
					EditorGUILayout.BeginHorizontal(GUILayout.Width(m_Diameter - 1));
					{
						var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
						centeredStyle.alignment = TextAnchor.UpperCenter;
						GUILayout.Label(m_Title, centeredStyle);
					}
					EditorGUILayout.EndHorizontal();

					// Hue wheel
					EditorGUILayout.BeginHorizontal(GUILayout.Width(m_Diameter));
					{
						Rect wheelRect = GUILayoutUtility.GetRect(m_Diameter, m_Diameter);
						wheelRect.x += 3;

						if (Event.current.type == EventType.Repaint)
						{
							// Wheel
							GUI.DrawTexture(wheelRect, m_WheelTexture);

							// Thumb
							Vector2 thumbPos = Vector2.zero;
							float theta = hsv.x * CLib.PI2;
							float len = hsv.y * m_Radius;
							thumbPos.x = Mathf.Cos(theta + CLib.PI_2);
							thumbPos.y = Mathf.Sin(theta - CLib.PI_2);
							thumbPos *= len;
							GUI.DrawTexture(new Rect(wheelRect.x + m_Radius + thumbPos.x - 4f, wheelRect.y + m_Radius + thumbPos.y - 4f, 8f, 8f), m_ThumbTexture);
						}

						hsv = GetInput(wheelRect, hsv);
					}
					EditorGUILayout.EndHorizontal();
					
#if UNITY_5_3_PLUS
					color = Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
#else
					color = EditorGUIUtility.HSVToRGB(hsv.x, hsv.y, hsv.z);
#endif

					color.a = GUILayout.HorizontalSlider(alpha, 0f, 1f);
				}
				EditorGUILayout.EndVertical();

				return color;
			}

			Vector3 GetInput(Rect bounds, Vector3 hsv)
			{
				Event e = Event.current;

				if (e.type == EventType.MouseDown && e.button == 0)
				{
					Vector2 mousePos = e.mousePosition;

					if (bounds.Contains(mousePos))
					{
						Vector2 center = new Vector2(bounds.x + m_Radius, bounds.y + m_Radius);
						float dist = Vector2.Distance(center, mousePos);

						if (dist <= m_Radius)
						{
							e.Use();
							Vector2 relativePos = mousePos - new Vector2(bounds.x, bounds.y);
							m_Cursor = relativePos;
							GetWheelHueSaturation(m_Cursor.x, m_Cursor.y, ref hsv.x, ref hsv.y);
							m_Dragging = this;
						}
					}
				}
				else if (m_Dragging == this && e.type == EventType.MouseDrag && e.button == 0)
				{
					e.Use();
					float precision = e.alt ? 0.5f : 1f;
					m_Cursor += e.delta * precision;
					GetWheelHueSaturation(Mathf.Clamp(m_Cursor.x, 0f, m_Diameter), Mathf.Clamp(m_Cursor.y, 0f, m_Diameter), ref hsv.x, ref hsv.y);
				}
				else if (m_Dragging == this && e.type == EventType.MouseUp && e.button == 0)
				{
					e.Use();
					m_Dragging = null;
				}

				return hsv;
			}

			void GetWheelHueSaturation(float x, float y, ref float hue, ref float saturation)
			{
				float dx = (float)(x - m_Radius) / m_Radius;
				float dy = (float)(y - m_Radius) / m_Radius;
				float d = Mathf.Sqrt((dx * dx + dy * dy));
				hue = Mathf.Atan2(dx, -dy);
				hue = 1f - ((hue > 0) ? hue : CLib.PI2 + hue) / CLib.PI2;
				saturation = Mathf.Clamp01(d);
			}

			void UpdateHueWheel(bool sizeChanged)
			{
				if (sizeChanged)
				{
					CleanTexture(m_WheelTexture);
					m_WheelTexture = MakeTexture(m_Diameter, m_Diameter);
				}

				Color[] pixels = m_WheelTexture.GetPixels();

				for (int y = 0; y < m_Diameter; y++)
				{
					for (int x = 0; x < m_Diameter; x++)
					{
						int index = y * m_Diameter + x;
						float dx = (float)(x - m_Radius) / m_Radius;
						float dy = (float)(y - m_Radius) / m_Radius;
						float d = Mathf.Sqrt(dx * dx + dy * dy);

						// Out of the wheel, early exit
						if (d >= 1f)
						{
							pixels[index] = new Color(0f, 0f, 0f, 0f);
							continue;
						}

						// Red (0) on top, counter-clockwise (industry standard)
						float saturation = d;
						float hue = Mathf.Atan2(dx, dy);
						hue = 1f - ((hue > 0) ? hue : CLib.PI2 + hue) / CLib.PI2;
						
#if UNITY_5_3_PLUS
						Color color = Color.HSVToRGB(hue, saturation, 1f);
#else
						Color color = EditorGUIUtility.HSVToRGB(hue, saturation, 1f);
#endif

						// Quick & dirty antialiasing
						color.a = (saturation > 0.99) ? (1f - saturation) * 100f : 1f;

						pixels[index] = color;
					}
				}

				m_WheelTexture.SetPixels(pixels);
				m_WheelTexture.Apply();
			}

			Texture2D MakeTexture(int width, int height)
			{
				Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
				tex.filterMode = FilterMode.Point;
				tex.wrapMode = TextureWrapMode.Clamp;
				tex.hideFlags = HideFlags.HideAndDontSave;
				tex.alphaIsTransparency = true; // Used for cheap AA
				return tex;
			}

			void CleanTexture(Texture2D texture)
			{
				if (texture != null)
					DestroyImmediate(texture);
			}
		}
	}
}
