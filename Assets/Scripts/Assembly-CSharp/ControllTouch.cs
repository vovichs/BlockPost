using UnityEngine;

public class ControllTouch : MonoBehaviour
{
	public static ControllTouch cs = null;

	private Texture2D tJoy;

	private Color ca = new Color(1f, 1f, 1f, 0.25f);

	private Color cb = new Color(0f, 0f, 0f, 0.25f);

	private Rect rJoy;

	private Rect rJoyImg;

	private Rect rJoyMove;

	private Rect rJoyMoveImg;

	private Texture2D tCircle;

	public static Rect[] rButton = new Rect[15];

	private Texture2D[] tButton = new Texture2D[15];

	public static int[] fingerKeys = new int[17]
	{
		-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
		-1, -1, -1, -1, -1, -1, -1
	};

	private string[] sButtonName = new string[15]
	{
		"ATTACK", "JUMP", "DUCK", "CHANGE", "RELOAD", "ZOOM", "MENU", "SPECIAL", "SET", "CHAT",
		"SCORE", "ATTACK2", "MOVE", "???", "???"
	};

	public static int btncount = 13;

	public static int currbutton = -1;

	public static int currbuttonuid = -1;

	public static int lastbutton = -1;

	public static float movex = 0f;

	public static float movez = 0f;

	private Touch[] t;

	private int fingerMove = -1;

	private int fingerLook = -1;

	public static float tx = 0f;

	public static float ty = 0f;

	public Vector2 startpos = Vector2.zero;

	private Vector2 dstart = Vector2.zero;

	private Vector2 dend = Vector2.zero;

	private Vector2 dn = Vector2.zero;

	private int[] skillidx = new int[btncount];

	private Vector2 v2con = Vector2.zero;

	private float screen_height;

	public static bool[] buttoncanscale = new bool[15]
	{
		true, true, true, true, false, true, true, true, true, true,
		true, true, false, true, true
	};

	public static Vector2[] buttonsize = new Vector2[15]
	{
		new Vector2(100f, 100f),
		new Vector2(80f, 80f),
		new Vector2(80f, 80f),
		new Vector2(300f, 60f),
		new Vector2(80f, 80f),
		new Vector2(80f, 80f),
		new Vector2(60f, 60f),
		new Vector2(80f, 80f),
		new Vector2(60f, 60f),
		new Vector2(60f, 60f),
		new Vector2(200f, 60f),
		new Vector2(164f, 164f),
		new Vector2(128f, 128f),
		new Vector2(100f, 100f),
		new Vector2(100f, 100f)
	};

	public static Vector2[] buttonpos_default = new Vector2[15]
	{
		new Vector2(420f, 340f),
		new Vector2(90f, 250f),
		new Vector2(90f, 160f),
		new Vector2(500f, 66f),
		new Vector2(180f, 90f),
		new Vector2(240f, 380f),
		new Vector2(226f, 16f),
		new Vector2(90f, 440f),
		new Vector2(70f, 10f),
		new Vector2(226f, 116f),
		new Vector2(100f, 16f),
		new Vector2(32f, 32f),
		new Vector2(128f, 228f),
		Vector2.zero,
		Vector2.zero
	};

	public static Vector2[] buttonpos = new Vector2[15]
	{
		new Vector2(420f, 340f),
		new Vector2(90f, 250f),
		new Vector2(90f, 160f),
		new Vector2(500f, 66f),
		new Vector2(180f, 90f),
		new Vector2(240f, 380f),
		new Vector2(226f, 16f),
		new Vector2(90f, 440f),
		new Vector2(70f, 10f),
		new Vector2(226f, 116f),
		new Vector2(100f, 16f),
		new Vector2(32f, 32f),
		new Vector2(128f, 228f),
		Vector2.zero,
		Vector2.zero
	};

	public static int[] buttonalign = new int[15]
	{
		0, 0, 0, 0, 0, 0, 1, 0, 2, 1,
		3, 1, 4, 0, 0
	};

	public static int[] buttonscale = new int[15]
	{
		100, 100, 100, 100, 100, 100, 100, 100, 100, 100,
		100, 100, 100, 100, 100
	};

	private Texture2D tPlus;

	private Texture2D tMinus;

	private Texture2D tCell;

	private void Awake()
	{
		cs = this;
		LoadEnd();
		OnResize();
	}

