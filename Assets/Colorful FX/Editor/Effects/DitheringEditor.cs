// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Dithering))]
	public class DitheringEditor : BaseEffectEditor
	{
		SerializedProperty p_ShowOriginal;
		SerializedProperty p_ConvertToGrayscale;
		SerializedProperty p_RedLuminance;
		SerializedProperty p_GreenLuminance;
		SerializedProperty p_BlueLuminance;
		SerializedProperty p_Amount;

		void OnEnable()
		{
			p_ShowOriginal = serializedObject.FindProperty("ShowOriginal");
			p_ConvertToGrayscale = serializedObject.FindProperty("ConvertToGrayscale");
			p_RedLuminance = serializedObject.FindProperty("RedLuminance");
			p_GreenLuminance = serializedObject.FindProperty("GreenLuminance");
			p_BlueLuminance = serializedObject.FindProperty("BlueLuminance");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_ShowOriginal);
			EditorGUILayout.PropertyField(p_ConvertToGrayscale);

			if (p_ConvertToGrayscale.boolValue)
			{
				EditorGUI.indentLevel++;
				{
					EditorGUILayout.PropertyField(p_RedLuminance, GetContent("Red"));
					EditorGUILayout.PropertyField(p_GreenLuminance, GetContent("Green"));
					EditorGUILayout.PropertyField(p_BlueLuminance, GetContent("Blue"));
				}
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
