using System.Collections.Generic;
using Player;
using UnityEngine;

public class GUIShop : MonoBehaviour
{
	public static bool show = false;

	private static Texture2D tBlack = null;

	private static Texture2D tWhite = null;

	private static Texture2D tGray = null;

	private static Texture2D tYellow = null;

	private static Texture2D tReq = null;

	private Rect rBack;

	private Rect rDesc;

	private static Rect rScroll;

	private static Rect rZoneAll;

	private static Rect[] rZone = null;

	private Rect rBackCheck;

	private Rect rScrollCheck;

	private static Vector2 scroll = Vector2.zero;

	private static Color c8 = new Color(1f, 1f, 1f, 0.65f);

	public static List<CaseInv> cshop = new List<CaseInv>();

	public static List<KeyInv> kshop = new List<KeyInv>();

	public static List<PlayerSkinInv> psshop = new List<PlayerSkinInv>();

	public static CaseInv caseselect = null;

	public static KeyInv keyselect = null;

	public static PlayerSkinInv psselect = null;

	public static bool inbuy = false;

	public static bool inlock = false;

	public static bool inOverlay = false;

	public static int buycode = -1;

	public static string[] buymsg = new string[3];

	private static string sCaseContain;

	private static string sCaseWorhshop;

	private static string sCaseJob;

	private static string sAvailCase;

	private static string sSupplyThrough;

	private static string sKeyOpen;

	private static string sCostumeCombine;

	private static string sOpen;

	private string sCost;

	private string sBuyMore;

	private string sGoto;

	private string sNotCoin;

	private string sFillCoin;

	private string sCoin;

	private string sContinue;

	private string sCancel;

	private static string sBuy;

	public void LoadLang()
	{
		buymsg[0] = Lang.GetString("_WAITING_CONFIRM");
		buymsg[1] = Lang.GetString("_THANK_YOU_PURCHASE");
		buymsg[2] = Lang.GetString("_TRY_AGAIN");
		sCaseContain = Lang.GetString("_CASE_CONTAIN_SUBJECTS");
		sCaseWorhshop = Lang.GetString("_CASE_IN_WORKSHOP");
		sCaseJob = Lang.GetString("_CASE_JOB");
		sAvailCase = Lang.GetString("_AVAILABLE_CASE");
		sSupplyThrough = Lang.GetString("_SUPPLY THROUGH");
		sKeyOpen = Lang.GetString("_KEY_ONE_OPEN");
		sCostumeCombine = Lang.GetString("_COSTUME_CAN_COMBINED");
		sOpen = Lang.GetString("_OPEN");
		sCost = Lang.GetString("_COST");
		sBuyMore = Lang.GetString("_BUY_MORE");
		sGoto = Lang.GetString("_GOTO");
		sNotCoin = Lang.GetString("_NOT_MISS_COIN");
		sFillCoin = Lang.GetString("_FILL_COINS");
		sCoin = Lang.GetString("_COIN");
		sContinue = Lang.GetString("_CONTINUE");
		sCancel = Lang.GetString("_REPEAL");
		sBuy = Lang.GetString("_BUY");
	}

	public static void SetActive(bool val)
	{
		show = val;
		if (show)
		{
			GUIFX.Set();
			cshop.Clear();
			kshop.Clear();
			psshop.Clear();
			for (int i = 0; i < 1024; i++)
			{
				if (GUIInv.cinfo[i] != null && GUIInv.cinfo[i].shop != 0)
				{
					cshop.Add(new CaseInv(0uL, GUIInv.cinfo[i]));
				}
			}
			for (int j = 0; j < 1024; j++)
			{
				if (GUIInv.kinfo[j] != null && GUIInv.kinfo[j].shop != 0)
				{
					kshop.Add(new KeyInv(0uL, GUIInv.kinfo[j]));
				}
			}
			for (int k = 0; k < GUIInv.psinfo.Length; k++)
			{
				if (GUIInv.psinfo[k] != null && GUIInv.psinfo[k].shop != 0)
				{
					psshop.Add(new PlayerSkinInv(0, GUIInv.psinfo[k]));
				}
			}
			buycode = -1;
			MasterClient.cs.send_shop();
			OnResize_Zone();
		}
		else
		{
			GUIFX.End();
		}
		caseselect = null;
		keyselect = null;
		inbuy = false;
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		tGray = TEX.GetTextureByName("gray");
		tYellow = TEX.GetTextureByName("orange");
		tReq = Resources.Load("req_icon") as Texture2D;
	}

