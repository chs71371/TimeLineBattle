// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Kuwahara))]
	public class KuwaharaEditor : BaseEffectEditor
	{
		SerializedProperty p_Radius;

		void OnEnable()
		{
			p_Radius = serializedObject.FindProperty("Radius");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(p_Radius);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
