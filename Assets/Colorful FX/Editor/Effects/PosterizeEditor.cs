// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Posterize))]
	public class PosterizeEditor : BaseEffectEditor
	{
		SerializedProperty p_Levels;
		SerializedProperty p_Amount;
		SerializedProperty p_LuminosityOnly;

		void OnEnable()
		{
			p_Levels = serializedObject.FindProperty("Levels");
			p_Amount = serializedObject.FindProperty("Amount");
			p_LuminosityOnly = serializedObject.FindProperty("LuminosityOnly");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Levels);
			EditorGUILayout.PropertyField(p_Amount);
			EditorGUILayout.PropertyField(p_LuminosityOnly);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
