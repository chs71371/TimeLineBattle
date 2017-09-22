// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(GrainyBlur))]
	public class GrainyBlurEditor : BaseEffectEditor
	{
		SerializedProperty p_Radius;
		SerializedProperty p_Samples;

		void OnEnable()
		{
			p_Radius = serializedObject.FindProperty("Radius");
			p_Samples = serializedObject.FindProperty("Samples");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Radius);
			EditorGUILayout.PropertyField(p_Samples);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
