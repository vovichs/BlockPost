using System;
using UnityEngine;

public class GUIM : MonoBehaviour
{
	public static Color[] colorlist = new Color[12];

	public static GUIStyle guistyle = new GUIStyle();

	public static Font[] fontlist = new Font[2];

	private static int nextelement = 0;

	private static bool debug = false;

	private static Texture2D tDebug = null;

	private static Texture2D[] tBar = new Texture2D[6];

	public static float offsetx = 0f;

	public static float offsety = 0f;

	public static float offsety2 = 0f;

	public static float viewzoneheight = 0f;

	private static Texture2D tButton = null;

	private static Texture2D[] tButtonPic = new Texture2D[3];

	public static float currwidth;

	public static float currheight;

	private static Rect r2;

	private static Color org;

	private static string hackControlName = "matrix";

	private static Rect rBar;

	private static int hover_uid = 0;

	private static float hover_time = 0f;

	private float lastwidth;

	private GameObject goGUI;

	public static void Init()
	{
		colorlist[0] = new Color(0f, 0f, 0f, 1f);
		colorlist[1] = new Color(1f, 1f, 1f, 1f);
		colorlist[2] = new Color(1f, 0f, 0f, 1f);
		colorlist[3] = new Color(0f, 1f, 0f, 1f);
		colorlist[4] = new Color(0f, 0.4f, 1f, 1f);
		colorlist[5] = new Color(1f, 0.8f, 0f, 1f);
		colorlist[6] = new Color(0.5f, 0.5f, 0.5f, 1f);
		colorlist[7] = new Color(0.94f, 0.41f, 0f, 1f);
		colorlist[8] = new Color(1f, 0.4f, 0.35f, 1f);
		colorlist[9] = new Color(0.75f, 0.75f, 0.75f, 1f);
		colorlist[10] = new Color(0.784f, 0.784f, 0.784f, 1f);
		colorlist[11] = new Color(0.2f, 0.2f, 0.2f, 1f);
		currwidth = Screen.width;
		currheight = Screen.height;
	}

	public static void LoadFont()
	{
		if (fontlist[0] == null)
		{
			fontlist[0] = ContentLoader_.LoadFont("BravoSC");
		}
		if (fontlist[1] == null)
		{
			fontlist[1] = ContentLoader_.LoadFont("Kelson Sans Regular RU");
		}
	}

	public void LoadEnd()
	{
		if (fontlist[0] == null)
		{
			fontlist[0] = ContentLoader_.LoadFont("BravoSC");
		}
		if (fontlist[1] == null)
		{
			fontlist[1] = ContentLoader_.LoadFont("Kelson Sans Regular RU");
		}
		if (fontlist[0] == null)
		{
			Debug.Log("error load font");
		}
		if (fontlist[1] == null)
		{
			Debug.Log("error load font");
		}
		tDebug = TEX.GetTextureByName("red");
		tBar[0] = TEX.GetTextureByName("lightgray");
		tBar[1] = TEX.GetTextureByName("lightgray");
		tBar[2] = TEX.GetTextureByName("lightgray");
		tBar[3] = TEX.GetTextureByName("lightgray");
		tBar[4] = TEX.GetTextureByName("red");
		tBar[5] = TEX.GetTextureByName("gray");
		tButton = TEX.GetTextureByName("button");
		tButtonPic[0] = TEX.GetTextureByName("button_start");
		tButtonPic[1] = TEX.GetTextureByName("button_body");
		tButtonPic[2] = TEX.GetTextureByName("button_end");
	}

	public static void ResetGUI()
	{
		nextelement = 0;
	}

	public static void DrawText(Rect r, string text, TextAnchor pos, BaseColor fontcolor, int fontpos, int fontsize, bool fontshadow)
	{
		fontsize = (int)YRES(fontsize);
		guistyle.font = fontlist[fontpos];
		guistyle.alignment = pos;
		guistyle.fontSize = fontsize;
		if (fontshadow)
		{
			int num = (int)YRES(2f);
			if (num < 1)
			{
				num = 1;
			}
			if (num > 4)
			{
				num = 4;
			}
			guistyle.normal.textColor = colorlist[0];
			r2.Set(r.x + (float)num, r.y + (float)num, r.width, r.height);
			GUI.Label(r2, text, guistyle);
		}
		guistyle.normal.textColor = colorlist[(int)fontcolor];
		GUI.Label(r, text, guistyle);
	}

