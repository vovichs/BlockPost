using Player;
using UnityEngine;

public class GUIGameSet : MonoBehaviour
{
	public static bool show;

	private Texture2D tBlack;

	private static Texture2D tWhite;

	private Rect[] rSet = new Rect[3];

	private static Texture2D tTex;

	private Texture2D[] tPrimary = new Texture2D[3];

	private Vector2[] vPrimary = new Vector2[3];

	private string[] sPrimary = new string[3] { "AK47", "M4A1", "SIG552" };

	private string[] sSet = new string[3];

	private string sPrimWeap;

	private string sSecWeap;

	private string sMeleeWeap;

	private string sBuildBlock;

	private string sAddit;

	public void LoadLang()
	{
		sSet[0] = Lang.GetString("_SET1");
		sSet[1] = Lang.GetString("_SET2");
		sSet[2] = Lang.GetString("_SET3");
		sPrimWeap = Lang.GetString("_PRIMARY");
		sSecWeap = Lang.GetString("_SECONDARY");
		sMeleeWeap = Lang.GetString("_MELEE");
		sBuildBlock = Lang.GetString("_BUILDING_BLOCK");
		sAddit = Lang.GetString("_ADDITIONAL");
	}

	public static void SetActive(bool val)
	{
		show = val;
		Crosshair.SetCursor(show);
		Controll.SetLockLook(show);
		Controll.SetLockAttack(show);
		if (val)
		{
			GUIFX.Set();
		}
		else
		{
			GUIFX.End();
		}
	}

	public static void Toggle()
	{
		SetActive(!show);
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
	}

	private void OnResize()
	{
		for (int i = 0; i < 3; i++)
		{
			rSet[i] = new Rect((float)Screen.width / 2f - GUIM.YRES(332f) + GUIM.YRES(232f) * (float)i, GUIM.YRES(100f), GUIM.YRES(200f), GUIM.YRES(500f));
		}
	}

	private void OnGUI()
	{
		if (!show)
		{
			return;
		}
		GUIFX.Begin();
		for (int i = 0; i < 3; i++)
		{
			DrawSet(i, rSet[i]);
			if (GUIM.HideButton(rSet[i]))
			{
				bool flag = false;
				if (GUIInv.wset[i].w[0] == null && GUIInv.wset[i].w[1] == null && GUIInv.wset[i].w[2] == null)
				{
					flag = true;
				}
				if (!flag)
				{
					SetActive(false);
					Client.cs.send_playerset(i);
					HUD.SetHint(1, 5f);
				}
			}
		}
		GUIFX.End();
	}

	private void DrawSet(int i, Rect r)
	{
		tTex = tBlack;
		if (Controll.pl.currset == i)
		{
			tTex = TEX.GetTextureByName("orange");
		}
		if (GUIM.Contains(r))
		{
			r = new Rect(r.x, r.y - GUIM.YRES(8f), r.width, r.height);
		}
		GUIM.DrawBox(new Rect(r.x, r.y - GUIM.YRES(36f), GUIM.YRES(40f), GUIM.YRES(32f)), tTex);
		GUIM.DrawText(new Rect(r.x, r.y - GUIM.YRES(36f), GUIM.YRES(40f), GUIM.YRES(32f)), sSet[i], TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, true);
		DrawWeapon(r, 0, sPrimWeap, GUIInv.wset[i].w[0]);
		DrawWeapon(r, 1, sSecWeap, GUIInv.wset[i].w[1]);
		DrawWeapon(r, 2, sMeleeWeap, GUIInv.wset[i].w[2]);
		DrawWeapon(r, 3, sBuildBlock, GUIInv.wset[i].w[3]);
		DrawWeapon(r, 4, sAddit, GUIInv.wset[i].w[4]);
	}

	public static void DrawWeapon(Rect r, int pos, string desc, WeaponInv w)
	{
		int num = (int)GUIM.YRES(84f);
		int num2 = (int)GUIM.YRES(88f);
		GUIM.DrawBox(new Rect(r.x, r.y + (float)(num2 * pos), r.width, num), tTex);
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(4f), r.y + (float)(num2 * pos), r.width, GUIM.YRES(20f)), (w == null) ? desc : w.wi.fullname, TextAnchor.MiddleLeft, BaseColor.LightGray, 0, 16, true);
		Rect rect = new Rect(r.x + GUIM.YRES(2f), r.y + (float)(num2 * pos) + GUIM.YRES(20f), r.width - GUIM.YRES(4f), GUIM.YRES(62f));
		if (w == null)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUI.DrawTexture(rect, tWhite);
			GUI.color = Color.white;
		}
		else
		{
			GUIInv.DrawWeapon(rect, w, false, true);
		}
	}

	public static Vector2 CalcSize(Texture2D tex)
	{
		if (tex == null)
		{
			return Vector2.one;
		}
		int num = 0;
		int num2 = 0;
		for (int num3 = tex.height - 1; num3 >= 0; num3--)
		{
			for (int num4 = tex.width - 1; num4 > num; num4--)
			{
				if (tex.GetPixel(num4, num3).a != 0f)
				{
					num = num4;
				}
			}
		}
		num++;
		num2 = tex.height - 1;
		for (int i = 0; i < tex.width; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				if (tex.GetPixel(i, j).a != 0f)
				{
					num2 = j;
				}
			}
		}
		num2 = tex.height - num2;
		return new Vector2(num, num2);
	}
}
