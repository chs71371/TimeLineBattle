// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Convolution3x3))]
	public class Convolution3x3Editor : BaseEffectEditor
	{
		SerializedProperty p_Divisor;
		SerializedProperty p_KernelTop;
		SerializedProperty p_KernelMiddle;
		SerializedProperty p_KernelBottom;
		SerializedProperty p_Amount;

		int selectedPreset = 0;
		static GUIContent[] presets = {
				new GUIContent("Default"),
				new GUIContent("Sharpen"),
				new GUIContent("Emboss"),
				new GUIContent("Gaussian Blur"),
				new GUIContent("Laplacian Edge Detection"),
				new GUIContent("Prewitt Edge Detection"),
				new GUIContent("Frei-Chen Edge Detection")
			};
		static Vector3[,] presetsData = { { new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 0f) },
									  { new Vector3(0f, -1f, 0f), new Vector3(-1f, 5f, -1f), new Vector3(0f, -1f, 0f) },
									  { new Vector3(-2f, -1f, 0f), new Vector3(-1f, 1f, 1f), new Vector3(0f, 1f, 2f) },
									  { new Vector3(1f, 2f, 1f), new Vector3(2f, 4f, 2f), new Vector3(1f, 2f, 1f) },
									  { new Vector3(0f, -1f, 0f), new Vector3(-1f, 4f, -1f), new Vector3(0f, -1f, 0f) },
									  { new Vector3(0f, 1f, 1f), new Vector3(-1f, 0f, 1f), new Vector3(-1f, -1f, 0f) },
									  { new Vector3(-1f, -1.4142f, -1f), new Vector3(0f, 0f, 0f), new Vector3(1f, 1.4142f, 1f) } };
		static float[] presetsDiv = { 1f, 1f, 1f, 16f, 1f, 1f, 1f };

		void OnEnable()
		{
			p_Divisor = serializedObject.FindProperty("Divisor");
			p_KernelTop = serializedObject.FindProperty("KernelTop");
			p_KernelMiddle = serializedObject.FindProperty("KernelMiddle");
			p_KernelBottom = serializedObject.FindProperty("KernelBottom");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Divisor);
			p_Divisor.floatValue = Mathf.Max(1e-5f, p_Divisor.floatValue);

			Vector3 temp = p_KernelTop.vector3Value;
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel(GetContent("Kernel|The kernel matrix."));
				temp.x = EditorGUILayout.FloatField(temp.x);
				temp.y = EditorGUILayout.FloatField(temp.y);
				temp.z = EditorGUILayout.FloatField(temp.z);
			}
			EditorGUILayout.EndHorizontal();
			p_KernelTop.vector3Value = temp;

			temp = p_KernelMiddle.vector3Value;
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel(" ");
				temp.x = EditorGUILayout.FloatField(temp.x);
				temp.y = EditorGUILayout.FloatField(temp.y);
				temp.z = EditorGUILayout.FloatField(temp.z);
			}
			EditorGUILayout.EndHorizontal();
			p_KernelMiddle.vector3Value = temp;

			temp = p_KernelBottom.vector3Value;
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel(" ");
				temp.x = EditorGUILayout.FloatField(temp.x);
				temp.y = EditorGUILayout.FloatField(temp.y);
				temp.z = EditorGUILayout.FloatField(temp.z);
			}
			EditorGUILayout.EndHorizontal();
			p_KernelBottom.vector3Value = temp;

			EditorGUILayout.PropertyField(p_Amount);

			EditorGUI.BeginChangeCheck();
			selectedPreset = EditorGUILayout.Popup(GetContent("Preset"), selectedPreset, presets);

			if (EditorGUI.EndChangeCheck())
			{
				p_KernelTop.vector3Value = presetsData[selectedPreset, 0];
				p_KernelMiddle.vector3Value = presetsData[selectedPreset, 1];
				p_KernelBottom.vector3Value = presetsData[selectedPreset, 2];
				p_Divisor.floatValue = presetsDiv[selectedPreset];
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
