using Player;
using UnityEngine;

public class GUIGameExit : MonoBehaviour
{
	public static bool show = false;

	private Texture2D tBlack;

	private Rect rBack;

	private Rect rBackReport;

	private Rect rBackReportTitle;

	private Rect rBackPlayer;

	private string sSaved;

	private string[] sSaving = new string[3];

	private string[] sChatEnable = new string[2];

	private string sGameM;

	private string sBackGame;

	private string sChangWeap;

	private string sSaveMap;

	private string sExitMainM;

	private string sChangeTeam;

	private static bool showreportmenu = false;

	public static bool message_map_saved = false;

	public static bool message_map_insave = false;

	private Rect rReportButtons;

	private Rect rButton;

	private Rect rZone;

	private Rect rPlayer;

	private Rect rScroll;

	private static Vector2 sv = Vector2.zero;

	private static PlayerData ph = null;

	private static float cv_send = 0f;

	private int plcount;

	public void LoadLang()
	{
		sSaved = Lang.GetString("_MAP_SAVED");
		sSaving[0] = Lang.GetString("_CONSERVATION") + ".";
		sSaving[1] = Lang.GetString("_CONSERVATION") + "..";
		sSaving[2] = Lang.GetString("_CONSERVATION") + "...";
		sChatEnable[0] = Lang.GetString("_ENABLE_CHAT");
		sChatEnable[1] = Lang.GetString("_DISABLE_CHAT");
		sGameM = Lang.GetString("_GAME_MENU");
		sBackGame = Lang.GetString("_BACK_TO_GAME");
		sChangWeap = Lang.GetString("_CHANGE_WEAPON");
		sSaveMap = Lang.GetString("_SAVE_MAP");
		sExitMainM = Lang.GetString("_EXIT_MAIN_MENU");
		sChangeTeam = Lang.GetString("_CHANGE_TEAM");
	}

	public static void SetActive(bool val)
	{
		show = val;
		showreportmenu = false;
		sv = Vector2.zero;
		ph = null;
		message_map_saved = false;
		message_map_insave = false;
		Main.HideMenus();
	}

	public static void Toggle()
	{
		SetActive(!show);
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
	}

	private void OnResize()
	{
		rBack = new Rect(GUIM.YRES(32f), GUIM.YRES(160f), GUIM.YRES(240f), GUIM.YRES(400f));
		rBackReport = new Rect((float)Screen.width - GUIM.YRES(272f), GUIM.YRES(160f), GUIM.YRES(240f), GUIM.YRES(170f));
		rBackReportTitle = rBackReport;
		rBackReportTitle.height = GUIM.YRES(64f);
		rBackPlayer = new Rect((float)(Screen.width / 2) - GUIM.YRES(300f), GUIM.YRES(160f), GUIM.YRES(600f), GUIM.YRES(400f));
		rReportButtons = new Rect(rBackPlayer.x, rBackPlayer.y + rBackPlayer.height + GUIM.YRES(4f), rBackPlayer.width, GUIM.YRES(48f));
		rButton = new Rect(rReportButtons.x + rReportButtons.width - GUIM.YRES(208f), rReportButtons.y + GUIM.YRES(8f), GUIM.YRES(200f), GUIM.YRES(32f));
		rZone = new Rect(rBackPlayer.x + GUIM.YRES(16f), rBackPlayer.y + GUIM.YRES(16f), rBackPlayer.width - GUIM.YRES(32f), rBackPlayer.height - GUIM.YRES(32f));
	}

