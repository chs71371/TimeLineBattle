// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(LoFiPalette))]
	public class LoFiPaletteEditor : BaseEffectEditor
	{
		SerializedProperty p_Palette;
		SerializedProperty p_Amount;
		SerializedProperty p_ForceCompatibility;
		SerializedProperty p_Pixelize;
		SerializedProperty p_PixelSize;

		static GUIContent[] palettes = {
				new GUIContent("None"),
				new GUIContent(""),
				new GUIContent("Amstrad CPC"),
				new GUIContent("CGA"),
				new GUIContent("Commodore 64"),
				new GUIContent("Commodore Plus"),
				new GUIContent("EGA"),
				new GUIContent("GameBoy"),
				new GUIContent("MacOS 16"),
				new GUIContent("MacOS 256"),
				new GUIContent("Master System"),
				new GUIContent("RiscOS 16"),
				new GUIContent("Teletex"),
				new GUIContent("Windows 16"),
				new GUIContent("Windows 256"),
				new GUIContent("ZX Spectrum"),
				new GUIContent(""),
				new GUIContent("Andrae"),
				new GUIContent("Anodomani"),
				new GUIContent("Crayolo"),
				new GUIContent("DB16"),
				new GUIContent("DB32"),
				new GUIContent("DJinn"),
				new GUIContent("Drazile A"),
				new GUIContent("Drazile B"),
				new GUIContent("Drazile C"),
				new GUIContent("Eggy"),
				new GUIContent("Finlal A"),
				new GUIContent("Finlal B"),
				new GUIContent("Hapiel"),
				new GUIContent("Pavanz A"),
				new GUIContent("Pavanz B"),
				new GUIContent("Peyton"),
				new GUIContent("SpeedyCube")
			};

		void OnEnable()
		{
			p_Palette = serializedObject.FindProperty("Palette");
			p_Amount = serializedObject.FindProperty("Amount");
			p_ForceCompatibility = serializedObject.FindProperty("ForceCompatibility");
			p_Pixelize = serializedObject.FindProperty("Pixelize");
			p_PixelSize = serializedObject.FindProperty("PixelSize");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Using intValue, enumValueIndex seems broken
			p_Palette.intValue = EditorGUILayout.Popup(GetContent("Palette"), p_Palette.intValue, palettes);
			EditorGUILayout.PropertyField(p_Amount);
			EditorGUILayout.PropertyField(p_ForceCompatibility);
			EditorGUILayout.PropertyField(p_Pixelize);

			if (p_Pixelize.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(p_PixelSize);
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
