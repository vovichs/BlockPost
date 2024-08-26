using System.Collections.Generic;
using GameClass;
using UnityEngine;

public class GUIPlay : MonoBehaviour
{
	public static bool show = false;

	public static GUIPlay cs = null;

	private static Texture2D tBlack;

	private static Texture2D tYellow;

	private static Texture2D tWhite;

	private static Texture2D tPVP;

	private Texture2D tGradient;

	private Texture2D[] tDevButton;

	private Texture2D[] tMode;

	private Texture2D[] tModeBack;

	public static int state = -1;

	public static int editorstate = -1;

	public static int editor = 0;

	private string[] sModeHeader = new string[2];

	private string[] sModeDesc = new string[2];

	public static string[] sGameMode = new string[10];

	private string sCrossPlay;

	private const int MAX_STATES = 13;

	private string[] sFilter = new string[13];

	private static bool[] bFilter = new bool[13]
	{
		false, false, true, false, false, false, false, false, false, false,
		false, false, false
	};

	private Rect[] rLeftFilterFull = new Rect[14];

	private Rect[] rLeftFilterBox = new Rect[14];

	private Rect[] rLeftFilterDot = new Rect[14];

	private Rect[] rLeftFilterText = new Rect[14];

	public static List<ServerData> srvlist = new List<ServerData>();

	private static Vector2 scroll = Vector2.zero;

	private static ServerData currServer = null;

	private string sRenderDev;

	private string sRenderCase;

	private string sEditSkin;

	private string sEditMap;

	private string sSquad;

	private string sCreateServer;

	private string sRefrServer;

	private static string sServer;

	private string sMode;

	private string sPlayers;

	private string sLevel;

	private string sConnect;

	private string sTestMap;

	private string sLoadMap;

	private string sNewMap;

	private string sNewSkin_;

	private string sLoadSkin;

	private string sReturn;

	private string sQuickPlay;

	private string sServerList;

	private string sUploadFiles;

	private string sCreate;

	public static string sRound;

	public static string sRoundWon;

	public static string sRoundLose;

	public static string sEnemyEliminated;

	public static string sRountTimeEnd;

	public static string sZombiesWin = "ZOMBIES WIN";

	public static string sHumansWin = "HUMANS WIN";

	private string[] sRegion = new string[5] { "REGION RU", "REGION EU", "REGION US", "REGION #3", "REGION #4" };

	public static int pull = 1;

	public static bool quickplay = false;

	public GUIDevDemoViewer csDemoViewer;

	private Rect[] rMode;

	private Rect[] rText;

	private Rect[] rText2;

	private Rect[] rGradient;

	private Rect rSquad;

	private Rect rSquadText;

	private Rect rSquadDev;

	private Rect rSquadTextDev;

	private Rect[] rDevButton = new Rect[5];

	private Rect[] rDevButtonIcon = new Rect[5];

	private Rect[] rDevButtonIconOffset = new Rect[5];

	private Rect[] rDevButtonText = new Rect[5];

	private Rect rBack;

	private Rect rLeft;

	private Rect rLeftButton;

	private Rect rRefresh;

	private Rect rCreate;

	private Rect rCreateRegion;

	private Rect rRegionBack;

	private Rect rRegion0;

	private Rect rRegion1;

	private Rect rRegion2;

	private Rect rServerOpt;

	private float[] scrollx = new float[2];

	private Rect rZone;

	private Rect rScroll;

	private float nextrefresh;

	private int srvcountfilter;

	private GUIOptions guiopt;

	public static string[] varweapon = new string[152]
	{
		"ak47", "m4a1", "famas", "sig552", "glock17", "colt1911", "mp5", "ump", "uzi", "tec9",
		"svd", "l96", "scout", "vintorez", "spas12", "sss", "galil", "sa80", "groza", "deagle",
		"beretta92", "mp7", "g36c", "scar_l", "aug_a3", "kriss_vector", "g3", "qbz95", "colt_anaconda", "makarov",
		"p90", "bizon", "tar21", "xm8", "steyr_tmp", "mp5k", "mp5sd", "uzi_micro", "uzi_mini", "lr300",
		"mac10", "ruger_mp9", "scorpion", "svu", "psg1", "gol_sniper", "mag7", "sr25", "vsk94", "barrett_m82",
		"pgm_hecate2", "beretta_rx4", "beretta_mx4", "m14_ebr", "usp", "saiga12", "daewoo_k1", "sv98", "m21", "shovel",
		"ak107", "sl8", "m24", "m249", "rpk", "sks", "qbu88", "vssk", "aek", "aa12",
		"an94", "hk416", "asval", "stechkin", "rsh12", "pb", "aksu", "kac_pdw", "magpul_pdr", "ruger_mk2",
		"keltec_ksg", "grach", "fn57", "cz75auto", "a91", "m16a1", "msg90", "penal", "veresk", "kedr",
		"kukri", "katana", "karambit_gold", "tt", "browning", "c96", "sm_m10", "mosin", "enfield", "m98k",
		"m1_garand", "ppsh", "sten", "mp40", "thompson", "karambit_blood", "bayonet", "machete", "strider", "tomahawk",
		"ak12_gold", "svt", "stg44", "luger", "pps", "mg42", "m1a1", "victorinox", "shadow_dagger", "say",
		"kama", "gut", "falchion", "cleaver", "6x4", "maxim9", "hudson9", "thompson_t", "ak_alfa", "savage_10ba",
		"sv98m", "flamethrower", "m1906", "beretta93r", "glock18", "pl15", "balisong", "biker", "bowie", "extractor",
		"stechkin_gold", "fedorov", "hatchet", "podbyrin", "musket", "derringer", "holyshotgun", "night_slasher", "scimitar", "vendetta",
		"daewoo_k2", "r8"
	};

	private int skinwid;

	public static int skinstate = -1;

	private bool inEdit;

	public static string skinname = "";

	private static string sNewSkin = "NEW SKIN";

	private static Vector2 sv = Vector2.zero;

	private int devstate = -1;

	private static List<LogData> loglist = new List<LogData>();

	private static DevClient dcl = null;

	private Rect rDevLog;

	private static Camera csMobileCam = null;

