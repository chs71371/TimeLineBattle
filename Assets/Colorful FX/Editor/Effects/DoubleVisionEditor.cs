// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(DoubleVision))]
	public class DoubleVisionEditor : BaseEffectEditor
	{
		SerializedProperty p_Displace;
		SerializedProperty p_Amount;

		void OnEnable()
		{
			p_Displace = serializedObject.FindProperty("Displace");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Displace);
			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
