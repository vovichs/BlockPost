using System;
using System.Collections.Generic;
using System.Globalization;
using UIClass;
using UnityEngine;
using VK.Unity;
using VK.Unity.Responses;

public class Main : MonoBehaviour
{
	public static bool show = false;

	public static int gid = 0;

	private static Texture2D tBlack = null;

	private static Texture2D tWhite = null;

	private static Texture2D tYellow = null;

	private static Texture2D tGradient = null;

	private static Texture2D tGreen = null;

	public static Texture2D tCoin = null;

	private static GameObject goCamera = null;

	public static Camera cam;

	public static Texture2D tVig = null;

	private static Texture2D tLogo = null;

	private static Texture2D tLogoEn = null;

	public static Texture2D tLogo_mini = null;

	public static Texture2D tLogoEn_mini = null;

	private static Texture2D tOptions = null;

	private static Texture2D tFullScreen = null;

	public static Texture2D tAvatar = null;

	private Texture2D tSale;

	private Texture2D tExit;

	private Texture2D tVK;

	public static int state = 0;

	private static Rect rScreen;

	private static Rect rOptions;

	private static Rect rFull;

	private static Rect rName;

	private static Rect rNameText;

	private static Rect rGold;

	private static Rect rGoldText;

	public static Rect rLogoMini;

	private Rect rExit;

	private Vector2 pOptions;

	private Vector2 pFull;

	private float angle;

	private float scale = 1f;

	private int scaleforward = 1;

	private Rect rEventLeft;

	private Rect rEventRight;

	public static string sLeftMsg = "VK.COM/BLOCKPOSTCLUB";

	public static string sRightMsg = "";

	public static string sBuildMsg = "";

	private string sPlay;

	private string sInventory;

	private string sWeapon;

	private string sCharacter;

	private string sCases;

	private string sProfile;

	private string sShop;

	private string sKeys;

	private string sSuit;

	private string sWorkshop;

	private string sParse;

	private string sUpgrade;

	private string sBuyingCase;

	private string sTasks;

	private string sCombat;

	private string sBonus;

	private string sRating;

	private string sOptions;

	private string sGame;

	private string sGraphics;

	private string sControl;

	private string sSound;

	private string sSelectName;

	private string sCompletion;

	private string sClans;

	private string sMyClan;

	private string sFindClan;

	private string sMessages;

	private string sClickChangeName;

	private string sClickFillCoins;

	private string sClickContinue;

	private string sPlayers;

	private string sClanChat;

	private string sTapContinue;

	private string sLoginGame;

	private string sLoginVK;

	private static string sAuthSuccess;

	private static string sYouKilledPlayer;

	private static string sYouKilled;

	private static string sSurvive;

	private static string sMin;

	private static string sNextEnter;

	private static string sGameOver;

	public static string sOnline;

	private string[] sAuth = new string[3];

	private Color[] cAuth = new Color[3]
	{
		new Color(1f, 1f, 1f, 0.5f),
		new Color(1f, 1f, 1f, 0.75f),
		new Color(1f, 1f, 1f, 1f)
	};

	public static bool mauth = false;

	public static bool macc = false;

	private static CButtonMenu[] Button = null;

	private static CButtonMenu currButton = null;

	public static int currSubMenu = -1;

	private static int MAX_MENU = 7;

	public static bool lockmenu = false;

	private Color cg = new Color(0.8f, 0.8f, 0.8f, 1f);

	public static bool halloween = false;

	public static bool winter = false;

	private static Texture2D tWinterLeft = null;

	private static Texture2D tWinterRight = null;

	private Rect rWinterLeft;

	private Rect rWinterRight;

	public static bool unlockclan = true;

	private Rect rVig;

	private Rect rLogo;

	private Rect rLeftMsg;

	private Rect rRightMsg;

	private Rect rCenterMsg;

	private Rect rGoogle;

	private Rect rVK;

	private Rect rBottom;

	private Matrix4x4 matrixBackup;

	private Color hc = new Color(0.313f, 0.196f, 0f);

	private static float btime = 0f;

	private static float bsize = 0f;

	private static float bforward = 1f;

	private static CButtonMenu bcn = null;

	private static float bw = 0f;

	private static string header;

	private static string line0;

	private static string line1;

	private static string line2;

	private static string line3;

	private static bool showdevmsg = false;

