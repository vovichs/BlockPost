using Player;
using UnityEngine;

public class GUIIcon : MonoBehaviour
{
	public static Texture2D CreateIcon(Texture2D org, CharColor cs, CharColor cc)
	{
		if (org == null)
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(16, 16, TextureFormat.RGBA32, false);
		Color color = new Color(0f, 0f, 0f, 0f);
		int num = 13;
		int num2 = 35;
		if (org.name == "skin_hair4")
		{
			num = 41;
		}
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				if (i == 0 || j == 0 || i == 15 || j == 15)
				{
					texture2D.SetPixel(i, j, color);
					continue;
				}
				Color pixel = org.GetPixel(i + num, j + num2);
				if (pixel.a == 0f)
				{
					texture2D.SetPixel(i, j, cs.a);
				}
				else if (pixel.r == 1f && pixel.g == 0f)
				{
					texture2D.SetPixel(i, j, cc.a);
				}
				else if (pixel.g == 1f && pixel.r == 0f)
				{
					texture2D.SetPixel(i, j, cc.b);
				}
				else if (pixel.b == 1f && pixel.r == 0f && pixel.g == 0f)
				{
					texture2D.SetPixel(i, j, cs.b);
				}
				else
				{
					texture2D.SetPixel(i, j, pixel);
				}
			}
		}
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply(false);
		return texture2D;
	}

	public static Texture2D CreateIconBody(Texture2D org, CharColor cs)
	{
		if (org == null)
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(18, 22, TextureFormat.RGBA32, false);
		Color color = new Color(0f, 0f, 0f, 0f);
		for (int i = 0; i < 18; i++)
		{
			for (int j = 0; j < 22; j++)
			{
				if (i == 0 || j == 0 || i == 17 || j == 21)
				{
					texture2D.SetPixel(i, j, color);
					continue;
				}
				Color pixel = org.GetPixel(i + 9, j + 33);
				if (pixel.a == 0f)
				{
					texture2D.SetPixel(i, j, color);
				}
				else if (pixel.r == 1f && pixel.g == 0f)
				{
					texture2D.SetPixel(i, j, cs.a);
				}
				else if (pixel.g == 1f && pixel.r == 0f)
				{
					texture2D.SetPixel(i, j, cs.b);
				}
				else
				{
					texture2D.SetPixel(i, j, pixel);
				}
			}
		}
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply(false);
		return texture2D;
	}

	public static Texture2D CreateIconPants(Texture2D org, CharColor cs)
	{
		if (org == null)
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(18, 23, TextureFormat.RGBA32, false);
		Color color = new Color(0f, 0f, 0f, 0f);
		for (int i = 0; i < 9; i++)
		{
			for (int j = 0; j < 23; j++)
			{
				if (i == 0)
				{
					texture2D.SetPixel(i, j, color);
					texture2D.SetPixel(i + 17, j, color);
					continue;
				}
				Color pixel = org.GetPixel(i + 8 - 1, j + 43 - 1);
				if (pixel.a == 0f)
				{
					texture2D.SetPixel(i, j, color);
				}
				else if (pixel.r == 1f && pixel.g == 0f)
				{
					texture2D.SetPixel(i, j, cs.a);
				}
				else if (pixel.g == 1f && pixel.r == 0f)
				{
					texture2D.SetPixel(i, j, cs.b);
				}
				else
				{
					texture2D.SetPixel(i, j, pixel);
				}
				pixel = texture2D.GetPixel(i, j);
				texture2D.SetPixel(i + 8, j, pixel);
			}
		}
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply(false);
		return texture2D;
	}

	public static Texture2D CreateIconBoots(Texture2D org, CharColor cs)
	{
		if (org == null)
		{
			return null;
		}
		int num = 18;
		int num2 = 12;
		int num3 = -1;
		int num4 = 18;
		Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, false);
		Color color = new Color(0f, 0f, 0f, 0f);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				if (i == 0 || j == 0 || i == num - 1 || j == num2 - 1)
				{
					texture2D.SetPixel(i, j, color);
					continue;
				}
				Color pixel = org.GetPixel(i + num3, j + num4);
				if (pixel.a == 0f)
				{
					texture2D.SetPixel(i, j, color);
				}
				else if (pixel.r == 1f && pixel.g == 0f)
				{
					texture2D.SetPixel(i, j, cs.a);
				}
				else if (pixel.g == 1f && pixel.r == 0f)
				{
					texture2D.SetPixel(i, j, cs.b);
				}
				else if (i < 9)
				{
					texture2D.SetPixel(i, j, pixel);
					texture2D.SetPixel(i + 8, j, pixel);
				}
			}
		}
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply(false);
		return texture2D;
	}
}
