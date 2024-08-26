using GameClass;
using UnityEngine;

public class GUIRank : MonoBehaviour
{
	public class TPRect
	{
		public Rect rb0;

		public Rect rb1;

		public Rect rName;

		public Rect rLevel;

		public Rect rKD;

		public Rect rF;

		public Rect rD;

		public Rect rA;

		public Rect rH;

		public Rect rEXP;

		public TPRect(Rect rBase)
		{
			rb0 = new Rect(rBase.x, rBase.y, GUIM.YRES(40f), rBase.height);
			rb1 = new Rect(rBase.x + GUIM.YRES(44f), rBase.y, rBase.width - GUIM.YRES(44f), rBase.height);
			rName = new Rect(rb1.x + GUIM.YRES(20f), rb1.y + GUIM.YRES(2f), rb1.width, rb1.height);
			rLevel = new Rect(rb1.x + GUIM.YRES(190f), rb1.y + GUIM.YRES(2f), rb1.width, rb1.height);
			rKD = new Rect(rb1.x + GUIM.YRES(220f), rb1.y + GUIM.YRES(2f), GUIM.YRES(50f), rb1.height);
			rF = new Rect(rb1.x + GUIM.YRES(270f), rb1.y + GUIM.YRES(2f), GUIM.YRES(50f), rb1.height);
			rD = new Rect(rb1.x + GUIM.YRES(320f), rb1.y + GUIM.YRES(2f), GUIM.YRES(50f), rb1.height);
			rA = new Rect(rb1.x + GUIM.YRES(370f), rb1.y + GUIM.YRES(2f), GUIM.YRES(50f), rb1.height);
			rH = new Rect(rb1.x + GUIM.YRES(420f), rb1.y + GUIM.YRES(2f), GUIM.YRES(50f), rb1.height);
			rEXP = new Rect(rb1.x + GUIM.YRES(470f), rb1.y + GUIM.YRES(2f), GUIM.YRES(60f), rb1.height);
		}
	}

	public static bool show = false;

	private static Texture2D tBlack = null;

	private static Texture2D tWhite = null;

	private static Texture2D tGray = null;

	private static Texture2D tYellow = null;

	private static Texture2D tBlue = null;

	public static TopPlayer[] tp = null;

	public static TopClan[] tc = null;

	private static Rect rBack;

	private static Rect rZone;

	private static int currpage = 0;

	private static string sCurrpage = "1-25";

	public static int position = 9999;

	public static float progress = 0f;

	public static string sProgress = "0%";

	public static string sYouTake = "";

	public static string sPlaceRating = "";

	private static TPRect[] tpr = null;

	private static TPRect header = null;

	private string sPlayer;

	private string sNotRating;

	private string sShow;

	private Vector2 scroll = Vector2.zero;

	public void LoadLang()
	{
		sPlayer = Lang.GetString("_PLAYER");
		sNotRating = Lang.GetString("_NOT_ENTER_RATING");
		sShow = Lang.GetString("_SHOW");
		sYouTake = Lang.GetString("_YOU_TAKE");
		sPlaceRating = Lang.GetString("_PLACE_RATING");
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
		if (tp == null)
		{
			tp = new TopPlayer[500];
		}
		MasterClient.cs.send_rank(currpage);
	}

	private void OnResize()
	{
		rBack.Set((float)Screen.width / 2f - GUIM.YRES(300f), GUIM.YRES(140f) + GUIM.YRES(40f), GUIM.YRES(600f), GUIM.YRES(420f));
		int num = (int)GUIM.YRES(4f);
		int num2 = (int)GUIM.YRES(26f);
		if (tpr == null)
		{
			tpr = new TPRect[25];
		}
		for (int i = 0; i < 25; i++)
		{
			Rect rBase = new Rect(0f, (num2 + num) * i, rBack.width - GUIM.YRES(18f), num2);
			tpr[i] = new TPRect(rBase);
		}
		rZone = new Rect(0f, 0f, 0f, (num2 + num) * 25 - num);
		header = new TPRect(new Rect((float)Screen.width / 2f - GUIM.YRES(300f), GUIM.YRES(142f), rBack.width, num2));
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tBlue = TEX.GetTextureByName("blue");
		tWhite = TEX.GetTextureByName("lightgray");
	}

	private void OnGUI()
	{
		if (show && tp != null)
		{
			if (Main.currSubMenu == 0)
			{
				DrawPlayers();
			}
			else if (Main.currSubMenu == 1)
			{
				DrawClans();
			}
		}
	}

