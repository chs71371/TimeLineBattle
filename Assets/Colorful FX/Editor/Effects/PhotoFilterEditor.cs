// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(PhotoFilter))]
	public class PhotoFilterEditor : BaseEffectEditor
	{
		SerializedProperty p_Color;
		SerializedProperty p_Density;

		static GUIContent[] presets = {
				new GUIContent("Choose a preset..."),
				new GUIContent("Warming Filter (85)"),
				new GUIContent("Warming Filter (LBA)"),
				new GUIContent("Warming Filter (81)"),
				new GUIContent("Cooling Filter (80)"),
				new GUIContent("Cooling Filter (LBB)"),
				new GUIContent("Cooling Filter (82)"),
				new GUIContent("Red"),
				new GUIContent("Orange"),
				new GUIContent("Yellow"),
				new GUIContent("Green"),
				new GUIContent("Cyan"),
				new GUIContent("Blue"),
				new GUIContent("Violet"),
				new GUIContent("Magenta"),
				new GUIContent("Sepia"),
				new GUIContent("Deep Red"),
				new GUIContent("Deep Blue"),
				new GUIContent("Deep Emerald"),
				new GUIContent("Deep Yellow"),
				new GUIContent("Underwater")
			};

		static float[,] presetsData = { { 0.925f, 0.541f, 0.0f }, { 0.98f, 0.541f, 0.0f }, { 0.922f, 0.694f, 0.075f },
									 { 0.0f, 0.427f, 1.0f }, { 0.0f, 0.365f, 1.0f }, { 0.0f, 0.71f, 1.0f },
									 { 0.918f, 0.102f, 0.102f }, { 0.956f, 0.518f, 0.09f }, { 0.976f, 0.89f, 0.11f },
									 { 0.098f, 0.788f, 0.098f }, { 0.114f, 0.796f, 0.918f }, { 0.114f, 0.209f, 0.918f },
									 { 0.608f, 0.114f, 0.918f }, { 0.89f, 0.094f, 0.89f }, { 0.675f, 0.478f, 0.2f },
									 { 1.0f, 0.0f, 0.0f }, { 0.0f, 0.133f, 0.804f }, { 0.0f, 0.553f, 0.0f },
									 { 1.0f, 0.835f, 0.0f }, { 0.0f, 0.761f, 0.694f } };

		void OnEnable()
		{
			p_Color = serializedObject.FindProperty("Color");
			p_Density = serializedObject.FindProperty("Density");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Color);
			EditorGUILayout.PropertyField(p_Density);

			EditorGUI.BeginChangeCheck();
			int selectedPreset = EditorGUILayout.Popup(GetContent("Preset"), 0, presets);

			if (EditorGUI.EndChangeCheck() && selectedPreset > 0)
			{
				selectedPreset--;
				p_Color.colorValue = new Color(
						presetsData[selectedPreset, 0],
						presetsData[selectedPreset, 1],
						presetsData[selectedPreset, 2]
					);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
