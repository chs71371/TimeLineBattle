// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Technicolor))]
	public class TechnicolorEditor : BaseEffectEditor
	{
		SerializedProperty p_Exposure;
		SerializedProperty p_Balance;
		SerializedProperty p_Amount;

		void OnEnable()
		{
			p_Exposure = serializedObject.FindProperty("Exposure");
			p_Balance = serializedObject.FindProperty("Balance");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Exposure);

			EditorGUILayout.LabelField("Balance", EditorStyles.boldLabel);

			EditorGUI.indentLevel++;
			{
				Vector3 balance = p_Balance.vector3Value;
				balance.x = EditorGUILayout.Slider(GetContent("Red"), balance.x, 0f, 1f);
				balance.y = EditorGUILayout.Slider(GetContent("Green"), balance.y, 0f, 1f);
				balance.z = EditorGUILayout.Slider(GetContent("Blue"), balance.z, 0f, 1f);
				p_Balance.vector3Value = balance;
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
