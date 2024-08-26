using UnityEngine;

public class VMap : MonoBehaviour
{
	public class CVChunk
	{
		public int x;

		public int z;

		public int[,] block;

		public int[,] height;

		public GameObject go;

		public MeshFilter mf;

		public MeshRenderer mr;

		public MeshCollider mc;

		public CVChunk(int x, int z)
		{
			this.x = x;
			this.z = z;
			block = new int[256, 256];
			height = new int[256, 256];
			go = null;
			mf = null;
			mc = null;
		}

		~CVChunk()
		{
			block = null;
		}
	}

	public const int MAP_SIZE_X = 32;

	public const int MAP_SIZE_Z = 32;

	public const int CHUNK_SIZE_X = 256;

	public const int CHUNK_SIZE_Z = 256;

	public const int BLOCK_SIZE_X = 8192;

	public const int BLOCK_SIZE_Z = 8192;

	public static CVChunk[,] chunk = new CVChunk[32, 32];

	private static GameObject goMap = null;

	public static bool SetBlock(int x, int h, int z, int flag)
	{
		int num = x / 256;
		int num2 = z / 256;
		if (num >= 32)
		{
			return false;
		}
		if (num2 >= 32)
		{
			return false;
		}
		if (chunk[num, num2] == null)
		{
			chunk[num, num2] = new CVChunk(num, num2);
		}
		int num3 = x - num * 256;
		int num4 = z - num2 * 256;
		chunk[num, num2].height[num3, num4] = h;
		chunk[num, num2].block[num3, num4] = flag;
		return true;
	}

	public static int GetBlock(Vector3 pos)
	{
		return GetBlock((int)pos.x, (int)pos.z);
	}

	public static int GetBlock(int x, int z)
	{
		int num = x / 256;
		int num2 = z / 256;
		if (num >= 32)
		{
			return 0;
		}
		if (num2 >= 32)
		{
			return 0;
		}
		if (chunk[num, num2] == null)
		{
			return 0;
		}
		int num3 = x - num * 256;
		int num4 = z - num2 * 256;
		return chunk[num, num2].block[num3, num4];
	}

	public static void Render(int cx, int cz)
	{
		if (chunk[cx, cz] != null)
		{
			chunk[cx, cz].go = new GameObject("vchunk_" + cx + "_" + cz, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
			if (goMap == null)
			{
				goMap = GameObject.Find("Map");
			}
			if (goMap != null)
			{
				chunk[cx, cz].go.transform.parent = goMap.transform;
			}
			chunk[cx, cz].go.transform.localPosition = new Vector3(cx * 256, 0f, cz * 256);
			chunk[cx, cz].go.transform.localRotation = Quaternion.identity;
			chunk[cx, cz].go.transform.localScale = Vector3.one;
			chunk[cx, cz].mf = chunk[cx, cz].go.GetComponent<MeshFilter>();
			chunk[cx, cz].mr = chunk[cx, cz].go.GetComponent<MeshRenderer>();
			chunk[cx, cz].mr.sharedMaterial = Atlas.grass;
			RenderMesh(chunk[cx, cz]);
		}
	}

	public static void RenderMesh(CVChunk c)
	{
		MeshBuilder.Create();
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 256; j++)
			{
				int num = c.block[j, i];
				if (num != 0)
				{
					float y = c.height[j, i];
					int num2 = j + c.x * 256;
					int num3 = i + c.z * 256;
					Vector3 localpos = new Vector3(j, y, i);
					Vector3 worldpos = new Vector3(num2, y, num3);
					BlockBuilder.BuildGrass(num, localpos, worldpos);
				}
			}
		}
		c.mf.sharedMesh = MeshBuilder.ToMesh();
	}

	public static void RenderAll()
	{
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				Render(i, j);
			}
		}
	}
}
