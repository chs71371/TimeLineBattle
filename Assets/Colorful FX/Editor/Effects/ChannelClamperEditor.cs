// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(ChannelClamper))]
	public class ChannelClamperEditor : BaseEffectEditor
	{
		SerializedProperty p_Red;
		SerializedProperty p_Green;
		SerializedProperty p_Blue;

		void OnEnable()
		{
			p_Red = serializedObject.FindProperty("Red");
			p_Green = serializedObject.FindProperty("Green");
			p_Blue = serializedObject.FindProperty("Blue");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			Vector2 red = p_Red.vector2Value;
			Vector2 green = p_Green.vector2Value;
			Vector2 blue = p_Blue.vector2Value;

			EditorGUILayout.MinMaxSlider(GetContent("Red Channel|Red channel limits."), ref red.x, ref red.y, 0f, 1f);
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel(" ");
				red.x = EditorGUILayout.FloatField(red.x, GUILayout.Width(60));
				GUILayout.FlexibleSpace();
				red.y = EditorGUILayout.FloatField(red.y, GUILayout.Width(60));
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.MinMaxSlider(GetContent("Green Channel|Green channel limits."), ref green.x, ref green.y, 0f, 1f);
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel(" ");
				green.x = EditorGUILayout.FloatField(green.x, GUILayout.Width(60));
				GUILayout.FlexibleSpace();
				green.y = EditorGUILayout.FloatField(green.y, GUILayout.Width(60));
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.MinMaxSlider(GetContent("Blue Channel|Blue channel limits."), ref blue.x, ref blue.y, 0f, 1f);
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel(" ");
				blue.x = EditorGUILayout.FloatField(blue.x, GUILayout.Width(60));
				GUILayout.FlexibleSpace();
				blue.y = EditorGUILayout.FloatField(blue.y, GUILayout.Width(60));
			}
			EditorGUILayout.EndHorizontal();

			p_Red.vector2Value = red;
			p_Green.vector2Value = green;
			p_Blue.vector2Value = blue;

			serializedObject.ApplyModifiedProperties();
		}
	}
}
