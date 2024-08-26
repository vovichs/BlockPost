using Player;
using UnityEngine;

public class HUDKiller : MonoBehaviour
{
	public static HUDKiller cs;

	private static bool show;

	private Texture2D tBlack;

	private Rect[] rKill;

	private Rect[] rBlock;

	private Rect[] rText;

	private Rect[] rIcon;

	private Rect rHint;

	private string sKillText = "ВАС УБИЛ";

	private string sKillName = "PLAYERNAME";

	private Color b50 = new Color(0f, 0f, 0f, 0.7f);

	private string sFormatName;

	private string sWeaponName;

	private string sYourFrags = "0";

	private string sEnemyFrags = "0";

	private Vector2 v;

	private Texture2D tex;

	private Texture2D spec_tex;

	private Texture2D[] tSpec;

	private static bool data;

	private string sWeapon;

	private string sSpecial;

	private string sYou;

	private string sEnemy;

	private string sHoldView;

	public static float tautoclose;

	public void LoadLang()
	{
		sKillText = Lang.GetString("_YOU_WERE_KILLED");
		sWeapon = Lang.GetString("_WEAPON");
		sSpecial = Lang.GetString("_SPECIALIZATION");
		sYou = Lang.GetString("_YOU");
		sEnemy = Lang.GetString("_ENEMY");
		sHoldView = Lang.GetString("_HOLD_REDIAL_VIEWING");
	}

	public static void SetActive(bool val)
	{
		if (data)
		{
			show = val;
		}
	}

	public void Set(int aid, int vid, int slot)
	{
		if (PLH.player[aid] == null || PLH.player[vid] == null)
		{
			return;
		}
		PlayerData playerData = PLH.player[aid];
		playerData.fragcount[vid]++;
		if (playerData.wset[playerData.currset].w[slot] == null)
		{
			return;
		}
		WeaponInfo wi = playerData.wset[playerData.currset].w[slot].wi;
		if (wi == null)
		{
			return;
		}
		HUDMessage.LoadWeaponIcon(wi);
		sKillName = playerData.name;
		sFormatName = playerData.formatname;
		sYourFrags = Controll.pl.fragcount[aid].ToString();
		sEnemyFrags = playerData.fragcount[vid].ToString();
		sWeaponName = sWeapon + " " + wi.fullname;
		tex = wi.tIconDMSG;
		v = wi.vIconDMSG;
		if (playerData.wset[playerData.currset].w[4] == null)
		{
			spec_tex = null;
		}
		else
		{
			switch (playerData.wset[playerData.currset].w[4].wi.id)
			{
			case 107:
				spec_tex = tSpec[0];
				break;
			case 108:
				spec_tex = tSpec[1];
				break;
			case 109:
				spec_tex = tSpec[2];
				break;
			case 110:
				spec_tex = tSpec[3];
				break;
			}
		}
		data = true;
		SetActive(true);
		tautoclose = Time.time + 5f;
	}

	private void Awake()
	{
		cs = this;
		rKill = new Rect[3];
		rBlock = new Rect[3];
		rText = new Rect[7];
		rIcon = new Rect[2];
	}

	private void LoadEnd()
	{
		tSpec = new Texture2D[4];
		tBlack = TEX.GetTextureByName("black");
		tSpec[0] = Resources.Load("weapons/ammo") as Texture2D;
		tSpec[1] = Resources.Load("weapons/grenade") as Texture2D;
		tSpec[2] = Resources.Load("weapons/medkit") as Texture2D;
		tSpec[3] = Resources.Load("weapons/shield") as Texture2D;
	}

