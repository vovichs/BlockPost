using Player;
using UnityEngine;

public class HUD : MonoBehaviour
{
	public static HUD cs = null;

	private static bool show = false;

	public static int showhint = 0;

	private static float tHintTime = 0f;

	private static Texture2D tVig = null;

	private static Texture2D tHealth = null;

	private static Texture2D tBlack = null;

	private static Texture2D tGreen = null;

	private static Texture2D tYellow = null;

	private static Texture2D tWhite = null;

	private static Texture2D tOrange = null;

	private static Texture2D tRed = null;

	private Rect rScreen;

	private static Rect rHealth;

	private static Rect rHealthIcon;

	private static Rect rHealthText;

	private static Rect rHealthLine;

	private static Rect rHealthLineShadow;

	private static Rect rAmmo;

	private static Rect rAmmoClip;

	private static Rect rAmmoPack;

	private static Rect rWeaponName;

	private static Rect rWeaponIcon;

	private static Rect rAmmoLine;

	private Rect rHint;

	private static Rect rScoreBack;

	private static Rect rRD;

	private static Rect rBL;

	private static Rect rScore0;

	private static Rect rScore1;

	private static Rect rTimer;

	private Rect rNoAmmo;

	private Rect rc;

	private Rect rcw;

	private Rect rReload;

	private Rect rReloadBar;

	private Rect rReloadBarEnd;

	private Rect rReloadActive;

	private Rect rReloadBack;

	private Rect rReloadText;

	private Rect rIcon;

	private Rect rIconSpecial;

	private Rect rConsole;

	private Rect rDemoLine;

	private static Rect[] rPlayerIconsRed = new Rect[8];

	private static Rect[] rPlayerIconsBlue = new Rect[8];

	private static Rect[] rPlayerIconsRed_2 = new Rect[16];

	private static Rect[] rPlayerIconsBlue_2 = new Rect[16];

	public static int iHealth = 0;

	private static string sHealth = "100";

	private static string sAmmoClip = "30";

	private static string sAmmoPack = "|  250";

	public static string sScore0 = "-";

	public static string sScore1 = "-";

	public static int iScore0 = 0;

	public static int iScore1 = 0;

	private static string sSec = "00:00";

	public static int iAmmoClip = 0;

	private static int iSec = 0;

	private static int iLastSec = 0;

	private static float tSec = 0f;

	private string sWeapon = "НЕТ ОРУЖИЯ";

	private string[] sReloadMsg = new string[4] { "МАСТЕРСКИ!", "ОТЛИЧНО!", "ПРЕВОСХОДНО!", "ИДЕАЛЬНО!" };

	private string[] sHint = new string[2] { "НАЖМИТЕ <color=yellow>E</color> ДЛЯ ВЫБОРА СЕТА ОРУЖИЯ", "ОРУЖИЕ СМЕНИТСЯ ПРИ СЛЕДУЮЩЕМ ВОЗРОЖДЕНИИ" };

	private Color a25 = new Color(0f, 0f, 0f, 0.25f);

	private Color a75 = new Color(1f, 1f, 1f, 0.75f);

	private Color aw25 = new Color(1f, 1f, 1f, 0.25f);

	private string sNoAmmo = "НЕТ ПАТРОНОВ";

	private Color cNoAmmo = Color.white;

	private static float tNoAmmo = 0f;

	private string sOverHeat = "ПЕРЕГРЕВ";

	private Color cOverHeat = Color.white;

	private static float tOverHeat = 0f;

	private static int[] medal = new int[128];

	private static int medalcount = 0;

	public static int streakcount = 0;

	private Texture2D tFrag;

	private Texture2D tHeadshot;

	private Texture2D t10;

	private Texture2D t25;

	private Texture2D t50;

	private Texture2D t100;

	private Texture2D[] tIconSpecial = new Texture2D[4];

	private static bool fragfx = true;

	private static float fragtime = 0f;

	private static int fragmedal = 0;

	private static string fragstreak = "";

	private static AudioClip sDryFire = null;

	private static AudioClip sSelfHit = null;

	public static AudioClip sMedkit = null;

	public static AudioClip sAmmokit = null;

	private static AudioClip sOverheat = null;

	private static AudioClip sHeartBeat = null;

	public static bool show_armor = false;

	private Rect rGGRank;

	private Rect rGGIcon;

	private Rect rGGIconStar;

	private string demomode = "PLAYDEMO MODE ALPHA";

	public static float maxhealth = 100f;

	public static string ggrank = "";

	public static string ggfrags = "";

	public static WeaponInfo ggw = null;

	public static int iggrank = 0;

	public static int iggmaxrank = 0;

	private Texture2D tStar;

	private Rect r;

	private float ofs;

	private float ofsx;

	private float ofsy;

	private float scale;

	public static bool fx = false;

	private WeaponSet ws;

	private int wid;

	private Color a3 = new Color(1f, 1f, 1f, 0.3f);

	private Rect rProgress;

	private Rect rIcon2;

	private static Color fadecolor = Color.black;

	private static bool inFade = false;

	private static float fadetime = 0f;

	private static float fadesec = 0f;

	private static float transparent = 0f;

	private static bool drawmessage = false;

	private static float timemessage = 0f;

	private static string message = "";

	private static bool drawmessage2 = false;

	private static float timemessage2 = 0f;

	private static string message2 = "";

	private static bool bInfect = false;

	private float tInfect;

	private int iInfect;

