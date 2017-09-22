// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Led))]
	public class LedEditor : BaseEffectEditor
	{
		SerializedProperty p_Scale;
		SerializedProperty p_Ratio;
		SerializedProperty p_AutomaticRatio;
		SerializedProperty p_Brightness;
		SerializedProperty p_Shape;
		SerializedProperty p_Mode;

		void OnEnable()
		{
			p_Scale = serializedObject.FindProperty("Scale");
			p_Ratio = serializedObject.FindProperty("Ratio");
			p_AutomaticRatio = serializedObject.FindProperty("AutomaticRatio");
			p_Brightness = serializedObject.FindProperty("Brightness");
			p_Shape = serializedObject.FindProperty("Shape");
			p_Mode = serializedObject.FindProperty("Mode");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);
			EditorGUILayout.PropertyField(p_Scale);
			EditorGUILayout.PropertyField(p_Shape);
			EditorGUILayout.PropertyField(p_AutomaticRatio);

			if (!p_AutomaticRatio.boolValue)
			{
				EditorGUI.indentLevel++;
				{
					EditorGUILayout.PropertyField(p_Ratio);
				}
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.PropertyField(p_Brightness);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