	private void OnResize()
	{
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(360f));
		rDesc = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f) + GUIM.YRES(376f), GUIM.YRES(920f), GUIM.YRES(124f));
		rScroll = new Rect(rBack.x + GUIM.YRES(8f), rBack.y + GUIM.YRES(8f), GUIM.YRES(904f), rBack.height - GUIM.YRES(16f));
		rBackCheck = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(186f));
		rScrollCheck = new Rect(rBackCheck.x + GUIM.YRES(8f), rBackCheck.y + GUIM.YRES(8f), GUIM.YRES(904f), rBackCheck.height - GUIM.YRES(16f));
		OnResize_Zone();
	}

	private static void OnResize_Zone()
	{
		if (rZone == null)
		{
			rZone = new Rect[3];
		}
		float num = GUIM.YRES(170f) + GUIM.YRES(8f);
		int num2 = (cshop.Count + kshop.Count + psshop.Count + 4) / 5;
		int num3 = (cshop.Count + 4) / 5;
		int num4 = (kshop.Count + 4) / 5;
		int num5 = (psshop.Count + 4) / 5;
		rZoneAll = new Rect(0f, 0f, 0f, num * (float)num2 - GUIM.YRES(8f));
		rZone[0] = new Rect(0f, 0f, 0f, num * (float)num3 - GUIM.YRES(8f));
		rZone[1] = new Rect(0f, 0f, 0f, num * (float)num4 - GUIM.YRES(8f));
		rZone[2] = new Rect(0f, 0f, 0f, num * (float)num5 - GUIM.YRES(8f));
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIFX.Begin();
			if (inbuy)
			{
				DrawCheckOut();
			}
			else
			{
				DrawShop();
			}
			GUIFX.End();
		}
	}

	private void DrawShop()
	{
		GUIM.DrawBox(rBack, tBlack, 0.05f);
		DrawDescription(rDesc, caseselect, keyselect, psselect);
		float num = GUIM.YRES(170f);
		float num2 = num + GUIM.YRES(8f);
		scroll = GUIM.BeginScrollView(rScroll, scroll, (Main.currSubMenu < 0) ? rZoneAll : rZone[Main.currSubMenu]);
		int num3 = 0;
		if (Main.currSubMenu == 0 || Main.currSubMenu < 0)
		{
			for (int i = 0; i < cshop.Count; i++)
			{
				int num4 = i / 5;
				int num5 = i - num4 * 5;
				DrawItemCase(new Rect(num2 * (float)num5, num2 * (float)num4, num, num), cshop[i], rScroll, scroll);
			}
			num3 = cshop.Count;
		}
		if (Main.currSubMenu == 1 || Main.currSubMenu < 0)
		{
			for (int j = 0; j < kshop.Count; j++)
			{
				int num6 = (j + num3) / 5;
				int num7 = j + num3 - num6 * 5;
				DrawItemKey(new Rect(num2 * (float)num7, num2 * (float)num6, num, num), kshop[j]);
			}
			num3 += kshop.Count;
		}
		if (Main.currSubMenu == 2 || Main.currSubMenu < 0)
		{
			for (int k = 0; k < psshop.Count; k++)
			{
				int num8 = (k + num3) / 5;
				int num9 = k + num3 - num8 * 5;
				DrawItemPlayerSkin(new Rect(num2 * (float)num9, num2 * (float)num8, num, num), psshop[k]);
			}
		}
		GUIM.EndScrollView();
	}

	public static void DrawDescription(Rect r, CaseInv c, KeyInv k, PlayerSkinInv ps)
	{
		GUIM.DrawBox(r, tBlack, 0.05f);
		if (c != null)
		{
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y + GUIM.YRES(10f), r.width - GUIM.YRES(32f), GUIM.YRES(22f)), sCaseContain, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
			for (int i = 0; i < c.ci.weaponcount; i++)
			{
				BaseColor fontcolor = BaseColor.White;
				if (c.ci.weapon[i].p == 1)
				{
					fontcolor = BaseColor.Yellow;
				}
				else if (c.ci.weapon[i].p == 2)
				{
					fontcolor = BaseColor.Orange;
				}
				GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f) + (float)(i / 4) * GUIM.YRES(200f), r.y + GUIM.YRES(10f) + GUIM.YRES(22f) * (float)(i - i / 4 * 4 + 1), r.width - GUIM.YRES(32f), GUIM.YRES(22f)), c.ci.weapon[i].fullname, TextAnchor.MiddleLeft, fontcolor, 0, 20, false);
			}
			if (c.count != null)
			{
				if (c.ci.idx == 4 || c.ci.idx == 5 || c.ci.idx == 6)
				{
					GUIM.DrawText(new Rect(r.x + r.width - GUIM.YRES(216f), r.y + GUIM.YRES(10f), GUIM.YRES(200f), GUIM.YRES(22f)), sCaseWorhshop, TextAnchor.MiddleRight, BaseColor.Green, 0, 20, false);
				}
				else if (c.ci.idx == 9 || c.ci.idx == 10 || c.ci.idx == 11 || c.ci.idx == 12 || c.ci.idx == 13 || c.ci.idx == 14 || c.ci.idx == 15 || c.ci.idx == 16)
				{
					GUIM.DrawText(new Rect(r.x + r.width - GUIM.YRES(216f), r.y + GUIM.YRES(10f), GUIM.YRES(200f), GUIM.YRES(22f)), sCaseJob, TextAnchor.MiddleRight, BaseColor.Green, 0, 20, false);
				}
				if (c.ci.idx == 16)
				{
					GUIM.DrawText(new Rect(r.x + r.width - GUIM.YRES(216f), r.y + GUIM.YRES(98f), GUIM.YRES(200f), GUIM.YRES(22f)), sAvailCase + " " + c.count, TextAnchor.MiddleRight, BaseColor.Green, 0, 20, false);
					int num = c.sec - (int)(Time.time - c.time);
					int num2 = num / 3600;
					int num3 = (num - num2 * 3600) / 60;
					int num4 = num - num2 * 3600 - num3 * 60;
					string text = sSupplyThrough + " " + num2.ToString("00") + ":" + num3.ToString("00") + ":" + num4.ToString("00");
					GUIM.DrawText(new Rect(r.x + r.width - GUIM.YRES(216f), r.y + GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(22f)), text, TextAnchor.MiddleRight, BaseColor.White, 0, 20, false);
				}
			}
		}
		else if (k != null)
		{
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y + GUIM.YRES(10f), r.width - GUIM.YRES(32f), GUIM.YRES(22f)), sKeyOpen, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		}
		else if (ps != null)
		{
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y + GUIM.YRES(10f), r.width - GUIM.YRES(32f), GUIM.YRES(22f)), sCostumeCombine, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		}
	}

	public static void DrawCamHack()
	{
		Main.cam.cullingMask = 512;
		Main.cam.clearFlags = CameraClearFlags.Depth;
		Main.cam.farClipPlane = 18f;
		Main.cam.orthographic = false;
		Main.cam.fieldOfView = 30f;
		Main.cam.transform.position = new Vector3(0f, 0f, -10f);
		Main.cam.Render();
		Main.cam.clearFlags = CameraClearFlags.Color;
		Main.cam.farClipPlane = 1000f;
		Main.cam.cullingMask = 256;
		Main.cam.orthographic = true;
		Main.cam.transform.position = Vector3.zero;
	}

	private void OnChangeSubMenu()
	{
		if (show)
		{
			caseselect = null;
			keyselect = null;
			inbuy = false;
			buycode = -1;
			scroll = Vector2.zero;
		}
	}

	public static bool DrawItemCase(Rect r, CaseInv c, Rect rScroll, Vector2 pos)
	{
		if (caseselect == c)
		{
			GUIM.DrawBox(r, tYellow, 0.2f);
		}
		else if (GUIM.Contains(r))
		{
			GUIM.DrawBox(r, tWhite, 0.05f);
		}
		else
		{
			GUIM.DrawBox(r, tBlack, 0.05f);
		}
		int num = (int)GUIM.YRES(20f);
		Rect rect = new Rect(r.x + (float)num, r.y + (float)num * 0.5f, r.width - (float)(num * 2), r.height - (float)(num * 2));
		GUI.DrawTexture(rect, c.ci.img);
		if (c.ci.idx == 20)
		{
			Rect position = new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(108f), GUIM.YRES(20f), GUIM.YRES(20f));
			Rect r2 = new Rect(r.x + GUIM.YRES(26f), r.y + GUIM.YRES(107f), GUIM.YRES(40f), GUIM.YRES(24f));
			GUI.color = Color.red;
			GUI.DrawTexture(position, tReq);
			GUI.color = Color.white;
			GUIM.DrawText(r2, "<b>10 LVL</b>", TextAnchor.MiddleCenter, BaseColor.Red, 0, 20, false);
		}
		if (c.count != null)
		{
			Rect r3 = new Rect(r.x + GUIM.YRES(4f), r.y + GUIM.YRES(4f), GUIM.YRES(32f), GUIM.YRES(20f));
			GUIM.DrawBox(r3, tBlack);
			GUIM.DrawText(r3, c.count, TextAnchor.MiddleCenter, (c.icount == 0) ? BaseColor.Red : BaseColor.Green, 0, 20, false);
			if (c.icount == 0 && c.ci.idx != 4 && c.ci.idx != 5 && c.ci.idx != 6 && c.ci.idx != 9 && c.ci.idx != 10 && c.ci.idx != 11 && c.ci.idx != 12 && c.ci.idx != 13 && c.ci.idx != 14 && c.ci.idx != 15 && c.ci.idx != 16)
			{
				int num2 = c.sec - (int)(Time.time - c.time);
				int num3 = num2 / 3600;
				int num4 = (num2 - num3 * 3600) / 60;
				int num5 = num2 - num3 * 3600 - num4 * 60;
				string text = num3.ToString("00") + ":" + num4.ToString("00") + ":" + num5.ToString("00");
				if (num2 < 0)
				{
					text = "";
				}
				GUI.color = new Color(0f, 0f, 0f, 0.85f);
				GUI.DrawTexture(rect, c.ci.img);
				GUI.color = Color.white;
				GUIM.DrawText(rect, text, TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, true);
			}
		}
		GUIM.DrawTextColor(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(8f), r.width - GUIM.YRES(16f), GUIM.YRES(20f)), c.ci.fullname, TextAnchor.UpperRight, BaseColor.White, 1, 14, true);
		if (GUIM.Contains(r) && Input.GetMouseButton(0))
		{
			caseselect = c;
			keyselect = null;
			psselect = null;
		}
		if (c.uid != 0L)
		{
			if (GUIM.Button(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(36f), r.width - GUIM.YRES(16f), GUIM.YRES(28f)), BaseColor.White, sOpen, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				return true;
			}
			return false;
		}
		if (MainManager.steam)
		{
			if (c.ci.idx <= 2)
			{
				GUI.color = c8;
				GUI.DrawTexture(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(60f), GUIM.YRES(28f)), tBlack);
				GUI.color = Color.white;
				GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(32f), GUIM.YRES(60f), GUIM.YRES(28f)), c.ci.scost, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
				if (!inbuy)
				{
					if (c.count != null && c.icount == 0)
					{
						GUIM.Button(new Rect(r.x + r.width - GUIM.YRES(88f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(80f), GUIM.YRES(28f)), BaseColor.Gray, sBuy, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
					}
					else if (GUIM.Button(new Rect(r.x + r.width - GUIM.YRES(88f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(80f), GUIM.YRES(28f)), BaseColor.White, sBuy, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
					{
						inbuy = true;
					}
				}
			}
			else
			{
				Rect rect2 = new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(36f), r.width - GUIM.YRES(16f), GUIM.YRES(28f));
				GUI.color = c8;
				GUI.DrawTexture(rect2, tBlack);
				GUI.color = Color.white;
				if (!GUIM.Button(rect2, BaseColor.White, "GO TO MARKET", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
				{
				}
			}
		}
		else
		{
			if (!inbuy)
			{
				if (c.count != null && c.icount == 0)
				{
					GUIM.Button(new Rect(r.x + r.width - GUIM.YRES(88f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(80f), GUIM.YRES(28f)), BaseColor.Gray, sBuy, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
				}
				else if (GUIM.Button(new Rect(r.x + r.width - GUIM.YRES(88f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(80f), GUIM.YRES(28f)), BaseColor.White, sBuy, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
				{
					inbuy = true;
				}
			}
			GUI.color = c8;
			GUI.DrawTexture(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(60f), GUIM.YRES(28f)), tBlack);
			GUI.color = Color.white;
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(32f), GUIM.YRES(40f), GUIM.YRES(28f)), c.ci.scost, TextAnchor.MiddleCenter, BaseColor.White, 1, 20, false);
			GUI.DrawTexture(new Rect(r.x + GUIM.YRES(40f), r.y + r.height - GUIM.YRES(32f), GUIM.YRES(20f), GUIM.YRES(20f)), Main.tCoin);
		}
		return false;
	}

	public static void DrawItemKey(Rect r, KeyInv k)
	{
		if (keyselect == k)
		{
			GUIM.DrawBox(r, tYellow, 0.2f);
		}
		else if (GUIM.Contains(r))
		{
			GUIM.DrawBox(r, tWhite, 0.05f);
		}
		else
		{
			GUIM.DrawBox(r, tBlack, 0.05f);
		}
		GUI.DrawTexture(new Rect(r.x + GUIM.YRES(36f), r.y + GUIM.YRES(24f), r.width - GUIM.YRES(72f), r.height - GUIM.YRES(72f)), k.ki.img);
		GUIM.DrawTextColor(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(8f), r.width - GUIM.YRES(16f), GUIM.YRES(20f)), k.ki.fullname, TextAnchor.UpperRight, BaseColor.White, 1, 12, true);
		if (!inbuy && GUIM.Button(new Rect(r.x + r.width - GUIM.YRES(88f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(80f), GUIM.YRES(28f)), BaseColor.White, sBuy, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			inbuy = true;
		}
		GUI.color = c8;
		GUI.DrawTexture(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(60f), GUIM.YRES(28f)), tBlack);
		GUI.color = Color.white;
		if (MainManager.steam)
		{
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(32f), GUIM.YRES(60f), GUIM.YRES(28f)), k.ki.scost, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
		}
		else
		{
			GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(32f), GUIM.YRES(40f), GUIM.YRES(28f)), k.ki.scost, TextAnchor.MiddleCenter, BaseColor.White, 1, 20, false);
			GUI.DrawTexture(new Rect(r.x + GUIM.YRES(40f), r.y + r.height - GUIM.YRES(32f), GUIM.YRES(20f), GUIM.YRES(20f)), Main.tCoin);
		}
		if (GUIM.Contains(r) && Input.GetMouseButton(0))
		{
			keyselect = k;
			caseselect = null;
			psselect = null;
		}
	}

	public static void DrawItemPlayerSkin(Rect r, PlayerSkinInv ps)
	{
		if (psselect == ps)
		{
			GUIM.DrawBox(r, tYellow, 0.2f);
		}
		else if (GUIM.Contains(r))
		{
			GUIM.DrawBox(r, tWhite, 0.05f);
		}
		else
		{
			GUIM.DrawBox(r, tBlack, 0.05f);
		}
		Rect position = new Rect(r.x + GUIM.YRES(26f), r.y + GUIM.YRES(10f), r.width - GUIM.YRES(52f), r.height - GUIM.YRES(52f));
		if (ps.psi.img != null)
		{
			GUI.DrawTexture(position, ps.psi.img);
		}
		GUIM.DrawTextColor(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(8f), r.width - GUIM.YRES(16f), GUIM.YRES(20f)), ps.psi.fullname, TextAnchor.UpperRight, BaseColor.White, 1, 12, true);
		if (!inbuy && GUIM.Button(new Rect(r.x + r.width - GUIM.YRES(88f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(80f), GUIM.YRES(28f)), BaseColor.White, sBuy, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			inbuy = true;
		}
		GUI.color = c8;
		GUI.DrawTexture(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(36f), GUIM.YRES(60f), GUIM.YRES(28f)), tBlack);
		GUI.color = Color.white;
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + r.height - GUIM.YRES(32f), GUIM.YRES(40f), GUIM.YRES(28f)), ps.psi.scost, TextAnchor.MiddleCenter, BaseColor.White, 1, 20, false);
		GUI.DrawTexture(new Rect(r.x + GUIM.YRES(40f), r.y + r.height - GUIM.YRES(32f), GUIM.YRES(20f), GUIM.YRES(20f)), Main.tCoin);
		if (GUIM.Contains(r) && Input.GetMouseButton(0))
		{
			psselect = ps;
			caseselect = null;
			keyselect = null;
		}
	}

	private void DrawCheckOut()
	{
		int num = 9999;
		int num2 = 0;
		int num3 = 0;
		GUIM.DrawBox(rBackCheck, tBlack, 0.05f);
		float num4 = GUIM.YRES(170f);
		scroll = GUIM.BeginScrollView(rScroll, scroll, new Rect(0f, 0f, 0f, 0f));
		Rect r = new Rect(0f, 0f, num4, num4);
		if (caseselect != null)
		{
			DrawItemCase(r, caseselect, rScroll, scroll);
			num = caseselect.ci.cost;
			num2 = 1;
			num3 = caseselect.ci.idx;
		}
		else if (keyselect != null)
		{
			DrawItemKey(r, keyselect);
			num = keyselect.ki.cost;
			num2 = 2;
			num3 = keyselect.ki.idx;
		}
		else
		{
			if (psselect == null)
			{
				return;
			}
			DrawItemPlayerSkin(r, psselect);
			num = psselect.psi.cost;
			num2 = 3;
			num3 = psselect.psi.idx;
		}
		GUIM.EndScrollView();
		Rect r2 = new Rect(rBackCheck.x + rBackCheck.width - GUIM.YRES(200f), rBackCheck.y + GUIM.YRES(8f), GUIM.YRES(200f), GUIM.YRES(28f));
		if (MainManager.steam)
		{
			if (caseselect != null)
			{
				GUIM.DrawText(r2, sCost + " " + caseselect.ci.scost, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 14, false);
			}
			else if (keyselect != null)
			{
				GUIM.DrawText(r2, sCost + " " + keyselect.ki.scost, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 14, false);
			}
			else
			{
				GUIM.DrawText(r2, sCost + " " + num + " " + sCoin, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 14, false);
			}
			if (caseselect != null || keyselect != null)
			{
				if (GUIM.Button(new Rect(rBackCheck.x + rBackCheck.width - GUIM.YRES(200f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f), GUIM.YRES(60f), GUIM.YRES(28f)), BaseColor.Block, sCancel, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
				{
					inbuy = false;
				}
				if (GUIM.Button(new Rect(rBackCheck.x + rBackCheck.width - GUIM.YRES(132f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f), GUIM.YRES(124f), GUIM.YRES(28f)), BaseColor.White, sContinue, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && !inlock)
				{
					if (caseselect != null)
					{
						int idx = caseselect.ci.idx;
					}
					if (keyselect != null)
					{
						int idx2 = keyselect.ki.idx;
					}
					inOverlay = true;
				}
				return;
			}
		}
		else
		{
			GUIM.DrawText(r2, sCost + " " + num + " " + sCoin, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 14, false);
		}
		if (buycode >= 0)
		{
			if (psselect == null && GUIM.Button(new Rect(rBackCheck.x + rBackCheck.width - GUIM.YRES(188f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f) * 2f, GUIM.YRES(180f), GUIM.YRES(28f)), BaseColor.White, sBuyMore, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				buycode = -1;
			}
			if (GUIM.Button(new Rect(rBackCheck.x + rBackCheck.width - GUIM.YRES(188f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f), GUIM.YRES(180f), GUIM.YRES(28f)), BaseColor.Yellow, sGoto, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				Main.EmulateClick(1);
			}
			Rect r3 = new Rect(rBackCheck.x + GUIM.YRES(192f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f), GUIM.YRES(514f), GUIM.YRES(28f));
			if (buycode >= 0)
			{
				GUIM.DrawText(r3, buymsg[buycode], TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
			}
			return;
		}
		if (GUIOptions.Gold < num)
		{
			Rect rect = new Rect(rBackCheck.x + GUIM.YRES(192f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f), GUIM.YRES(514f), GUIM.YRES(28f));
			GUI.DrawTexture(rect, tBlack);
			GUIM.DrawText(rect, sNotCoin, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
			if (GUIM.Button(new Rect(rBackCheck.x + rBackCheck.width - GUIM.YRES(200f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f), GUIM.YRES(60f), GUIM.YRES(28f)), BaseColor.Block, sCancel, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				inbuy = false;
			}
			if (GUIM.Button(new Rect(rBackCheck.x + rBackCheck.width - GUIM.YRES(132f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f), GUIM.YRES(124f), GUIM.YRES(28f)), BaseColor.Yellow, sFillCoin, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				Main.EmulateClick(9);
			}
			return;
		}
		if (GUIM.Button(new Rect(rBackCheck.x + rBackCheck.width - GUIM.YRES(200f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f), GUIM.YRES(60f), GUIM.YRES(28f)), BaseColor.Block, sCancel, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			inbuy = false;
		}
		bool flag = false;
		if (psselect != null)
		{
			for (int i = 0; i < GUIInv.pslist.Count; i++)
			{
				if (GUIInv.pslist[i].psi == psselect.psi)
				{
					flag = true;
				}
			}
		}
		if (!flag && GUIM.Button(new Rect(rBackCheck.x + rBackCheck.width - GUIM.YRES(132f), rBackCheck.y + rBackCheck.height - GUIM.YRES(36f), GUIM.YRES(124f), GUIM.YRES(28f)), BaseColor.White, sContinue, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && !inlock)
		{
			MasterClient.cs.send_buy(num2, num3);
			inlock = true;
		}
	}
}