	public static void DrawTextShadow(Rect r, string text, TextAnchor pos, BaseColor fontcolor, int fontpos, int fontsize, BaseColor shadowcolor)
	{
		fontsize = (int)YRES(fontsize);
		guistyle.font = fontlist[fontpos];
		guistyle.alignment = pos;
		guistyle.fontSize = fontsize;
		guistyle.normal.textColor = colorlist[(int)shadowcolor];
		r2.Set(r.x + 3f, r.y + 3f, r.width, r.height);
		GUI.Label(r2, text, guistyle);
		guistyle.normal.textColor = colorlist[0];
		r2.Set(r.x + 2f, r.y + 2f, r.width, r.height);
		GUI.Label(r2, text, guistyle);
		guistyle.normal.textColor = colorlist[(int)fontcolor];
		GUI.Label(r, text, guistyle);
	}

	public static void DrawTextColor(Rect r, string text, TextAnchor pos, BaseColor fontcolor, int fontpos, int fontsize, bool fontshadow)
	{
		fontsize = (int)YRES(fontsize);
		guistyle.font = fontlist[fontpos];
		guistyle.alignment = pos;
		guistyle.fontSize = fontsize;
		if (fontshadow)
		{
			string text2 = text;
			text2 = text2.Replace("<color=", "");
			text2 = text2.Replace("</color>", "");
			text2 = text2.Replace("#F20>", "");
			text2 = text2.Replace("#F60>", "");
			text2 = text2.Replace("#06F>", "");
			text2 = text2.Replace("#FFF>", "");
			text2 = text2.Replace("#FF0>", "");
			text2 = text2.Replace("#2F2>", "");
			guistyle.normal.textColor = colorlist[0];
			r2.Set(r.x + 1f, r.y + 1f, r.width, r.height);
			GUI.Label(r2, text2, guistyle);
		}
		guistyle.normal.textColor = colorlist[(int)fontcolor];
		GUI.Label(r, text, guistyle);
	}

	public static void DrawEdit(Rect r, ref string text, TextAnchor pos, BaseColor fontcolor, int fontpos, int fontsize, bool fontshadow)
	{
		fontsize = (int)YRES(fontsize);
		GUI.SetNextControlName("123");
		guistyle.richText = false;
		guistyle.wordWrap = false;
		guistyle.clipping = TextClipping.Clip;
		guistyle.font = fontlist[fontpos];
		guistyle.alignment = pos;
		guistyle.fontSize = fontsize;
		if (fontshadow)
		{
			guistyle.normal.textColor = colorlist[0];
			r2.Set(r.x + 1f, r.y + 1f, r.width, r.height);
			GUI.Label(r2, text, guistyle);
		}
		guistyle.normal.textColor = colorlist[(int)fontcolor];
		try
		{
			text = GUI.TextField(r, text, guistyle);
		}
		catch (Exception ex)
		{
			bool flag = ex is ArgumentOutOfRangeException;
		}
		guistyle.clipping = TextClipping.Overflow;
		guistyle.richText = true;
		GUI.FocusControl("123");
	}

	public void SetFocus()
	{
		GUI.FocusControl("element" + (nextelement - 1));
	}

	public void AutoSetName()
	{
		GUI.SetNextControlName("element" + nextelement);
		nextelement++;
	}

	public void SetFocusHack()
	{
		GUI.SetNextControlName(hackControlName);
		GUI.Button(new Rect(-10000f, -10000f, 0f, 0f), GUIContent.none);
	}

	public static float YRES(float val)
	{
		return val * ((float)Screen.height / 720f);
	}

	public static float NATIVERES(float val)
	{
		return val * (720f / (float)Screen.height);
	}

	public static Vector2 CalcSize(string text, int fontpos, int fontsize)
	{
		fontsize = (int)YRES(fontsize);
		guistyle.font = fontlist[fontpos];
		guistyle.fontSize = fontsize;
		return guistyle.CalcSize(new GUIContent(text));
	}

	public static Vector2 BeginScrollViewORG(Rect viewzone, Vector2 scrollViewVector, Rect scrollzone)
	{
		GUI.skin.verticalScrollbar.normal.background = null;
		GUI.skin.verticalScrollbarThumb.normal.background = null;
		scrollViewVector = GUI.BeginScrollView(viewzone, scrollViewVector, scrollzone);
		float height = viewzone.height / scrollzone.height * viewzone.height;
		float num = scrollViewVector.y / scrollzone.height * viewzone.height;
		if (scrollzone.height <= viewzone.height)
		{
			rBar.height = 0f;
		}
		else
		{
			rBar = new Rect(viewzone.x + viewzone.width - 14f, viewzone.y + num, 14f, height);
		}
		return scrollViewVector;
	}

