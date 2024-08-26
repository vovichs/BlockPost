using System.Collections.Generic;
using System.Globalization;
using Player;
using UnityEngine;

public class GUIInv : MonoBehaviour
{
	public static bool show = false;

	public static bool changed = false;

	private static bool hideicon = false;

	private static Texture2D tBlack;

	private static Texture2D tWhite;

	private static Texture2D tYellow;

	private static Texture2D tOrange;

	private static Texture2D tRed;

	private static Texture2D tItem;

	private static Texture2D tDublicate;

	private static Vector2 vItem;

	private static Vector2 scroll = Vector2.zero;

	public static WeaponInfo[] winfo = new WeaponInfo[1024];

	public static List<WeaponInv> wlist = new List<WeaponInv>();

	public static WeaponSet[] wset = new WeaponSet[3];

	public static CaseInfo[] cinfo = new CaseInfo[1024];

	public static List<CaseInv> clist = new List<CaseInv>();

	public static KeyInfo[] kinfo = new KeyInfo[1024];

	public static List<KeyInv> klist = new List<KeyInv>();

	public static PlayerSkinInfo[] psinfo = new PlayerSkinInfo[32];

	public static List<PlayerSkinInv> pslist = new List<PlayerSkinInv>();

	private static GameObject goChar = null;

	private float start_x;

	private float lasttouch;

	private static WeaponInv cw = null;

	private static WeaponSet cs = null;

	private static WeaponData wd = null;

	private static bool detail = false;

	private static WeaponSkin currskin = null;

	private static int currskinid = 0;

	private static float hovertime = 0f;

	private static WeaponInv whover = null;

	private Rect rBack;

	private Rect rBackChar;

	private static Rect rScroll;

	private Rect rDetail;

	private Rect rEquip;

	private Rect rDescription;

	private Rect rCentral;

	private Rect rSet;

	private static GameObject goCamera = null;

	private static Camera csCharCam;

	private static PlayerData pr = new PlayerData();

	private static GameObject goDetail = null;

	private static GameObject goCameraDetail = null;

	private static bool mastersave = false;

	public static int[] hairlock = null;

	public static int[] hatlock = null;

	public static int[] bodylock = null;

	public static int[] pantslock = null;

	public static int[] bootslock = null;

	private Texture2D tArrow_l;

	private Texture2D tArrow_r;

	private string sDamage;

	private string sCage;

	private string sStock;

	private string sRateFire;

	private string sDistance;

	private string sAccuracy;

	private string sReload;

	private string sRecoil;

	private string sPropert;

	private string sMobility;

	private string sOverview;

	private string sWearWeap;

	private string sRemoveWeap;

	private string sColorSkin;

	private string sColorEye;

	private string sColorHair;

	private string sHairstyle;

	private string sEyeType;

	private string sBeard;

	private string sHat;

	private string sBody;

	private string sPants;

	private string sBoots;

	private string sBasicLvl;

	private string sFrags;

	private string sProgress;

	private string sOutSight;

	private string sSet1;

	private string sSet2;

	private string sSet3;

	private string sPrimWeap;

	private string sSecWeap;

	private string sMeleeWeap;

	private string sBuildBlock;

	private string sAddit;

	private string sReturn;

	private static float dif = 0f;

	private float start_y;

	private static CharColor colordev = new CharColor(0, Color.green, Color.green);

	public static int currcolorskin = 0;

	public static int currcoloreye = 0;

	public static int currcolorhair = 0;

	public static int currhair = 0;

	public static int curreye = 0;

	public static int currbeard = 0;

	public static int currhat = 0;

	public static int currbody = 0;

	public static int currpants = 0;

	public static int currboots = 0;

	private const int max_hair = 9;

	private const int max_eye = 7;

	private const int max_beard = 8;

	private Texture2D[] tHair_org = new Texture2D[9];

	private Texture2D[] tEye_org = new Texture2D[7];

	private Texture2D[] tBeard_org = new Texture2D[8];

	private Texture2D[] tHat_org = new Texture2D[31];

	private Texture2D[] tBody_org = new Texture2D[30];

	private Texture2D[] tPants_org = new Texture2D[29];

	private Texture2D[] tBoots_org = new Texture2D[3];

	public static Texture2D[] tHair = new Texture2D[9];

	public static Texture2D[] tEye = new Texture2D[7];

	public static Texture2D[] tBeard = new Texture2D[8];

	public static Texture2D[] tHat = new Texture2D[31];

	public static Texture2D[] tBody = new Texture2D[30];

	public static Texture2D[] tPants = new Texture2D[29];

	public static Texture2D[] tBoots = new Texture2D[3];

	private static int HairPage = 0;

	private static int HatPage = 0;

	private static int BodyPage = 0;

	private static int PantsPage = 0;

	private int preset = -1;

	private Rect rDetailHeader;

	private Rect rDetailHeader2;

	private Rect rDetailHeader3;

	private Rect rDetailHeader4;

	private Rect rAttachText;

	private Rect rIcon;

	private Rect rAttachLevel;

	public void LoadLang()
	{
		sDamage = Lang.GetString("_DAMAGE");
		sCage = Lang.GetString("_CAGE");
		sStock = Lang.GetString("_STOCK");
		sRateFire = Lang.GetString("_RATE_FIRE");
		sDistance = Lang.GetString("_DISTANCE");
		sAccuracy = Lang.GetString("_ACCURACY");
		sReload = Lang.GetString("_RELOAD_SPEED");
		sRecoil = Lang.GetString("_RECOIL");
		sPropert = Lang.GetString("_PROPERTIES");
		sMobility = Lang.GetString("_MOBILITY");
		sOverview = Lang.GetString("_OVERVIEW");
		sWearWeap = Lang.GetString("_WEAR_WEAPON");
		sRemoveWeap = Lang.GetString("_REMOVE_WEAPON");
		sColorSkin = Lang.GetString("_COLOR_SKIN");
		sColorEye = Lang.GetString("_COLOR_EYE");
		sColorHair = Lang.GetString("_COLOR_HAIR");
		sHairstyle = Lang.GetString("_HAIRSTYLE");
		sEyeType = Lang.GetString("_EYE_TYPE");
		sBeard = Lang.GetString("_BEARD");
		sHat = Lang.GetString("_HAT");
		sBody = Lang.GetString("_BODY");
		sPants = Lang.GetString("_PANTS");
		sBoots = Lang.GetString("_BOOTS");
		sBasicLvl = Lang.GetString("_BASIC_LVL");
		sFrags = Lang.GetString("_FRAGS");
		sProgress = Lang.GetString("_PROGRESS");
		sOutSight = Lang.GetString("_WITHOUT_SIGHT");
		sSet1 = Lang.GetString("_SET1");
		sSet2 = Lang.GetString("_SET2");
		sSet3 = Lang.GetString("_SET3");
		sPrimWeap = Lang.GetString("_PRIMARY");
		sSecWeap = Lang.GetString("_SECONDARY");
		sMeleeWeap = Lang.GetString("_MELEE");
		sBuildBlock = Lang.GetString("_BUILDING_BLOCK");
		sAddit = Lang.GetString("_ADDITIONAL");
		sReturn = Lang.GetString("_RETURN");
	}

	public static void SetActive(bool val)
	{
		show = val;
		if (val)
		{
			GUIFX.Set();
			CreateChar();
			CreateCamera();
			LoadSet();
			HatPage = 0;
			BodyPage = 0;
			PantsPage = 0;
		}
		else
		{
			DeleteChar();
			GUIFX.End();
			if (changed)
			{
				MasterClient.cs.send_savechar();
				Main.tAvatar = FaceGen.Build();
				PlayerPrefs.DeleteKey("bp_char" + MasterClient.vk_id);
			}
			DeleteCamera();
			DeleteDetail();
			if (wd != null)
			{
				Object.Destroy(wd.go);
				Object.Destroy(wd.goPW);
				wd = null;
			}
			if (mastersave)
			{
				if (!MainManager.steam)
				{
					MasterClient.cs.send_saveset();
				}
				PlayerPrefs.SetInt("bp_set", 1);
			}
		}
		mastersave = false;
		cw = null;
		whover = null;
		hovertime = 0f;
		changed = false;
		GUICase.show_case = false;
		Reset();
	}