	private Rect rInfect;

	private string sInfect;

	public static bool showcallvote = false;

	private static string cvplayername = "";

	private static string cvvote0 = "(0)";

	private static string cvvote1 = "(0)";

	private AudioClip[] sCustomSound;

	private void Awake()
	{
		cs = this;
	}

	public void LoadLang()
	{
		sWeapon = Lang.GetString("_NO_WEAPON");
		sReloadMsg[0] = Lang.GetString("_MASTERLY");
		sReloadMsg[1] = Lang.GetString("_PERFECTLY");
		sReloadMsg[2] = Lang.GetString("_EXCELLENT");
		sReloadMsg[3] = Lang.GetString("_IDEALLY");
		sHint[0] = Lang.GetString("_CLICK_SELECT_WEAPON");
		sHint[1] = Lang.GetString("_WEAPON_CHANGE_REVIVAL");
		sNoAmmo = Lang.GetString("_NO_AMMO");
	}

	public static void SetActive(bool val)
	{
		show = val;
		HUDMessage.SetActive(val);
		HUDIndicator.SetActive(val);
		if (!val)
		{
			HUDKiller.SetActive(false);
			bInfect = false;
			showcallvote = false;
		}
	}

	public static bool isActive()
	{
		return show;
	}

	public static void Clear()
	{
		medalcount = 0;
		streakcount = 0;
	}

	public static void SetHint(int val, float showtime = 120f)
	{
		showhint = val;
		tHintTime = Time.time + showtime;
	}

	public static void SetHealth(int val)
	{
		bool flag = false;
		if (val < iHealth)
		{
			flag = true;
		}
		iHealth = val;
		sHealth = val.ToString();
		float width = (rHealth.width - GUIM.YRES(20f)) * ((float)val / maxhealth);
		float num = (rHealth.width - GUIM.YRES(20f)) * (1f - (float)val / maxhealth);
		rHealthLine.Set(rHealthIcon.x + GUIM.YRES(10f) + num, (float)Screen.height - GUIM.YRES(18f), width, GUIM.YRES(4f));
		rHealthLineShadow.Set(rHealthLine.x + (float)(int)GUIM.YRES(2f), rHealthLine.y + (float)(int)GUIM.YRES(2f), rHealthLine.width, rHealthLine.height);
		if (val != 100 && flag)
		{
			SetFade(Color.red, 0.25f, 0.5f);
			Controll.pl.asFX.PlayOneShot(sSelfHit);
		}
	}

	public static void SetAmmo(int val, int max)
	{
		iAmmoClip = val;
		sAmmoClip = val.ToString();
		float width = (rAmmo.width - GUIM.YRES(20f)) * ((float)val / (float)max);
		float num = (rAmmo.width - GUIM.YRES(20f)) * (1f - (float)val / (float)max);
		rAmmoLine.Set(rAmmo.x + GUIM.YRES(10f) + num, (float)Screen.height - GUIM.YRES(18f), width, GUIM.YRES(4f));
	}

	public static void SetBackPack(int val)
	{
		if (val < 10)
		{
			sAmmoPack = "|    " + val;
		}
		else if (val < 100)
		{
			sAmmoPack = "|   " + val;
		}
		else if (val < 1000)
		{
			sAmmoPack = "|  " + val;
		}
		else
		{
			sAmmoPack = "| " + val;
		}
	}

	public static void SetScoreTime(int score0, int score1, int sec)
	{
		sScore0 = score0.ToString();
		sScore1 = score1.ToString();
		iScore0 = score0;
		iScore1 = score1;
		iSec = sec;
		tSec = Time.time;
		iLastSec = -1;
	}

	public static void SetNoAmmo()
	{
		tNoAmmo = Time.time + 1f;
		if (sDryFire == null)
		{
			sDryFire = ContentLoader_.LoadAudio("dryfire");
		}
		Controll.pl.asFX.PlayOneShot(sDryFire);
	}

	public static void SetOverHeat()
	{
		tOverHeat = Time.time + 1f;
		if (sOverheat == null)
		{
			sOverheat = Resources.Load("sounds/overheat") as AudioClip;
		}
		Controll.pl.asFX.PlayOneShot(sOverheat);
	}

	public static void AddMedal(int m)
	{
		streakcount++;
		int num = -1;
		for (int i = 0; i < medalcount; i++)
		{
			if (medal[i] <= 1)
			{
				num = i;
				break;
			}
		}
		if (streakcount == 10)
		{
			medal[num] = 10;
			medalcount = num + 1;
		}
		else if (streakcount == 25)
		{
			medal[num] = 25;
			medalcount = num + 1;
		}
		else if (streakcount == 50)
		{
			medal[num] = 50;
			medalcount = num + 1;
		}
		else if (streakcount == 100)
		{
			medal[num] = 100;
			medalcount = num + 1;
		}
		else
		{
			if (medalcount < 128)
			{
				medal[medalcount] = m;
			}
			medalcount++;
		}
		fragfx = true;
		fragtime = Time.time;
		fragmedal = m;
		fragstreak = streakcount.ToString();
	}

	public static void ResetStreak()
	{
		for (int i = 0; i < medalcount; i++)
		{
			if (medal[i] <= 1)
			{
				medalcount = i;
				break;
			}
		}
		streakcount = 0;
	}