	private void OnResize()
	{
		rKill[0] = new Rect((float)Screen.width / 2f - GUIM.YRES(100f), GUIM.YRES(80f), GUIM.YRES(200f), GUIM.YRES(26f));
		rKill[1] = new Rect((float)Screen.width / 2f - GUIM.YRES(100f), GUIM.YRES(120f), GUIM.YRES(200f), GUIM.YRES(76f));
		rKill[2] = new Rect((float)Screen.width / 2f - GUIM.YRES(100f), GUIM.YRES(192f), GUIM.YRES(200f), GUIM.YRES(26f));
		int num = (int)GUIM.YRES(6f);
		rBlock[0] = new Rect((float)Screen.width / 2f - GUIM.YRES(406f), (float)Screen.height - GUIM.YRES(267f), GUIM.YRES(320f), GUIM.YRES(160f));
		rBlock[1] = new Rect(rBlock[0].x + rBlock[0].width + (float)num, rBlock[0].y, GUIM.YRES(160f), rBlock[0].height);
		rBlock[2] = new Rect(rBlock[1].x + rBlock[1].width + (float)num, rBlock[1].y, rBlock[0].width, rBlock[1].height);
		rText[0] = new Rect(rBlock[0].x + GUIM.YRES(8f), rBlock[0].y, GUIM.YRES(160f), GUIM.YRES(32f));
		rText[1] = new Rect(rBlock[1].x, rBlock[1].y, GUIM.YRES(160f), GUIM.YRES(32f));
		rText[2] = new Rect(rBlock[2].x + GUIM.YRES(8f), rBlock[2].y, GUIM.YRES(160f), GUIM.YRES(32f));
		rText[3] = new Rect(rBlock[0].x, rBlock[0].y + GUIM.YRES(50f), GUIM.YRES(160f), GUIM.YRES(32f));
		rText[4] = new Rect(rBlock[0].x + GUIM.YRES(160f), rBlock[0].y + GUIM.YRES(50f), GUIM.YRES(160f), GUIM.YRES(32f));
		rText[5] = new Rect(rBlock[0].x, rBlock[0].y + GUIM.YRES(106f), GUIM.YRES(160f), GUIM.YRES(32f));
		rText[6] = new Rect(rBlock[0].x + GUIM.YRES(160f), rBlock[0].y + GUIM.YRES(106f), GUIM.YRES(160f), GUIM.YRES(32f));
		rIcon[0] = new Rect(rBlock[1].x + GUIM.YRES(54f), rBlock[1].y + GUIM.YRES(44f), GUIM.YRES(80f), GUIM.YRES(80f));
		rIcon[1] = new Rect(rBlock[2].x + GUIM.YRES(32f), rBlock[2].y + GUIM.YRES(32f), rBlock[2].width - GUIM.YRES(64f), rBlock[2].height - GUIM.YRES(64f));
		rHint = new Rect(rBlock[2].x, rBlock[2].y + rBlock[2].height + GUIM.YRES(4f), rBlock[2].width, GUIM.YRES(32f));
	}

	public void _OnGUI()
	{
		if (!show)
		{
			return;
		}
		if (Time.time > tautoclose)
		{
			show = false;
			return;
		}
		GUIM.DrawText(rKill[0], sKillText, TextAnchor.MiddleCenter, BaseColor.White, 1, 20, true);
		GUIM.DrawText(rKill[1], sKillName, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 40, true);
		GUI.color = b50;
		GUI.DrawTexture(rBlock[0], tBlack);
		GUI.DrawTexture(rBlock[1], tBlack);
		GUI.DrawTexture(rBlock[2], tBlack);
		if (HUD.showhint < 0)
		{
			GUI.DrawTexture(rHint, tBlack);
		}
		GUI.color = Color.white;
		GUIM.DrawText(rText[0], sFormatName, TextAnchor.MiddleLeft, BaseColor.White, 1, 20, false);
		GUIM.DrawText(rText[1], sSpecial, TextAnchor.MiddleCenter, BaseColor.White, 1, 20, false);
		GUIM.DrawText(rText[2], sWeaponName, TextAnchor.MiddleLeft, BaseColor.White, 1, 20, false);
		GUIM.DrawText(rText[3], sYou, TextAnchor.MiddleCenter, BaseColor.Blue, 1, 36, true);
		GUIM.DrawText(rText[4], sEnemy, TextAnchor.MiddleCenter, BaseColor.Orange, 1, 36, true);
		GUIM.DrawText(rText[5], sYourFrags, TextAnchor.MiddleCenter, BaseColor.White, 1, 32, true);
		GUIM.DrawText(rText[6], sEnemyFrags, TextAnchor.MiddleCenter, BaseColor.White, 1, 32, true);
		if (spec_tex != null)
		{
			GUI.DrawTexture(rIcon[0], spec_tex);
		}
		HUDMessage.DrawWeaponIcon(rIcon[1], tex, v);
		if (HUD.showhint < 0)
		{
			GUIM.DrawText(rHint, sHoldView, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
	}
}
