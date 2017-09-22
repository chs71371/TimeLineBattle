// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Threshold))]
	public class ThresholdEditor : BaseEffectEditor
	{
		SerializedProperty p_Value;
		SerializedProperty p_UseNoise;
		SerializedProperty p_NoiseRange;

		void OnEnable()
		{
			p_Value = serializedObject.FindProperty("Value");
			p_UseNoise = serializedObject.FindProperty("UseNoise");
			p_NoiseRange = serializedObject.FindProperty("NoiseRange");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Value);
			EditorGUILayout.PropertyField(p_UseNoise, GetContent("Noise"));

			if (p_UseNoise.boolValue)
			{
				EditorGUI.indentLevel++;
				{
					EditorGUILayout.PropertyField(p_NoiseRange, GetContent("Range"));
				}
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