	public void LoadLang()
	{
		sAuth[0] = Lang.GetString("_WAITING_AUTH") + ".";
		sAuth[1] = Lang.GetString("_WAITING_AUTH") + "..";
		sAuth[2] = Lang.GetString("_WAITING_AUTH") + "...";
		sPlay = Lang.GetString("_PLAY");
		sInventory = Lang.GetString("_INVENTORY");
		sWeapon = Lang.GetString("_MENU_WEAPON");
		sCharacter = Lang.GetString("_CHARACTER");
		sCases = Lang.GetString("_CASES");
		sProfile = Lang.GetString("_PROFILE");
		sShop = Lang.GetString("_SHOP");
		sKeys = Lang.GetString("_KEYS");
		sSuit = Lang.GetString("_SUIT");
		sWorkshop = Lang.GetString("_WORKSHOP");
		sParse = Lang.GetString("_PARSE");
		sUpgrade = Lang.GetString("_UPGRADE");
		sBuyingCase = Lang.GetString("_BUYING_CASES");
		sTasks = Lang.GetString("_TASKS");
		sCombat = Lang.GetString("_COMBAT");
		sBonus = Lang.GetString("_BONUSES");
		sRating = Lang.GetString("_RATING");
		sOptions = Lang.GetString("_OPTIONS");
		sGame = Lang.GetString("_GAME");
		sGraphics = Lang.GetString("_GRAPHICS");
		sControl = Lang.GetString("_CONTROL");
		sSound = Lang.GetString("_SOUND");
		sSelectName = Lang.GetString("_SELECT_NAME");
		sCompletion = Lang.GetString("_COMPLETION");
		sClans = Lang.GetString("_CLANS");
		sMyClan = Lang.GetString("_MY_CLAN");
		sFindClan = Lang.GetString("_FIND_CLAN");
		sMessages = Lang.GetString("_MESSAGES");
		sClickChangeName = Lang.GetString("_CLICK_CHANGE_NAME");
		sClickFillCoins = Lang.GetString("_CLICK_FILL_COINS");
		sClickContinue = Lang.GetString("_CLICK_CONTINUE");
		sAuthSuccess = Lang.GetString("_AUTH_SUCCESSFULLY");
		sYouKilledPlayer = Lang.GetString("_YOU_KILLED_PLAYER");
		sYouKilled = Lang.GetString("_YOU_KILLED");
		sSurvive = Lang.GetString("_SURVIVAL_TIME");
		sMin = Lang.GetString("_MIN");
		sNextEnter = Lang.GetString("_SERVER_AVAILABLE_WILL_BE");
		sGameOver = Lang.GetString("_GAME_OVER");
		sClanChat = Lang.GetString("_CLANCHAT");
		sPlayers = Lang.GetString("_PLAYERS");
		sTapContinue = Lang.GetString("_TAP_CONTINUE");
		sOnline = Lang.GetString("_ONLINE");
		sLoginGame = Lang.GetString("_LOGIN_GAME");
		sLoginVK = Lang.GetString("_LOGIN_VK");
		InitMenu();
		_ResizeMenu();
	}

	private void LoadEnd()
	{
		for (int i = 0; i < ContentLoader_.materialList.Count; i++)
		{
			Material material = ContentLoader_.materialList[i] as Material;
			if (material.name == "legacy_transparent")
			{
				material.shader = Shader.Find("Sprites/Default");
			}
		}
	}

	public void LoadEndFirst()
	{
		tVig = ContentLoader_.LoadTexture("vig") as Texture2D;
		tLogo = ContentLoader_.LoadTexture("gamelogo") as Texture2D;
		tLogoEn = Resources.Load("preload/gamelogo_en") as Texture2D;
		tBlack = TEX.GetTextureByName("gray0");
		tYellow = TEX.GetTextureByName("yellow");
		tWhite = TEX.GetTextureByName("white");
		tGreen = TEX.GetTextureByName("green");
		tOptions = ContentLoader_.LoadTexture("options") as Texture2D;
		tGradient = ContentLoader_.LoadTexture("top_gradient") as Texture2D;
		tCoin = ContentLoader_.LoadTexture("coin") as Texture2D;
		tFullScreen = Resources.Load("fullscreen") as Texture2D;
		DateTime now = DateTime.Now;
		int month = now.Month;
		int day = now.Day;
		if ((month == 10 && day >= 25) || (month == 11 && day < 5))
		{
			halloween = true;
		}
		else if ((month == 12 && day >= 15) || (month == 1 && day < 15))
		{
			winter = true;
		}
		if (halloween)
		{
			tWinterLeft = Resources.Load("texevent/blockpost_halloween_right") as Texture2D;
			tWinterRight = Resources.Load("texevent/blockpost_halloween_left") as Texture2D;
		}
		else if (winter)
		{
			tWinterLeft = Resources.Load("texevent/blockpost_snowman") as Texture2D;
			tWinterRight = Resources.Load("texevent/blockpost_snow") as Texture2D;
		}
		tSale = Resources.Load("sale") as Texture2D;
		tExit = Resources.Load("sa_quit") as Texture2D;
		tVK = Resources.Load("vkicon") as Texture2D;
		InitMenu();
		SetActive(true);
		OnResize();
		base.gameObject.AddComponent<GP2>();
		base.gameObject.AddComponent<VKWeb>();
	}

	private void InitMenu()
	{
		Button = new CButtonMenu[11];
		Button[0] = new CButtonMenu(0, "play", sPlay, null);
		Button[1] = new CButtonMenu(1, "inventory", sInventory, new string[3] { sWeapon, sCharacter, sCases });
		Button[2] = new CButtonMenu(2, "profiel", sProfile, null);
		Button[3] = new CButtonMenu(3, "shop", sShop, new string[3] { sCases, sKeys, sSuit });
		if (MainManager.steam)
		{
			Button[4] = new CButtonMenu(4, "workshop", sWorkshop, null);
			Button[5] = new CButtonMenu(5, "tasks", sTasks, null);
		}
		else
		{
			Button[4] = new CButtonMenu(4, "workshop", sWorkshop, new string[2] { sParse, sBuyingCase });
			Button[5] = new CButtonMenu(5, "tasks", sTasks, new string[2] { sCombat, sBonus });
		}
		Button[6] = new CButtonMenu(6, "rating", sRating, new string[2] { sPlayers, sClans });
		Button[7] = new CButtonMenu(7, "options", sOptions, new string[4] { sGame, sGraphics, sControl, sSound });
		Button[8] = new CButtonMenu(8, "selectname", sSelectName, null);
		Button[9] = new CButtonMenu(9, "gold", sCompletion, null);
		Button[10] = new CButtonMenu(10, "clans", sClans, new string[4] { sMyClan, sFindClan, sMessages, sClanChat });
		if (MainManager.steam)
		{
			sLeftMsg = "STEAM";
		}
		else if (MainManager.idc)
		{
			sLeftMsg = "IDC/GAMES";
		}
		sBuildMsg = Application.version;
	}

