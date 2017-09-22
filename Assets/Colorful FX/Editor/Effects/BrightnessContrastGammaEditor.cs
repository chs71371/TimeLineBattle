// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(BrightnessContrastGamma))]
	public class BrightnessContrastGammaEditor : BaseEffectEditor
	{
		SerializedProperty p_Brightness;
		SerializedProperty p_Contrast;
		SerializedProperty p_ContrastCoeff;
		SerializedProperty p_Gamma;

		void OnEnable()
		{
			p_Brightness = serializedObject.FindProperty("Brightness");
			p_Contrast = serializedObject.FindProperty("Contrast");
			p_ContrastCoeff = serializedObject.FindProperty("ContrastCoeff");
			p_Gamma = serializedObject.FindProperty("Gamma");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Brightness);

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

			EditorGUILayout.PropertyField(p_Gamma);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
