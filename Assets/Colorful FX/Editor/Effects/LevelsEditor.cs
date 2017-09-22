// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;
	using UnityEditorInternal;
	using System;
	using ColorMode = Levels.ColorMode;
	using Channel = Levels.Channel;

	[CustomEditor(typeof(Levels))]
	public class LevelsEditor : BaseEffectEditor
	{
		SerializedProperty p_Mode;
		SerializedProperty p_InputL;
		SerializedProperty p_InputR;
		SerializedProperty p_InputG;
		SerializedProperty p_InputB;
		SerializedProperty p_OutputL;
		SerializedProperty p_OutputR;
		SerializedProperty p_OutputG;
		SerializedProperty p_OutputB;

		SerializedProperty p_CurrentChannel;
		SerializedProperty p_Logarithmic;
		SerializedProperty p_AutoRefresh;

		Levels m_Target;
		Texture2D m_TempTexture;
		int[] m_Histogram = new int[256];
		Rect m_HistogramRect = new Rect(0f, 0f, 1f, 1f);

		Color MasterColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 2f) : new Color(0.1f, 0.1f, 0.1f, 2f);
		Color RedColor = new Color(1f, 0f, 0f, 2f);
		Color GreenColor = EditorGUIUtility.isProSkin ? new Color(0f, 1f, 0f, 2f) : new Color(0.2f, 0.8f, 0.2f, 2f);
		Color BlueColor = EditorGUIUtility.isProSkin ? new Color(0f, 1f, 1f, 2f) : new Color(0f, 0f, 1f, 2f);
		Texture2D RampTexture;

		static GUIContent[] presets = {
				new GUIContent("Choose a preset..."),
				new GUIContent("Default"),
				new GUIContent("Darker"),
				new GUIContent("Increase Contrast 1"),
				new GUIContent("Increase Contrast 2"),
				new GUIContent("Increase Contrast 3"),
				new GUIContent("Lighten Shadows"),
				new GUIContent("Lighter"),
				new GUIContent("Midtones Brighter"),
				new GUIContent("Midtones Darker")
			};

		static float[,] presetsData = { { 0, 1, 255, 0, 255 }, { 15, 1, 255, 0, 255 }, { 10, 1, 245, 0, 255 },
									 { 20, 1, 235, 0, 255 }, { 30, 1, 225, 0, 255 }, { 0, 1.6f, 255, 0, 255 },
									 { 0, 1, 230, 0, 255 }, { 0, 1.25f, 255, 0, 255 }, { 0, 0.75f, 255, 0, 255 } };

		void OnEnable()
		{
			p_Mode = serializedObject.FindProperty("Mode");
			p_InputL = serializedObject.FindProperty("InputL");
			p_InputR = serializedObject.FindProperty("InputR");
			p_InputG = serializedObject.FindProperty("InputG");
			p_InputB = serializedObject.FindProperty("InputB");
			p_OutputL = serializedObject.FindProperty("OutputL");
			p_OutputR = serializedObject.FindProperty("OutputR");
			p_OutputG = serializedObject.FindProperty("OutputG");
			p_OutputB = serializedObject.FindProperty("OutputB");

			p_CurrentChannel = serializedObject.FindProperty("e_CurrentChannel");
			p_Logarithmic = serializedObject.FindProperty("e_Logarithmic");
			p_AutoRefresh = serializedObject.FindProperty("e_AutoRefresh");

			RampTexture = Resources.Load<Texture2D>(CLib.IsLinearColorSpace() ? "UI/GrayscaleRampLinear" : "UI/GrayscaleRamp");

			m_Target = target as Levels;
			m_Target.e_OnFrameEnd = UpdateHistogram;
			m_Target.InternalForceRefresh();

			InternalEditorUtility.RepaintAllViews();
		}

		void OnDisable()
		{
			m_Target.e_OnFrameEnd = null;

			if (m_TempTexture != null)
				DestroyImmediate(m_TempTexture);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			{
				bool isRGB = p_Mode.intValue == (int)ColorMode.RGB;
				Channel currentChannel = (Channel)p_CurrentChannel.intValue;

				if (isRGB) currentChannel = (Channel)EditorGUILayout.EnumPopup(currentChannel);
				isRGB = GUILayout.Toggle(isRGB, GetContent("Multi-channel Mode"), EditorStyles.miniButton);

				p_Mode.intValue = isRGB ? (int)ColorMode.RGB : (int)ColorMode.Monochrome;
				p_CurrentChannel.intValue = (int)currentChannel;
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button(GetContent("Auto B&W"), EditorStyles.miniButton))
				{
					int min = 0, max = 255;

					for (int i = 0; i < 256; i++)
					{
						if (m_Histogram[255 - i] > 0)
							min = 255 - i;

						if (m_Histogram[i] > 0)
							max = i;
					}

					if (p_Mode.intValue == (int)ColorMode.RGB)
					{
						if (p_CurrentChannel.intValue == (int)Channel.Red)
						{
							Vector3 input = p_InputR.vector3Value;
							input.x = min;
							input.y = max;
							p_InputR.vector3Value = input;
						}
						else if (p_CurrentChannel.intValue == (int)Channel.Green)
						{
							Vector3 input = p_InputG.vector3Value;
							input.x = min;
							input.y = max;
							p_InputG.vector3Value = input;
						}
						else if (p_CurrentChannel.intValue == (int)Channel.Blue)
						{
							Vector3 input = p_InputB.vector3Value;
							input.x = min;
							input.y = max;
							p_InputB.vector3Value = input;
						}
					}
					else
					{
						Vector3 input = p_InputL.vector3Value;
						input.x = min;
						input.y = max;
						p_InputL.vector3Value = input;
					}
				}

				GUILayout.FlexibleSpace();

				p_Logarithmic.boolValue = GUILayout.Toggle(p_Logarithmic.boolValue, GetContent("Log"), EditorStyles.miniButtonLeft);
				p_AutoRefresh.boolValue = GUILayout.Toggle(p_AutoRefresh.boolValue, GetContent("Auto Refresh"), EditorStyles.miniButtonMid);

				EditorGUI.BeginDisabledGroup(p_AutoRefresh.boolValue);

				if (GUILayout.Button(GetContent("Refresh"), EditorStyles.miniButtonRight))
					Refresh();

				EditorGUI.EndDisabledGroup();
			}
			EditorGUILayout.EndHorizontal();

			if (EditorGUI.EndChangeCheck())
				Refresh();

			EditorGUILayout.Space();

			// Sizing
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
			
			// Histogram
			DrawHistogram(m_HistogramRect);

			// Selected Channel UI
			if (p_Mode.intValue == (int)ColorMode.RGB)
			{
				if (p_CurrentChannel.intValue == (int)Channel.Red) ChannelUI(m_HistogramRect.width, p_InputR, p_OutputR);
				else if (p_CurrentChannel.intValue == (int)Channel.Green) ChannelUI(m_HistogramRect.width, p_InputG, p_OutputG);
				else if (p_CurrentChannel.intValue == (int)Channel.Blue) ChannelUI(m_HistogramRect.width, p_InputB, p_OutputB);
			}
			else
			{
				ChannelUI(m_HistogramRect.width, p_InputL, p_OutputL);
			}

			// Presets
			EditorGUI.BeginChangeCheck();
			int selectedPreset = EditorGUILayout.Popup(GetContent("Preset"), 0, presets);
			if (EditorGUI.EndChangeCheck() && selectedPreset > 0)
			{
				selectedPreset--;
				p_Mode.intValue = (int)ColorMode.Monochrome;
				p_InputL.vector3Value = new Vector3(
						presetsData[selectedPreset, 0],
						presetsData[selectedPreset, 2],
						presetsData[selectedPreset, 1]
					);
				p_OutputL.vector2Value = new Vector2(
						presetsData[selectedPreset, 3],
						presetsData[selectedPreset, 4]
					);
			}

			serializedObject.ApplyModifiedProperties();
		}

		void Refresh()
		{
			m_Target.InternalForceRefresh();
			InternalEditorUtility.RepaintAllViews();
		}

		void ChannelUI(float width, SerializedProperty input, SerializedProperty output)
		{
			float inputMin = input.vector3Value.x;
			float inputGamma = input.vector3Value.z;
			float inputMax = input.vector3Value.y;
			float outputMin = output.vector2Value.x;
			float outputMax = output.vector2Value.y;

			// Input
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.MinMaxSlider(ref inputMin, ref inputMax, 0f, 255f, GUILayout.Width(width));
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal(GUILayout.Width(width));
				{
					inputMin = EditorGUILayout.FloatField((int)inputMin, GUILayout.Width(50));
					GUILayout.FlexibleSpace();
					inputGamma = EditorGUILayout.FloatField(inputGamma, GUILayout.Width(50));
					GUILayout.FlexibleSpace();
					inputMax = EditorGUILayout.FloatField((int)inputMax, GUILayout.Width(50));
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			EditorGUILayout.Space();

			// Ramp
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUI.DrawTexture(GUILayoutUtility.GetRect(width, 20f), RampTexture, ScaleMode.StretchToFill);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			// Output
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.MinMaxSlider(ref outputMin, ref outputMax, 0f, 255f, GUILayout.Width(width));
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal(GUILayout.Width(width));
				{
					outputMin = EditorGUILayout.FloatField((int)outputMin, GUILayout.Width(50));
					GUILayout.FlexibleSpace();
					outputMax = EditorGUILayout.FloatField((int)outputMax, GUILayout.Width(50));
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			input.vector3Value = new Vector3(inputMin, inputMax, Mathf.Clamp(inputGamma, 0.1f, 9.99f));
			output.vector2Value = new Vector2(outputMin, outputMax);

			EditorGUILayout.Separator();
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
			if (p_Mode.intValue == (int)ColorMode.RGB)
			{
				if (p_CurrentChannel.intValue == (int)Channel.Red)
					Handles.color = RedColor;
				else if (p_CurrentChannel.intValue == (int)Channel.Green)
					Handles.color = GreenColor;
				else if (p_CurrentChannel.intValue == (int)Channel.Blue)
					Handles.color = BlueColor;
			}
			else
			{
				Handles.color = MasterColor;
			}

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

			if (m_Target.Mode == ColorMode.Monochrome)
			{
				Array.Clear(m_Histogram, 0, 256);
				for (int i = 0; i < pixels.Length; i++)
				{
					Color c = pixels[i];
					m_Histogram[(int)((c.r * 0.2125f + c.g * 0.7154f + c.b * 0.0721f) * 255)]++;
				}
			}
			else
			{
				switch (m_Target.e_CurrentChannel)
				{
					case Channel.Red:
						Array.Clear(m_Histogram, 0, 256);
						for (int i = 0; i < pixels.Length; i++)
							m_Histogram[(int)(pixels[i].r * 255)]++;
						break;
					case Channel.Green:
						Array.Clear(m_Histogram, 0, 256);
						for (int i = 0; i < pixels.Length; i++)
							m_Histogram[(int)(pixels[i].g * 255)]++;
						break;
					case Channel.Blue:
						Array.Clear(m_Histogram, 0, 256);
						for (int i = 0; i < pixels.Length; i++)
							m_Histogram[(int)(pixels[i].b * 255)]++;
						break;
				}
			}
		}
	}
}
