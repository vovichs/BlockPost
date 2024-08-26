using System.Collections.Generic;
using GameClass;
using UnityEngine;

public class GUIObj : MonoBehaviour
{
	public static bool show = false;

	private static bool changed = false;

	private static Texture2D tBlack = null;

	private static Texture2D tBlue = null;

	private static Texture2D tWhite = null;

	private static Texture2D tGray = null;

	private static Texture2D tYellow = null;

	private static Texture2D tIcon = null;

	private static Texture2D tIconStar = null;

	private static Texture2D tIconGift = null;

	private static Rect rBack;

	private static Rect rZone;

	public static QuestData[] qinfo = new QuestData[128];

	public static List<Quest> qlist = new List<Quest>();

	public static int currqid = -1;

	private static string[] sHeader = new string[3];

	private static string[] sHeaderDesc = new string[3];

	private int lineheight;

	private static string sNewLvl;

	private static string sReachLvl;

	private static string sShTrooper;

	private static string sFrAssaultRifle;

	private static string sMadeInRussia;

	private static string sMadeInRussia2;

	private static string sFrRussianWeap;

	private static string sSniper;

	private static string sFrSniperRifle;

	private static string sSecondWeap;

	private static string sFrPistol;

	private static string sGiftCaseAnniv;

	private static string sGiftKeyAnniv;

	private static string sFrHeadShot;

	private static string sVicDay;

	private static string sGiftCaseVicday;

	private static string sGiftKeyVicday;

	private static string sVictoryCase;

	private static string sFrWeapCase;

	private static string sGiftCaseHlWn;

	private static string sGiftKeyHlWn;

	private static string sFrMeleeWeap;

	private static string sHappyNewYear;

	private static string sFrWeap;

	private string sReward;

	private string sTakeAward;

	private string sSelectTask;

	private string sSelected;

	private Vector2 scroll = Vector2.zero;

	private float yofs;

	public void LoadLang()
	{
		sHeader[0] = Lang.GetString("_REGULAR_TASKS");
		sHeader[1] = Lang.GetString("_ACTIVE_TASKS");
		sHeader[2] = Lang.GetString("_ACTIVE_TASKS");
		sHeaderDesc[0] = Lang.GetString("_PROGRESS_ACTIVITIES");
		sHeaderDesc[1] = Lang.GetString("_MUST_SELECT_TASK");
		sHeaderDesc[2] = Lang.GetString("_MUST_SELECT_TASK");
		sNewLvl = Lang.GetString("_NEW_LVL");
		sReachLvl = Lang.GetString("_REACH_LVL");
		sShTrooper = Lang.GetString("_SHTORM_TROOPER");
		sFrAssaultRifle = Lang.GetString("_FRAGS_ASSAULT_RIFLE");
		sMadeInRussia = Lang.GetString("_MADE_IN_RUSSIA");
		sMadeInRussia2 = Lang.GetString("_MADE_IN_RUSSIA2");
		sFrRussianWeap = Lang.GetString("_FRAGS_RUSSIAN_WEAPON");
		sSniper = Lang.GetString("_SNIPER");
		sFrSniperRifle = Lang.GetString("_FRAGS_SNIPER_RIFLE");
		sSecondWeap = Lang.GetString("_SECONDARY_WEAPON");
		sFrPistol = Lang.GetString("_FRAGS_PISTOL");
		sGiftCaseAnniv = Lang.GetString("_GIFT_CASE_ANNIVERSARY");
		sGiftKeyAnniv = Lang.GetString("_GIFT_KEY_ANNIVERSARY");
		sFrHeadShot = Lang.GetString("_FRAGS_HEADSHOT");
		sVicDay = Lang.GetString("_VICTORY_DAY");
		sGiftCaseVicday = Lang.GetString("_GIFT_CASE_VICTORYDAY");
		sGiftKeyVicday = Lang.GetString("_GIFT_KEY_VICTORYDAY");
		sVictoryCase = Lang.GetString("_VICTORY_CASE");
		sFrWeapCase = Lang.GetString("_FRAGS_ANY_WEAPON_THIS_CASE");
		sGiftCaseHlWn = Lang.GetString("_GIFT_CASE_HALLOWEEN");
		sGiftKeyHlWn = Lang.GetString("_GIFT_KEY_HALLOWEEN");
		sFrMeleeWeap = Lang.GetString("_FRAGS_MELEE_WEAPON");
		sHappyNewYear = Lang.GetString("_HAPPY_NEW_YEAR");
		sFrWeap = Lang.GetString("_FRAGS_ANY_WEAPON");
		sReward = Lang.GetString("_REWARD");
		sTakeAward = Lang.GetString("_AWARE_AWARD");
		sSelectTask = Lang.GetString("_SELECT_TASK");
		sSelected = Lang.GetString("_SELECTED");
	}

