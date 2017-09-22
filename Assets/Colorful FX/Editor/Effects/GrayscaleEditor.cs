// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Grayscale))]
	public class GrayscaleEditor : BaseEffectEditor
	{
		SerializedProperty p_RedLuminance;
		SerializedProperty p_GreenLuminance;
		SerializedProperty p_BlueLuminance;
		SerializedProperty p_Amount;

		static GUIContent[] presets = {
				new GUIContent("Choose a preset..."),
				new GUIContent("Default"),
				new GUIContent("Unity Default"),
				new GUIContent("Naive")
			};
		static float[,] presetsData = { { 0.299f, 0.587f, 0.114f }, { 0.222f, 0.707f, 0.071f }, { 0.333f, 0.334f, 0.333f } };

		void OnEnable()
		{
			p_RedLuminance = serializedObject.FindProperty("RedLuminance");
			p_GreenLuminance = serializedObject.FindProperty("GreenLuminance");
			p_BlueLuminance = serializedObject.FindProperty("BlueLuminance");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUILayout.Label("Luminance", EditorStyles.boldLabel);

			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_RedLuminance, GetContent("Red"));
				EditorGUILayout.PropertyField(p_GreenLuminance, GetContent("Green"));
				EditorGUILayout.PropertyField(p_BlueLuminance, GetContent("Blue"));
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(p_Amount);

			EditorGUI.BeginChangeCheck();
			int selectedPreset = EditorGUILayout.Popup(GetContent("Preset"), 0, presets);

			if (EditorGUI.EndChangeCheck() && selectedPreset > 0)
			{
				selectedPreset--;
				p_RedLuminance.floatValue = presetsData[selectedPreset, 0];
				p_GreenLuminance.floatValue = presetsData[selectedPreset, 1];
				p_BlueLuminance.floatValue = presetsData[selectedPreset, 2];
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
