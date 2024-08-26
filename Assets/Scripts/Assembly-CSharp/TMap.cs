using UnityEngine;

public class TMap : MonoBehaviour
{
	public class CTChunk
	{
		public int x;

		public int z;

		public int[,] block;

		public int[,] height;

		public Color[,] color;

		public GameObject go;

		public MeshFilter mf;

		public MeshRenderer mr;

		public MeshCollider mc;

		public Mesh currMesh;

		public int lodlevel;

		public int direction;

		public CTChunk(int x, int z)
		{
			this.x = x;
			this.z = z;
			block = new int[64, 64];
			height = new int[64, 64];
			color = new Color[64, 64];
			go = null;
			mf = null;
			mc = null;
			currMesh = null;
			stats[0]++;
			lodlevel = 255;
			direction = 255;
		}

		~CTChunk()
		{
			block = null;
			height = null;
			color = null;
			currMesh = null;
			go = null;
			mf = null;
			mc = null;
			stats[0]--;
		}
	}

	public const int MAP_SIZE_X = 2;

	public const int MAP_SIZE_Z = 2;

	public const int CHUNK_SIZE_X = 64;

	public const int CHUNK_SIZE_Z = 64;

	public const int BLOCK_SIZE_X = 128;

	public const int BLOCK_SIZE_Z = 128;

	public const float HEIGHT_STEP = 0.25f;

	public const int SCALE = 1;

	private static int[] lodsize = new int[9] { 1, 2, 4, 8, 16, 32, 64, 128, 256 };

	public static CTChunk[,] chunk = new CTChunk[2, 2];

	public static int[] stats = new int[3];

	public static GameObject goMap = null;

	private static bool[][] dirdata = new bool[8][]
	{
		new bool[4] { true, true, false, false },
		new bool[4] { true, true, true, false },
		new bool[4] { false, true, true, false },
		new bool[4] { false, true, true, true },
		new bool[4] { false, false, true, true },
		new bool[4] { true, false, true, true },
		new bool[4] { true, false, false, true },
		new bool[4] { true, true, false, true }
	};

	public static bool SetBlock(int x, int h, int z, int flag, Color color)
	{
		int num = x / 64;
		int num2 = z / 64;
		if (num >= 2)
		{
			return false;
		}
		if (num2 >= 2)
		{
			return false;
		}
		if (chunk[num, num2] == null)
		{
			chunk[num, num2] = new CTChunk(num, num2);
		}
		int num3 = x - num * 64;
		int num4 = z - num2 * 64;
		chunk[num, num2].block[num3, num4] = flag;
		chunk[num, num2].height[num3, num4] = h;
		chunk[num, num2].color[num3, num4] = color;
		return true;
	}

	public static int GetBlock(int x, int z)
	{
		if (x < 0)
		{
			return -1;
		}
		if (z < 0)
		{
			return -1;
		}
		int num = x / 64;
		int num2 = z / 64;
		if (num >= 2)
		{
			return -1;
		}
		if (num2 >= 2)
		{
			return -1;
		}
		if (chunk[num, num2] == null)
		{
			return -1;
		}
		int num3 = x - num * 64;
		int num4 = z - num2 * 64;
		return chunk[num, num2].block[num3, num4];
	}

	public static int GetBlockHeight(int x, int z)
	{
		if (x < 0)
		{
			return -1;
		}
		if (z < 0)
		{
			return -1;
		}
		int num = x / 64;
		int num2 = z / 64;
		if (num >= 2)
		{
			return -1;
		}
		if (num2 >= 2)
		{
			return -1;
		}
		if (chunk[num, num2] == null)
		{
			return -1;
		}
		int num3 = x - num * 64;
		int num4 = z - num2 * 64;
		if (chunk[num, num2].block[num3, num4] == 0)
		{
			return -1;
		}
		return chunk[num, num2].height[num3, num4];
	}

