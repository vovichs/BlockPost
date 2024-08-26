using UnityEngine;

public class GUIOptions : MonoBehaviour
{
	public static bool show = false;

	private Texture2D tBlack;

	private Texture2D tArrow_l;

	private Texture2D tArrow_r;

	private Rect rBack;

	private Rect rBackZone;

	private Rect rBackScroll;

	private Rect rBackScrollNone;

	private Rect[] rParam = new Rect[25];

	private Rect rLogout;

	private string[] var = new string[2];

	private string[] var1 = new string[2];

	private string[] varcolor = new string[5];

	private string[] varres;

	private string[] varfilter = new string[3];

	private string[] varmotion = new string[3];

	private string[] varpreset = new string[2];

	private string[] varmobileres = new string[6] { "50%", "60%", "70%", "80%", "90%", "100%" };

	private string[] varmobilefps = new string[3] { "30FPS", "45FPS", "60FPS" };

	private string[] varmobilefov = new string[4] { "50", "55", "60", "65" };

	private string[] vardistance = new string[7] { "64", "96", "128", "160", "192", "224", "256" };

	public static int gid = 0;

	public static string authkey = "key";

	public static int exp = 0;

	public static string playername = "RECRUIT";

	public static string sGold = "0";

	public static int Gold = 0;

	public static int NameCount = 0;

	public static string sNameCount;

	public static string sNameCount2 = "Доступных переименований: 0";

	public static string namekey = "key";

	public static int level = 0;

	public static bool globalpresetload = false;

	public static int globalpreset = 0;

	public static int distanceview = 0;

	public static int health_position = 0;

	public static int res = -1;

	public static int directlight = 0;

	public static int aa = 0;

	public static int flare = 0;

	public static int motion = 0;

	public static int particles = 1;

	public static int ssao = 0;

	public static float gamevolume = 0.5f;

	public static float musicvolume = 0.5f;

	public static int mobileres = 5;

	public static int mobilefps = 2;

	public static int mobilegreedy = 0;

	public static int mobilefov = 1;

	public static int mobilesecondattack = 0;

	private static bool changed = false;

	private static bool res_changed = false;

	private static float forg;

	private static int iorg;

	private static string saved = "";

	private static float savetime = 0f;

	public static bool configtouch = false;

	private static ControllTouch ct = null;

	private static GameObject goDlight = null;

	private Rect rCancel;

	private Rect rAccept;

	private Rect rBackingame;

	public static bool showingame = false;

	private string sColorSight;

	private string sDisplayLife;

	private string sFilterChat;

	private string sNetSurface;

	private string sDevelopMode;

	private string sResolut;

	private string sDynamicLight;

	private string sBlicks;

	private string sParticles;

	private string sSmoothing;

	private string sMotionBlur;

	private string sFullScreen;

	private string sMouseSens;

	private string sMouseSensAim;

	private string sSoundVolume;

	private string sMusicVolume;

	private string sRepeal;

	private string sSave;

	private string sSettSaved;

	private string sBack;

	private string sQuality;

	private string sLanguage;

	private string sExitFromProfile;

	private string sFPSLimit;

	private string sFieldOfView;

	private string sSens;

	private string sSensAim;

	private string sReset;

	private string sChangeControll;

	private string sPreset;

	private string sRenderDistance;

	private string sSecondAttack;

	private string sMovement;

	private string sWeapons;

	private string sChat;

	private string sGamePlay;

	private string sForward;

	private string sBackward;

	private string sStrafeLeft;

	private string sStrafeRight;

	private string sCrouch;

	private string sPrimary;

	private string sSecondary;

	private string sMelee;

	private string sBlock;

	private string sPrevious;

	private string sSpecial;

	private string sAllChat;

	private string sTeamChat;

	private string sClanChat;

	private string sChangeSet;

	private string sChangeTeam;

	public static KeyCode keyForward = KeyCode.W;

	public static KeyCode keyBackward = KeyCode.S;

	public static KeyCode keyStrafeLeft = KeyCode.A;

	public static KeyCode keyStrafeRight = KeyCode.D;

	public static KeyCode keyCrouch = KeyCode.LeftControl;

	public static KeyCode keyPrimary = KeyCode.Alpha1;

	public static KeyCode keySecondary = KeyCode.Alpha2;

	public static KeyCode keyShovel = KeyCode.Alpha3;

	public static KeyCode keyBlock = KeyCode.Alpha4;

	public static KeyCode keyPrevWeapon = KeyCode.Q;

	public static KeyCode keySpecial = KeyCode.G;

	public static KeyCode keyChat = KeyCode.Return;

	public static KeyCode keyClanChat = KeyCode.Y;

	public static KeyCode keyTeamChat = KeyCode.T;

	public static KeyCode keyChangeSet = KeyCode.E;

