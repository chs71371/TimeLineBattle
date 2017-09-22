// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(SmartSaturation))]
	public class SmartSaturationEditor : BaseEffectEditor
	{
		SerializedProperty p_Curve;
		SerializedProperty p_Boost;

		void OnEnable()
		{
			p_Curve = serializedObject.FindProperty("Curve");
			p_Boost = serializedObject.FindProperty("Boost");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();
			p_Curve.animationCurveValue = EditorGUILayout.CurveField(new GUIContent("Curve", "Selective saturation curve. Refer to the documentation for more information."), p_Curve.animationCurveValue, Color.white, new Rect(0f, 0f, 1f, 1f));
			if (EditorGUI.EndChangeCheck())
				(target as SmartSaturation).UpdateCurve();

			EditorGUILayout.PropertyField(p_Boost);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