	public static void FixHeight()
	{
		for (int i = 0; i < 127; i++)
		{
			for (int j = 0; j < 127; j++)
			{
				int blockHeight = GetBlockHeight(i, j);
				int num = GetBlockHeight(i + 1, j);
				int num2 = GetBlockHeight(i, j + 1);
				int num3 = GetBlockHeight(i + 1, j + 1);
				if (blockHeight - num > 1)
				{
					num = blockHeight - 1;
				}
				else if (blockHeight - num < -1)
				{
					num = blockHeight + 1;
				}
				if (blockHeight - num2 > 1)
				{
					num2 = blockHeight - 1;
				}
				else if (blockHeight - num2 < -1)
				{
					num2 = blockHeight + 1;
				}
				if (blockHeight - num3 > 1)
				{
					num3 = blockHeight - 1;
				}
				else if (blockHeight - num3 < -1)
				{
					num3 = blockHeight + 1;
				}
				SetBlock(i + 1, num, j, 1, Color.gray);
				SetBlock(i, num2, j + 1, 1, Color.gray);
				SetBlock(i + 1, num3, j + 1, 1, Color.gray);
			}
		}
	}

	public static void RenderAll(bool dev = false)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				Render(i, j, dev);
			}
		}
	}

	public static void Render(int cx, int cz, bool dev = false)
	{
		if (chunk[cx, cz] != null)
		{
			chunk[cx, cz].go = new GameObject("tchunk_" + cx + "_" + cz, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
			if (goMap == null)
			{
				goMap = GameObject.Find("Map");
			}
			if (goMap != null)
			{
				chunk[cx, cz].go.transform.parent = goMap.transform;
			}
			chunk[cx, cz].go.transform.localPosition = new Vector3(cx * 64, 0f, cz * 64);
			chunk[cx, cz].go.transform.localRotation = Quaternion.identity;
			chunk[cx, cz].go.transform.localScale = Vector3.one;
			chunk[cx, cz].mf = chunk[cx, cz].go.GetComponent<MeshFilter>();
			chunk[cx, cz].mr = chunk[cx, cz].go.GetComponent<MeshRenderer>();
			chunk[cx, cz].mc = chunk[cx, cz].go.GetComponent<MeshCollider>();
			chunk[cx, cz].mr.sharedMaterial = Atlas.mat;
			if (dev)
			{
				CreateLOD(chunk[cx, cz], 0);
			}
		}
	}

	public static void RenderCollider(CTChunk c)
	{
		c.mc.sharedMesh = c.mf.sharedMesh;
	}

	public static void CreateLOD(CTChunk c, int level)
	{
		MeshBuilder.Create();
		int num = lodsize[level];
		for (int i = 0; i < 64; i += num)
		{
			for (int j = 0; j < 64; j += num)
			{
				int flag = c.block[j, i];
				int num4 = c.height[j, i];
				Color c2 = c.color[j, i];
				int num2 = j + c.x * 64;
				int num3 = i + c.z * 64;
				Vector3 localpos = new Vector3(j, 777f, i);
				Vector3 worldpos = new Vector3(num2, 777f, num3);
				PlaneBuilder.Build(flag, localpos, worldpos, c2, num);
			}
		}
		c.currMesh = MeshBuilder.ToMesh();
		c.lodlevel = level;
		c.direction = -1;
		c.mf.sharedMesh = c.currMesh;
		c.mc.sharedMesh = c.mf.sharedMesh;
	}

	public static void CreateLODMerge(CTChunk c, int level, int dir)
	{
		MeshBuilder.Create();
		int num = lodsize[level];
		int num2 = lodsize[level + 1];
		for (int i = 0; i < 64; i += num)
		{
			for (int j = 0; j < 64; j += num)
			{
				int dir2 = -1;
				if (dir == 0 && i + num == 32)
				{
					dir2 = dir;
				}
				else if (dir == 1)
				{
					if (i + num == 32 && j + num == 32)
					{
						dir2 = dir;
					}
					else if (i + num == 32)
					{
						dir2 = 0;
					}
					else if (j + num == 32)
					{
						dir2 = 2;
					}
				}
				else if (dir == 2 && j + num == 32)
				{
					dir2 = dir;
				}
				else if (dir == 3)
				{
					if (i == 32 && j == 32 - num)
					{
						dir2 = dir;
					}
					else if (i == 32)
					{
						dir2 = 4;
					}
					else if (j == 32 - num)
					{
						dir2 = 2;
					}
				}
				else if (dir == 4 && i == 32)
				{
					dir2 = dir;
				}
				else if (dir == 5)
				{
					if (i == 32 && j == 32)
					{
						dir2 = dir;
					}
					else if (i == 32)
					{
						dir2 = 4;
					}
					else if (j == 32)
					{
						dir2 = 6;
					}
				}
				else if (dir == 6 && j == 32)
				{
					dir2 = dir;
				}
				else if (dir == 7)
				{
					if (i + num == 32 && j == 32)
					{
						dir2 = dir;
					}
					else if (i + num == 32)
					{
						dir2 = 0;
					}
					else if (j == 32)
					{
						dir2 = 6;
					}
				}
				int flag = c.block[j, i];
				float num3 = c.height[j, i];
				Color c2 = c.color[j, i];
				int num4 = j + c.x * 64;
				int num5 = i + c.z * 64;
				Vector3 localpos = new Vector3(j, num3 * 0.25f, i);
				Vector3 worldpos = new Vector3(num4, num3 * 0.25f, num5);
				if (!dirdata[dir][0] && i >= 32 && j < 32)
				{
					PlaneBuilder.Build(flag, localpos, worldpos, c2, num, dir2);
				}
				if (!dirdata[dir][1] && i >= 32 && j >= 32)
				{
					PlaneBuilder.Build(flag, localpos, worldpos, c2, num, dir2);
				}
				if (!dirdata[dir][2] && i < 32 && j >= 32)
				{
					PlaneBuilder.Build(flag, localpos, worldpos, c2, num, dir2);
				}
				if (!dirdata[dir][3] && i < 32 && j < 32)
				{
					PlaneBuilder.Build(flag, localpos, worldpos, c2, num, dir2);
				}
				if (num2 == 64 && j == 0 && i == 0)
				{
					PlaneBuilder.Build(flag, localpos, worldpos, c2, num2);
				}
			}
		}
		for (int k = 0; k < 64; k += num2)
		{
			if (num2 == 64)
			{
				break;
			}
			for (int l = 0; l < 64; l += num2)
			{
				int flag2 = c.block[l, k];
				float num6 = c.height[l, k];
				Color c3 = c.color[l, k];
				int num7 = l + c.x * 64;
				int num8 = k + c.z * 64;
				Vector3 localpos2 = new Vector3(l, num6 * 0.25f, k);
				Vector3 worldpos2 = new Vector3(num7, num6 * 0.25f, num8);
				if (dirdata[dir][0] && k >= 32 && l < 32)
				{
					PlaneBuilder.Build(flag2, localpos2, worldpos2, c3, num2);
				}
				if (dirdata[dir][1] && k >= 32 && l >= 32)
				{
					PlaneBuilder.Build(flag2, localpos2, worldpos2, c3, num2);
				}
				if (dirdata[dir][2] && k < 32 && l >= 32)
				{
					PlaneBuilder.Build(flag2, localpos2, worldpos2, c3, num2);
				}
				if (dirdata[dir][3] && k < 32 && l < 32)
				{
					PlaneBuilder.Build(flag2, localpos2, worldpos2, c3, num2);
				}
			}
		}
		c.currMesh = MeshBuilder.ToMesh();
		c.lodlevel = level;
		c.direction = dir;
		c.mf.sharedMesh = c.currMesh;
		c.mc.sharedMesh = c.mf.sharedMesh;
	}

	public static void TestLod(int i)
	{
		for (int j = 0; j < 2; j++)
		{
			for (int k = 0; k < 2; k++)
			{
			}
		}
	}

	public static void LoadData(int cx, int cz)
	{
		for (int i = cx * 64; i < cx * 64 + 64; i++)
		{
			for (int j = cz * 64; j < cz * 64 + 64; j++)
			{
				if (i >= 0 && j >= 0 && i < HMap.BLOCK_SIZE_X && j < HMap.BLOCK_SIZE_Z)
				{
					int height = HMap.GetHeight(i, j);
					int flag = HMap.GetFlag(i, j);
					Color color = HMap.GetColor(i, j);
					SetBlock(i, height, j, flag, color);
				}
			}
		}
	}
}
