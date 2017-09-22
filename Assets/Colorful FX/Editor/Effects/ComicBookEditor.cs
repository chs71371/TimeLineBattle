// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(ComicBook))]
	public class ComicBookEditor : BaseEffectEditor
	{
		SerializedProperty p_StripAngle;
		SerializedProperty p_StripDensity;
		SerializedProperty p_StripThickness;
		SerializedProperty p_StripLimits;
		SerializedProperty p_StripInnerColor;
		SerializedProperty p_StripOuterColor;

		SerializedProperty p_FillColor;
		SerializedProperty p_BackgroundColor;

		SerializedProperty p_EdgeDetection;
		SerializedProperty p_EdgeThreshold;
		SerializedProperty p_EdgeColor;

		SerializedProperty p_Amount;

		void OnEnable()
		{
			p_StripAngle = serializedObject.FindProperty("StripAngle");
			p_StripDensity = serializedObject.FindProperty("StripDensity");
			p_StripThickness = serializedObject.FindProperty("StripThickness");
			p_StripLimits = serializedObject.FindProperty("StripLimits");
			p_StripInnerColor = serializedObject.FindProperty("StripInnerColor");
			p_StripOuterColor = serializedObject.FindProperty("StripOuterColor");

			p_FillColor = serializedObject.FindProperty("FillColor");
			p_BackgroundColor = serializedObject.FindProperty("BackgroundColor");

			p_EdgeDetection = serializedObject.FindProperty("EdgeDetection");
			p_EdgeThreshold = serializedObject.FindProperty("EdgeThreshold");
			p_EdgeColor = serializedObject.FindProperty("EdgeColor");

			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_FillColor);
			EditorGUILayout.PropertyField(p_BackgroundColor);

			GUILayout.Label(GetContent("Strips"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_StripAngle, GetContent("Angle"));
				EditorGUILayout.PropertyField(p_StripDensity, GetContent("Density"));
				EditorGUILayout.PropertyField(p_StripThickness, GetContent("Thickness"));
				Vector2 l = p_StripLimits.vector2Value;
				EditorGUILayout.MinMaxSlider(GetContent("Limits|Luminance limits used to draw the strips. Pixel under the lower bound will be filled with the Fill Color."), ref l.x, ref l.y, 0f, 1.01f);
				p_StripLimits.vector2Value = l;
				EditorGUILayout.PropertyField(p_StripInnerColor, GetContent("Inner Color"));
				EditorGUILayout.PropertyField(p_StripOuterColor, GetContent("Outer Color"));
			}
			EditorGUI.indentLevel--;

			GUILayout.Label(GetContent("Edge Detection"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_EdgeDetection, GetContent("Toggle"));

				if (p_EdgeDetection.boolValue)
				{
					EditorGUILayout.PropertyField(p_EdgeThreshold, GetContent("Threshold"));
					EditorGUILayout.PropertyField(p_EdgeColor, GetContent("Color"));
				}
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