	private void LoadEnd()
	{
		tVig = ContentLoader_.LoadTexture("vig") as Texture2D;
		tHealth = ContentLoader_.LoadTexture("health") as Texture2D;
		tBlack = TEX.GetTextureByName("black");
		tGreen = TEX.GetTextureByName("green");
		tYellow = TEX.GetTextureByName("yellow");
		tWhite = TEX.GetTextureByName("white");
		tOrange = TEX.GetTextureByName("orange");
		tRed = TEX.GetTextureByName("red");
		tFrag = Resources.Load("icon_frag") as Texture2D;
		tHeadshot = Resources.Load("icon_headshot") as Texture2D;
		t10 = Resources.Load("icon_10") as Texture2D;
		t25 = Resources.Load("icon_25") as Texture2D;
		t50 = Resources.Load("icon_50") as Texture2D;
		t100 = Resources.Load("icon_100") as Texture2D;
		tIconSpecial[0] = Resources.Load("Textures/ammo_icon_32") as Texture2D;
		tIconSpecial[1] = Resources.Load("Textures/grenade_icon_32") as Texture2D;
		tIconSpecial[2] = Resources.Load("Textures/medkit_icon_32") as Texture2D;
		tIconSpecial[3] = Resources.Load("Textures/shield_icon_32") as Texture2D;
		sSelfHit = Resources.Load("selfhit") as AudioClip;
		sMedkit = Resources.Load("sounds/medkit") as AudioClip;
		sAmmokit = Resources.Load("sounds/ammokit") as AudioClip;
		OnResize();
	}

	private void OnResize()
	{
		rScreen.Set(0f, 0f, Screen.width, Screen.height);
		OnResize_Health();
		OnResize_Ammo();
		rHint.Set((float)Screen.width / 2f - GUIM.YRES(200f), (float)Screen.height - GUIM.YRES(120f), GUIM.YRES(400f), GUIM.YRES(32f));
		OnResize_Score();
		rNoAmmo.Set((float)Screen.width / 2f - GUIM.YRES(50f), (float)Screen.height / 2f + GUIM.YRES(60f), GUIM.YRES(100f), GUIM.YRES(32f));
		rIconSpecial.Set((float)Screen.width - GUIM.YRES(72f), GUIM.YRES(200f), GUIM.YRES(40f), GUIM.YRES(40f));
		rConsole = new Rect(0f, 0f, GUIM.YRES(100f), GUIM.YRES(100f));
		rGGRank = new Rect(GUIM.YRES(16f), GUIM.YRES(180f), GUIM.YRES(200f), GUIM.YRES(20f));
		rGGIcon = new Rect(rGGRank.x, rGGRank.y + GUIM.YRES(24f), GUIM.YRES(200f), GUIM.YRES(80f));
		rGGIconStar = new Rect(rGGRank.x + GUIM.YRES(65f), rGGRank.y + GUIM.YRES(39f), GUIM.YRES(70f), GUIM.YRES(70f));
		rDemoLine = new Rect(0f, 0f, Screen.width, GUIM.YRES(40f));
	}

	public static void OnResize_Score()
	{
		float num = GUIM.YRES(2f);
		rScoreBack.Set((float)Screen.width / 2f - GUIM.YRES(85f), GUIM.YRES(GUIGameMenu.show ? 96 : 32), GUIM.YRES(170f), GUIM.YRES(24f));
		rRD.Set(rScoreBack.x + GUIM.YRES(8f), rScoreBack.y + num, GUIM.YRES(60f), rScoreBack.height);
		rBL.Set(rScoreBack.x + rScoreBack.width - GUIM.YRES(8f) - GUIM.YRES(60f), rScoreBack.y + num, GUIM.YRES(60f), rScoreBack.height);
		rScore0.Set(rRD.x + GUIM.YRES(22f), rRD.y, GUIM.YRES(20f), rRD.height);
		rScore1.Set(rScoreBack.x + rScoreBack.width - GUIM.YRES(50f), rBL.y, GUIM.YRES(20f), rBL.height);
		rTimer.Set(rScoreBack.x, rScoreBack.y + num, rScoreBack.width, rScoreBack.height);
		for (int i = 0; i < 8; i++)
		{
			rPlayerIconsRed[i].Set(rScoreBack.x - GUIM.YRES(32f) * (float)i - GUIM.YRES(44f), rScoreBack.y - GUIM.YRES(2f), GUIM.YRES(28f), GUIM.YRES(28f));
			rPlayerIconsBlue[i].Set(rScoreBack.x + rScoreBack.width + GUIM.YRES(32f) * (float)i + GUIM.YRES(16f), rScoreBack.y - GUIM.YRES(2f), GUIM.YRES(28f), GUIM.YRES(28f));
			rPlayerIconsRed_2[i].Set(rScoreBack.x - GUIM.YRES(32f) * (float)i - GUIM.YRES(44f), rScoreBack.y - GUIM.YRES(2f) - GUIM.YRES(16f), GUIM.YRES(28f), GUIM.YRES(28f));
			rPlayerIconsBlue_2[i].Set(rScoreBack.x + rScoreBack.width + GUIM.YRES(32f) * (float)i + GUIM.YRES(16f), rScoreBack.y - GUIM.YRES(2f) - GUIM.YRES(16f), GUIM.YRES(28f), GUIM.YRES(28f));
			rPlayerIconsRed_2[i + 8] = rPlayerIconsRed_2[i];
			rPlayerIconsRed_2[i + 8].y += GUIM.YRES(32f);
			rPlayerIconsBlue_2[i + 8] = rPlayerIconsBlue_2[i];
			rPlayerIconsBlue_2[i + 8].y += GUIM.YRES(32f);
		}
	}

