// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

namespace Colorful
{
	using UnityEngine;

	public sealed class MinAttribute : PropertyAttribute
	{
		public readonly float Min;

		public MinAttribute(float min)
		{
			Min = min;
		}
	}
}
