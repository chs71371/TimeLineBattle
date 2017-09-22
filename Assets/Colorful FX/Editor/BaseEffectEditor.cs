// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	public class BaseEffectEditor : Editor
	{
		public static GUIStyle tabLeft;
		public static GUIStyle tabMiddle;
		public static GUIStyle tabRight;
		public static GUIStyle tabLeftOn;
		public static GUIStyle tabMiddleOn;
		public static GUIStyle tabRightOn;

		public override void OnInspectorGUI()
		{
			if (tabLeft == null)
			{
				tabLeft = new GUIStyle(EditorStyles.miniButtonLeft);
				tabMiddle = new GUIStyle(EditorStyles.miniButtonMid);
				tabRight = new GUIStyle(EditorStyles.miniButtonRight);

				tabLeftOn = new GUIStyle(tabLeft);
				tabLeftOn.active = tabLeft.onActive;
				tabLeftOn.normal = tabLeft.onNormal;
				tabLeftOn.hover = tabLeft.onHover;

				tabMiddleOn = new GUIStyle(tabMiddle);
				tabMiddleOn.active = tabMiddle.onActive;
				tabMiddleOn.normal = tabMiddle.onNormal;
				tabMiddleOn.hover = tabMiddle.onHover;

				tabRightOn = new GUIStyle(tabRight);
				tabRightOn.active = tabRight.onActive;
				tabRightOn.normal = tabRight.onNormal;
				tabRightOn.hover = tabRight.onHover;
			}
		}

		static Dictionary<string, GUIContent> m_GUIContentCache;

		protected static GUIContent GetContent(string textAndTooltip)
		{
			if (string.IsNullOrEmpty(textAndTooltip))
				return GUIContent.none;

			if (m_GUIContentCache == null)
				m_GUIContentCache = new Dictionary<string, GUIContent>();

			GUIContent content = null;

			if (!m_GUIContentCache.TryGetValue(textAndTooltip, out content))
			{
				string[] s = textAndTooltip.Split('|');
				content = new GUIContent(s[0]);

				if (s.Length > 1 && !string.IsNullOrEmpty(s[1]))
					content.tooltip = s[1];

				m_GUIContentCache.Add(textAndTooltip, content);
			}

			return content;
		}
	}
}
