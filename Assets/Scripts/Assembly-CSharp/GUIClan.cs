using System;
using System.Collections.Generic;
using GameClass;
using UnityEngine;

public class GUIClan : MonoBehaviour
{
	public class CRect
	{
		public Rect rb0;

		public Rect rb1;

		public Rect rName;

		public Rect rSlot;

		public Rect rOwner;

		public Rect rStatus;

		public CRect(Rect rBase)
		{
			rb0 = new Rect(rBase.x, rBase.y, GUIM.YRES(40f), rBase.height);
			rb1 = new Rect(rBase.x + GUIM.YRES(44f), rBase.y, rBase.width - GUIM.YRES(44f), rBase.height);
			rName = new Rect(rb1.x + GUIM.YRES(20f), rb1.y + GUIM.YRES(2f), rb1.width, rb1.height);
			rSlot = new Rect(rb1.x + GUIM.YRES(160f), rb1.y + GUIM.YRES(2f), rb1.width, rb1.height);
			rOwner = new Rect(rb1.x + GUIM.YRES(260f), rb1.y + GUIM.YRES(2f), rb1.width, rb1.height);
			rStatus = new Rect(rb1.x + GUIM.YRES(400f), rb1.y + GUIM.YRES(2f), rb1.width, rb1.height);
		}
	}

	public static bool show = false;

	private static Texture2D tWhite = null;

	private static Texture2D tBlack = null;

	private static Texture2D tBlue = null;

	private static Texture2D tGray = null;

	private Texture2D tCreate;

	private Texture2D tFind;

	private Texture2D tRed;

	private Texture2D tOptions;

	private Texture2D tArrowUp;

	private Texture2D tArrowDown;

	private Texture2D tMessageAdd;

	private Texture2D tMessageDel;

	private Rect rLeft;

	private Rect rLeftButton;

	private Rect rLeftIcon;

	private Rect rRight;

	private Rect rRightButton;

	private Rect rRightIcon;

	private Rect rBack;

	private Rect rZone;

	private Rect rFind;

	private Rect rFindText;

	private Rect rTop;

	private Rect rCreateBack;

	private Rect rCreateBack2;

	private Rect rCreateBack3;

	private Rect rCreateEditText;

	private Rect rCreateButton;

	private Rect rCreateEditText2;

	private Rect rClanName;

	private Rect[] rBase = new Rect[3];

	private Rect[] rProgress = new Rect[3];

	private Rect[] rStats = new Rect[3];

	private Rect rBack2;

	private Rect rBack3;

	private Rect rScroll;

	private Rect rOptions;

	private Rect rBackInvite;

	private Rect rZoneInvite;

	private Rect[] rBackChat = new Rect[3];

	private Vector2 scroll = Vector2.zero;

	private static ClanData nullclan = new ClanData(0, "", "", 0, -1, 0);

	private static ClanData currclan = null;

	public static string sEdit = "";

	private static bool clancreate = false;

	private static bool clanoptions = false;

	private static float tclandelete = 0f;

	public static string msg = "";

	public static BaseColor msg_color = BaseColor.White;

	public static string clanname = "";

	public static int clanid = 0;

	public static string clanname_owner = "";

	public static int clanid_owner = 0;

	public static int clanoid = 0;

	public static int clanpcount = 0;

	public static int clanslots = 0;

	public static string sClanslots = "";

	public static string clankey = "";

	public static int clanstatus = 0;

	public static int clannamecount = 0;

	public static List<ClanPlayer> playerlist = null;

	public static List<ClanMessage> messagelist = null;

	public static ClanPlayer currplayer = null;

	public static int playerlistonline = 0;

	public static List<ClanChatMessage> chatlist = null;

	public static string sFD = "0";

	public static string sF = "0";

	public static string sD = "0";

	public static string sLevel = "1";

	public static string sExp = "0";

	public static float fLevelProgress = 0f;

	public static string sLevelProgress = "0%";

	public static string sExpProgress = "0";

	private string sLeave;

	private string sLvConf;

	private string sCancel;

	private string sCind;

	private string sCreat;

	private string sLook;

	private string sSearch;

	private string sClName;

	private string sParty;

	private string sFound;

	private string sAdmiss;

	private string sSubmiss;

	private string sNotAccept;

	private string sApply;

	private string sSend;

	private string sIsOpen;

	private string sIsClose;

	private string sLastClans;

	private string sRebuild;

	private string sCreature;

	private string sCreatAvai;

	private string sAttentName;

	private string sNumPosts;

	private string sRenameCl;

	private string sStatus;

	private string sExpans;

	private string sRemove;

	private string sRemConf;

	private string sAvairen;

	private string sRename;

	private string sReturn;

	private string sCurrentOpen;

	private string sTakeAppl;

	private string sCurrentClose;

	private string sNotAppl;

	private string sChangest;

	private string sCurrentNumb;

	private string sYourMaxClan;

	private string sSlotAfterExp;

	private string sMaxClan;

	private string sCostOper;

	private string sContinue;

	private string sReqClan;

	private string sClanLevel;

	private string sClanMembers;

	private string sClanExp;

	private string sLevelProgress_;

	private string sRatio;

	private string sClanFrags;

	private string sClanDeaths;

	private string sFrags;

	private string sDeaths;

	private string sPlayerStats;

	private string sPlayerExp;

	private string sFind_;

	private Texture2D[] tRank = new Texture2D[4];

	private string[] sRank = new string[4] { "РЯДОВОЙ", "СЕРЖАНТ", "КОМАНДИР", "КАПИТАН" };

	private float nextfind;

	private int adminmenu;

	private string sPurchaseChange;

	private string sBuy;

	private static Color c8 = new Color(1f, 1f, 1f, 0.65f);

	private static CRect[] cr = null;

	private static CRect header = null;

	public static ClanData[] cl = new ClanData[25];

	private Vector2 sv = Vector2.zero;

	public static string sFind = "";

	private Vector2 scrollinvite = Vector2.zero;

	private float tlastsend;