	public static void SetActive(bool val)
	{
		show = val;
		if (val)
		{
			GUIFX.Set();
			if (GUIInv.cinfo[7] == null)
			{
				return;
			}
			if (qinfo[0] == null)
			{
				qinfo[0] = new QuestData(0, sNewLvl, sReachLvl, null, GUIInv.cinfo[7].img, 1, 0);
			}
			if (qinfo[1] == null)
			{
				qinfo[1] = new QuestData(1, sNewLvl + " II", sReachLvl, null, GUIInv.kinfo[4].img, 2, 0);
			}
			if (qinfo[4] == null)
			{
				qinfo[4] = new QuestData(4, sShTrooper, sFrAssaultRifle, null, GUIInv.cinfo[9].img, 1, 1);
			}
			if (qinfo[5] == null)
			{
				qinfo[5] = new QuestData(5, sMadeInRussia, sFrRussianWeap, null, GUIInv.cinfo[10].img, 1, 1);
			}
			if (qinfo[6] == null)
			{
				qinfo[6] = new QuestData(6, sSniper, sFrSniperRifle, null, (GUIInv.cinfo[11] != null) ? GUIInv.cinfo[11].img : null, 1, 1);
			}
			if (qinfo[7] == null)
			{
				qinfo[7] = new QuestData(7, sSecondWeap, sFrPistol, null, (GUIInv.cinfo[12] != null) ? GUIInv.cinfo[12].img : null, 1, 1);
			}
			if (qinfo[8] == null)
			{
				qinfo[8] = new QuestData(8, "Skullcap Case", sGiftCaseAnniv, null, (GUIInv.cinfo[13] != null) ? GUIInv.cinfo[13].img : null, 1, 0);
			}
			if (qinfo[9] == null)
			{
				qinfo[9] = new QuestData(9, "Skullcap Key", sGiftKeyAnniv, null, (GUIInv.kinfo[10] != null) ? GUIInv.kinfo[10].img : null, 2, 0);
			}
			if (qinfo[10] == null)
			{
				qinfo[10] = new QuestData(10, "SKULLSHOT", sFrHeadShot, null, (GUIInv.cinfo[13] != null) ? GUIInv.cinfo[13].img : null, 1, 1);
			}
			if (qinfo[11] == null)
			{
				qinfo[11] = new QuestData(11, sVicDay, sGiftCaseVicday, null, (GUIInv.cinfo[14] != null) ? GUIInv.cinfo[14].img : null, 1, 0);
			}
			if (qinfo[12] == null)
			{
				qinfo[12] = new QuestData(12, sVicDay, sGiftKeyVicday, null, (GUIInv.kinfo[11] != null) ? GUIInv.kinfo[11].img : null, 2, 0);
			}
			if (qinfo[13] == null)
			{
				qinfo[13] = new QuestData(13, sVictoryCase, sFrWeapCase, null, (GUIInv.cinfo[14] != null) ? GUIInv.cinfo[14].img : null, 1, 1);
			}
			if (qinfo[14] == null)
			{
				qinfo[14] = new QuestData(14, sMadeInRussia2, sFrRussianWeap, null, (GUIInv.cinfo[15] != null) ? GUIInv.cinfo[15].img : null, 1, 1);
			}
			if (!MainManager.steam && qinfo[17] == null)
			{
				qinfo[17] = new QuestData(17, "Halloween Case", sFrMeleeWeap, null, (GUIInv.cinfo[16] != null) ? GUIInv.cinfo[16].img : null, 1, 1);
			}
			if (qinfo[18] == null)
			{
				qinfo[18] = new QuestData(18, "❄2018❄", sHappyNewYear, null, (GUIInv.cinfo[17] != null) ? GUIInv.cinfo[17].img : null, 1, 0);
			}
			if (qinfo[19] == null)
			{
				qinfo[19] = new QuestData(19, "❄2018❄", sHappyNewYear, null, (GUIInv.kinfo[14] != null) ? GUIInv.kinfo[14].img : null, 2, 0);
			}
			if (qinfo[20] == null)
			{
				qinfo[20] = new QuestData(20, "Winter Case", sFrWeap, null, (GUIInv.cinfo[17] != null) ? GUIInv.cinfo[17].img : null, 1, 1);
			}
			if (qinfo[21] == null)
			{
				qinfo[21] = new QuestData(21, "BP Gift", "Подарок в честь годовщины игры (оружие)", null, Resources.Load("block_logo") as Texture2D, 1, 0);
			}
			if (qinfo[22] == null)
			{
				qinfo[22] = new QuestData(22, "BP Gift", "Подарок в честь годовщины игры (15 монет)", null, Resources.Load("block_logo") as Texture2D, 1, 0);
			}
			if (qinfo[23] == null)
			{
				qinfo[23] = new QuestData(23, "ДЕНЬ ПОБЕДЫ", "Подарочный кейс в честь Дня Победы", null, (GUIInv.cinfo[14] != null) ? GUIInv.cinfo[14].img : null, 1, 0);
			}
			if (qinfo[24] == null)
			{
				qinfo[24] = new QuestData(24, "ДЕНЬ ПОБЕДЫ", "Подарочный ключ в честь Дня Победы", null, (GUIInv.kinfo[11] != null) ? GUIInv.kinfo[11].img : null, 2, 0);
			}
			if (qinfo[25] == null)
			{
				qinfo[25] = new QuestData(25, "ДЕНЬ ПОБЕДЫ", "Подарочный кейс в честь Дня Победы", null, (GUIInv.cinfo[18] != null) ? GUIInv.cinfo[18].img : null, 1, 0);
			}
			if (qinfo[26] == null)
			{
				qinfo[26] = new QuestData(26, "ДЕНЬ ПОБЕДЫ", "Подарочный ключ в честь Дня Победы", null, (GUIInv.kinfo[15] != null) ? GUIInv.kinfo[15].img : null, 2, 0);
			}
			if (qinfo[27] == null)
			{
				qinfo[27] = new QuestData(27, sVictoryCase + " 2018", sFrWeapCase, null, (GUIInv.cinfo[18] != null) ? GUIInv.cinfo[18].img : null, 1, 1);
			}
			if (qinfo[28] == null)
			{
				qinfo[28] = new QuestData(28, "Razor Case", "Update Gift!", null, (GUIInv.cinfo[19] != null) ? GUIInv.cinfo[19].img : null, 1, 0);
			}
			if (qinfo[29] == null)
			{
				qinfo[29] = new QuestData(29, "Razor Key", "Update Gift!", null, (GUIInv.kinfo[16] != null) ? GUIInv.kinfo[16].img : null, 2, 0);
			}
			if (qinfo[30] == null)
			{
				qinfo[30] = new QuestData(30, "Razor Case", sFrMeleeWeap, null, (GUIInv.cinfo[19] != null) ? GUIInv.cinfo[19].img : null, 1, 1);
			}
			if (qinfo[33] == null)
			{
				qinfo[33] = new QuestData(33, "Future Case", sFrWeap, null, (GUIInv.cinfo[20] != null) ? GUIInv.cinfo[20].img : null, 1, 1);
			}
			if (!MainManager.steam && qinfo[39] == null)
			{
				qinfo[39] = new QuestData(39, "NY Case", sFrWeap, null, (GUIInv.cinfo[22] != null) ? GUIInv.cinfo[22].img : null, 1, 1);
			}
			if (qinfo[44] == null)
			{
				qinfo[44] = new QuestData(44, "Steel Case", sFrWeap, null, (GUIInv.cinfo[23] != null) ? GUIInv.cinfo[23].img : null, 1, 1);
			}
			if (MainManager.steam)
			{
				if (qinfo[52] == null)
				{
					qinfo[52] = new QuestData(52, "Halloween Case #2", sFrMeleeWeap, null, (GUIInv.cinfo[24] != null) ? GUIInv.cinfo[24].img : null, 1, 1);
				}
				if (qinfo[56] == null)
				{
					qinfo[56] = new QuestData(56, "NY2020 Case", sFrWeap, null, (GUIInv.cinfo[25] != null) ? GUIInv.cinfo[25].img : null, 1, 1);
				}
			}
			else
			{
				if (qinfo[52] == null)
				{
					qinfo[52] = new QuestData(52, "Halloween Case #2", sFrMeleeWeap, null, (GUIInv.cinfo[24] != null) ? GUIInv.cinfo[24].img : null, 1, 1);
				}
				if (qinfo[56] == null)
				{
					qinfo[56] = new QuestData(56, "NY2020 Case", sFrWeap, null, (GUIInv.cinfo[25] != null) ? GUIInv.cinfo[25].img : null, 1, 1);
				}
			}
			if (qinfo[63] == null)
			{
				qinfo[63] = new QuestData(63, "Kitchen Knife", sFrWeap.Replace("1000", "100"), null, Resources.Load("kitchen_icon") as Texture2D, 1, 2);
			}
			if (qinfo[64] == null)
			{
				qinfo[64] = new QuestData(64, "Sawed-off", sFrWeap.Replace("1000", "250"), null, Resources.Load("sawed_off_icon") as Texture2D, 1, 2);
			}
			if (qinfo[65] == null)
			{
				qinfo[65] = new QuestData(65, "Brass Knuckles", sFrWeap.Replace("1000", "500"), null, Resources.Load("brass_knuckles_icon") as Texture2D, 1, 2);
			}
			if (qinfo[66] == null)
			{
				qinfo[66] = new QuestData(66, "AKS74", sFrWeap.Replace("1000", "1000"), null, Resources.Load("aks74_icon") as Texture2D, 1, 2);
			}
			if (qinfo[67] == null)
			{
				qinfo[67] = new QuestData(67, "21 Coin", sHappyNewYear, null, Resources.Load("block_logo") as Texture2D, 1, 0);
			}
			QuestData.CalcCount(qinfo);
			MasterClient.cs.send_questlist();
		}
		else
		{
			if (changed)
			{
				MasterClient.cs.send_savequest(currqid);
				PlayerPrefs.SetInt("bp_quest", currqid);
			}
			GUIFX.End();
		}
		changed = false;
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		tBlue = TEX.GetTextureByName("blue");
		tGray = TEX.GetTextureByName("gray");
		tYellow = TEX.GetTextureByName("yellow");
		tIcon = Resources.Load("quest_icon") as Texture2D;
		tIconStar = Resources.Load("quest_icon_star") as Texture2D;
		tIconGift = Resources.Load("quest_icon_gift") as Texture2D;
	}

