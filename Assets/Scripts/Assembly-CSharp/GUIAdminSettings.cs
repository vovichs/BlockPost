using UnityEngine;

public class GUIAdminSettings : MonoBehaviour
{
	public bool show;

	private Rect rBack;

	private Rect rCol0;

	private Rect[] rCol0Row;

	private Rect rCol1;

	private Rect[] rCol1Row;

	private Rect rCol2;

	private Rect[] rCol2Row;

	private Texture2D tBlack;

	private Texture2D tGray;

	private Texture2D tWhite;

	private Color[] cList;

	private int gamemode;

	private int srv_hidden = 1;

	private int mod_blockbuild;

	private int mod_blockdestr;

	private int mod_blockdamage;

	private string sGameMode;

	private string sServerVisibility;

	private string sVisibleAll;

	private string sHidden;

	private string sModifiers;

	private string sAllowBreak;

	private string sForbiddenBreak;

	private string sAllowBuild;

	private string sForbiddenBuild;

	private string sAllowAttack;

	private string sForbiddenAttack;

	private void Awake()
	{
		tBlack = TEX.GetTextureByName("black");
		tGray = TEX.GetTextureByName("gray");
		tWhite = TEX.GetTextureByName("white");
		cList = new Color[5];
		cList[0] = new Color(0.1f, 0.1f, 0.1f, 1f);
		cList[1] = new Color(0.2f, 0.2f, 0.2f, 1f);
		cList[2] = new Color(0.3f, 0.3f, 0.3f, 1f);
		cList[3] = new Color(0.1f, 0.2f, 0.4f, 1f);
		cList[4] = new Color(0.4f, 0.2f, 0.1f, 1f);
	}

	private void LoadLang()
	{
		sGameMode = Lang.GetString("_MODE_GAME");
		sServerVisibility = Lang.GetString("_VISIBILITY_SERVER");
		sVisibleAll = Lang.GetString("_VISIBLE_EVERYONE");
		sHidden = Lang.GetString("_HIDDEN");
		sModifiers = Lang.GetString("_MODIFIERS");
		sAllowBreak = Lang.GetString("_ALLOW_BREAK");
		sForbiddenBreak = Lang.GetString("_FORBIDDEN_BREAK");
		sAllowBuild = Lang.GetString("_ALLOW_BUILD");
		sForbiddenBuild = Lang.GetString("_FORBIDDEN_BUILD");
		sAllowAttack = Lang.GetString("_ALLOW_ATTACK");
		sForbiddenAttack = Lang.GetString("_FORBIDDEN_ATTACK");
	}

	private void Start()
	{
		OnResize();
		LoadLang();
	}

