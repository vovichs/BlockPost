using UnityEngine;

public class HUDGameEnd : MonoBehaviour
{
	public static bool show = false;

	private Rect rWin;

	private Rect rTime;

	private Rect[] rScore = new Rect[2];

	private Rect[] rScoreLine = new Rect[2];

	private Rect rTeam;

	private static float timereconnect = 0f;

	public static string sWin = "";

	public static string sWon;

	private string sNextGame;

	private string sGameOver;

	private string sRedWins;

	private string sBlueWins;

	private string sDraw;

	private string sGameZombiesWin = "ZOMBIES WIN";

	private string sGameHumansWin = "HUMANS WINS";

	public void LoadLang()
	{
		sWon = Lang.GetString("_WON");
		sNextGame = Lang.GetString("_NEXT_GAME_WILLBE");
		sGameOver = Lang.GetString("_GAME_OVER");
		sRedWins = Lang.GetString("_TEAM_RED_WINS");
		sBlueWins = Lang.GetString("_TEAM_BLUE_WINS");
		sDraw = Lang.GetString("_DRAW");
	}

	public static void SetActive(bool val)
	{
		show = val;
		if (!val)
		{
			return;
		}
		timereconnect = Time.time + 5f + (float)Controll.pl.idx * 0.05f;
		if (Controll.gamemode != 2)
		{
			return;
		}
		sWin = "";
		HUDTab.SortPlayers();
		if (HUDTab.sorted[0] >= 0)
		{
			int num = HUDTab.sorted[0];
			if (PLH.player[num] != null)
			{
				sWin = sWon + " " + PLH.player[num].name;
			}
		}
	}

	private void OnResize()
	{
		rWin.Set(0f, GUIM.YRES(170f), Screen.width, GUIM.YRES(40f));
		rScore[0].Set((float)Screen.width / 2f - GUIM.YRES(60f), GUIM.YRES(220f), GUIM.YRES(40f), GUIM.YRES(40f));
		rScore[1].Set((float)Screen.width / 2f + GUIM.YRES(20f), GUIM.YRES(220f), GUIM.YRES(40f), GUIM.YRES(40f));
		rTeam.Set(0f, GUIM.YRES(270f), Screen.width, GUIM.YRES(40f));
		rTime.Set(0f, GUIM.YRES(300f), Screen.width, GUIM.YRES(40f));
		rScoreLine[0].Set(rScore[0].x, rScore[0].y + rScore[0].height, rScore[0].width, GUIM.YRES(3f));
		rScoreLine[1].Set(rScore[1].x, rScore[1].y + rScore[1].height, rScore[1].width, GUIM.YRES(3f));
	}

	private void OnGUI()
	{
		if (show)
		{
			if (Controll.gamemode == 2 || Controll.gamemode == 8)
			{
				DrawEndFree();
			}
			else
			{
				DrawEndTeam();
			}
			if (Time.time > timereconnect)
			{
				timereconnect = 0f;
				GUIGameExit.ExitMainMenu();
				Client.cs.Connect();
			}
			else
			{
				int num = (int)(timereconnect - Time.time);
				GUIM.DrawText(rTime, sNextGame + " " + num, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, true);
			}
		}
	}

	private void DrawEndTeam()
	{
		GUIM.DrawText(rWin, sGameOver, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 40, true);
		GUIM.DrawText(rScore[0], HUD.sScore0, TextAnchor.MiddleCenter, BaseColor.White, 1, 40, true);
		GUIM.DrawText(rScore[1], HUD.sScore1, TextAnchor.MiddleCenter, BaseColor.White, 1, 40, true);
		if (Controll.gamemode == 5)
		{
			if (HUD.iScore0 > HUD.iScore1)
			{
				GUIM.DrawText(rTeam, sGameHumansWin, TextAnchor.MiddleCenter, BaseColor.Yellow, 1, 32, true);
				GUI.DrawTexture(rScoreLine[0], TEX.tYellow);
			}
			else if (HUD.iScore0 < HUD.iScore1)
			{
				GUIM.DrawText(rTeam, sGameZombiesWin, TextAnchor.MiddleCenter, BaseColor.Green, 1, 32, true);
				GUI.DrawTexture(rScoreLine[1], TEX.tGreen);
			}
			else
			{
				GUIM.DrawText(rTeam, sDraw, TextAnchor.MiddleCenter, BaseColor.Gray, 1, 32, true);
			}
		}
		else if (HUD.iScore0 > HUD.iScore1)
		{
			GUIM.DrawText(rTeam, sRedWins, TextAnchor.MiddleCenter, BaseColor.Red, 1, 32, true);
			GUI.DrawTexture(rScoreLine[0], TEX.tRed);
		}
		else if (HUD.iScore0 < HUD.iScore1)
		{
			GUIM.DrawText(rTeam, sBlueWins, TextAnchor.MiddleCenter, BaseColor.Blue, 1, 32, true);
			GUI.DrawTexture(rScoreLine[1], TEX.tBlue);
		}
		else
		{
			GUIM.DrawText(rTeam, sDraw, TextAnchor.MiddleCenter, BaseColor.Gray, 1, 32, true);
		}
	}

	private void DrawEndFree()
	{
		GUIM.DrawText(rWin, sGameOver, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 40, true);
		GUIM.DrawText(rTeam, sWin, TextAnchor.MiddleCenter, BaseColor.Green, 1, 32, true);
	}
}
