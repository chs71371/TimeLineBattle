// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(GradientRampDynamic))]
	public class GradientRampDynamicEditor : BaseEffectEditor
	{
		SerializedProperty p_Ramp;
		SerializedProperty p_Amount;

		void OnEnable()
		{
			p_Ramp = serializedObject.FindProperty("Ramp");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(p_Ramp);
			if (EditorGUI.EndChangeCheck())
				(target as GradientRampDynamic).UpdateGradientCache();

			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
