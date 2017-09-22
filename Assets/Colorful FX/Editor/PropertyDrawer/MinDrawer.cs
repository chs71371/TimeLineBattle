// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful.Editors
{
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(MinAttribute))]
	internal sealed class MinDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			MinAttribute attribute = (MinAttribute)base.attribute;

			if (property.propertyType == SerializedPropertyType.Integer)
			{
				int v = EditorGUI.IntField(position, label, property.intValue);
				property.intValue = (int)Mathf.Max(v, attribute.Min);
			}
			else if (property.propertyType == SerializedPropertyType.Float)
			{
				float v = EditorGUI.FloatField(position, label, property.floatValue);
				property.floatValue = Mathf.Max(v, attribute.Min);
			}
			else
			{
				EditorGUI.LabelField(position, label.text, "Use Min with float or int.");
			}
		}
	}
}
