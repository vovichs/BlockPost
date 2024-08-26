using Player;
using UnityEngine;

public class HUDTab : MonoBehaviour
{
	public static HUDTab cs = null;

	public static bool show = false;

	private static Texture2D tBlack;

	private static Texture2D tWhite;

	private static Texture2D tYellow;

	private static Rect rBack;

	private static Rect[] rBottom = new Rect[2];

	private static Rect[] rHeader = new Rect[2];

	private static string sTotal = "-";

	private static string sServer = "-";

	public static int scorebar = 0;

	private Rect rRadarGUI;

	private Rect rRadar;

	private Rect ra;

	private Rect rb;

	private Rect rBack0;

	private Rect[] rPlayer = new Rect[40];

	private Color a5 = new Color(1f, 1f, 1f, 0.5f);

	private static string sServ;

	private static string sTot;

	private static int c = 0;

	public static int[] sorted = new int[40];

	public static void SetActive(bool val)
	{
		show = val;
		if (val)
		{
			if (Controll.goRadarCam2 != null)
			{
				Controll.goRadarCam2.SetActive(true);
				Controll.trRadarCam2.transform.position = new Vector3((float)Map.BLOCK_SIZE_X / 2f, 128f, (float)Map.BLOCK_SIZE_X / 2f);
				Controll.csRadarCam2.orthographicSize = (float)Map.BLOCK_SIZE_X / 2f;
			}
			SortPlayers();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < 40; i++)
			{
				if (PLH.player[i] != null)
				{
					num4++;
					if (PLH.player[i].team == 0)
					{
						num++;
					}
					else if (PLH.player[i].team == 1)
					{
						num2++;
					}
					else
					{
						num3++;
					}
				}
			}
			sTotal = "<color=#F60>RD " + num + "</color> / <color=#06F>BL " + num2 + "</color> / SPEC " + num3 + " / <color=#FF0>" + sTot + " " + num4 + "</color>";
			if (MainManager.idc)
			{
				sServer = sServ + " #" + (Client.PORT - 20000 + 1);
			}
			else
			{
				sServer = sServ + " #" + (Client.PORT - 40000 + 1);
			}
		}
		else if (Controll.goRadarCam2 != null)
		{
			Controll.goRadarCam2.SetActive(false);
		}
	}

	private void Awake()
	{
		cs = this;
	}

	public void LoadLang()
	{
		sServ = Lang.GetString("_SERVER");
		sTot = Lang.GetString("_TOTAL");
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		tYellow = TEX.GetTextureByName("yellow");
	}

	public void SetScoreBar(int t)
	{
		scorebar = t;
		OnResize();
	}

	private void OnResize()
	{
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(400f), GUIM.YRES(120f), Screen.width, Screen.height);
		if (scorebar == 0)
		{
			OnResizeTabTeam();
		}
		else if (scorebar == 1)
		{
			OnResizeTabFree();
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			GUI.depth = -1;
			if (scorebar == 0)
			{
				DrawTabTeam();
			}
			else if (scorebar == 1)
			{
				DrawTabFree();
			}
		}
	}

	private void DrawPlayer(Rect r, PlayerData pl)
	{
		GUI.color = new Color(1f, 1f, 1f, 0.25f);
		if (Controll.pl == pl)
		{
			GUI.DrawTexture(r, tYellow);
		}
		else if (DemoRec.isDemo() && Controll.demoidx == pl.idx)
		{
			GUI.DrawTexture(r, TEX.tBlue);
		}
		else
		{
			GUI.DrawTexture(r, tWhite);
		}
		GUI.color = Color.white;
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(4f), r.width, r.height), pl.sLVL, TextAnchor.MiddleLeft, BaseColor.Orange, 1, 18, true);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(26f), r.y - GUIM.YRES(3f), r.width, r.height), pl.name, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, true);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(26f), r.y + GUIM.YRES(11f), r.width, r.height), pl.clanname, TextAnchor.MiddleLeft, BaseColor.White, 1, 10, false);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f) + GUIM.YRES(200f), r.y - GUIM.YRES(4f), GUIM.YRES(30f), r.height), pl.sEXP, TextAnchor.MiddleRight, BaseColor.Yellow, 1, 12, true);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f) + GUIM.YRES(200f), r.y + GUIM.YRES(10f), GUIM.YRES(30f), r.height), pl.sFDA, TextAnchor.MiddleRight, BaseColor.White, 1, 12, false);
	}

	private void DrawPlayerHeader(Rect r)
	{
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f), r.y + GUIM.YRES(2f), r.width, r.height), "LV PLAYER CLAN", TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(8f) + GUIM.YRES(200f), r.y + GUIM.YRES(2f), GUIM.YRES(30f), r.height), "SCORE K / D / A", TextAnchor.MiddleRight, BaseColor.White, 1, 14, false);
	}

	public static void SortPlayers()
	{
		c = 0;
		for (int i = 0; i < 40; i++)
		{
			sorted[i] = -1;
		}
		for (int j = 0; j < 40; j++)
		{
			if (PLH.player[j] != null)
			{
				sorted[c] = j;
				c++;
			}
		}
		if (c <= 1)
		{
			return;
		}
		bool flag = false;
		while (!flag)
		{
			flag = true;
			for (int k = 1; k < c; k++)
			{
				int num = sorted[k];
				int num2 = sorted[k - 1];
				if (PLH.player[num].exp > PLH.player[num2].exp)
				{
					sorted[k - 1] = num;
					sorted[k] = num2;
					flag = false;
				}
			}
		}
	}

	private void DrawTabTeam()
	{
		int num = (int)GUIM.YRES(28f);
		int num2 = (int)GUIM.YRES(1f);
		if (num2 < 1)
		{
			num2 = 1;
		}
		float num3 = (num + num2) * 20 + num2;
		float num4 = ((float)Screen.height - num3) / 2f;
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - GUIM.YRES(400f), num4, GUIM.YRES(250f), num3), tBlack);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f + GUIM.YRES(150f), num4, GUIM.YRES(250f), num3), tBlack);
		float num5 = (float)Screen.height / 2f - GUIM.YRES(150f) - num4;
		GUI.color = new Color(1f, 1f, 1f, 0.25f);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - GUIM.YRES(150f), num4, GUIM.YRES(300f), num5), tBlack);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - GUIM.YRES(150f), num4 + num3, GUIM.YRES(300f), 0f - num5), tBlack);
		GUI.color = Color.white;
		DrawPlayerHeader(new Rect(ra.x + (float)num2, ra.y + (float)num2 + (float)((num + num2) * -1), ra.width, num));
		DrawPlayerHeader(new Rect(rb.x + (float)num2, rb.y + (float)num2 + (float)((num + num2) * -1), rb.width, num));
		int num6 = 0;
		for (int i = 0; i < 40; i++)
		{
			if (sorted[i] >= 0)
			{
				int num7 = sorted[i];
				if (PLH.player[num7] != null && PLH.player[num7].team == 0)
				{
					DrawPlayer(new Rect(ra.x + (float)num2, ra.y + (float)num2 + (float)((num + num2) * num6), ra.width, num), PLH.player[num7]);
					num6++;
				}
			}
		}
		num6 = 0;
		for (int j = 0; j < 40; j++)
		{
			if (sorted[j] >= 0)
			{
				int num8 = sorted[j];
				if (PLH.player[num8] != null && PLH.player[num8].team == 1)
				{
					DrawPlayer(new Rect(rb.x + (float)num2, rb.y + (float)num2 + (float)((num + num2) * num6), rb.width, num), PLH.player[num8]);
					num6++;
				}
			}
		}
		DrawBottom();
	}

	private void DrawBottom()
	{
		GUIM.DrawTextColor(rBottom[0], sTotal, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, false);
		GUIM.DrawText(rBottom[1], sServer, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, false);
	}

	private void OnResizeTabTeam()
	{
		int num = (int)GUIM.YRES(28f);
		int num2 = (int)GUIM.YRES(1f);
		if (num2 < 1)
		{
			num2 = 1;
		}
		float num3 = (num + num2) * 20 + num2;
		float y = ((float)Screen.height - num3) / 2f;
		rRadarGUI = new Rect((float)Screen.width / 2f - GUIM.YRES(150f), (float)Screen.height / 2f - GUIM.YRES(150f), GUIM.YRES(300f), GUIM.YRES(300f));
		ra = new Rect((float)Screen.width / 2f - GUIM.YRES(400f), y, GUIM.YRES(250f) - (float)(2 * num2), num3);
		rb = new Rect((float)Screen.width / 2f + GUIM.YRES(150f), y, GUIM.YRES(250f) - (float)(2 * num2), num3);
		rBottom[0].Set(ra.x + (float)num2, ra.y + (float)num2 + (float)((num + num2) * 20), ra.width, num);
		rBottom[1].Set(rb.x, rBottom[0].y, rBottom[0].width, rBottom[0].height);
		rRadar = new Rect(rRadarGUI.x / (float)Screen.width, rRadarGUI.y / (float)Screen.height, rRadarGUI.width / (float)Screen.width, rRadarGUI.height / (float)Screen.height);
		if (Controll.csRadarCam2 != null)
		{
			Controll.csRadarCam2.rect = rRadar;
		}
	}

	private void OnResizeTabFree()
	{
		int num = (int)GUIM.YRES(28f);
		int num2 = (int)GUIM.YRES(1f);
		if (num2 < 1)
		{
			num2 = 1;
		}
		float num3 = (num + num2) * 16 + num2;
		float y = ((float)Screen.height - num3) / 2f;
		for (int i = 0; i < 40; i++)
		{
			rPlayer[i] = new Rect(ra.x + (float)num2, ra.y + (float)num2 + (float)((num + num2) * i), ra.width, num);
		}
		float num4 = num3 - GUIM.YRES(96f);
		float num5 = (GUIM.YRES(550f) - num4) / 2f;
		ra = new Rect((float)Screen.width / 2f - GUIM.YRES(400f), y, GUIM.YRES(250f) - (float)(2 * num2), num3);
		rb = new Rect((float)Screen.width / 2f + GUIM.YRES(150f), y, GUIM.YRES(250f) - (float)(2 * num2), num3);
		rBack0 = new Rect((float)Screen.width / 2f - GUIM.YRES(400f), y, GUIM.YRES(250f), num3);
		rRadarGUI = new Rect((float)Screen.width / 2f - GUIM.YRES(150f) + num5, (float)Screen.height / 2f - num4 / 2f, num4, num4);
		rHeader[0] = new Rect(ra.x + (float)num2, ra.y + (float)num2 + (float)((num + num2) * -1), ra.width, num);
		rBottom[0].Set(ra.x + (float)num2, ra.y + (float)num2 + (float)((num + num2) * 16), ra.width, num);
		rBottom[1].Set(rb.x, rBottom[0].y, rBottom[0].width, rBottom[0].height);
		rRadar = new Rect(rRadarGUI.x / (float)Screen.width, rRadarGUI.y / (float)Screen.height, rRadarGUI.width / (float)Screen.width, rRadarGUI.height / (float)Screen.height);
		if (Controll.csRadarCam2 != null)
		{
			Controll.csRadarCam2.rect = rRadar;
		}
	}

	private void DrawTabFree()
	{
		OnResizeTabFree();
		GUI.color = a5;
		GUI.DrawTexture(rBack0, tBlack);
		DrawPlayerHeader(rHeader[0]);
		DrawBottom();
		int num = 0;
		for (int i = 0; i < 40; i++)
		{
			if (sorted[i] >= 0)
			{
				int num2 = sorted[i];
				if (PLH.player[num2] != null)
				{
					DrawPlayer(rPlayer[num], PLH.player[num2]);
					num++;
				}
			}
		}
		DrawRadarDebug();
	}

	private void DrawRadarDebug()
	{
		if (Controll.csRadarCam2 == null)
		{
			GUI.DrawTexture(rRadarGUI, tWhite);
		}
	}
}
