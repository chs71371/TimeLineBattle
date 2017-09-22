// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(AnalogTV))]
	public class AnalogTVEditor : BaseEffectEditor
	{
		SerializedProperty p_AutomaticPhase;
		SerializedProperty p_Phase;
		SerializedProperty p_ConvertToGrayscale;
		SerializedProperty p_NoiseIntensity;
		SerializedProperty p_ScanlinesIntensity;
		SerializedProperty p_ScanlinesCount;
		SerializedProperty p_ScanlinesOffset;
		SerializedProperty p_VerticalScanlines;
		SerializedProperty p_Distortion;
		SerializedProperty p_CubicDistortion;
		SerializedProperty p_Scale;

		void OnEnable()
		{
			p_AutomaticPhase = serializedObject.FindProperty("AutomaticPhase");
			p_Phase = serializedObject.FindProperty("Phase");
			p_ConvertToGrayscale = serializedObject.FindProperty("ConvertToGrayscale");
			p_NoiseIntensity = serializedObject.FindProperty("NoiseIntensity");
			p_ScanlinesIntensity = serializedObject.FindProperty("ScanlinesIntensity");
			p_ScanlinesCount = serializedObject.FindProperty("ScanlinesCount");
			p_ScanlinesOffset = serializedObject.FindProperty("ScanlinesOffset");
			p_VerticalScanlines = serializedObject.FindProperty("VerticalScanlines");
			p_Distortion = serializedObject.FindProperty("Distortion");
			p_CubicDistortion = serializedObject.FindProperty("CubicDistortion");
			p_Scale = serializedObject.FindProperty("Scale");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_AutomaticPhase);
			EditorGUI.BeginDisabledGroup(p_AutomaticPhase.boolValue);
			{
				EditorGUILayout.PropertyField(p_Phase);
			}
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.PropertyField(p_ConvertToGrayscale);

			GUILayout.Label(GetContent("Analog Effect"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_NoiseIntensity);
				EditorGUILayout.PropertyField(p_ScanlinesIntensity);
				EditorGUILayout.PropertyField(p_ScanlinesCount);
				EditorGUILayout.PropertyField(p_ScanlinesOffset);
				EditorGUILayout.PropertyField(p_VerticalScanlines);
			}
			EditorGUI.indentLevel--;

			GUILayout.Label(GetContent("Barrel Distortion"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_Distortion);
				EditorGUILayout.PropertyField(p_CubicDistortion);
				EditorGUILayout.PropertyField(p_Scale, GetContent("Scale (Zoom)"));
			}
			EditorGUI.indentLevel--;

			serializedObject.ApplyModifiedProperties();
		}
	}
}
