// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(LookupFilter))]
	public class LookupFilterEditor : BaseEffectEditor
	{
		SerializedProperty p_LookupTexture;
		SerializedProperty p_Amout;

		void OnEnable()
		{
			p_LookupTexture = serializedObject.FindProperty("LookupTexture");
			p_Amout = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.HelpBox("This effect is deprecated. Use \"Lookup Filter 3D\" instead for better performances!", MessageType.Warning);

			EditorGUILayout.PropertyField(p_LookupTexture);
			EditorGUILayout.PropertyField(p_Amout);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
