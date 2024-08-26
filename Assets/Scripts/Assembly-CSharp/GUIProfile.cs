using UnityEngine;

public class GUIProfile : MonoBehaviour
{
	public static bool show = false;

	private static Texture2D tBlack;

	private static Texture2D tWhite;

	public static string sLevel = "УРОВЕНЬ: N/A";

	public static string sExp = "0";

	public static string sLevelProgress = "0%";

	public static float fLevelProgress = 0f;

	public static string sExpProgress = "0/0";

	public static string sF = "0";

	public static string sD = "0";

	public static string sA = "0";

	public static string sH = "0";

	public static string sFD = "0";

	public static string hash_stats = "_";

	private string sExper;

	private string sLvlProgress;

	private string sDeath;

	private string sRatio;

	private string sAssist;

	private string sHeadshot;

	private string sFrags;

	public static string sLeveltext;

	public void LoadLang()
	{
		sExper = Lang.GetString("_EXPA");
		sLvlProgress = Lang.GetString("_LEVEL_PROGRESS");
		sDeath = Lang.GetString("_DEATH");
		sRatio = Lang.GetString("_RATIO");
		sAssist = Lang.GetString("_ASSIST");
		sHeadshot = Lang.GetString("_HEADSHOT");
		sFrags = Lang.GetString("_FRAGS");
		sLeveltext = Lang.GetString("_LEVEL");
		sLevel = sLeveltext + " N/A";
	}

	public static void SetActive(bool val)
	{
		show = val;
		if (val)
		{
			GUIFX.Set();
			Load();
		}
		else
		{
			GUIFX.End();
		}
	}

	public static void Load()
	{
		MasterClient.cs.send_stats();
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("lightgray");
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIFX.Begin();
			Rect r = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(80f));
			GUIM.DrawBox(r, tBlack, 0.05f);
			GUIM.DrawText(new Rect(r.x, r.y + GUIM.YRES(8f), r.width, GUIM.YRES(32f)), GUIOptions.playername, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, true);
			GUIM.DrawText(new Rect(r.x, r.y + GUIM.YRES(40f), r.width, GUIM.YRES(32f)), sLevel, TextAnchor.MiddleCenter, BaseColor.LightGray, 1, 16, false);
			DrawStatLeft(new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f) + GUIM.YRES(96f), GUIM.YRES(452f), GUIM.YRES(200f)));
			DrawStatRight(new Rect((float)Screen.width / 2f + GUIM.YRES(8f), GUIM.YRES(140f) + GUIM.YRES(96f), GUIM.YRES(452f), GUIM.YRES(200f)));
			GUIM.DrawBox(new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f) + GUIM.YRES(96f) + GUIM.YRES(216f), GUIM.YRES(920f), GUIM.YRES(188f)), tBlack, 0.05f);
			GUIFX.End();
		}
	}

	private void DrawStatLeft(Rect r)
	{
		GUIM.DrawBox(r, tBlack, 0.05f);
		DrawLine(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f), r.width - GUIM.YRES(128f), GUIM.YRES(32f)), sExper, sExp);
		DrawLine(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f) + GUIM.YRES(32f), r.width - GUIM.YRES(128f), GUIM.YRES(32f)), sLvlProgress, sLevelProgress);
		DrawLine(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f) + GUIM.YRES(32f) * 2f, r.width - GUIM.YRES(128f), GUIM.YRES(32f)), "", sExpProgress);
		GUI.DrawTexture(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f) + GUIM.YRES(32f) * 2f + GUIM.YRES(8f), GUIM.YRES(150f), GUIM.YRES(8f)), tBlack);
		GUI.DrawTexture(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f) + GUIM.YRES(32f) * 2f + GUIM.YRES(8f), GUIM.YRES(150f) * fLevelProgress, GUIM.YRES(8f)), tWhite);
	}

	private void DrawStatRight(Rect r)
	{
		GUIM.DrawBox(r, tBlack, 0.05f);
		DrawLine(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f), r.width - GUIM.YRES(128f), GUIM.YRES(32f)), sFrags, sF);
		DrawLine(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f) + GUIM.YRES(32f), r.width - GUIM.YRES(128f), GUIM.YRES(32f)), sDeath, sD);
		DrawLine(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f) + GUIM.YRES(32f) * 2f, r.width - GUIM.YRES(128f), GUIM.YRES(32f)), sRatio, sFD);
		DrawLine(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f) + GUIM.YRES(32f) * 3f, r.width - GUIM.YRES(128f), GUIM.YRES(32f)), sAssist, sA);
		DrawLine(new Rect(r.x + GUIM.YRES(64f), r.y + GUIM.YRES(16f) + GUIM.YRES(32f) * 4f, r.width - GUIM.YRES(128f), GUIM.YRES(32f)), sHeadshot, sH);
	}

	private void DrawLine(Rect r, string label, string value)
	{
		GUIM.DrawText(r, label, TextAnchor.MiddleLeft, BaseColor.LightGray, 1, 16, false);
		GUIM.DrawText(r, value, TextAnchor.MiddleRight, BaseColor.White, 1, 16, false);
	}
}
