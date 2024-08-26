using System;
using UnityEngine;

public class Lang : MonoBehaviour
{
	public class LocalizedString
	{
		public string original;

		public string localized;

		public LocalizedString(string org, string loc)
		{
			original = org;
			localized = loc.Replace("<br>", Environment.NewLine);
		}
	}

	public static string notfound = "STRING_NOT_FOUND";

	public static string[] langlist_org = new string[5] { "ru", "en", "ua", "cn", "jp" };

	public static string[] langlistdesc_org = new string[5] { "РУССКИЙ", "ENGLISH", "УКРАЇНСЬКИЙ", "简体中文", "日本語" };

	public static string[] langlist = new string[5] { "ru", "en", "ua", "cn", "jp" };

	public static int lang = 0;

	public static string langname = "";

	public static string langtranslation = "";

	private static LocalizedString[] strarr = null;

	public static void LoadExternal()
	{
	}

	public static void Init()
	{
		if (MainManager.mycom && PlayerPrefs.HasKey("bp_mycom_lang"))
		{
			lang = PlayerPrefs.GetInt("bp_mycom_lang", 1);
		}
		else if (PlayerPrefs.HasKey("lang"))
		{
			lang = PlayerPrefs.GetInt("lang", 1);
		}
		else
		{
			AutoDetectLang();
		}
		if (lang >= langlist.Length)
		{
			AutoDetectLang();
		}
		Load(langlist[lang]);
	}

	private static void AutoDetectLang()
	{
		lang = 1;
		if (Application.systemLanguage == SystemLanguage.Russian)
		{
			lang = 0;
		}
		else if (Application.systemLanguage == SystemLanguage.Ukrainian)
		{
			lang = 2;
		}
		else if (Application.systemLanguage == SystemLanguage.Belarusian)
		{
			lang = 0;
		}
	}

	public static void Load(string local)
	{
		strarr = null;
		string[] array = null;
		TextAsset textAsset = Resources.Load("local/local_" + local) as TextAsset;
		if (textAsset == null)
		{
			textAsset = Resources.Load("local/community/local_" + local) as TextAsset;
		}
		if (textAsset != null)
		{
			array = textAsset.text.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
		}
		if (array != null)
		{
			int num = 0;
			int i = 0;
			for (int num2 = array.Length; i < num2; i++)
			{
				if (array[i].Split('|').Length == 2)
				{
					num++;
				}
			}
			strarr = new LocalizedString[num];
			num = 0;
			int j = 0;
			for (int num3 = array.Length; j < num3; j++)
			{
				string[] array2 = array[j].Split('|');
				if (array2.Length == 2)
				{
					strarr[num] = new LocalizedString(array2[0], array2[1]);
					num++;
				}
			}
		}
		langname = GetString("_LANGUAGE_NAME");
		langtranslation = GetString("_LANGUAGE_TRANSLATION");
	}

	public static string GetString(string org)
	{
		if (strarr == null)
		{
			return org;
		}
		int i = 0;
		for (int num = strarr.Length; i < num; i++)
		{
			if (string.Compare(org, strarr[i].original) == 0)
			{
				return strarr[i].localized;
			}
		}
		return org;
	}
}
