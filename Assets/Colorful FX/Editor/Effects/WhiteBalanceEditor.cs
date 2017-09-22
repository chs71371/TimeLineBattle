// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(WhiteBalance))]
	public class WhiteBalanceEditor : BaseEffectEditor
	{
		SerializedProperty p_White;
		SerializedProperty p_Mode;

		void OnEnable()
		{
			p_White = serializedObject.FindProperty("White");
			p_Mode = serializedObject.FindProperty("Mode");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);
			EditorGUILayout.PropertyField(p_White, GetContent("White Point"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}
