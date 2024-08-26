using System.Collections.Generic;
using UnityEngine;

public class TEX : MonoBehaviour
{
	private static List<Texture2D> texlist = new List<Texture2D>();

	private static Texture2D curTex = null;

	public static Texture2D tBlack = null;

	public static Texture2D tWhite = null;

	public static Texture2D tYellow = null;

	public static Texture2D tGreen = null;

	public static Texture2D tVK = null;

	public static Texture2D tRed = null;

	public static Texture2D tGray = null;

	public static Texture2D tBlue = null;

	public static Texture2D tBlackAlpha = null;

	public static Texture2D tWhiteAlpha = null;

	public static Texture2D tDarkGray = null;

	public static Texture2D tLightGray = null;

	public static Texture2D tOrange = null;

	public static void Init()
	{
		tBlack = GenerateTexture(Color.black, "black");
		tWhite = GenerateTexture(Color.white, "white");
		GenerateTexture(new Color(0.1f, 0.1f, 0.1f, 1f), "gray0");
		GenerateTexture(new Color(0.25f, 0.25f, 0.25f, 1f), "gray1");
		GenerateTexture(new Color(0.35f, 0.35f, 0.35f, 1f), "gray2");
		tRed = GenerateTexture(new Color(1f, 0.2f, 0f, 1f), "red");
		tBlue = GenerateTexture(new Color(0f, 0.4f, 1f, 1f), "blue");
		tYellow = GenerateTexture(new Color(1f, 0.8f, 0f, 1f), "yellow");
		tGray = GenerateTexture(new Color(0.3f, 0.3f, 0.3f, 1f), "gray");
		tGreen = GenerateTexture(new Color(0.15f, 0.8f, 0.15f, 1f), "green");
		GenerateTexture(new Color(0f, 0.5f, 1f, 1f), "lightblue");
		tOrange = GenerateTexture(new Color(0.94f, 0.41f, 0f, 1f), "orange");
		tLightGray = GenerateTexture(new Color(0.8f, 0.8f, 0.8f, 1f), "lightgray");
		GenerateTexture(new Color(1f, 1f, 1f, 0.15f), "white15");
		GenerateTexture(new Color(1f, 1f, 0f, 0.15f), "yellow15");
		GenerateTexture(new Color(0f, 0f, 0f, 0.15f), "black15");
		tVK = GenerateTexture(new Color(0.29f, 0.46f, 0.66f), "vkcolor");
		tBlackAlpha = GenerateTexture(new Color(0f, 0f, 0f, 0.5f), "blackalpha");
		tWhiteAlpha = GenerateTexture(new Color(1f, 1f, 1f, 0.5f), "whitealpha");
		tDarkGray = GenerateTexture(new Color(0.2f, 0.2f, 0.2f, 1f), "darkgray");
	}

	public static Texture2D GenerateTexture(Color c, string _name)
	{
		Texture2D texture2D = new Texture2D(8, 8);
		texture2D.name = _name;
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				texture2D.SetPixel(i, j, c);
			}
		}
		texture2D.Apply(true);
		texlist.Add(texture2D);
		return texture2D;
	}

	public static Texture2D GetTextureByIndex(int index)
	{
		if (texlist.Count < index)
		{
			return null;
		}
		return texlist[index];
	}

	public static Texture2D GetTextureByName(string _name)
	{
		foreach (Texture2D item in texlist)
		{
			if (item.name == _name)
			{
				return item;
			}
		}
		curTex = null;
		string[] array = _name.Split('/');
		if (array.Length > 1)
		{
			curTex = ContentLoader_.LoadTexture(array[array.Length - 1]) as Texture2D;
		}
		else
		{
			curTex = ContentLoader_.LoadTexture(_name) as Texture2D;
		}
		return curTex;
	}

	public static Texture2D LoadTexture(string path, bool filter = false)
	{
		return null;
	}
}
