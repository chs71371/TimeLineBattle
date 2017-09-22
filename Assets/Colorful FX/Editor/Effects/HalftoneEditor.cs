// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Halftone))]
	public class HalftoneEditor : BaseEffectEditor
	{
		SerializedProperty p_Scale;
		SerializedProperty p_DotSize;
		SerializedProperty p_Angle;
		SerializedProperty p_Smoothness;
		SerializedProperty p_Center;
		SerializedProperty p_Desaturate;

		void OnEnable()
		{
			p_Scale = serializedObject.FindProperty("Scale");
			p_DotSize = serializedObject.FindProperty("DotSize");
			p_Angle = serializedObject.FindProperty("Angle");
			p_Smoothness = serializedObject.FindProperty("Smoothness");
			p_Center = serializedObject.FindProperty("Center");
			p_Desaturate = serializedObject.FindProperty("Desaturate");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Scale);
			EditorGUILayout.PropertyField(p_DotSize);
			EditorGUILayout.PropertyField(p_Smoothness);
			EditorGUILayout.PropertyField(p_Angle);

			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_Center);
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField(p_Desaturate);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
