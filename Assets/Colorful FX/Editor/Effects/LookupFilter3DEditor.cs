// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(LookupFilter3D))]
	public class LookupFilter3DEditor : BaseEffectEditor
	{
		SerializedProperty p_LookupTexture;
		SerializedProperty p_Amout;
		SerializedProperty p_ForceCompatibility;

		void OnEnable()
		{
			p_LookupTexture = serializedObject.FindProperty("LookupTexture");
			p_Amout = serializedObject.FindProperty("Amount");
			p_ForceCompatibility = serializedObject.FindProperty("ForceCompatibility");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_LookupTexture);
			EditorGUILayout.PropertyField(p_Amout);
			EditorGUILayout.PropertyField(p_ForceCompatibility);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