	private void LoadEnd()
	{
		tJoy = Resources.Load("arrow_radar2") as Texture2D;
		tCircle = Resources.Load("button_circle") as Texture2D;
		tButton[0] = ContentLoader_.LoadTexture("button_shoot2") as Texture2D;
		tButton[1] = ContentLoader_.LoadTexture("button_jump") as Texture2D;
		tButton[2] = ContentLoader_.LoadTexture("button_crouch") as Texture2D;
		tButton[4] = Resources.Load("gui/button_reload") as Texture2D;
		tButton[5] = ContentLoader_.LoadTexture("button_zoom") as Texture2D;
		tButton[6] = ContentLoader_.LoadTexture("button_menu") as Texture2D;
		tButton[7] = ContentLoader_.LoadTexture("button_grenade") as Texture2D;
		tButton[8] = ContentLoader_.LoadTexture("button_sets") as Texture2D;
		tButton[9] = ContentLoader_.LoadTexture("button_chat") as Texture2D;
	}

	public void UpdateTouch()
	{
		if (!GUIGameExit.show && !GUIOptions.show)
		{
			t = Input.touches;
			UpdateMoveKey();
			UpdateKey();
			UpdateLook();
		}
	}

	public void UpdateMoveKey()
	{
		movex = 0f;
		movez = 0f;
		float num = GUIM.currwidth / 2f - GUIM.YRES(80f);
		float num2 = GUIM.currheight - GUIM.YRES(200f);
		int num3 = -1;
		if (fingerMove >= 0)
		{
			int i = 0;
			for (int num4 = t.Length; i < num4; i++)
			{
				for (int j = 0; j < btncount; j++)
				{
					int fingerId = t[i].fingerId;
					int num9 = fingerKeys[j];
				}
				if (t[i].fingerId != fingerLook && t[i].fingerId == fingerMove)
				{
					num3 = i;
					break;
				}
			}
		}
		if (num3 < 0)
		{
			fingerMove = -1;
			int k = 0;
			for (int num5 = t.Length; k < num5; k++)
			{
				for (int l = 0; l < btncount; l++)
				{
					int fingerId2 = t[k].fingerId;
					int num10 = fingerKeys[l];
				}
				if (t[k].fingerId != fingerLook && t[k].position.x < num && t[k].position.y < num2)
				{
					fingerMove = t[k].fingerId;
					num3 = k;
					break;
				}
			}
		}
		if (num3 >= 0)
		{
			float x = t[num3].position.x;
			float y = t[num3].position.y;
			float num6 = GUIM.YRES(buttonpos[12].x + 64f);
			float num7 = GUIM.YRES(buttonpos[12].y - 64f);
			dstart.x = x;
			dstart.y = y;
			dend.x = num6;
			dend.y = num7;
			float num8 = Vector2.Distance(dstart, dend) / GUIM.YRES(128f);
			if (num8 > 1f)
			{
				num8 = 1f;
			}
			dn.x = x - num6;
			dn.y = y - num7;
			dn.Normalize();
			movex = dn.y * num8;
			movez = dn.x * num8;
		}
		OnResizeStick();
	}

	private void UpdateLook()
	{
		float num = GUIM.currwidth / 2f - GUIM.YRES(80f);
		float num2 = GUIM.currheight - GUIM.YRES(100f);
		tx = 0f;
		ty = 0f;
		int num3 = -1;
		if (fingerLook >= 0)
		{
			int i = 0;
			for (int num4 = t.Length; i < num4; i++)
			{
				for (int j = 1; j < btncount; j++)
				{
					int fingerId = t[i].fingerId;
					int num6 = fingerKeys[j];
				}
				if (t[i].fingerId != fingerMove && t[i].fingerId == fingerLook)
				{
					num3 = i;
					break;
				}
			}
		}
		if (num3 < 0)
		{
			fingerLook = -1;
			int k = 0;
			for (int num5 = t.Length; k < num5; k++)
			{
				for (int l = 1; l < btncount; l++)
				{
					int fingerId2 = t[k].fingerId;
					int num7 = fingerKeys[l];
				}
				if (t[k].fingerId != fingerMove && t[k].position.x > num && t[k].position.y < num2)
				{
					fingerLook = t[k].fingerId;
					num3 = k;
					startpos = t[k].position;
					break;
				}
			}
		}
		if (num3 >= 0 && t[num3].phase == TouchPhase.Moved)
		{
			tx = t[num3].deltaPosition.x;
			ty = t[num3].deltaPosition.y;
			tx *= 0.2f * Time.deltaTime * 60f;
			ty *= 0.125f * Time.deltaTime * 60f;
		}
	}

