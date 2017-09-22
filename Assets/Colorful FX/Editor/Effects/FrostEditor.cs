// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Frost))]
	public class FrostEditor : BaseEffectEditor
	{
		SerializedProperty p_Scale;
		SerializedProperty p_Sharpness;
		SerializedProperty p_Darkness;
		SerializedProperty p_EnableVignette;

		void OnEnable()
		{
			p_Scale = serializedObject.FindProperty("Scale");
			p_Sharpness = serializedObject.FindProperty("Sharpness");
			p_Darkness = serializedObject.FindProperty("Darkness");
			p_EnableVignette = serializedObject.FindProperty("EnableVignette");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Scale);
			EditorGUILayout.PropertyField(p_EnableVignette, new GUIContent("Vignette"));

			if (p_EnableVignette.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(p_Sharpness);
				EditorGUILayout.PropertyField(p_Darkness);
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
