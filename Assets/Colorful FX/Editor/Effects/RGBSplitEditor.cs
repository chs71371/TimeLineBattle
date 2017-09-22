// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(RGBSplit))]
	public class RGBSplitEditor : BaseEffectEditor
	{
		SerializedProperty p_Amount;
		SerializedProperty p_Angle;

		void OnEnable()
		{
			p_Amount = serializedObject.FindProperty("Amount");
			p_Angle = serializedObject.FindProperty("Angle");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Amount);
			EditorGUILayout.PropertyField(p_Angle);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
