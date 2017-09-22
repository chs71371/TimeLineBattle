// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(DirectionalBlur))]
	public class DirectionalBlurEditor : BaseEffectEditor
	{
		SerializedProperty p_Quality;
		SerializedProperty p_Samples;
		SerializedProperty p_Strength;
		SerializedProperty p_Angle;

		void OnEnable()
		{
			p_Quality = serializedObject.FindProperty("Quality");
			p_Samples = serializedObject.FindProperty("Samples");
			p_Strength = serializedObject.FindProperty("Strength");
			p_Angle = serializedObject.FindProperty("Angle");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Quality);

			if (p_Quality.intValue == (int)DirectionalBlur.QualityPreset.Custom)
				EditorGUILayout.PropertyField(p_Samples);

			EditorGUILayout.PropertyField(p_Strength);
			EditorGUILayout.PropertyField(p_Angle);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
