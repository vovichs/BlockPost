using UnityEngine;

public class GUIBonus : MonoBehaviour
{
	public class BonusItem
	{
		public int id;

		public string label;

		public int bid;

		public Rect r;

		public Rect rLabel;

		public Rect rPic;

		public Rect rPic0;

		public Rect rPic1;

		public Rect rButton;

		public BonusItem(int id, string label, int bid)
		{
			this.id = id;
			this.label = label;
			this.bid = bid;
		}
	}

	public static GUIBonus cs;

	public static bool show;

	private Texture2D tBlack;

	private Texture2D tBid2;

	private Texture2D tWhite;

	private Texture2D tYellow;

	private Rect rBack;

	private Rect rHelp;

	private Rect rNext;

	private string sLeft;

	private string sDay;

	private string sCome;

	private string sCont;

	private string sColl;

	private BonusItem[] item;

	private int act;

	private int day;

	private int sec;

	private string sHelp = "";

	public bool shownext;

	public void Set(int v0, int v1, int v2)
	{
		act = v0;
		day = v1;
		sec = v2;
		int num = sec / 3600;
		int num2 = (sec - num * 3600) / 60;
		sHelp = string.Format(sLeft, num, num2);
		if (act == 1)
		{
			shownext = true;
			Main.HideMenus();
			Main.currSubMenu = 1;
			GUIObj.show = true;
			cs._SetActive(true);
		}
	}

	private void Awake()
	{
		cs = this;
	}

	public void LoadLang()
	{
		sLeft = Lang.GetString("_BONUS_LEFT");
		sDay = Lang.GetString("_DAY");
		sCome = Lang.GetString("_COME_EVER_DAY");
		sCont = Lang.GetString("_CONTINUE");
		sColl = Lang.GetString("_COLLECT");
	}

	public void _SetActive(bool val)
	{
		if (val)
		{
			GUIFX.Set();
			if (item == null)
			{
				item = new BonusItem[28];
				for (int i = 0; i < 28; i++)
				{
					item[i] = new BonusItem(i, sDay + " " + (i + 1), 0);
					if ((i + 1) % 7 == 0)
					{
						item[i].bid = 1;
					}
					if (i == 27)
					{
						item[i].bid = 2;
					}
				}
			}
		}
		else
		{
			GUIFX.End();
			if (item != null)
			{
				item = null;
			}
		}
		OnResize();
		shownext = false;
	}

	private void OnResize()
	{
		if (item == null)
		{
			return;
		}
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(500f));
		int num = (int)GUIM.YRES(120f);
		int num2 = (int)GUIM.YRES(100f);
		int num3 = (int)GUIM.YRES(16f) + (int)rBack.x;
		int num4 = (int)GUIM.YRES(16f) + (int)rBack.y;
		int num5 = (int)GUIM.YRES(8f);
		for (int i = 0; i < 28; i++)
		{
			BonusItem bonusItem = item[i];
			bonusItem.r = new Rect(num3, num4, num, num2);
			num3 += num5 + num;
			if ((i + 1) % 7 == 0)
			{
				num4 += num5 + num2;
				num3 = (int)GUIM.YRES(16f) + (int)rBack.x;
			}
			bonusItem.rLabel = new Rect(bonusItem.r.x, bonusItem.r.y, bonusItem.r.width, GUIM.YRES(32f));
			bonusItem.rPic = new Rect(bonusItem.r.x + GUIM.YRES(41f), bonusItem.r.y + GUIM.YRES(38f), bonusItem.r.width - GUIM.YRES(82f), bonusItem.r.width - GUIM.YRES(82f));
			if (bonusItem.bid == 1)
			{
				bonusItem.rPic0 = new Rect(bonusItem.rPic.x - GUIM.YRES(16f), bonusItem.rPic.y, bonusItem.rPic.width, bonusItem.rPic.height);
				bonusItem.rPic1 = new Rect(bonusItem.rPic.x + GUIM.YRES(16f), bonusItem.rPic.y, bonusItem.rPic.width, bonusItem.rPic.height);
			}
			else if (bonusItem.bid == 2)
			{
				bonusItem.rPic = new Rect(bonusItem.r.x + GUIM.YRES(30f), bonusItem.r.y + GUIM.YRES(26f), bonusItem.r.width - GUIM.YRES(60f), bonusItem.r.width - GUIM.YRES(60f));
			}
			if (i == day && act == 1)
			{
				bonusItem.rPic0.y -= GUIM.YRES(10f);
				bonusItem.rPic.y -= GUIM.YRES(10f);
				bonusItem.rPic1.y -= GUIM.YRES(10f);
			}
			bonusItem.rButton = new Rect(bonusItem.r.x + GUIM.YRES(4f), bonusItem.r.y + bonusItem.r.height - GUIM.YRES(30f), bonusItem.r.width - GUIM.YRES(8f), GUIM.YRES(26f));
		}
		rHelp = new Rect(rBack.x + GUIM.YRES(16f), rBack.y + rBack.height - GUIM.YRES(48f), GUIM.YRES(200f), GUIM.YRES(32f));
		rNext = new Rect(rBack.x + rBack.width - GUIM.YRES(216f), rBack.y + rBack.height - GUIM.YRES(42f), GUIM.YRES(200f), GUIM.YRES(26f));
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		tYellow = TEX.GetTextureByName("orange");
		tBid2 = ContentLoader_.LoadTexture("case_skullcap_icon") as Texture2D;
	}

	public void _OnGUI()
	{
		GUIM.DrawBox(rBack, tBlack, 0.05f);
		for (int i = 0; i < 28; i++)
		{
			DrawItem(item[i]);
		}
		if (act == 0)
		{
			GUIM.DrawText(rHelp, sHelp, TextAnchor.MiddleLeft, BaseColor.White, 0, 16, false);
		}
		else
		{
			GUIM.DrawText(rHelp, sCome, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 16, false);
		}
		if (shownext && GUIM.Button(rNext, BaseColor.Orange, sCont, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Main.HideMenus();
			GUIPlay.SetActive(true);
			if (act == 1)
			{
				MasterClient.cs.send_bonus(1);
				act = 0;
			}
		}
	}

	private void DrawItem(BonusItem item)
	{
		if (item.id < day)
		{
			GUIM.DrawBox(item.r, tBlack, 0.15f);
		}
		else if (item.id == day && act == 1)
		{
			GUIM.DrawBox(item.r, tYellow, 0.2f);
		}
		else
		{
			GUIM.DrawBox(item.r, tBlack, 0.05f);
		}
		GUIM.DrawText(item.rLabel, item.label, TextAnchor.MiddleCenter, BaseColor.White, 0, 16, false);
		if (item.bid == 1)
		{
			GUI.DrawTexture(item.rPic0, Main.tCoin);
			GUI.DrawTexture(item.rPic, Main.tCoin);
			GUI.DrawTexture(item.rPic1, Main.tCoin);
		}
		else if (item.bid == 0)
		{
			GUI.DrawTexture(item.rPic, Main.tCoin);
		}
		else if (item.bid == 2)
		{
			GUI.DrawTexture(item.rPic, tBid2);
		}
		if (item.id == day && act == 1 && GUIM.Button(item.rButton, BaseColor.White, sColl, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			MasterClient.cs.send_bonus(1);
			act = 0;
			OnResize();
		}
	}
}
