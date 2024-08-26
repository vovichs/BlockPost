using System.Collections.Generic;
using GameClass;
using UnityEngine;

public class GUIMap : MonoBehaviour
{
	public static bool show = false;

	public static GUIMap cs = null;

	private Texture2D tblack;

	private static Rect rBack;

	public static int state = 0;

	public static string mapname = "";

	private static bool inEdit = false;

	private static string sNewMap = "НОВАЯ КАРТА";

	public static int mapsize = 0;

	private static int mapheight = 0;

	public static int[] arrmapsize = new int[6] { 4, 6, 8, 16, 32, 64 };

	private static int[] arrmapheight = new int[3] { 2, 4, 8 };

	private static bool inLoad = false;

	private string[] sLoading = new string[3];

	public static string owner_id = "";

	public static string delete_did = "";

	private string sNameMap;

	private string sSizeMap;

	private string sHeightMap;

	private string sCreate;

	private string sLoad;

	public static List<MapData> maplist = new List<MapData>();

	public static MapData currmap = null;

	private static Vector2 sv = Vector2.zero;

	public void LoadLang()
	{
		sNewMap = Lang.GetString("_NEW_MAP");
		sLoading[0] = Lang.GetString("_IS_LOADING") + ".";
		sLoading[1] = Lang.GetString("_IS_LOADING") + "..";
		sLoading[2] = Lang.GetString("_IS_LOADING") + "...";
		sNameMap = Lang.GetString("_NAME_MAP");
		sSizeMap = Lang.GetString("_SIZE_MAP");
		sHeightMap = Lang.GetString("_HEIGHT_MAP");
		sCreate = Lang.GetString("_CREATE");
		sLoad = Lang.GetString("_LOAD");
	}

	public static void SetActive(bool val)
	{
		show = val;
		state = 0;
		inEdit = false;
		inLoad = false;
	}

	private void Awake()
	{
		cs = this;
	}

	private void LoadEnd()
	{
		tblack = TEX.GetTextureByName("black");
	}

