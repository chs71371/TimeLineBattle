// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(ContrastVignette))]
	public class ContrastVignetteEditor : BaseEffectEditor
	{
		SerializedProperty p_Center;
		SerializedProperty p_Sharpness;
		SerializedProperty p_Darkness;
		SerializedProperty p_Contrast;
		SerializedProperty p_ContrastCoeff;
		SerializedProperty p_EdgeBlending;

		void OnEnable()
		{
			p_Center = serializedObject.FindProperty("Center");
			p_Sharpness = serializedObject.FindProperty("Sharpness");
			p_Darkness = serializedObject.FindProperty("Darkness");
			p_Contrast = serializedObject.FindProperty("Contrast");
			p_ContrastCoeff = serializedObject.FindProperty("ContrastCoeff");
			p_EdgeBlending = serializedObject.FindProperty("EdgeBlending");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Center);
			EditorGUILayout.PropertyField(p_Sharpness);
			EditorGUILayout.PropertyField(p_Darkness);

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(p_Contrast);
			EditorGUI.indentLevel++;
			{
				Vector3 coeff = p_ContrastCoeff.vector3Value;
				coeff.x = EditorGUILayout.Slider(GetContent("Red Channel|Contrast factor on the red channel."), coeff.x, 0f, 1f);
				coeff.y = EditorGUILayout.Slider(GetContent("Green Channel|Contrast factor on the green channel."), coeff.y, 0f, 1f);
				coeff.z = EditorGUILayout.Slider(GetContent("Blue Channel|Contrast factor on the blue channel."), coeff.z, 0f, 1f);
				p_ContrastCoeff.vector3Value = coeff;
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(p_EdgeBlending);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
