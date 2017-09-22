// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Vintage))]
	public class VintageEditor : BaseEffectEditor
	{
		SerializedProperty p_Filter;
		SerializedProperty p_Amount;

		void OnEnable()
		{
			p_Filter = serializedObject.FindProperty("Filter");
			p_Amount = serializedObject.FindProperty("Amount");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.HelpBox("This effect is deprecated. Use \"Vintage (Fast)\" instead for better performances!", MessageType.Warning);

			EditorGUILayout.PropertyField(p_Filter);
			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
