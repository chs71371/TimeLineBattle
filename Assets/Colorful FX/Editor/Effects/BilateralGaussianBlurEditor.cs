// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(BilateralGaussianBlur))]
	public class BilateralGaussianBlurEditor : BaseEffectEditor
	{
		SerializedProperty p_Passes;
		SerializedProperty p_Threshold;
		SerializedProperty p_Amount;

		void OnEnable()
		{
			p_Passes = serializedObject.FindProperty("Passes");
			p_Threshold = serializedObject.FindProperty("Threshold");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Passes);
			EditorGUILayout.PropertyField(p_Threshold);
			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
