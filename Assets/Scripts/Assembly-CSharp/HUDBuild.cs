using UnityEngine;

public class HUDBuild : MonoBehaviour
{
	public static bool show = false;

	private Texture2D tIconFly;

	private Texture2D tBlack;

	private Texture2D tYellow;

	private Texture2D tWhite;

	private static int[] pos = new int[3];

	private static string sPos = "";

	private Rect[] rEnt = new Rect[2];

	private string[] sEnt = new string[2] { "SPWN\nA", "SPWN\nB" };

	private Rect[] rt = new Rect[5];

	private static Vector2 sv = Vector2.zero;

	public static void SetActive(bool val)
	{
		show = val;
	}

	private void OnResize()
	{
		for (int i = 0; i < 2; i++)
		{
			rEnt[i] = new Rect((float)Screen.width / 2f - GUIM.YRES(200f) + GUIM.YRES(50f) * (float)i, GUIM.YRES(540f), GUIM.YRES(48f), GUIM.YRES(48f));
		}
		rt[0].Set(GUIM.YRES(16f), (float)Screen.height - GUIM.YRES(40f), GUIM.YRES(80f), GUIM.YRES(20f));
		rt[1].Set(GUIM.YRES(16f) + GUIM.YRES(88f), (float)Screen.height - GUIM.YRES(40f), GUIM.YRES(64f), GUIM.YRES(20f));
		rt[2].Set(GUIM.YRES(16f) + GUIM.YRES(88f) * 2f, (float)Screen.height - GUIM.YRES(40f), GUIM.YRES(64f), GUIM.YRES(20f));
		rt[3].Set(GUIM.YRES(16f) + GUIM.YRES(88f) * 3f, (float)Screen.height - GUIM.YRES(40f), GUIM.YRES(64f), GUIM.YRES(20f));
		rt[4].Set(GUIM.YRES(16f) + GUIM.YRES(88f) * 4f, (float)Screen.height - GUIM.YRES(40f), GUIM.YRES(64f), GUIM.YRES(20f));
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tYellow = TEX.GetTextureByName("yellow");
		tWhite = TEX.GetTextureByName("white");
		tIconFly = ContentLoader_.LoadTexture("icon_fly") as Texture2D;
	}

	private void OnGUI()
	{
		if (show)
		{
			DrawAtlasSelect();
			DrawEntSelect();
			DrawIcons();
			DrawToolSelect();
			DrawPosition();
			DrawPrefabs();
		}
	}

	private void DrawEntSelect()
	{
		if (Builder.toolmode != 1)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			GUI.DrawTexture(rEnt[i], tBlack);
			if (Builder.current == i)
			{
				GUIM.DrawText(rEnt[i], sEnt[i], TextAnchor.MiddleCenter, BaseColor.Red, 0, 20, false);
			}
			else
			{
				GUIM.DrawText(rEnt[i], sEnt[i], TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
			}
		}
	}