	private void OnGUI()
	{
		if (!show)
		{
			return;
		}
		GUI.depth = -2;
		GUIM.DrawBox(rBack, tBlack);
		float num = rBack.x + GUIM.YRES(20f);
		GUIM.DrawBox(new Rect(0f, GUIM.YRES(86f), (float)Screen.width / 2f - GUIM.YRES(460f) + GUIM.YRES(156f), GUIM.YRES(32f)), tBlack, 0.05f);
		GUIM.DrawText(new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(90f), GUIM.YRES(200f), GUIM.YRES(32f)), sGameM, TextAnchor.MiddleLeft, BaseColor.Gray, 1, 20, false);
		if (GUIM.Button(new Rect(num, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 0f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.Yellow, sBackGame, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			GUIGameMenu.Toggle();
			Main.HideMenus();
		}
		if (!Builder.active)
		{
			if (GUIM.Button(new Rect(num, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 2f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sChangWeap, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				GUIGameMenu.Toggle();
				GUIGameSet.SetActive(true);
			}
			if (GUIM.Button(new Rect(num, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sChatEnable[HUDMessage.chatenable], TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				if (HUDMessage.chatenable == 0)
				{
					HUDMessage.chatenable = 1;
				}
				else
				{
					HUDMessage.chatenable = 0;
				}
			}
			if (GUIM.Button(new Rect(num, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 4f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sChangeTeam, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				int team = ((Controll.pl.team == 0) ? 1 : 0);
				Client.cs.send_changeteam(team);
				SetActive(false);
			}
		}
		if (Builder.active)
		{
			if (GUIM.Button(new Rect(num, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 5f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.White, sSaveMap, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				if (GUIMap.currmap == null)
				{
					Map.SaveBin(GUIMap.mapname);
					message_map_saved = true;
				}
				if (GUIMap.currmap != null)
				{
					Map.SaveBin(GUIMap.currmap.title);
					message_map_saved = true;
				}
			}
			if (message_map_saved)
			{
				GUIM.DrawText(new Rect(num + GUIM.YRES(220f) + GUIM.YRES(16f), rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 4f, GUIM.YRES(200f), GUIM.YRES(32f)), sSaved + " " + GUIMap.mapname, TextAnchor.MiddleLeft, BaseColor.Green, 0, 20, true);
			}
		}
		if (GUIM.Button(new Rect(num, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 7f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.Red, sExitMainM, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			ExitMainMenu();
		}
	}

	private void DrawReport()
	{
		GUIM.DrawBox(rBackReport, tBlack);
		float x = rBackReport.x + GUIM.YRES(20f);
		GUIM.DrawText(rBackReportTitle, "REPORT MENU", TextAnchor.MiddleCenter, BaseColor.Gray, 1, 20, false);
		if (GUIM.Button(new Rect(x, rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 0f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.Orange, "VOTE KICK", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			showreportmenu = !showreportmenu;
		}
		if (showreportmenu)
		{
			DrawReportPlayers();
		}
	}

	private void DrawReportPlayers()
	{
		GUIM.DrawBox(rBackPlayer, tBlack);
		rPlayer = new Rect(0f, 0f, rBackPlayer.width - GUIM.YRES(50f), GUIM.YRES(30f));
		rScroll = new Rect(0f, 0f, rPlayer.width, GUIM.YRES(32f) * (float)plcount);
		sv = GUIM.BeginScrollView(rZone, sv, rScroll);
		plcount = 0;
		for (int i = 0; i < 40; i++)
		{
			if (PLH.player[i] != null)
			{
				DrawPlayer(rPlayer, PLH.player[i]);
				rPlayer.y += GUIM.YRES(32f);
				plcount++;
			}
		}
		GUIM.EndScrollView();
		if (ph != null)
		{
			GUIM.DrawBox(rReportButtons, TEX.tBlack);
			if (!(Time.time < cv_send) && GUIM.Button(rButton, BaseColor.Red, "CALLVOTE KICK", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				cv_send = Time.time + 180f;
				Client.cs.send_callvote(0, ph.idx);
			}
		}
	}

	private void DrawPlayer(Rect r, PlayerData pl)
	{
		if (GUIM.HideButton(r))
		{
			ph = pl;
		}
		if (ph == pl)
		{
			GUI.DrawTexture(r, TEX.tDarkGray);
		}
		else
		{
			GUI.DrawTexture(r, TEX.tGray);
		}
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(4f), r.width, r.height), pl.sLVL, TextAnchor.MiddleLeft, BaseColor.Orange, 1, 18, true);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(26f), r.y - GUIM.YRES(3f), r.width, r.height), pl.name, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, true);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(26f), r.y + GUIM.YRES(11f), r.width, r.height), pl.clanname, TextAnchor.MiddleLeft, BaseColor.White, 1, 10, false);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f) + GUIM.YRES(200f), r.y - GUIM.YRES(4f), GUIM.YRES(30f), r.height), pl.sEXP, TextAnchor.MiddleRight, BaseColor.Yellow, 1, 12, true);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f) + GUIM.YRES(200f), r.y + GUIM.YRES(10f), GUIM.YRES(30f), r.height), pl.sFDA, TextAnchor.MiddleRight, BaseColor.White, 1, 12, false);
	}

	public static void ExitMainMenu()
	{
		GUIGameSet.SetActive(false);
		Main.SetActive(true);
		MainManager.state = 0;
		PLH.Clear();
		Map.Clear();
		HUD.Clear();
		Controll.Clear();
		Client.cs.Disconnect();
		if (GUIAdmin.cs != null)
		{
			GUIAdmin.cs.showhint = false;
		}
		GOpt.custom_render = false;
		RenderSettings.fogColor = new Color(40f / 51f, 46f / 51f, 1f, 1f);
		RenderSettings.fogStartDistance = 128f;
		RenderSettings.fogEndDistance = 256f;
		GameObject obj = GameObject.Find("Dlight");
		obj.GetComponent<Light>().intensity = 1f;
		obj.transform.eulerAngles = new Vector3(50f, 330f, 0f);
		Map.sunpos = new Vector3(50f, 330f, 0f);
		GOP.Clear();
	}
}
