// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(CrossStitch))]
	public class CrossStitchEditor : BaseEffectEditor
	{
		SerializedProperty p_Size;
		SerializedProperty p_Brightness;
		SerializedProperty p_Invert;
		SerializedProperty p_Pixelize;

		void OnEnable()
		{
			p_Size = serializedObject.FindProperty("Size");
			p_Brightness = serializedObject.FindProperty("Brightness");
			p_Invert = serializedObject.FindProperty("Invert");
			p_Pixelize = serializedObject.FindProperty("Pixelize");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Size);
			EditorGUILayout.PropertyField(p_Brightness);
			EditorGUILayout.PropertyField(p_Invert);
			EditorGUILayout.PropertyField(p_Pixelize);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