	public void LoadLang()
	{
		sRank[0] = Lang.GetString("_SOLDIER");
		sRank[1] = Lang.GetString("_SERGEANT");
		sRank[2] = Lang.GetString("_COMMANDER");
		sRank[3] = Lang.GetString("_CAPTAIN");
		sLeave = Lang.GetString("_LEAVE_CLAN");
		sLvConf = Lang.GetString("_LEAVE_CLAN_CONFIRM");
		sCancel = Lang.GetString("_CANCEL");
		sFind_ = Lang.GetString("_FIND_CLAN");
		sCreat = Lang.GetString("_CREATE_CLAN");
		sLook = Lang.GetString("_LOOKFOR");
		sSearch = Lang.GetString("_SEARCH");
		sClName = Lang.GetString("_CLAN_NAME");
		sParty = Lang.GetString("_PARTY");
		sFound = Lang.GetString("_FOUNDER");
		sAdmiss = Lang.GetString("_ADMISSION");
		sSubmiss = Lang.GetString("_SUBMISSION_FOR_BID");
		sNotAccept = Lang.GetString("_CLAN_NOT_ACCEPT");
		sApply = Lang.GetString("_APPLY_TO_CLAN");
		sSend = Lang.GetString("_SEND");
		sIsOpen = Lang.GetString("_IS_OPEN");
		sIsClose = Lang.GetString("_IS_CLOSE");
		sLastClans = Lang.GetString("_SHOW_LAST_CLANS");
		sRebuild = Lang.GetString("_REBUILD_CLAN");
		sCreature = Lang.GetString("_CREATURE");
		sCreatAvai = Lang.GetString("_CLAN_CREATION_AVAILABLE");
		sAttentName = Lang.GetString("_ATTENTION_NAME");
		sNumPosts = Lang.GetString("_NUMBER_POSTS");
		sRenameCl = Lang.GetString("_RENAME_CLAN");
		sStatus = Lang.GetString("_CLAN_STATUS");
		sExpans = Lang.GetString("_CLAN_EXPANSION");
		sRemove = Lang.GetString("_REMOVE_CLAN");
		sRemConf = Lang.GetString("_REMOVE_CLAN_CONFIRM");
		sAvairen = Lang.GetString("_AVAILABLE_RENAME");
		sRename = Lang.GetString("_RENAME");
		sReturn = Lang.GetString("_RETURN");
		sCurrentOpen = Lang.GetString("_CURRENT_STATUS_CLAN_OPEN");
		sTakeAppl = Lang.GetString("_CLAN_TAKES_APPLICATION");
		sCurrentClose = Lang.GetString("_CURRENT_STATUS_CLAN_CLOSE");
		sNotAppl = Lang.GetString("_CLAN_NOT_ACCEPT_APPLICATION");
		sChangest = Lang.GetString("_CHANGE_STATUS");
		sCurrentNumb = Lang.GetString("_CURRENT_NUMBER_SLOTS");
		sYourMaxClan = Lang.GetString("_YOUR_CLAN_MAXIMUM");
		sSlotAfterExp = Lang.GetString("_SLOTS_AFTER_EXPANSION");
		sMaxClan = Lang.GetString("_MAXIMUM_200");
		sCostOper = Lang.GetString("_COST_OPERATION");
		sContinue = Lang.GetString("_CONTINUE");
		sReqClan = Lang.GetString("_REQUEST_PLAYER_TO_CLAN");
		sClanLevel = Lang.GetString("_LEVEL");
		sClanMembers = Lang.GetString("_MEMBERS");
		sClanExp = Lang.GetString("_CLAN_EXP");
		sLevelProgress_ = Lang.GetString("_LEVEL_PROGRESS");
		sRatio = Lang.GetString("_RATIO");
		sClanFrags = Lang.GetString("_CLAN_FRAGS");
		sClanDeaths = Lang.GetString("_CLAN_DEATHS");
		sFrags = Lang.GetString("_FRAGS");
		sDeaths = Lang.GetString("_DEATH");
		sPlayerStats = Lang.GetString("_PLAYER_STATS");
		sPlayerExp = Lang.GetString("_PLAYER_GAINED");
		sPurchaseChange = Lang.GetString("_PURCHASE_CHANGE_NAME");
		sBuy = Lang.GetString("_BUY");
	}

	public static void SetActive(bool val)
	{
		show = val;
		NullClans();
		clancreate = false;
		clanoptions = false;
		tclandelete = 0f;
		if (val)
		{
			MasterClient.cs.send_clanstats();
			MasterClient.cs.send_clanplayers();
			MasterClient.cs.send_clanmessage();
			currplayer = null;
		}
	}

	public static void NullClans()
	{
		for (int i = 0; i < 25; i++)
		{
			if (cl[i] == null)
			{
				cl[i] = nullclan;
			}
		}
	}

	private void LoadEnd()
	{
		tWhite = TEX.GetTextureByName("white");
		tBlack = TEX.GetTextureByName("black");
		tGray = TEX.GetTextureByName("gray0");
		tRed = TEX.GetTextureByName("orange");
		tBlue = TEX.GetTextureByName("blue");
		tCreate = Resources.Load("gui/clan_create") as Texture2D;
		tFind = Resources.Load("gui/clan_find") as Texture2D;
		tRank[0] = Resources.Load("gui/rank_0") as Texture2D;
		tRank[1] = Resources.Load("gui/rank_1") as Texture2D;
		tRank[2] = Resources.Load("gui/rank_2") as Texture2D;
		tRank[3] = Resources.Load("gui/rank_3") as Texture2D;
		tOptions = ContentLoader_.LoadTexture("options") as Texture2D;
		tArrowUp = Resources.Load("arrow_u") as Texture2D;
		tArrowDown = Resources.Load("arrow_d") as Texture2D;
		tMessageAdd = Resources.Load("gui/message_add") as Texture2D;
		tMessageDel = Resources.Load("gui/message_del") as Texture2D;
	}