	public static void LoadSet()
	{
		if (PlayerPrefs.HasKey("bp_set"))
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					string key = "set_" + i + "_slot_" + j + "_uid";
					if (PlayerPrefs.HasKey(key))
					{
						ulong result = 0uL;
						if (MainManager.steam)
						{
							ulong.TryParse(PlayerPrefs.GetString(key), NumberStyles.Any, CultureInfo.InvariantCulture, out result);
						}
						else
						{
							result = (ulong)PlayerPrefs.GetInt(key);
						}
						WeaponInv weaponInv = FindWeapon(result);
						if (weaponInv != null && weaponInv.wi.slot == j)
						{
							wset[i].w[j] = weaponInv;
						}
					}
				}
			}
		}
		else if (!MainManager.steam)
		{
			MasterClient.cs.send_set();
		}
	}

	public static void CheckNullSet()
	{
		if (wset[0].w[0] == null)
		{
			wset[0].w[0] = FindWeaponByWid(28);
		}
		if (wset[0].w[1] == null)
		{
			wset[0].w[1] = FindWeaponByWid(23);
		}
		if (wset[0].w[2] == null)
		{
			wset[0].w[2] = FindWeaponByWid(68);
		}
		if (wset[0].w[3] == null)
		{
			wset[0].w[3] = FindWeaponByWid(69);
		}
	}

	public static WeaponInv FindWeapon(ulong uid)
	{
		for (int i = 0; i < wlist.Count; i++)
		{
			if (wlist[i].uid == uid)
			{
				return wlist[i];
			}
		}
		return null;
	}

	public static WeaponInv FindWeaponByWid(int wid)
	{
		for (int i = 0; i < wlist.Count; i++)
		{
			if (wlist[i].wi.id == wid)
			{
				return wlist[i];
			}
		}
		return null;
	}

	public static void UpdateWeaponFrags(ulong uid, int frags)
	{
		for (int i = 0; i < wlist.Count; i++)
		{
			if (wlist[i].uid == uid)
			{
				wlist[i].detail_level = null;
				wlist[i].frags = frags;
				break;
			}
		}
	}

	public static void CreateCamera()
	{
		goCamera = new GameObject();
		goCamera.transform.position = Vector3.zero + new Vector3(0f, 0.5f, 0f);
		goCamera.transform.eulerAngles = Vector3.zero;
		goCamera.name = "PreviewCamera_" + goCamera.GetInstanceID();
		csCharCam = goCamera.AddComponent<Camera>();
		csCharCam.nearClipPlane = 0.01f;
		csCharCam.cullingMask = -257;
		csCharCam.fieldOfView = 40f;
		csCharCam.clearFlags = CameraClearFlags.Depth;
		csCharCam.backgroundColor = new Color(0f, 0f, 0f, 0f);
		csCharCam.rect = new Rect(((float)Screen.width / 2f - GUIM.YRES(460f)) / (float)Screen.width, GUIM.YRES(180f) / (float)Screen.height, GUIM.YRES(240f) / (float)Screen.width, GUIM.YRES(400f) / (float)Screen.height);
	}

	private static void DeleteCamera()
	{
		Object.Destroy(goCamera);
	}

	private static void CreateDetail()
	{
		goDetail = new GameObject();
		goDetail.name = "Detail_" + Time.time;
		goDetail.transform.position = new Vector3(9.9f, 0f, 0.6f);
		goCameraDetail = new GameObject();
		goCameraDetail.transform.position = Vector3.zero + new Vector3(10f, 0f, 0f);
		goCameraDetail.transform.eulerAngles = Vector3.zero;
		goCameraDetail.name = "DetailCamera_" + goCameraDetail.GetInstanceID();
		Camera camera = goCameraDetail.AddComponent<Camera>();
		camera.nearClipPlane = 0.01f;
		camera.cullingMask = -257;
		camera.fieldOfView = 40f;
		camera.clearFlags = CameraClearFlags.Depth;
		camera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		camera.rect = new Rect(0.325f, 0.245f, 0.56f, 0.56f);
		if (csCharCam != null)
		{
			csCharCam.enabled = false;
		}
	}

	private static void DeleteDetail()
	{
		Object.Destroy(goCameraDetail);
		Object.Destroy(goDetail);
		if (csCharCam != null)
		{
			csCharCam.enabled = true;
		}
	}

	public static void CreateChar()
	{
		float y = 145f;
		if (goChar != null)
		{
			y = goChar.transform.eulerAngles.y;
		}
		DeleteChar();
		for (int i = 0; i < 10; i++)
		{
			pr.cp[i] = Controll.pl.cp[i];
		}
		pr.team = 2;
		goChar = VCGen.Build(pr);
		goChar.name = "preview_" + goChar.GetInstanceID();
		goChar.transform.position = new Vector3(0f, 1.25f, 10f);
		goChar.transform.eulerAngles = new Vector3(0f, y, 0f);
		goChar.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
		pr.bstate = 2;
		CharAnimator.SetIdleNew(pr);
	}

	public static void DeleteChar()
	{
		if (goChar != null)
		{
			Object.Destroy(goChar);
		}
		PLH.ClearWeapon(pr);
	}

	public static void Load()
	{
		if (wset[0] == null)
		{
			wset[0] = new WeaponSet(0);
			wset[1] = new WeaponSet(1);
			wset[2] = new WeaponSet(2);
		}
		cs = wset[0];
	}

	public static void LoadChar()
	{
		if (PlayerPrefs.HasKey("bp_char" + MasterClient.vk_id))
		{
			PlayerData pl = Controll.pl;
			pl.cp[0] = PlayerPrefs.GetInt("cp0" + MasterClient.vk_id, 0);
			pl.cp[1] = PlayerPrefs.GetInt("cp1" + MasterClient.vk_id, 0);
			pl.cp[2] = PlayerPrefs.GetInt("cp2" + MasterClient.vk_id, 0);
			pl.cp[3] = PlayerPrefs.GetInt("cp3" + MasterClient.vk_id, 1);
			pl.cp[4] = PlayerPrefs.GetInt("cp4" + MasterClient.vk_id, 0);
			pl.cp[5] = PlayerPrefs.GetInt("cp5" + MasterClient.vk_id, 1);
			pl.cp[6] = PlayerPrefs.GetInt("cp6" + MasterClient.vk_id, 0);
			pl.cp[7] = PlayerPrefs.GetInt("cp7" + MasterClient.vk_id, 0);
			pl.cp[8] = PlayerPrefs.GetInt("cp8" + MasterClient.vk_id, 0);
			pl.cp[9] = PlayerPrefs.GetInt("cp9" + MasterClient.vk_id, 0);
			currcolorskin = pl.cp[0];
			currcoloreye = pl.cp[1];
			currcolorhair = pl.cp[2];
			currhair = pl.cp[3];
			curreye = pl.cp[4];
			currbeard = pl.cp[5];
			currhat = pl.cp[6];
			currbody = pl.cp[7];
			currpants = pl.cp[8];
			currboots = pl.cp[9];
			UtilChar.ccskin = UtilChar.colorskin[currcolorskin];
			UtilChar.cceye = UtilChar.coloreye[currcoloreye];
			UtilChar.cchair = UtilChar.colorhair[currcolorhair];
			changed = false;
			Main.tAvatar = FaceGen.Build();
			if (GUIPlay.show)
			{
				GUIPlay.MCreateChar();
			}
		}
		else
		{
			MasterClient.cs.send_char();
		}
	}

	public static WeaponInfo GetWeaponInfo(string weaponname)
	{
		for (int i = 0; i < 1024; i++)
		{
			if (winfo[i] != null && winfo[i].name == weaponname)
			{
				return winfo[i];
			}
		}
		return null;
	}

	public static WeaponInfo GetWeaponInfo(int wid)
	{
		if (winfo[wid] != null)
		{
			return winfo[wid];
		}
		return null;
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("lightgray");
		tYellow = TEX.GetTextureByName("yellow");
		tOrange = TEX.GetTextureByName("orange");
		tRed = TEX.GetTextureByName("red");
		tDublicate = Resources.Load("dublicate") as Texture2D;
		tItem = ContentLoader_.LoadTexture("ak47") as Texture2D;
		vItem = GUIGameSet.CalcSize(tItem);
		InitColors();
		hideicon = ((PlayerPrefs.GetInt("p_hideicon", 0) != 0) ? true : false);
		if (psinfo[1] == null)
		{
			psinfo[1] = new PlayerSkinInfo(1, "Paratrooper", -1, 3, 4, 3, -1, 20);
		}
		if (psinfo[2] == null)
		{
			psinfo[2] = new PlayerSkinInfo(2, "Police", -1, 4, 5, 4, -1, 15);
		}
		if (psinfo[3] == null)
		{
			psinfo[3] = new PlayerSkinInfo(3, "Rebel", 3, -1, 6, 5, 1, 10);
		}
		if (psinfo[4] == null)
		{
			psinfo[4] = new PlayerSkinInfo(4, "Professional", -1, 5, 7, 6, 2, 15);
		}
		if (psinfo[5] == null)
		{
			psinfo[5] = new PlayerSkinInfo(5, "Jacket", -1, 6, 8, 7, -1, 25);
		}
		if (psinfo[6] == null)
		{
			psinfo[6] = new PlayerSkinInfo(6, "Skullcap", -1, 7, 9, 8, -1, 100);
		}
		if (psinfo[7] == null)
		{
			psinfo[7] = new PlayerSkinInfo(7, "Smoking", -1, 8, 10, 9, -1, 15);
		}
		if (psinfo[8] == null)
		{
			psinfo[8] = new PlayerSkinInfo(8, "Sniper", -1, 9, 11, 10, -1, 50);
		}
		if (psinfo[9] == null)
		{
			psinfo[9] = new PlayerSkinInfo(9, "Mummy", -1, 10, 12, 11, -1, 20);
		}
		if (psinfo[10] == null)
		{
			psinfo[10] = new PlayerSkinInfo(10, "Pumpkin", -1, 11, 13, 12, -1, 25);
		}
		if (psinfo[11] == null)
		{
			psinfo[11] = new PlayerSkinInfo(11, "Forester", -1, 13, 14, 13, -1, 25);
		}
		if (psinfo[12] == null)
		{
			psinfo[12] = new PlayerSkinInfo(12, "Winter", -1, 14, 15, 14, -1, 20);
		}
		if (psinfo[13] == null)
		{
			psinfo[13] = new PlayerSkinInfo(13, "Clown", -1, 15, 16, 15, -1, 25);
		}
		if (psinfo[14] == null)
		{
			psinfo[14] = new PlayerSkinInfo(14, "Tankman", -1, 16, 17, 16, -1, 15);
		}
		if (psinfo[15] == null)
		{
			psinfo[15] = new PlayerSkinInfo(15, "Specops", -1, 17, 18, 17, -1, 35);
		}
		if (psinfo[16] == null)
		{
			psinfo[16] = new PlayerSkinInfo(16, "Punk", 4, -1, 19, 18, -1, 25);
		}
		if (psinfo[17] == null)
		{
			psinfo[17] = new PlayerSkinInfo(17, "Dracula", -1, 18, 20, 19, -1, 30);
		}
		if (psinfo[18] == null)
		{
			psinfo[18] = new PlayerSkinInfo(18, "Pirate", -1, 19, 21, 20, -1, 25);
		}
		if (psinfo[19] == null)
		{
			psinfo[19] = new PlayerSkinInfo(19, "Drifter", -1, 20, 22, 21, -1, 30);
		}
		if (psinfo[20] == null)
		{
			psinfo[20] = new PlayerSkinInfo(20, "Elf", -1, 21, 23, 22, -1, 20);
		}
		if (psinfo[21] == null)
		{
			psinfo[21] = new PlayerSkinInfo(21, "Biker", -1, 22, 24, 23, -1, 75);
		}
		if (psinfo[22] == null)
		{
			psinfo[22] = new PlayerSkinInfo(22, "Seaman", -1, 24, 25, 24, -1, 35);
		}
		if (psinfo[23] == null)
		{
			psinfo[23] = new PlayerSkinInfo(23, "Cyborg", -1, 25, 26, 25, -1, 40);
		}
		if (psinfo[24] == null)
		{
			psinfo[24] = new PlayerSkinInfo(24, "Nutcracker", -1, -1, 27, 26, -1, 15);
		}
		if (psinfo[25] == null)
		{
			psinfo[25] = new PlayerSkinInfo(25, "Hazmat", -1, 29, 28, 27, -1, 45);
		}
		if (psinfo[26] == null)
		{
			psinfo[26] = new PlayerSkinInfo(26, "Guerilla", -1, 30, 29, 28, -1, 25);
		}
		tArrow_l = ContentLoader_.LoadTexture("arrow_l") as Texture2D;
		tArrow_r = ContentLoader_.LoadTexture("arrow_r") as Texture2D;
	}

	private void OnResize()
	{
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(400f));
		rBackChar = new Rect(rBack.x, rBack.y, GUIM.YRES(240f), rBack.height);
		rScroll = new Rect(rBack.x + GUIM.YRES(264f), rBack.y + GUIM.YRES(8f), GUIM.YRES(392f), rBack.height - GUIM.YRES(48f));
		rCentral = new Rect(rBack.x + GUIM.YRES(256f), rBack.y, GUIM.YRES(408f), rBack.height - GUIM.YRES(32f));
		rDescription.Set(rBack.x + GUIM.YRES(680f), rBack.y, GUIM.YRES(240f), rBack.height);
		rSet.Set(rBack.x + GUIM.YRES(40f), rBack.y + GUIM.YRES(416f), GUIM.YRES(920f), GUIM.YRES(84f));
		if (csCharCam != null)
		{
			csCharCam.rect = new Rect(rBack.x / (float)Screen.width, GUIM.YRES(180f) / (float)Screen.height, GUIM.YRES(240f) / (float)Screen.width, rBack.height / (float)Screen.height);
		}
	}

	private void OnChangeSubMenu()
	{
		if (show)
		{
			Reset();
			scroll = Vector2.zero;
		}
	}

	private static void Reset()
	{
		if (wd != null && wd.go != null && wd.goWeapon != null)
		{
			wd.goWeapon.transform.parent = wd.go.transform;
			wd.goWeapon.SetActive(false);
			wd.goWeapon.transform.localPosition = Vector3.zero;
			wd.goWeapon.transform.localEulerAngles = Vector3.zero;
			wd.goWeapon.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
		}
		DeleteDetail();
		detail = false;
		if (Main.currSubMenu == 2)
		{
			if (csCharCam != null)
			{
				csCharCam.enabled = false;
			}
		}
		else if (csCharCam != null)
		{
			csCharCam.enabled = true;
		}
		if (GUICase.opencase != null)
		{
			Object.Destroy(GUICase.opencase);
		}
		GUICase.opencase = null;
		GUICase.caselock = false;
		currskin = null;
		currskinid = 0;
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIFX.Begin();
			if (detail)
			{
				DrawDetailWeapon();
			}
			else if (Main.currSubMenu == 0)
			{
				DrawSectionWeapon();
			}
			else if (Main.currSubMenu == 1)
			{
				DrawSectionCharacter();
			}
			else if (Main.currSubMenu == 2)
			{
				GUICase.cs.Draw();
			}
			GUIFX.End();
		}
	}

	private void DrawChar(Rect r)
	{
		GUIM.DrawBox(r, tBlack, 0.05f);
		r.Set(r.x, r.y, r.width, r.width * 2f);
	}

	private void DrawSectionWeapon()
	{
		DrawSet(rSet);
		DrawChar(rBackChar);
		GUIM.DrawBox(rCentral, tBlack, 0.05f);
		bool flag = false;
		scroll = GUIM.BeginScrollView(rScroll, scroll, new Rect(0f, 0f, 0f, GUIM.YRES(68f) * (float)(int)((float)wlist.Count / 3f + 0.5f + 1f)));
		int num = 0;
		for (int i = 0; i < wlist.Count; i++)
		{
			if (hideicon && wlist[i].dublicate == 1)
			{
				continue;
			}
			int num2 = num / 3;
			int num3 = num - num2 * 3;
			if (DrawWeapon(new Rect(GUIM.YRES(128 * num3), GUIM.YRES(68 * num2), GUIM.YRES(120f), GUIM.YRES(60f)), wlist[i]))
			{
				if (whover != wlist[i])
				{
					whover = wlist[i];
					hovertime = Time.time;
				}
				flag = true;
			}
			num++;
		}
		GUIM.EndScrollView();
		if (!flag)
		{
			whover = null;
		}
		DrawDescription(rDescription, cw, whover);
		Rect r = new Rect(rCentral.x, rCentral.y + rCentral.height + GUIM.YRES(4f), GUIM.YRES(36f), GUIM.YRES(28f));
		GUIM.DrawBox(r, tBlack, 0.05f);
		GUIM.DrawText(r, wlist.Count.ToString(), TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, false);
		Rect r2 = new Rect(rCentral.x + rCentral.width - GUIM.YRES(36f), rCentral.y + rCentral.height + GUIM.YRES(4f), GUIM.YRES(36f), GUIM.YRES(28f));
		Rect position = new Rect(rCentral.x + rCentral.width - GUIM.YRES(32f), rCentral.y + rCentral.height + GUIM.YRES(4f), GUIM.YRES(28f), GUIM.YRES(28f));
		GUIM.DrawBox(r2, tBlack, 0.05f);
		if (GUIM.Contains(r2))
		{
			GUI.color = Color.yellow;
		}
		else if (!hideicon)
		{
			GUI.color = Color.gray;
		}
		GUI.DrawTexture(position, tDublicate);
		GUI.color = Color.white;
		if (GUIM.HideButton(r2))
		{
			hideicon = !hideicon;
			PlayerPrefs.SetInt("p_hideicon", hideicon ? 1 : 0);
		}
	}

	public static bool DrawWeapon(Rect r, WeaponInv w, bool smooth = false, bool hidename = false)
	{
		bool result = false;
		if (w == null)
		{
			return result;
		}
		if (GUIM.Contains(r, rScroll))
		{
			GUI.color = new Color(1f, 1f, 1f, 0.75f);
			if (smooth)
			{
				GUIM.DrawBox(r, tYellow);
			}
			else
			{
				GUI.DrawTexture(r, tYellow);
			}
			GUI.color = Color.white;
			result = true;
		}
		else
		{
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			if (smooth)
			{
				GUIM.DrawBox(r, tWhite);
			}
			else
			{
				GUI.DrawTexture(r, tWhite);
			}
			GUI.color = Color.white;
		}
		if (w == cw)
		{
			GUI.color = new Color(0.94f, 0.41f, 0f, 0.85f);
			if (smooth)
			{
				GUIM.DrawBox(r, tOrange);
			}
			else
			{
				GUI.DrawTexture(r, tWhite);
			}
			GUI.color = Color.white;
		}
		WeaponInfo wi = w.wi;
		if (wi.slot == 4)
		{
			int num = (int)GUIM.YRES(14f);
			GUI.DrawTexture(new Rect(r.x + (r.width - r.height) / 2f + (float)num + (r.height - (float)(num * 2)) * 0.1875f, r.y + (float)num + GUIM.YRES(4f), r.height - (float)(num * 2), r.height - (float)(num * 2)), wi.tIcon);
		}
		else
		{
			DrawWeaponIcon(new Rect(r.x, r.y + GUIM.YRES(2f), r.width, r.height), wi.tIcon, wi.vIcon);
		}
		if (!hidename)
		{
			DrawWeaponText(r, w);
		}
		if (show && GUIM.HideButton(r))
		{
			cw = w;
			if (wd != null && wd.goWeapon != null && wd.go != null)
			{
				wd.goWeapon.transform.parent = wd.go.transform;
				wd.goWeapon.SetActive(false);
				wd.goWeapon.transform.localPosition = Vector3.zero;
				wd.goWeapon.transform.localEulerAngles = Vector3.zero;
			}
			PLH.ClearWeapon(pr);
			wd = VWGen.BuildWeaponFPS(pr, w.wi.name, w.wi, w.scopeid);
			PLH.AddWeapon(pr, wd);
			wd = PLH.GetWeapon(pr, w.wi.name);
			if (wd == null)
			{
				return false;
			}
			if (wd.goPW != null)
			{
				wd.goPW.name += Time.time;
				wd.goPW.SetActive(true);
			}
			if (detail)
			{
				wd.goWeapon.transform.parent = goDetail.transform;
				wd.goWeapon.SetActive(true);
				wd.goWeapon.transform.localPosition = Vector3.zero;
				wd.goWeapon.transform.localEulerAngles = Vector3.zero;
				wd.goWeapon.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
			}
			pr.currweapon = wd;
			PLH.UpdateWeaponSpine(pr, true);
			CharAnimator.UpdateBody(pr);
			pr.currweapon = null;
		}
		return result;
	}

	private void DrawDescription(Rect r, WeaponInv ww, WeaponInv ww2)
	{
		GUIM.DrawBox(r, tBlack, 0.05f);
		if (ww == null)
		{
			return;
		}
		if (ww == ww2)
		{
			ww2 = null;
		}
		dif = Time.time - hovertime;
		if ((double)dif < 0.25)
		{
			ww2 = null;
		}
		else
		{
			dif -= 0.25f;
			dif *= 3f;
			if (dif > 1f)
			{
				dif = 1f;
			}
		}
		WeaponInfo wi = ww.wi;
		WeaponInfo weaponInfo = ((ww2 == null) ? null : ww2.wi);
		if (wi == null)
		{
			return;
		}
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(8f), r.width - GUIM.YRES(16f), GUIM.YRES(20f)), wi.fullname, TextAnchor.MiddleLeft, BaseColor.Orange, 1, 12, false);
		if (weaponInfo != null)
		{
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(8f), r.width - GUIM.YRES(16f), GUIM.YRES(20f)), weaponInfo.fullname, TextAnchor.MiddleRight, BaseColor.LightGray2, 1, 12, false);
		}
		float num = r.width - GUIM.YRES(40f);
		float height = num / 2f;
		DrawWeaponIcon(new Rect(r.x + GUIM.YRES(20f), r.y + GUIM.YRES(12f), num, height), wi.tIcon, wi.vIcon);
		if (wi.id != 69 && wi.id != 107 && wi.id != 108 && wi.id != 109 && wi.id != 110)
		{
			float num2 = (float)wi.damage / 100f;
			float num3 = -1f;
			if (weaponInfo != null)
			{
				num3 = (float)weaponInfo.damage / 100f;
			}
			if (wi.id == 16)
			{
				num2 *= 6f;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			if (num3 > 1f)
			{
				num3 = 1f;
			}
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 0f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sDamage, num2, -1, num3, -1);
		}
		if (wi.id != 68 && wi.id != 69 && wi.id != 104 && wi.id != 105 && wi.id != 106 && wi.id != 107 && wi.id != 108 && wi.id != 109 && wi.id != 110 && wi.id != 171)
		{
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 1f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sCage, (float)wi.ammo / 60f, wi.ammo, (weaponInfo == null) ? (-1f) : ((float)weaponInfo.ammo / 60f), (weaponInfo == null) ? (-1) : weaponInfo.ammo);
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 2f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sStock, (float)wi.backpack / 200f, wi.backpack, (weaponInfo == null) ? (-1f) : ((float)weaponInfo.backpack / 200f), (weaponInfo == null) ? (-1) : weaponInfo.backpack);
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 3f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sRateFire, 1f - (float)wi.firerate / 1000f, -1, (weaponInfo == null) ? (-1f) : (1f - (float)weaponInfo.firerate / 1000f), -1);
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 4f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sDistance, (float)wi.distance / 128f, -1, (weaponInfo == null) ? (-1f) : ((float)weaponInfo.distance / 128f), -1);
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 5f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sAccuracy, (float)wi.accuracy / 100f, -1, (weaponInfo == null) ? (-1f) : ((float)weaponInfo.accuracy / 100f), -1);
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 6f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sReload, 1f - (float)wi.reload / 3000f, -1, (weaponInfo == null) ? (-1f) : (1f - (float)weaponInfo.reload / 3000f), -1);
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 7f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sRecoil, (float)wi.recoil / 10f, -1, (weaponInfo == null) ? (-1f) : ((float)weaponInfo.recoil / 10f), -1);
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 8f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sPropert, (float)wi.piercing / 100f, -1, (weaponInfo == null) ? (-1f) : ((float)weaponInfo.piercing / 100f), -1);
			DrawBar(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(100f) + GUIM.YRES(26f) * 9f, r.width - GUIM.YRES(16f), GUIM.YRES(20f)), sMobility, (float)wi.mobility / 160f, -1, (weaponInfo == null) ? (-1f) : ((float)weaponInfo.mobility / 160f), -1);
		}
		int slot = wi.slot;
		rDetail.Set(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(364f), GUIM.YRES(60f), GUIM.YRES(28f));
		if (!detail && GUIM.Button(rDetail, BaseColor.Yellow, sOverview, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			detail = true;
			CreateDetail();
			if (wd != null && wd.goWeapon != null)
			{
				wd.goWeapon.transform.parent = goDetail.transform;
				wd.goWeapon.SetActive(true);
				wd.goWeapon.transform.localPosition = Vector3.zero;
				wd.goWeapon.transform.localEulerAngles = Vector3.zero;
				wd.goWeapon.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
			}
			lasttouch = Time.time;
			if (MainManager.steam)
			{
				MasterClient.cs.send_weaponfrags_steam(ww.uid);
			}
			else
			{
				MasterClient.cs.send_weaponfrags((int)ww.uid);
			}
		}
		if (detail)
		{
			rEquip.Set(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(364f), r.width - GUIM.YRES(16f), GUIM.YRES(28f));
		}
		else
		{
			rEquip.Set(r.x + GUIM.YRES(8f) + GUIM.YRES(64f), r.y + GUIM.YRES(364f), r.width - GUIM.YRES(80f), GUIM.YRES(28f));
		}
		if (cs.w[slot] != ww && GUIM.Button(rEquip, BaseColor.Orange, sWearWeap, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			cs.w[slot] = ww;
			string key = "set_" + cs.idx + "_slot_" + slot + "_uid";
			if (MainManager.steam)
			{
				PlayerPrefs.SetString(key, ww.uid.ToString());
			}
			else
			{
				PlayerPrefs.SetInt(key, (int)ww.uid);
			}
			mastersave = true;
		}
		if (cs.w[slot] == ww && GUIM.Button(rEquip, BaseColor.Block, sRemoveWeap, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			cs.w[slot] = null;
			PlayerPrefs.DeleteKey("set_" + cs.idx + "_slot_" + slot + "_uid");
			mastersave = true;
		}
	}

	private void DrawBar(Rect r, string label, float val, int v, float val2, int v2)
	{
		if (val < 0.01f)
		{
			val = 0.01f;
		}
		if (val > 1f)
		{
			val = 1f;
		}
		GUIM.DrawText(new Rect(r.x, r.y, r.width, GUIM.YRES(20f)), label, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 12, false);
		if (v > 0)
		{
			GUIM.DrawText(new Rect(r.x, r.y, r.width, GUIM.YRES(20f)), v.ToString(), TextAnchor.MiddleRight, BaseColor.LightGray, 1, 12, false);
		}
		float num = (int)GUIM.YRES(3f);
		if (num < 1f)
		{
			num = 1f;
		}
		GUI.color = new Color(1f, 1f, 1f, 0.25f);
		GUI.DrawTexture(new Rect(r.x, r.y + GUIM.YRES(20f), r.width, num), tBlack);
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(r.x, r.y + GUIM.YRES(20f), r.width * val, num), tWhite);
		if (val2 > 1f)
		{
			val2 = 1f;
		}
		if (val2 > 0f && val != val2)
		{
			if (val > val2)
			{
				GUI.color = Color.red;
			}
			else if (val < val2)
			{
				GUI.color = Color.green;
			}
			GUI.DrawTexture(new Rect(r.x, r.y + GUIM.YRES(20f) + num, r.width * val2 * dif, num), tWhite);
			if (v2 < 0)
			{
				GUIM.DrawText(new Rect(r.x, r.y, r.width - GUIM.YRES(32f), GUIM.YRES(20f)), ((val < val2) ? "+" : "") + ((val2 - val) * 100f).ToString("0") + "%", TextAnchor.MiddleRight, BaseColor.LightGray, 1, 12, false);
			}
			else
			{
				GUIM.DrawText(new Rect(r.x, r.y, r.width - GUIM.YRES(32f), GUIM.YRES(20f)), ((v < v2) ? "+" : "") + (v2 - v), TextAnchor.MiddleRight, BaseColor.LightGray, 1, 12, false);
			}
			GUI.color = Color.white;
		}
	}

	private void DrawSet(Rect r)
	{
		Rect r2 = new Rect(r.x + GUIM.YRES((wset[0] != cs) ? 4 : 0), r.y, GUIM.YRES(40f), GUIM.YRES(24f));
		GUIM.DrawBox(r2, (wset[0] == cs) ? tOrange : (GUIM.Contains(r2) ? tYellow : tBlack));
		GUIM.DrawText(r2, sSet1, TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, true);
		if (GUIM.HideButton(r2))
		{
			cs = wset[0];
		}
		Rect r3 = new Rect(r.x + GUIM.YRES((wset[1] != cs) ? 4 : 0), r.y + GUIM.YRES(30f) * 1f, GUIM.YRES(40f), GUIM.YRES(24f));
		GUIM.DrawBox(r3, (wset[1] == cs) ? tOrange : (GUIM.Contains(r3) ? tYellow : tBlack));
		GUIM.DrawText(r3, sSet2, TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, true);
		if (GUIM.HideButton(r3))
		{
			cs = wset[1];
		}
		Rect r4 = new Rect(r.x + GUIM.YRES((wset[2] != cs) ? 4 : 0), r.y + GUIM.YRES(30f) * 2f, GUIM.YRES(40f), GUIM.YRES(24f));
		GUIM.DrawBox(r4, (wset[2] == cs) ? tOrange : (GUIM.Contains(r4) ? tYellow : tBlack));
		GUIM.DrawText(r4, sSet3, TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, true);
		if (GUIM.HideButton(r4))
		{
			cs = wset[2];
		}
		DrawSlot(new Rect(r.x + GUIM.YRES(48f) + GUIM.YRES(152f) * 0f, r.y, GUIM.YRES(144f), GUIM.YRES(84f)), cs.w[0], sPrimWeap);
		DrawSlot(new Rect(r.x + GUIM.YRES(48f) + GUIM.YRES(152f) * 1f, r.y, GUIM.YRES(144f), GUIM.YRES(84f)), cs.w[1], sSecWeap);
		DrawSlot(new Rect(r.x + GUIM.YRES(48f) + GUIM.YRES(152f) * 2f, r.y, GUIM.YRES(144f), GUIM.YRES(84f)), cs.w[2], sMeleeWeap);
		DrawSlot(new Rect(r.x + GUIM.YRES(48f) + GUIM.YRES(152f) * 3f, r.y, GUIM.YRES(144f), GUIM.YRES(84f)), cs.w[3], sBuildBlock);
		DrawSlot(new Rect(r.x + GUIM.YRES(48f) + GUIM.YRES(152f) * 4f, r.y, GUIM.YRES(144f), GUIM.YRES(84f)), cs.w[4], sAddit);
	}

	private void DrawSlot(Rect r, WeaponInv w, string label)
	{
		GUIM.DrawBox(r, tBlack);
		if (w == null)
		{
			GUIM.DrawText(r, label, TextAnchor.MiddleCenter, BaseColor.LightGray, 0, 16, true);
			return;
		}
		float num = 4f;
		DrawWeapon(new Rect(r.x + GUIM.YRES(num), r.y + GUIM.YRES(num), r.width - GUIM.YRES(num * 2f), r.height - GUIM.YRES(num * 2f)), w, true);
	}

	private void Update()
	{
		if (show)
		{
			if (!detail)
			{
				UpdateChar();
			}
			else
			{
				UpdateWeapon();
			}
		}
	}

	private void UpdateChar()
	{
		if (Input.GetMouseButtonDown(0) && GUIM.Contains(rBackChar))
		{
			start_x = Input.mousePosition.x;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			start_x = -1f;
		}
		if (start_x > 0f)
		{
			float x = Input.mousePosition.x;
			float num = x - start_x;
			start_x = x;
			goChar.transform.localEulerAngles = new Vector3(0f, goChar.transform.localEulerAngles.y - num, 0f);
			lasttouch = Time.time;
		}
		if (Time.time > lasttouch + 30f)
		{
			goChar.transform.eulerAngles = new Vector3(0f, goChar.transform.eulerAngles.y - 20f * Time.deltaTime, 0f);
		}
	}

	private void UpdateWeapon()
	{
		if (Input.GetMouseButtonDown(0) && GUIM.Contains(rCentral))
		{
			start_x = Input.mousePosition.x;
			start_y = (float)Screen.height - Input.mousePosition.y;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			start_x = -1f;
			start_y = -1f;
		}
		if (start_x > 0f)
		{
			float x = Input.mousePosition.x;
			float num = x - start_x;
			start_x = x;
			goDetail.transform.Rotate(Vector3.up * num * -1f, Space.World);
			lasttouch = Time.time;
		}
		if (start_y > 0f)
		{
			float num2 = (float)Screen.height - Input.mousePosition.y;
			float num3 = num2 - start_y;
			start_y = num2;
			if (goDetail.transform.eulerAngles.x >= 270f || goDetail.transform.eulerAngles.x <= 90f)
			{
				num3 *= -1f;
			}
			goDetail.transform.Rotate(Vector3.right * num3, Space.World);
			lasttouch = Time.time;
		}
		if (Time.time > lasttouch + 30f)
		{
			goDetail.transform.eulerAngles = new Vector3(0f, goDetail.transform.eulerAngles.y - 20f * Time.deltaTime, 0f);
		}
	}

	public static void DrawWeaponIcon(Rect r, Texture2D tex, Vector2 tc)
	{
		if (!(tex == null))
		{
			float num = GUIM.YRES(16f) + GUIM.YRES(16f) * 20f / tc.x;
			float num2 = tc.x / tc.y;
			float num3 = r.width - num;
			float num4 = r.height - num;
			if (num3 / num2 > num4)
			{
				num3 = num4 * num2;
			}
			float num5 = (r.width - num3) / 2f;
			float num6 = (r.height - num4) / 2f;
			float num7 = tc.y / tc.x * num3;
			float num8 = (num4 - num7) / 2f;
			GUI.DrawTextureWithTexCoords(texCoords: new Rect(0f, 1f - tc.y / (float)tex.height, tc.x / (float)tex.width, tc.y / (float)tex.height), position: new Rect(r.x + num5, r.y + num6 + num8, num3, num7), image: tex);
		}
	}

	public static void DrawWeaponText(Rect r, WeaponInv w)
	{
		Rect r2 = new Rect(r.x + GUIM.YRES(4f), r.y + GUIM.YRES(4f), r.width, r.height);
		if (w.uid > 6 && w.uid < 8)
		{
			GUIM.DrawText(r2, w.wi.fullname + " DEMO", TextAnchor.UpperLeft, BaseColor.Green, 1, 12, false);
		}
		else if (w.dublicate == 1)
		{
			GUIM.DrawText(r2, w.wi.fullname, TextAnchor.UpperLeft, BaseColor.Blue, 1, 12, false);
		}
		else
		{
			GUIM.DrawText(r2, w.wi.fullname, TextAnchor.UpperLeft, BaseColor.Black, 1, 12, false);
		}
	}

	private void InitColors()
	{
		UtilChar.InitColors();
		tHair_org[0] = ContentLoader_.LoadTexture("skin_hair0") as Texture2D;
		tHair_org[1] = ContentLoader_.LoadTexture("skin_hair1") as Texture2D;
		tHair_org[2] = ContentLoader_.LoadTexture("skin_hair2") as Texture2D;
		tHair_org[3] = ContentLoader_.LoadTexture("skin_hair3_0") as Texture2D;
		tHair_org[4] = ContentLoader_.LoadTexture("skin_hair4") as Texture2D;
		tHair_org[5] = Resources.Load("Textures/skins/skin_hair5") as Texture2D;
		tHair_org[6] = Resources.Load("Textures/skins/skin_hair6") as Texture2D;
		tHair_org[7] = Resources.Load("Textures/skins/skin_hair7") as Texture2D;
		tHair_org[8] = Resources.Load("Textures/skins/skin_hair8") as Texture2D;
		tEye_org[0] = ContentLoader_.LoadTexture("skin_eye0") as Texture2D;
		tEye_org[1] = ContentLoader_.LoadTexture("skin_eye1") as Texture2D;
		tEye_org[2] = ContentLoader_.LoadTexture("skin_eye2") as Texture2D;
		tEye_org[3] = Resources.Load("Textures/skins/skin_eye3") as Texture2D;
		tEye_org[4] = Resources.Load("Textures/skins/skin_eye4") as Texture2D;
		tEye_org[5] = Resources.Load("Textures/skins/skin_eye5") as Texture2D;
		tEye_org[6] = Resources.Load("Textures/skins/skin_eye6") as Texture2D;
		tBeard_org[0] = ContentLoader_.LoadTexture("skin_beard0") as Texture2D;
		tBeard_org[1] = ContentLoader_.LoadTexture("skin_beard1") as Texture2D;
		tBeard_org[2] = ContentLoader_.LoadTexture("skin_beard2") as Texture2D;
		tBeard_org[3] = ContentLoader_.LoadTexture("skin_beard3") as Texture2D;
		tBeard_org[4] = Resources.Load("Textures/skins/skin_beard4") as Texture2D;
		tBeard_org[5] = Resources.Load("Textures/skins/skin_beard5") as Texture2D;
		tBeard_org[6] = Resources.Load("Textures/skins/skin_beard6") as Texture2D;
		tBeard_org[7] = Resources.Load("Textures/skins/skin_beard7") as Texture2D;
		tHat_org[0] = tBeard_org[0];
		tHat_org[1] = ContentLoader_.LoadTexture("skin_hat1") as Texture2D;
		tHat_org[2] = ContentLoader_.LoadTexture("skin_hat2_0") as Texture2D;
		tHat_org[3] = ContentLoader_.LoadTexture("skin_hat3_0") as Texture2D;
		tHat_org[4] = ContentLoader_.LoadTexture("skin_hat4") as Texture2D;
		tHat_org[5] = ContentLoader_.LoadTexture("skin_hat5") as Texture2D;
		tHat_org[6] = ContentLoader_.LoadTexture("skin_hat6") as Texture2D;
		tHat_org[7] = ContentLoader_.LoadTexture("skin_hat7") as Texture2D;
		tHat_org[8] = ContentLoader_.LoadTexture("skin_hat8") as Texture2D;
		tHat_org[9] = ContentLoader_.LoadTexture("skin_hat9_0") as Texture2D;
		tHat_org[10] = ContentLoader_.LoadTexture("skin_hat10") as Texture2D;
		tHat_org[11] = ContentLoader_.LoadTexture("skin_hat11") as Texture2D;
		tHat_org[12] = ContentLoader_.LoadTexture("skin_hat12_0") as Texture2D;
		tHat_org[13] = ContentLoader_.LoadTexture("skin_hat13") as Texture2D;
		tHat_org[14] = ContentLoader_.LoadTexture("skin_hat14") as Texture2D;
		tHat_org[15] = ContentLoader_.LoadTexture("skin_hat15") as Texture2D;
		tHat_org[16] = ContentLoader_.LoadTexture("skin_hat16") as Texture2D;
		tHat_org[17] = ContentLoader_.LoadTexture("skin_hat17") as Texture2D;
		tHat_org[18] = ContentLoader_.LoadTexture("skin_hat18") as Texture2D;
		tHat_org[19] = ContentLoader_.LoadTexture("skin_hat19_1") as Texture2D;
		tHat_org[20] = ContentLoader_.LoadTexture("skin_hat20_1") as Texture2D;
		tHat_org[21] = ContentLoader_.LoadTexture("skin_hat21_1") as Texture2D;
		tHat_org[22] = ContentLoader_.LoadTexture("skin_hat22_1") as Texture2D;
		tHat_org[23] = ContentLoader_.LoadTexture("skin_hat23_1") as Texture2D;
		tHat_org[24] = ContentLoader_.LoadTexture("skin_hat24_1") as Texture2D;
		tHat_org[25] = ContentLoader_.LoadTexture("skin_hat25") as Texture2D;
		tHat_org[26] = ContentLoader_.LoadTexture("skin_hat26") as Texture2D;
		tHat_org[27] = ContentLoader_.LoadTexture("skin_hat27") as Texture2D;
		tHat_org[28] = ContentLoader_.LoadTexture("skin_hat28_1") as Texture2D;
		tHat_org[29] = Resources.Load("Textures/skins/skin_hat29_0") as Texture2D;
		tHat_org[30] = Resources.Load("Textures/skins/skin_hat30") as Texture2D;
		tBody_org[0] = ContentLoader_.LoadTexture("skin_body0_1") as Texture2D;
		tBody_org[1] = ContentLoader_.LoadTexture("skin_body1_1") as Texture2D;
		tBody_org[2] = ContentLoader_.LoadTexture("skin_body2_1") as Texture2D;
		tBody_org[3] = ContentLoader_.LoadTexture("skin_body3_1") as Texture2D;
		tBody_org[4] = ContentLoader_.LoadTexture("skin_body4_1") as Texture2D;
		tBody_org[5] = ContentLoader_.LoadTexture("skin_body5_1") as Texture2D;
		tBody_org[6] = ContentLoader_.LoadTexture("skin_body6_0") as Texture2D;
		tBody_org[7] = ContentLoader_.LoadTexture("skin_body7_1") as Texture2D;
		tBody_org[8] = ContentLoader_.LoadTexture("skin_body8_1") as Texture2D;
		tBody_org[9] = ContentLoader_.LoadTexture("skin_body9_1") as Texture2D;
		tBody_org[10] = ContentLoader_.LoadTexture("skin_body10_1") as Texture2D;
		tBody_org[11] = ContentLoader_.LoadTexture("skin_body11_1") as Texture2D;
		tBody_org[12] = ContentLoader_.LoadTexture("skin_body12_0") as Texture2D;
		tBody_org[13] = ContentLoader_.LoadTexture("skin_body13_0") as Texture2D;
		tBody_org[14] = ContentLoader_.LoadTexture("skin_body14_0") as Texture2D;
		tBody_org[15] = ContentLoader_.LoadTexture("skin_body15_0") as Texture2D;
		tBody_org[16] = ContentLoader_.LoadTexture("skin_body16_0") as Texture2D;
		tBody_org[17] = ContentLoader_.LoadTexture("skin_body17_0") as Texture2D;
		tBody_org[18] = ContentLoader_.LoadTexture("skin_body18_0") as Texture2D;
		tBody_org[19] = ContentLoader_.LoadTexture("skin_body19_0") as Texture2D;
		tBody_org[20] = ContentLoader_.LoadTexture("skin_body20_1") as Texture2D;
		tBody_org[21] = ContentLoader_.LoadTexture("skin_body21_1") as Texture2D;
		tBody_org[22] = ContentLoader_.LoadTexture("skin_body22_1") as Texture2D;
		tBody_org[23] = ContentLoader_.LoadTexture("skin_body23_1") as Texture2D;
		tBody_org[24] = ContentLoader_.LoadTexture("skin_body24_1") as Texture2D;
		tBody_org[25] = ContentLoader_.LoadTexture("skin_body25_1") as Texture2D;
		tBody_org[26] = ContentLoader_.LoadTexture("skin_body26_0") as Texture2D;
		tBody_org[27] = ContentLoader_.LoadTexture("skin_body27_0") as Texture2D;
		tBody_org[28] = Resources.Load("Textures/skins/skin_body28_0") as Texture2D;
		tBody_org[29] = Resources.Load("Textures/skins/skin_body29_0") as Texture2D;
		tPants_org[0] = ContentLoader_.LoadTexture("skin_pants0") as Texture2D;
		tPants_org[1] = ContentLoader_.LoadTexture("skin_pants1") as Texture2D;
		tPants_org[2] = ContentLoader_.LoadTexture("skin_pants2_0") as Texture2D;
		tPants_org[3] = ContentLoader_.LoadTexture("skin_pants3") as Texture2D;
		tPants_org[4] = ContentLoader_.LoadTexture("skin_pants4_1") as Texture2D;
		tPants_org[5] = ContentLoader_.LoadTexture("skin_pants5") as Texture2D;
		tPants_org[6] = ContentLoader_.LoadTexture("skin_pants6") as Texture2D;
		tPants_org[7] = ContentLoader_.LoadTexture("skin_pants7") as Texture2D;
		tPants_org[8] = ContentLoader_.LoadTexture("skin_pants8") as Texture2D;
		tPants_org[9] = ContentLoader_.LoadTexture("skin_pants9_1") as Texture2D;
		tPants_org[10] = ContentLoader_.LoadTexture("skin_pants10_1") as Texture2D;
		tPants_org[11] = ContentLoader_.LoadTexture("skin_pants11") as Texture2D;
		tPants_org[12] = ContentLoader_.LoadTexture("skin_pants12_0") as Texture2D;
		tPants_org[13] = ContentLoader_.LoadTexture("skin_pants13") as Texture2D;
		tPants_org[14] = ContentLoader_.LoadTexture("skin_pants14") as Texture2D;
		tPants_org[15] = ContentLoader_.LoadTexture("skin_pants15_0") as Texture2D;
		tPants_org[16] = ContentLoader_.LoadTexture("skin_pants16_0") as Texture2D;
		tPants_org[17] = ContentLoader_.LoadTexture("skin_pants17") as Texture2D;
		tPants_org[18] = ContentLoader_.LoadTexture("skin_pants18") as Texture2D;
		tPants_org[19] = ContentLoader_.LoadTexture("skin_pants19_1") as Texture2D;
		tPants_org[20] = ContentLoader_.LoadTexture("skin_pants20_1") as Texture2D;
		tPants_org[21] = ContentLoader_.LoadTexture("skin_pants21_1") as Texture2D;
		tPants_org[22] = ContentLoader_.LoadTexture("skin_pants22_1") as Texture2D;
		tPants_org[23] = ContentLoader_.LoadTexture("skin_pants23") as Texture2D;
		tPants_org[24] = ContentLoader_.LoadTexture("skin_pants24") as Texture2D;
		tPants_org[25] = ContentLoader_.LoadTexture("skin_pants25_0") as Texture2D;
		tPants_org[26] = ContentLoader_.LoadTexture("skin_pants26_0") as Texture2D;
		tPants_org[27] = Resources.Load("Textures/skins/skin_pants27_0") as Texture2D;
		tPants_org[28] = Resources.Load("Textures/skins/skin_pants28") as Texture2D;
		tBoots_org[0] = ContentLoader_.LoadTexture("skin_boots0") as Texture2D;
		tBoots_org[1] = ContentLoader_.LoadTexture("skin_boots1_0") as Texture2D;
		tBoots_org[2] = ContentLoader_.LoadTexture("skin_boots2") as Texture2D;
		GenerateIcons();
	}

	private void GenerateIcons()
	{
		for (int i = 0; i < tHair.Length; i++)
		{
			tHair[i] = GUIIcon.CreateIcon(tHair_org[i], UtilChar.ccskin, UtilChar.cchair);
		}
		for (int j = 0; j < tEye.Length; j++)
		{
			tEye[j] = GUIIcon.CreateIcon(tEye_org[j], UtilChar.ccskin, UtilChar.cceye);
		}
		for (int k = 0; k < tBeard.Length; k++)
		{
			tBeard[k] = GUIIcon.CreateIcon(tBeard_org[k], UtilChar.ccskin, UtilChar.cchair);
		}
		for (int l = 0; l < tHat.Length; l++)
		{
			tHat[l] = GUIIcon.CreateIcon(tHat_org[l], UtilChar.ccskin, colordev);
		}
		for (int m = 0; m < tBody.Length; m++)
		{
			tBody[m] = GUIIcon.CreateIconBody(tBody_org[m], UtilChar.ccskin);
		}
		for (int n = 0; n < tPants.Length; n++)
		{
			tPants[n] = GUIIcon.CreateIconPants(tPants_org[n], UtilChar.ccskin);
		}
		for (int num = 0; num < tBoots.Length; num++)
		{
			tBoots[num] = GUIIcon.CreateIconBoots(tBoots_org[num], UtilChar.ccskin);
		}
	}

	private void DrawSectionCharacter()
	{
		DrawSet(rSet);
		DrawChar(rBackChar);
		Rect viewzone = new Rect(rBack.x + GUIM.YRES(256f), rBack.y, GUIM.YRES(648f), rBack.height);
		scroll = GUIM.BeginScrollView(viewzone, scroll, new Rect(0f, 0f, 0f, GUIM.YRES(626f)));
		bool flag = false;
		int page = 0;
		if (DrawColor(new Rect(0f, 0f + GUIM.YRES(36f) * 0f, viewzone.width, GUIM.YRES(32f)), sColorSkin, UtilChar.colorskin, ref UtilChar.ccskin))
		{
			flag = true;
		}
		if (DrawColor(new Rect(0f, 0f + GUIM.YRES(36f) * 1f, viewzone.width, GUIM.YRES(32f)), sColorEye, UtilChar.coloreye, ref UtilChar.cceye))
		{
			flag = true;
		}
		if (DrawColor(new Rect(0f, 0f + GUIM.YRES(36f) * 2f, viewzone.width, GUIM.YRES(32f)), sColorHair, UtilChar.colorhair, ref UtilChar.cchair))
		{
			flag = true;
		}
		if (DrawItems(new Rect(0f, 0f + GUIM.YRES(36f) * 3f + GUIM.YRES(74f) * 0f, viewzone.width, GUIM.YRES(70f)), sHairstyle, tHair, ref currhair, ref HairPage))
		{
			flag = true;
		}
		if (DrawItems(new Rect(0f, 0f + GUIM.YRES(36f) * 3f + GUIM.YRES(74f) * 1f, viewzone.width, GUIM.YRES(70f)), sEyeType, tEye, ref curreye, ref page))
		{
			flag = true;
		}
		if (DrawItems(new Rect(0f, 0f + GUIM.YRES(36f) * 3f + GUIM.YRES(74f) * 2f, viewzone.width, GUIM.YRES(70f)), sBeard, tBeard, ref currbeard, ref page))
		{
			flag = true;
		}
		if (DrawItems(new Rect(0f, 0f + GUIM.YRES(36f) * 3f + GUIM.YRES(74f) * 3f, viewzone.width, GUIM.YRES(70f)), sHat, tHat, ref currhat, ref HatPage))
		{
			flag = true;
		}
		if (DrawItems(new Rect(0f, 0f + GUIM.YRES(36f) * 3f + GUIM.YRES(74f) * 4f, viewzone.width, GUIM.YRES(70f)), sBody, tBody, ref currbody, ref BodyPage))
		{
			flag = true;
		}
		if (DrawItems(new Rect(0f, 0f + GUIM.YRES(36f) * 3f + GUIM.YRES(74f) * 5f, viewzone.width, GUIM.YRES(70f)), sPants, tPants, ref currpants, ref PantsPage))
		{
			flag = true;
		}
		if (DrawItems(new Rect(0f, 0f + GUIM.YRES(36f) * 3f + GUIM.YRES(74f) * 6f, viewzone.width, GUIM.YRES(70f)), sBoots, tBoots, ref currboots, ref page))
		{
			flag = true;
		}
		GUIM.EndScrollView();
		Rect rect = new Rect(rBackChar.x + GUIM.YRES(16f), rBackChar.y + GUIM.YRES(172f), GUIM.YRES(32f), GUIM.YRES(32f));
		Rect rect2 = new Rect(rBackChar.x + rBackChar.width - GUIM.YRES(48f), rBackChar.y + GUIM.YRES(172f), GUIM.YRES(32f), GUIM.YRES(32f));
		if (GUIM.Contains(rect))
		{
			GUI.color = Color.yellow;
		}
		GUI.DrawTexture(rect, tArrow_l);
		GUI.color = Color.white;
		if (GUIM.Contains(rect2))
		{
			GUI.color = Color.yellow;
		}
		GUI.DrawTexture(rect2, tArrow_r);
		GUI.color = Color.white;
		bool flag2 = false;
		if (GUIM.HideButton(rect) && pslist.Count > 0)
		{
			preset--;
			if (preset < 0)
			{
				preset = 0;
			}
			flag2 = true;
		}
		if (GUIM.HideButton(rect2) && pslist.Count > 0)
		{
			preset++;
			if (preset >= pslist.Count)
			{
				preset = pslist.Count - 1;
			}
			flag2 = true;
		}
		if (flag2)
		{
			PlayerSkinInv playerSkinInv = pslist[preset];
			if (playerSkinInv.psi.hairid > 0)
			{
				currhair = playerSkinInv.psi.hairid;
			}
			else
			{
				currhair = 0;
			}
			if (playerSkinInv.psi.hatid > 0)
			{
				currhat = playerSkinInv.psi.hatid;
			}
			else
			{
				currhat = 0;
			}
			if (playerSkinInv.psi.bodyid > 0)
			{
				currbody = playerSkinInv.psi.bodyid;
			}
			else
			{
				currbody = 0;
			}
			if (playerSkinInv.psi.pantsid > 0)
			{
				currpants = playerSkinInv.psi.pantsid;
			}
			else
			{
				currpants = 0;
			}
			if (playerSkinInv.psi.bootsid > 0)
			{
				currboots = playerSkinInv.psi.bootsid;
			}
			else
			{
				currboots = 0;
			}
			flag = true;
		}
		if (flag)
		{
			GenerateIcons();
			currcolorskin = UtilChar.ccskin.index;
			currcoloreye = UtilChar.cceye.index;
			currcolorhair = UtilChar.cchair.index;
			Controll.pl.cp[0] = currcolorskin;
			Controll.pl.cp[1] = currcoloreye;
			Controll.pl.cp[2] = currcolorhair;
			Controll.pl.cp[3] = currhair;
			Controll.pl.cp[4] = curreye;
			Controll.pl.cp[5] = currbeard;
			Controll.pl.cp[6] = currhat;
			Controll.pl.cp[7] = currbody;
			Controll.pl.cp[8] = currpants;
			Controll.pl.cp[9] = currboots;
			CreateChar();
			changed = true;
		}
	}

	private bool DrawColor(Rect r, string label, CharColor[] cc, ref CharColor ccout)
	{
		bool result = false;
		GUIM.DrawBox(r, tBlack, 0.05f);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y, r.width, r.height), label, TextAnchor.MiddleLeft, BaseColor.White, 1, 12, false);
		int num = (int)GUIM.YRES(2f);
		for (int i = 0; i < cc.Length; i++)
		{
			Rect r2 = new Rect(r.x + GUIM.YRES(120f) + GUIM.YRES(24f) * (float)i, r.y + GUIM.YRES(8f), GUIM.YRES(16f), GUIM.YRES(16f));
			if (GUIM.Contains(r2))
			{
				GUI.DrawTexture(new Rect(r2.x, r2.y + r2.height + (float)(num * 2), r2.width, num), tYellow);
			}
			if (cc[i] == ccout)
			{
				GUI.DrawTexture(new Rect(r2.x - (float)num, r2.y - (float)num, r2.width + (float)(num * 2), r2.height + (float)(num * 2)), tBlack);
			}
			DrawColorPick(r2, cc[i].a);
			if (GUIM.HideButton(r2))
			{
				ccout = cc[i];
				result = true;
			}
		}
		return result;
	}

	private void DrawColorPick(Rect r, Color c)
	{
		GUI.color = c;
		GUI.DrawTexture(r, tWhite);
		GUI.color = Color.white;
	}

	private bool DrawItems(Rect r, string label, Texture2D[] tt, ref int iid, ref int page)
	{
		if (tt == null)
		{
			return false;
		}
		if (hairlock == null)
		{
			return false;
		}
		bool result = false;
		GUIM.DrawBox(r, tBlack, 0.05f);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(8f), r.width, GUIM.YRES(16f)), label, TextAnchor.UpperLeft, BaseColor.White, 1, 12, false);
		int num = (int)GUIM.YRES(2f);
		int num7 = (int)((float)num / 2f);
		int num8 = 1;
		if (tt == null)
		{
			return result;
		}
		int num2 = 0;
		for (int i = 0; i < tt.Length; i++)
		{
			if (!(tt[i] == null) && (tt != tHair || hairlock[i] != 1) && (tt != tHat || hatlock[i] != 1) && (tt != tBody || bodylock[i] != 1) && (tt != tPants || pantslock[i] != 1) && (tt != tBoots || bootslock[i] != 1))
			{
				num2++;
			}
		}
		int num3 = 0;
		if (num2 > 8)
		{
			num3 = page * 8;
		}
		int num4 = 0;
		int num5 = 0;
		for (int j = 0; j < tt.Length; j++)
		{
			if (tt[j] == null || (tt == tHair && hairlock[j] == 1) || (tt == tHat && hatlock[j] == 1) || (tt == tBody && bodylock[j] == 1) || (tt == tPants && pantslock[j] == 1) || (tt == tBoots && bootslock[j] == 1))
			{
				continue;
			}
			num5++;
			Rect r2 = new Rect(r.x + GUIM.YRES(120f) + GUIM.YRES(56f) * (float)num4, r.y + GUIM.YRES(13f), GUIM.YRES(44f), GUIM.YRES(44f));
			if (num5 > num3 && num4 < 8)
			{
				if (GUIM.Contains(r2))
				{
					GUI.DrawTexture(new Rect(r2.x, r2.y + r2.height + (float)(num * 2), r2.width, num), tYellow);
				}
				if (j == iid)
				{
					GUI.DrawTexture(new Rect(r2.x, r2.y, r2.width, r2.height), tBlack);
				}
				GUI.DrawTexture(new Rect(r2.x, r2.y, r2.width, r2.height), tt[j]);
				if (GUIM.HideButton(r2))
				{
					iid = j;
					result = true;
				}
				num4++;
			}
		}
		if (num2 > 8)
		{
			int num6 = (num2 + 8) / 8;
			Rect rect = new Rect(r.x + GUIM.YRES(72f), r.y + GUIM.YRES(19f), GUIM.YRES(32f), GUIM.YRES(32f));
			if (GUIM.Contains(rect))
			{
				GUI.color = Color.yellow;
			}
			GUI.DrawTexture(rect, tArrow_l);
			GUI.color = Color.white;
			if (GUIM.HideButton(rect))
			{
				page--;
				if (page < 0)
				{
					page = 0;
				}
			}
			Rect rect2 = new Rect(r.x + r.width - GUIM.YRES(72f), r.y + GUIM.YRES(19f), GUIM.YRES(32f), GUIM.YRES(32f));
			if (GUIM.Contains(rect2))
			{
				GUI.color = Color.yellow;
			}
			GUI.DrawTexture(rect2, tArrow_r);
			GUI.color = Color.white;
			if (GUIM.HideButton(rect2))
			{
				page++;
				if (page >= num6)
				{
					page = num6 - 1;
				}
			}
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(20f), r.width, GUIM.YRES(16f)), page + 1 + "/" + num6, TextAnchor.UpperLeft, BaseColor.LightGray, 1, 12, false);
		}
		return result;
	}

	private void DrawDetailWeapon()
	{
		if (GUIM.Button(new Rect(rDescription.x, rDescription.y + GUIM.YRES(364f), rDescription.width, GUIM.YRES(28f)), BaseColor.White, sReturn, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Reset();
		}
		if (wd == null)
		{
			return;
		}
		DrawSet(rSet);
		DrawDescription(rBackChar, cw, null);
		rDetailHeader.Set(rBack.x, rBack.y, rBack.width, GUIM.YRES(20f));
		rDetailHeader2.Set(rBack.x, rBack.y + GUIM.YRES(20f), rBack.width, GUIM.YRES(20f));
		rDetailHeader3.Set(rBack.x, rBack.y + GUIM.YRES(37f), rBack.width, GUIM.YRES(20f));
		rDetailHeader4.Set(rBack.x, rBack.y + rBack.height - GUIM.YRES(32f), rBack.width, GUIM.YRES(20f));
		GUIM.DrawText(rDetailHeader, cw.wi.fullname, TextAnchor.MiddleCenter, BaseColor.White, 1, 20, true);
		if (cw.wi.id != 68 && cw.wi.id != 69 && cw.wi.id != 104 && cw.wi.id != 105 && cw.wi.id != 106 && cw.wi.id != 107 && cw.wi.id != 108 && cw.wi.id != 109 && cw.wi.id != 110 && cw.wi.id != 171)
		{
			if (cw.detail_level == null)
			{
				cw.detail_level = sBasicLvl + cw.GetLevel();
				cw.detail_progress = sFrags + " " + cw.frags + " (" + sProgress + " " + (cw.frags - (cw.GetLevel() - 1) * 100).ToString() + "%)";
			}
			GUIM.DrawText(rDetailHeader2, cw.detail_level, TextAnchor.MiddleCenter, BaseColor.LightGray2, 1, 16, true);
			GUIM.DrawText(rDetailHeader3, cw.detail_progress, TextAnchor.MiddleCenter, BaseColor.Yellow, 1, 14, true);
		}
		if (wd.scope != null)
		{
			Rect r = new Rect(rDescription.x, rDescription.y, rDescription.width, GUIM.YRES(40f));
			DrawAttach(r, 0, sOutSight, "", "", 0, 0);
			r.y += GUIM.YRES(44f);
			int level = cw.GetLevel();
			ScopeData scopeData = null;
			for (int i = 0; i < wd.scope.Length; i++)
			{
				scopeData = wd.scope[i];
				DrawAttach(r, scopeData.id, scopeData.fullname, scopeData.name_, scopeData.slevel, scopeData.level, level);
				r.y += GUIM.YRES(44f);
			}
		}
	}

	private void DrawAttach(Rect r, int id, string label, string _name, string level, int req_level, int cur_level)
	{
		if (cur_level < req_level)
		{
			GUIM.DrawBox(r, tRed, 0.05f);
		}
		else if (GUIM.Contains(r))
		{
			GUIM.DrawBox(r, tYellow, 0.05f);
		}
		else
		{
			GUIM.DrawBox(r, tBlack, 0.05f);
		}
		rAttachText.Set(r.x + GUIM.YRES(8f), r.y - GUIM.YRES(2f), r.width, r.height);
		GUIM.DrawText(rAttachText, label, TextAnchor.MiddleLeft, BaseColor.White, 1, 16, false);
		Texture2D texture2D = Resources.Load("scopes/icon_" + _name) as Texture2D;
		if (texture2D == null)
		{
			texture2D = ContentLoader_.LoadTexture("icon_" + _name) as Texture2D;
		}
		Vector2 tc = GUIGameSet.CalcSize(texture2D);
		rIcon.Set(r.x + GUIM.YRES(60f), r.y - GUIM.YRES(2f), GUIM.YRES(180f), r.height);
		DrawAttachIcon(rIcon, texture2D, tc);
		rAttachLevel.Set(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(26f), r.width, GUIM.YRES(16f));
		GUIM.DrawText(rAttachLevel, level, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 14, false);
		if (GUIM.HideButton(r) && cur_level >= req_level && wd != null)
		{
			if (wd.goScope != null)
			{
				Object.Destroy(wd.goScope);
			}
			ScopeGen.BuildScope(id, wd);
			cw.scopeid = id;
			PlayerPrefs.SetInt("wi_" + cw.uid + "_scopeid", id);
			mastersave = true;
		}
	}

	public static void DrawAttachIcon(Rect r, Texture2D tex, Vector2 tc)
	{
		if (!(tex == null))
		{
			float num = GUIM.YRES(6f);
			float num2 = tc.x / tc.y;
			float num3 = r.width - num;
			float num4 = r.height - num;
			if (num3 / num2 > num4)
			{
				num3 = num4 * num2;
			}
			float num5 = (r.width - num3) / 2f;
			float num6 = (r.height - num4) / 2f;
			float num7 = tc.y / tc.x * num3;
			float num8 = (num4 - num7) / 2f;
			GUI.DrawTextureWithTexCoords(texCoords: new Rect(0f, 1f - tc.y / (float)tex.height, tc.x / (float)tex.width, tc.y / (float)tex.height), position: new Rect(r.x + num5, r.y + num6 + num8, num3, num7), image: tex);
		}
	}
}