	private static GameObject goChar = null;

	private static GameObject goCamera = null;

	private static Camera cam = null;

	private float rBodyY;

	private int bodyforward = 1;

	private float[] bodyylimit = new float[2] { -5f, 5f };

	private float rHeadY;

	private int headforward = 1;

	private float[] headylimit = new float[2] { -20f, 20f };

	private float rHeadX;

	private int headxforward = 1;

	private float[] headxlimit = new float[2] { -5f, 5f };

	public void Awake()
	{
		cs = this;
	}

	public void LoadLang()
	{
		sModeHeader[0] = Lang.GetString("_OFFICIAL_SERVER");
		sModeHeader[1] = Lang.GetString("_USER_SERVER");
		sModeDesc[0] = Lang.GetString("_BASIC_MODE_GAME");
		sModeDesc[1] = Lang.GetString("_MODIFIED_MODE_GAME");
		sFilter[0] = Lang.GetString("_HIDE_FULL_SERVER");
		sFilter[1] = Lang.GetString("_HIDE_EMPTY_SERVER");
		sFilter[2] = Lang.GetString("_ALL_MODES");
		sFilter[3] = Lang.GetString("_FILTER_TDM") + " 4x4";
		sFilter[4] = Lang.GetString("_FILTER_TDM") + " 8x8";
		sFilter[5] = Lang.GetString("_FILTER_TDM") + " 20x20";
		sFilter[6] = Lang.GetString("_FILTER_FFA");
		sFilter[7] = Lang.GetString("_FILTER_SHOVEL");
		sFilter[8] = Lang.GetString("_FILTER_ELIMINATION");
		sFilter[9] = Lang.GetString("_FILTER_INFECTION");
		sGameMode[0] = Lang.GetString("_MODE_TDM");
		sGameMode[1] = Lang.GetString("_MODE_SHOVEL");
		sGameMode[2] = Lang.GetString("_MODE_FFA");
		sGameMode[3] = Lang.GetString("_MODE_FREEMODE");
		sGameMode[4] = Lang.GetString("_MODE_ELIMINATION");
		sGameMode[5] = Lang.GetString("_MODE_INFECTION");
		sRenderDev = Lang.GetString("_RENDER_DEV");
		sRenderCase = Lang.GetString("_RENDER_CASE");
		sEditSkin = Lang.GetString("_EDITOR_SKIN");
		sEditMap = Lang.GetString("_EDITOR_MAP");
		sSquad = Lang.GetString("_SQUAD");
		sCreateServer = Lang.GetString("_CREATE_SERVER");
		sRefrServer = Lang.GetString("_REFRESH_SERVER");
		sServer = Lang.GetString("_SERVER");
		sMode = Lang.GetString("_MODE");
		sPlayers = Lang.GetString("_PLAYERS");
		sLevel = Lang.GetString("_LEVEL");
		sConnect = Lang.GetString("_CONNECT");
		sTestMap = Lang.GetString("_TEST_MAP");
		sLoadMap = Lang.GetString("_LOAD_MAP");
		sNewMap = Lang.GetString("_NEW_MAP");
		sNewSkin_ = Lang.GetString("_NEW_SKIN");
		sLoadSkin = Lang.GetString("_LOAD_SKIN");
		sReturn = Lang.GetString("_RETURN");
		sCreate = Lang.GetString("_CREATE");
		sCrossPlay = Lang.GetString("_CROSSPLAY");
		sQuickPlay = Lang.GetString("_QUICK_PLAY");
		sServerList = Lang.GetString("_SERVER_LIST");
		sUploadFiles = Lang.GetString("_UPLOAD_FILES");
		sRound = Lang.GetString("_ROUND");
		sRoundWon = Lang.GetString("_ROUND_WON");
		sRoundLose = Lang.GetString("_ROUND_LOSE");
		sEnemyEliminated = Lang.GetString("_ENEMY_ELIMINATED");
		sRountTimeEnd = Lang.GetString("_ROUND_TIME_END");
		sFilter[10] = Lang.GetString("_FILTER_FREEZETAG");
		sFilter[11] = Lang.GetString("_FILTER_PISTOL");
		sFilter[12] = Lang.GetString("_FILTER_GUNGAME");
		sGameMode[6] = Lang.GetString("_MODE_FREEZETAG");
		sGameMode[7] = Lang.GetString("_MODE_PISTOL");
		sGameMode[8] = Lang.GetString("_MODE_GUNGAME");
		sGameMode[9] = Lang.GetString("_MODE_TEST");
	}

	public static void SetActive(bool val)
	{
		show = val;
		state = -1;
		editorstate = -1;
		if (val)
		{
			GUIFX.Set();
		}
		else
		{
			GUIFX.End();
		}
		if (val)
		{
			if (Main.tAvatar != null)
			{
				MCreateChar();
			}
			MCreateCamera();
		}
		else
		{
			if (goChar != null)
			{
				Object.Destroy(goChar);
			}
			if (goCamera != null)
			{
				Object.Destroy(goCamera);
			}
		}
		if (!val && cs.csDemoViewer != null)
		{
			Object.Destroy(cs.csDemoViewer);
		}
	}

	private void LoadEnd()
	{
		if (tMode == null)
		{
			tMode = new Texture2D[2];
		}
		if (tModeBack == null)
		{
			tModeBack = new Texture2D[2];
		}
		if (tDevButton == null)
		{
			tDevButton = new Texture2D[5];
		}
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("lightgray");
		tYellow = TEX.GetTextureByName("yellow");
		tMode[0] = ContentLoader_.LoadTexture("play_char_0") as Texture2D;
		tMode[1] = ContentLoader_.LoadTexture("play_char_1") as Texture2D;
		tModeBack[0] = ContentLoader_.LoadTexture("play_back_0") as Texture2D;
		tModeBack[1] = ContentLoader_.LoadTexture("play_back_1") as Texture2D;
		tGradient = ContentLoader_.LoadTexture("top_gradient") as Texture2D;
		tDevButton[0] = Resources.Load("editor_render") as Texture2D;
		tDevButton[1] = Resources.Load("editor_skin") as Texture2D;
		tDevButton[2] = Resources.Load("editor_map") as Texture2D;
		tDevButton[3] = Resources.Load("editor_upload") as Texture2D;
		tDevButton[4] = Resources.Load("editor_demo") as Texture2D;
	}

