// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(SCurveContrast))]
	public class SCurveContrastEditor : BaseEffectEditor
	{
		SerializedProperty p_RedSteepness;
		SerializedProperty p_RedGamma;
		SerializedProperty p_GreenSteepness;
		SerializedProperty p_GreenGamma;
		SerializedProperty p_BlueSteepness;
		SerializedProperty p_BlueGamma;
		SerializedProperty p_ShowCurves;

		void OnEnable()
		{
			p_RedSteepness = serializedObject.FindProperty("RedSteepness");
			p_RedGamma = serializedObject.FindProperty("RedGamma");
			p_GreenSteepness = serializedObject.FindProperty("GreenSteepness");
			p_GreenGamma = serializedObject.FindProperty("GreenGamma");
			p_BlueSteepness = serializedObject.FindProperty("BlueSteepness");
			p_BlueGamma = serializedObject.FindProperty("BlueGamma");
			p_ShowCurves = serializedObject.FindProperty("e_ShowCurves");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			EditorGUILayout.LabelField(GetContent("Red"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_RedSteepness, GetContent("Steepness"));
				EditorGUILayout.PropertyField(p_RedGamma, GetContent("Gamma"));
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.LabelField(GetContent("Green"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_GreenSteepness, GetContent("Steepness"));
				EditorGUILayout.PropertyField(p_GreenGamma, GetContent("Gamma"));
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.LabelField(GetContent("Blue"), EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			{
				EditorGUILayout.PropertyField(p_BlueSteepness, GetContent("Steepness"));
				EditorGUILayout.PropertyField(p_BlueGamma, GetContent("Gamma"));
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(p_ShowCurves, GetContent("Show Curves"));

			if (p_ShowCurves.boolValue)
				DrawCurve();

			serializedObject.ApplyModifiedProperties();
		}

		void DrawCurve()
		{
			int h = 200;
			int h_1 = h - 1;
			Rect r = GUILayoutUtility.GetRect(256f, h);
			GUI.Box(r, GUIContent.none);

			float rs = p_RedSteepness.floatValue;
			float rg = p_RedGamma.floatValue;
			float gs = p_GreenSteepness.floatValue;
			float gg = p_GreenGamma.floatValue;
			float bs = p_BlueSteepness.floatValue;
			float bg = p_BlueGamma.floatValue;

			int w = Mathf.FloorToInt(r.width);
			Vector3[] red = new Vector3[w];
			Vector3[] green = new Vector3[w];
			Vector3[] blue = new Vector3[w];

			for (int i = 0; i < w; i++)
			{
				float v = (float)i / (w - 1);
				red[i] = new Vector3(r.x + i, r.y + (h - curve(v, rs, rg) * h_1), 0f);
				green[i] = new Vector3(r.x + i, r.y + (h - curve(v, gs, gg) * h_1), 0f);
				blue[i] = new Vector3(r.x + i, r.y + (h - curve(v, bs, bg) * h_1), 0f);
			}

			Handles.color = EditorGUIUtility.isProSkin ? new Color(0f, 1f, 1f, 2f) : new Color(0f, 0f, 1f, 2f);
			Handles.DrawAAPolyLine(1f, blue);
			Handles.color = EditorGUIUtility.isProSkin ? new Color(0f, 1f, 0f, 2f) : new Color(0.2f, 0.8f, 0.2f, 2f);
			Handles.DrawAAPolyLine(1f, green);
			Handles.color = new Color(1f, 0f, 0f, 2f);
			Handles.DrawAAPolyLine(1f, red);
			Handles.color = Color.white;
		}

		float curve(float o, float steepness, float gamma)
		{
			float g = Mathf.Pow(2f, steepness) * 0.5f;
			float c = (o < 0.5f) ? Mathf.Pow(o, steepness) * g : 1f - Mathf.Pow(1f - o, steepness) * g;
			return Mathf.Clamp01(Mathf.Pow(c, gamma));
		}
	}
}