	public static void OnResize_Health()
	{
		float num = GUIM.YRES(16f);
		if (GUIOptions.health_position == 0)
		{
			num = (float)Screen.width / 2f - GUIM.YRES(132f);
		}
		num = (float)Screen.width / 2f - GUIM.YRES(160f);
		float width = GUIM.YRES(140f);
		float width2 = GUIM.YRES(60f);
		if (maxhealth > 999f)
		{
			width = GUIM.YRES(166f);
			width2 = GUIM.YRES(80f);
		}
		rHealth.Set(num, (float)Screen.height - GUIM.YRES(66f), width, GUIM.YRES(54f));
		rHealthText.Set(rHealth.x + GUIM.YRES(68f), (float)Screen.height - GUIM.YRES(48f), width2, GUIM.YRES(40f));
		rHealthIcon.Set(rHealthText.x - GUIM.YRES(68f), (float)Screen.height - GUIM.YRES(64f), GUIM.YRES(48f), GUIM.YRES(48f));
		rHealthLine.Set(rHealthIcon.x + GUIM.YRES(10f), (float)Screen.height - GUIM.YRES(18f), rHealth.width - GUIM.YRES(20f), GUIM.YRES(4f));
		rHealthLineShadow.Set(rHealthLine.x + (float)(int)GUIM.YRES(2f), rHealthLine.y + (float)(int)GUIM.YRES(2f), rHealthLine.width, rHealthLine.height);
	}

	public static void OnResize_Ammo(float x = 0f)
	{
		float x2 = ControllTouch.buttonpos[3].x;
		ref Vector2 reference = ref ControllTouch.buttonpos[3];
		rAmmo.Set((float)Screen.width - GUIM.YRES(x2) + x, (float)Screen.height - GUIM.YRES(66f), GUIM.YRES(304f), GUIM.YRES(54f));
		rAmmoClip.Set(rAmmo.x + GUIM.YRES(16f), (float)Screen.height - GUIM.YRES(48f), GUIM.YRES(60f), GUIM.YRES(40f));
		rAmmoPack.Set(rAmmo.x + GUIM.YRES(84f), (float)Screen.height - GUIM.YRES(60f), GUIM.YRES(60f), GUIM.YRES(40f));
		rWeaponName.Set(rAmmo.x + GUIM.YRES(200f), (float)Screen.height - GUIM.YRES(52f), GUIM.YRES(60f), GUIM.YRES(30f));
		rWeaponIcon.Set(rAmmo.x + GUIM.YRES(155f), (float)Screen.height - GUIM.YRES(62f), GUIM.YRES(130f), GUIM.YRES(40f));
		rAmmoLine.Set(rAmmo.x + GUIM.YRES(10f), (float)Screen.height - GUIM.YRES(18f), rAmmo.width - GUIM.YRES(20f), GUIM.YRES(4f));
	}

	private void OnGUI()
	{
		if (GUIM.HideButton(rConsole) && Console.cs != null)
		{
			Console.cs.ToggleActive();
		}
		if (show)
		{
			GUI.depth = -1;
			DrawFrag();
			DrawHealth();
			DrawAmmo();
			DrawHint();
			DrawNoAmmo();
			DrawOverHeat();
			DrawReload();
			DrawReloadText();
			DrawScores();
			DrawMedals();
			DrawSpecial();
			HUDKiller.cs._OnGUI();
			DrawMessage();
			DrawMessage2();
			DrawFade();
			DrawFreeze();
			DrawZMessage();
			DrawCallVote();
			DrawGunGame();
		}
	}

	private void DrawGunGame()
	{
		if (Controll.gamemode != 8)
		{
			return;
		}
		GUIM.DrawText(rGGRank, ggrank, TextAnchor.MiddleLeft, BaseColor.Yellow, 1, 16, true);
		GUIM.DrawText(rGGRank, ggfrags, TextAnchor.MiddleRight, BaseColor.White, 1, 16, true);
		if (iggrank + 1 == iggmaxrank)
		{
			if (tStar == null)
			{
				tStar = Resources.Load("quest_icon_star") as Texture2D;
			}
			GUI.color = Color.yellow;
			GUI.DrawTexture(rGGIconStar, tStar);
			GUI.color = Color.white;
		}
		else if (ggw != null)
		{
			GUIInv.DrawWeaponIcon(rGGIcon, ggw.tIcon, ggw.vIcon);
		}
	}

