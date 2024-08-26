using UnityEngine;

public class GUIGold : MonoBehaviour
{
	public class CGold
	{
		public int idx;

		public Rect r;

		public Rect rLabel;

		public Rect rCost;

		public Rect rBonus;

		public string label;

		public string cost;

		public string bonus;

		public Texture2D tex;

		public string codename;

		public CGold(int id, string label, string bonus, string cost, string codename)
		{
			idx = id;
			this.label = label;
			this.cost = cost;
			this.bonus = bonus;
			this.codename = codename;
		}

		~CGold()
		{
		}

		public void UpdateTexture()
		{
			tex = ContentLoader_.LoadTexture("coin_" + idx) as Texture2D;
			if (tex == null)
			{
				tex = ContentLoader_.LoadTexture("coin_4") as Texture2D;
			}
		}
	}

	public static bool show = false;

	public static GUIGold cs = null;

	public static bool draworderlist = false;

	private Texture2D tBlack;

	private Texture2D tBlue;

	private Texture2D tVig;

	private static string sEdit = "";

	private Rect rBack;

	private Rect rBack2;

	private Rect rHint;

	private CGold[] gold = new CGold[7];

	private static float clicktime = 0f;

	private string sCoin;

	private string sCoins;

	private string sVoice;

	private string sVoices;

	private string sOrderList;

	public static bool discount = false;

	private Rect rBack3;

	private Rect rZone;

	private Rect rScroll;

	public void LoadLang()
	{
		sCoin = Lang.GetString("_COIN");
		sCoins = Lang.GetString("_COINS");
		sVoice = Lang.GetString("_VOICE");
		sVoices = Lang.GetString("_VOICES");
		sOrderList = Lang.GetString("_ORDER_LIST");
	}

	public static void SetActive(bool val)
	{
		show = val;
		if (show)
		{
			GUIFX.Set();
		}
		else
		{
			GUIFX.End();
		}
		draworderlist = false;
	}

	private void Awake()
	{
		cs = this;
		gold[1] = new CGold(1, "30 " + sCoin, null, "5 голосов", "coinpack_1");
		gold[2] = new CGold(2, "60 " + sCoin, "+ 6 " + sCoin, "10 голосов", "coinpack_2");
		gold[3] = new CGold(3, "150 " + sCoin, "+ 20 " + sCoin, "25 голосов", "coinpack_3");
		gold[4] = new CGold(4, "300 " + sCoin, "+ 50 " + sCoin, "50 голосов", "coinpack_4");
		gold[5] = new CGold(5, "600 " + sCoin, "+ 120 " + sCoin, "100 голосов", "coinpack_5");
		discount = false;
		if (MainManager.steam)
		{
			discount = false;
		}
		if (discount)
		{
			gold[6] = new CGold(6, "90 монет", "+ 30 монет", "15 голосов", "coinpack_6");
		}
		if (MainManager.steam)
		{
			gold[0] = null;
			UpdatePrice("coinpack_1", "1$");
			UpdatePrice("coinpack_2", "2$");
			UpdatePrice("coinpack_3", "5$");
			UpdatePrice("coinpack_4", "10$");
			UpdatePrice("coinpack_5", "20$");
		}
		else if (MainManager.idc)
		{
			gold[0] = null;
			UpdatePrice("coinpack_1", "");
			UpdatePrice("coinpack_2", "");
			UpdatePrice("coinpack_3", "");
			UpdatePrice("coinpack_4", "");
			UpdatePrice("coinpack_5", "");
		}
		else if (MainManager.mycom)
		{
			gold[0] = null;
			if (MainManager.ui_currency == "RUB")
			{
				UpdatePrice("coinpack_1", "35₽");
				UpdatePrice("coinpack_2", "70₽");
				UpdatePrice("coinpack_3", "175₽");
				UpdatePrice("coinpack_4", "350₽");
				UpdatePrice("coinpack_5", "700₽");
			}
			else if (MainManager.ui_currency == "USD")
			{
				UpdatePrice("coinpack_1", "1$");
				UpdatePrice("coinpack_2", "2$");
				UpdatePrice("coinpack_3", "3$");
				UpdatePrice("coinpack_4", "4$");
				UpdatePrice("coinpack_5", "5$");
			}
			else if (MainManager.ui_currency == "EUR")
			{
				UpdatePrice("coinpack_1", "1€");
				UpdatePrice("coinpack_2", "2€");
				UpdatePrice("coinpack_3", "3€");
				UpdatePrice("coinpack_4", "4€");
				UpdatePrice("coinpack_5", "5€");
			}
			gold[1].bonus = "";
		}
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tBlue = TEX.GetTextureByName("blue");
		tVig = ContentLoader_.LoadTexture("vig") as Texture2D;
		UpdateTextures();
		OnResize();
	}

	public void UpdateTextures()
	{
		for (int i = 0; i < 7; i++)
		{
			if (gold[i] != null)
			{
				gold[i].UpdateTexture();
			}
		}
	}

	public void UpdatePrice(string codename, string price)
	{
		for (int i = 0; i < 6; i++)
		{
			if (gold[i] != null && gold[i].codename == codename)
			{
				gold[i].cost = price;
			}
		}
	}

