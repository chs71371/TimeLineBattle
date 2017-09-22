// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(VintageFast))]
	public class VintageFastEditor : BaseEffectEditor
	{
		SerializedProperty p_Filter;
		SerializedProperty p_Amount;
		SerializedProperty p_ForceCompatibility;

		void OnEnable()
		{
			p_Filter = serializedObject.FindProperty("Filter");
			p_Amount = serializedObject.FindProperty("Amount");
			p_ForceCompatibility = serializedObject.FindProperty("ForceCompatibility");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Filter);
			EditorGUILayout.PropertyField(p_Amount);
			EditorGUILayout.PropertyField(p_ForceCompatibility);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