	public static void EndScrollViewORG()
	{
		GUI.EndScrollView();
	}

	public static void DrawBar(Texture2D top, Texture2D middle, Texture2D bottom)
	{
		if (rBar.height != 0f)
		{
			GUI.DrawTexture(new Rect(rBar.x, rBar.y, rBar.width, 4f), top);
			if (rBar.height - 8f > 0f)
			{
				GUI.DrawTexture(new Rect(rBar.x, rBar.y, rBar.width, rBar.height + 2f), middle);
			}
			GUI.DrawTexture(new Rect(rBar.x, rBar.y + rBar.height - 4f, rBar.width, 4f), bottom);
		}
	}

	public static bool HideButton(Rect r)
	{
		return GUI.Button(r, "", guistyle);
	}

	public static bool HideButtonDown(Rect r)
	{
		Vector2 mpos = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		if (Contains(r, mpos))
		{
			return Input.GetMouseButton(0);
		}
		return false;
	}

	public static Vector2 BeginScrollView(Rect viewzone, Vector2 scrollViewVector, Rect scrollzone)
	{
		offsetx = viewzone.x;
		offsety = viewzone.y;
		viewzoneheight = viewzone.height;
		Vector2 result = BeginScrollViewORG(viewzone, scrollViewVector, scrollzone);
		offsety2 = result.y;
		return result;
	}

	public static void EndScrollView()
	{
		EndScrollViewORG();
		DrawBar(tBar[0], tBar[1], tBar[2]);
		offsetx = 0f;
		offsety = 0f;
		offsety2 = 0f;
	}

	public static bool Contains(Rect r, Vector2 mpos)
	{
		return r.Contains(new Vector2(mpos.x - offsetx, mpos.y - offsety + offsety2));
	}

	public static bool Contains(Rect r)
	{
		Vector2 mpos = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		return Contains(r, mpos);
	}

	public static bool Contains(Rect r, Rect rmin)
	{
		Vector2 vector = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		if (!rmin.Contains(vector))
		{
			return false;
		}
		return Contains(r, vector);
	}

	public static void DrawBox(Rect r, Texture2D t)
	{
		int num = (int)YRES(1f);
		if (num < 1)
		{
			num = 1;
		}
		GUI.color = new Color(1f, 1f, 1f, 0.4f);
		GUI.DrawTexture(new Rect(r.x - (float)num, r.y - (float)num, r.width + (float)(num * 2), r.height + (float)(num * 2)), t);
		GUI.color = new Color(1f, 1f, 1f, 0.4f);
		GUI.DrawTexture(r, t);
		GUI.color = new Color(1f, 1f, 1f, 0.2f);
		GUI.DrawTexture(new Rect(r.x, r.y - (float)(num * 2), r.width, num), t);
		GUI.DrawTexture(new Rect(r.x, r.y + r.height + (float)num, r.width, num), t);
		GUI.DrawTexture(new Rect(r.x - (float)(num * 2), r.y, num, r.height), t);
		GUI.DrawTexture(new Rect(r.x + r.width + (float)num, r.y, num, r.height), t);
		GUI.color = Color.white;
	}

	public static void DrawBox(Rect r, Texture2D t, float a)
	{
		int num = (int)YRES(1f);
		if (num < 1)
		{
			num = 1;
		}
		GUI.color = new Color(1f, 1f, 1f, a + a);
		GUI.DrawTexture(new Rect(r.x - (float)num, r.y - (float)num, r.width + (float)(num * 2), r.height + (float)(num * 2)), t);
		GUI.color = new Color(1f, 1f, 1f, a + a);
		GUI.DrawTexture(r, t);
		GUI.color = new Color(1f, 1f, 1f, a);
		GUI.DrawTexture(new Rect(r.x, r.y - (float)(num * 2), r.width, num), t);
		GUI.DrawTexture(new Rect(r.x, r.y + r.height + (float)num, r.width, num), t);
		GUI.DrawTexture(new Rect(r.x - (float)(num * 2), r.y, num, r.height), t);
		GUI.DrawTexture(new Rect(r.x + r.width + (float)num, r.y, num, r.height), t);
		GUI.color = Color.white;
	}

