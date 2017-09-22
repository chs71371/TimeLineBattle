// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Letterbox))]
	public class LetterboxEditor : BaseEffectEditor
	{
		SerializedProperty p_FillColor;
		SerializedProperty p_Aspect;

		static GUIContent[] presets = {
				new GUIContent("Choose a preset..."),
				new GUIContent("1:1"),
				new GUIContent("5:4"),
				new GUIContent("4:3"),
				new GUIContent("3:2"),
				new GUIContent("16:9"),
				new GUIContent("16:10"),
				new GUIContent("21:9")
			};
		static float[] presetsData = { 1f, 5f/4f, 4f/3f, 3f/2f, 16f/9f, 16f/10f, 21f/9f };

		void OnEnable()
		{
			p_FillColor = serializedObject.FindProperty("FillColor");
			p_Aspect = serializedObject.FindProperty("Aspect");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_FillColor);
			EditorGUILayout.PropertyField(p_Aspect, GetContent("Aspect Ratio"));

			EditorGUI.BeginChangeCheck();
			int selectedPreset = EditorGUILayout.Popup(GetContent("Preset"), 0, presets);

			if (EditorGUI.EndChangeCheck() && selectedPreset > 0)
			{
				selectedPreset--;
				p_Aspect.floatValue = presetsData[selectedPreset];
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
