using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class GUISkinEditor : MonoBehaviour
{
	public static GUISkinEditor cs = null;

	public static string wname;

	public static string skinname = "";

	public static List<string> skinlist = null;

	public static List<WeaponSkin> wskinlist = null;

	private ColorPicker cp;

	private Rect rBack;

	private Rect rBox;

	private Rect rArea;

	private Rect rExit;

	private Rect rSave;

	private Rect rLabel;

	private Texture2D tGray;

	private Texture2D tBlack;

	private Texture2D tWhite;

	private WeaponData wd;

	private Texture2D tSkin;

	private Texture2D tSkinOrg;

	private Texture2D tSkinLS;

	public static Texture2D tSkinImport = null;

	private float ry;

	private float lightoffset = 0.1f;

	private float shadowoffset = 0.1f;

	private int brushsize = 1;

	private float scale = 6f;

	private float posx;

	private float posy;

	private bool shiftlock;

	private bool shiftpick;

	private int lockxy = -1;

	private float lx = -1f;

	private float ly = -1f;

	private void Awake()
	{
		cs = this;
		tGray = TEX.GetTextureByName("gray1");
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		posx = 0f;
		posy = 0f;
		cp = base.gameObject.AddComponent<ColorPicker>();
		wd = VWGen2.BuildWeaponPreview(wname);
		Texture2D texture2D = ContentLoader_.LoadTexture(wname + "_ls") as Texture2D;
		tSkinLS = new Texture2D(wd.tex.width * 2, wd.tex.height * 2, TextureFormat.RGBA32, false);
		tSkinLS.filterMode = FilterMode.Point;
		tSkin = new Texture2D(wd.tex.width * 2, wd.tex.height * 2, TextureFormat.RGBA32, false);
		tSkin.filterMode = FilterMode.Point;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < wd.tex.width; i++)
		{
			for (int j = 0; j < wd.tex.height; j++)
			{
				Color pixel = wd.tex.GetPixel(i, j);
				tSkin.SetPixel(num, num2, pixel);
				tSkin.SetPixel(num + 1, num2, pixel);
				tSkin.SetPixel(num + 1, num2 + 1, pixel);
				tSkin.SetPixel(num, num2 + 1, pixel);
				Color pixel2 = texture2D.GetPixel(i, j);
				tSkinLS.SetPixel(num, num2, pixel2);
				tSkinLS.SetPixel(num + 1, num2, pixel2);
				tSkinLS.SetPixel(num + 1, num2 + 1, pixel2);
				tSkinLS.SetPixel(num, num2 + 1, pixel2);
				num2 += 2;
			}
			num += 2;
		}
		tSkin.Apply(false);
		tSkinLS.Apply(false);
		tSkinOrg = new Texture2D(wd.tex.width * 2, wd.tex.height * 2, TextureFormat.RGBA32, false);
		tSkinOrg.filterMode = FilterMode.Point;
		tSkinOrg.SetPixels(tSkin.GetPixels());
		tSkinOrg.Apply(false);
		wd.mat.SetTexture("_MainTex", tSkin);
		if (tSkinImport != null && tSkinImport.width == tSkin.width && tSkinImport.height == tSkin.height)
		{
			bool flag = true;
			Color[] pixels = tSkin.GetPixels();
			Color[] pixels2 = tSkinImport.GetPixels();
			for (int k = 0; k < pixels.Length; k++)
			{
				if (pixels[k].a != pixels2[k].a)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				tSkin.SetPixels(pixels2);
				tSkin.Apply(false);
			}
		}
		tSkinImport = null;
		Cursor.visible = false;
		OnResize();
	}

	private void OnResize()
	{
		rBack = new Rect(0f, 0f, Screen.width, Screen.height);
		rBox = new Rect(0f, 0f, GUIM.YRES(400f), GUIM.YRES(200f));
		rArea = new Rect(0f, 0f, (float)tSkin.width * scale, (float)tSkin.height * scale);
		rArea.center = new Vector2(Screen.width, Screen.height) / 2f + new Vector2(posx, posy);
		rExit = new Rect((float)Screen.width - GUIM.YRES(84f), (float)Screen.height - GUIM.YRES(40f), GUIM.YRES(80f), GUIM.YRES(32f));
		rSave.Set((float)Screen.width - GUIM.YRES(84f), GUIM.YRES(8f), GUIM.YRES(80f), GUIM.YRES(32f));
		rLabel = new Rect(0f, 0f, Screen.width, GUIM.YRES(64f));
	}

	private void Update()
	{
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			scale += 1f;
			if (scale > 8f)
			{
				scale = 8f;
			}
			OnResize();
			cp.SetCursorSize(brushsize * (int)scale);
		}
		else if (axis < 0f)
		{
			scale -= 1f;
			if (scale < 1f)
			{
				scale = 1f;
			}
			OnResize();
			cp.SetCursorSize(brushsize * (int)scale);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (brushsize == 1)
			{
				brushsize = 2;
			}
			else if (brushsize == 2)
			{
				brushsize = 4;
			}
			cp.SetCursorSize(brushsize * (int)scale);
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			if (brushsize == 4)
			{
				brushsize = 2;
			}
			else if (brushsize == 2)
			{
				brushsize = 1;
			}
			cp.SetCursorSize(brushsize * (int)scale);
		}
		bool flag = false;
		if (Input.GetKey(KeyCode.A))
		{
			posx -= 1f;
			flag = true;
		}
		if (Input.GetKey(KeyCode.D))
		{
			posx += 1f;
			flag = true;
		}
		if (Input.GetKey(KeyCode.W))
		{
			posy -= 1f;
			flag = true;
		}
		if (Input.GetKey(KeyCode.S))
		{
			posy += 1f;
			flag = true;
		}
		if (flag)
		{
			rArea.center = new Vector2(Screen.width, Screen.height) / 2f + new Vector2(posx, posy);
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			shiftlock = true;
			shiftpick = false;
			lockxy = -1;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			shiftlock = false;
		}
	}

	private void OnGUI()
	{
		if (tGray == null)
		{
			return;
		}
		GUI.DrawTexture(rBack, tGray);
		GUIM.DrawText(rLabel, skinname, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		ry += Time.deltaTime * 10f;
		GUIM.DrawBox(rBox, tBlack, 0.05f);
		wd.goWeapon.layer = 9;
		GUI3D.Draw(rBox, wd.goWeapon, new Vector3(0f, 0f, 1f), new Vector3(0f, ry, 0f));
		wd.goWeapon.layer = 0;
		GUI.DrawTexture(rArea, tSkin);
		if ((GUIM.Contains(rArea) && Input.GetMouseButton(0)) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.F))
		{
			float x = Input.mousePosition.x;
			float num = (float)Screen.height - Input.mousePosition.y;
			if (shiftlock && shiftpick)
			{
				if (lockxy < 0)
				{
					int num2 = (int)Mathf.Abs(x - lx);
					int num3 = (int)Mathf.Abs(num - ly);
					if (num2 > 1 || num3 > 1)
					{
						if (num2 > num3)
						{
							lockxy = 0;
						}
						else
						{
							lockxy = 1;
						}
					}
				}
				if (lockxy == 0)
				{
					num = ly;
				}
				else if (lockxy == 1)
				{
					x = lx;
				}
			}
			int x2 = (int)((x - rArea.x) / scale);
			int y = (int)((0f - (num - rArea.y)) / scale);
			if (Input.GetMouseButton(0))
			{
				Paint(x2, y);
				if (shiftlock && !shiftpick)
				{
					lx = x;
					ly = num;
					shiftpick = true;
				}
			}
			else if (Input.GetMouseButton(1))
			{
				Restore(x2, y);
				if (shiftlock && !shiftpick)
				{
					lx = x;
					ly = num;
					shiftpick = true;
				}
			}
			else
			{
				Color pixel = tSkin.GetPixel(x2, y);
				bool flag = false;
				if ((pixel.r == 0f && pixel.g == 0f && pixel.b == 0f) || pixel.a == 0f)
				{
					flag = true;
				}
				if (!flag)
				{
					cp.currColor = pixel;
				}
			}
		}
		if (GUIM.Button(rExit, BaseColor.Black, "EXIT", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false))
		{
			UnityEngine.Object.Destroy(wd.go);
			wd = null;
			UnityEngine.Object.Destroy(cp);
			UnityEngine.Object.Destroy(this);
			Cursor.visible = true;
			Main.SetActive(true);
			GUIPlay.skinstate = -1;
			GUIPlay.skinname = "";
			if (skinlist != null)
			{
				skinlist.Clear();
				skinlist = null;
			}
		}
		if (!GUIM.Button(rSave, BaseColor.Gray, "SAVE", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false))
		{
			return;
		}
		Color32[] pixels = tSkin.GetPixels32();
		int length = wname.Length;
		int num4 = 0;
		string text = GUIOptions.gid.ToString();
		if (text.Length > length)
		{
			length = text.Length;
		}
		length++;
		for (int num5 = pixels.Length - 1; num5 >= 0; num5--)
		{
			if (pixels[num5].a == 0)
			{
				if (num4 > length)
				{
					break;
				}
				if (num4 <= wname.Length)
				{
					if (num4 < wname.Length)
					{
						pixels[num5].r = (byte)wname[num4];
					}
					if (num4 == wname.Length)
					{
						pixels[num5].r = 0;
					}
				}
				if (num4 <= text.Length)
				{
					if (num4 < text.Length)
					{
						pixels[num5].g = (byte)text[num4];
					}
					if (num4 == text.Length)
					{
						pixels[num5].g = 0;
					}
				}
				num4++;
			}
		}
		tSkin.SetPixels32(pixels);
		tSkin.Apply(false);
		string text2 = Convert.ToBase64String(tSkin.EncodeToPNG());
		Application.ExternalCall("savefileskin", text2);
	}

	private void Paint(int x, int y)
	{
		if (brushsize == 1)
		{
			PaintPixel(x, y);
		}
		else if (brushsize == 2)
		{
			PaintPixel(x, y);
			PaintPixel(x + 1, y);
			PaintPixel(x, y + 1);
			PaintPixel(x + 1, y + 1);
		}
		else
		{
			if (brushsize != 4)
			{
				return;
			}
			for (int i = x; i < x + 4; i++)
			{
				for (int j = y; j < y + 4; j++)
				{
					PaintPixel(i, j);
				}
			}
		}
	}

	private void PaintPixel(int x, int y)
	{
		Color pixel = tSkin.GetPixel(x, y);
		Color pixel2 = tSkinLS.GetPixel(x, y);
		bool flag = false;
		if ((pixel.r == 0f && pixel.g == 0f && pixel.b == 0f) || cp.currColor.a == 0f || pixel.a == 0f)
		{
			flag = true;
		}
		if (flag)
		{
			return;
		}
		if ((double)cp.currColor.r < 0.005)
		{
			cp.currColor.r = 0.005f;
		}
		if ((double)cp.currColor.g < 0.005)
		{
			cp.currColor.g = 0.005f;
		}
		if ((double)cp.currColor.b < 0.005)
		{
			cp.currColor.b = 0.005f;
		}
		if (pixel2.a != 0f)
		{
			if (pixel2.r > 0f)
			{
				pixel = cp.currColor;
				pixel.r += lightoffset;
				pixel.g += lightoffset;
				pixel.b += lightoffset;
			}
			else
			{
				pixel = cp.currColor;
				pixel.r -= shadowoffset;
				pixel.g -= shadowoffset;
				pixel.b -= shadowoffset;
				if (pixel.r <= 0f)
				{
					pixel.r = 0.005f;
				}
				if (pixel.g <= 0f)
				{
					pixel.g = 0.005f;
				}
				if (pixel.b <= 0f)
				{
					pixel.b = 0.005f;
				}
			}
			tSkin.SetPixel(x, y, pixel);
		}
		else
		{
			tSkin.SetPixel(x, y, cp.currColor);
		}
		tSkin.Apply(false);
	}

	private void Restore(int x, int y)
	{
		if (brushsize == 1)
		{
			RestorePixel(x, y);
		}
		else if (brushsize == 2)
		{
			RestorePixel(x, y);
			RestorePixel(x + 1, y);
			RestorePixel(x, y + 1);
			RestorePixel(x + 1, y + 1);
		}
		else
		{
			if (brushsize != 4)
			{
				return;
			}
			for (int i = x; i < x + 4; i++)
			{
				for (int j = y; j < y + 4; j++)
				{
					RestorePixel(i, j);
				}
			}
		}
	}

	private void RestorePixel(int x, int y)
	{
		Color pixel = tSkinOrg.GetPixel(x, y);
		bool flag = false;
		if ((pixel.r == 0f && pixel.g == 0f && pixel.b == 0f) || pixel.a == 0f)
		{
			flag = true;
		}
		if (!flag)
		{
			tSkin.SetPixel(x, y, pixel);
			tSkin.Apply(false);
		}
	}

	public static string GetWeaponName(Color32[] c)
	{
		string text = "";
		for (int num = c.Length - 1; num >= 0; num--)
		{
			byte r = c[num].r;
			if (c[num].a == 0)
			{
				if (r == 0)
				{
					break;
				}
				string text2 = text;
				char c2 = (char)r;
				text = text2 + c2;
			}
		}
		return text;
	}

	public static string GetOwnerName(Color32[] c)
	{
		string text = "";
		for (int num = c.Length - 1; num >= 0; num--)
		{
			byte g = c[num].g;
			if (c[num].a == 0)
			{
				if (g == 0)
				{
					break;
				}
				string text2 = text;
				char c2 = (char)g;
				text = text2 + c2;
			}
		}
		return text;
	}
}