	private void OnResize()
	{
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(370f), GUIM.YRES(140f), GUIM.YRES(740f), GUIM.YRES(500f));
		lineheight = (int)GUIM.YRES(3f);
		OnResize_zone();
	}

	public static void OnResize_zone()
	{
		rZone = new Rect(0f, 0f, 0f, GUIM.YRES(80 + 88 * qlist.Count));
	}

	private void OnChangeSubMenu()
	{
		if (show)
		{
			if (Main.currSubMenu == 1)
			{
				GUIBonus.cs._SetActive(true);
			}
			else
			{
				GUIBonus.cs._SetActive(false);
			}
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIFX.Begin();
			if (Main.currSubMenu == 1)
			{
				GUIBonus.cs._OnGUI();
			}
			else
			{
				_OnGUI();
			}
			GUIFX.End();
		}
	}

	private void _OnGUI()
	{
		yofs = 0f;
		scroll = GUIM.BeginScrollView(rBack, scroll, rZone);
		yofs = DrawQuests(yofs, 0);
		yofs = DrawQuests(yofs, 2);
		yofs = DrawQuests(yofs, 1);
		GUIM.EndScrollView();
		rZone.height = yofs - GUIM.YRES(8f);
	}

	private float DrawQuests(float yofs, int active)
	{
		if (Quest.qcount[active] == 0)
		{
			return yofs;
		}
		Rect r = new Rect(0f, yofs, GUIM.YRES(200f), GUIM.YRES(32f));
		Rect r2 = new Rect(GUIM.YRES(200f) + GUIM.YRES(8f), yofs, GUIM.YRES(512f), GUIM.YRES(32f));
		GUIM.DrawBox(r, tWhite, 0.15f);
		GUIM.DrawBox(r2, tBlack, 0.02f);
		GUIM.DrawText(r, sHeader[active], TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		GUIM.DrawText(r2, sHeaderDesc[active], TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
		yofs += GUIM.YRES(40f);
		for (int i = 0; i < qlist.Count; i++)
		{
			if (qlist[i].data.active == active)
			{
				Rect r3 = new Rect(0f, yofs, GUIM.YRES(720f), GUIM.YRES(80f));
				DrawObj(r3, qlist[i]);
				yofs += GUIM.YRES(88f);
			}
		}
		return yofs;
	}

	private void DrawObj(Rect r, Quest q)
	{
		if (currqid == q.data.id)
		{
			GUIM.DrawBox(r, tBlue, 0.05f);
		}
		else
		{
			GUIM.DrawBox(r, tBlack, 0.05f);
		}
		Rect rect = new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(8f), GUIM.YRES(64f), GUIM.YRES(64f));
		GUIM.DrawBox(rect, tBlack);
		if (q.data.id == 0 || q.data.id == 1)
		{
			GUI.DrawTexture(rect, tIcon);
		}
		else if (q.data.id == 8 || q.data.id == 9 || q.data.id == 21 || q.data.id == 22)
		{
			GUI.DrawTexture(rect, tIconGift);
		}
		else
		{
			GUI.DrawTexture(rect, tIconStar);
		}
		Rect r2 = new Rect(r.x + GUIM.YRES(96f), r.y + GUIM.YRES(10f), GUIM.YRES(200f), GUIM.YRES(20f));
		Rect r3 = new Rect(r.x + GUIM.YRES(96f), r.y + GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(20f));
		GUIM.DrawText(r2, q.data.label, TextAnchor.MiddleLeft, BaseColor.White, 1, 16, false);
		GUIM.DrawText(r3, q.text, TextAnchor.UpperLeft, BaseColor.LightGray2, 1, 16, false);
		Rect position = new Rect(r.x + GUIM.YRES(96f), r.y + GUIM.YRES(60f), GUIM.YRES(350f), lineheight);
		Rect position2 = new Rect(r.x + GUIM.YRES(96f), r.y + GUIM.YRES(60f), GUIM.YRES(350f) * q.progress * 0.01f, lineheight);
		GUI.DrawTexture(position, tBlack);
		GUI.DrawTexture(position2, tYellow);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(460f), r.y + GUIM.YRES(63f), GUIM.YRES(40f), lineheight), q.sProgress, TextAnchor.MiddleLeft, BaseColor.Yellow, 1, 16, false);
		GUIM.DrawText(new Rect(r2.x + GUIM.YRES(435f), r.y, r2.width, r2.height), sReward, TextAnchor.MiddleLeft, BaseColor.Gray, 1, 10, false);
		Rect position3 = default(Rect);
		if (q.data.c == 1)
		{
			position3 = new Rect(r.x + GUIM.YRES(516f), r.y + GUIM.YRES(16f), GUIM.YRES(64f), GUIM.YRES(64f));
		}
		else if (q.data.c == 2)
		{
			position3 = new Rect(r.x + GUIM.YRES(524f), r.y + GUIM.YRES(16f), GUIM.YRES(48f), GUIM.YRES(48f));
		}
		if (q.data.rewardicon != null)
		{
			GUI.DrawTexture(position3, q.data.rewardicon);
		}
		if (q.complete > 0 && GUIM.Button(new Rect(r.x + r.width - GUIM.YRES(128f), r.y + GUIM.YRES(8f), GUIM.YRES(120f), GUIM.YRES(28f)), BaseColor.White, sTakeAward, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			MasterClient.cs.send_questreward(q.data.id);
		}
		if (q.data.active < 1)
		{
			return;
		}
		Rect r4 = new Rect(r.x + r.width - GUIM.YRES(128f), r.y + GUIM.YRES(44f), GUIM.YRES(120f), GUIM.YRES(28f));
		if (q.data.id != currqid)
		{
			if (GUIM.Button(r4, BaseColor.Yellow, sSelectTask, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				currqid = q.data.id;
				changed = true;
			}
		}
		else
		{
			GUIM.DrawText(r4, sSelected, TextAnchor.MiddleCenter, BaseColor.Green, 0, 20, false);
		}
	}
}
