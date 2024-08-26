using Player;
using UnityEngine;

public class GUICase : MonoBehaviour
{
	public static GUICase cs = null;

	private Rect rBack;

	private Rect rKeys;

	private Rect rDesc;

	private Rect rScroll;

	private Texture2D tBlack;

	private Texture2D tWhite;

	private Texture2D tYellow;

	private Texture2D tArrowUp;

	private Texture2D tArrowDown;

	private Vector2 scroll;

	private Color cg = new Color(0.8f, 0.8f, 0.8f, 0.5f);

	private Color cg2 = new Color(1f, 1f, 1f, 0.9f);

	private Color cg3 = new Color(0.25f, 0.25f, 0.25f, 0.95f);

	public static bool needkey = false;

	public static CaseInv itemcase = null;

	public static GameObject opencase = null;

	public static bool emulate = false;

	public static bool caselock = false;

	public static int rnd = -1;

	public static float rw = 0f;

	public static float rw_start = 0f;

	public static bool hideroll = false;

	private string sRecv;

	private string sRequir;

	private string sToStore;

	private string sUsedKey;

	private string sOpenEmul;

	private string sOpen;

	private string sContinue;

	private string sReq;

	private string sReq10;

	private static float arrow_ofs = 0f;

	private static float arrow_forward = 1f;

	private static float offset_x = 0f;

	public static bool show_case = false;

	private static WeaponInfo show_weapon = null;

	private static float show_time = 0f;

	private void Awake()
	{
		cs = this;
	}

	public void LoadLang()
	{
		sRecv = Lang.GetString("_RECEIVED");
		sRequir = Lang.GetString("_REQUIRES_KEY");
		sToStore = Lang.GetString("_GOTO_STORE");
		sUsedKey = Lang.GetString("_USED_KEY");
		sOpenEmul = Lang.GetString("_OPEN_CASE_EMUL");
		sOpen = Lang.GetString("_OPEN_CASE");
		sContinue = Lang.GetString("_CONTINUE");
		sReq = Lang.GetString("_REQUIRED_LEVEL");
		sReq10 = string.Format(sReq, 10);
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		tYellow = TEX.GetTextureByName("yellow");
		tArrowUp = Resources.Load("arrow_u") as Texture2D;
		tArrowDown = Resources.Load("arrow_d") as Texture2D;
	}

