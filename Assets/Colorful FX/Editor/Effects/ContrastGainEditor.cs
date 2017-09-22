// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(ContrastGain))]
	public class ContrastGainEditor : BaseEffectEditor
	{
		SerializedProperty p_Gain;

		void OnEnable()
		{
			p_Gain = serializedObject.FindProperty("Gain");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Gain);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
