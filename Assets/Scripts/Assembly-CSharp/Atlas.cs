using UnityEngine;

public class Atlas : MonoBehaviour
{
	public static Material mat = null;

	public static Material grass = null;

	public static Material stone = null;

	public static Texture2D tex = null;

	public static Texture2D texs = null;

	public static Texture2D texmark = null;

	private const int aw = 16;

	private const int ah = 16;

	public const int tilecount = 52;

	public const int tilemax = 256;

	private static Vector2[][] blockuv = new Vector2[256][];

	private static Vector2[][] blockuvhard = new Vector2[256][];

	private static Vector2[][] blockuv_top = new Vector2[256][];

	private static Vector2[][] blockuvhard_top = new Vector2[256][];

	private static Vector2[][] blockuv_bottom = new Vector2[256][];

	private static Vector2[][] blockuvhard_bottom = new Vector2[256][];

	public static Rect[] blocktx = new Rect[256];

	private static Texture2D newtex = null;

	private static Texture2D newtexs = null;

	public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
	{
		source.filterMode = FilterMode.Point;
		RenderTexture temporary = RenderTexture.GetTemporary(newWidth, newHeight);
		temporary.filterMode = FilterMode.Point;
		RenderTexture.active = temporary;
		Graphics.Blit(source, temporary);
		Texture2D texture2D = new Texture2D(newWidth, newHeight);
		texture2D.ReadPixels(new Rect(0f, 0f, newWidth, newWidth), 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		return texture2D;
	}

	public static void SetFilter(bool val)
	{
		if (mat == null || tex == null)
		{
			return;
		}
		if (val)
		{
			if (newtex == null)
			{
				newtex = Resize(tex, 2048, 2048);
				newtex.filterMode = FilterMode.Point;
				newtex.mipMapBias = 0f;
				newtexs = Resize(texs, 2048, 2048);
				newtexs.filterMode = FilterMode.Point;
				newtexs.mipMapBias = 0f;
			}
			mat.SetTexture("_MainTex", newtex);
			mat.SetTexture("_MetallicGlossMap", newtexs);
		}
		else
		{
			mat.SetTexture("_MainTex", tex);
			mat.SetTexture("_MetallicGlossMap", texs);
			if (newtex != null)
			{
				Object.Destroy(newtex);
			}
			if (newtexs != null)
			{
				Object.Destroy(newtexs);
			}
		}
	}

	public static void Init()
	{
		tex = ContentLoader_.LoadTexture("atlas") as Texture2D;
		texs = ContentLoader_.LoadTexture("atlas_s") as Texture2D;
		mat = ContentLoader_.LoadMaterial("matlas");
		SetFilter((GUIOptions.aa > 0) ? true : false);
		float num = 0.0625f;
		float num2 = 0.0625f;
		int num3 = 0;
		for (int num4 = 15; num4 >= 0; num4--)
		{
			for (int i = 0; i < 16; i++)
			{
				float x = (float)i * num;
				float x2 = (float)i * num + num;
				float y = (float)num4 * num2;
				float y2 = (float)num4 * num2 + num2;
				blockuv[num3] = new Vector2[4]
				{
					new Vector2(x2, y),
					new Vector2(x2, y2),
					new Vector2(x, y2),
					new Vector2(x, y)
				};
				blockuvhard[num3] = new Vector2[6]
				{
					new Vector2(x2, y),
					new Vector2(x2, y2),
					new Vector2(x, y2),
					new Vector2(x, y),
					new Vector2(x2, y),
					new Vector2(x, y2)
				};
				blocktx[num3] = new Rect(x, y, num, num2);
				num3++;
			}
		}
		AddCustomUV(0, 255, 8);
		AddCustomUV(1, 254, 8);
		AddCustomUV(2, 253, 8);
		AddCustomUV(3, 252, 8);
		AddCustomUV(4, 251, 251);
		AddCustomUV(5, 250, 250);
		AddCustomUV(6, 249, 249);
		AddCustomUV(7, 248, 248);
		Color32[] pixels = tex.GetPixels32();
		texmark = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
		texmark.SetPixels32(pixels);
		texmark.filterMode = FilterMode.Point;
		Color black = Color.black;
		for (int j = 0; j < tex.width; j += 16)
		{
			for (int k = 0; k < tex.height; k += 16)
			{
				for (int l = 0; l < 16; l++)
				{
					texmark.SetPixel(j + l, k, black);
					texmark.SetPixel(j, k + l, black);
					texmark.SetPixel(j + 15, k + l, black);
					texmark.SetPixel(j + l, k + 15, black);
				}
			}
		}
		texmark.Apply(false);
	}

	public static void AddCustomUV(int org, int top, int bottom)
	{
		blockuv_top[org] = blockuv[top];
		blockuv_bottom[org] = blockuv[bottom];
	}

	public static Vector2[] GetUV(int flag, bool hard = false)
	{
		Vector2[] array = null;
		if (hard)
		{
			return blockuvhard[flag - 1];
		}
		return blockuv[flag - 1];
	}

	public static Vector2[] GetUV(int flag, int face)
	{
		Vector2[] array = null;
		if (face == 4 && blockuv_top[flag - 1] != null)
		{
			return blockuv_top[flag - 1];
		}
		if (face == 5 && blockuv_bottom[flag - 1] != null)
		{
			return blockuv_bottom[flag - 1];
		}
		return blockuv[flag - 1];
	}
}
