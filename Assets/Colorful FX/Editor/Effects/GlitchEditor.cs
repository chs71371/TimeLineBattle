// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Glitch))]
	public class GlitchEditor : BaseEffectEditor
	{
		SerializedProperty p_RandomActivation;
		SerializedProperty p_RandomEvery;
		SerializedProperty p_RandomDuration;

		SerializedProperty p_Mode;
		SerializedProperty p_InterferencesSettings;
		SerializedProperty p_TearingSettings;

		SerializedProperty p_InterferencesSpeed;
		SerializedProperty p_InterferencesDensity;
		SerializedProperty p_InterferencesMaxDisplacement;

		SerializedProperty p_TearingSpeed;
		SerializedProperty p_TearingIntensity;
		SerializedProperty p_TearingMaxDisplacement;
		SerializedProperty p_TearingAllowFlipping;
		SerializedProperty p_TearingYuvColorBleeding;
		SerializedProperty p_TearingYuvOffset;

		void OnEnable()
		{
			p_RandomActivation = serializedObject.FindProperty("RandomActivation");
			p_RandomEvery = serializedObject.FindProperty("RandomEvery");
			p_RandomDuration = serializedObject.FindProperty("RandomDuration");

			p_Mode = serializedObject.FindProperty("Mode");
			p_InterferencesSettings = serializedObject.FindProperty("SettingsInterferences");
			p_TearingSettings = serializedObject.FindProperty("SettingsTearing");

			p_InterferencesSpeed = p_InterferencesSettings.FindPropertyRelative("Speed");
			p_InterferencesDensity = p_InterferencesSettings.FindPropertyRelative("Density");
			p_InterferencesMaxDisplacement = p_InterferencesSettings.FindPropertyRelative("MaxDisplacement");

			p_TearingSpeed = p_TearingSettings.FindPropertyRelative("Speed");
			p_TearingIntensity = p_TearingSettings.FindPropertyRelative("Intensity");
			p_TearingMaxDisplacement = p_TearingSettings.FindPropertyRelative("MaxDisplacement");
			p_TearingAllowFlipping = p_TearingSettings.FindPropertyRelative("AllowFlipping");
			p_TearingYuvColorBleeding = p_TearingSettings.FindPropertyRelative("YuvColorBleeding");
			p_TearingYuvOffset = p_TearingSettings.FindPropertyRelative("YuvOffset");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_RandomActivation);

			if (p_RandomActivation.boolValue)
			{
				DoTimingUI(p_RandomEvery, GetContent("Every"), 50f);
				DoTimingUI(p_RandomDuration, GetContent("For"), 50f);
				EditorGUILayout.Space();
			}

			EditorGUILayout.PropertyField(p_Mode);

			if (p_Mode.enumValueIndex == (int)Glitch.GlitchingMode.Interferences)
			{
				DoInterferencesUI();
			}
			else if (p_Mode.enumValueIndex == (int)Glitch.GlitchingMode.Tearing)
			{
				DoTearingUI();
			}
			else // Complete
			{
				EditorGUILayout.LabelField(GetContent("Interferences"), EditorStyles.boldLabel);

				EditorGUI.indentLevel++;
				{
					DoInterferencesUI();
				}
				EditorGUI.indentLevel--;

				EditorGUILayout.Space();
				EditorGUILayout.LabelField(GetContent("Tearing"), EditorStyles.boldLabel);

				EditorGUI.indentLevel++;
				{
					DoTearingUI();
				}
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}

		void DoInterferencesUI()
		{
			EditorGUILayout.PropertyField(p_InterferencesSpeed);
			EditorGUILayout.PropertyField(p_InterferencesDensity);
			EditorGUILayout.PropertyField(p_InterferencesMaxDisplacement);
		}

		void DoTearingUI()
		{
			EditorGUILayout.PropertyField(p_TearingSpeed);
			EditorGUILayout.PropertyField(p_TearingIntensity);
			EditorGUILayout.PropertyField(p_TearingMaxDisplacement);
			EditorGUILayout.PropertyField(p_TearingYuvColorBleeding, GetContent("YUV Color Bleeding"));

			if (p_TearingYuvColorBleeding.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(p_TearingYuvOffset, GetContent("Offset"));
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.PropertyField(p_TearingAllowFlipping);
		}

		void DoTimingUI(SerializedProperty prop, GUIContent label, float labelWidth)
		{
			Vector2 v = prop.vector2Value;

			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(EditorGUIUtility.labelWidth - 3);
				GUILayout.Label(label, GUILayout.ExpandWidth(false), GUILayout.Width(labelWidth));
				v.x = EditorGUILayout.FloatField(v.x, GUILayout.MaxWidth(75));
				GUILayout.Label(GetContent("to"), GUILayout.ExpandWidth(false));
				v.y = EditorGUILayout.FloatField(v.y, GUILayout.MaxWidth(75));
				GUILayout.Label(GetContent("second(s)"), GUILayout.ExpandWidth(false));
			}
			EditorGUILayout.EndHorizontal();

			prop.vector2Value = v;
		}
	}
}