	private void DrawFrag()
	{
		if (!fragfx)
		{
			return;
		}
		float num = Time.time - fragtime;
		if (num > 2f)
		{
			num = 2f;
			fragfx = false;
		}
		scale = 1f;
		if (num <= 0.1f)
		{
			ofs = num * 12f * GUIM.YRES(48f);
			ofsx = ofs / 2f;
			ofsy = ofsx;
		}
		else if (num <= 0.2f)
		{
			num -= 0.1f;
			num = 0.1f - num;
			ofs = num * 12f * GUIM.YRES(48f) + GUIM.YRES(48f);
			ofsx = ofs / 2f;
			ofsy = ofsx;
		}
		else if (num <= 0.75f)
		{
			ofs = GUIM.YRES(48f);
			ofsx = ofs / 2f;
			ofsy = ofsx;
		}
		else if (num > 0.75f && num <= 0.85f)
		{
			num -= 0.75f;
			num = 0.1f - num;
			scale = num * 10f;
			ofsx = GUIM.YRES(48f) / 2f;
			ofsy = scale * GUIM.YRES(48f) / 2f;
		}
		if (num >= 0.75f)
		{
			r.Set((float)Screen.width / 2f - GUIM.YRES(24f), GUIM.YRES(280f) - GUIM.YRES(14f), GUIM.YRES(48f), GUIM.YRES(48f));
			GUIM.DrawText(r, fragstreak, TextAnchor.MiddleCenter, BaseColor.Yellow, 1, 38, true);
		}
		if (num <= 0.85f)
		{
			r.Set((float)Screen.width / 2f - ofsx, GUIM.YRES(280f) - ofsy, ofs, ofs * scale);
			if (fragmedal == 0)
			{
				GUI.DrawTexture(r, tFrag);
			}
			else if (fragmedal == 1)
			{
				GUI.DrawTexture(r, tHeadshot);
			}
		}
	}

	public void DrawHealth()
	{
		GUIM.DrawBox(rHealth, tBlack);
		GUI.DrawTexture(rHealthIcon, tHealth);
		GUI.color = Color.white;
		GUIM.DrawText(rHealthText, sHealth, TextAnchor.LowerRight, BaseColor.White, 1, 50, true);
		GUI.DrawTexture(rHealthLine, tGreen);
	}

