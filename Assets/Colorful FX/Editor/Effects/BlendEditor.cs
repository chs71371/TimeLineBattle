// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Blend))]
	public class BlendEditor : BaseEffectEditor
	{
		SerializedProperty p_Amount;
		SerializedProperty p_Texture;
		SerializedProperty p_Mode;

		static GUIContent[] modes = {
				new GUIContent("Darken"),
				new GUIContent("Multiply"),
				new GUIContent("Color Burn"),
				new GUIContent("Linear Burn"),
				new GUIContent("Darker Color"),
				new GUIContent(""),
				new GUIContent("Lighten"),
				new GUIContent("Screen"),
				new GUIContent("Color Dodge"),
				new GUIContent("Linear Dodge (Add)"),
				new GUIContent("Lighter Color"),
				new GUIContent(""),
				new GUIContent("Overlay"),
				new GUIContent("Soft Light"),
				new GUIContent("Hard Light"),
				new GUIContent("Vivid Light"),
				new GUIContent("Linear Light"),
				new GUIContent("Pin Light"),
				new GUIContent("Hard Mix"),
				new GUIContent(""),
				new GUIContent("Difference"),
				new GUIContent("Exclusion"),
				new GUIContent("Subtract"),
				new GUIContent("Divide")
			};

		void OnEnable()
		{
			p_Amount = serializedObject.FindProperty("Amount");
			p_Texture = serializedObject.FindProperty("Texture");
			p_Mode = serializedObject.FindProperty("Mode");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Using intValue, enumValueIndex seems broken in some versions of Unity
			p_Mode.intValue = EditorGUILayout.Popup(GetContent("Mode|Blending Mode."), p_Mode.intValue, modes);
			EditorGUILayout.PropertyField(p_Texture);
			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
