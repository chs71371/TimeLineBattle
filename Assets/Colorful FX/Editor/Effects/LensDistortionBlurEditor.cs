// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(LensDistortionBlur))]
	public class LensDistortionBlurEditor : BaseEffectEditor
	{
		SerializedProperty p_Quality;
		SerializedProperty p_Samples;
		SerializedProperty p_Distortion;
		SerializedProperty p_CubicDistortion;
		SerializedProperty p_Scale;

		void OnEnable()
		{
			p_Quality = serializedObject.FindProperty("Quality");
			p_Samples = serializedObject.FindProperty("Samples");
			p_Distortion = serializedObject.FindProperty("Distortion");
			p_CubicDistortion = serializedObject.FindProperty("CubicDistortion");
			p_Scale = serializedObject.FindProperty("Scale");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Quality);

			if (p_Quality.intValue == (int)LensDistortionBlur.QualityPreset.Custom)
				EditorGUILayout.PropertyField(p_Samples);

			EditorGUILayout.PropertyField(p_Distortion);
			EditorGUILayout.PropertyField(p_CubicDistortion);
			EditorGUILayout.PropertyField(p_Scale, GetContent("Scale (Zoom)"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}