	private void OnResize()
	{
		rKeys = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(80f));
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), rKeys.y + GUIM.YRES(96f), GUIM.YRES(920f), GUIM.YRES(264f));
		rDesc = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f) + GUIM.YRES(376f), GUIM.YRES(920f), GUIM.YRES(124f));
		rScroll = new Rect(rBack.x + GUIM.YRES(8f), rBack.y + GUIM.YRES(8f), GUIM.YRES(920f) - GUIM.YRES(16f), rBack.height - GUIM.YRES(16f));
		if (rnd >= 0)
		{
			hideroll = true;
		}
	}

	public void Draw()
	{
		if (opencase != null)
		{
			DrawOpenCase();
			DrawOpenCaseEnd();
		}
		else
		{
			DrawKeys();
			DrawCases();
			GUIShop.DrawDescription(rDesc, GUIShop.caseselect, null, null);
		}
	}

	private void DrawKeys()
	{
		float num = GUIM.YRES(52f);
		float num2 = num + GUIM.YRES(8f);
		GUIM.DrawBox(rKeys, tBlack, 0.05f);
		int num3 = GUIInv.klist.Count;
		if (num3 > 15)
		{
			num3 = 15;
		}
		for (int i = 0; i < num3; i++)
		{
			GUI.DrawTexture(new Rect(rKeys.x + GUIM.YRES(8f) + num2 * (float)i, rKeys.y + GUIM.YRES(8f), num, num), GUIInv.klist[i].ki.img);
		}
	}

	private void DrawCases()
	{
		float num = GUIM.YRES(170f);
		float num2 = num + GUIM.YRES(8f);
		GUIM.DrawBox(rBack, tBlack, 0.05f);
		scroll = GUIM.BeginScrollView(rScroll, scroll, new Rect(0f, 0f, 0f, (float)Mathf.CeilToInt((float)GUIInv.clist.Count / 5f) * num2));
		for (int i = 0; i < GUIInv.clist.Count; i++)
		{
			int num3 = i / 5;
			int num4 = i - num3 * 5;
			if (!GUIShop.DrawItemCase(new Rect(num2 * (float)num4, num2 * (float)num3, num, num), GUIInv.clist[i], rScroll, scroll))
			{
				continue;
			}
			itemcase = GUIInv.clist[i];
			opencase = CaseGen.Build("case_" + itemcase.ci.name);
			InitOpenCase();
			needkey = true;
			for (int j = 0; j < GUIInv.klist.Count; j++)
			{
				if (GUIInv.klist[j].ki.idx == itemcase.ci.keyid)
				{
					needkey = false;
					break;
				}
			}
			GUIM.EndScrollView();
			return;
		}
		GUIM.EndScrollView();
	}

	private void InitOpenCase()
	{
		opencase.transform.position = Vector3.zero;
		opencase.SetActive(true);
		opencase.transform.localEulerAngles = new Vector3(350f, 45f, 350f);
		opencase.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
		FXScaleCase fXScaleCase = opencase.AddComponent<FXScaleCase>();
		fXScaleCase.minscale = 0.0295f;
		fXScaleCase.maxscale = 0.03f;
		fXScaleCase.speed = 0.004f;
		offset_x = GUIM.YRES(4f);
		hideroll = false;
	}

	private void DrawOpenCase()
	{
		Rect position = new Rect((float)Screen.width / 2f - GUIM.YRES(150f), GUIM.YRES(460f), GUIM.YRES(300f), GUIM.YRES(32f));
		GUI.color = cg;
		GUI.DrawTexture(position, tBlack);
		GUI.color = Color.white;
		GUIM.DrawText(new Rect(position.x, position.y + GUIM.YRES(4f), position.width, position.height), itemcase.ci.fullname, TextAnchor.MiddleCenter, BaseColor.White, 1, 20, true);
		GUI.DrawTexture(new Rect(position.x, position.y + position.height, position.width, GUIM.YRES(2f)), tYellow);
		Rect position2 = new Rect((float)Screen.width / 2f - GUIM.YRES(200f), GUIM.YRES(516f), GUIM.YRES(400f), GUIM.YRES(84f));
		GUI.color = cg;
		GUI.DrawTexture(position2, tBlack);
		GUI.color = Color.white;
		float num = GUIM.YRES(132f) * (float)itemcase.ci.weaponcount;
		if (rnd >= 0)
		{
			float num2 = GUIM.YRES(1f);
			if (num2 < 1f)
			{
				num2 = 1f;
			}
			float num3 = 1f - rw_start / rw;
			float num4 = GUIM.YRES(500f) * num3 * Time.deltaTime + num2 / 2f;
			if (num4 < num2)
			{
				num4 = num2;
			}
			rw_start += num4;
			if (rw_start > rw)
			{
				num4 = rw_start - rw;
				OpenCaseEnd(rnd);
				rnd = -1;
			}
			offset_x += num4;
		}
		while (offset_x > num)
		{
			offset_x -= num;
		}
		GUI.BeginGroup(position2);
		float num5 = position2.width / 2f;
		float num6 = position2.height / 2f;
		for (int i = 0; i < itemcase.ci.weaponcount; i++)
		{
			Rect r = new Rect((float)i * GUIM.YRES(132f) + offset_x - num, GUIM.YRES(4f), GUIM.YRES(128f), GUIM.YRES(76f));
			bool hover = r.Contains(new Vector3(position2.width / 2f, position2.height / 2f));
			if (hideroll)
			{
				hover = false;
			}
			DrawItem(r, itemcase.ci.weapon[i], hover);
			r.x += num;
			hover = r.Contains(new Vector3(position2.width / 2f, position2.height / 2f));
			DrawItem(r, itemcase.ci.weapon[i], hover);
		}
		GUI.EndGroup();
		GUI.DrawTexture(new Rect(position2.x - GUIM.YRES(2f), position2.y, GUIM.YRES(2f), position2.height), tWhite);
		GUI.DrawTexture(new Rect(position2.x + position2.width, position2.y, GUIM.YRES(2f), position2.height), tWhite);
		if (!hideroll)
		{
			arrow_ofs += GUIM.YRES(6f) * Time.deltaTime * arrow_forward;
			if (arrow_ofs > GUIM.YRES(4f))
			{
				arrow_forward = -1f;
			}
			if (arrow_ofs < 0f)
			{
				arrow_forward = 1f;
			}
			GUI.color = GUIM.colorlist[5];
			GUI.DrawTexture(new Rect(position2.x + position2.width / 2f - GUIM.YRES(8f), position2.y - GUIM.YRES(20f) + arrow_ofs, GUIM.YRES(16f), GUIM.YRES(16f)), tArrowDown);
			GUI.DrawTexture(new Rect(position2.x + position2.width / 2f - GUIM.YRES(8f), position2.y + position2.height + GUIM.YRES(4f) - arrow_ofs, GUIM.YRES(16f), GUIM.YRES(16f)), tArrowUp);
			GUI.color = Color.white;
		}
		GUIShop.DrawCamHack();
		GUIM.DrawBox(new Rect((float)Screen.width / 2f + GUIM.YRES(276f), GUIM.YRES(492f), GUIM.YRES(192f), GUIM.YRES(84f)), tBlack, 0.1f);
		if (itemcase.ci.idx == 20)
		{
			Rect r2 = new Rect((float)Screen.width / 2f + GUIM.YRES(276f), GUIM.YRES(443f), GUIM.YRES(192f), GUIM.YRES(40f));
			GUIM.DrawBox(r2, tBlack, 0.1f);
			GUIM.DrawText(r2, sReq10, TextAnchor.MiddleCenter, BaseColor.Block, 0, 20, true);
		}
		if (needkey)
		{
			GUIM.DrawText(new Rect((float)Screen.width / 2f + GUIM.YRES(280f), GUIM.YRES(503f), GUIM.YRES(152f), GUIM.YRES(32f)), sRequir, TextAnchor.MiddleCenter, BaseColor.Block, 1, 12, true);
			if (GUIInv.kinfo[itemcase.ci.keyid] != null)
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f + GUIM.YRES(428f), GUIM.YRES(500f), GUIM.YRES(32f), GUIM.YRES(32f)), GUIInv.kinfo[itemcase.ci.keyid].img);
				if (GUIM.Button(new Rect((float)Screen.width / 2f + GUIM.YRES(280f), GUIM.YRES(544f), GUIM.YRES(184f), GUIM.YRES(28f)), BaseColor.Yellow, sToStore, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
				{
					Main.EmulateClick(3);
				}
			}
			return;
		}
		GUIM.DrawText(new Rect((float)Screen.width / 2f + GUIM.YRES(280f), GUIM.YRES(503f), GUIM.YRES(152f), GUIM.YRES(32f)), sUsedKey, TextAnchor.MiddleCenter, BaseColor.White, 1, 12, true);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f + GUIM.YRES(428f), GUIM.YRES(500f), GUIM.YRES(32f), GUIM.YRES(32f)), GUIInv.kinfo[itemcase.ci.keyid].img);
		if (emulate)
		{
			if (GUIM.Button(new Rect((float)Screen.width / 2f + GUIM.YRES(280f), GUIM.YRES(544f), GUIM.YRES(184f), GUIM.YRES(28f)), BaseColor.Orange, sOpenEmul, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				if (MainManager.steam)
				{
					MasterClient.cs.send_caseopen_steam(1, itemcase.uid);
				}
				else
				{
					MasterClient.cs.send_caseopen(1, (int)itemcase.uid);
				}
			}
		}
		else if (GUIM.Button(new Rect((float)Screen.width / 2f + GUIM.YRES(280f), GUIM.YRES(544f), GUIM.YRES(184f), GUIM.YRES(28f)), BaseColor.White, sOpen, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && (itemcase.ci.idx != 20 || GUIOptions.level >= 10) && !caselock)
		{
			if (MainManager.steam)
			{
				MasterClient.cs.send_caseopen_steam(0, itemcase.uid);
			}
			else
			{
				MasterClient.cs.send_caseopen(0, (int)itemcase.uid);
			}
			caselock = true;
			Main.lockmenu = true;
		}
	}

	private void DrawItem(Rect r, WeaponInfo wi, bool hover = false)
	{
		if (hover)
		{
			GUI.DrawTexture(r, tWhite);
		}
		else
		{
			GUI.color = cg;
			GUI.DrawTexture(r, tWhite);
			GUI.color = Color.white;
		}
		GUIInv.DrawWeaponIcon(r, wi.tIcon, wi.vIcon);
	}

	public static void StartRoll(int val)
	{
		rnd = val;
		int num = Random.Range(3, 10);
		if (itemcase.ci.weaponcount > 8)
		{
			num = Random.Range(2, 4);
		}
		float num2 = Random.Range(-0.51f, 0.49f);
		float num3 = GUIM.YRES(128f) * num2;
		rw = GUIM.YRES(132f) * (float)(itemcase.ci.weaponcount * num - rnd + 1) + GUIM.YRES(4f) + num3;
		rw_start = 0f;
		offset_x = 0f;
		show_case = false;
		show_weapon = null;
	}

	private void OpenCaseEnd(int i)
	{
		show_case = true;
		show_weapon = itemcase.ci.weapon[i];
		show_time = Time.time;
		GameObject original = ContentLoader_.LoadGameObject("debris");
		for (int j = 0; j < 10; j++)
		{
			GameObject obj = Object.Instantiate(original);
			obj.transform.position = opencase.transform.position;
			ParticleSystem component = obj.GetComponent<ParticleSystem>();
			component.startColor = Color.yellow;
			obj.AddComponent<DelayDestroy>();
			obj.layer = 9;
			component.gravityModifier = 0.01f;
			component.maxParticles = 100;
			component.startSpeed = j + 3;
		}
		Main.lockmenu = false;
	}

	private void DrawOpenCaseEnd()
	{
		if (!show_case)
		{
			return;
		}
		if ((double)Time.time < (double)show_time + 0.5)
		{
			float num = 1f - (show_time + 0.5f - Time.time) * 2f;
			float num2 = num * 20f;
			float num3 = num * 180f;
			float num4 = num * 20f;
			opencase.transform.localEulerAngles = new Vector3(350f + num2, 45f + num3, 350f + num4);
			return;
		}
		opencase.transform.localEulerAngles = new Vector3(10f, 225f, 10f);
		if (Time.time < show_time + 0.75f)
		{
			float num5 = 1f - (show_time + 0.75f - Time.time) * 4f;
			GameObject obj = GameObject.Find(opencase.name + "/cover");
			float num6 = num5 * 45f;
			obj.transform.localEulerAngles = new Vector3(360f - num6, 0f, 0f);
			return;
		}
		if ((double)Time.time < (double)show_time + 0.85)
		{
			float num7 = 1f - (show_time + 0.85f - Time.time) * 10f;
			GUI.color = cg2;
			GUI.DrawTexture(new Rect(0f, GUIM.YRES(300f) - GUIM.YRES(200f) * num7 / 2f, Screen.width, GUIM.YRES(200f) * num7), tWhite);
			GUI.color = Color.white;
			return;
		}
		GUI.color = cg3;
		GUI.DrawTexture(new Rect(0f, GUIM.YRES(200f), Screen.width, GUIM.YRES(200f)), tWhite);
		GUI.color = Color.white;
		GUI.color = Color.black;
		GUI.DrawTexture(new Rect(0f, GUIM.YRES(200f), Screen.width, GUIM.YRES(200f)), Main.tVig);
		GUI.color = Color.white;
		float num8 = GUIM.YRES(200f);
		Rect r = new Rect((float)Screen.width / 2f - num8 / 2f, GUIM.YRES(300f) - num8 / 2f, num8, num8);
		if ((double)Time.time < (double)show_time + 0.95)
		{
			float num9 = 1f - (show_time + 0.95f - Time.time) * 10f;
			float num10 = num8 / 2f * num9;
			r = new Rect((float)Screen.width / 2f - num10, GUIM.YRES(300f) - num10, num10 * 2f, num10 * 2f);
		}
		GUIInv.DrawWeaponIcon(r, show_weapon.tIcon, show_weapon.vIcon);
		if (Time.time < show_time + 1f)
		{
			GUI.color = Color.black;
			GUIInv.DrawWeaponIcon(r, show_weapon.tIcon, show_weapon.vIcon);
			GUI.color = Color.white;
		}
		GUI.DrawTexture(new Rect(0f, r.y - GUIM.YRES(2f), Screen.width, GUIM.YRES(2f)), tYellow);
		GUI.DrawTexture(new Rect(0f, r.y + r.height, Screen.width, GUIM.YRES(2f)), tYellow);
		GUIM.DrawText(new Rect(r.x, r.y + GUIM.YRES(8f), r.width, GUIM.YRES(32f)), sRecv + " " + show_weapon.fullname, TextAnchor.MiddleCenter, BaseColor.Yellow, 1, 20, true);
		if (GUIM.Button(new Rect((float)Screen.width / 2f - GUIM.YRES(100f), r.y + r.height - GUIM.YRES(40f), GUIM.YRES(200f), GUIM.YRES(26f)), BaseColor.White, sContinue, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Main.EmulateClick(1);
		}
	}
}