	private void UpdateKey()
	{
		screen_height = Screen.height;
		for (int i = 0; i < btncount; i++)
		{
			skillidx[i] = -1;
			if (GUIOptions.mobilesecondattack == 0 && i == 11)
			{
				continue;
			}
			if (fingerKeys[i] >= 0)
			{
				int j = 0;
				for (int num = t.Length; j < num; j++)
				{
					if ((t[j].fingerId != fingerLook || fingerKeys[i] >= 0) && t[j].fingerId != fingerMove && t[j].fingerId == fingerKeys[i])
					{
						skillidx[i] = j;
						break;
					}
				}
				if (skillidx[i] < 0)
				{
					if (i == 0 || i == 11)
					{
						Controll.manualAttack = false;
					}
					if (i == 1)
					{
						Controll.inJumpKey = false;
					}
					if (i == 5)
					{
						Controll.manualZoom = false;
					}
				}
			}
			if (skillidx[i] >= 0)
			{
				continue;
			}
			fingerKeys[i] = -1;
			int k = 0;
			for (int num2 = t.Length; k < num2; k++)
			{
				if (t[k].fingerId == fingerLook || t[k].fingerId == fingerMove)
				{
					continue;
				}
				v2con.x = t[k].position.x;
				v2con.y = screen_height - t[k].position.y;
				if (!rButton[i].Contains(v2con))
				{
					continue;
				}
				fingerKeys[i] = t[k].fingerId;
				skillidx[i] = k;
				switch (i)
				{
				case 1:
					Controll.inJumpKey = true;
					Controll.inJumpKeyPressed = false;
					break;
				case 2:
					Controll.inDuckKey = !Controll.inDuckKey;
					break;
				case 3:
					if (Controll.currweaponinput == 3)
					{
						Controll.nextweaponkey = 0;
					}
					Controll.ChangeWeapon(Controll.nextweaponkey);
					break;
				case 4:
					Controll.ReloadWeapon();
					break;
				case 6:
					GUIGameMenu.Toggle();
					break;
				case 7:
					Controll.UseSpecial();
					break;
				case 8:
					GUIGameSet.Toggle();
					break;
				case 9:
					HUDMessage.SetChatEdit(!HUDMessage.showchatedit);
					break;
				case 10:
					HUDTab.SetActive(!HUDTab.show);
					break;
				}
				break;
			}
		}
	}

	public void OnResize()
	{
		OnResizeJoy();
		OnResizeStick();
		OnResizeButtons();
	}

	private void OnResizeJoy()
	{
		rJoy = new Rect(GUIM.YRES(buttonpos[12].x) + GUIM.YRES(64f), (float)Screen.height - GUIM.YRES(buttonpos[12].y) + GUIM.YRES(64f), GUIM.YRES(4f), GUIM.YRES(4f));
		rJoyImg = new Rect(0f, 0f, GUIM.YRES(128f), GUIM.YRES(128f));
		rJoyImg.center = new Vector2(rJoy.x, rJoy.y);
	}

	private void OnResizeButtons()
	{
		for (int i = 0; i < rButton.Length; i++)
		{
			if (buttonalign[i] == 0)
			{
				rButton[i].Set((float)Screen.width - GUIM.YRES(buttonpos[i].x), (float)Screen.height - GUIM.YRES(buttonpos[i].y), GUIM.YRES(buttonsize[i].x * (float)buttonscale[i] * 0.01f), GUIM.YRES(buttonsize[i].y * (float)buttonscale[i] * 0.01f));
			}
			else if (buttonalign[i] == 1)
			{
				rButton[i].Set(GUIM.YRES(buttonpos[i].x), GUIM.YRES(buttonpos[i].y), GUIM.YRES(buttonsize[i].x * (float)buttonscale[i] * 0.01f), GUIM.YRES(buttonsize[i].y * (float)buttonscale[i] * 0.01f));
			}
			else if (buttonalign[i] == 2)
			{
				rButton[i].Set((float)Screen.width - GUIM.YRES(buttonpos[i].x), GUIM.YRES(buttonpos[i].y), GUIM.YRES(buttonsize[i].x * (float)buttonscale[i] * 0.01f), GUIM.YRES(buttonsize[i].y * (float)buttonscale[i] * 0.01f));
			}
			else if (buttonalign[i] == 3)
			{
				rButton[i].Set((float)Screen.width / 2f - GUIM.YRES(buttonpos[i].x), GUIM.YRES(buttonpos[i].y), GUIM.YRES(buttonsize[i].x * (float)buttonscale[i] * 0.01f), GUIM.YRES(buttonsize[i].y * (float)buttonscale[i] * 0.01f));
			}
			else if (buttonalign[i] == 4)
			{
				rButton[i].Set(GUIM.YRES(buttonpos[i].x), (float)Screen.height - GUIM.YRES(buttonpos[i].y), GUIM.YRES(buttonsize[i].x * (float)buttonscale[i] * 0.01f), GUIM.YRES(buttonsize[i].y * (float)buttonscale[i] * 0.01f));
			}
		}
	}