	private void OnResize()
	{
		float y = GUIM.YRES(240f);
		if (rMode == null)
		{
			rMode = new Rect[2];
		}
		if (rText == null)
		{
			rText = new Rect[2];
		}
		if (rText2 == null)
		{
			rText2 = new Rect[2];
		}
		if (rGradient == null)
		{
			rGradient = new Rect[2];
		}
		rMode[0].Set((float)Screen.width / 2f - GUIM.YRES(355f), y, GUIM.YRES(350f), GUIM.YRES(175f));
		rMode[1].Set((float)Screen.width / 2f - GUIM.YRES(355f) + GUIM.YRES(360f), y, GUIM.YRES(350f), GUIM.YRES(175f));
		for (int i = 0; i < 2; i++)
		{
			rText[i].Set(rMode[i].x + GUIM.YRES(8f), rMode[i].y + GUIM.YRES(136f), rMode[i].width, GUIM.YRES(20f));
			rText2[i].Set(rMode[i].x + GUIM.YRES(8f), rMode[i].y + GUIM.YRES(160f), rMode[i].width, GUIM.YRES(15f));
			rGradient[i].Set(rMode[i].x, rMode[i].y + rMode[i].height + GUIM.YRES(5f), rMode[i].width, 0f - GUIM.YRES(132f));
		}
		rSquad.Set((float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(708f), y, GUIM.YRES(212f), GUIM.YRES(175f));
		rSquadText.Set(rSquad.x + GUIM.YRES(8f), rSquad.y + GUIM.YRES(8f), rSquad.width - GUIM.YRES(16f), GUIM.YRES(20f));
		rSquadDev.Set((float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(708f), GUIM.YRES(140f), GUIM.YRES(212f), GUIM.YRES(175f));
		rSquadTextDev.Set(rSquadDev.x + GUIM.YRES(8f), rSquadDev.y + GUIM.YRES(8f), rSquadDev.width - GUIM.YRES(16f), GUIM.YRES(20f));
		int num = (int)GUIM.YRES(3f);
		int num2 = (int)GUIM.YRES(16f);
		int num3 = num2 - num * 2;
		rLeft = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f) + GUIM.YRES(32f), GUIM.YRES(300f), GUIM.YRES(468f));
		rLeftButton = new Rect(rLeft.x, GUIM.YRES(140f), rLeft.width, GUIM.YRES(28f));
		rRefresh = new Rect(rLeft.x + GUIM.YRES(8f), rLeft.y + rLeft.height - GUIM.YRES(36f), rLeft.width - GUIM.YRES(16f), GUIM.YRES(28f));
		rCreate = new Rect(rLeft.x + GUIM.YRES(8f), rLeft.y + rLeft.height - GUIM.YRES(72f), rLeft.width - GUIM.YRES(16f), GUIM.YRES(28f));
		if (MainManager.steam)
		{
			rCreate.width = GUIM.YRES(180f);
			rCreateRegion = new Rect(rCreate.x + rCreate.width, rCreate.y, rLeft.width - GUIM.YRES(196f), rCreate.height);
		}
		for (int j = 0; j < 5; j++)
		{
			rDevButton[j] = new Rect((float)Screen.width / 2f - GUIM.YRES(355f) + GUIM.YRES(90f) * (float)j, GUIM.YRES(500f), GUIM.YRES(80f), GUIM.YRES(80f));
			rDevButtonIcon[j] = new Rect((float)Screen.width / 2f - GUIM.YRES(355f) + GUIM.YRES(90f) * (float)j + GUIM.YRES(24f), GUIM.YRES(500f) + GUIM.YRES(14f), GUIM.YRES(32f), GUIM.YRES(32f));
			rDevButtonText[j] = new Rect((float)Screen.width / 2f - GUIM.YRES(355f) + GUIM.YRES(90f) * (float)j, GUIM.YRES(548f), GUIM.YRES(80f), GUIM.YRES(32f));
			rDevButtonIconOffset[j] = rDevButtonIcon[j];
			rDevButtonIconOffset[j].x += GUIM.YRES(1f);
			rDevButtonIconOffset[j].y += GUIM.YRES(1f);
		}
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(316f), GUIM.YRES(140f), GUIM.YRES(388f), GUIM.YRES(500f));
		rRegionBack = new Rect(rBack.x + rBack.width + GUIM.YRES(8f), rBack.y - GUIM.YRES(4f), GUIM.YRES(212f), GUIM.YRES(132f) - GUIM.YRES(28f));
		rRegion0 = new Rect(rRegionBack.x + GUIM.YRES(14f), rBack.y + GUIM.YRES(14f), GUIM.YRES(184f), GUIM.YRES(28f));
		rRegion1 = new Rect(rRegionBack.x + GUIM.YRES(14f), rBack.y + GUIM.YRES(50f), GUIM.YRES(184f), GUIM.YRES(28f));
		rRegion2 = new Rect(rRegionBack.x + GUIM.YRES(14f), rBack.y + GUIM.YRES(86f), GUIM.YRES(184f), GUIM.YRES(28f));
		if (MainManager.steam)
		{
			rServerOpt = new Rect(rBack.x + rBack.width + GUIM.YRES(8f), rBack.y + GUIM.YRES(140f), GUIM.YRES(212f), GUIM.YRES(90f));
		}
		else
		{
			rServerOpt = new Rect(rBack.x + rBack.width + GUIM.YRES(8f), rBack.y - GUIM.YRES(4f), GUIM.YRES(212f), GUIM.YRES(90f));
		}
		for (int k = 0; k < 14; k++)
		{
			float num4;
			float num5;
			if (k <= 1)
			{
				num4 = rServerOpt.x + GUIM.YRES(20f);
				num5 = rServerOpt.y + GUIM.YRES(20f) + GUIM.YRES(32f) * (float)k;
			}
			else
			{
				num4 = rLeft.x + GUIM.YRES(20f);
				num5 = GUIM.YRES(190f) + GUIM.YRES(32f) * (float)(k - 2);
			}
			rLeftFilterFull[k] = new Rect(num4, num5, GUIM.YRES(216f), GUIM.YRES(20f));
			rLeftFilterBox[k] = new Rect(num4, num5, num2, num2);
			rLeftFilterDot[k] = new Rect(num4 + (float)num, num5 + (float)num, num3, num3);
			rLeftFilterText[k] = new Rect(num4 + GUIM.YRES(20f), num5 - GUIM.YRES(5f), GUIM.YRES(200f), GUIM.YRES(26f));
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			MOnGUI();
		}
	}

	private void DrawPlay()
	{
		if (state < 0)
		{
			bool flag = true;
			if (DrawGame(0))
			{
				flag = false;
			}
			if (DrawGame(1))
			{
				flag = false;
			}
			if (flag)
			{
				scrollx[0] = 0f;
				scrollx[1] = 0f;
			}
		}
	}

	private void DrawSquad()
	{
	}

	private bool DrawGame(int mode)
	{
		bool result = false;
		if (GUIM.Contains(rMode[mode]))
		{
			scrollx[mode] += Time.deltaTime * 0.005f;
			if (scrollx[mode] > 0.5f)
			{
				scrollx[mode] = 0.5f;
			}
			GUIM.DrawBox(rMode[mode], tYellow, 1f);
			GUI.DrawTextureWithTexCoords(rMode[mode], tModeBack[mode], new Rect(scrollx[mode], 0f, 0.5f, 1f));
			result = true;
		}
		else
		{
			GUIM.DrawBox(rMode[mode], tBlack);
			GUI.DrawTextureWithTexCoords(rMode[mode], tModeBack[mode], new Rect(0f, 0f, 0.5f, 1f));
		}
		GUI.DrawTexture(rMode[mode], tMode[mode]);
		GUI.color = new Color(0f, 0f, 0f, 0.75f);
		GUI.DrawTexture(rGradient[mode], tGradient);
		GUI.DrawTexture(rGradient[mode], tGradient);
		GUI.color = Color.white;
		GUIM.DrawText(rText[mode], sModeHeader[mode], TextAnchor.MiddleLeft, BaseColor.White, 0, 26, true);
		GUIM.DrawText(rText2[mode], sModeDesc[mode], TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 16, true);
		if (GUIM.HideButton(rMode[mode]))
		{
			state = mode;
			MasterClient.cs.send_list(pull);
			nextrefresh = Time.time + 3f;
			currServer = null;
		}
		return result;
	}

	private void DrawMode(int idx, Rect r, string txt, Texture2D icon = null)
	{
		Rect rect = new Rect(r.x, r.y + GUIM.YRES(68f), GUIM.YRES(300f), GUIM.YRES(32f));
		GUIM.DrawBox(r, tBlack, 0.05f);
		if (icon != null)
		{
			GUI.DrawTexture(new Rect(r.x, r.y, GUIM.YRES(400f), GUIM.YRES(100f)), icon);
		}
		if (GUIM.Contains(r) || state == idx)
		{
			GUIM.DrawBoxBorder(r, tWhite, 0.5f);
			GUI.DrawTexture(rect, tWhite);
		}
		else
		{
			GUIM.DrawBox(rect, tBlack, 0.25f);
		}
		GUIM.DrawText(rect, txt, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, true);
		if (GUIM.HideButton(r))
		{
			state = idx;
			if (state == 0)
			{
				MasterClient.cs.send_list(pull);
			}
		}
	}

	private void DrawModeDev(int id, int _state, string label)
	{
		if (GUIM.Contains(rDevButton[id]))
		{
			GUIM.DrawBox(rDevButton[id], TEX.tYellow);
		}
		else
		{
			GUIM.DrawBox(rDevButton[id], TEX.tBlack);
		}
		GUI.color = Color.black;
		GUI.DrawTexture(rDevButtonIconOffset[id], tDevButton[id]);
		GUI.color = Color.white;
		GUI.DrawTexture(rDevButtonIcon[id], tDevButton[id]);
		GUIM.DrawText(rDevButtonText[id], label, TextAnchor.MiddleCenter, BaseColor.White, 0, 16, true);
		if (GUIM.HideButton(rDevButton[id]))
		{
			state = _state;
			if (state == 7)
			{
				Main.HideMenus();
				Main.SetActive(false);
				GUI3D.csCam.fieldOfView = 65f;
				GUI3D.csCam.enabled = true;
				GUI3D.csCam.clearFlags = CameraClearFlags.Color;
				GUI3D.csCam.backgroundColor = Color.gray;
				GUI3D.csCam.cullingMask = 3;
				Log.active = false;
				base.gameObject.AddComponent<GUICharEditor>();
				OutlineSystem outlineSystem = base.gameObject.AddComponent<OutlineSystem>();
				outlineSystem.mainCamera = GUI3D.csCam;
				outlineSystem.outlineLayer = 2;
				outlineSystem.blurMaterial = Resources.Load("MobileBlur") as Material;
				outlineSystem.outlineMaterial = Resources.Load("ScreenSpaceOutlineShader") as Material;
				outlineSystem.outlineColor = Color.white;
			}
			int state2 = state;
			int num = 8;
			if (state == 9 && csDemoViewer == null)
			{
				csDemoViewer = base.gameObject.AddComponent<GUIDevDemoViewer>();
			}
		}
	}

	public static void CreateControll()
	{
		GameObject.Find("Core").GetComponent<Controll>().PostAwake();
		Controll.SetLockMove(true);
		Controll.SetLockLook(true);
		Controll.SetCameraSolid(true);
	}

	public static void StartPoligon()
	{
		Main.HideMenus();
		Main.SetActive(false);
		Builder.active = false;
		MainManager.SetState(1);
	}

	public static void StartEditor()
	{
		Main.HideMenus();
		Main.SetActive(false);
		Builder.active = true;
		MainManager.SetState(1);
	}

	private void DrawFilter(int id)
	{
		if (GUIM.HideButton(rLeftFilterFull[id]))
		{
			bFilter[id] = !bFilter[id];
			if (id > 1)
			{
				for (int i = 2; i < bFilter.Length; i++)
				{
					bFilter[i] = false;
				}
				bFilter[id] = true;
			}
		}
		GUI.DrawTexture(rLeftFilterBox[id], tBlack);
		if (bFilter[id])
		{
			GUI.DrawTexture(rLeftFilterDot[id], tYellow);
		}
		GUIM.DrawText(rLeftFilterText[id], sFilter[id], TextAnchor.MiddleLeft, (!GUIM.Contains(rLeftFilterFull[id])) ? BaseColor.White : BaseColor.Yellow, 0, 20, false);
	}

	private void DrawFilterCrossPlay()
	{
		if (GUIM.HideButton(rLeftFilterFull[13]))
		{
			if (pull == 0)
			{
				pull = 1;
			}
			else
			{
				pull = 0;
			}
			nextrefresh = Time.time + 1f;
			MasterClient.cs.send_list(pull);
		}
		GUI.DrawTexture(rLeftFilterBox[13], tBlack);
		if (pull == 0)
		{
			GUI.DrawTexture(rLeftFilterDot[13], tYellow);
		}
		GUIM.DrawText(rLeftFilterText[13], sCrossPlay, TextAnchor.MiddleLeft, (!GUIM.Contains(rLeftFilterFull[13])) ? BaseColor.White : BaseColor.Yellow, 0, 20, false);
	}

	private void DrawServers()
	{
		if (GUIM.Button(rLeftButton, BaseColor.LightGray2, sReturn, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			state = -1;
			if (Main.tAvatar != null)
			{
				MCreateChar();
			}
		}
		GUIM.DrawBox(rLeft, tBlack, 0.05f);
		if (state == 1)
		{
			if (MainManager.steam)
			{
				if (GUIM.Button(rCreate, BaseColor.White, sCreateServer, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
				{
					MasterClient.cs.send_customplay(pull);
				}
				GUI.DrawTexture(rCreateRegion, TEX.tBlackAlpha);
				GUIM.DrawText(rCreateRegion, sRegion[pull], TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, false);
			}
			else if (GUIM.Button(rCreate, BaseColor.White, sCreateServer, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				MasterClient.cs.send_customplay(pull);
			}
		}
		bool flag = Time.time > nextrefresh;
		if (GUIM.Button(rRefresh, flag ? BaseColor.LightGray2 : BaseColor.Gray, sRefrServer, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && flag)
		{
			nextrefresh = Time.time + 3f;
			MasterClient.cs.send_list(pull);
		}
		DrawFilter(0);
		DrawFilter(1);
		DrawFilter(2);
		DrawFilter(3);
		DrawFilter(4);
		DrawFilter(5);
		DrawFilter(6);
		DrawFilter(7);
		DrawFilter(8);
		DrawFilter(9);
		DrawFilter(10);
		DrawFilter(11);
		DrawFilter(12);
		if (!GP.auth)
		{
			DrawFilterCrossPlay();
		}
		GUIM.DrawBox(rBack, tBlack);
		GUIM.YRES(4f);
		TEX.GetTextureByName("blue");
		Rect r = new Rect(rBack.x + GUIM.YRES(4f), rBack.y + GUIM.YRES(4f), GUIM.YRES(40f), GUIM.YRES(24f));
		GUIM.DrawText(r, "#", TextAnchor.MiddleCenter, BaseColor.White, 1, 12, false);
		r.x = r.x + GUIM.YRES(2f) + r.width;
		r.width = GUIM.YRES(104f);
		GUIM.DrawText(r, sServer, TextAnchor.MiddleCenter, BaseColor.White, 1, 12, false);
		r.x = r.x + GUIM.YRES(2f) + r.width;
		r.width = GUIM.YRES(84f);
		GUIM.DrawText(r, sMode, TextAnchor.MiddleCenter, BaseColor.White, 1, 12, false);
		r.x = r.x + GUIM.YRES(2f) + r.width;
		r.width = GUIM.YRES(84f);
		GUIM.DrawText(r, sPlayers, TextAnchor.MiddleCenter, BaseColor.White, 1, 12, false);
		r.x = r.x + GUIM.YRES(2f) + r.width;
		r.width = GUIM.YRES(40f);
		GUIM.DrawText(r, sLevel, TextAnchor.MiddleCenter, BaseColor.White, 1, 12, false);
		rZone.Set(rBack.x + GUIM.YRES(4f), rBack.y + GUIM.YRES(32f), rBack.width - GUIM.YRES(8f), rBack.height - GUIM.YRES(40f));
		rScroll.Set(0f, 0f, 0f, (float)srvcountfilter * GUIM.YRES(26f));
		scroll = GUIM.BeginScrollView(rZone, scroll, rScroll);
		int num = 0;
		for (int num2 = 40; num2 >= 0; num2--)
		{
			for (int i = 0; i < srvlist.Count; i++)
			{
				if (srvlist[i].players == num2 && (!bFilter[0] || srvlist[i].players != srvlist[i].maxplayers) && (!bFilter[1] || srvlist[i].players != 0) && (state != 0 || srvlist[i].status != 255) && (state != 1 || srvlist[i].status == 255) && (bFilter[2] || ((!bFilter[3] || (srvlist[i].gamemode == 0 && srvlist[i].maxplayers == 8)) && (!bFilter[4] || (srvlist[i].gamemode == 0 && srvlist[i].maxplayers == 16)) && (!bFilter[5] || (srvlist[i].gamemode == 0 && srvlist[i].maxplayers == 40)) && (!bFilter[6] || srvlist[i].gamemode == 2) && (!bFilter[7] || srvlist[i].gamemode == 1) && (!bFilter[8] || srvlist[i].gamemode == 4) && (!bFilter[9] || srvlist[i].gamemode == 5) && (!bFilter[10] || srvlist[i].gamemode == 6) && (!bFilter[11] || srvlist[i].gamemode == 7) && (!bFilter[12] || srvlist[i].gamemode == 8))))
				{
					if (DrawButtonServer(srvlist[i], num))
					{
						currServer = srvlist[i];
					}
					num++;
				}
			}
		}
		GUIM.EndScrollView();
		srvcountfilter = num;
		if (currServer != null)
		{
			Rect r2 = new Rect(rBack.x + rBack.width + GUIM.YRES(24f), r.y + GUIM.YRES(454f), GUIM.YRES(184f), GUIM.YRES(28f));
			GUIM.DrawBox(new Rect(r2.x - GUIM.YRES(14f), r2.y - GUIM.YRES(14f), GUIM.YRES(212f), GUIM.YRES(56f)), tBlack, 0.05f);
			if (GUIM.Button(r2, BaseColor.Yellow, sConnect, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				GUIInv.CheckNullSet();
				Client.IP = currServer.ip;
				Client.PORT = currServer.port;
				Client.cs.Connect();
			}
		}
		if (MainManager.steam)
		{
			GUIM.DrawBox(rRegionBack, tBlack, 0.05f);
			if (GUIM.Button(rRegion0, (pull != 0) ? BaseColor.White : BaseColor.Yellow, sRegion[0], TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && pull != 0)
			{
				pull = 0;
				nextrefresh = Time.time + 1f;
				MasterClient.cs.send_list(pull);
				srvlist.Clear();
			}
			if (GUIM.Button(rRegion1, (pull != 1) ? BaseColor.White : BaseColor.Yellow, sRegion[1], TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && pull != 1)
			{
				pull = 1;
				nextrefresh = Time.time + 1f;
				MasterClient.cs.send_list(pull);
				srvlist.Clear();
			}
		}
		GUIM.DrawBox(rServerOpt, tBlack, 0.05f);
	}

	private static bool DrawButtonServer(ServerData s, int pos)
	{
		TEX.GetTextureByName("black");
		Texture2D textureByName = TEX.GetTextureByName("white");
		BaseColor fontcolor = BaseColor.LightGray;
		if (s.players == s.maxplayers)
		{
			fontcolor = BaseColor.Red;
		}
		else if (s.players > 0)
		{
			fontcolor = BaseColor.Yellow;
		}
		Rect r = new Rect(0f, GUIM.YRES(pos * 26), GUIM.YRES(362f), GUIM.YRES(24f));
		if (s == currServer)
		{
			fontcolor = BaseColor.Black;
			GUI.color = new Color(1f, 1f, 1f, 0.9f);
			GUI.DrawTexture(new Rect(r.x, r.y - GUIM.YRES(3f), r.width, r.height), textureByName);
			GUI.color = Color.white;
		}
		else if (GUIM.Contains(r))
		{
			fontcolor = BaseColor.White;
			GUI.color = new Color(1f, 1f, 1f, 0.25f);
			GUI.DrawTexture(new Rect(r.x, r.y - GUIM.YRES(3f), r.width, r.height), textureByName);
			GUI.color = Color.white;
			Event current = Event.current;
			if (current.isMouse && current.type == EventType.MouseDown)
			{
				int clickCount = current.clickCount;
				int num = 2;
			}
		}
		r.width = GUIM.YRES(40f);
		GUIM.DrawText(r, (s.idx + 1).ToString(), TextAnchor.MiddleCenter, fontcolor, 1, 12, false);
		r.x = r.x + r.width + GUIM.YRES(2f);
		r.width = GUIM.YRES(104f);
		GUIM.DrawText(r, sServer + " #" + (s.idx + 1), TextAnchor.MiddleCenter, fontcolor, 1, 12, false);
		r.x = r.x + r.width + GUIM.YRES(2f);
		r.width = GUIM.YRES(84f);
		GUIM.DrawText(r, sGameMode[s.gamemode], TextAnchor.MiddleCenter, fontcolor, 1, 12, false);
		r.x = r.x + r.width + GUIM.YRES(2f);
		r.width = GUIM.YRES(84f);
		GUIM.DrawText(r, s.formatplayers, TextAnchor.MiddleCenter, fontcolor, 1, 12, false);
		r.x = r.x + r.width + GUIM.YRES(2f);
		r.width = GUIM.YRES(40f);
		GUIM.DrawText(r, s.slevel, TextAnchor.MiddleCenter, fontcolor, 1, 12, false);
		r.x = 0f;
		r.width = GUIM.YRES(580f);
		if (GUIM.HideButton(r))
		{
			return true;
		}
		return false;
	}

	private void DrawPoligon()
	{
		Rect r = new Rect((float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(316f), GUIM.YRES(140f), GUIM.YRES(604f), GUIM.YRES(500f));
		GUIM.DrawBox(r, tBlack);
		float x = r.x + GUIM.YRES(200f);
		if (GUIM.Button(new Rect(x, r.y + GUIM.YRES(48f), GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sTestMap, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Map.Create(6, 4, 6);
			MapLoader.GenerateClearMap(40, 12);
			Console.cs.Command("sun 50 330");
			Builder.active = false;
			CreateControll();
			StartPoligon();
			GUIMap.SPAWN();
		}
		if (GUIM.Button(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f), GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sLoadMap, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Console.cs.Command("jscall loadfilemap");
		}
		if (GUIMap.cs != null)
		{
			GUIMap.cs.DrawState2(r);
		}
	}

	private void DrawEditor()
	{
		Rect r = new Rect((float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(316f), GUIM.YRES(140f), GUIM.YRES(604f), GUIM.YRES(500f));
		GUIM.DrawBox(r, tBlack);
		float x = r.x + GUIM.YRES(200f);
		if (GUIM.Button(new Rect(x, r.y + GUIM.YRES(48f), GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sNewMap, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			editorstate = 0;
			GUIMap.mapname = "";
		}
		if (GUIM.Button(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f), GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sLoadMap, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Console.cs.Command("jscall loadfilemap");
			editorstate = 1;
		}
		if (GUIMap.cs != null)
		{
			if (editorstate == 0)
			{
				GUIMap.cs.DrawState1(r);
			}
			else if (editorstate == 1)
			{
				GUIMap.cs.DrawState2(r);
			}
		}
	}

	private void DrawSkinEditor()
	{
		Rect r = new Rect((float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(316f), GUIM.YRES(140f), GUIM.YRES(604f), GUIM.YRES(500f));
		float x = r.x + GUIM.YRES(200f);
		GUIM.DrawBox(r, tBlack);
		if (GUIM.Button(new Rect(x, r.y + GUIM.YRES(48f), GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sNewSkin_, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			skinname = "";
			skinstate = 0;
		}
		if (GUIM.Button(new Rect(x, r.y + GUIM.YRES(84f), GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sLoadSkin, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Console.cs.Command("jscall loadfileskin");
		}
		if (skinstate == 0)
		{
			if (guiopt == null)
			{
				guiopt = GetComponent<GUIOptions>();
			}
			GUIM.YRES(40f);
			GUIM.YRES(66f);
			GUIM.DrawText(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 2f, GUIM.YRES(200f), GUIM.YRES(32f)), "SKIN NAME", TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, false);
			if (!inEdit)
			{
				GUI.DrawTexture(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), TEX.tBlack);
				string text = skinname;
				if (text == "")
				{
					text = sNewSkin;
				}
				GUIM.DrawText(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), text, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
				if (GUIM.HideButton(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f))))
				{
					inEdit = true;
				}
			}
			else
			{
				GUI.DrawTexture(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), TEX.tBlack);
				GUIM.DrawEdit(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), ref skinname, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
			}
			if (Input.GetMouseButtonDown(0) && !GUIM.Contains(new Rect(r.x + GUIM.YRES(200f), r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f))))
			{
				inEdit = false;
			}
			GUIM.DrawText(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 4f, GUIM.YRES(200f), GUIM.YRES(32f)), "SKIN TYPE", TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, false);
			Rect r2 = new Rect(r.x - GUIM.YRES(56f), r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 5f, GUIM.YRES(200f), GUIM.YRES(32f));
			GUI.DrawTexture(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 5f, GUIM.YRES(200f), GUIM.YRES(32f)), TEX.tBlack);
			guiopt.DrawVar(r2, "", varweapon, ref skinwid);
			if (GUIM.Button(new Rect(x, r.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 6f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.Yellow, sCreate, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				if (skinname == "")
				{
					skinname = "newskin";
				}
				GUISkinEditor.wname = varweapon[skinwid];
				GUISkinEditor.skinname = GUISkinEditor.wname + "_" + skinname;
				Main.HideMenus();
				Main.SetActive(false);
				base.gameObject.AddComponent<GUISkinEditor>();
			}
		}
		else
		{
			if (skinstate != 1)
			{
				return;
			}
			sv = GUIM.BeginScrollView(new Rect(x, r.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 2f, GUIM.YRES(200f), GUIM.YRES(200f)), sv, new Rect(0f, 0f, GUIM.YRES(180f), (float)GUISkinEditor.skinlist.Count * GUIM.YRES(26f)));
			for (int i = 0; i < GUISkinEditor.skinlist.Count; i++)
			{
				Rect r3 = new Rect(0f, 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(100f), GUIM.YRES(20f));
				BaseColor fontcolor = BaseColor.White;
				if (GUIM.Contains(r3))
				{
					fontcolor = BaseColor.Yellow;
				}
				if (skinname == GUISkinEditor.skinlist[i])
				{
					fontcolor = BaseColor.Green;
				}
				GUIM.DrawText(new Rect(0f, 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(20f), GUIM.YRES(20f)), i.ToString(), TextAnchor.MiddleCenter, fontcolor, 0, 20, true);
				GUIM.DrawText(new Rect(GUIM.YRES(22f), 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(80f), GUIM.YRES(20f)), GUISkinEditor.skinlist[i], TextAnchor.MiddleLeft, fontcolor, 0, 20, true);
				if (GUIM.HideButton(r3))
				{
					skinname = GUISkinEditor.skinlist[i];
				}
			}
			GUIM.EndScrollView();
			if (!GUIM.Button(new Rect(x, r.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 8f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.Yellow, "LOAD", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				return;
			}
			Debug.Log("load skin " + skinname);
			if (skinname != "")
			{
				GUISkinEditor.tSkinImport = TEX.LoadTexture("Skins/" + skinname + ".bpskn");
				if (GUISkinEditor.tSkinImport != null)
				{
					GUISkinEditor.wname = GUISkinEditor.GetWeaponName(GUISkinEditor.tSkinImport.GetPixels32());
					GUISkinEditor.skinname = skinname;
					Main.HideMenus();
					Main.SetActive(false);
					GameObject.Find("GUI").AddComponent<GUISkinEditor>();
				}
			}
		}
	}

	private void DrawRenderEditor()
	{
		Rect r = new Rect((float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(316f), GUIM.YRES(140f), GUIM.YRES(604f), GUIM.YRES(500f));
		float x = r.x + GUIM.YRES(200f);
		GUIM.DrawBox(r, tBlack);
		if (GUIM.Button(new Rect(x, r.y + GUIM.YRES(48f), GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, ">>>", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Main.HideMenus();
			Main.SetActive(false);
			GameObject obj = CaseGen.Build("case_wwii2");
			obj.transform.position = new Vector3(0f, -0.35f, 8f);
			obj.transform.eulerAngles = new Vector3(350f, 45f, 350f);
			GUI3D.csCam.fieldOfView = 10f;
			GUI3D.csCam.enabled = true;
			GUI3D.csCam.clearFlags = CameraClearFlags.Color;
			GUI3D.csCam.backgroundColor = Color.green;
			Log.active = false;
			base.gameObject.AddComponent<FXAA>();
			GameObject.Find("Dlight").transform.eulerAngles = new Vector3(15f, 15f, 0f);
		}
	}

	private void DrawCharEditor()
	{
		Rect r = new Rect((float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(316f), GUIM.YRES(140f), GUIM.YRES(604f), GUIM.YRES(500f));
		float x = r.x + GUIM.YRES(200f);
		GUIM.DrawBox(r, tBlack);
		GUIM.Button(new Rect(x, r.y + GUIM.YRES(48f), GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, ">>>", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
	}

	private void DrawDevCenter()
	{
	}

	public static void DevCenterLog(string msg)
	{
		loglist.Add(new LogData(msg));
	}

	public void DrawDemoViewer()
	{
	}

	public static void MCreateChar()
	{
		if (!(goChar != null))
		{
			GameObject obj = VCGen.Build(Controll.pl);
			Controll.pl.goHead.transform.localEulerAngles = new Vector3(355f, 0f, 0f);
			Controll.pl.goLArm[0].transform.localPosition = new Vector3(-8.000148f, 0f, 0f);
			Controll.pl.goLArm[0].transform.localEulerAngles = new Vector3(-3.080189f, -1.88176f, 81.94669f);
			Controll.pl.goLArm[1].transform.localPosition = new Vector3(-12.00017f, 2.001597f, 0f);
			Controll.pl.goLArm[1].transform.localEulerAngles = new Vector3(358.6052f, 10.77333f, 12.44588f);
			Controll.pl.goRArm[0].transform.localPosition = new Vector3(8.000176f, 0f, 0f);
			Controll.pl.goRArm[0].transform.localEulerAngles = new Vector3(1.675015f, -1.381974f, 281.9428f);
			Controll.pl.goRArm[1].transform.localPosition = new Vector3(12.00018f, 2.000503f, 0f);
			Controll.pl.goRArm[1].transform.localEulerAngles = new Vector3(351.9885f, 4.819155f, 344.9216f);
			Controll.pl.goWaterSplash.SetActive(false);
			Controll.pl.goRLeg[0].transform.localEulerAngles = new Vector3(4.39091f, 6.96573f, 2.32498f);
			Controll.pl.goRLeg[1].transform.localEulerAngles = new Vector3(357.4252f, 0f, 0f);
			Controll.pl.goLLeg[0].transform.localEulerAngles = new Vector3(357.1478f, 346.0201f, 358.097f);
			Controll.pl.goLLeg[1].transform.localEulerAngles = new Vector3(3.94814f, 2.31111f, 0f);
			obj.transform.localPosition = new Vector3(1f, 0.5f, 5f);
			obj.transform.eulerAngles = new Vector3(0f, 220f, 0f);
			goChar = obj;
		}
	}

	private static void MCreateCamera()
	{
		goCamera = new GameObject();
		goCamera.transform.position = new Vector3(0f, 0f, 0f);
		goCamera.transform.eulerAngles = Vector3.zero;
		goCamera.name = "MobileCamera";
		cam = goCamera.AddComponent<Camera>();
		cam.nearClipPlane = 0.1f;
		cam.farClipPlane = 25f;
		cam.fieldOfView = 45f;
		cam.clearFlags = CameraClearFlags.Depth;
		cam.backgroundColor = new Color(27f / 85f, 0.30980393f, 24f / 85f);
		cam.cullingMask = 1;
	}

	private void MOnGUI()
	{
		AnimateChar();
		MDrawPlay();
		if (state == 0)
		{
			DrawServers();
		}
	}

	private void MDrawPlay()
	{
		if (state >= 0)
		{
			return;
		}
		Rect rect = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(240f), GUIM.YRES(300f), GUIM.YRES(100f));
		Rect rect2 = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(360f), GUIM.YRES(300f), GUIM.YRES(100f));
		GUI.DrawTexture(rect, TEX.tYellow);
		GUIM.DrawText(rect, sQuickPlay, TextAnchor.MiddleCenter, BaseColor.Black, 0, 40, false);
		if (GUIM.HideButton(rect))
		{
			quickplay = true;
			MasterClient.cs.send_list(pull);
		}
		GUI.DrawTexture(rect2, TEX.tDarkGray);
		GUIM.DrawText(rect2, sServerList, TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 40, false);
		if (GUIM.HideButton(rect2))
		{
			state = 0;
			MasterClient.cs.send_list(pull);
			nextrefresh = Time.time + 3f;
			if (goChar != null)
			{
				Object.Destroy(goChar);
			}
		}
	}

	public static void QuickConnect()
	{
		currServer = null;
		ServerData serverData = null;
		for (int i = 0; i < srvlist.Count; i++)
		{
			if (srvlist[i].players != srvlist[i].maxplayers)
			{
				if (serverData == null)
				{
					serverData = srvlist[i];
				}
				if (srvlist[i].players != 0)
				{
					serverData = srvlist[i];
					break;
				}
			}
		}
		currServer = serverData;
		GUIInv.CheckNullSet();
		Client.IP = currServer.ip;
		Client.PORT = currServer.port;
		Client.cs.Connect();
	}

	private void AnimateChar()
	{
		if (goChar == null)
		{
			return;
		}
		rBodyY += Time.deltaTime * 2f * (float)bodyforward;
		if (rBodyY > bodyylimit[1])
		{
			rBodyY = bodyylimit[1];
			bodyforward = -1;
		}
		else if (rBodyY < bodyylimit[0])
		{
			rBodyY = bodyylimit[0];
			bodyforward = 1;
		}
		Controll.pl.goBody.transform.localEulerAngles = new Vector3(0f, rBodyY, 0f);
		rHeadY += Time.deltaTime * 4f * (float)headforward;
		if (rHeadY > headylimit[1])
		{
			rHeadY = headylimit[1];
			headforward = -1;
			if (Random.Range(0, 3) == 1)
			{
				headxlimit[0] = Random.Range(-20f, -1f);
				headxlimit[1] = Random.Range(1f, 20f);
			}
			else
			{
				headxlimit[0] = 0f;
				headxlimit[1] = 0f;
			}
		}
		else if (rHeadY < headylimit[0])
		{
			rHeadY = headylimit[0];
			headforward = 1;
			if (Random.Range(0, 3) == 1)
			{
				headxlimit[0] = Random.Range(-20f, -1f);
				headxlimit[1] = Random.Range(1f, 20f);
			}
			else
			{
				headxlimit[0] = 0f;
				headxlimit[1] = 0f;
			}
		}
		rHeadX += Time.deltaTime * 4f * (float)headxforward;
		if (rHeadX > headxlimit[1])
		{
			rHeadX = headxlimit[1];
			headxforward = -1;
		}
		else if (rHeadX < headxlimit[0])
		{
			rHeadX = headxlimit[0];
			headxforward = 1;
		}
		Controll.pl.goHead.transform.localEulerAngles = new Vector3(rHeadX, rHeadY, 0f);
	}
}
