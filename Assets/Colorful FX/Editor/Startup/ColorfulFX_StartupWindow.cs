// Colorful FX - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

#if !(UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0)
#define UNITY_5_1_PLUS
#endif

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class Colorful_StartupWindowProcessor : AssetPostprocessor
{
	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		string[] entries = Array.FindAll(importedAssets, name => name.Contains("ColorfulFX_StartupWindow") && !name.EndsWith(".meta"));

		for (int i = 0; i < entries.Length; i++)
			if (ColorfulFX_StartupWindow.Init(false))
				break;
	}
}

public sealed class ColorfulFX_StartupWindow : EditorWindow
{
	public static string identifier = "TH_Colorful_FX";
	static string pathChangelog = "Assets/Colorful FX/Changelog.txt";
	static string pathImages = "Assets/Colorful FX/Editor/Startup/Images/";

	Texture2D headerPic;
	string changelogText = "";
	Vector2 changelogScroll = Vector2.zero;
	GUIStyle richLabelStyle;
	GUIStyle richButtonStyle;
	GUIStyle iconButtonStyle;
	Texture2D iconColorful;
	Texture2D iconSSAOPro;
	Texture2D iconLutify;

	[MenuItem("Help/Colorful FX/About", false, 0)]
	public static void MenuInit()
	{
		ColorfulFX_StartupWindow.Init(true);
	}

	[MenuItem("Help/Colorful FX/User Manual", false, 0)]
	public static void MenuManual()
	{
		Application.OpenURL("http://thomashourdel.com/colorful/doc/");
	}

	public static void FindAssets()
	{
		// Get the relative data path
		string[] results = AssetDatabase.FindAssets("ColorfulFX_StartupWindow t:Script", null);
		if (results.Length > 0)
		{
			string p = AssetDatabase.GUIDToAssetPath(results[0]);
			p = Path.GetDirectoryName(p);
			p = p.Substring(0, p.LastIndexOf('/'));
			p = p.Substring(0, p.LastIndexOf('/'));
			pathChangelog = p + "/Changelog.txt";
			pathImages = p + "/Editor/Startup/Images/";
		}
	}

	public static T LoadAssetAt<T>(string path) where T : UnityEngine.Object
	{ 
#if UNITY_5_1_PLUS
		return AssetDatabase.LoadAssetAtPath<T>(path);
#else
		return Resources.LoadAssetAtPath<T>(path);
#endif
	}

	public static bool Init(bool forceOpen)
	{
		FindAssets();

		// First line in the changelog is the version string
		TextAsset textAsset = LoadAssetAt<TextAsset>(pathChangelog);

		if (textAsset == null && forceOpen == false)
			forceOpen = true; // Changelog.txt hasn't been imported yet (???)
		else if (textAsset == null)
			return false; // Something's wrong, should never happen

		if (forceOpen || EditorPrefs.GetString(identifier) != GetVersion())
		{
			ColorfulFX_StartupWindow window;
			window = EditorWindow.GetWindow<ColorfulFX_StartupWindow>(true, "About Colorful FX", true);
			Vector2 size = new Vector2(530, 670);
			window.minSize = size;
			window.maxSize = size;
			window.ShowUtility();
			return true;
		}
		
		return false;
	}

	static string GetVersion()
	{
		TextAsset textAsset = LoadAssetAt<TextAsset>(pathChangelog);
		string version = textAsset.text.Split('\n')[0];
		return version;
	}