	private void OnResizeStick()
	{
		rJoyMove.Set(rJoy.x + movez * GUIM.YRES(40f), rJoy.y - movex * GUIM.YRES(40f), GUIM.YRES(4f), GUIM.YRES(4f));
		rJoyMoveImg.Set(0f, 0f, GUIM.YRES(72f), GUIM.YRES(72f));
		rJoyMoveImg.center = new Vector2(rJoyMove.x, rJoyMove.y);
	}

	private void OnGUI()
	{
		if (!GUIGameExit.show && !GUIOptions.show)
		{
			GUI.depth = -3;
			DrawButtons();
		}
	}

	public void CollectTouch()
	{
		t = Input.touches;
	}

	public void DrawButtons()
	{
		GUI.color = cb;
		GUI.DrawTexture(rJoyImg, tCircle);
		GUI.color = cb;
		GUI.DrawTexture(rButton[0], tCircle);
		GUI.DrawTexture(rButton[1], tCircle);
		GUI.DrawTexture(rButton[2], tCircle);
		GUI.DrawTexture(rButton[4], tCircle);
		GUI.DrawTexture(rButton[5], tCircle);
		GUI.DrawTexture(rButton[6], tCircle);
		GUI.DrawTexture(rButton[7], tCircle);
		GUI.DrawTexture(rButton[8], tCircle);
		GUI.DrawTexture(rButton[9], tCircle);
		GUI.color = ca;
		GUI.DrawTexture(rButton[4], tButton[4]);
		GUI.DrawTexture(rButton[6], tButton[6]);
		GUI.DrawTexture(rButton[8], tButton[8]);
		GUI.DrawTexture(rButton[9], tButton[9]);
		GUI.DrawTexture(rButton[0], tButton[0]);
		GUI.DrawTexture(rButton[1], tButton[1]);
		GUI.DrawTexture(rButton[2], tButton[2]);
		GUI.DrawTexture(rButton[5], tButton[5]);
		GUI.DrawTexture(rButton[7], tButton[7]);
		if (GUIOptions.mobilesecondattack > 0)
		{
			GUI.DrawTexture(rButton[11], tButton[0]);
		}
		GUI.DrawTexture(rJoyMoveImg, tCircle);
		GUI.color = Color.white;
	}

	public void SelectButton()
	{
		bool flag = false;
		if (currbutton >= 0)
		{
			int i = 0;
			for (int num = t.Length; i < num; i++)
			{
				if (t[i].fingerId == currbuttonuid)
				{
					flag = true;
					rButton[currbutton].center = new Vector2(t[i].position.x, (float)Screen.height - t[i].position.y);
					break;
				}
			}
			if (!flag)
			{
				float num2 = (int)((float)Screen.height / 60f);
				float num3 = (int)(rButton[currbutton].x / num2 + 0.5f);
				float num4 = (int)(rButton[currbutton].y / num2 + 0.5f);
				rButton[currbutton].x = num3 * num2;
				rButton[currbutton].y = num4 * num2;
				GUIOptions.UpdateButtonTouch(currbutton);
				HUD.fx = true;
				OnResizeJoy();
				OnResizeStick();
				lastbutton = currbutton;
				currbutton = -1;
			}
		}
		if (flag)
		{
			return;
		}
		for (int j = 0; j < btncount; j++)
		{
			if (GUIOptions.mobilesecondattack == 0 && j == 11)
			{
				continue;
			}
			int k = 0;
			for (int num5 = t.Length; k < num5; k++)
			{
				if (rButton[j].Contains(new Vector2(t[k].position.x, (float)Screen.height - t[k].position.y)))
				{
					lastbutton = -1;
					currbuttonuid = t[k].fingerId;
					currbutton = j;
					return;
				}
			}
		}
	}