	private void OnResize()
	{
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(200f), (float)Screen.height / 2f - GUIM.YRES(200f), GUIM.YRES(600f), GUIM.YRES(400f));
		rCol0 = new Rect(rBack.x + GUIM.YRES(16f), rBack.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(26f));
		rCol0Row = new Rect[10];
		for (int i = 0; i < 10; i++)
		{
			rCol0Row[i] = new Rect(rCol0.x, rCol0.y + GUIM.YRES(26f) * (float)(i + 1), rCol0.width, rCol0.height);
		}
		rCol1 = new Rect(rBack.x + GUIM.YRES(16f) + GUIM.YRES(216f), rBack.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(26f));
		rCol1Row = new Rect[3];
		for (int j = 0; j < 3; j++)
		{
			rCol1Row[j] = new Rect(rCol1.x, rCol1.y + GUIM.YRES(26f) * (float)(j + 1), rCol1.width, rCol1.height);
		}
		rCol2 = new Rect(rBack.x + GUIM.YRES(16f) + GUIM.YRES(216f), rBack.y + GUIM.YRES(16f) * 3f + GUIM.YRES(26f) * 3f, GUIM.YRES(200f), GUIM.YRES(26f));
		rCol2Row = new Rect[4];
		for (int k = 0; k < 4; k++)
		{
			rCol2Row[k] = new Rect(rCol2.x, rCol2.y + GUIM.YRES(26f) * (float)(k + 1), rCol2.width, rCol2.height);
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			GUI.depth = -2;
			GUI.DrawTexture(rBack, tBlack);
			DrawModes();
			DrawVisible();
			DrawModif();
		}
	}

	private void DrawModes()
	{
		GUI.DrawTexture(rCol0, tGray);
		GUIM.DrawText(rCol0, sGameMode, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		DrawModeRow(rCol0Row[0], 0);
		DrawModeRow(rCol0Row[1], 1);
		DrawModeRow(rCol0Row[2], 2);
		DrawModeRow(rCol0Row[3], 4);
		DrawModeRow(rCol0Row[4], 5);
		DrawModeRow(rCol0Row[5], 6);
		DrawModeRow(rCol0Row[6], 7);
	}

	private void DrawModeRow(Rect r, int i)
	{
		if (gamemode == i)
		{
			GUI.color = cList[3];
			GUI.DrawTexture(r, tWhite);
			GUI.color = Color.white;
		}
		GUIM.DrawText(r, GUIPlay.sGameMode[i], TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		if (GUIM.HideButton(r))
		{
			gamemode = i;
			Client.cs.send_customop(11, gamemode, 0);
			GUIAdmin.cs.SetActive(false);
		}
	}

	private void DrawVisible()
	{
		GUI.DrawTexture(rCol1, tGray);
		GUIM.DrawText(rCol1, sServerVisibility, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		GUI.color = cList[3];
		GUI.DrawTexture(rCol1Row[0], tWhite);
		GUI.color = Color.white;
		if (srv_hidden == 0)
		{
			GUI.color = cList[3];
			GUI.DrawTexture(rCol1Row[0], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rCol1Row[0], sVisibleAll, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		else
		{
			GUI.color = cList[4];
			GUI.DrawTexture(rCol1Row[0], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rCol1Row[0], sHidden, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		if (GUIM.HideButton(rCol1Row[0]))
		{
			if (srv_hidden == 0)
			{
				srv_hidden = 1;
			}
			else
			{
				srv_hidden = 0;
			}
			Client.cs.send_customop(4, srv_hidden, 0);
		}
	}

	private void DrawModif()
	{
		GUI.DrawTexture(rCol2, tGray);
		GUIM.DrawText(rCol2, sModifiers, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (mod_blockdestr == 0)
		{
			GUI.color = cList[3];
			GUI.DrawTexture(rCol2Row[0], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rCol2Row[0], sAllowBreak, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		else
		{
			GUI.color = cList[4];
			GUI.DrawTexture(rCol2Row[0], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rCol2Row[0], sForbiddenBreak, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		if (GUIM.HideButton(rCol2Row[0]))
		{
			if (mod_blockdestr == 0)
			{
				mod_blockdestr = 1;
			}
			else
			{
				mod_blockdestr = 0;
			}
			Client.cs.send_customop(5, mod_blockdestr, 0);
		}
		if (mod_blockbuild == 0)
		{
			GUI.color = cList[3];
			GUI.DrawTexture(rCol2Row[1], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rCol2Row[1], sAllowBuild, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		else
		{
			GUI.color = cList[4];
			GUI.DrawTexture(rCol2Row[1], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rCol2Row[1], sForbiddenBuild, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		if (GUIM.HideButton(rCol2Row[1]))
		{
			if (mod_blockbuild == 0)
			{
				mod_blockbuild = 1;
			}
			else
			{
				mod_blockbuild = 0;
			}
			Client.cs.send_customop(6, mod_blockbuild, 0);
		}
		if (mod_blockdamage == 0)
		{
			GUI.color = cList[3];
			GUI.DrawTexture(rCol2Row[2], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rCol2Row[2], sAllowAttack, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		else
		{
			GUI.color = cList[4];
			GUI.DrawTexture(rCol2Row[2], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rCol2Row[2], sForbiddenAttack, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		if (GUIM.HideButton(rCol2Row[2]))
		{
			if (mod_blockdamage == 0)
			{
				mod_blockdamage = 1;
			}
			else
			{
				mod_blockdamage = 0;
			}
			Client.cs.send_customop(8, mod_blockdamage, 0);
		}
	}
}