	public static void SetActive(bool val)
	{
		if (val)
		{
			Map.Clear();
			Controll.SetLockMove(false);
			Controll.SetLockLook(false);
			Controll.SetLockAttack(false);
			Controll.Clear();
			GUIGameMenu.SetActive(false);
			SaveMap.SetActive(false);
			Palette.SetActive(false);
			HUDBuild.SetActive(false);
			Crosshair.SetActive(false);
			HUD.SetActive(false);
			HUDGameEnd.SetActive(false);
			HUDTab.SetActive(false);
			GUIPlay.SetActive(false);
			Crosshair.SetCursor(true);
			goCamera = new GameObject();
			goCamera.transform.position = new Vector3(0f, 0f, -1f);
			goCamera.transform.eulerAngles = Vector3.zero;
			goCamera.name = "MenuCamera";
			cam = goCamera.AddComponent<Camera>();
			cam.nearClipPlane = 0.01f;
			cam.orthographic = true;
			cam.orthographicSize = 5f;
			cam.clearFlags = CameraClearFlags.Color;
			cam.eventMask = 0;
			if (winter)
			{
				cam.backgroundColor = new Color(24f / 85f, 0.30980393f, 27f / 85f);
			}
			else if (halloween)
			{
				cam.backgroundColor = new Color(27f / 85f, 27f / 85f, 4f / 15f);
			}
			else
			{
				cam.backgroundColor = new Color(27f / 85f, 0.30980393f, 24f / 85f);
			}
			cam.cullingMask = 256;
			Controll.pl.currweapon = null;
			PLH.ClearWeapon(Controll.pl);
			Controll.freefly = false;
			Radar.SetActive(false);
			MainBack.Create();
			Controll.pl.idx = -1;
			StartMenu();
			if (MainManager.delaydrop)
			{
				MasterClient.cs.drop();
			}
		}
		else
		{
			MainBack.Clear();
			UnityEngine.Object.Destroy(goCamera);
		}
		show = val;
	}

	public static void SelectMenu(int menu, int submenu)
	{
		HideMenus();
		currButton = Button[menu];
		currSubMenu = submenu;
	}

	public static void SetMenuNewItem(int menu)
	{
		Button[menu].newitem++;
	}

	private void OnGUI()
	{
		if (show && !GUIOptions.configtouch)
		{
			if (GUIM.HideButton(new Rect(0f, 0f, GUIM.YRES(100f), GUIM.YRES(100f))) && Console.cs != null)
			{
				Console.cs.ToggleActive();
			}
			if (state == 0)
			{
				DrawStart();
			}
			else if (state == 1)
			{
				DrawMenu();
			}
		}
	}

	private void DrawInfo()
	{
		rName.Set((float)Screen.width - GUIM.YRES(200f), GUIM.YRES(70f), GUIM.YRES(200f), GUIM.YRES(32f));
		rNameText.Set(rName.x - GUIM.YRES(200f), rName.y, GUIM.YRES(180f), rName.height);
		if (GUIM.Contains(rName))
		{
			GUIM.DrawBox(rName, TEX.tOrange, 0.5f);
			GUIM.DrawText(rNameText, sClickChangeName, TextAnchor.MiddleRight, BaseColor.White, 1, 14, true);
		}
		else
		{
			GUIM.DrawBox(rName, tBlack, 0.05f);
		}
		Rect position = new Rect((float)Screen.width - GUIM.YRES(200f) + GUIM.YRES(4f), GUIM.YRES(70f) + GUIM.YRES(1f), GUIM.YRES(28f), GUIM.YRES(28f));
		if (tAvatar != null)
		{
			GUI.DrawTexture(position, tAvatar);
		}
		GUIM.DrawText(new Rect((float)Screen.width - GUIM.YRES(200f) + GUIM.YRES(36f), GUIM.YRES(70f) + GUIM.YRES(7f), GUIM.YRES(160f), GUIM.YRES(24f)), GUIOptions.playername, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 14, true);
		if (GUIM.HideButton(rName) && !lockmenu)
		{
			HideMenus();
			currButton = Button[8];
			currSubMenu = 0;
			GUIName.SetActive(true);
		}
		rGold.Set((float)Screen.width - GUIM.YRES(120f), GUIM.YRES(104f), GUIM.YRES(200f), GUIM.YRES(32f));
		rGoldText.Set(rGold.x - GUIM.YRES(200f), rGold.y, GUIM.YRES(180f), rGold.height);
		if (GUIM.Contains(rGold))
		{
			GUIM.DrawBox(rGold, TEX.tOrange, 0.5f);
			GUIM.DrawText(rGoldText, sClickFillCoins, TextAnchor.MiddleRight, BaseColor.White, 1, 14, true);
		}
		else
		{
			GUIM.DrawBox(rGold, tBlack, 0.05f);
		}
		GUI.DrawTexture(new Rect((float)Screen.width - GUIM.YRES(120f) + GUIM.YRES(4f), GUIM.YRES(104f) + GUIM.YRES(4f), GUIM.YRES(24f), GUIM.YRES(24f)), tCoin);
		GUIM.DrawText(new Rect((float)Screen.width - GUIM.YRES(120f) + GUIM.YRES(32f), GUIM.YRES(104f) + GUIM.YRES(7f), GUIM.YRES(24f), GUIM.YRES(24f)), GUIOptions.sGold, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 14, true);
		if (GUIM.HideButton(rGold) && !lockmenu)
		{
			EmulateClick(9);
		}
		if (GUIGold.discount)
		{
			GUI.DrawTexture(new Rect(rGold.x + GUIM.YRES(80f), rGold.y + GUIM.YRES(4f), GUIM.YRES(24f), GUIM.YRES(24f)), tSale);
		}
	}