	public void DrawButtonsOptions()
	{
		for (int i = 0; i < rButton.Length; i++)
		{
			if (GUIOptions.mobilesecondattack != 0 || i != 11)
			{
				if (i == currbutton)
				{
					GUI.DrawTexture(rButton[i], TEX.tYellow);
				}
				else if (i == lastbutton)
				{
					GUI.DrawTexture(rButton[i], TEX.tOrange);
				}
				else
				{
					GUI.DrawTexture(rButton[i], TEX.tBlack);
				}
			}
		}
		if (lastbutton < 0)
		{
			return;
		}
		int num = (int)GUIM.YRES(64f);
		int num2 = (int)GUIM.YRES(20f);
		Rect rect = new Rect(rButton[lastbutton].x - (float)(num + num2), rButton[lastbutton].y + (rButton[lastbutton].height - (float)num) / 2f, num, num);
		Rect rect2 = rect;
		rect2.x += (float)(num + num2 * 2) + rButton[lastbutton].width;
		Rect r;
		Rect r2 = (r = new Rect(rButton[lastbutton].x - GUIM.YRES(64f), rButton[lastbutton].y - GUIM.YRES(40f), rButton[lastbutton].width + GUIM.YRES(128f), GUIM.YRES(32f)));
		r.y += GUIM.YRES(56f) + rButton[lastbutton].height;
		GUIM.DrawText(r2, sButtonName[lastbutton], TextAnchor.MiddleCenter, BaseColor.White, 1, 26, true);
		if (!buttoncanscale[lastbutton])
		{
			return;
		}
		if (tPlus == null)
		{
			tPlus = Resources.Load("gui/clan_create") as Texture2D;
		}
		if (tMinus == null)
		{
			tMinus = Resources.Load("gui/clan_minus") as Texture2D;
		}
		GUI.DrawTexture(rect, TEX.tBlack);
		GUI.DrawTexture(rect2, TEX.tBlack);
		GUI.DrawTexture(rect, tMinus);
		GUI.DrawTexture(rect2, tPlus);
		GUIM.DrawText(r, buttonscale[lastbutton] + "%", TextAnchor.MiddleCenter, BaseColor.White, 1, 26, true);
		if (GUIM.HideButton(rect))
		{
			buttonscale[lastbutton] -= 10;
			if (buttonscale[lastbutton] < 50)
			{
				buttonscale[lastbutton] = 50;
			}
			Vector2 center = rButton[lastbutton].center;
			rButton[lastbutton].width = GUIM.YRES(buttonsize[lastbutton].x * (float)buttonscale[lastbutton] * 0.01f);
			rButton[lastbutton].height = GUIM.YRES(buttonsize[lastbutton].y * (float)buttonscale[lastbutton] * 0.01f);
			rButton[lastbutton].center = center;
		}
		if (GUIM.HideButton(rect2))
		{
			buttonscale[lastbutton] += 10;
			if (buttonscale[lastbutton] > 200)
			{
				buttonscale[lastbutton] = 200;
			}
			Vector2 center2 = rButton[lastbutton].center;
			rButton[lastbutton].width = GUIM.YRES(buttonsize[lastbutton].x * (float)buttonscale[lastbutton] * 0.01f);
			rButton[lastbutton].height = GUIM.YRES(buttonsize[lastbutton].y * (float)buttonscale[lastbutton] * 0.01f);
			rButton[lastbutton].center = center2;
		}
	}

	private void DrawButtonsDebug()
	{
		GUI.color = cb;
		GUI.DrawTexture(rJoyImg, TEX.tWhite);
		GUI.color = Color.white;
		GUI.DrawTexture(rJoyMoveImg, TEX.tWhite);
		GUI.color = Color.white;
		GUI.color = ca;
		for (int i = 0; i < btncount; i++)
		{
			GUI.DrawTexture(rButton[i], TEX.tWhite);
			GUIM.DrawText(rButton[i], sButtonName[i], TextAnchor.MiddleCenter, BaseColor.Black, 0, 24, false);
		}
		GUI.color = Color.white;
	}

	public void DrawCell()
	{
		if (tCell == null)
		{
			GenerateCell();
		}
		int num = (int)((float)Screen.height / 60f);
		GUI.DrawTexture(new Rect(0f, 0f, num * 128, num * 64), tCell);
		float width = GUIM.currwidth / 2f - GUIM.YRES(80f);
		float height = GUIM.currheight - GUIM.YRES(200f);
		GUI.DrawTexture(new Rect(0f, GUIM.YRES(200f), width, height), TEX.tBlackAlpha);
	}

	private void GenerateCell()
	{
		bool flag = tCell == null;
		Color color = new Color(1f, 1f, 1f, 0.03f);
		Color color2 = new Color(0f, 0f, 0f, 0.03f);
		tCell = new Texture2D(128, 64, TextureFormat.RGBA32, false);
		for (int i = 0; i < 128; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
				{
					tCell.SetPixel(i, j, color);
				}
				else
				{
					tCell.SetPixel(i, j, color2);
				}
			}
		}
		tCell.Apply(false);
		tCell.filterMode = FilterMode.Point;
	}
}