	void OnEnable()
	{
		FindAssets();

		EditorPrefs.SetString(identifier, GetVersion());

		string versionColor = EditorGUIUtility.isProSkin ? "#ffffffee" : "#000000ee";
		changelogText = LoadAssetAt<TextAsset>(pathChangelog).text;
		int maxLength = 10200;
		bool tooLong = changelogText.Length > maxLength;

		if (tooLong)
		{
			changelogText = changelogText.Substring(0, maxLength);
			changelogText += "...\n\n<color=" + versionColor + ">[See the online changelog for more]</color>";
		}

		changelogText = Regex.Replace(changelogText, @"^[0-9].*", "<color=" + versionColor + "><size=13><b>Version $0</b></size></color>", RegexOptions.Multiline);
		changelogText = Regex.Replace(changelogText, @"^- (\w+:)", "  <color=" + versionColor + ">$0</color>", RegexOptions.Multiline);

		headerPic = LoadAssetAt<Texture2D>(pathImages + "header.jpg");
		iconColorful = LoadAssetAt<Texture2D>(pathImages + "icon-colorful.png");
		iconSSAOPro = LoadAssetAt<Texture2D>(pathImages + "icon-ssaopro.png");
		iconLutify = LoadAssetAt<Texture2D>(pathImages + "icon-lutify.png");
	}

	void OnGUI()
	{
		if (richLabelStyle == null)
		{
			richLabelStyle = new GUIStyle(GUI.skin.label);
			richLabelStyle.richText = true;
			richLabelStyle.wordWrap = true;
			richButtonStyle = new GUIStyle(GUI.skin.button);
			richButtonStyle.richText = true;
			iconButtonStyle = new GUIStyle(GUI.skin.button);
			iconButtonStyle.normal.background = null;
			iconButtonStyle.imagePosition = ImagePosition.ImageOnly;
			iconButtonStyle.fixedWidth = 96;
			iconButtonStyle.fixedHeight = 96;
		}

		Rect headerRect = new Rect(0, 0, 530, 207);
		GUI.DrawTexture(headerRect, headerPic, ScaleMode.ScaleAndCrop, false);

		GUILayout.Space(214);

		GUILayout.BeginVertical();
		{
			HR(0, 2);

			// Doc
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("<b>Documentation</b>\n<size=9>Complete manual, examples, tips & tricks</size>", richButtonStyle, GUILayout.MaxWidth(260), GUILayout.Height(36)))
					Application.OpenURL("http://thomashourdel.com/colorful/doc/");

				if (GUILayout.Button("<b>Rate it</b>\n<size=9>Leave a review on the Asset Store</size>", richButtonStyle, GUILayout.Height(36)))
					Application.OpenURL("com.unity3d.kharma:content/44845");
			}
			GUILayout.EndHorizontal();

			// Contact
			HR(4, 2);

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("<b>E-mail</b>\n<size=9>thomas@hourdel.com</size>", richButtonStyle, GUILayout.MaxWidth(172), GUILayout.Height(36)))
					Application.OpenURL("mailto:thomas@hourdel");

				if (GUILayout.Button("<b>Twitter</b>\n<size=9>@Chman</size>", richButtonStyle, GUILayout.Height(36)))
					Application.OpenURL("http://twitter.com/Chman");

				if (GUILayout.Button("<b>Support Forum</b>\n<size=9>Unity Community</size>", richButtonStyle, GUILayout.MaxWidth(172), GUILayout.Height(36)))
					Application.OpenURL("http://forum.unity3d.com/threads/colorful-post-fx-photoshop-like-color-correction-tools.143417/");
			}
			GUILayout.EndHorizontal();

			// Changelog
			HR(4, 0);

			changelogScroll = GUILayout.BeginScrollView(changelogScroll);
			GUILayout.Label(changelogText, richLabelStyle);
			GUILayout.EndScrollView();

			// Promo
			HR(0, 0);

			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();

				if (GUILayout.Button(iconColorful, iconButtonStyle))
					Application.OpenURL("com.unity3d.kharma:content/44845");

				if (GUILayout.Button(iconSSAOPro, iconButtonStyle))
					Application.OpenURL("com.unity3d.kharma:content/22369");

				if (GUILayout.Button(iconLutify, iconButtonStyle))
					Application.OpenURL("com.unity3d.kharma:content/46012");

				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	void HR(int prevSpace, int nextSpace)
	{
		GUILayout.Space(prevSpace);
		Rect r = GUILayoutUtility.GetRect(Screen.width, 2);
		Color og = GUI.backgroundColor;
		GUI.backgroundColor = Color.black;
		GUI.Box(r, "");
		GUI.backgroundColor = og;
		GUILayout.Space(nextSpace);
	}
}