	public static KeyCode keyChangeTeam = KeyCode.M;

	private static Vector2 scroll = Vector2.zero;

	private string selectedkey = "";

	private static int nativewidth = -1;

	private static int nativeheight = -1;

	private Color ca = new Color(1f, 1f, 1f, 0.25f);

	private bool resetconfig;

	private float tresetconfig;

	private bool saveconfig;

	private float tsaveconfig;

	public void LoadLang()
	{
		sNameCount2 = Lang.GetString("_AVAILABLE_RENAME") + " 0";
		sNameCount = Lang.GetString("_AVAILABLE_RENAME");
		var[0] = Lang.GetString("_SWITCHED_OFF");
		var[1] = Lang.GetString("_SWITCHED_ON");
		var1[0] = Lang.GetString("_CENTER");
		var1[1] = Lang.GetString("_LEFT");
		varcolor[0] = Lang.GetString("_WHITE");
		varcolor[1] = Lang.GetString("_GREEN");
		varcolor[2] = Lang.GetString("_YELLOW");
		varcolor[3] = Lang.GetString("_BLUE");
		varcolor[4] = Lang.GetString("_ORANGE");
		varfilter[0] = Lang.GetString("_SWITCHED_OFF");
		varfilter[1] = Lang.GetString("_MEAN");
		varfilter[2] = Lang.GetString("_HIGH");
		varmotion[0] = Lang.GetString("_SWITCHED_OFF");
		varmotion[1] = Lang.GetString("_MEAN");
		varmotion[2] = Lang.GetString("_FULL");
		varpreset[0] = Lang.GetString("_PERFOMANCE");
		varpreset[1] = Lang.GetString("_QUALITY");
		sColorSight = Lang.GetString("_COLOR_SIGHT");
		sDisplayLife = Lang.GetString("_DISPLAY_LIFE");
		sFilterChat = Lang.GetString("_CHAT_FILTER");
		sNetSurface = Lang.GetString("_NETWORK_SURFACE");
		sDevelopMode = Lang.GetString("_DEVELOPERS_MODE");
		sResolut = Lang.GetString("_SCREEN_RESOLUTION");
		sDynamicLight = Lang.GetString("_DYNAMIC_LIGHTING");
		sBlicks = Lang.GetString("_BLICKS");
		sParticles = Lang.GetString("_PARTICLES");
		sSmoothing = Lang.GetString("_SMOOTHING");
		sMotionBlur = Lang.GetString("_MOTION_BLUR");
		sFullScreen = Lang.GetString("_FULL_SCREEN");
		sMouseSens = Lang.GetString("_MOUSE_SENSITIVITY");
		sMouseSensAim = Lang.GetString("_MOUSE_SENSITIVITY_AIM");
		sSoundVolume = Lang.GetString("_SOUND_VOLUME");
		sMusicVolume = Lang.GetString("_MUSIC_VOLUME");
		sRepeal = Lang.GetString("_REPEAL");
		sSave = Lang.GetString("_SAVE");
		sSettSaved = Lang.GetString("_SETTINGS_SAVED");
		sBack = Lang.GetString("_BACK");
		sQuality = Lang.GetString("_QUALITY_GRAPHICS");
		sLanguage = Lang.GetString("_LANGUAGE");
		sExitFromProfile = Lang.GetString("_LOGOUT");
		sFPSLimit = Lang.GetString("_FPS_LIMIT");
		sFieldOfView = Lang.GetString("_FIELDOFVIEW");
		sSens = Lang.GetString("_SENSITIVITY");
		sSensAim = Lang.GetString("_SENSITIVITY_AIM");
		sReset = Lang.GetString("_RESET");
		sChangeControll = Lang.GetString("_CHANGE_CONTROL");
		sPreset = Lang.GetString("_PRESET");
		sRenderDistance = Lang.GetString("_RENDER_DISTANCE");
		sMovement = Lang.GetString("_MOVEMENT");
		sWeapons = Lang.GetString("_WEAPONS");
		sChat = Lang.GetString("_CHAT");
		sGamePlay = Lang.GetString("_GAMEPLAY");
		sForward = Lang.GetString("_FORWARD");
		sBackward = Lang.GetString("_BACKWARD");
		sStrafeLeft = Lang.GetString("_STRAFE_LEFT");
		sStrafeRight = Lang.GetString("_STRAFE_RIGHT");
		sCrouch = Lang.GetString("_CROUCH");
		sPrimary = Lang.GetString("_PRIMARY");
		sSecondary = Lang.GetString("_SECONDARY");
		sMelee = Lang.GetString("_MELEE");
		sBlock = Lang.GetString("_BUILDING_BLOCK");
		sPrevious = Lang.GetString("_PREVIOUS_WEAPON");
		sSpecial = Lang.GetString("_ADDITIONAL");
		sAllChat = Lang.GetString("_ALL_CHAT");
		sTeamChat = Lang.GetString("_TEAM_CHAT");
		sClanChat = Lang.GetString("_CLANCHAT");
		sChangeSet = Lang.GetString("_CHANGE_SET");
		sChangeTeam = Lang.GetString("_CHANGE_TEAM");
		sSecondAttack = Lang.GetString("_SECOND_ATTACK");
	}