	public static void DrawBoxBorder(Rect r, Texture2D t, float a)
	{
		int num = (int)YRES(1f);
		if (num < 1)
		{
			num = 1;
		}
		GUI.color = new Color(1f, 1f, 1f, a);
		GUI.DrawTexture(new Rect(r.x, r.y - (float)num, r.width, num), t);
		GUI.DrawTexture(new Rect(r.x, r.y + r.height, r.width, num), t);
		GUI.DrawTexture(new Rect(r.x - (float)num, r.y, num, r.height), t);
		GUI.DrawTexture(new Rect(r.x + r.width, r.y, num, r.height), t);
		GUI.color = Color.white;
	}

	public static float DrawSlider(Rect r, int size, float start, float end, float val)
	{
		GUI.skin.horizontalSliderThumb.normal.background = TEX.tYellow;
		GUI.skin.horizontalSliderThumb.hover.background = tBar[4];
		GUI.skin.horizontalSliderThumb.active.background = tBar[4];
		GUI.skin.horizontalSlider.normal.background = null;
		GUI.skin.horizontalSliderThumb.fixedHeight = YRES(32f);
		GUI.skin.horizontalSliderThumb.fixedWidth = YRES(32f);
		GUI.skin.horizontalSlider.fixedHeight = YRES(16f);
		GUI.skin.horizontalSlider.fixedWidth = size;
		GUILayout.BeginArea(r);
		val = GUILayout.HorizontalSlider(val, start, end, GUILayout.Width(size));
		GUILayout.EndArea();
		return val;
	}

	public static bool ButtonPic(Rect r, BaseColor c, string text, TextAnchor anchor, BaseColor tc, int fp, int size, bool shadow, Texture2D tPic)
	{
		if (tButtonPic[0] == null)
		{
			return false;
		}
		float num = 0.75f;
		if (Contains(mpos: new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y), r: r))
		{
			num = 1f;
		}
		GUI.color = new Color(colorlist[(int)c].r * num, colorlist[(int)c].g * num, colorlist[(int)c].b * num, colorlist[(int)c].a);
		GUI.DrawTexture(new Rect(r.x, r.y, r.width, r.height * 1.6f), tButtonPic[1]);
		GUI.DrawTexture(new Rect(r.x, r.y, r.height * 1.6f, r.height * 1.6f), tButtonPic[0]);
		GUI.DrawTexture(new Rect(r.x + r.width - r.height * 1.6f, r.y, r.height * 1.6f, r.height * 1.6f), tButtonPic[2]);
		if (tPic != null)
		{
			GUI.DrawTexture(new Rect(r.x, r.y, r.width * 1.6f, r.height * 1.6f), tPic);
		}
		GUI.color = Color.white;
		DrawText(r, text, anchor, tc, fp, size, shadow);
		return HideButton(r);
	}

	public static bool Button(Rect r, BaseColor c, string text, TextAnchor anchor, BaseColor tc, int fp, int size, bool shadow)
	{
		if (tButton == null)
		{
			return false;
		}
		GUI.color = colorlist[(int)c];
		GUI.DrawTexture(r, tButton);
		GUI.color = Color.white;
		if (Contains(r))
		{
			int num = (int)(r.x + r.y + r.width + r.height);
			if (num != hover_uid)
			{
				hover_time = 0f;
			}
			if (hover_time == 0f)
			{
				hover_uid = num;
				hover_time = Time.time;
			}
			float num2 = Time.time - hover_time;
			if (num2 > 0f)
			{
				num2 *= 10f;
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				GUI.color = colorlist[5];
				GUI.DrawTexture(r, tButton);
				GUI.color = Color.white;
			}
			tc = BaseColor.DarkGray;
		}
		else if (hover_time != 0f)
		{
			hover_time = 0f;
			hover_uid = 0;
		}
		DrawText(r, text, anchor, tc, fp, size, shadow);
		return HideButton(r);
	}

	private void Update()
	{
		if (lastwidth != (float)Screen.width)
		{
			if (goGUI == null)
			{
				goGUI = GameObject.Find("GUI");
			}
			lastwidth = Screen.width;
			goGUI.BroadcastMessage("OnResize", SendMessageOptions.DontRequireReceiver);
			currwidth = Screen.width;
			currheight = Screen.height;
		}
	}
}