	private void OnChangeSubMenu()
	{
		if (!show)
		{
			return;
		}
		currpage = 0;
		sCurrpage = "1-25";
		if (Main.currSubMenu == 1)
		{
			if (tc == null)
			{
				tc = new TopClan[100];
			}
			MasterClient.cs.send_rankclan(currpage);
		}
	}

	private void DrawPlayers()
	{
		GUIFX.Begin();
		GUIM.DrawBox(new Rect(rBack.x, GUIM.YRES(140f), rBack.width, GUIM.YRES(32f)), tBlack, 0.05f);
		GUIM.DrawText(header.rb0, "#", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rName, sPlayer, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rLevel, "LvL", TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rKD, "K/D", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rF, "F", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rD, "D", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rA, "A", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rH, "H", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rEXP, "EXP", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		Rect r = new Rect(rBack.x, rBack.y + rBack.height + GUIM.YRES(8f), rBack.width, GUIM.YRES(32f));
		GUIM.DrawBox(r, tBlack, 0.05f);
		if (GUIM.Button(new Rect(r.x + GUIM.YRES(420f), r.y + GUIM.YRES(5f), GUIM.YRES(52f), GUIM.YRES(22f)), BaseColor.White, "<", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && currpage > 0)
		{
			currpage--;
			sCurrpage = currpage * 25 + 1 + "-" + (currpage + 1) * 25;
			MasterClient.cs.send_rank(currpage);
		}
		if (GUIM.Button(new Rect(r.x + GUIM.YRES(540f), r.y + GUIM.YRES(5f), GUIM.YRES(52f), GUIM.YRES(22f)), BaseColor.White, ">", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && currpage < 19)
		{
			currpage++;
			sCurrpage = currpage * 25 + 1 + "-" + (currpage + 1) * 25;
			MasterClient.cs.send_rank(currpage);
		}
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(472f), r.y + GUIM.YRES(5f), GUIM.YRES(68f), GUIM.YRES(22f)), sCurrpage, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		if (position == 9999)
		{
			Rect r2 = new Rect(r.x + GUIM.YRES(20f), r.y + GUIM.YRES(5f), GUIM.YRES(68f), GUIM.YRES(22f));
			Rect rect = new Rect(r.x + GUIM.YRES(180f), r.y + GUIM.YRES(10f), GUIM.YRES(100f), GUIM.YRES(10f));
			Rect rect2 = new Rect(rect.x, rect.y, rect.width * progress, rect.height);
			Rect r3 = new Rect(rect.x + rect.width, r2.y, GUIM.YRES(32f), GUIM.YRES(22f));
			GUIM.DrawText(r2, sNotRating, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, false);
			GUI.DrawTexture(rect, tBlack);
			GUI.DrawTexture(rect2, tWhite);
			GUIM.DrawText(r3, sProgress, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		else
		{
			Rect r4 = new Rect(r.x + GUIM.YRES(20f), r.y + GUIM.YRES(5f), GUIM.YRES(68f), GUIM.YRES(22f));
			Rect r5 = new Rect(r.x + GUIM.YRES(240f), r.y + GUIM.YRES(5f), GUIM.YRES(80f), GUIM.YRES(22f));
			GUIM.DrawText(r4, sProgress, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, false);
			if (GUIM.Button(r5, BaseColor.White, sShow, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				currpage = position / 25;
				sCurrpage = currpage * 25 + 1 + "-" + (currpage + 1) * 25;
				MasterClient.cs.send_rank(currpage);
			}
		}
		scroll = GUIM.BeginScrollView(rBack, scroll, rZone);
		int num = currpage * 25;
		int num2 = (currpage + 1) * 25;
		int num3 = 0;
		for (int i = num; i < num2; i++)
		{
			DrawField(num3, tp[i]);
			num3++;
		}
		GUIM.EndScrollView();
		GUIFX.End();
	}

	private void DrawClans()
	{
		GUIFX.Begin();
		GUIM.DrawBox(new Rect(rBack.x, GUIM.YRES(140f), rBack.width, GUIM.YRES(32f)), tBlack, 0.05f);
		GUIM.DrawText(header.rb0, "#", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rName, Lang.GetString("_CLAN"), TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rLevel, "LvL", TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rA, Lang.GetString("_PLAYERS"), TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rH, Lang.GetString("_SLOTS"), TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rEXP, "EXP", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		scroll = GUIM.BeginScrollView(rBack, scroll, rZone);
		int num = currpage * 25;
		int num2 = (currpage + 1) * 25;
		int num3 = 0;
		for (int i = num; i < num2; i++)
		{
			DrawField(num3, tc[i]);
			num3++;
		}
		GUIM.EndScrollView();
		GUIFX.End();
		Rect r = new Rect(rBack.x, rBack.y + rBack.height + GUIM.YRES(8f), rBack.width, GUIM.YRES(32f));
		GUIM.DrawBox(r, tBlack, 0.05f);
		if (GUIM.Button(new Rect(r.x + GUIM.YRES(420f), r.y + GUIM.YRES(5f), GUIM.YRES(52f), GUIM.YRES(22f)), BaseColor.White, "<", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && currpage > 0)
		{
			currpage--;
			sCurrpage = currpage * 25 + 1 + "-" + (currpage + 1) * 25;
			MasterClient.cs.send_rankclan(currpage);
		}
		if (GUIM.Button(new Rect(r.x + GUIM.YRES(540f), r.y + GUIM.YRES(5f), GUIM.YRES(52f), GUIM.YRES(22f)), BaseColor.White, ">", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && currpage < 3)
		{
			currpage++;
			sCurrpage = currpage * 25 + 1 + "-" + (currpage + 1) * 25;
			MasterClient.cs.send_rankclan(currpage);
		}
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(472f), r.y + GUIM.YRES(5f), GUIM.YRES(68f), GUIM.YRES(22f)), sCurrpage, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
	}

	private void DrawField(int slot, TopPlayer data)
	{
		if (data != null)
		{
			TPRect tPRect = tpr[slot];
			if (data.pos == position)
			{
				GUIM.DrawBox(tPRect.rb0, tBlue, 0.15f);
				GUIM.DrawBox(tPRect.rb1, tBlue, 0.05f);
			}
			else if (GUIM.Contains(tPRect.rb1))
			{
				GUIM.DrawBox(tPRect.rb0, tWhite, 0.15f);
				GUIM.DrawBox(tPRect.rb1, tWhite, 0.05f);
			}
			else
			{
				GUIM.DrawBox(tPRect.rb0, tBlack, 0.15f);
				GUIM.DrawBox(tPRect.rb1, tBlack, 0.05f);
			}
			GUIM.DrawText(tPRect.rb0, data.sPos, TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
			GUIM.DrawText(tPRect.rName, data.n, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
			GUIM.DrawText(tPRect.rLevel, data.sLV, TextAnchor.MiddleLeft, BaseColor.Orange, 1, 14, false);
			GUIM.DrawText(tPRect.rKD, data.sKD, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
			GUIM.DrawText(tPRect.rF, data.sF, TextAnchor.MiddleCenter, BaseColor.LightGray, 1, 14, false);
			GUIM.DrawText(tPRect.rD, data.sD, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
			GUIM.DrawText(tPRect.rA, data.sA, TextAnchor.MiddleCenter, BaseColor.LightGray2, 1, 14, false);
			GUIM.DrawText(tPRect.rH, data.sH, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
			GUIM.DrawText(tPRect.rEXP, data.sEXP, TextAnchor.MiddleCenter, BaseColor.Gray, 1, 14, false);
		}
	}

	private void DrawField(int slot, TopClan data)
	{
		if (data != null)
		{
			TPRect tPRect = tpr[slot];
			if (data.gid == GUIClan.clanid)
			{
				GUIM.DrawBox(tPRect.rb0, tBlue, 0.15f);
				GUIM.DrawBox(tPRect.rb1, tBlue, 0.05f);
			}
			else if (GUIM.Contains(tPRect.rb1))
			{
				GUIM.DrawBox(tPRect.rb0, tWhite, 0.15f);
				GUIM.DrawBox(tPRect.rb1, tWhite, 0.05f);
			}
			else
			{
				GUIM.DrawBox(tPRect.rb0, tBlack, 0.15f);
				GUIM.DrawBox(tPRect.rb1, tBlack, 0.05f);
			}
			GUIM.DrawText(tPRect.rb0, data.sPos, TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
			GUIM.DrawText(tPRect.rName, data.n, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
			GUIM.DrawText(tPRect.rLevel, data.sLV, TextAnchor.MiddleLeft, BaseColor.Orange, 1, 14, false);
			GUIM.DrawText(tPRect.rA, data.sPlayers, TextAnchor.MiddleCenter, BaseColor.LightGray2, 1, 14, false);
			GUIM.DrawText(tPRect.rH, data.sSlots, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
			GUIM.DrawText(tPRect.rEXP, data.sEXP, TextAnchor.MiddleCenter, BaseColor.Gray, 1, 14, false);
		}
	}
}