	public static void SetActive(bool val)
	{
		show = val;
		changed = false;
		saved = "";
		res_changed = false;
		if (val)
		{
			GUIFX.Set();
			return;
		}
		Apply();
		GUIFX.End();
		configtouch = false;
		if (ct != null && !HUD.isActive())
		{
			Object.Destroy(ct);
		}
		LoadConfigTouch();
		showingame = false;
	}

	public static void Load()
	{
		if (PlayerPrefs.HasKey("bp_options"))
		{
			Crosshair.crosshair_color = PlayerPrefs.GetInt("crosshair_color", 0);
			int @int = PlayerPrefs.GetInt("crosshair_type", 0);
			int int2 = PlayerPrefs.GetInt("crosshair_size", 0);
			Crosshair.SetCrosshair(@int);
			Crosshair.SetCrosshairSize(int2);
			health_position = PlayerPrefs.GetInt("health_position", 0);
			res = PlayerPrefs.GetInt("res", -1);
			globalpreset = PlayerPrefs.GetInt("globalpreset", 0);
			distanceview = PlayerPrefs.GetInt("distanceview", 0);
			mobileres = PlayerPrefs.GetInt("mobileres", 5);
			mobilefps = PlayerPrefs.GetInt("mobilefps", 1);
			mobilegreedy = PlayerPrefs.GetInt("mobilegreedy", 0);
			mobilefov = PlayerPrefs.GetInt("mobilefov", 1);
			mobilesecondattack = PlayerPrefs.GetInt("mobilesecondattack", 0);
			directlight = PlayerPrefs.GetInt("directlight", 0);
			Controll.sens = PlayerPrefs.GetFloat("sens", 5f);
			Controll.zoomsens = PlayerPrefs.GetFloat("zoomsens", 4f);
			aa = PlayerPrefs.GetInt("aa", 1);
			particles = PlayerPrefs.GetInt("particles", 0);
			flare = PlayerPrefs.GetInt("flare", 1);
			gamevolume = PlayerPrefs.GetFloat("gamevolume", 0.5f);
			musicvolume = PlayerPrefs.GetFloat("musicvolume", 0.5f);
			motion = PlayerPrefs.GetInt("motion", 0);
			ssao = PlayerPrefs.GetInt("ssao", 0);
			PLH.smooth_move = PlayerPrefs.GetInt("smooth_move", 1);
			HUDMessage.chatfilter = PlayerPrefs.GetInt("chatfilter", 1);
			GUIPlay.editor = PlayerPrefs.GetInt("editor", 0);
			if (PlayerPrefs.GetInt("invertmouse", 0) > 0)
			{
				Controll.invertmouse = true;
			}
			if (globalpreset == 0)
			{
				directlight = 0;
				aa = 1;
				particles = 0;
				flare = 1;
				ssao = 0;
				motion = 0;
			}
			globalpresetload = true;
		}
		else
		{
			MasterClient.cs.send_options();
		}
		LoadKeyConfig();
	}

	private static void LoadKeyConfig()
	{
		keyForward = (KeyCode)PlayerPrefs.GetInt("bp_key_forward", 119);
		keyBackward = (KeyCode)PlayerPrefs.GetInt("bp_key_backward", 115);
		keyStrafeLeft = (KeyCode)PlayerPrefs.GetInt("bp_key_strafeleft", 97);
		keyStrafeRight = (KeyCode)PlayerPrefs.GetInt("bp_key_straferight", 100);
		keyCrouch = (KeyCode)PlayerPrefs.GetInt("bp_key_crouch", 306);
		keyPrimary = (KeyCode)PlayerPrefs.GetInt("bp_key_primary", 49);
		keySecondary = (KeyCode)PlayerPrefs.GetInt("bp_key_secondary", 50);
		keyShovel = (KeyCode)PlayerPrefs.GetInt("bp_key_shovel", 51);
		keyBlock = (KeyCode)PlayerPrefs.GetInt("bp_key_block", 52);
		keyPrevWeapon = (KeyCode)PlayerPrefs.GetInt("bp_key_prevweapon", 113);
		keySpecial = (KeyCode)PlayerPrefs.GetInt("bp_key_special", 103);
		keyChat = (KeyCode)PlayerPrefs.GetInt("bp_key_chat", 13);
		keyClanChat = (KeyCode)PlayerPrefs.GetInt("bp_key_clanchat", 121);
		keyTeamChat = (KeyCode)PlayerPrefs.GetInt("bp_key_teamchat", 116);
		keyChangeSet = (KeyCode)PlayerPrefs.GetInt("bp_key_changeset", 101);
		keyChangeTeam = (KeyCode)PlayerPrefs.GetInt("bp_key_changeteam", 109);
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tArrow_l = ContentLoader_.LoadTexture("arrow_l") as Texture2D;
		tArrow_r = ContentLoader_.LoadTexture("arrow_r") as Texture2D;
		Resolution[] resolutions = Screen.resolutions;
		varres = new string[resolutions.Length];
		for (int i = 0; i < resolutions.Length; i++)
		{
			varres[i] = resolutions[i].width + "x" + resolutions[i].height;
		}
	}