	public void DrawState1(Rect rBack)
	{
		int num = (int)GUIM.YRES(40f);
		int num2 = (int)GUIM.YRES(66f);
		float num3 = rBack.x + GUIM.YRES(200f);
		GUIM.DrawText(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 2f, GUIM.YRES(200f), GUIM.YRES(32f)), sNameMap, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, false);
		if (!inEdit)
		{
			GUI.DrawTexture(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), tblack);
			string text = mapname;
			if (text == "")
			{
				text = sNewMap;
			}
			GUIM.DrawText(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), text, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
			if (GUIM.HideButton(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f))))
			{
				inEdit = true;
			}
		}
		else
		{
			GUI.DrawTexture(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), tblack);
			GUIM.DrawEdit(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), ref mapname, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		if (Input.GetMouseButtonDown(0) && !GUIM.Contains(new Rect(rBack.x + GUIM.YRES(200f), rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f))))
		{
			inEdit = false;
		}
		GUIM.DrawText(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 4f, GUIM.YRES(200f), GUIM.YRES(32f)), sSizeMap, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, false);
		if (GUIM.Button(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 5f, GUIM.YRES(38f), GUIM.YRES(32f)), (mapsize == 0) ? BaseColor.Green : BaseColor.LightGray, "64", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			mapsize = 0;
		}
		if (GUIM.Button(new Rect(num3 + (float)num, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 5f, GUIM.YRES(38f), GUIM.YRES(32f)), (1 == mapsize) ? BaseColor.Green : BaseColor.LightGray, "96", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			mapsize = 1;
		}
		if (GUIM.Button(new Rect(num3 + (float)(num * 2), rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 5f, GUIM.YRES(38f), GUIM.YRES(32f)), (2 == mapsize) ? BaseColor.Green : BaseColor.LightGray, "128", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			mapsize = 2;
		}
		if (GUIM.Button(new Rect(num3 + (float)(num * 3), rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 5f, GUIM.YRES(38f), GUIM.YRES(32f)), (3 == mapsize) ? BaseColor.Green : BaseColor.LightGray, "256", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			mapsize = 3;
		}
		GUIM.Button(new Rect(num3 + (float)(num * 4), rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 5f, GUIM.YRES(38f), GUIM.YRES(32f)), BaseColor.Block, "512", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		GUIM.DrawText(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 6f, GUIM.YRES(200f), GUIM.YRES(32f)), sHeightMap, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, false);
		if (GUIM.Button(new Rect(num3, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 7f, (int)GUIM.YRES(64f), GUIM.YRES(32f)), (mapheight == 0) ? BaseColor.Green : BaseColor.LightGray, "32", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			mapheight = 0;
		}
		if (GUIM.Button(new Rect(num3 + (float)num2, rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 7f, (int)GUIM.YRES(64f), GUIM.YRES(32f)), (1 == mapheight) ? BaseColor.Green : BaseColor.LightGray, "64", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			mapheight = 1;
		}
		if (GUIM.Button(new Rect(num3 + (float)(num2 * 2), rBack.y + GUIM.YRES(48f) + GUIM.YRES(36f) * 7f, (int)GUIM.YRES(64f), GUIM.YRES(32f)), (2 == mapheight) ? BaseColor.Green : BaseColor.LightGray, "128", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			mapheight = 2;
		}
		if (GUIM.Button(new Rect(num3, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 8f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.Yellow, sCreate, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			if (mapname == "")
			{
				mapname = "newmap";
			}
			Map.Create(arrmapsize[mapsize], arrmapsize[mapheight], arrmapsize[mapsize]);
			MapLoader.GenerateClearMap(1, 0);
			Console.cs.Command("sun 50 330");
			if (GUIPlay.state == 3)
			{
				GUIPlay.StartPoligon();
			}
			else if (GUIPlay.state == 4)
			{
				GUIPlay.StartEditor();
			}
			GUIPlay.CreateControll();
			SPAWN();
			Builder.SetActive(true);
			Controll.pl.team = 0;
			HUD.SetActive(false);
		}
	}

	public void DrawState2(Rect rBack)
	{
		float num = rBack.x + GUIM.YRES(200f);
		sv = GUIM.BeginScrollView(new Rect(num, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 2f, GUIM.YRES(200f), GUIM.YRES(200f)), sv, new Rect(0f, 0f, GUIM.YRES(180f), (float)maplist.Count * GUIM.YRES(26f)));
		for (int i = 0; i < maplist.Count; i++)
		{
			Rect r = new Rect(0f, 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(100f), GUIM.YRES(20f));
			BaseColor fontcolor = BaseColor.White;
			if (GUIM.Contains(r))
			{
				fontcolor = BaseColor.Yellow;
			}
			if (currmap == maplist[i])
			{
				fontcolor = BaseColor.Green;
			}
			GUIM.DrawText(new Rect(0f, 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(20f), GUIM.YRES(20f)), i.ToString(), TextAnchor.MiddleCenter, fontcolor, 0, 20, true);
			GUIM.DrawText(new Rect(GUIM.YRES(22f), 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(80f), GUIM.YRES(20f)), maplist[i].title, TextAnchor.MiddleLeft, fontcolor, 0, 20, true);
			if (GUIM.HideButton(r))
			{
				currmap = maplist[i];
			}
		}
		GUIM.EndScrollView();
		if (GUIM.Button(new Rect(num, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 8f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.Yellow, sLoad, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && currmap != null && !inLoad)
		{
			LOAD_AND_SPAWN();
		}
		if (inLoad)
		{
			int num2 = (int)(Time.time * 3f) % 3;
			GUIM.DrawText(new Rect(num + GUIM.YRES(216f), rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 8f, GUIM.YRES(200f), GUIM.YRES(32f)), sLoading[num2], TextAnchor.MiddleLeft, BaseColor.White, 0, 20, false);
		}
	}

	public static void LOAD_AND_SPAWN()
	{
	}

	public static void SPAWN_AFTER_LOAD()
	{
		if (GUIPlay.state == 3)
		{
			GUIPlay.StartPoligon();
		}
		else if (GUIPlay.state == 4)
		{
			GUIPlay.StartEditor();
		}
		GUIPlay.CreateControll();
		SPAWN();
		Builder.SetActive(true);
		Controll.pl.team = 0;
		HUD.SetActive(false);
	}

	public static void SPAWN()
	{
		Controll.SetLockMove(false);
		Controll.SetLockLook(false);
		Controll.SetLockAttack(false);
		Controll.SetCameraSolid(false);
		GUIGameMenu.SetActive(false);
		SetActive(false);
		HUD.SetActive(true);
		Crosshair.SetActive(true);
		Controll.SpectatorSpawn();
		Palette.SetActive(true);
		HUDBuild.SetActive(true);
		Crosshair.SetCursor(false);
		Radar.SetActive(true);
		Radar.GenerateRadar();
		if (!Builder.active)
		{
			HUDBuild.SetActive(false);
			Palette.SetActive(false);
			if (Builder.cs != null)
			{
				Object.Destroy(Builder.cs);
			}
		}
		else if (Builder.cs == null)
		{
			Builder.cs = GameObject.Find("Core").AddComponent<Builder>();
		}
		GUIOptions.Apply();
	}
}