	public void DrawAmmo()
	{
		if (fx)
		{
			if (Time.time < Controll.pl.drawtime + 0.1f)
			{
				fx = true;
				OnResize_Ammo((Controll.pl.drawtime + 0.1f - Time.time) * GUIM.YRES(300f));
			}
			else
			{
				fx = false;
				OnResize_Ammo(1f);
			}
		}
		GUIM.DrawBox(rAmmo, tBlack);
		GUIM.DrawText(rAmmoClip, sAmmoClip, TextAnchor.LowerRight, BaseColor.White, 1, 50, true);
		GUIM.DrawText(rAmmoPack, sAmmoPack, TextAnchor.LowerRight, BaseColor.White, 1, 25, true);
		if (Controll.pl.currweapon == null)
		{
			GUIM.DrawText(rWeaponName, sWeapon, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		else
		{
			if (Controll.pl.currweapon.hudicon == null)
			{
				return;
			}
			Vector2 vtc = Controll.pl.currweapon.vtc;
			float num = GUIM.YRES(10f);
			float num2 = vtc.x / vtc.y;
			float num3 = rWeaponIcon.width - num;
			float num4 = rWeaponIcon.height - num;
			if (num3 / num2 > num4)
			{
				num3 = num4 * num2;
			}
			float num5 = (rWeaponIcon.width - num3) / 2f;
			float num6 = (rWeaponIcon.height - num4) / 2f;
			rc.Set(0f, 1f - vtc.y / (float)Controll.pl.currweapon.hudicon.height, vtc.x / (float)Controll.pl.currweapon.hudicon.width, vtc.y / (float)Controll.pl.currweapon.hudicon.height);
			rcw.Set(rWeaponIcon.x + num5, rWeaponIcon.y + num6, num3, num4);
			GUI.DrawTextureWithTexCoords(rcw, Controll.pl.currweapon.hudicon, rc);
		}
		GUI.DrawTexture(rAmmoLine, tYellow);
	}

	private void DrawHint()
	{
		if (showhint >= 0 && !Builder.active && !DemoRec.isDemo())
		{
			if (Time.time > tHintTime)
			{
				showhint = -1;
				return;
			}
			GUIM.DrawBox(rHint, tBlack);
			GUIM.DrawText(rHint, sHint[showhint], TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
	}

	private void DrawNoAmmo()
	{
		if (!(Time.time > tNoAmmo))
		{
			float a = tNoAmmo - Time.time;
			cNoAmmo.a = a;
			GUI.color = cNoAmmo;
			GUIM.DrawText(rNoAmmo, sNoAmmo, TextAnchor.MiddleCenter, BaseColor.Red, 0, 16, false);
			GUI.color = Color.white;
		}
	}

	private void DrawOverHeat()
	{
		if (!(Time.time > tOverHeat))
		{
			float a = tOverHeat - Time.time;
			cOverHeat.a = a;
			GUI.color = cNoAmmo;
			GUIM.DrawText(rNoAmmo, sOverHeat, TextAnchor.MiddleCenter, BaseColor.Red, 0, 16, false);
			GUI.color = Color.white;
		}
	}

	private void DrawReload()
	{
		if (Controll.inReload)
		{
			rReload.Set(rAmmo.x, rAmmo.y - GUIM.YRES(20f), rAmmo.width, GUIM.YRES(16f));
			float num = (Time.time - Controll.reload_start) / (Controll.reload_end - Controll.reload_start);
			num *= num;
			int num2 = (int)GUIM.YRES(2f);
			if (num2 < 1)
			{
				num2 = 1;
			}
			int num3 = (int)GUIM.YRES(4f);
			if (Controll.reload_result == 1f)
			{
				num = Controll.reload_forcept;
			}
			rReloadBar.Set(rReload.x + (float)num3, rReload.y + (float)num3, (rReload.width - (float)(num3 * 2)) * num, rReload.height - (float)(num3 * 2));
			rReloadBarEnd.Set(rReloadBar.x + rReloadBar.width - (float)num2, rReloadBar.y, num2, rReloadBar.height);
			rReloadActive.Set(rReloadBar.x + (rReload.width - (float)(num3 * 2)) * Controll.reload_active, rReloadBar.y - (float)num2, GUIM.YRES(16f), rReloadBar.height + (float)(num2 * 2));
			rReloadBack.Set(rReloadActive.x - (float)num2, rReloadActive.y - (float)num2, rReloadActive.width + (float)(num2 * 2), rReloadActive.height + (float)(num2 * 2));
			GUIM.DrawBox(rReload, tBlack);
			GUIM.DrawBox(rReloadBack, tWhite, 0.1f);
			if (Controll.reload_result == 1f)
			{
				GUI.DrawTexture(rReloadBar, tGreen);
			}
			else if (Controll.reload_result == 2f)
			{
				GUI.DrawTexture(rReloadBar, tRed);
			}
			else
			{
				GUI.DrawTexture(rReloadBar, tYellow);
			}
			GUI.DrawTexture(rReloadBarEnd, tOrange);
		}
	}

	private void DrawReloadText()
	{
		if (!(Time.time > Controll.reload_end_msg))
		{
			rReloadText.Set(rAmmo.x, rAmmo.y - GUIM.YRES(40f), rAmmo.width, GUIM.YRES(16f));
			GUIM.DrawText(rReloadText, sReloadMsg[Controll.reload_count], TextAnchor.MiddleRight, BaseColor.Yellow, 1, 14, true);
		}
	}

	private void DrawScores()
	{
		int num = (int)(Time.time - tSec);
		if (iLastSec != num)
		{
			iLastSec = num;
			num = iSec - num;
			if (num < 0)
			{
				num = 0;
			}
			int num2 = num / 60;
			int num3 = num - num2 * 60;
			sSec = num2.ToString("00") + ":" + num3.ToString("00");
		}
		GUIM.DrawBox(rScoreBack, TEX.tBlack, 0.1f);
		GUIM.DrawText(rTimer, sSec, TextAnchor.MiddleCenter, BaseColor.LightGray2, 1, 12, false);
		if (Controll.gamemode != 2 && Controll.gamemode != 8)
		{
			if (Controll.gamemode == 5)
			{
				GUIM.DrawText(rRD, "HM", TextAnchor.MiddleLeft, BaseColor.Yellow, 1, 14, false);
				GUIM.DrawText(rBL, "ZM", TextAnchor.MiddleRight, BaseColor.Green, 1, 14, false);
				GUIM.DrawText(rScore0, sScore0, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
				GUIM.DrawText(rScore1, sScore1, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
			}
			else
			{
				GUIM.DrawText(rRD, "RD", TextAnchor.MiddleLeft, BaseColor.Orange, 1, 14, false);
				GUIM.DrawText(rBL, "BL", TextAnchor.MiddleRight, BaseColor.Blue, 1, 14, false);
				GUIM.DrawText(rScore0, sScore0, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
				GUIM.DrawText(rScore1, sScore1, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
			}
		}
		if (Controll.gamemode == 4 || Controll.gamemode == 6)
		{
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < 40; i++)
			{
				if (PLH.player[i] != null)
				{
					if (PLH.player[i].tAvatar == null)
					{
						PlayerData playerData = PLH.player[i];
						playerData.tAvatar = FaceGen.Build(playerData.cp[0], playerData.cp[1], playerData.cp[2], playerData.cp[3], playerData.cp[4], playerData.cp[5], playerData.cp[6], playerData.cp[7], playerData.cp[8], playerData.cp[9]);
					}
					if (PLH.player[i].bstate == 5)
					{
						GUI.color = aw25;
					}
					else
					{
						GUI.color = Color.white;
					}
					if (Controll.gamemode == 6 && PLH.player[i].goAttach != null)
					{
						GUI.color = GUIM.colorlist[4];
					}
					if (PLH.player[i].team == 0 && num4 < 8)
					{
						GUI.DrawTexture(rPlayerIconsRed[num4], PLH.player[i].tAvatar);
						num4++;
					}
					else if (PLH.player[i].team == 1 && num5 < 8)
					{
						GUI.DrawTexture(rPlayerIconsBlue[num5], PLH.player[i].tAvatar);
						num5++;
					}
				}
			}
			GUI.color = Color.white;
		}
		else
		{
			if (Controll.gamemode != 5)
			{
				return;
			}
			int num6 = 0;
			int num7 = 0;
			for (int j = 0; j < 40; j++)
			{
				if (PLH.player[j] == null || PLH.player[j].team >= 1)
				{
					continue;
				}
				if (PLH.player[j].skinstate == 0)
				{
					if (PLH.player[j].tAvatar == null)
					{
						PlayerData playerData2 = PLH.player[j];
						playerData2.tAvatar = FaceGen.Build(playerData2.cp[0], playerData2.cp[1], playerData2.cp[2], playerData2.cp[3], playerData2.cp[4], playerData2.cp[5], playerData2.cp[6], playerData2.cp[7], playerData2.cp[8], playerData2.cp[9]);
					}
					if (num6 < 16)
					{
						GUI.DrawTexture(rPlayerIconsRed_2[num6], PLH.player[j].tAvatar);
						num6++;
					}
					continue;
				}
				if (PLH.player[j].tAvatarZombie == null)
				{
					PlayerData playerData3 = PLH.player[j];
					playerData3.tAvatarZombie = FaceGen.BuildZombie(playerData3.idx, playerData3.team, playerData3.cp[0], playerData3.cp[1], playerData3.cp[2], playerData3.cp[3], playerData3.cp[4], playerData3.cp[5], playerData3.cp[6], playerData3.cp[7], playerData3.cp[8], playerData3.cp[9]);
				}
				if (PLH.player[j].bstate == 5)
				{
					GUI.color = aw25;
				}
				else
				{
					GUI.color = Color.white;
				}
				if (num7 < 16)
				{
					GUI.DrawTexture(rPlayerIconsBlue_2[num7], PLH.player[j].tAvatarZombie);
					num7++;
				}
			}
			GUI.color = Color.white;
		}
	}

	private void DrawSpecial()
	{
		ws = Controll.pl.wset[Controll.pl.currset];
		if (ws == null || ws.w[4] == null)
		{
			return;
		}
		wid = ws.w[4].wi.id;
		bool flag = false;
		if (Time.time < Controll.specialtime_end)
		{
			flag = true;
		}
		int num = 0;
		if (wid == 107)
		{
			num = 0;
		}
		else if (wid == 108)
		{
			num = 1;
		}
		else if (wid == 109)
		{
			num = 2;
		}
		else if (wid == 110)
		{
			num = 3;
		}
		if (num == 3)
		{
			if (show_armor)
			{
				GUI.color = Color.yellow;
				GUI.DrawTexture(rIconSpecial, tIconSpecial[num]);
			}
			else
			{
				GUI.color = a3;
				GUI.DrawTexture(rIconSpecial, tIconSpecial[num]);
			}
		}
		else if (flag)
		{
			GUI.color = a3;
			GUI.DrawTexture(rIconSpecial, tIconSpecial[num]);
			GUI.color = Color.white;
			float num2 = (Time.time - Controll.specialtime_start) / Controll.specialtime_duration;
			rProgress.Set(0f, 1f, 1f, num2);
			rIcon2.Set(rIconSpecial.x, rIconSpecial.y + rIconSpecial.height - rIconSpecial.height * num2, rIconSpecial.width, rIconSpecial.height * num2);
			GUI.DrawTextureWithTexCoords(rIcon2, tIconSpecial[num], rProgress);
		}
		else
		{
			GUI.color = Color.yellow;
			GUI.DrawTexture(rIconSpecial, tIconSpecial[num]);
		}
		GUI.color = Color.white;
	}

	private void DrawMedals()
	{
		int medalcount2 = medalcount;
		int num = 34;
		for (int i = 0; i < medalcount; i++)
		{
			float x = GUIM.YRES(16f) + GUIM.YRES(28f) * (float)i + ((GUIOptions.health_position == 1) ? GUIM.YRES(148f) : 0f);
			float y = (float)Screen.height - GUIM.YRES(40f);
			if (i > 16)
			{
				y = (float)Screen.height - GUIM.YRES(68f);
				x = GUIM.YRES(16f) + GUIM.YRES(28f) * (float)(i - 17) + ((GUIOptions.health_position == 1) ? GUIM.YRES(148f) : 0f);
			}
			rIcon.Set(x, y, GUIM.YRES(26f), GUIM.YRES(26f));
			if (medal[i] == 0)
			{
				GUI.DrawTexture(rIcon, tFrag);
			}
			else if (medal[i] == 1)
			{
				GUI.DrawTexture(rIcon, tHeadshot);
			}
			else if (medal[i] == 10)
			{
				GUI.DrawTexture(rIcon, t10);
			}
			else if (medal[i] == 25)
			{
				GUI.DrawTexture(rIcon, t25);
			}
			else if (medal[i] == 50)
			{
				GUI.DrawTexture(rIcon, t50);
			}
			else if (medal[i] == 100)
			{
				GUI.DrawTexture(rIcon, t100);
			}
		}
	}

	public static void SetFade(Color c, float time, float tr = 1f)
	{
		fadecolor = c;
		fadetime = Time.time + time;
		fadesec = time;
		inFade = true;
		transparent = tr;
	}

	private void DrawFade()
	{
		if (inFade)
		{
			if (Time.time > fadetime)
			{
				inFade = false;
				return;
			}
			float num = (fadetime - Time.time) / fadesec;
			fadecolor.a = num * transparent;
			GUI.color = fadecolor;
			GUI.DrawTexture(rScreen, tWhite);
			GUI.color = Color.white;
		}
	}

	private void DrawFreeze()
	{
		if (!Controll.inFreeze)
		{
			return;
		}
		float num = Controll.tFreeze - Time.time;
		if (!(num < 0f))
		{
			if (num <= 0.1f && Controll.ccc != null)
			{
				Controll.ccc.saturation = (0.1f - num) * 10f;
			}
			if (Controll.gamemode == 6 && num > 200f)
			{
				GUI.color = GUIM.colorlist[4];
				GUI.DrawTexture(rScreen, TEX.tWhiteAlpha);
				GUI.color = Color.white;
			}
		}
	}

	private void DrawMessage()
	{
		if (drawmessage)
		{
			if (Time.time > timemessage)
			{
				drawmessage = false;
			}
			else
			{
				GUIM.DrawText(new Rect(0f, GUIM.YRES(200f), Screen.width, GUIM.YRES(60f)), message, TextAnchor.MiddleCenter, BaseColor.White, 1, 40, true);
			}
		}
	}

	public static void SetMessage(string msg, int sec)
	{
		drawmessage = true;
		timemessage = Time.time + (float)sec;
		message = msg;
	}

	private void DrawMessage2()
	{
		if (drawmessage2)
		{
			if (Time.time > timemessage2)
			{
				drawmessage2 = false;
			}
			else
			{
				GUIM.DrawText(new Rect(0f, GUIM.YRES(292f), Screen.width, GUIM.YRES(64f)), message2, TextAnchor.MiddleCenter, BaseColor.Orange, 0, 28, true);
			}
		}
	}

	public static void SetMessage2(string msg)
	{
		drawmessage2 = true;
		timemessage2 = Time.time + 5f;
		message2 = msg;
	}

	private void Update()
	{
		UpdateZM();
	}

	public void SetZMessage(int msgid)
	{
		if (msgid == 0)
		{
			bInfect = true;
			tInfect = Time.time + 15f;
		}
	}

	private void DrawZMessage()
	{
		if (bInfect)
		{
			rInfect.Set(0f, GUIM.YRES(246f), Screen.width, GUIM.YRES(64f));
			int fontsize = 28;
			if (iInfect == 3)
			{
				fontsize = 30;
			}
			else if (iInfect == 2)
			{
				fontsize = 32;
			}
			else if (iInfect == 1)
			{
				fontsize = 34;
			}
			else if (iInfect == 0)
			{
				fontsize = 40;
			}
			GUIM.DrawText(rInfect, sInfect, TextAnchor.MiddleCenter, BaseColor.Red, 0, fontsize, true);
		}
	}

	private void UpdateZM()
	{
		if (!bInfect)
		{
			return;
		}
		if (Time.time >= tInfect)
		{
			bInfect = false;
			return;
		}
		int num = (int)(tInfect - Time.time);
		if (num != iInfect)
		{
			iInfect = num;
			if (num == 0)
			{
				sInfect = "SURVIVE";
			}
			else
			{
				sInfect = num.ToString();
			}
			if (sHeartBeat == null)
			{
				sHeartBeat = Resources.Load("sounds/heartbeat") as AudioClip;
			}
			if (num < 4 && Controll.pl.asFX != null)
			{
				Controll.pl.asFX.PlayOneShot(sHeartBeat);
			}
		}
	}

	public static void SetCallVote(int code, int id)
	{
		switch (code)
		{
		case 0:
			if (PLH.player[id] != null)
			{
				showcallvote = true;
				cvplayername = PLH.player[id].name;
				cvvote0 = "(0)";
				cvvote1 = "(0)";
			}
			break;
		case 1:
			showcallvote = false;
			break;
		}
	}

	public static void SetCallVoteResult(int v0, int v1)
	{
		cvvote1 = "(" + v0 + ")";
		cvvote0 = "(" + v1 + ")";
	}

	private void DrawCallVote()
	{
		if (showcallvote)
		{
			Rect rect = new Rect((float)Screen.width / 2f - GUIM.YRES(120f), GUIM.YRES(80f), GUIM.YRES(240f), GUIM.YRES(80f));
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, GUIM.YRES(32f));
			Rect rect3 = new Rect(rect.x, rect.y + GUIM.YRES(26f), rect.width, GUIM.YRES(26f));
			Rect rect4 = new Rect(rect.x + GUIM.YRES(16f), rect.y + rect.height - GUIM.YRES(26f), GUIM.YRES(80f), GUIM.YRES(26f));
			Rect rect5 = new Rect(rect.x + rect.width - GUIM.YRES(96f), rect.y + rect.height - GUIM.YRES(26f), GUIM.YRES(80f), GUIM.YRES(26f));
			Rect rect6 = rect4;
			rect6.x += GUIM.YRES(34f);
			Rect rect7 = rect5;
			rect7.x += GUIM.YRES(30f);
			GUIM.DrawBox(rect, TEX.tBlack, 0.15f);
			GUIM.DrawText(rect2, "VOTE KICK", TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, false);
			GUIM.DrawText(rect3, cvplayername, TextAnchor.MiddleCenter, BaseColor.White, 0, 24, false);
			GUIM.DrawText(rect4, "F1 <color=#2F2>YES</color>", TextAnchor.MiddleCenter, BaseColor.White, 0, 22, false);
			GUIM.DrawText(rect5, "F2 <color=#F20>NO</color>", TextAnchor.MiddleCenter, BaseColor.White, 0, 22, false);
			GUIM.DrawText(rect6, cvvote0, TextAnchor.MiddleCenter, BaseColor.White, 0, 18, false);
			GUIM.DrawText(rect7, cvvote1, TextAnchor.MiddleCenter, BaseColor.White, 0, 18, false);
		}
	}

	public void PlayCustomSound(int sid)
	{
		if (sCustomSound == null)
		{
			sCustomSound = new AudioClip[16];
		}
		if (sid == 0 && sCustomSound[sid] == null)
		{
			sCustomSound[sid] = Resources.Load("sounds/gg_frag") as AudioClip;
		}
		if (sid == 1 && sCustomSound[sid] == null)
		{
			sCustomSound[sid] = Resources.Load("sounds/gg_up") as AudioClip;
		}
		if (sid == 2 && sCustomSound[sid] == null)
		{
			sCustomSound[sid] = Resources.Load("sounds/gg_down") as AudioClip;
		}
		if (Controll.pl != null && Controll.pl.asFX != null)
		{
			Controll.pl.asFX.PlayOneShot(sCustomSound[sid], GUIOptions.gamevolume * ((sid == 0) ? 1f : 0.75f));
		}
	}
}
