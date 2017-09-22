// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(HueFocus))]
	public class HueFocusEditor : BaseEffectEditor
	{
		SerializedProperty p_Hue;
		SerializedProperty p_Range;
		SerializedProperty p_Boost;
		SerializedProperty p_Amount;
		Texture2D m_HueRamp;

		void OnEnable()
		{
			p_Hue = serializedObject.FindProperty("Hue");
			p_Range = serializedObject.FindProperty("Range");
			p_Boost = serializedObject.FindProperty("Boost");
			p_Amount = serializedObject.FindProperty("Amount");

			m_HueRamp = Resources.Load<Texture2D>("UI/HueRamp");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.Separator();

			Rect rect = GUILayoutUtility.GetRect(0, 20);
			GUI.DrawTextureWithTexCoords(rect, m_HueRamp, new Rect(0.5f + p_Hue.floatValue / 360f, 0f, 1f, 1f));

			GUI.enabled = false;
			float min = 180f - p_Range.floatValue;
			float max = 180f + p_Range.floatValue;
			EditorGUILayout.MinMaxSlider(ref min, ref max, 0f, 360f);
			GUI.enabled = true;

			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(p_Hue);
			EditorGUILayout.PropertyField(p_Range);
			EditorGUILayout.PropertyField(p_Boost);
			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