	private void OnResize()
	{
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(500f));
		rBackZone = new Rect(rBack.x, rBack.y, rBack.width, rBack.height - GUIM.YRES(64f));
		rBackScrollNone = new Rect(0f, 0f, rBack.width - GUIM.YRES(32f), rBack.height - GUIM.YRES(64f));
		rBackScroll = new Rect(0f, 0f, rBack.width - GUIM.YRES(32f), GUIM.YRES(836f));
		for (int i = 0; i < rParam.Length; i++)
		{
			rParam[i] = new Rect(rBack.x + (rBack.width - GUIM.YRES(500f)) / 2f, rBack.y + GUIM.YRES(48f) + GUIM.YRES(70f) * (float)i, GUIM.YRES(500f), GUIM.YRES(64f));
		}
		rLogout = new Rect(rBack.x + (rBack.width - GUIM.YRES(300f)) / 2f, rBack.y + GUIM.YRES(48f) + GUIM.YRES(70f) * 5f, GUIM.YRES(300f), GUIM.YRES(40f));
	}

	private void OnChangeSubMenu()
	{
		if (show)
		{
			scroll = Vector2.zero;
			selectedkey = "";
		}
	}

	private void OnGUI()
	{
		if (!show)
		{
			return;
		}
		if (configtouch)
		{
			DrawConfigTouch();
			return;
		}
		GUIFX.Begin();
		if (showingame)
		{
			GUIM.DrawBox(rBack, TEX.tGray, 0.5f);
		}
		else
		{
			GUIM.DrawBox(rBack, tBlack, 0.05f);
		}
		if (Main.currSubMenu == 0)
		{
			DrawVar(rParam[0], sColorSight, varcolor, ref Crosshair.crosshair_color);
			DrawVar(rParam[1], sFilterChat, var, ref HUDMessage.chatfilter);
			if (DrawVar(rParam[3], sLanguage, Lang.langlistdesc_org, ref Lang.lang))
			{
				PlayerPrefs.SetInt("lang", Lang.lang);
				if (MainManager.mycom)
				{
					PlayerPrefs.SetInt("bp_mycom_lang", Lang.lang);
				}
				Lang.Init();
				ContentLoader_.BroadcastAll("LoadLang", "");
			}
			if (!showingame && GUIM.Button(rLogout, BaseColor.Orange, sExitFromProfile, TextAnchor.MiddleCenter, BaseColor.White, 0, 26, false))
			{
				MasterClient.cs.drop();
				MasterClient.cs.CloseClient();
				MasterClient.cs.PostAwake();
				MainManager.error = false;
				MainManager.authed = false;
				Main.mauth = false;
				Main.state = 0;
				GP.auth = false;
				GP.tokenloaded = false;
				PlayerPrefs.DeleteKey("bp_mobile_auth_id");
				PlayerPrefs.DeleteKey("bp_mobile_auth_key");
				GUIGameExit.ExitMainMenu();
			}
		}
		else if (Main.currSubMenu == 1)
		{
			DrawVar(rParam[0], sResolut, varmobileres, ref mobileres);
			DrawVar(rParam[1], sFPSLimit, varmobilefps, ref mobilefps);
			DrawVar(rParam[2], sFieldOfView, varmobilefov, ref mobilefov);
			DrawVar(rParam[3], sRenderDistance, vardistance, ref distanceview);
		}
		else if (Main.currSubMenu == 2)
		{
			DrawSlider(rParam[0], sSens, 0, 10, ref Controll.sens);
			DrawSlider(rParam[1], sSensAim, 0, 10, ref Controll.zoomsens);
			DrawVar(rParam[2], sSecondAttack, var, ref mobilesecondattack);
			if (GUIM.Button(rLogout, BaseColor.Blue, sChangeControll, TextAnchor.MiddleCenter, BaseColor.White, 0, 26, false))
			{
				configtouch = true;
				if (ct == null)
				{
					ct = base.gameObject.GetComponent<ControllTouch>();
				}
				if (ct == null)
				{
					ct = base.gameObject.AddComponent<ControllTouch>();
				}
			}
		}
		else if (Main.currSubMenu == 3)
		{
			DrawSlider(rParam[0], sSoundVolume, 0, 1, ref gamevolume);
			DrawSlider(rParam[1], sMusicVolume, 0, 1, ref musicvolume);
		}
		rAccept.Set(rBack.x + rBack.width - GUIM.YRES(200f), rBack.y + rBack.height - GUIM.YRES(44f), GUIM.YRES(184f), GUIM.YRES(28f));
		rCancel.Set(rBack.x + rBack.width - GUIM.YRES(200f) * 2f, rBack.y + rBack.height - GUIM.YRES(44f), GUIM.YRES(184f), GUIM.YRES(28f));
		if (changed)
		{
			if (GUIM.Button(rCancel, BaseColor.White, sRepeal, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				Load();
				changed = false;
			}
			if (GUIM.Button(rAccept, BaseColor.Orange, sSave, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				PlayerPrefs.SetInt("bp_options", 1);
				PlayerPrefs.SetInt("globalpreset", globalpreset);
				PlayerPrefs.SetInt("distanceview", distanceview);
				PlayerPrefs.SetInt("crosshair_color", Crosshair.crosshair_color);
				PlayerPrefs.SetInt("health_position", health_position);
				PlayerPrefs.SetInt("res", res);
				PlayerPrefs.SetInt("directlight", directlight);
				PlayerPrefs.SetFloat("sens", Controll.sens);
				PlayerPrefs.SetFloat("zoomsens", Controll.zoomsens);
				PlayerPrefs.SetFloat("gamevolume", gamevolume);
				PlayerPrefs.SetFloat("musicvolume", musicvolume);
				PlayerPrefs.SetInt("aa", aa);
				PlayerPrefs.SetInt("flare", flare);
				PlayerPrefs.SetInt("motion", motion);
				PlayerPrefs.SetInt("ssao", ssao);
				PlayerPrefs.SetInt("particles", particles);
				PlayerPrefs.SetInt("smooth_move", PLH.smooth_move);
				PlayerPrefs.SetInt("chatfilter", HUDMessage.chatfilter);
				PlayerPrefs.SetInt("editor", GUIPlay.editor);
				PlayerPrefs.SetInt("mobileres", mobileres);
				PlayerPrefs.SetInt("mobilefps", mobilefps);
				PlayerPrefs.SetInt("mobilegreedy", mobilegreedy);
				PlayerPrefs.SetInt("mobilefov", mobilefov);
				PlayerPrefs.SetInt("mobilesecondattack", mobilesecondattack);
				saved = sSettSaved;
				changed = false;
				savetime = Time.time + 2f;
				for (int i = 0; i < 40; i++)
				{
					if (PLH.player[i] != null)
					{
						Controll.SetVolume(PLH.player[i]);
					}
				}
				MasterClient.cs.send_saveoptions();
				if (res_changed)
				{
					res_changed = false;
					Resolution resolution = Screen.resolutions[res];
					Screen.SetResolution(resolution.width, resolution.height, true);
				}
			}
		}
		if (Time.time < savetime)
		{
			GUIM.DrawText(rAccept, saved, TextAnchor.MiddleCenter, BaseColor.Green, 1, 14, true);
		}
		if (showingame)
		{
			Main.DrawActiveMenu();
			rBackingame.Set(rBack.x + GUIM.YRES(16f), rBack.y + rBack.height - GUIM.YRES(44f), GUIM.YRES(184f), GUIM.YRES(28f));
			if (GUIM.Button(rBackingame, BaseColor.White, sBack, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				SetActive(false);
				GUIGameMenu.SetActive(false);
			}
		}
		GUIFX.End();
	}

	public bool DrawVarStatic(Rect r, string label, string[] var, ref int currvar)
	{
		if (currvar < 0)
		{
			currvar = var.Length - 1;
		}
		if (currvar >= var.Length)
		{
			currvar = var.Length - 1;
		}
		if (label.Length > 0)
		{
			GUIM.DrawBox(r, TEX.GetTextureByName("black"));
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y, GUIM.YRES(200f), GUIM.YRES(32f)), label, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, true);
		}
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(260f), r.y, GUIM.YRES(200f), GUIM.YRES(32f)), var[currvar], TextAnchor.MiddleCenter, BaseColor.White, 0, 20, true);
		return false;
	}

	public bool DrawVar(Rect r, string label, string[] var, ref int currvar, string customvar = null)
	{
		bool result = false;
		if (currvar < 0)
		{
			currvar = var.Length - 1;
		}
		if (currvar >= var.Length)
		{
			currvar = var.Length - 1;
		}
		int num = currvar;
		if (label.Length > 0)
		{
			GUIM.DrawBox(r, TEX.GetTextureByName("black"));
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y, GUIM.YRES(200f), GUIM.YRES(64f)), label, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 26, true);
		}
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(255f), r.y, GUIM.YRES(200f), GUIM.YRES(64f)), var[currvar], TextAnchor.MiddleCenter, BaseColor.White, 0, 26, true);
		Rect rect = new Rect(r.x + GUIM.YRES(214f), r.y + GUIM.YRES(8f), GUIM.YRES(48f), GUIM.YRES(48f));
		Rect rect2 = new Rect(r.x + GUIM.YRES(446f), r.y + GUIM.YRES(8f), GUIM.YRES(48f), GUIM.YRES(48f));
		if (GUIM.Contains(rect))
		{
			GUI.color = Color.yellow;
		}
		else
		{
			GUI.color = Color.white;
		}
		if (currvar == 0)
		{
			GUI.color = Color.gray;
		}
		GUI.DrawTexture(rect, tArrow_l);
		if (GUIM.Contains(rect2))
		{
			GUI.color = Color.yellow;
		}
		else
		{
			GUI.color = Color.white;
		}
		if (currvar == var.Length - 1)
		{
			GUI.color = Color.gray;
		}
		GUI.DrawTexture(rect2, tArrow_r);
		if (GUIM.HideButton(rect))
		{
			currvar--;
			if (currvar < 0)
			{
				currvar = 0;
			}
		}
		else if (GUIM.HideButton(rect2))
		{
			currvar++;
			if (currvar > var.Length - 1)
			{
				currvar = var.Length - 1;
			}
		}
		if (num != currvar)
		{
			changed = true;
			result = true;
			saved = "";
		}
		GUI.color = Color.white;
		return result;
	}

	public bool DrawHint(Rect r, string label)
	{
		GUIM.DrawBox(r, TEX.GetTextureByName("black"));
		GUIM.DrawText(r, label, TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, true);
		return true;
	}

	private void DrawSlider(Rect r, string label, int min, int max, ref float param)
	{
		forg = param;
		GUIM.DrawBox(r, TEX.GetTextureByName("black"));
		GUI.DrawTexture(new Rect(r.x + GUIM.YRES(260f), r.y + GUIM.YRES(24f), GUIM.YRES(200f), GUIM.YRES(16f)), TEX.tGray);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y, GUIM.YRES(200f), GUIM.YRES(64f)), label, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 26, true);
		param = GUIM.DrawSlider(new Rect(r.x + GUIM.YRES(260f), r.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(64f)), (int)GUIM.YRES(200f), min, max, param);
		param = (float)(int)(param * 10f + 0.05f) / 10f;
		GUIM.DrawText(new Rect(r.x + r.width - GUIM.YRES(56f), r.y, GUIM.YRES(40f), GUIM.YRES(64f)), param.ToString("0.0"), TextAnchor.MiddleRight, BaseColor.White, 0, 26, true);
		if (param != forg)
		{
			changed = true;
			saved = "";
		}
	}

	private void DrawLabel(Rect r, string label)
	{
		GUIM.DrawText(new Rect(r.x, r.y, GUIM.YRES(200f), GUIM.YRES(32f)), label, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
	}

	private void DrawLabelCenter(Rect r, string label)
	{
		GUIM.DrawText(r, label, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, true);
	}

	private void DrawKey(Rect r, string label, string codename, ref KeyCode key)
	{
		GUIM.DrawBox(r, TEX.GetTextureByName("black"));
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y, GUIM.YRES(200f), GUIM.YRES(32f)), label, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, true);
		bool flag = selectedkey == codename;
		if (flag)
		{
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(260f), r.y, GUIM.YRES(200f), GUIM.YRES(32f)), "_", TextAnchor.MiddleCenter, BaseColor.Orange, 0, 20, true);
		}
		else
		{
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(260f), r.y, GUIM.YRES(200f), GUIM.YRES(32f)), key.ToString(), TextAnchor.MiddleCenter, BaseColor.White, 0, 20, true);
		}
		if (GUIM.HideButton(r) && !flag)
		{
			selectedkey = codename;
		}
		if (!flag)
		{
			return;
		}
		Event current = Event.current;
		if (current.isKey)
		{
			if (current.keyCode != KeyCode.Escape && current.keyCode != KeyCode.BackQuote)
			{
				key = current.keyCode;
				PlayerPrefs.SetInt(codename, (int)key);
			}
			selectedkey = "";
		}
		else if (current.shift)
		{
			key = KeyCode.LeftShift;
			PlayerPrefs.SetInt(codename, (int)key);
			selectedkey = "";
		}
	}

	private void DrawPics(Rect r, string label, Texture2D[] var, ref int currvar)
	{
		GUIM.DrawBox(r, TEX.GetTextureByName("black"));
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y, GUIM.YRES(200f), GUIM.YRES(32f)), label, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, true);
	}

	public static void Apply()
	{
		if (nativewidth < 0)
		{
			nativewidth = Screen.width;
			nativeheight = Screen.height;
		}
		if (mobileres == 5)
		{
			Screen.SetResolution(nativewidth, nativeheight, true);
		}
		else if (mobileres == 4)
		{
			Screen.SetResolution((int)((float)nativewidth * 0.9f), (int)((float)nativeheight * 0.9f), true);
		}
		else if (mobileres == 3)
		{
			Screen.SetResolution((int)((float)nativewidth * 0.8f), (int)((float)nativeheight * 0.8f), true);
		}
		else if (mobileres == 2)
		{
			Screen.SetResolution((int)((float)nativewidth * 0.7f), (int)((float)nativeheight * 0.7f), true);
		}
		else if (mobileres == 1)
		{
			Screen.SetResolution((int)((float)nativewidth * 0.6f), (int)((float)nativeheight * 0.6f), true);
		}
		else if (mobileres == 0)
		{
			Screen.SetResolution((int)((float)nativewidth * 0.5f), (int)((float)nativeheight * 0.5f), true);
		}
		else if (mobilefps == 0)
		{
			Application.targetFrameRate = 30;
		}
		else if (mobilefps == 1)
		{
			Application.targetFrameRate = 45;
		}
		else if (mobilefps == 2)
		{
			Application.targetFrameRate = 60;
		}
		if (goDlight == null)
		{
			goDlight = GameObject.Find("Dlight");
		}
		Light component = goDlight.GetComponent<Light>();
		if (component != null)
		{
			if (directlight == 1)
			{
				component.shadows = LightShadows.Soft;
			}
			else
			{
				component.shadows = LightShadows.None;
			}
		}
		if (Controll.flCamera != null)
		{
			Controll.flCamera.enabled = ((flare != 0) ? true : false);
		}
		if (motion > 0)
		{
			if (Controll.ame != null)
			{
				Controll.ame.enabled = true;
				if (motion == 2)
				{
					Controll.ame.CameraMotionMult = 0.1f;
				}
				else
				{
					Controll.ame.CameraMotionMult = 0f;
				}
			}
			else
			{
				Controll.AddMotionCamera();
				for (int i = 0; i < 40; i++)
				{
					if (PLH.player[i] != null && !(PLH.player[i].go == null) && PLH.player[i].go.GetComponent<AmplifyMotionObject>() == null)
					{
						PLH.player[i].go.AddComponent<AmplifyMotionObject>();
					}
				}
			}
		}
		else if (Controll.ame != null)
		{
			Controll.ame.enabled = false;
		}
		if (globalpresetload && globalpreset == 0)
		{
			ssao = 0;
		}
		if (ssao > 0)
		{
			if (Controll.aoe != null)
			{
				Controll.aoe.enabled = true;
			}
			else
			{
				Controll.AddSSAO();
			}
		}
		else if (Controll.aoe != null)
		{
			Controll.aoe.enabled = false;
		}
		if (aa == 1 || aa == 2)
		{
			Atlas.SetFilter(true);
		}
		else
		{
			Atlas.SetFilter(false);
		}
		if (aa == 2)
		{
			if (Controll.fxaa != null)
			{
				Controll.fxaa.enabled = true;
			}
			else
			{
				Controll.AddFXAA();
			}
		}
		else if (Controll.fxaa != null)
		{
			Controll.fxaa.enabled = false;
		}
		HUD.OnResize_Health();
		GOpt.SetShaders(globalpreset);
	}

	private void DrawConfigTouch()
	{
		if (ct == null)
		{
			return;
		}
		ControllTouch.cs.DrawCell();
		ControllTouch.cs.DrawButtonsOptions();
		ControllTouch.cs.CollectTouch();
		ControllTouch.cs.SelectButton();
		ControllTouch.cs.DrawButtons();
		HUD.cs.DrawHealth();
		HUD.cs.DrawAmmo();
		Rect rect = new Rect((float)Screen.width / 2f - GUIM.YRES(100f), GUIM.YRES(160f) + GUIM.YRES(60f) * 0f, GUIM.YRES(200f), GUIM.YRES(45f));
		Rect rect2 = new Rect((float)Screen.width / 2f - GUIM.YRES(100f), GUIM.YRES(160f) + GUIM.YRES(60f) * 1f, GUIM.YRES(200f), GUIM.YRES(45f));
		Rect rect3 = new Rect((float)Screen.width / 2f - GUIM.YRES(100f), GUIM.YRES(160f) + GUIM.YRES(60f) * 2f, GUIM.YRES(200f), GUIM.YRES(45f));
		GUI.DrawTexture(rect, TEX.tBlue);
		GUIM.DrawText(rect, sBack, TextAnchor.MiddleCenter, BaseColor.White, 0, 26, false);
		if (GUIM.HideButton(rect))
		{
			configtouch = false;
			if (ct != null && !HUD.isActive())
			{
				Object.Destroy(ct);
			}
			LoadConfigTouch();
		}
		if (resetconfig)
		{
			if (Time.time > tresetconfig)
			{
				resetconfig = false;
			}
		}
		else
		{
			GUI.color = ca;
		}
		GUI.DrawTexture(rect2, TEX.tOrange);
		GUIM.DrawText(rect2, sReset, TextAnchor.MiddleCenter, BaseColor.White, 0, 26, false);
		GUI.color = Color.white;
		if (GUIM.HideButton(rect2))
		{
			if (resetconfig)
			{
				resetconfig = false;
				for (int i = 0; i < ControllTouch.btncount; i++)
				{
					ControllTouch.buttonpos[i].x = ControllTouch.buttonpos_default[i].x;
					ControllTouch.buttonpos[i].y = ControllTouch.buttonpos_default[i].y;
					ControllTouch.buttonscale[i] = 100;
				}
				ControllTouch.cs.OnResize();
			}
			else
			{
				resetconfig = true;
				tresetconfig = Time.time + 3f;
			}
		}
		if (saveconfig)
		{
			if (Time.time > tsaveconfig)
			{
				saveconfig = false;
			}
			GUI.color = ca;
		}
		GUI.DrawTexture(rect3, TEX.tGreen);
		GUIM.DrawText(rect3, saveconfig ? sSettSaved : sSave, TextAnchor.MiddleCenter, BaseColor.White, 0, 26, false);
		if (GUIM.HideButton(rect3))
		{
			SaveConfigTouch();
			saveconfig = true;
			tsaveconfig = Time.time + 3f;
		}
	}

	public static void LoadConfigTouch()
	{
		if (PlayerPrefs.HasKey("bp_touch"))
		{
			for (int i = 0; i < ControllTouch.btncount; i++)
			{
				ControllTouch.buttonpos[i].x = PlayerPrefs.GetInt("bp_touch_button_" + i + "_x", (int)ControllTouch.buttonpos_default[i].x);
				ControllTouch.buttonpos[i].y = PlayerPrefs.GetInt("bp_touch_button_" + i + "_y", (int)ControllTouch.buttonpos_default[i].y);
				ControllTouch.buttonscale[i] = PlayerPrefs.GetInt("bp_touch_button_" + i + "_s", ControllTouch.buttonscale[i]);
			}
			if (ControllTouch.cs != null)
			{
				ControllTouch.cs.OnResize();
			}
		}
	}

	private static void SaveConfigTouch()
	{
		PlayerPrefs.SetInt("bp_touch", 1);
		for (int i = 0; i < ControllTouch.btncount; i++)
		{
			UpdateButtonTouch(i);
			PlayerPrefs.SetInt("bp_touch_button_" + i + "_x", (int)ControllTouch.buttonpos[i].x);
			PlayerPrefs.SetInt("bp_touch_button_" + i + "_y", (int)ControllTouch.buttonpos[i].y);
			PlayerPrefs.SetInt("bp_touch_button_" + i + "_s", ControllTouch.buttonscale[i]);
		}
	}

	public static void UpdateButtonTouch(int i)
	{
		int num = 0;
		int num2 = 0;
		if (ControllTouch.buttonalign[i] == 0)
		{
			num = (int)GUIM.NATIVERES((float)Screen.width - ControllTouch.rButton[i].x);
			num2 = (int)GUIM.NATIVERES((float)Screen.height - ControllTouch.rButton[i].y);
		}
		else if (ControllTouch.buttonalign[i] == 1)
		{
			num = (int)GUIM.NATIVERES(ControllTouch.rButton[i].x);
			num2 = (int)GUIM.NATIVERES(ControllTouch.rButton[i].y);
		}
		else if (ControllTouch.buttonalign[i] == 2)
		{
			num = (int)GUIM.NATIVERES((float)Screen.width - ControllTouch.rButton[i].x);
			num2 = (int)GUIM.NATIVERES(ControllTouch.rButton[i].y);
		}
		else if (ControllTouch.buttonalign[i] == 3)
		{
			num = (int)GUIM.NATIVERES((float)Screen.width / 2f - ControllTouch.rButton[i].x);
			num2 = (int)GUIM.NATIVERES(ControllTouch.rButton[i].y);
		}
		else if (ControllTouch.buttonalign[i] == 4)
		{
			num = (int)GUIM.NATIVERES(ControllTouch.rButton[i].x);
			num2 = (int)GUIM.NATIVERES((float)Screen.height - ControllTouch.rButton[i].y);
		}
		ControllTouch.buttonpos[i].x = num;
		ControllTouch.buttonpos[i].y = num2;
	}
}
