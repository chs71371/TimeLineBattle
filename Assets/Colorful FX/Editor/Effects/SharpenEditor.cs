// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Sharpen))]
	public class SharpenEditor : BaseEffectEditor
	{
		SerializedProperty p_Mode;
		SerializedProperty p_Strength;
		SerializedProperty p_Clamp;

		void OnEnable()
		{
			p_Mode = serializedObject.FindProperty("Mode");
			p_Strength = serializedObject.FindProperty("Strength");
			p_Clamp = serializedObject.FindProperty("Clamp");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Mode);
			EditorGUILayout.PropertyField(p_Strength);

			if (p_Mode.intValue == (int)Sharpen.Algorithm.TypeA)
				EditorGUILayout.PropertyField(p_Clamp);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