	private void OnResize()
	{
		rLeft.Set((float)Screen.width / 2f - GUIM.YRES(320f) - GUIM.YRES(8f), GUIM.YRES(200f), GUIM.YRES(320f), GUIM.YRES(240f));
		rLeftButton.Set(rLeft.x + GUIM.YRES(60f), rLeft.y + rLeft.height - GUIM.YRES(80f), GUIM.YRES(200f), GUIM.YRES(28f));
		rLeftIcon.Set(rLeft.x + GUIM.YRES(128f), rLeft.y + GUIM.YRES(60f), GUIM.YRES(64f), GUIM.YRES(64f));
		rRight.Set((float)Screen.width / 2f + GUIM.YRES(8f), GUIM.YRES(200f), GUIM.YRES(320f), GUIM.YRES(240f));
		rRightButton.Set(rRight.x + GUIM.YRES(60f), rRight.y + rRight.height - GUIM.YRES(80f), GUIM.YRES(200f), GUIM.YRES(28f));
		rRightIcon.Set(rRight.x + GUIM.YRES(128f), rRight.y + GUIM.YRES(60f), GUIM.YRES(64f), GUIM.YRES(64f));
		rBack.Set((float)Screen.width / 2f - GUIM.YRES(300f), GUIM.YRES(140f) + GUIM.YRES(90f), GUIM.YRES(600f), GUIM.YRES(370f));
		if (cr == null)
		{
			cr = new CRect[25];
		}
		int num = (int)GUIM.YRES(4f);
		int num2 = (int)GUIM.YRES(26f);
		for (int i = 0; i < 25; i++)
		{
			Rect rect = new Rect(0f, (num2 + num) * i, rBack.width - GUIM.YRES(18f), num2);
			cr[i] = new CRect(rect);
		}
		rZone = new Rect(0f, 0f, 0f, (num2 + num) * 25 - num);
		rFind = new Rect(rBack.x, GUIM.YRES(140f), rBack.width, GUIM.YRES(42f));
		rFindText = new Rect(rFind.x + GUIM.YRES(10f), rFind.y + GUIM.YRES(2f), GUIM.YRES(200f), GUIM.YRES(42f));
		rTop = new Rect(rBack.x, GUIM.YRES(190f), rBack.width, GUIM.YRES(32f));
		header = new CRect(new Rect((float)Screen.width / 2f - GUIM.YRES(300f), GUIM.YRES(192f), rBack.width, num2));
		rCreateBack.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(160f));
		rCreateBack2.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f) + GUIM.YRES(168f), GUIM.YRES(920f), GUIM.YRES(120f));
		rCreateBack3.Set(rCreateBack2.x, rCreateBack2.y + rCreateBack2.height + GUIM.YRES(8f), GUIM.YRES(920f), GUIM.YRES(60f));
		rCreateEditText.Set(rCreateBack.x + GUIM.YRES(360f), rCreateBack.y + GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(32f));
		rCreateButton.Set(rCreateBack.x + GUIM.YRES(360f), rCreateBack.y + GUIM.YRES(72f), GUIM.YRES(200f), GUIM.YRES(32f));
		rCreateEditText2.Set(rCreateBack.x + GUIM.YRES(360f) + GUIM.YRES(216f), rCreateBack.y + GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(32f));
		rClanName.Set(rCreateBack.x, rCreateBack.y + GUIM.YRES(16f), rCreateBack.width, GUIM.YRES(40f));
		rBase[0].Set(rCreateBack.x + GUIM.YRES(40f), rCreateBack.y + GUIM.YRES(56f), GUIM.YRES(200f), GUIM.YRES(32f));
		rBase[1].Set(rCreateBack.x + GUIM.YRES(40f), rCreateBack.y + GUIM.YRES(88f), GUIM.YRES(200f), GUIM.YRES(32f));
		rProgress[0].Set(rBase[0].x + GUIM.YRES(260f), rBase[0].y + GUIM.YRES(32f) * 0f, GUIM.YRES(300f), GUIM.YRES(32f));
		rProgress[1].Set(rBase[0].x + GUIM.YRES(260f), rBase[0].y + GUIM.YRES(32f) * 1f, GUIM.YRES(300f), GUIM.YRES(32f));
		rProgress[2].Set(rBase[0].x + GUIM.YRES(260f), rBase[0].y + GUIM.YRES(32f) * 2f, GUIM.YRES(300f), GUIM.YRES(32f));
		rStats[0] = rProgress[0];
		rStats[0].x += GUIM.YRES(350f);
		rStats[0].width = GUIM.YRES(200f);
		rStats[1] = rProgress[1];
		rStats[1].x += GUIM.YRES(350f);
		rStats[1].width = GUIM.YRES(200f);
		rStats[2] = rProgress[2];
		rStats[2].x += GUIM.YRES(350f);
		rStats[2].width = GUIM.YRES(200f);
		rBack2.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(316f), GUIM.YRES(600f), GUIM.YRES(324f));
		rScroll.Set(rBack2.x + GUIM.YRES(8f), rBack2.y + GUIM.YRES(8f), rBack2.width - GUIM.YRES(16f), rBack2.height - GUIM.YRES(16f));
		rBack3.Set(rBack2.x + rBack2.width + GUIM.YRES(16f), rBack2.y, GUIM.YRES(304f), rBack2.height);
		rOptions.Set(rCreateBack.x + rCreateBack.width - GUIM.YRES(36f), rCreateBack.y + GUIM.YRES(8f), GUIM.YRES(28f), GUIM.YRES(28f));
		rBackInvite.Set((float)Screen.width / 2f - GUIM.YRES(300f), GUIM.YRES(140f) + GUIM.YRES(40f), GUIM.YRES(600f), GUIM.YRES(420f));
		rZoneInvite = new Rect(0f, 0f, 0f, GUIM.YRES(420f));
		rBackChat[0].Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(600f), GUIM.YRES(420f));
		rBackChat[1].Set(rBackChat[0].x + rBackChat[0].width + GUIM.YRES(16f), rBackChat[0].y, GUIM.YRES(304f), rBackChat[0].height);
		rBackChat[2].Set(rBackChat[0].x, rBackChat[0].y + GUIM.YRES(436f), rBackChat[0].width, GUIM.YRES(64f));
	}

	private void OnGUI()
	{
		if (!show)
		{
			return;
		}
		if (Main.currSubMenu <= 0)
		{
			if (clancreate)
			{
				DrawClanCreate();
			}
			else if (clanid > 0)
			{
				DrawClan();
			}
			else
			{
				DrawSelector();
			}
		}
		else if (Main.currSubMenu == 1)
		{
			DrawFind();
		}
		else if (Main.currSubMenu == 2)
		{
			DrawInvites();
		}
		else if (Main.currSubMenu == 3)
		{
			DrawChat();
		}
	}

	private void DrawClan()
	{
		GUIM.DrawBox(rCreateBack, tBlack, 0.05f);
		if (!clanoptions)
		{
			GUIM.DrawText(rClanName, clanname, TextAnchor.MiddleCenter, BaseColor.Yellow, 1, 32, false);
			DrawLine(rBase[0], sClanLevel + ":", sLevel);
			DrawLine(rBase[1], sClanMembers + ":", sClanslots);
			DrawLine(rProgress[0], sClanExp + ":", sExp);
			DrawLine(rProgress[1], sLevelProgress_, sLevelProgress);
			DrawLine(rProgress[2], "", sExpProgress);
			GUI.DrawTexture(new Rect(rProgress[2].x, rProgress[2].y + GUIM.YRES(8f), GUIM.YRES(150f), GUIM.YRES(8f)), tBlack);
			GUI.DrawTexture(new Rect(rProgress[2].x, rProgress[2].y + GUIM.YRES(8f), GUIM.YRES(150f) * fLevelProgress, GUIM.YRES(8f)), tWhite);
			DrawLine(rStats[0], sClanFrags, sF);
			DrawLine(rStats[1], sClanDeaths, sD);
			DrawLine(rStats[2], sRatio, sFD);
		}
		else if (clanoid == GUIOptions.gid)
		{
			if (adminmenu == 0)
			{
				DrawClanAdmin();
			}
			if (adminmenu == 1)
			{
				DrawClanName();
			}
			if (adminmenu == 2)
			{
				DrawClanStatus();
			}
			if (adminmenu == 3)
			{
				DrawClanExtend();
			}
		}
		else
		{
			Rect r = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 0f, GUIM.YRES(200f), GUIM.YRES(28f));
			bool flag = false;
			if ((tclandelete == 0f) ? GUIM.Button(r, BaseColor.Block, sLeave, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) : ((!(Time.time > tclandelete)) ? GUIM.Button(r, BaseColor.White, sCancel + " ( " + (tclandelete - Time.time).ToString("0.0") + " )", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) : GUIM.Button(r, BaseColor.Red, sLvConf, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false)))
			{
				if (tclandelete == 0f)
				{
					tclandelete = Time.time + 60f;
				}
				else if (Time.time > tclandelete)
				{
					MasterClient.cs.send_clanleave();
				}
				else
				{
					tclandelete = 0f;
				}
			}
		}
		GUI.DrawTexture(rOptions, tOptions);
		if (GUIM.HideButton(rOptions))
		{
			clanoptions = !clanoptions;
			adminmenu = 0;
		}
		GUIM.DrawBox(rBack2, tBlack, 0.05f);
		GUIM.DrawBox(rBack3, tBlack, 0.05f);
		if (currplayer != null)
		{
			Rect r2 = new Rect(rBack3.x + GUIM.YRES(20f), rBack3.y + GUIM.YRES(20f), rBack3.width - GUIM.YRES(40f), GUIM.YRES(40f));
			Rect r3 = new Rect(rBack3.x + GUIM.YRES(20f), rBack3.y + GUIM.YRES(60f) + GUIM.YRES(32f) * 0f, rBack3.width - GUIM.YRES(40f), GUIM.YRES(32f));
			Rect r4 = new Rect(rBack3.x + GUIM.YRES(20f), rBack3.y + GUIM.YRES(60f) + GUIM.YRES(32f) * 1f, rBack3.width - GUIM.YRES(40f), GUIM.YRES(32f));
			Rect r5 = new Rect(rBack3.x + GUIM.YRES(20f), rBack3.y + GUIM.YRES(60f) + GUIM.YRES(32f) * 2f, rBack3.width - GUIM.YRES(40f), GUIM.YRES(32f));
			Rect r6 = new Rect(rBack3.x + GUIM.YRES(20f), rBack3.y + GUIM.YRES(60f) + GUIM.YRES(32f) * 3f, rBack3.width - GUIM.YRES(40f), GUIM.YRES(32f));
			GUIM.DrawText(r2, sPlayerStats + " " + currplayer.n, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
			DrawLine(r3, sPlayerExp, currplayer.full_exp.ToString());
			DrawLine(r4, sFrags, currplayer.full_f.ToString());
			DrawLine(r5, sDeaths, currplayer.full_d.ToString());
			DrawLine(r6, sRatio, currplayer.full_fd);
		}
		if (playerlist != null)
		{
			float num = ((clanoid == GUIOptions.gid && clanoptions) ? GUIM.YRES(50f) : 0f);
			sv = GUIM.BeginScrollView(new Rect(rBack2.x + GUIM.YRES(120f), rBack2.y + GUIM.YRES(20f), GUIM.YRES(370f) + num, rBack2.height - GUIM.YRES(40f)), scrollzone: new Rect(0f, 0f, GUIM.YRES(350f) + num, GUIM.YRES(36f) * (float)(playerlist.Count + 1)), scrollViewVector: sv);
			for (int i = 0; i < playerlist.Count; i++)
			{
				DrawPlayer(i, playerlist[i]);
			}
			GUIM.EndScrollView();
		}
	}

	private void DrawPlayer(int pos, ClanPlayer p)
	{
		Rect r = new Rect(0f, GUIM.YRES(36f) * (float)pos, GUIM.YRES(350f), GUIM.YRES(32f));
		Rect position = new Rect(r.x + GUIM.YRES(6f), r.y + GUIM.YRES(2f), GUIM.YRES(28f), GUIM.YRES(28f));
		Rect r2 = new Rect(r.x + GUIM.YRES(40f), r.y + GUIM.YRES(2f), GUIM.YRES(200f), GUIM.YRES(32f));
		Rect r3 = new Rect(r.x + GUIM.YRES(200f), r.y + GUIM.YRES(2f), GUIM.YRES(80f), GUIM.YRES(32f));
		Rect position2 = new Rect(r.x + GUIM.YRES(240f), r.y + GUIM.YRES(4f), GUIM.YRES(24f), GUIM.YRES(24f));
		Rect r4 = new Rect(r.x + GUIM.YRES(276f), r.y + GUIM.YRES(2f), GUIM.YRES(80f), GUIM.YRES(32f));
		Rect r5 = new Rect(r.x + GUIM.YRES(360f), r.y, GUIM.YRES(40f), GUIM.YRES(32f));
		GUIM.DrawBox(r, tGray, 0.2f);
		if (currplayer == p)
		{
			GUI.DrawTexture(new Rect(r.x, r.y, GUIM.YRES(2f), r.height), TEX.tYellow);
		}
		if (p.tAvatar == null)
		{
			p.tAvatar = FaceGen.Build(p.p0, p.p1, p.p2, p.p3, p.p4, p.p5, p.p6, p.p7, p.p8, p.p9);
		}
		GUI.DrawTexture(position, p.tAvatar);
		GUIM.DrawText(r2, p.n, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
		GUIM.DrawText(r3, p.sLV, TextAnchor.MiddleLeft, BaseColor.Orange, 1, 14, false);
		GUI.DrawTexture(position2, tRank[p.rank]);
		GUIM.DrawText(r4, sRank[p.rank], TextAnchor.MiddleLeft, BaseColor.LightGray, 1, 14, false);
		if (GUIM.HideButton(r))
		{
			currplayer = p;
		}
		if (clanoid == GUIOptions.gid && clanoptions && clanoid != p.id)
		{
			GUIM.DrawBox(r5, tGray, 0.2f);
			Rect rect = new Rect(r5.x + GUIM.YRES(11f), r5.y + GUIM.YRES(7f), GUIM.YRES(18f), GUIM.YRES(18f));
			if (GUIM.Contains(rect))
			{
				GUI.color = GUIM.colorlist[2];
			}
			GUI.DrawTexture(rect, tMessageDel);
			GUI.color = Color.white;
			if (GUIM.HideButton(rect))
			{
				MasterClient.cs.send_clanplayerop(0, p.id);
			}
		}
	}

	private void DrawSelector()
	{
		GUIM.DrawBox(rLeft, tBlack, 0.1f);
		GUI.DrawTexture(rLeftIcon, tFind);
		if (GUIM.Button(rLeftButton, BaseColor.White, sFind_, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Main.currSubMenu = 1;
			clancreate = false;
		}
		GUIM.DrawBox(rRight, tBlack, 0.1f);
		GUI.DrawTexture(rRightIcon, tCreate);
		if (GUIM.Button(rRightButton, BaseColor.White, sCreat, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Main.currSubMenu = 0;
			clancreate = true;
			msg = "";
			MasterClient.cs.send_clanrestore();
		}
	}

	private void DrawFind()
	{
		GUIM.DrawBox(rFind, tBlack, 0.05f);
		GUIM.DrawText(rFindText, sFind, TextAnchor.MiddleLeft, BaseColor.Yellow, 1, 14, false);
		Rect rect = new Rect(rFind.x + rFind.width - GUIM.YRES(280f), rFind.y + GUIM.YRES(7f), GUIM.YRES(140f), GUIM.YRES(28f));
		GUI.DrawTexture(rect, tBlack);
		GUIM.DrawEdit(rect, ref sEdit, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
		Rect rect2 = new Rect(rFind.x + rFind.width - GUIM.YRES(132f), rFind.y + GUIM.YRES(7f), GUIM.YRES(116f), GUIM.YRES(28f));
		bool flag = false;
		if (Time.time > nextfind)
		{
			flag = GUIM.Button(rect2, BaseColor.White, sLook, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		}
		else
		{
			GUI.DrawTexture(rect2, tBlack);
			GUIM.DrawText(rect2, (nextfind - Time.time).ToString("0.0"), TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, false);
		}
		if (flag)
		{
			nextfind = Time.time + 5f;
			MasterClient.cs.send_clanfind(sEdit);
			sFind = sSearch;
		}
		GUIM.DrawBox(rTop, tBlack, 0.05f);
		GUIM.DrawText(header.rb0, "#", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rName, sClName, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rSlot, sParty, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rOwner, sFound, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(header.rStatus, sAdmiss, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		scroll = GUIM.BeginScrollView(rBack, scroll, rZone);
		for (int i = 0; i < 25; i++)
		{
			DrawField(i, cl[i]);
		}
		GUIM.EndScrollView();
		Rect r = new Rect(rBack.x, rBack.y + rBack.height + GUIM.YRES(8f), rBack.width, GUIM.YRES(32f));
		GUIM.DrawBox(r, tBlack, 0.05f);
		if (currclan != null && clanid == 0 && (messagelist == null || (messagelist != null && messagelist.Count == 0)))
		{
			int num = 10;
			if (GUIOptions.level < num)
			{
				GUIM.DrawText(r, sSubmiss, TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
			}
			else if (currclan.status == 1)
			{
				GUIM.DrawText(r, sNotAccept, TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
			}
			else
			{
				Rect r2 = new Rect(r.x + r.width - GUIM.YRES(352f), r.y, GUIM.YRES(200f), r.height);
				Rect r3 = new Rect(r.x + r.width - GUIM.YRES(136f), r.y + GUIM.YRES(2f), GUIM.YRES(120f), GUIM.YRES(28f));
				GUIM.DrawText(r2, sApply, TextAnchor.MiddleRight, BaseColor.White, 0, 20, false);
				if (GUIM.Button(r3, BaseColor.Yellow, sSend, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
				{
					MasterClient.cs.send_clannewmessage(0, currclan.id);
					Main.currSubMenu = 2;
					currclan = null;
				}
			}
		}
		CheckEdit();
	}

	private void DrawField(int i, ClanData data)
	{
		if (data == null)
		{
			return;
		}
		CRect cRect = cr[i];
		GUIM.DrawBox(cRect.rb0, tBlack, 0.15f);
		if (currclan == data)
		{
			GUIM.DrawBox(cRect.rb1, tBlue, 0.05f);
		}
		else if (GUIM.Contains(cRect.rb1))
		{
			GUIM.DrawBox(cRect.rb1, tWhite, 0.05f);
		}
		else
		{
			GUIM.DrawBox(cRect.rb1, tBlack, 0.05f);
		}
		GUIM.DrawText(cRect.rb0, (i + 1).ToString(), TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		if (data.status >= 0)
		{
			GUIM.DrawText(cRect.rName, data.n, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
			GUIM.DrawText(cRect.rSlot, data.slots, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
			GUIM.DrawText(cRect.rOwner, data.owner, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
			GUIM.DrawText(cRect.rStatus, (data.status == 0) ? sIsOpen : sIsClose, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
			if (GUIM.HideButton(cRect.rb1))
			{
				currclan = data;
			}
		}
	}

	private void OnChangeSubMenu()
	{
		if (show)
		{
			sEdit = "";
			if (Main.currSubMenu == 1)
			{
				MasterClient.cs.send_clanlist();
				sFind = sLastClans;
			}
			else if (Main.currSubMenu == 3)
			{
				MasterClient.cs.send_clanchatlist();
			}
			clancreate = false;
		}
	}

	private void DrawClanCreate()
	{
		GUIM.DrawBox(rCreateBack, tBlack, 0.05f);
		GUI.DrawTexture(rCreateEditText, tBlack);
		if (clanid_owner > 0)
		{
			GUIM.DrawText(rCreateEditText, clanname_owner, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, true);
		}
		else
		{
			GUIM.DrawEdit(rCreateEditText, ref sEdit, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, true);
			GUIM.DrawText(rCreateEditText2, msg, TextAnchor.MiddleLeft, msg_color, 1, 14, true);
		}
		if (GUIOptions.level < 15)
		{
			GUIM.Button(rCreateButton, BaseColor.Block, sCreat, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		}
		else if (clanid_owner > 0)
		{
			if (GUIM.Button(rCreateButton, BaseColor.White, sRebuild, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				MasterClient.cs.send_clancreate("null");
			}
		}
		else if (GUIM.Button(rCreateButton, BaseColor.White, sCreat, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			msg = sCreature;
			msg_color = BaseColor.White;
			MasterClient.cs.send_clancreate(sEdit);
		}
		GUIM.DrawText(rCreateEditText2, msg, TextAnchor.MiddleLeft, msg_color, 1, 14, true);
		GUIM.DrawText(new Rect(rCreateBack.x, rCreateBack.y + GUIM.YRES(120f), rCreateBack.width, GUIM.YRES(20f)), sCreatAvai, TextAnchor.MiddleCenter, BaseColor.Yellow, 1, 14, true);
		DrawHint();
		CheckEdit();
	}

	private void DrawHint()
	{
		GUIM.DrawBox(rCreateBack2, tBlack, 0.05f);
		GUIM.DrawText(rCreateBack2, sAttentName, TextAnchor.MiddleCenter, BaseColor.LightGray2, 1, 14, true);
	}

	private void DrawInvites()
	{
		GUIM.DrawBox(new Rect(rBack.x, GUIM.YRES(140f), rBack.width, GUIM.YRES(32f)), tBlack, 0.05f);
		GUIM.DrawBox(rBackInvite, tBlack, 0.05f);
		if (messagelist != null)
		{
			scrollinvite = GUIM.BeginScrollView(rBackInvite, scrollinvite, rZoneInvite);
			for (int i = 0; i < messagelist.Count; i++)
			{
				DrawMessage(i, messagelist[i]);
			}
			GUIM.EndScrollView();
		}
		Rect r = new Rect(rBack.x, rBack.y + rBack.height + GUIM.YRES(8f), rBack.width, GUIM.YRES(32f));
		GUIM.DrawBox(r, tBlack, 0.05f);
		if (messagelist != null)
		{
			GUIM.DrawText(r, sNumPosts + " " + messagelist.Count, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
	}

	private void DrawMessage(int pos, ClanMessage m)
	{
		int num = (int)GUIM.YRES(26f);
		int num2 = (int)GUIM.YRES(4f);
		int num3 = (num + num2) * pos;
		Rect r = new Rect(0f, num3, rBack.width, num);
		Rect position = new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(4f), GUIM.YRES(18f), GUIM.YRES(18f));
		Rect r2 = new Rect(r.x + GUIM.YRES(36f), r.y + GUIM.YRES(2f), GUIM.YRES(400f), num);
		Rect r3 = new Rect(r.x + GUIM.YRES(450f), r.y + GUIM.YRES(2f), GUIM.YRES(100f), num);
		Rect rect = new Rect(r.x + GUIM.YRES(538f), r.y + GUIM.YRES(4f), GUIM.YRES(18f), GUIM.YRES(18f));
		Rect rect2 = new Rect(r.x + GUIM.YRES(568f), r.y + GUIM.YRES(4f), GUIM.YRES(18f), GUIM.YRES(18f));
		GUIM.DrawBox(r, tBlack, 0.05f);
		GUI.DrawTexture(position, tArrowUp);
		GUIM.DrawText(r2, m.msg, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
		GUIM.DrawText(r3, m.datetime, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 14, false);
		if (clanoid == GUIOptions.gid)
		{
			if (GUIM.Contains(rect))
			{
				GUI.color = GUIM.colorlist[3];
			}
			GUI.DrawTexture(rect, tMessageAdd);
			GUI.color = Color.white;
			if (GUIM.HideButton(rect))
			{
				MasterClient.cs.send_claninviteop(1, m.id);
				messagelist.Clear();
			}
			if (GUIM.Contains(rect2))
			{
				GUI.color = GUIM.colorlist[2];
			}
			GUI.DrawTexture(rect2, tMessageDel);
			GUI.color = Color.white;
			if (GUIM.HideButton(rect2))
			{
				MasterClient.cs.send_claninviteop(0, m.id);
				messagelist.Clear();
			}
		}
		else if (clanid <= 0)
		{
			if (GUIM.Contains(rect2))
			{
				GUI.color = GUIM.colorlist[2];
			}
			GUI.DrawTexture(rect2, tMessageDel);
			GUI.color = Color.white;
			if (GUIM.HideButton(rect2))
			{
				MasterClient.cs.send_claninviteop(0, m.id);
				messagelist.Clear();
			}
		}
	}

	private void CheckEdit()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < sEdit.Length; i++)
		{
			char c = sEdit[i];
			if ((c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я'))
			{
				flag2 = true;
				break;
			}
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
			{
				flag = true;
				break;
			}
		}
		char character = Event.current.character;
		if ((character < 'а' || character > 'я') && (character < 'А' || character > 'Я') && (character < 'a' || character > 'z') && (character < 'A' || character > 'Z') && (character < '0' || character > '9') && character != '_' && character != '.')
		{
			Event.current.character = '\0';
		}
		if (flag && ((character >= 'а' && character <= 'я') || (character >= 'А' && character <= 'Я')))
		{
			sEdit = "";
		}
		if (flag2 && ((character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z')))
		{
			sEdit = "";
		}
		if (sEdit.Length > 16)
		{
			Event.current.character = '\0';
		}
	}

	private void DrawClanAdmin()
	{
		if (GUIM.Button(new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f), GUIM.YRES(200f), GUIM.YRES(28f)), BaseColor.White, sRenameCl, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			adminmenu = 1;
			msg = "";
			MasterClient.cs.send_clanadminop(0, 0);
		}
		if (GUIM.Button(new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(28f)), BaseColor.White, sStatus, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			adminmenu = 2;
			MasterClient.cs.send_clanadminop(1, 0);
		}
		if (GUIM.Button(new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 2f, GUIM.YRES(200f), GUIM.YRES(28f)), BaseColor.White, sExpans, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			adminmenu = 3;
		}
		Rect r = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 3f, GUIM.YRES(200f), GUIM.YRES(28f));
		bool flag = false;
		if ((tclandelete == 0f) ? GUIM.Button(r, BaseColor.Block, sRemove, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) : ((!(Time.time > tclandelete)) ? GUIM.Button(r, BaseColor.White, sCancel + " ( " + (tclandelete - Time.time).ToString("0.0") + " )", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) : GUIM.Button(r, BaseColor.Red, sRemConf, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false)))
		{
			if (tclandelete == 0f)
			{
				tclandelete = Time.time + 180f;
			}
			else if (Time.time > tclandelete)
			{
				MasterClient.cs.send_clandelete();
			}
			else
			{
				tclandelete = 0f;
			}
		}
	}

	private void DrawClanName()
	{
		Rect rect = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 3f, rCreateBack.width, GUIM.YRES(28f));
		Rect r = new Rect(rect.x + GUIM.YRES(464f), rect.y, GUIM.YRES(200f), GUIM.YRES(28f));
		Rect r2 = new Rect(rect.x + GUIM.YRES(256f), rect.y, GUIM.YRES(200f), GUIM.YRES(28f));
		GUI.DrawTexture(rCreateEditText, tBlack);
		GUIM.DrawEdit(rCreateEditText, ref sEdit, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, true);
		GUIM.DrawText(rCreateEditText2, msg, TextAnchor.MiddleLeft, msg_color, 1, 14, true);
		GUIM.DrawText(new Rect(rCreateBack.x, rCreateBack.y + GUIM.YRES(80f), rCreateBack.width, GUIM.YRES(20f)), sAvairen + " " + clannamecount, TextAnchor.MiddleCenter, BaseColor.Yellow, 1, 14, true);
		if (clannamecount == 0)
		{
			BuyClanRename();
		}
		else
		{
			if (clannamecount == 0)
			{
				GUIM.Button(r, BaseColor.Block, sRename, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			}
			else if (GUIM.Button(r, BaseColor.Yellow, sRename, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				MasterClient.cs.send_clanrename(sEdit);
			}
			if (GUIM.Button(r2, BaseColor.White, sReturn, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				adminmenu = 0;
			}
		}
		CheckEdit();
	}

	private void BuyClanRename()
	{
		Rect rect = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(240f), 1f, 1f);
		GUI.DrawTexture(rect, TEX.tRed);
		GUIM.DrawBox(rect, tBlack, 0.05f);
		GUIM.DrawText(new Rect(rect.x + GUIM.YRES(280f), rect.y + GUIM.YRES(16f), GUIM.YRES(120f), GUIM.YRES(28f)), sPurchaseChange, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, true);
		GUI.color = c8;
		GUI.DrawTexture(new Rect(rect.x + GUIM.YRES(460f), rect.y + GUIM.YRES(16f), GUIM.YRES(60f), GUIM.YRES(28f)), tBlack);
		GUI.color = Color.white;
		GUIM.DrawText(new Rect(rect.x + GUIM.YRES(460f), rect.y + GUIM.YRES(20f), GUIM.YRES(40f), GUIM.YRES(28f)), "50", TextAnchor.MiddleCenter, BaseColor.White, 1, 20, false);
		GUI.DrawTexture(new Rect(rect.x + GUIM.YRES(492f), rect.y + GUIM.YRES(20f), GUIM.YRES(20f), GUIM.YRES(20f)), Main.tCoin);
		if (GUIM.Button(new Rect(rect.x + GUIM.YRES(530f), rect.y + GUIM.YRES(16f), GUIM.YRES(80f), GUIM.YRES(28f)), BaseColor.White, sBuy, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			MasterClient.cs.send_buyclanname();
		}
	}

	private void DrawClanStatus()
	{
		Rect r = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 0f, rCreateBack.width, GUIM.YRES(28f));
		Rect r2 = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 1f, rCreateBack.width, GUIM.YRES(28f));
		Rect rect = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 3f, rCreateBack.width, GUIM.YRES(28f));
		if (clanstatus == 0)
		{
			GUIM.DrawText(r, sCurrentOpen, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
			GUIM.DrawText(r2, sTakeAppl, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
		}
		else if (clanstatus == 1)
		{
			GUIM.DrawText(r, sCurrentClose, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
			GUIM.DrawText(r2, sNotAppl, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
		}
		Rect r3 = new Rect(rect.x + GUIM.YRES(464f), rect.y, GUIM.YRES(200f), GUIM.YRES(28f));
		Rect r4 = new Rect(rect.x + GUIM.YRES(256f), rect.y, GUIM.YRES(200f), GUIM.YRES(28f));
		if (GUIM.Button(r3, BaseColor.Yellow, sChangest, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			adminmenu = 0;
			MasterClient.cs.send_clanadminop(1, 1);
		}
		if (GUIM.Button(r4, BaseColor.White, sReturn, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			adminmenu = 0;
		}
	}

	private void DrawClanExtend()
	{
		Rect r = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 0f, rCreateBack.width, GUIM.YRES(28f));
		Rect r2 = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 1f, rCreateBack.width, GUIM.YRES(28f));
		Rect r3 = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 2f, rCreateBack.width, GUIM.YRES(28f));
		Rect rect = new Rect(rCreateBack.x + GUIM.YRES(20f), rCreateBack.y + GUIM.YRES(20f) + GUIM.YRES(32f) * 3f, rCreateBack.width, GUIM.YRES(28f));
		Rect r4 = new Rect(rect.x + GUIM.YRES(464f), rect.y, GUIM.YRES(200f), GUIM.YRES(28f));
		Rect r5 = new Rect(rect.x + GUIM.YRES(256f), rect.y, GUIM.YRES(200f), GUIM.YRES(28f));
		GUIM.DrawText(r, sCurrentNumb + clanslots + "</color>", TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
		if (clanslots >= 200)
		{
			GUIM.DrawText(r2, sYourMaxClan, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
		}
		else
		{
			GUIM.DrawText(r2, sSlotAfterExp + " <color>" + (clanslots + 8) + "</color> " + sMaxClan, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, false);
			GUIM.DrawText(r3, sCostOper, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 16, true);
			if (GUIM.Button(r4, BaseColor.Yellow, sContinue, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				adminmenu = 0;
				MasterClient.cs.send_clanadminop(2, 1);
			}
		}
		if (GUIM.Button(r5, BaseColor.White, sReturn, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			adminmenu = 0;
		}
	}

	private void DrawLine(Rect r, string label, string value)
	{
		GUIM.DrawText(r, label, TextAnchor.MiddleLeft, BaseColor.LightGray, 1, 16, false);
		GUIM.DrawText(r, value, TextAnchor.MiddleRight, BaseColor.White, 1, 16, false);
	}

	private void DrawChat()
	{
		GUIM.DrawBox(rBackChat[0], tBlack, 0.05f);
		GUIM.DrawBox(rBackChat[1], tBlack, 0.05f);
		GUIM.DrawBox(rBackChat[2], tBlack, 0.05f);
		if (playerlist == null)
		{
			return;
		}
		sv = GUIM.BeginScrollView(new Rect(rBackChat[1].x + GUIM.YRES(16f), rBackChat[1].y + GUIM.YRES(16f), rBackChat[1].width - GUIM.YRES(32f), rBackChat[1].height - GUIM.YRES(32f)), scrollzone: new Rect(0f, 0f, rBackChat[1].width - GUIM.YRES(52f), GUIM.YRES(36f) * (float)playerlistonline), scrollViewVector: sv);
		int num = 0;
		for (int i = 0; i < playerlist.Count; i++)
		{
			if (playerlist[i].online != 0)
			{
				DrawPlayerChat(num, playerlist[i]);
				num++;
			}
		}
		GUIM.EndScrollView();
		Rect rect = new Rect(rBackChat[2].x + GUIM.YRES(16f), rBackChat[2].y + GUIM.YRES(16f), rBackChat[2].width - GUIM.YRES(208f), GUIM.YRES(32f));
		GUI.DrawTexture(rect, tBlack);
		GUIM.DrawEdit(rect, ref sEdit, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
		Rect r = new Rect(rBackChat[2].x + rBackChat[2].width - GUIM.YRES(176f), rBackChat[2].y + GUIM.YRES(18f), GUIM.YRES(160f), GUIM.YRES(28f));
		if (Time.time < tlastsend)
		{
			GUIM.Button(r, BaseColor.Block, sSend, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		}
		else if (GUIM.Button(r, BaseColor.White, sSend, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			if (sEdit.Length > 0)
			{
				sEdit = string.Join(" ", sEdit.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries));
				sEdit = sEdit.Replace("<", "");
				sEdit = sEdit.Replace(">", "");
				sEdit = sEdit.Replace(":", "");
				sEdit = sEdit.Replace("/", "");
				sEdit = sEdit.Replace(".", "");
				if (sEdit.Length > 127)
				{
					sEdit = sEdit.Substring(0, 127);
				}
				MasterClient.cs.send_clanchatsend(sEdit);
				tlastsend = Time.time + 3f;
			}
			sEdit = "";
		}
		if (sEdit.Length > 64)
		{
			Event.current.character = '\0';
		}
		if (chatlist != null)
		{
			for (int j = 0; j < chatlist.Count; j++)
			{
				GUIM.DrawText(new Rect(rBackChat[0].x + GUIM.YRES(16f), rBackChat[0].y + GUIM.YRES(16f) + GUIM.YRES(28f) * (float)j, rBackChat[0].width - GUIM.YRES(32f), GUIM.YRES(28f)), chatlist[j].n + " <color=#FF0>:</color> " + chatlist[j].msg, TextAnchor.MiddleLeft, BaseColor.White, 1, 16, false);
			}
		}
	}

	private void DrawPlayerChat(int pos, ClanPlayer p)
	{
		Rect r = new Rect(0f, GUIM.YRES(36f) * (float)pos, GUIM.YRES(262f), GUIM.YRES(32f));
		Rect position = new Rect(r.x + GUIM.YRES(6f), r.y + GUIM.YRES(2f), GUIM.YRES(28f), GUIM.YRES(28f));
		Rect r2 = new Rect(r.x + GUIM.YRES(40f), r.y + GUIM.YRES(2f), GUIM.YRES(200f), GUIM.YRES(32f));
		Rect r3 = new Rect(r.x + GUIM.YRES(190f), r.y + GUIM.YRES(2f), GUIM.YRES(80f), GUIM.YRES(32f));
		Rect position2 = new Rect(r.x + GUIM.YRES(230f), r.y + GUIM.YRES(4f), GUIM.YRES(24f), GUIM.YRES(24f));
		GUIM.DrawBox(r, tGray, 0.2f);
		if (currplayer == p)
		{
			GUI.DrawTexture(new Rect(r.x, r.y, GUIM.YRES(2f), r.height), TEX.tYellow);
		}
		if (p.tAvatar == null)
		{
			p.tAvatar = FaceGen.Build(p.p0, p.p1, p.p2, p.p3, p.p4, p.p5, p.p6, p.p7, p.p8, p.p9);
		}
		GUI.DrawTexture(position, p.tAvatar);
		GUIM.DrawText(r2, p.n, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
		GUIM.DrawText(r3, p.sLV, TextAnchor.MiddleLeft, BaseColor.Orange, 1, 14, false);
		GUI.DrawTexture(position2, tRank[p.rank]);
	}
}
