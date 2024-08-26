using Player;
using UnityEngine;

public class HUDNames : MonoBehaviour
{
	private static bool show = true;

	private Texture2D tRed;

	private Texture2D tGreen;

	private Texture2D tOrange;

	private Texture2D tYellow;

	private Texture2D tEcho;

	private Vector3 _pos;

	private Vector3 _dir;

	private float CacheTime;

	private static Vector3 p2d;

	private static Vector3 pos;

	private static Rect[] r = new Rect[40];

	private static Rect[] rBar = new Rect[40];

	private Color ca = new Color(0.94f, 0.25f, 0f, 1f);

	public static void SetActive(bool val)
	{
		show = val;
	}

	private void LoadEnd()
	{
		tRed = TEX.GetTextureByName("red");
		tGreen = TEX.GetTextureByName("green");
		tOrange = TEX.GetTextureByName("orange");
		tYellow = TEX.GetTextureByName("yellow");
		tEcho = Resources.Load("heart_echo") as Texture2D;
	}

	private void OnGUI()
	{
		if (!show || Controll.csCam == null)
		{
			return;
		}
		if (DemoRec.isDemo())
		{
			CacheTime = Time.time;
			GUI.depth = -1;
			for (int i = 0; i < 40; i++)
			{
				if (PLH.player[i] != null)
				{
					DrawPlayerName(PLH.player[i]);
				}
			}
		}
		else
		{
			if (Controll.trControll == null || Controll.gamemode == 2 || Controll.gamemode == 8)
			{
				return;
			}
			CacheTime = Time.time;
			GUI.depth = -1;
			_pos = Controll.trControll.position;
			_dir = Controll.trControll.forward;
			for (int j = 0; j < 40; j++)
			{
				if (PLH.player[j] == null || Controll.pl == PLH.player[j] || Controll.pl.team != PLH.player[j].team)
				{
					continue;
				}
				if (Controll.gamemode == 5)
				{
					if (Controll.pl.skinstate != PLH.player[j].skinstate)
					{
						if (Controll.pl.skinstate == 1)
						{
							DrawPlayerHeart(PLH.player[j]);
						}
					}
					else
					{
						DrawPlayerName(PLH.player[j]);
					}
				}
				else
				{
					DrawPlayerName(PLH.player[j]);
				}
			}
		}
	}

	private void DrawPlayerName(PlayerData p)
	{
		pos.Set(p.currPos.x, p.currPos.y + 0.75f, p.currPos.z);
		p2d = Controll.csCam.WorldToScreenPoint(pos);
		p2d.y = GUIM.currheight - p2d.y;
		if (!(Vector3.Angle(p.currPos - _pos, _dir) < 90f) || !(p2d.z > 0f))
		{
			return;
		}
		int idx = p.idx;
		ref Rect reference = ref r[idx];
		r[idx].Set(p2d.x - GUIM.YRES(100f), p2d.y - GUIM.YRES(20f), GUIM.YRES(200f), GUIM.YRES(20f));
		ref Rect reference2 = ref rBar[idx];
		rBar[idx].Set(p2d.x - GUIM.YRES(16f), r[idx].y + r[idx].height, GUIM.YRES(32f) * (float)p.health / 100f, GUIM.YRES(2f));
		GUIM.DrawTextColor(r[idx], p.formatname, TextAnchor.LowerCenter, BaseColor.White, 1, 10, false);
		if (p.health != 0)
		{
			if (p.health <= 10)
			{
				GUI.DrawTexture(rBar[idx], tRed);
			}
			else if (p.health <= 40)
			{
				GUI.DrawTexture(rBar[idx], tOrange);
			}
			else if (p.health <= 70)
			{
				GUI.DrawTexture(rBar[idx], tYellow);
			}
			else
			{
				GUI.DrawTexture(rBar[idx], tGreen);
			}
		}
	}

	private void DrawPlayerHeart(PlayerData p)
	{
		if (CacheTime > p.tHeartBeat)
		{
			p.tHeartBeat = CacheTime + 2f;
		}
		float num = p.tHeartBeat - CacheTime - 1.5f;
		if (!(num < 0f))
		{
			pos.Set(p.currPos.x, p.currPos.y, p.currPos.z);
			p2d = Controll.csCam.WorldToScreenPoint(pos);
			p2d.y = GUIM.currheight - p2d.y;
			if (Vector3.Angle(p.currPos - _pos, _dir) < 90f && p2d.z > 0f)
			{
				float num2 = 1f;
				float num3 = GUIM.YRES(64f);
				num2 = 1f - num * 2f;
				num3 *= num2;
				int idx = p.idx;
				ca.a = 1f - num2;
				GUI.color = ca;
				r[idx].Set(p2d.x - num3 / 2f, p2d.y - num3 / 2f, num3, num3);
				GUI.DrawTexture(r[idx], tEcho);
				GUI.color = Color.white;
			}
		}
	}
}