	private void OnResize()
	{
		rBack3.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(490f));
		rZone.Set(rBack3.x + GUIM.YRES(200f), rBack3.y + GUIM.YRES(32f), GUIM.YRES(520f), rBack3.height - GUIM.YRES(64f));
		rBack.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(240f));
		rBack2.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(390f), GUIM.YRES(920f), GUIM.YRES(240f));
		float num = GUIM.YRES(100f);
		float num2 = num + GUIM.YRES(24f);
		float num3 = 0f;
		if (MainManager.steam)
		{
			num3 = num / 2f;
		}
		else if (MainManager.idc)
		{
			num3 = num / 2f;
		}
		else if (MainManager.mycom)
		{
			num3 = num / 2f;
		}
		num3 = num / 2f;
		for (int i = 0; i < 6; i++)
		{
			if (gold[i] != null)
			{
				if (gold[i].bonus == null)
				{
					gold[i].rLabel.Set(0f - num3 + rBack.x + GUIM.YRES(98f) + num2 * (float)i, rBack.y + GUIM.YRES(64f) - GUIM.YRES(32f), num, GUIM.YRES(26f));
					gold[i].rBonus.Set(0f - num3 + rBack.x + GUIM.YRES(98f) + num2 * (float)i, rBack.y + GUIM.YRES(64f) - GUIM.YRES(32f), num, GUIM.YRES(26f));
				}
				else
				{
					gold[i].rLabel.Set(0f - num3 + rBack.x + GUIM.YRES(98f) + num2 * (float)i, rBack.y + GUIM.YRES(64f) - GUIM.YRES(40f), num, GUIM.YRES(26f));
					gold[i].rBonus.Set(0f - num3 + rBack.x + GUIM.YRES(98f) + num2 * (float)i, rBack.y + GUIM.YRES(64f) - GUIM.YRES(20f), num, GUIM.YRES(20f));
				}
				gold[i].r.Set(0f - num3 + rBack.x + GUIM.YRES(98f) + num2 * (float)i, rBack.y + GUIM.YRES(64f), num, num);
				gold[i].rCost.Set(0f - num3 + rBack.x + GUIM.YRES(98f) + num2 * (float)i, rBack.y + GUIM.YRES(64f) + num, num, GUIM.YRES(26f));
			}
		}
		if (gold[6] != null)
		{
			gold[6].rLabel.Set(rBack2.x + GUIM.YRES(98f) + num2 * 0f, rBack2.y + GUIM.YRES(64f) - GUIM.YRES(40f), num, GUIM.YRES(26f));
			gold[6].rBonus.Set(rBack2.x + GUIM.YRES(98f) + num2 * 0f, rBack2.y + GUIM.YRES(64f) - GUIM.YRES(20f), num, GUIM.YRES(20f));
			gold[6].r.Set(rBack2.x + GUIM.YRES(98f) + num2 * 0f, rBack2.y + GUIM.YRES(64f), num, num);
			gold[6].rCost.Set(rBack2.x + GUIM.YRES(98f) + num2 * 0f, rBack2.y + GUIM.YRES(64f) + num, num, GUIM.YRES(26f));
			rHint.Set(rBack2.x + GUIM.YRES(98f) + num2 * 0f + num + GUIM.YRES(32f), rBack2.y + GUIM.YRES(64f), GUIM.YRES(200f), GUIM.YRES(20f));
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIFX.Begin();
			DrawItems();
			GUIFX.End();
		}
	}

	private void DrawItems()
	{
		if (draworderlist)
		{
			return;
		}
		GUIM.DrawBox(rBack, tBlack, 0.05f);
		for (int i = 0; i < 6; i++)
		{
			if (gold[i] != null)
			{
				DrawItem(gold[i]);
			}
		}
		if (discount)
		{
			GUIM.DrawBox(rBack2, tBlack, 0.05f);
			DrawItem(gold[6], true);
			GUIM.DrawText(rHint, "Акция действует до 15 мая (включительно)", TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
		}
	}

	private void DrawItem(CGold g, bool focus = false)
	{
		GUIM.DrawText(g.rLabel, g.label, TextAnchor.MiddleCenter, BaseColor.Yellow, 1, 16, true);
		GUIM.DrawText(g.rBonus, g.bonus, TextAnchor.MiddleCenter, BaseColor.Green, 1, 14, true);
		GUIM.DrawBox(g.r, focus ? TEX.GetTextureByName("red") : TEX.GetTextureByName("gray"), 1f);
		if (GUIM.Contains(g.r))
		{
			GUIM.DrawBoxBorder(g.r, TEX.GetTextureByName("yellow"), 1f);
			if (GUIM.HideButton(g.r) && Time.time > clicktime + 0.5f)
			{
				if (IAPManager.cs != null)
				{
					IAPManager.cs.BuyItem(g.idx);
				}
				MainManager.inorder = true;
				clicktime = Time.time;
			}
		}
		GUI.color = Color.black;
		GUI.DrawTexture(g.r, tVig);
		GUI.color = Color.white;
		GUI.DrawTexture(g.r, g.tex);
		GUIM.DrawText(g.rCost, g.cost, TextAnchor.MiddleCenter, BaseColor.White, 1, 16, true);
	}

	public void OpenUrl(string s)
	{
	}

	public void UpdateItem(string gpa, int net)
	{
	}

	public void RemoveItem(string sku)
	{
	}
}
