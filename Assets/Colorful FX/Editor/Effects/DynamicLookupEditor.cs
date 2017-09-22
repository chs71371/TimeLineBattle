// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(DynamicLookup))]
	public class DynamicLookupEditor : BaseEffectEditor
	{
		SerializedProperty p_Amount;
		SerializedProperty p_White;
		SerializedProperty p_Black;
		SerializedProperty p_Red;
		SerializedProperty p_Green;
		SerializedProperty p_Blue;
		SerializedProperty p_Yellow;
		SerializedProperty p_Magenta;
		SerializedProperty p_Cyan;

		static Material m_GraphMaterial;
		static Texture2D m_CursorActive;
		static Texture2D m_CursorInactive;

		SerializedProperty[] m_Colors;
		SerializedProperty m_SelectedColor;
		Rect[] m_ClickRectsWhite;
		Rect[] m_ClickRectsBlack;
		float m_InspectorWidth = 1f;

		void OnEnable()
		{
			p_Amount = serializedObject.FindProperty("Amount");
			p_White = serializedObject.FindProperty("White");
			p_Black = serializedObject.FindProperty("Black");
			p_Red = serializedObject.FindProperty("Red");
			p_Green = serializedObject.FindProperty("Green");
			p_Blue = serializedObject.FindProperty("Blue");
			p_Yellow = serializedObject.FindProperty("Yellow");
			p_Magenta = serializedObject.FindProperty("Magenta");
			p_Cyan = serializedObject.FindProperty("Cyan");

			m_Colors = new SerializedProperty[] { p_White, p_Red, p_Magenta, p_Blue, p_Cyan, p_Green, p_Yellow };
			m_ClickRectsWhite = new Rect[7];
			m_ClickRectsBlack = new Rect[7];
		}

		void CheckResources()
		{
			if (m_GraphMaterial == null)
			{
				m_GraphMaterial = new Material(Shader.Find("Hidden/Colorful/Editor/_DynamicLookup"));
				m_GraphMaterial.hideFlags = HideFlags.HideAndDontSave;
			}

			if (m_CursorActive == null)
				m_CursorActive = Resources.Load<Texture2D>("UI/ColorCubeCursorActive");

			if (m_CursorInactive == null)
				m_CursorInactive = Resources.Load<Texture2D>("UI/ColorCubeCursorInactive");
		}

		void DrawIsometricCube(Rect r, SerializedProperty centerColor, bool seeThrough)
		{
			CheckResources();
			m_Colors[0] = centerColor;
			m_GraphMaterial.SetPass(0);

			// Events
			Event e = Event.current;
			Rect[] clickRects = seeThrough ? m_ClickRectsBlack : m_ClickRectsWhite;

			if (e.type == EventType.MouseDown)
			{
				Vector2 pos = e.mousePosition;

				for (int i = 0; i < m_ClickRectsWhite.Length; i++)
				{
					if (clickRects[i].Contains(pos))
					{
						m_SelectedColor = m_Colors[i];
						e.Use();
						break;
					}
				}
			}

			// Points
			float size = Mathf.Min(r.height, r.width) - 30f;
			float size_2 = size / 2f;
			float size_4 = size / 4f;
			float centerW = r.x + r.width / 2f;
			float centerH = r.y + r.height / 2f + 5f;

			Vector3[] points = {
				new Vector3(centerW, centerH),						// White / Black
				new Vector3(centerW, centerH - size_2),				// Red
				new Vector3(centerW + size_2, centerH - size_4),	// Magenta
				new Vector3(centerW + size_2, centerH + size_4),	// Blue
				new Vector3(centerW, centerH + size_2),				// Cyan
				new Vector3(centerW - size_2, centerH + size_4),	// Green
				new Vector3(centerW - size_2, centerH - size_4),	// Yellow
			};

			// Fill
			GL.PushMatrix();
			{
				GL.Begin(GL.TRIANGLES);
				{
					// Top right
					GL.Color(centerColor.colorValue);	GL.Vertex(points[0]);
					GL.Color(p_Red.colorValue);			GL.Vertex(points[1]);
					GL.Color(p_Magenta.colorValue);		GL.Vertex(points[2]);

					// Right
					GL.Color(centerColor.colorValue);	GL.Vertex(points[0]);
					GL.Color(p_Magenta.colorValue);		GL.Vertex(points[2]);
					GL.Color(p_Blue.colorValue);		GL.Vertex(points[3]);

					// Bottom right
					GL.Color(centerColor.colorValue);	GL.Vertex(points[0]);
					GL.Color(p_Blue.colorValue);		GL.Vertex(points[3]);
					GL.Color(p_Cyan.colorValue);		GL.Vertex(points[4]);

					// Bottom left
					GL.Color(centerColor.colorValue);	GL.Vertex(points[0]);
					GL.Color(p_Cyan.colorValue);		GL.Vertex(points[4]);
					GL.Color(p_Green.colorValue);		GL.Vertex(points[5]);

					// Left
					GL.Color(centerColor.colorValue);	GL.Vertex(points[0]);
					GL.Color(p_Green.colorValue);		GL.Vertex(points[5]);
					GL.Color(p_Yellow.colorValue);		GL.Vertex(points[6]);

					// Top left
					GL.Color(centerColor.colorValue);	GL.Vertex(points[0]);
					GL.Color(p_Yellow.colorValue);		GL.Vertex(points[6]);
					GL.Color(p_Red.colorValue);			GL.Vertex(points[1]);
				}
				GL.End();
			}
			GL.PopMatrix();

			// Inlines
			if (!seeThrough)
			{
				Handles.DrawAAPolyLine(1f, new Vector3[] { points[0], points[2] });
				Handles.DrawAAPolyLine(1f, new Vector3[] { points[0], points[4] });
				Handles.DrawAAPolyLine(1f, new Vector3[] { points[0], points[6] });
			}
			else
			{
				Handles.DrawAAPolyLine(1f, new Vector3[] { points[0], points[1] });
				Handles.DrawAAPolyLine(1f, new Vector3[] { points[0], points[3] });
				Handles.DrawAAPolyLine(1f, new Vector3[] { points[0], points[5] });
			}

			// Outlines
			Handles.color = new Color(1f, 1f, 1f, 2f);
			Handles.DrawAAPolyLine(4f, new Vector3[] {
				points[1], points[2], points[3], points[4],
				points[5], points[6], points[1]
			});

			// Cursors
			for (int i = 0; i < points.Length; i++)
			{
				Rect cursorRect = new Rect(points[i].x - 8f, points[i].y - 8f, 16f, 16f);
				Texture2D cursor = m_CursorInactive;

				if (m_SelectedColor == m_Colors[i])
					cursor = m_CursorActive;

				GUI.DrawTexture(cursorRect, cursor);
				cursorRect = new Rect(cursorRect.x - 6f, cursorRect.y - 6f, cursorRect.width + 12f, cursorRect.height + 12f);

				if (e.type == EventType.Repaint)
				{
					if (seeThrough)
						m_ClickRectsBlack[i] = cursorRect;
					else
						m_ClickRectsWhite[i] = cursorRect;
				}
			}
		}

		void ComputeInspectorWidth()
		{
			// Dirty trick to get the inspector width, works with scrollbars too
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			float w = GUILayoutUtility.GetLastRect().width;

			if (Event.current.type == EventType.Repaint)
				m_InspectorWidth = w;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			ComputeInspectorWidth();

			EditorGUILayout.BeginHorizontal();
			{
				float h = Mathf.Min(m_InspectorWidth / 2f, 216f);

				DrawIsometricCube(GUILayoutUtility.GetRect(50f, h), p_White, false);
				DrawIsometricCube(GUILayoutUtility.GetRect(50f, h), p_Black, true);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			if (m_SelectedColor != null)
				EditorGUILayout.PropertyField(m_SelectedColor);

			EditorGUILayout.PropertyField(p_Amount);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
