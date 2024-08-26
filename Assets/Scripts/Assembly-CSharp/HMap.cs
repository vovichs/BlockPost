using UnityEngine;

public class HMap : MonoBehaviour
{
	public class CHChunk
	{
		public int x;

		public int z;

		public int[,] height;

		public int[,] block;

		public Color[,] color;

		public CHChunk(int x, int z)
		{
			this.x = x;
			this.z = z;
			height = new int[128, 128];
			block = new int[128, 128];
			color = new Color[128, 128];
		}

		~CHChunk()
		{
			height = null;
			block = null;
			color = null;
		}
	}

	public const int CHUNK_SIZE_X = 128;

	public const int CHUNK_SIZE_Z = 128;

	private static Texture2D tex_terrain = null;

	private static Texture2D tex_color = null;

	public static Texture2D minimap = null;

	public static int MAP_SIZE_X = 0;

	public static int MAP_SIZE_Z = 0;

	public static int BLOCK_SIZE_X = 0;

	public static int BLOCK_SIZE_Z = 0;

	public static CHChunk[,] chunk = null;

	public static int[,] preview = new int[128, 128];

	private static Color c;

	private static Color c1;

	public static int GetHeight(int x, int z)
	{
		int num = x / 128;
		int num2 = z / 128;
		if (num < 0)
		{
			return -1;
		}
		if (num2 < 0)
		{
			return -1;
		}
		if (num >= MAP_SIZE_X)
		{
			return -1;
		}
		if (num2 >= MAP_SIZE_Z)
		{
			return -1;
		}
		if (chunk[num, num2] == null)
		{
			LoadChunk(num, num2);
		}
		int num3 = x - num * 128;
		int num4 = z - num2 * 128;
		return chunk[num, num2].height[num3, num4];
	}

	public static int GetFlag(int x, int z)
	{
		int num = x / 128;
		int num2 = z / 128;
		if (num < 0)
		{
			return -1;
		}
		if (num2 < 0)
		{
			return -1;
		}
		if (num >= MAP_SIZE_X)
		{
			return -1;
		}
		if (num2 >= MAP_SIZE_Z)
		{
			return -1;
		}
		if (chunk[num, num2] == null)
		{
			LoadChunk(num, num2);
		}
		int num3 = x - num * 128;
		int num4 = z - num2 * 128;
		return chunk[num, num2].block[num3, num4];
	}

	public static Color GetColor(int x, int z)
	{
		int num = x / 128;
		int num2 = z / 128;
		if (num < 0)
		{
			return Color.red;
		}
		if (num2 < 0)
		{
			return Color.red;
		}
		if (num >= MAP_SIZE_X)
		{
			return Color.red;
		}
		if (num2 >= MAP_SIZE_Z)
		{
			return Color.red;
		}
		if (chunk[num, num2] == null)
		{
			LoadChunk(num, num2);
		}
		int num3 = x - num * 128;
		int num4 = z - num2 * 128;
		return chunk[num, num2].color[num3, num4];
	}

	public static void LoadChunk(int cx, int cz)
	{
		chunk[cx, cz] = new CHChunk(cx, cz);
		int num = 0;
		int num2 = 0;
		for (int i = cx * 128; i < (cx + 1) * 128; i++)
		{
			for (int j = cz * 128; j < (cz + 1) * 128; j++)
			{
				c = tex_terrain.GetPixel(i, j);
				int num3 = (int)(c.r * 512f);
				chunk[cx, cz].height[num, num2] = num3;
				chunk[cx, cz].color[num, num2] = tex_color.GetPixel(i, j);
				chunk[cx, cz].block[num, num2] = 1;
				num2++;
			}
			num++;
			num2 = 0;
		}
	}

	public static void CreatePreview()
	{
	}

	public static void CreateMiniMap()
	{
	}

	public static void LoadTextureMap()
	{
	}

	private static float GetHeightTex(int x, int z, float def)
	{
		if (x < 0)
		{
			return def;
		}
		if (z < 0)
		{
			return def;
		}
		if (x >= BLOCK_SIZE_X)
		{
			return def;
		}
		if (z >= BLOCK_SIZE_Z)
		{
			return def;
		}
		return tex_terrain.GetPixel(x, z).r;
	}

	private static void OnQuit()
	{
		tex_terrain = null;
		tex_color = null;
		Resources.UnloadUnusedAssets();
	}
}
