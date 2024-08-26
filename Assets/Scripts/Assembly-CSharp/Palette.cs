using UnityEngine;

public class Palette : MonoBehaviour
{
	public static bool show = false;

	private static Texture2D palette = null;

	private static Texture2D tWhite;

	private static Color[] p = new Color[102];

	private static int currp = 0;

	private static float selectint = 0f;

	private int key = -1;

	public static void Init()
	{
		tWhite = TEX.GetTextureByName("white");
	}

	public static void SetActive(bool val)
	{
		show = val;
	}

	private void LoadEnd()
	{
		palette = ContentLoader_.LoadTexture("palette") as Texture2D;
		Pars();
	}

	private void Update()
	{
		if (!show || Builder.toolmode != 0 || Controll.toolmode != 0)
		{
			return;
		}
		if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q))
		{
			key = -1;
		}
		bool flag = false;
		if (Input.GetKeyDown(KeyCode.E) || key == 0)
		{
			if (Time.time > selectint || key < 0)
			{
				currp++;
				if (currp > 101)
				{
					currp = 0;
				}
				flag = true;
				selectint = Time.time + 0.03f;
			}
			if (key < 0)
			{
				selectint = Time.time + 0.2f;
			}
			key = 0;
		}
		else if (Input.GetKeyDown(KeyCode.Q) || key == 1)
		{
			if (Time.time > selectint || key < 0)
			{
				currp--;
				if (currp < 0)
				{
					currp = 101;
				}
				flag = true;
				selectint = Time.time + 0.03f;
			}
			if (key < 0)
			{
				selectint = Time.time + 0.2f;
			}
			key = 1;
		}
		if (flag)
		{
			Builder.BuildCursor(false, -1);
		}
	}

	private void OnGUI()
	{
		if (!show || Controll.trControll == null || palette == null || GUIGameMenu.show || Controll.toolmode == 4)
		{
			return;
		}
		int num = (int)GUIM.YRES(6f);
		int num2 = (int)GUIM.YRES(12f);
		int num3 = (int)GUIM.YRES(2f);
		float num4 = GUIM.YRES(360f);
		int num5 = 0;
		for (int i = 0; i < 17; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				if (num5 == currp)
				{
					GUI.color = Color.black;
					GUI.DrawTexture(new Rect(GUIM.YRES(16f) + (float)((num2 + num) * j) - (float)(num3 * 2), num4 + (float)((num2 + num) * i) - (float)(num3 * 2), num2 + num3 * 4, num2 + num3 * 4), tWhite);
					GUI.color = Color.white;
					GUI.DrawTexture(new Rect(GUIM.YRES(16f) + (float)((num2 + num) * j) - (float)num3, num4 + (float)((num2 + num) * i) - (float)num3, num2 + num3 * 2, num2 + num3 * 2), tWhite);
				}
				GUI.color = p[num5];
				num5++;
				GUI.DrawTexture(new Rect(GUIM.YRES(16f) + (float)((num2 + num) * j), num4 + (float)((num2 + num) * i), num2, num2), tWhite);
			}
		}
		GUI.color = Color.white;
	}

	private static void Pars()
	{
		int width = palette.width;
		int height = palette.height;
		int num = 0;
		for (int num2 = height - 1; num2 >= 0; num2 -= 2)
		{
			for (int i = 0; i < width; i += 2)
			{
				p[num] = palette.GetPixel(i, num2);
				num++;
				if (num >= 102)
				{
					return;
				}
			}
		}
	}

	public static Color GetColor()
	{
		return p[currp];
	}

	public static Color GetColor(int c)
	{
		return p[c];
	}

	public static int GetColor(Color c)
	{
		for (int i = 0; i < p.Length; i++)
		{
			if (c.r == p[i].r && c.g == p[i].g && c.b == p[i].b)
			{
				return i;
			}
		}
		return 0;
	}

	public static void SetColor(Color c)
	{
		currp = GetColor(c);
	}
}