	public static void EmulateClick(int button_id)
	{
		if (button_id == 1)
		{
			HideMenus();
			currButton = Button[1];
			currSubMenu = 0;
			GUIInv.SetActive(true);
		}
		if (button_id == 3)
		{
			HideMenus();
			currButton = Button[3];
			currSubMenu = 1;
			GUIShop.SetActive(true);
		}
		if (button_id == 9)
		{
			HideMenus();
			currButton = Button[9];
			currSubMenu = 0;
			GUIGold.SetActive(true);
		}
	}

	private void OnResize()
	{
		rLogoMini = new Rect(0f, GUIM.YRES(3f), (int)GUIM.YRES(58f) * 2, (int)GUIM.YRES(58f));
		if (tLogo != null)
		{
			tLogo_mini = ScaleTexture(tLogo, (int)GUIM.YRES(58f) * 2, (int)GUIM.YRES(58f));
		}
		if (tLogoEn != null)
		{
			tLogoEn_mini = ScaleTexture(tLogoEn, (int)GUIM.YRES(58f) * 2, (int)GUIM.YRES(58f));
		}
		rFull = new Rect((float)Screen.width - GUIM.YRES(120f), GUIM.YRES(12f), GUIM.YRES(40f), GUIM.YRES(40f));
		pFull = new Vector2(rFull.xMin + rFull.width * 0.5f, rFull.yMin + rFull.height * 0.5f);
		rOptions = new Rect((float)Screen.width - GUIM.YRES(64f), GUIM.YRES(12f), GUIM.YRES(40f), GUIM.YRES(40f));
		pOptions = new Vector2(rOptions.xMin + rOptions.width * 0.5f, rOptions.yMin + rOptions.height * 0.5f);
		rExit = new Rect((float)Screen.width - GUIM.YRES(18f), GUIM.YRES(4f), GUIM.YRES(14f), GUIM.YRES(14f));
		rVig = new Rect(0f, 0f, Screen.width, Screen.height);
		rLogo = new Rect((float)Screen.width / 2f - GUIM.YRES(160f), GUIM.YRES(80f), GUIM.YRES(320f), GUIM.YRES(160f));
		rLeftMsg = new Rect(GUIM.YRES(16f), (float)Screen.height - GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(32f));
		rRightMsg = new Rect((float)Screen.width - GUIM.YRES(216f), (float)Screen.height - GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(32f));
		rCenterMsg = new Rect((float)Screen.width / 2f - GUIM.YRES(100f), (float)Screen.height - GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(32f));
		rGoogle = new Rect((float)Screen.width / 2f - GUIM.YRES(130f), GUIM.YRES(340f), GUIM.YRES(260f), GUIM.YRES(60f));
		rVK = new Rect((float)Screen.width / 2f - GUIM.YRES(130f), GUIM.YRES(420f), GUIM.YRES(260f), GUIM.YRES(60f));
		rBottom = new Rect(0f, (float)Screen.height - GUIM.YRES(32f), Screen.width, GUIM.YRES(32f));
		rEventLeft = new Rect(GUIM.YRES(22f), (float)Screen.height - GUIM.YRES(32f) - GUIM.YRES(120f), GUIM.YRES(128f), GUIM.YRES(128f));
		rEventRight = new Rect((float)Screen.width - GUIM.YRES(278f), (float)Screen.height - GUIM.YRES(32f) - GUIM.YRES(128f), GUIM.YRES(256f), GUIM.YRES(128f));
		_ResizeMenu();
	}

	private void _ResizeMenu()
	{
		if (Button[0] == null)
		{
			return;
		}
		float num = GUIM.YRES(16f);
		float num2 = 0f;
		for (int i = 0; i < 11; i++)
		{
			if (i != 7 && i != 8 && i != 9)
			{
				Button[i].width = GUIM.CalcSize(Button[i].text, 1, 20).x + num;
				num2 += Button[i].width;
			}
		}
		Button[0].r = new Rect((float)Screen.width / 2f - num2 / 2f, GUIM.YRES(16f), Button[0].width, GUIM.YRES(40f));
		Button[1].r = new Rect(Button[0].r.x + Button[0].r.width, GUIM.YRES(16f), Button[1].width, GUIM.YRES(40f));
		Button[2].r = new Rect(Button[1].r.x + Button[1].r.width, GUIM.YRES(16f), Button[2].width, GUIM.YRES(40f));
		Button[3].r = new Rect(Button[2].r.x + Button[2].r.width, GUIM.YRES(16f), Button[3].width, GUIM.YRES(40f));
		Button[4].r = new Rect(Button[3].r.x + Button[3].r.width, GUIM.YRES(16f), Button[4].width, GUIM.YRES(40f));
		Button[5].r = new Rect(Button[4].r.x + Button[4].r.width, GUIM.YRES(16f), Button[5].width, GUIM.YRES(40f));
		Button[6].r = new Rect(Button[5].r.x + Button[5].r.width, GUIM.YRES(16f), Button[6].width, GUIM.YRES(40f));
		Button[10].r = new Rect(Button[6].r.x + Button[6].r.width, GUIM.YRES(16f), Button[10].width, GUIM.YRES(40f));
		for (int j = 0; j < 11; j++)
		{
			CalcButton2(Button[j]);
		}
	}

