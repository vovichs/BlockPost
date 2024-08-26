using UnityEngine;

public class Crosshair : MonoBehaviour
{
	public static bool show = false;

	private Texture2D tBlack;

	private Texture2D tWhite;

	private static int maxstep = 6;

	private static Rect[,] rC = new Rect[maxstep, 4];

	private static Rect rM;

	private static Rect rDot;

	private static int crosshair_type = 0;

	private static int crosshair_offset = 0;

	private static int crosshair_attack = 0;

	private static int crosshair_currOffset = 0;

	private static int crosshair_size = 1;

	private static Texture2D[] cr0 = new Texture2D[4];

	private static Texture2D[] cr1 = new Texture2D[3];

	private static Texture2D[] cr2 = new Texture2D[2];

	private static Texture2D[] cr3 = new Texture2D[4];

	private static Texture2D cr_mark;

	private static Texture2D crFront;

	private static Texture2D crBack;

	public static int crosshair_color = 0;

	private static Color mark_color;

	private static float tUpdate = 0f;

	private static float tMark = 0f;

	private static int size = 16;

	private static int size2 = size / 2;

	private static int x = 0;

	private static int y = 0;

	private static Color[] cc = new Color[5]
	{
		Color.white,
		new Color(1f / 51f, 0.83137256f, 0f),
		Color.yellow,
		new Color(0.29803923f, 32f / 51f, 1f),
		new Color(1f, 0.5f, 0f)
	};

	private static Color ctr = new Color(1f, 1f, 1f, 0.2f);

	private float t;

