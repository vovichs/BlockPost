using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIAdmin : MonoBehaviour
{
	public static GUIAdmin cs;

	public bool show;

	public bool showhint;

	private Texture2D tBlack;

	private Texture2D tRed;

	private Texture2D tWhite;

	private Color[] cList;

	private Rect rBack;

	private Rect rBody;

	private Rect rLog;

	private Rect[] rButton;

	private Rect rHint;

	private Rect rClose;

	private Rect rHeader;

	private Rect[] rLogRow;

	private Rect rLogView;

	private Vector2 sv = Vector2.zero;

	private Rect rLogScroll;

	private List<string> loglist;

	private GUIAdminSettings csSettings;

	private GUIAdminUpload csUploadMap;

	private GUIAdminMaplist csMaplist;

	private GUIAdminPlayers csPlayers;

	private string sHint = "НАЖМИТЕ <color=yellow>K</color> ЧТОБЫ ОТКРЫТЬ МЕНЮ АДМИНИСТРАТОРА СЕРВЕРА";

	private string sHeader = "МЕНЮ АДМИНИСТРАТОРА СЕРВЕРА";

	private string sServerSettings = "НАСТРОЙКА СЕРВЕРА";

	private string sMapManagement = "УПРАВЛЕНИЕ КАРТАМИ";

	private string sPlayerManagement = "УПРАВЛЕНИЕ ИГРОКАМИ";

	private string sUploadMaps = "ЗАГРУЗКА КАРТ";

	private void Awake()
	{
		cs = this;
		cList = new Color[4];
		cList[0] = new Color(0.1f, 0.1f, 0.1f, 1f);
		cList[1] = new Color(0.2f, 0.2f, 0.2f, 1f);
		cList[2] = new Color(0.3f, 0.3f, 0.3f, 1f);
		cList[3] = new Color(0.1f, 0.2f, 0.4f, 1f);
		loglist = new List<string>();
	}

	private void Start()
	{
		tBlack = TEX.GetTextureByName("black");
		tRed = TEX.GetTextureByName("red");
		tWhite = TEX.GetTextureByName("white");
		OnResize();
		LoadLang();
		Log("log started");
	}

	private void LoadLang()
	{
		sHint = Lang.GetString("_CLICK_OPEN_ADMIN_MENU_SERVER");
		sHeader = Lang.GetString("_MENU_ADMIN_SERVER");
		sServerSettings = Lang.GetString("_SERVER_CONFIG");
		sMapManagement = Lang.GetString("_MAPS_MANAGER");
		sPlayerManagement = Lang.GetString("_PLAYERS_MANAGER");
		sUploadMaps = Lang.GetString("_MAPS_LOADING");
	}

	private void OnResize()
	{
		rHint.Set((float)Screen.width / 2f - GUIM.YRES(200f), (float)Screen.height - GUIM.YRES(158f), GUIM.YRES(400f), GUIM.YRES(32f));
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(400f), (float)Screen.height / 2f - GUIM.YRES(200f), GUIM.YRES(196f), GUIM.YRES(32f));
		rBody = new Rect((float)Screen.width / 2f - GUIM.YRES(200f), (float)Screen.height / 2f - GUIM.YRES(200f), GUIM.YRES(600f), GUIM.YRES(400f));
		rLog = new Rect(rBody.x, rBody.y + rBody.height + GUIM.YRES(4f), rBody.width, GUIM.YRES(100f));
		rLogView = new Rect(rLog.x + GUIM.YRES(4f), rLog.y + GUIM.YRES(4f), rLog.width - GUIM.YRES(8f), rLog.height - GUIM.YRES(8f));
		int num = (int)GUIM.YRES(4f);
		rButton = new Rect[8];
		rButton[0] = rBack;
		rButton[1] = new Rect(rBack.x, rBack.y + (GUIM.YRES(32f) + (float)num) * 1f, rBack.width, rBack.height);
		rButton[2] = new Rect(rBack.x, rBack.y + (GUIM.YRES(32f) + (float)num) * 2f, rBack.width, rBack.height);
		rButton[3] = new Rect(rBack.x, rBack.y + (GUIM.YRES(32f) + (float)num) * 3f, rBack.width, rBack.height);
		rButton[4] = new Rect(rBack.x, rBack.y + (GUIM.YRES(32f) + (float)num) * 4f, rBack.width, rBack.height);
		rButton[5] = new Rect(rBack.x, rBack.y + (GUIM.YRES(32f) + (float)num) * 5f, rBack.width, rBack.height);
		rButton[6] = new Rect(rBack.x, rBack.y + (GUIM.YRES(32f) + (float)num) * 6f, rBack.width, rBack.height);
		rButton[7] = new Rect(rBack.x, rBack.y + (GUIM.YRES(32f) + (float)num) * 7f, rBack.width, rBack.height);
		rHeader = new Rect(rBack.x, rBack.y - GUIM.YRES(36f), GUIM.YRES(200f), GUIM.YRES(32f));
		rClose = new Rect((float)Screen.width / 2f + GUIM.YRES(368f), (float)Screen.height / 2f - GUIM.YRES(236f), GUIM.YRES(32f), GUIM.YRES(32f));
		rLogRow = new Rect[16];
		for (int i = 0; i < 16; i++)
		{
			rLogRow[i] = new Rect(0f, (float)i * GUIM.YRES(26f), rLogView.width, GUIM.YRES(26f));
		}
		rLogScroll = new Rect(0f, 0f, rLogView.width - GUIM.YRES(20f), 15f * GUIM.YRES(26f));
	}

	private void OnGUI()
	{
		GUI.depth = -2;
		if (showhint)
		{
			DrawHint();
		}
		if (show)
		{
			GUIM.DrawText(rHeader, sHeader, TextAnchor.MiddleLeft, BaseColor.White, 1, 20, true);
			DrawButton(0, rButton[0], sServerSettings);
			DrawButton(1, rButton[1], sMapManagement);
			DrawButton(2, rButton[2], sPlayerManagement);
			DrawButton(4, rButton[4], sUploadMaps);
			GUI.DrawTexture(rClose, tBlack);
			GUIM.DrawText(rClose, "X", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
			GUI.DrawTexture(rLog, tBlack);
			sv = GUIM.BeginScrollView(rLogView, sv, rLogScroll);
			for (int i = 0; i < loglist.Count; i++)
			{
				GUI.color = ((i % 2 == 0) ? cList[0] : cList[1]);
				GUI.DrawTexture(rLogRow[i], tWhite);
				GUI.color = Color.white;
				GUIM.DrawText(rLogRow[i], loglist[i], TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
			}
			GUIM.EndScrollView();
		}
	}

	private void DrawHint()
	{
		GUIM.DrawBox(rHint, tRed);
		GUIM.DrawText(rHint, sHint, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.K))
		{
			SetActive(!show);
			showhint = false;
		}
	}

	private void DrawButton(int id, Rect r, string label)
	{
		GUI.DrawTexture(r, tBlack);
		GUIM.DrawText(r, label, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		if (GUIM.HideButton(r))
		{
			Activate(id);
		}
	}

	private void Activate(int id)
	{
		HideAll();
		if (id == 0)
		{
			if (csSettings == null)
			{
				csSettings = base.gameObject.AddComponent<GUIAdminSettings>();
			}
			csSettings.show = true;
		}
		if (id == 1)
		{
			if (csMaplist == null)
			{
				csMaplist = base.gameObject.AddComponent<GUIAdminMaplist>();
			}
			csMaplist.show = true;
			csMaplist.setmap = null;
			csMaplist.delmap = null;
		}
		if (id == 2)
		{
			if (csPlayers == null)
			{
				csPlayers = base.gameObject.AddComponent<GUIAdminPlayers>();
			}
			csPlayers.show = true;
		}
		if (id == 4)
		{
			if (csUploadMap == null)
			{
				csUploadMap = base.gameObject.AddComponent<GUIAdminUpload>();
			}
			csUploadMap.LoadMapList();
			csUploadMap.show = true;
		}
	}

	private void HideAll()
	{
		if (csSettings != null)
		{
			csSettings.show = false;
		}
		if (csUploadMap != null)
		{
			csUploadMap.show = false;
		}
		if (csMaplist != null)
		{
			csMaplist.show = false;
		}
		if (csPlayers != null)
		{
			csPlayers.show = false;
		}
	}

	public void SetActive(bool val)
	{
		if (GUIGameMenu.show)
		{
			GUIGameMenu.SetActive(false);
		}
		HideAll();
		show = val;
		Crosshair.SetCursor(show);
		Controll.SetLockLook(show);
		Controll.SetLockAttack(show);
		if (val)
		{
			Activate(0);
		}
	}

	public void Log(string msg)
	{
		loglist.Add(DateTime.Now.ToString("mm:ss") + " " + msg);
		if (loglist.Count > 15)
		{
			loglist.RemoveAt(0);
		}
	}
}