	private void CalcButton2(CButtonMenu b)
	{
		if (b.text2 != null)
		{
			float num = GUIM.YRES(20f);
			float height = GUIM.YRES(56f);
			float y = GUIM.YRES(74f);
			int fontsize = 20;
			int num2 = b.text2.Length;
			float x = (float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(124f) + GUIM.YRES(8f);
			b.r2[0] = new Rect(x, y, GUIM.CalcSize(b.text2[0], 1, fontsize).x + num, height);
			for (int i = 1; i < num2; i++)
			{
				b.r2[i] = new Rect(b.r2[i - 1].x + b.r2[i - 1].width + num / 2f, y, GUIM.CalcSize(b.text2[1], 1, fontsize).x + num, height);
			}
			for (int j = 0; j < num2; j++)
			{
				b.r2_text[j] = new Rect(b.r2[j].x, b.r2[j].y + GUIM.YRES(2f), b.r2[j].width, b.r2[j].height);
			}
		}
	}

	public static void HideMenus()
	{
		GUIChar.SetActive(false);
		GUIPlay.SetActive(false);
		GUIOptions.SetActive(false);
		GUIInv.SetActive(false);
		GUIProfile.SetActive(false);
		GUIName.SetActive(false);
		GUIGold.SetActive(false);
		GUIShop.SetActive(false);
		GUICraft.SetActive(false);
		GUIObj.SetActive(false);
		GUIRank.SetActive(false);
		GUIClan.SetActive(false);
		MainManager.goUI.SendMessage("HideMenu");
	}

	private void DrawStart()
	{
		GUI.color = Color.black;
		GUI.DrawTexture(rVig, tVig);
		GUI.color = Color.white;
		if (Lang.lang == 0)
		{
			GUI.DrawTexture(rLogo, tLogo);
		}
		else
		{
			GUI.DrawTexture(rLogo, tLogoEn);
		}
		GUIM.DrawText(rLeftMsg, sLeftMsg, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
		GUIM.DrawText(rRightMsg, sRightMsg, TextAnchor.MiddleRight, BaseColor.White, 0, 20, true);
		GUIM.DrawText(rCenterMsg, sBuildMsg, TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, true);
		if (MainManager.error)
		{
			GUIM.DrawText(new Rect(0f, GUIM.YRES(400f), Screen.width, GUIM.YRES(32f)), MainManager.error_msg, TextAnchor.MiddleCenter, MasterClient.proxy_change ? BaseColor.Yellow : BaseColor.Orange, 0, 28, true);
		}
		else if (MainManager.authed)
		{
			if (ContentLoader_.proceed)
			{
				GUIM.DrawText(new Rect(0f, GUIM.YRES(400f), Screen.width, GUIM.YRES(32f)), sTapContinue, TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 28, true);
				if (GUIM.HideButton(new Rect(0f, 0f, Screen.width, Screen.height)))
				{
					state = 1;
					StartMenu();
					GUIProfile.Load();
					GUIOptions.Load();
					GUIOptions.LoadConfigTouch();
					GUIInv.LoadChar();
					GUIInv.Load();
					MasterClient.cs.send_inv();
					MasterClient.cs.send_clanbase();
					MasterClient.cs.send_bonus(0);
					if (PlayerPrefs.HasKey("bp_quest"))
					{
						GUIObj.currqid = PlayerPrefs.GetInt("bp_quest");
						MasterClient.cs.send_loadquest(GUIObj.currqid);
					}
					else
					{
						MasterClient.cs.send_loadquest(-1);
					}
					if (MainManager.steam && PlayerPrefs.HasKey("steam_order"))
					{
						string @string = PlayerPrefs.GetString("steam_order");
						ulong result = 0uL;
						if (!ulong.TryParse(@string, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
						{
							return;
						}
						MasterClient.cs.send_microtxnstate(1, result);
					}
				}
			}
			else
			{
				GUIM.DrawText(new Rect(0f, GUIM.YRES(400f), Screen.width, GUIM.YRES(32f)), sAuthSuccess, TextAnchor.MiddleCenter, BaseColor.Green, 0, 28, true);
			}
		}
		else if (!mauth)
		{
			GUI.DrawTexture(rGoogle, TEX.tYellow);
			GUIM.DrawText(rGoogle, sLoginGame, TextAnchor.MiddleCenter, BaseColor.Black, 0, 28, false);
			if (GUIM.HideButton(rGoogle))
			{
				if (PlayerPrefs.HasKey("bp_mobile_auth_id") && PlayerPrefs.HasKey("bp_mobile_auth_key"))
				{
					Log.AddMainLog("Continue auth");
					MasterClient.vk_id = PlayerPrefs.GetString("bp_mobile_auth_id");
					MasterClient.vk_key = PlayerPrefs.GetString("bp_mobile_auth_key");
					GP.force = true;
					GP.auth = true;
					MasterClient.IP = MainConfig.DEFAULT_IP;
					MasterClient.PORT = 7780;
					MasterClient.cs.Connect();
				}
				else
				{
					GP2.cs.Auth();
				}
				mauth = true;
			}
			float x = GUIM.CalcSize(sLoginVK, 0, 28).x;
			Rect position = new Rect(rVK.x + (rVK.width - x) / 2f - GUIM.YRES(40f), rVK.y + GUIM.YRES(10f), GUIM.YRES(40f), GUIM.YRES(40f));
			GUI.DrawTexture(rVK, TEX.tVK);
			GUI.DrawTexture(position, tVK);
			GUIM.DrawText(rVK, "   " + sLoginVK, TextAnchor.MiddleCenter, BaseColor.White, 0, 28, false);
			if (GUIM.HideButton(rVK))
			{
				MasterClient.IP = MainConfig.DEFAULT_IP;
				MasterClient.PORT = 7778;
				mauth = true;
				Log.AddMainLog("VK.Init");
				if (!VKSDK.IsInitialized)
				{
					VKSDK.Init(VKOnInitComplete);
				}
			}
		}
		else if (!macc)
		{
			int num = (int)(Time.time * 3f) % 3;
			int num2 = (int)(Time.time * 10f) % 10;
			GUI.color = new Color(1f, 1f, 1f, 0.5f + (float)num2 / 15f);
			GUIM.DrawText(new Rect((float)Screen.width / 2f - GUIM.YRES(100f), GUIM.YRES(400f), Screen.width, GUIM.YRES(32f)), sAuth[num], TextAnchor.MiddleLeft, BaseColor.White, 0, 28, true);
			GUI.color = Color.white;
		}
		Log.DrawLog();
	}

	private static void StartMenu()
	{
		if (state == 1)
		{
			SelectMenu(Button[0]);
			MainBack.SetTransparent(0.25f, new Color(0f, 0f, 0f, 0.05f));
			MainBack.ClampSpeed(true);
		}
	}

	private static void CharTest()
	{
		HideMenus();
		GUIChar.SetActive(true);
	}

	private void DrawMenu()
	{
		GUI.color = Color.black;
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), tVig);
		GUI.color = Color.white;
		DrawInfo();
		GUIM.DrawBox(new Rect(0f, 0f, Screen.width, GUIM.YRES(64f)), tBlack);
		if (Lang.lang == 0)
		{
			GUI.DrawTexture(new Rect(rLogoMini.x, rLogoMini.y, tLogo_mini.width, tLogo_mini.height), tLogo_mini);
		}
		else
		{
			GUI.DrawTexture(new Rect(rLogoMini.x, rLogoMini.y, tLogoEn_mini.width, tLogoEn_mini.height), tLogoEn_mini);
		}
		for (int i = 0; i < MAX_MENU; i++)
		{
			DrawButtonTop(Button[i]);
		}
		if (unlockclan)
		{
			DrawButtonTop(Button[10]);
		}
		DrawActiveMenu();
		DrawOptions();
		DrawBottom();
		DrawDevMsg();
	}

	private void DrawFullScreen()
	{
		matrixBackup = GUI.matrix;
		if (GUIM.Contains(rFull))
		{
			GUI.color = Color.yellow;
			scale += Time.deltaTime * (float)scaleforward * 0.5f;
			if (scale > 1.1f)
			{
				scale = 1.1f;
				scaleforward = -1;
			}
			if (scale < 0.9f)
			{
				scale = 0.9f;
				scaleforward = 1;
			}
			GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), pFull);
			GUI.DrawTexture(rFull, tFullScreen);
			GUI.matrix = matrixBackup;
		}
		else
		{
			GUI.color = cg;
			GUI.DrawTexture(rFull, tFullScreen);
			GUI.color = Color.white;
		}
		if (GUIM.HideButton(rFull))
		{
			Controll.SetFullScreen();
		}
	}