	private float tl;

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		cr_mark = ContentLoader_.LoadTexture("cr_mark") as Texture2D;
		cr0[0] = ContentLoader_.LoadTexture("cr0_left") as Texture2D;
		cr0[1] = ContentLoader_.LoadTexture("cr0_right") as Texture2D;
		cr0[2] = ContentLoader_.LoadTexture("cr0_top") as Texture2D;
		cr0[3] = ContentLoader_.LoadTexture("cr0_down") as Texture2D;
		cr1[0] = ContentLoader_.LoadTexture("cr1_left") as Texture2D;
		cr1[1] = ContentLoader_.LoadTexture("cr1_right") as Texture2D;
		cr1[2] = ContentLoader_.LoadTexture("cr1_down") as Texture2D;
		cr2[0] = ContentLoader_.LoadTexture("cr1_left") as Texture2D;
		cr2[1] = ContentLoader_.LoadTexture("cr1_right") as Texture2D;
		crFront = ContentLoader_.LoadTexture("cr2_front") as Texture2D;
		crBack = ContentLoader_.LoadTexture("cr2_back") as Texture2D;
		OnResize();
	}

	public static void SetActive(bool val)
	{
		if (!val || HUD.isActive())
		{
			show = val;
		}
	}

	public static void SetCursor(bool val)
	{
		if (val)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public static void SetCrosshair(int val)
	{
		crosshair_type = val;
		static_OnResize();
	}

	public static void SetCrosshairSize(int val)
	{
		if (val < 1)
		{
			val = 1;
		}
		if (val > 4)
		{
			val = 4;
		}
		crosshair_size = val;
		size = 16 * val;
		size2 = size / 2;
		static_OnResize();
	}

	public static void SetMark(Color c)
	{
		tMark = Time.time + 0.5f;
		mark_color = c;
	}

	private void OnResize()
	{
		static_OnResize();
	}

	private static void static_OnResize()
	{
		int num = (int)GUIM.YRES(1f);
		if (num < 1)
		{
			num = 1;
		}
		int num2 = num * 4;
		x = (int)((float)Screen.width / 2f);
		y = (int)((float)Screen.height / 2f);
		for (int i = 0; i < maxstep; i++)
		{
			rC[i, 0] = new Rect(x - size - num2, y - size2, size, size);
			rC[i, 1] = new Rect(x + num2, y - size2, size, size);
			rC[i, 2] = new Rect(x - size2, y - size - num2, size, size);
			rC[i, 3] = new Rect(x - size2, y + num2, size, size);
			num2++;
		}
		rM.Set(x - size, y - size, size * 2, size * 2);
		rDot.Set(x - size2, y - size2, size, size);
	}

	private void Update()
	{
		bool show2 = show;
	}

	private void OnGUI()
	{
		DrawHitMark();
		if (!show || Controll.trControll == null)
		{
			return;
		}
		if (Controll.speed > 0f)
		{
			if (Controll.inAir)
			{
				crosshair_offset = maxstep - 1;
			}
			else if (Controll.inDuck)
			{
				crosshair_offset = 1;
			}
			else
			{
				crosshair_offset = 2;
			}
		}
		else if (Controll.inAir)
		{
			crosshair_offset = maxstep - 1;
		}
		else if (Controll.inDuck)
		{
			crosshair_offset = 0;
		}
		else
		{
			crosshair_offset = 1;
		}
		if (Controll.inAttack)
		{
			crosshair_attack = 5;
		}
		if (Time.time > tUpdate)
		{
			tUpdate = Time.time + 0.02f;
			if (crosshair_offset + crosshair_attack > crosshair_currOffset)
			{
				crosshair_currOffset++;
			}
			else if (crosshair_offset + crosshair_attack < crosshair_currOffset)
			{
				crosshair_currOffset--;
			}
			if (crosshair_currOffset < 0)
			{
				crosshair_currOffset = 0;
			}
			if (crosshair_currOffset > maxstep - 1)
			{
				crosshair_currOffset = maxstep - 1;
			}
			crosshair_attack--;
			if (crosshair_attack < 0)
			{
				crosshair_attack = 0;
			}
		}
		GUI.color = cc[crosshair_color];
		if (crosshair_type == 0)
		{
			GUI.DrawTexture(rC[crosshair_currOffset, 0], cr0[0]);
			GUI.DrawTexture(rC[crosshair_currOffset, 1], cr0[1]);
			GUI.DrawTexture(rC[crosshair_currOffset, 2], cr0[2]);
			GUI.DrawTexture(rC[crosshair_currOffset, 3], cr0[3]);
		}
		else if (crosshair_type == 1)
		{
			GUI.DrawTexture(rC[crosshair_currOffset, 0], cr0[0]);
			GUI.DrawTexture(rC[crosshair_currOffset, 1], cr0[1]);
			GUI.DrawTexture(rC[crosshair_currOffset, 2], cr0[2]);
			GUI.DrawTexture(rC[crosshair_currOffset, 3], cr0[3]);
			GUI.DrawTexture(rDot, crFront);
		}
		else if (crosshair_type == 2)
		{
			GUI.DrawTexture(rDot, crFront);
		}
		else if (crosshair_type == 3)
		{
			GUI.DrawTexture(rC[crosshair_currOffset, 0], cr1[0]);
			GUI.DrawTexture(rC[crosshair_currOffset, 1], cr1[1]);
			GUI.DrawTexture(rC[crosshair_currOffset, 3], cr1[2]);
		}
		else if (crosshair_type == 4)
		{
			GUI.DrawTexture(rC[crosshair_currOffset, 0], cr1[0]);
			GUI.DrawTexture(rC[crosshair_currOffset, 1], cr1[1]);
			GUI.DrawTexture(rC[crosshair_currOffset, 3], cr1[2]);
			GUI.DrawTexture(rDot, crFront);
		}
		else if (crosshair_type == 5)
		{
			GUI.DrawTexture(rC[crosshair_currOffset, 1], cr2[0]);
			GUI.DrawTexture(rC[crosshair_currOffset, 0], cr2[1]);
		}
		else if (crosshair_type == 6)
		{
			GUI.DrawTexture(rC[crosshair_currOffset, 1], cr2[0]);
			GUI.DrawTexture(rC[crosshair_currOffset, 0], cr2[1]);
			GUI.DrawTexture(rDot, crFront);
		}
		else if (crosshair_type == 7)
		{
			GUI.color = ctr;
			GUI.DrawTexture(rM, crBack);
			GUI.color = Color.white;
			GUI.DrawTexture(rDot, crFront);
		}
	}

	private void DrawHitMark()
	{
		if (tMark != 0f && Time.time < tMark)
		{
			GUI.color = mark_color;
			float num = (tMark - Time.time) * 32f;
			if (num < 0f)
			{
				num = 0f;
			}
			if (num > 8f)
			{
				num = 16f - num;
			}
			float num2 = num * 2f;
			rM.Set((float)(x - size) - num, (float)(y - size) - num, (float)(size * 2) + num2, (float)(size * 2) + num2);
			GUI.DrawTexture(rM, cr_mark);
			GUI.color = Color.white;
		}
	}
}
