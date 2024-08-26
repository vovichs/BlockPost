using Player;
using UnityEngine;

public class GUICraft : MonoBehaviour
{
	public static bool show = false;

	private static float crafttime = 5f;

	private static float selltime = 2f;

	public static bool clicklock = false;

	private static Texture2D process = null;

	private Rect rBack;

	private Rect rBackHint;

	private Rect rBack2;

	private Rect rScroll;

	private Rect rRes;

	private Rect[] rItem = new Rect[3];

	private Rect rArrow;

	private Rect rButton;

	private Rect[] rSteamHint = new Rect[3];

	private static WeaponInv[] Item = new WeaponInv[3];

	private static float tStart = 0f;

	private static CaseInv[] caseItem = new CaseInv[3];

	private Texture2D tBlack;

	private Texture2D tYellow;

	private Texture2D tWhite;

	private Texture2D tOrange;

	private Texture2D tArrowR;

	private Texture2D tAngle;

	private static Vector2 scroll = Vector2.zero;

	private bool caseinforeq;

	public static int cs_sec = 0;

	public static float cs_time = 0f;

	private string sParseCase;

	private string sParseWeap;

	private string sSale2;

	private string sSale3;

	private string sSale1;

	private string sWaitServ;

	private string sStPars;

	private string sStUpgNot;

	private string sSaleWill;

	private string sStSale;

	private string sContinue;

	private string sCancel;

	private string sParseCaseSteam;

	private static float angle = 0f;

	private WeaponInv it;

	private static bool craftend = false;

	private static CaseInv craftcase = null;

	public void LoadLang()
	{
		sParseCaseSteam = Lang.GetString("_PLACE_WEAPON_CASE_STEAM");
		sParseCase = Lang.GetString("_PLACE_WEAPON_CASE");
		sParseWeap = Lang.GetString("_PLACE_WEAPON_WEAPON");
		sSale2 = Lang.GetString("_PLACE_CASE_2_COIN");
		sSale3 = Lang.GetString("_PLACE_CASE_3_COIN");
		sSale1 = Lang.GetString("_PLACE_CASE_1_COIN");
		sWaitServ = Lang.GetString("_WAITING_SERVER");
		sStPars = Lang.GetString("_START_DISARMAMENT");
		sStUpgNot = Lang.GetString("_START_UPGRADE_NOT");
		sSaleWill = Lang.GetString("_SALE_WILL_BE");
		sStSale = Lang.GetString("_START_SALE");
		sContinue = Lang.GetString("_CONTINUE");
		sCancel = Lang.GetString("_CANCEL");
	}

	public static void SetActive(bool val)
	{
		show = val;
		if (show)
		{
			GUIFX.Set();
			Reset();
		}
		else
		{
			GUIFX.End();
		}
		craftend = false;
		if (craftcase != null)
		{
			craftcase = null;
		}
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		tYellow = TEX.GetTextureByName("yellow");
		tOrange = TEX.GetTextureByName("orange");
		tArrowR = TEX.GetTextureByName("arrow_r");
		process = Resources.Load("process") as Texture2D;
		tAngle = Resources.Load("pangle") as Texture2D;
	}