	private void DrawOptions()
	{
		if (currButton == Button[7])
		{
			GUI.color = GUIM.colorlist[7];
			angle += 64f * Time.smoothDeltaTime;
		}
		else if (GUIM.Contains(rOptions))
		{
			GUI.color = Color.yellow;
			angle += 64f * Time.smoothDeltaTime;
		}
		else
		{
			GUI.color = cg;
		}
		if (angle > 45f)
		{
			angle -= 45f;
		}
		float num = (int)angle;
		matrixBackup = GUI.matrix;
		GUIUtility.RotateAroundPivot(num, pOptions);
		if (tOptions != null)
		{
			GUI.DrawTexture(rOptions, tOptions);
		}
		GUI.matrix = matrixBackup;
		if (GUIM.HideButton(rOptions) && !lockmenu)
		{
			currButton = Button[7];
			currSubMenu = 0;
			HideMenus();
			GUIOptions.SetActive(true);
		}
		GUI.color = Color.white;
	}

	public static Matrix4x4 RotateX(float aAngleRad)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m11 = (identity.m22 = Mathf.Cos(aAngleRad));
		identity.m21 = Mathf.Sin(aAngleRad);
		identity.m12 = 0f - identity.m21;
		return identity;
	}

	private void DrawExit()
	{
		GUI.DrawTexture(rExit, tBlack);
		if (GUIM.Contains(rExit))
		{
			GUI.color = Color.red;
		}
		else
		{
			GUI.color = Color.gray;
		}
		GUI.DrawTexture(rExit, tExit);
		GUI.color = Color.white;
		if (GUIM.HideButton(rExit))
		{
			Application.Quit();
		}
	}

	private void DrawBottom()
	{
		if (winter || halloween)
		{
			GUI.DrawTexture(rEventLeft, tWinterLeft);
			GUI.DrawTexture(rEventRight, tWinterRight);
			if (winter)
			{
				GUI.DrawTexture(rBottom, tWhite);
				GUIM.DrawText(rLeftMsg, sLeftMsg, TextAnchor.MiddleLeft, BaseColor.Blue, 0, 20, false);
				GUIM.DrawText(rRightMsg, sRightMsg, TextAnchor.MiddleRight, BaseColor.Blue, 0, 20, false);
			}
			else if (halloween)
			{
				GUI.color = hc;
				GUI.DrawTexture(rBottom, tWhite);
				GUI.color = Color.white;
				GUIM.DrawText(rLeftMsg, sLeftMsg, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
				GUIM.DrawText(rRightMsg, sRightMsg, TextAnchor.MiddleRight, BaseColor.White, 0, 20, true);
			}
			else
			{
				GUIM.DrawBox(rBottom, tBlack);
				GUIM.DrawText(rLeftMsg, sLeftMsg, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
				GUIM.DrawText(rRightMsg, sRightMsg, TextAnchor.MiddleRight, BaseColor.White, 0, 20, true);
			}
		}
		else
		{
			GUIM.DrawBox(rBottom, tBlack);
			GUIM.DrawText(rLeftMsg, sLeftMsg, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
			GUIM.DrawText(rRightMsg, sRightMsg, TextAnchor.MiddleRight, BaseColor.White, 0, 20, true);
		}
		GUIM.DrawText(rCenterMsg, sBuildMsg, TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, true);
	}

	public static void DrawActiveMenu()
	{
		GUIM.DrawBox(new Rect(0f, GUIM.YRES(86f), (float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(124f), GUIM.YRES(32f)), tBlack, 0.05f);
		GUIM.DrawText(new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(90f), GUIM.YRES(200f), GUIM.YRES(32f)), currButton.text, TextAnchor.MiddleLeft, BaseColor.Gray, 1, 20, false);
		if (currButton.text2 != null)
		{
			for (int i = 0; i < currButton.text2.Length; i++)
			{
				DrawButtonTop2(i, currButton.r2[i], currButton.r2_text[i], currButton.text2[i]);
			}
		}
	}

	private static void DrawButtonTop(CButtonMenu b)
	{
		Rect r = b.r;
		bool flag = GUIM.Contains(r);
		if (b == currButton)
		{
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(r.x, 0f, r.width, GUIM.YRES(64f)), tGradient);
			GUI.color = Color.white;
			GUIM.DrawText(r, b.text, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 14, true);
		}
		else if (b.idx == 5)
		{
			GUIM.DrawText(r, b.text, TextAnchor.MiddleCenter, (!flag) ? BaseColor.White : BaseColor.Yellow, 1, 14, true);
		}
		else
		{
			GUIM.DrawText(r, b.text, TextAnchor.MiddleCenter, flag ? BaseColor.Yellow : BaseColor.LightGray2, 1, 14, true);
		}
		if (b.newitem > 0)
		{
			GUI.DrawTexture(new Rect(r.x + r.width - GUIM.YRES(12f), GUIM.YRES(7f), GUIM.YRES(6f), GUIM.YRES(6f)), tGreen);
		}
		if (flag)
		{
			if (b != bcn)
			{
				bw = r.width - GUIM.YRES(10f);
			}
			if (Time.time > btime)
			{
				btime = Time.time + 0.02f;
				bw += GUIM.YRES(2f) * bforward;
				if (bw > r.width)
				{
					bforward = -1f;
				}
				else if (bw < r.width - GUIM.YRES(10f))
				{
					bforward = 1f;
				}
			}
			GUI.DrawTexture(new Rect(r.x + (r.width - bw) / 2f, r.y + r.height - GUIM.YRES(2f), bw, GUIM.YRES(2f)), tYellow);
			bcn = b;
		}
		if (GUIM.HideButton(r) && currButton != b && !lockmenu)
		{
			SelectMenu(b);
		}
	}

	private static void SelectMenu(CButtonMenu b)
	{
		currButton = b;
		currSubMenu = -1;
		HideMenus();
		if (currButton == Button[0])
		{
			GUIPlay.SetActive(true);
		}
		if (currButton == Button[1])
		{
			GUIInv.SetActive(true);
			currSubMenu = 0;
			b.newitem = 0;
		}
		if (currButton == Button[2])
		{
			GUIProfile.SetActive(true);
		}
		if (currButton == Button[3])
		{
			GUIShop.SetActive(true);
		}
		if (currButton == Button[4])
		{
			GUICraft.SetActive(true);
			currSubMenu = 0;
		}
		if (currButton == Button[5])
		{
			GUIObj.SetActive(true);
			currSubMenu = 0;
		}
		if (currButton == Button[6])
		{
			GUIRank.SetActive(true);
			currSubMenu = 0;
		}
		if (currButton == Button[10])
		{
			GUIClan.SetActive(true);
			currSubMenu = 0;
		}
		MainManager.goUI.SendMessage("SelectMenu", currButton);
	}

	private static void DrawButtonTop2(int idx, Rect r, Rect r2, string text)
	{
		if (GUIM.Contains(r))
		{
			if (currSubMenu == idx)
			{
				GUI.DrawTexture(r, tBlack);
				GUIM.DrawText(r2, text, TextAnchor.MiddleCenter, BaseColor.LightGray2, 1, 16, false);
			}
			else
			{
				GUI.color = new Color(1f, 1f, 1f, 0.8f);
				GUI.DrawTexture(r, tWhite);
				GUI.color = Color.white;
				GUIM.DrawText(r2, text, TextAnchor.MiddleCenter, BaseColor.Black, 1, 16, false);
			}
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(r.x, r.y + r.height - GUIM.YRES(2f), r.width, GUIM.YRES(2f)), tYellow);
		}
		else if (currSubMenu == idx)
		{
			GUI.DrawTexture(r, tBlack);
			GUIM.DrawText(r2, text, TextAnchor.MiddleCenter, BaseColor.LightGray2, 1, 16, false);
		}
		else
		{
			if (GUIOptions.showingame)
			{
				GUI.color = new Color(0.6f, 0.6f, 0.6f, 1f);
			}
			else
			{
				GUI.color = new Color(1f, 1f, 1f, 0.1f);
			}
			GUI.DrawTexture(r, tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(r2, text, TextAnchor.MiddleCenter, BaseColor.Black, 1, 16, false);
		}
		if (GUIM.HideButton(r) && !lockmenu)
		{
			currSubMenu = idx;
			GameObject gameObject = GameObject.Find("GUI");
			if (gameObject != null)
			{
				gameObject.BroadcastMessage("OnChangeSubMenu", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, false);
		float num = 1f / (float)targetWidth;
		float num2 = 1f / (float)targetHeight;
		for (int i = 0; i < texture2D.height; i++)
		{
			for (int j = 0; j < texture2D.width; j++)
			{
				Color pixelBilinear = source.GetPixelBilinear((float)j / (float)texture2D.width, (float)i / (float)texture2D.height);
				Color pixelBilinear2 = source.GetPixelBilinear((float)(j - 1) / (float)texture2D.width, (float)i / (float)texture2D.height);
				Color pixelBilinear3 = source.GetPixelBilinear((float)(j + 1) / (float)texture2D.width, (float)i / (float)texture2D.height);
				Color pixelBilinear4 = source.GetPixelBilinear((float)j / (float)texture2D.width, (float)(i + 1) / (float)texture2D.height);
				Color pixelBilinear5 = source.GetPixelBilinear((float)j / (float)texture2D.width, (float)(i - 1) / (float)texture2D.height);
				Color color = pixelBilinear * 0.4f + (pixelBilinear2 + pixelBilinear3 + pixelBilinear4 + pixelBilinear5) / 4f * 0.6f;
				texture2D.SetPixel(j, i, color);
			}
		}
		texture2D.Apply(false);
		texture2D.filterMode = FilterMode.Point;
		return texture2D;
	}

	public static int CalcLevel(int exp)
	{
		for (int i = 2; i <= 100; i++)
		{
			if (exp < i * i * i * 10)
			{
				return i - 1;
			}
		}
		return 100;
	}

	public static int CalcExp(int level)
	{
		if (level < 2)
		{
			return 0;
		}
		return level * level * level * 10;
	}

	public static int CalcNextExp(int level)
	{
		int num = level + 1;
		return num * num * num * 10;
	}

	public static int CalcClanLevel(int exp)
	{
		for (int i = 5; i <= 100; i++)
		{
			if (exp < i * i * i * 10 * 100)
			{
				return i - 4;
			}
		}
		return 100;
	}

	public static int CalcClanExp(int level)
	{
		if (level < 2)
		{
			return 0;
		}
		int num = level + 3;
		return num * num * num * 10 * 100;
	}

	public static int CalcClanNextExp(int level)
	{
		int num = level + 4;
		return num * num * num * 10 * 100;
	}

	public static void SetDevMsg(int sec, int fragscount, int livetime, string killername)
	{
		header = "ИГРА ОКОНЧЕНА";
		line0 = "Вас убил игрок <color=#FF0>" + killername + "</color>";
		line1 = "Вы убили <color=#FF0>" + fragscount + "</color>";
		line2 = "Время выживания <color=#FF0>" + (livetime / 60).ToString("00") + "</color> мин";
		line3 = "Следующий вход на сервер будет доступен через <color=#FF0>" + (sec / 60).ToString("00") + "</color> мин";
		showdevmsg = true;
	}

	private void DrawDevMsg()
	{
		if (showdevmsg)
		{
			Rect rect = new Rect(0f, 0f, Screen.width, Screen.height);
			GUI.DrawTexture(rect, tBlack);
			Rect r = new Rect(0f, GUIM.YRES(80f), Screen.width, GUIM.YRES(40f));
			Rect r2 = new Rect(0f, GUIM.YRES(200f), Screen.width, GUIM.YRES(40f));
			Rect r3 = new Rect(0f, GUIM.YRES(240f), Screen.width, GUIM.YRES(40f));
			Rect r4 = new Rect(0f, GUIM.YRES(280f), Screen.width, GUIM.YRES(40f));
			Rect r5 = new Rect(0f, GUIM.YRES(360f), Screen.width, GUIM.YRES(40f));
			Rect r6 = new Rect(0f, GUIM.YRES(500f), Screen.width, GUIM.YRES(40f));
			GUIM.DrawText(r, header, TextAnchor.MiddleCenter, BaseColor.Red, 0, 26, false);
			GUIM.DrawText(r2, line0, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 16, false);
			GUIM.DrawText(r3, line1, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 16, false);
			GUIM.DrawText(r4, line2, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 16, false);
			GUIM.DrawText(r5, line3, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 16, false);
			GUIM.DrawText(r6, "НАЖМИТЕ КЛАВИШУ МЫШИ ДЛЯ ПРОДОЛЖЕНИЯ", TextAnchor.MiddleCenter, BaseColor.Green, 0, 20, false);
			if (GUIM.HideButton(rect))
			{
				showdevmsg = false;
			}
		}
	}

	private void VKOnInitComplete()
	{
		Log.AddMainLog("Init completed: SDK version =  " + VKSDK.SDKVersion + ", appId = " + VKSDK.AppId + ", isLoggedIn = " + VKSDK.IsLoggedIn.ToString());
		CallLogin();
	}

	private void CallLogin()
	{
		Log.AddMainLog("VK.Login");
		VKSDK.Login(new List<Scope> { Scope.Offline }, VKOnLoginCompleted);
	}

	private void VKOnLoginCompleted(AuthResponse response)
	{
		VKWeb.cs.GetAuth(response.accessToken);
	}
}
