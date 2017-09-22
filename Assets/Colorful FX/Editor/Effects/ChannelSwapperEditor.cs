// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(ChannelSwapper))]
	public class ChannelSwapperEditor : BaseEffectEditor
	{
		SerializedProperty p_RedSource;
		SerializedProperty p_GreenSource;
		SerializedProperty p_BlueSource;

		void OnEnable()
		{
			p_RedSource = serializedObject.FindProperty("RedSource");
			p_GreenSource = serializedObject.FindProperty("GreenSource");
			p_BlueSource = serializedObject.FindProperty("BlueSource");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_RedSource);
			EditorGUILayout.PropertyField(p_GreenSource);
			EditorGUILayout.PropertyField(p_BlueSource);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
