using System;
using System.Collections.Generic;
using GameClass;
using UnityEngine;

public class MainManager : MonoBehaviour
{
	public static int state = 0;

	public static bool authed = false;

	public static bool error = false;

	public static string error_msg = "";

	public static bool inorder = false;

	public static bool proto = true;

	public static bool steam = false;

	public static bool idc = false;

	public static bool mycom = false;

	public static int private_key = 0;

	public static bool delaydrop = false;

	public static string mkey = "_";

	public static bool mod = false;

	public static string steamlang = "";

	private string t = "null";

	private string u = "null";

	public static string sz_pers_id = "null";

	public static string sz_token = "null";

	public static string ui_locale = "ru-RU";

	public static string ui_currency = "RUB";

	private static int devselect = 0;

	public static GameObject goGUI = null;

	public static GameObject goCore = null;

	public static GameObject goUI = null;

	public static List<byte[]> bytemaplist = null;

	private void Awake()
	{
		Application.runInBackground = true;
		Lang.Init();
		Client.Init();
		TEX.Init();
		GUIM.Init();
		GOP.Init();
		Palette.Init();
		ContentLoader_.Init();
		ScopeGen.Init();
		goGUI = GameObject.Find("GUI");
		ContentLoader_.BroadcastAll("LoadLang", "");
		goCore = GameObject.Find("Core");
		goCore.GetComponent<Client>().PostAwake();
		goUI = GameObject.Find("UI");
		InitBaseSettings();
		SteamInit();
		MyComPlatformInit();
	}

	private void InitBaseSettings()
	{
		Application.targetFrameRate = 200;
		Application.targetFrameRate = 60;
		base.gameObject.AddComponent<IAPManager>();
	}

	private void SteamInit()
	{
	}

	private void MyComPlatformInit()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i].Contains("idctk") && i + 1 < commandLineArgs.Length)
			{
				t = commandLineArgs[i + 1];
			}
			else if (commandLineArgs[i].Contains("idcuuid") && i + 1 < commandLineArgs.Length)
			{
				u = commandLineArgs[i + 1];
			}
		}
		if (t != "null" && u != "null")
		{
			idc = true;
			if (!PlayerPrefs.HasKey("lang"))
			{
				Lang.lang = 1;
				PlayerPrefs.SetInt("lang", Lang.lang);
				Lang.Init();
				ContentLoader_.BroadcastAll("LoadLang", "");
			}
		}
		for (int j = 0; j < commandLineArgs.Length; j++)
		{
			if (commandLineArgs[j].Contains("sz_pers_id"))
			{
				string[] array = commandLineArgs[j].Split('=');
				if (array.Length > 1)
				{
					sz_pers_id = array[1];
				}
			}
			else if (commandLineArgs[j].Contains("sz_token"))
			{
				string[] array2 = commandLineArgs[j].Split('=');
				if (array2.Length > 1)
				{
					sz_token = array2[1];
				}
			}
			else if (commandLineArgs[j].Contains("my_ui_locale_arg"))
			{
				string[] array3 = commandLineArgs[j].Split('=');
				if (array3.Length > 1)
				{
					ui_locale = array3[1];
				}
			}
			else if (commandLineArgs[j].Contains("my_ui_currency_arg"))
			{
				string[] array4 = commandLineArgs[j].Split('=');
				if (array4.Length > 1)
				{
					ui_currency = array4[1];
				}
			}
		}
		if (!(sz_pers_id != "null") || !(sz_token != "null"))
		{
			return;
		}
		mycom = true;
		Main.sLeftMsg = "MY.GAMES";
		if (!PlayerPrefs.HasKey("bp_mycom_lang"))
		{
			Lang.lang = 1;
			if (ui_locale == "ru-RU")
			{
				Lang.lang = 0;
			}
			PlayerPrefs.SetInt("bp_mycom_lang", Lang.lang);
			Lang.Init();
			ContentLoader_.BroadcastAll("LoadLang", "");
		}
	}

	private void CheckPrivateKey()
	{
	}

	private void CheckSteam()
	{
	}

	public static void SetState(int val)
	{
		state = val;
	}

	private void LoadEndFirst()
	{
		Application.ExternalCall("get_data");
	}

	private void cb_get_auth_id(string value)
	{
		MasterClient.vk_id = value;
	}

	private void cb_get_auth_key(string value)
	{
		MasterClient.vk_key = value;
		MasterClient.cs.Connect();
	}

	private void cb_get_policy(string value)
	{
		MasterClient.IP = value;
	}

	private void cb_onfocus()
	{
		if (inorder)
		{
			inorder = false;
			MasterClient.cs.send_reload();
		}
	}

	private void cb_onsettings()
	{
	}

	private void cb_set_uu(string value)
	{
	}

	private void cb_clear_map()
	{
		GUIMap.maplist.Clear();
	}

	private void cb_add_map(string data)
	{
		string[] array = data.Split('|');
		GUIMap.maplist.Add(new MapData(array[0], array[1], array[2], array[3], array[4], 0));
	}

	private void cb_set_mapsaved()
	{
		SaveMap.inSave = false;
		SaveMap.Saved = true;
	}

	private void cb_import_map(string value)
	{
		byte[] array = Convert.FromBase64String(value.Split(',')[1]);
		Map.LoadBin("00", array, array.Length);
		GUIMap.SPAWN_AFTER_LOAD();
	}

	private void cb_import_skin(string value)
	{
		byte[] data = Convert.FromBase64String(value.Split(',')[1]);
		GUISkinEditor.tSkinImport = new Texture2D(2, 2);
		GUISkinEditor.tSkinImport.LoadImage(data);
		GUISkinEditor.wname = GUISkinEditor.GetWeaponName(GUISkinEditor.tSkinImport.GetPixels32());
		Main.HideMenus();
		Main.SetActive(false);
		GameObject.Find("GUI").AddComponent<GUISkinEditor>();
	}

	private void cb_import_mapbyte(string value)
	{
		if (bytemaplist == null)
		{
			bytemaplist = new List<byte[]>();
		}
		byte[] array = Convert.FromBase64String(value.Split(',')[1]);
		bytemaplist.Add(array);
		Console.Log("map added to memory list " + array.Length);
		if (GUIAdminUpload.cs != null)
		{
			GUIAdminUpload.cs.LoadMapList();
		}
	}
}