	private void OnResize()
	{
		rBack.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(624f), GUIM.YRES(160f));
		rBackHint.Set(rBack.x, rBack.y + rBack.height - GUIM.YRES(24f), rBack.width, GUIM.YRES(24f));
		rArrow.Set(rBack.x + rBack.width - GUIM.YRES(56f), rBack.y + GUIM.YRES(48f), GUIM.YRES(40f), GUIM.YRES(40f));
		rRes.Set(rBack.x + GUIM.YRES(632f), rBack.y, GUIM.YRES(288f), GUIM.YRES(160f));
		rButton.Set(rRes.x + GUIM.YRES(8f), rRes.y + rRes.height - GUIM.YRES(32f), rRes.width - GUIM.YRES(16f), GUIM.YRES(24f));
		rBack2.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(316f), GUIM.YRES(920f), GUIM.YRES(324f));
		rScroll.Set(rBack2.x + GUIM.YRES(8f), rBack2.y + GUIM.YRES(8f), rBack2.width - GUIM.YRES(16f), rBack2.height - GUIM.YRES(16f));
		for (int i = 0; i < 3; i++)
		{
			rItem[i].Set(rBack.x + GUIM.YRES(64f) + GUIM.YRES(168f) * (float)i, rBack.y + GUIM.YRES(28f), GUIM.YRES(160f), GUIM.YRES(80f));
			rSteamHint[i].Set(rItem[i].x, rItem[i].y + rItem[i].height + GUIM.YRES(8f), rItem[i].width, GUIM.YRES(24f));
		}
	}

	private void OnChangeSubMenu()
	{
		if (show)
		{
			Reset();
		}
	}

	private static void Reset()
	{
		Drop(0);
		Drop(1);
		Drop(2);
		tStart = 0f;
		craftend = false;
	}

	private void OnGUI()
	{
		if (!show)
		{
			return;
		}
		GUIFX.Begin();
		if (Main.currSubMenu == 0)
		{
			Draw3();
			DrawHint();
			DrawResult();
			DrawWeapons();
		}
		else if (Main.currSubMenu == 1)
		{
			Draw1();
			DrawHintSell();
			DrawCases();
			DrawResultSell();
			if (!caseinforeq)
			{
				caseinforeq = true;
				MasterClient.cs.send_casesellinfo();
			}
		}
		else if (Main.currSubMenu == 2)
		{
			Draw3();
			DrawHintUpgrade();
			DrawResultUpgrade();
		}
		GUIFX.End();
	}

	private void Draw3()
	{
		GUIM.DrawBox(rBack, tBlack, 0.05f);
		for (int i = 0; i < 3; i++)
		{
			if (GUIM.Contains(rItem[i]))
			{
				GUIM.DrawBox(rItem[i], tWhite, 0.05f);
			}
			else
			{
				GUIM.DrawBox(rItem[i], tBlack, 0.05f);
			}
			if (Item[i] != null)
			{
				if (tStart != 0f || clicklock)
				{
					GUI.color = Color.black;
				}
				GUIInv.DrawWeaponIcon(rItem[i], Item[i].wi.tIcon, Item[i].wi.vIcon);
				if (GUIM.HideButton(rItem[i]) && !clicklock)
				{
					Drop(i);
				}
			}
		}
		GUI.color = new Color(0f, 0f, 0f, 0.3f);
		GUI.DrawTexture(rArrow, tArrowR);
		GUI.color = Color.white;
	}

	private void Draw1()
	{
		GUIM.DrawBox(rBack, tBlack, 0.05f);
		for (int i = 0; i < 1; i++)
		{
			if (GUIM.Contains(rItem[i]))
			{
				GUIM.DrawBox(rItem[i], tWhite, 0.05f);
			}
			else
			{
				GUIM.DrawBox(rItem[i], tBlack, 0.05f);
			}
			if (caseItem[i] != null)
			{
				if (tStart != 0f || clicklock)
				{
					GUI.color = Color.black;
				}
				GUI.DrawTexture(new Rect(rItem[i].x + (rItem[i].width - rItem[i].height) / 2f, rItem[i].y, rItem[i].height, rItem[i].height), caseItem[i].ci.img);
				if (GUIM.HideButton(rItem[i]) && !clicklock)
				{
					Drop(i);
				}
			}
		}
		GUI.color = new Color(0f, 0f, 0f, 0.3f);
		GUI.DrawTexture(rArrow, tArrowR);
		GUI.color = Color.white;
	}

	private void DrawHint()
	{
		GUI.DrawTexture(rBackHint, tBlack);
		GUIM.DrawText(rBackHint, MainManager.steam ? sParseCaseSteam : sParseCase, TextAnchor.MiddleCenter, BaseColor.White, 0, 16, false);
	}

	private void DrawHintUpgrade()
	{
		GUI.DrawTexture(rBackHint, tBlack);
		GUIM.DrawText(rBackHint, sParseWeap, TextAnchor.MiddleCenter, BaseColor.White, 0, 16, false);
	}

	private void DrawHintSell()
	{
		GUI.DrawTexture(rBackHint, tBlack);
		if (caseItem[0] != null && caseItem[0].ci.idx == 5)
		{
			GUIM.DrawText(rBackHint, sSale2, TextAnchor.MiddleCenter, BaseColor.White, 0, 16, false);
		}
		else if (caseItem[0] != null && caseItem[0].ci.idx == 6)
		{
			GUIM.DrawText(rBackHint, sSale3, TextAnchor.MiddleCenter, BaseColor.White, 0, 16, false);
		}
		else
		{
			GUIM.DrawText(rBackHint, sSale1, TextAnchor.MiddleCenter, BaseColor.White, 0, 16, false);
		}
	}

	private void DrawResult()
	{
		bool flag = false;
		if (Item[0] != null && Item[1] != null && Item[2] != null)
		{
			flag = true;
		}
		GUIM.DrawBox(rRes, tBlack, 0.05f);
		if (tStart != 0f && Time.time > tStart + crafttime)
		{
			tStart = 0f;
			if (MainManager.steam)
			{
				MasterClient.cs.send_casecraft_steam(Item[0].uid, Item[1].uid, Item[2].uid);
			}
			else
			{
				MasterClient.cs.send_casecraft((int)Item[0].uid, (int)Item[1].uid, (int)Item[2].uid);
			}
			clicklock = true;
		}
		if (clicklock)
		{
			GUIM.DrawText(rButton, Lang.GetString("_WAITING_SERVER"), TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
		}
		else if (craftend)
		{
			if (GUIM.Button(rButton, BaseColor.White, Lang.GetString("_CONTINUE"), TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				craftend = false;
				if (craftcase != null)
				{
					craftcase = null;
				}
			}
		}
		else if (tStart != 0f)
		{
			float num = 1f - (tStart + crafttime - Time.time) / crafttime;
			GUI.DrawTexture(new Rect(rButton.x, rButton.y - GUIM.YRES(2f), rButton.width * num, GUIM.YRES(2f)), tOrange);
			if (GUIM.Button(rButton, BaseColor.White, sCancel, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				tStart = 0f;
			}
		}
		else if (GUIM.Button(rButton, flag ? BaseColor.White : BaseColor.Gray, sStPars, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			if (!flag)
			{
				return;
			}
			craftend = false;
			tStart = Time.time;
		}
		CraftDraw();
	}

	private void DrawResultUpgrade()
	{
		GUIM.DrawBox(rRes, tBlack, 0.05f);
		GUIM.Button(rButton, BaseColor.Block, sStUpgNot, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
	}

	private void DrawResultSell()
	{
		bool flag = false;
		if (caseItem[0] != null)
		{
			flag = true;
		}
		GUIM.DrawBox(rRes, tBlack, 0.05f);
		if (tStart != 0f && Time.time > tStart + selltime)
		{
			tStart = 0f;
			MasterClient.cs.send_casesell((int)caseItem[0].uid);
			clicklock = true;
		}
		if (clicklock)
		{
			GUIM.DrawText(rButton, sWaitServ, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
		}
		else if (craftend)
		{
			if (GUIM.Button(rButton, BaseColor.White, sContinue, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				craftend = false;
			}
		}
		else if (tStart != 0f)
		{
			float num = 1f - (tStart + selltime - Time.time) / selltime;
			GUI.DrawTexture(new Rect(rButton.x, rButton.y - GUIM.YRES(2f), rButton.width * num, GUIM.YRES(2f)), tOrange);
			if (GUIM.Button(rButton, BaseColor.White, sCancel, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				tStart = 0f;
			}
		}
		else if (cs_time != 0f)
		{
			int num2 = cs_sec - (int)(Time.time - cs_time);
			if (num2 <= 0)
			{
				cs_time = 0f;
			}
			else if (num2 > 0)
			{
				int num3 = num2 / 3600;
				int num4 = (num2 - num3 * 3600) / 60;
				int num5 = num2 - num3 * 3600 - num4 * 60;
				GUIM.Button(rButton, BaseColor.Gray, sSaleWill + " " + num3.ToString("00") + ":" + num4.ToString("00") + ":" + num5.ToString("00"), TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			}
		}
		else if (GUIM.Button(rButton, flag ? BaseColor.White : BaseColor.Gray, sStSale, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			if (!flag)
			{
				return;
			}
			craftend = false;
			tStart = Time.time;
		}
		SellDraw();
	}

	private void DrawWeapons()
	{
		GUIM.DrawBox(rBack2, tBlack, 0.05f);
		scroll = GUIM.BeginScrollView(rScroll, scroll, new Rect(0f, 0f, 0f, GUIM.YRES(68f) * (float)(int)((float)GUIInv.wlist.Count / 7f + 0.5f)));
		if (MainManager.steam)
		{
			it = null;
			if (it == null && Item[0] != null)
			{
				it = Item[0];
			}
			if (it == null && Item[1] != null)
			{
				it = Item[1];
			}
			if (it == null && Item[2] != null)
			{
				it = Item[2];
			}
		}
		int num = 0;
		for (int i = 0; i < GUIInv.wlist.Count; i++)
		{
			if (MainManager.steam)
			{
				if (GUIInv.wlist[i].wi.slot >= 3 || (it != null && GUIInv.wlist[i].wi.id != it.wi.id))
				{
					continue;
				}
			}
			else if (GUIInv.wlist[i].dublicate == 0)
			{
				continue;
			}
			int num2 = num / 7;
			int num3 = num - num2 * 7;
			Rect r = new Rect(GUIM.YRES(128 * num3), GUIM.YRES(68 * num2), GUIM.YRES(120f), GUIM.YRES(60f));
			DrawWeapon(r, GUIInv.wlist[i]);
			num++;
		}
		GUIM.EndScrollView();
	}

	private void DrawCases()
	{
		GUIM.DrawBox(rBack2, tBlack, 0.05f);
		scroll = GUIM.BeginScrollView(rScroll, scroll, new Rect(0f, 0f, 0f, GUIM.YRES(68f) * (float)(int)((float)GUIInv.clist.Count / 7f + 0.5f)));
		int num = 0;
		for (int i = 0; i < GUIInv.clist.Count; i++)
		{
			if (GUIInv.clist[i].ci.idx >= 4)
			{
				int num2 = num / 7;
				int num3 = num - num2 * 7;
				Rect r = new Rect(GUIM.YRES(128 * num3), GUIM.YRES(68 * num2), GUIM.YRES(120f), GUIM.YRES(60f));
				DrawCase(r, GUIInv.clist[i]);
				num++;
			}
		}
		GUIM.EndScrollView();
	}

	private bool DrawCase(Rect r, CaseInv c, bool smooth = false, bool hidename = false)
	{
		bool result = false;
		if (c == null)
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
			if (c == caseItem[0] || c == caseItem[1] || c == caseItem[2])
			{
				GUI.color = new Color(0f, 0f, 0f, 0.5f);
			}
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
		CaseInfo ci = c.ci;
		GUI.DrawTexture(new Rect(r.x + (r.width - r.height) / 2f, r.y + GUIM.YRES(5f), r.height, r.height), ci.img);
		if (!hidename)
		{
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(4f), r.y + GUIM.YRES(4f), r.width, r.height), c.ci.fullname, TextAnchor.UpperLeft, BaseColor.White, 1, 12, false);
		}
		if (show && GUIM.HideButton(r))
		{
			Add(c);
		}
		return result;
	}

	public bool DrawWeapon(Rect r, WeaponInv w, bool smooth = false, bool hidename = false)
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
			if (w == Item[0] || w == Item[1] || w == Item[2])
			{
				GUI.color = new Color(0f, 0f, 0f, 0.5f);
			}
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
		WeaponInfo wi = w.wi;
		GUIInv.DrawWeaponIcon(new Rect(r.x, r.y + GUIM.YRES(2f), r.width, r.height), wi.tIcon, wi.vIcon);
		if (!hidename)
		{
			GUIInv.DrawWeaponText(r, w);
		}
		if (show && GUIM.HideButton(r))
		{
			Add(w);
		}
		Rect position = new Rect(r.x + r.width - GUIM.YRES(10f), r.y + r.height - GUIM.YRES(10f), GUIM.YRES(10f), GUIM.YRES(10f));
		if (wi.p == 0)
		{
			GUI.color = Color.white;
		}
		else if (wi.p == 1)
		{
			GUI.color = Color.yellow;
		}
		else if (wi.p == 2)
		{
			GUI.color = GUIM.colorlist[7];
		}
		if (wi.p >= 0)
		{
			GUI.DrawTexture(position, tAngle);
			GUI.color = Color.white;
		}
		return result;
	}

	private void Add(WeaponInv w)
	{
		if (Main.currSubMenu == 0)
		{
			for (int i = 0; i < 3; i++)
			{
				if (Item[i] == w)
				{
					return;
				}
			}
			for (int j = 0; j < 3; j++)
			{
				if (Item[j] == null)
				{
					Item[j] = w;
					break;
				}
			}
		}
		else if (Main.currSubMenu != 1)
		{
			int currSubMenu = Main.currSubMenu;
			int num = 2;
		}
	}

	private void Add(CaseInv c)
	{
		if (Main.currSubMenu == 1 && caseItem[0] == null)
		{
			caseItem[0] = c;
		}
	}

	public static void Drop(int slot)
	{
		if (Item[slot] != null)
		{
			Item[slot] = null;
			tStart = 0f;
		}
		if (caseItem[slot] != null)
		{
			caseItem[slot] = null;
			tStart = 0f;
		}
	}

	public static void CraftEnd(int cid)
	{
		GUI3D.SetFX(1);
		craftend = true;
		craftcase = new CaseInv(0uL, GUIInv.cinfo[cid]);
		Item[0] = null;
		Item[1] = null;
		Item[2] = null;
		clicklock = false;
	}

	private void CraftDraw()
	{
		if (craftend)
		{
			GUI.DrawTexture(new Rect(rRes.x + GUIM.YRES(90f), rRes.y + GUIM.YRES(16f), GUIM.YRES(108f), GUIM.YRES(108f)), craftcase.ci.img);
			GUIM.DrawText(new Rect(rRes.x, rRes.y + GUIM.YRES(8f), rRes.width, GUIM.YRES(20f)), craftcase.ci.fullname, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
		}
	}

	public static void SellEnd()
	{
		craftend = true;
		caseItem[0] = null;
		clicklock = false;
	}

	private void SellDraw()
	{
		if (craftend)
		{
			GUI.DrawTexture(new Rect(rRes.x + GUIM.YRES(112f), rRes.y + GUIM.YRES(38f), GUIM.YRES(64f), GUIM.YRES(64f)), Main.tCoin);
		}
	}
}
