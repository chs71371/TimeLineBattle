// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Strokes))]
	public class StrokesEditor : BaseEffectEditor
	{
		SerializedProperty p_Mode;
		SerializedProperty p_Amplitude;
		SerializedProperty p_Frequency;
		SerializedProperty p_Scaling;
		SerializedProperty p_MaxThickness;
		SerializedProperty p_RedLuminance;
		SerializedProperty p_GreenLuminance;
		SerializedProperty p_BlueLuminance;
		SerializedProperty p_Threshold;
		SerializedProperty p_Harshness;

		void OnEnable()
		{
			p_Mode = serializedObject.FindProperty("Mode");
			p_Amplitude = serializedObject.FindProperty("Amplitude");
			p_Frequency = serializedObject.FindProperty("Frequency");
			p_Scaling = serializedObject.FindProperty("Scaling");
			p_MaxThickness = serializedObject.FindProperty("MaxThickness");
			p_RedLuminance = serializedObject.FindProperty("RedLuminance");
			p_GreenLuminance = serializedObject.FindProperty("GreenLuminance");
			p_BlueLuminance = serializedObject.FindProperty("BlueLuminance");
			p_Threshold = serializedObject.FindProperty("Threshold");
			p_Harshness = serializedObject.FindProperty("Harshness");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(p_Amplitude);
			EditorGUILayout.PropertyField(p_Frequency);
			EditorGUILayout.PropertyField(p_Scaling);
			EditorGUILayout.PropertyField(p_MaxThickness);

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(p_Threshold);
			EditorGUILayout.PropertyField(p_Harshness);

			EditorGUILayout.Space();

			EditorGUILayout.LabelField(GetContent("Contribution"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_RedLuminance, GetContent("Red"));
				EditorGUILayout.PropertyField(p_GreenLuminance, GetContent("Green"));
				EditorGUILayout.PropertyField(p_BlueLuminance, GetContent("Blue"));
			}
			EditorGUI.indentLevel--;

			serializedObject.ApplyModifiedProperties();
		}
	}
}
