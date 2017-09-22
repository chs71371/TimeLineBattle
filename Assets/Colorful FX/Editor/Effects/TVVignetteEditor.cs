// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(TVVignette))]
	public class TVVignetteEditor : BaseEffectEditor
	{
		SerializedProperty p_Size;
		SerializedProperty p_Offset;

		void OnEnable()
		{
			p_Size = serializedObject.FindProperty("Size");
			p_Offset = serializedObject.FindProperty("Offset");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Size);
			EditorGUILayout.PropertyField(p_Offset);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