	private void DrawAtlasSelect()
	{
		if (Builder.toolmode != 0 || Controll.toolmode != 0 || Atlas.tex == null)
		{
			return;
		}
		int num = Builder.currblock - 5;
		int num2 = Builder.currblock + 6;
		int num3 = 5 - Builder.currblock;
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num < 0)
		{
			num = 0;
		}
		if (num2 >= 52)
		{
			num2 = 52;
		}
		Color color2 = (GUI.color = Palette.GetColor());
		int num4 = 0;
		for (int i = num; i < num2; i++)
		{
			Rect position = new Rect((float)Screen.width / 2f - GUIM.YRES(48f) * 5.5f + (float)(num4 + num3) * GUIM.YRES(48f), GUIM.YRES(540f), GUIM.YRES(40f), GUIM.YRES(40f));
			if (Builder.currblock == i)
			{
				GUI.color = Color.white;
				GUI.DrawTexture(new Rect(position.x - GUIM.YRES(4f) - 1f, position.y - GUIM.YRES(4f) - 1f, GUIM.YRES(48f) + 2f, GUIM.YRES(48f) + 2f), tYellow);
				GUI.DrawTexture(new Rect(position.x - GUIM.YRES(4f), position.y - GUIM.YRES(4f), GUIM.YRES(48f), GUIM.YRES(48f)), tBlack);
				GUI.color = color2;
			}
			GUI.DrawTexture(position, tWhite);
			Rect texCoords = Atlas.blocktx[i];
			GUI.DrawTextureWithTexCoords(position, Atlas.tex, texCoords);
			num4++;
		}
	}

	private void DrawIcons()
	{
		GUI.color = Color.white;
		if ((bool)tIconFly && Controll.freefly)
		{
			GUI.DrawTexture(new Rect((float)Screen.width - GUIM.YRES(40f), GUIM.YRES(600f), GUIM.YRES(32f), GUIM.YRES(32f)), tIconFly);
		}
	}

	private void DrawPosition()
	{
		if (!(Controll.trControll == null))
		{
			bool flag = false;
			if (pos[0] != (int)Controll.trControll.position[0])
			{
				flag = true;
			}
			if (pos[1] != (int)Controll.trControll.position[1])
			{
				flag = true;
			}
			if (pos[2] != (int)Controll.trControll.position[2])
			{
				flag = true;
			}
			if (flag)
			{
				pos[0] = (int)Controll.trControll.position[0];
				pos[1] = (int)Controll.trControll.position[1];
				pos[2] = (int)Controll.trControll.position[2];
				sPos = pos[0] + " " + pos[1] + " " + pos[2];
			}
			GUIM.DrawText(new Rect(16f, GUIM.YRES(170f), GUIM.YRES(100f), GUIM.YRES(32f)), sPos, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, true);
		}
	}

	private void DrawToolSelect()
	{
		if (Controll.toolmode == 0)
		{
			GUI.DrawTexture(rt[0], TEX.GetTextureByName("black"));
		}
		if (Controll.toolmode == 1)
		{
			GUI.DrawTexture(rt[1], TEX.GetTextureByName("black"));
		}
		if (Controll.toolmode == 2)
		{
			GUI.DrawTexture(rt[2], TEX.GetTextureByName("black"));
		}
		if (Controll.toolmode == 3)
		{
			GUI.DrawTexture(rt[3], TEX.GetTextureByName("black"));
		}
		if (Controll.toolmode == 4)
		{
			GUI.DrawTexture(rt[4], TEX.GetTextureByName("black"));
		}
		GUIM.DrawText(rt[0], "[1] NORMAL", TextAnchor.MiddleCenter, BaseColor.White, 0, 12, true);
		GUIM.DrawText(rt[1], "[2] A/B+", TextAnchor.MiddleCenter, BaseColor.White, 0, 12, true);
		GUIM.DrawText(rt[2], "[3] A/B-", TextAnchor.MiddleCenter, BaseColor.White, 0, 12, true);
		GUIM.DrawText(rt[3], "[4] COPY/PASTE", TextAnchor.MiddleCenter, BaseColor.White, 0, 12, true);
		GUIM.DrawText(rt[4], "[5] INSERT PREFAB", TextAnchor.MiddleCenter, BaseColor.White, 0, 12, true);
	}

	private void DrawPrefabs()
	{
		if (Controll.toolmode != 4 || GUIGameExit.show)
		{
			return;
		}
		Rect rect = new Rect(GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(200f), GUIM.YRES(400f));
		Rect position = default(Rect);
		Rect r = default(Rect);
		Rect r2 = default(Rect);
		GUI.DrawTexture(rect, TEX.tBlackAlpha);
		sv = GUIM.BeginScrollView(rect, sv, new Rect(0f, 0f, GUIM.YRES(180f), (float)MapPrefab.p.Count * GUIM.YRES(26f)));
		for (int i = 0; i < MapPrefab.p.Count; i++)
		{
			position.Set(0f, 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(100f), GUIM.YRES(20f));
			r.Set(0f, 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(20f), GUIM.YRES(20f));
			r2.Set(GUIM.YRES(22f), 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(80f), GUIM.YRES(20f));
			GUIM.DrawText(r, i.ToString(), TextAnchor.MiddleCenter, BaseColor.White, 0, 20, true);
			if (Builder.prefabpos == i)
			{
				GUI.DrawTexture(position, TEX.tBlue);
			}
			GUIM.DrawText(r2, MapPrefab.p[i].prefabname, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
		}
		GUIM.EndScrollView();
	}
}
