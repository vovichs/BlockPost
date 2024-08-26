using System;
using System.Collections.Generic;
using System.IO;
using GameClass;
using Ionic.Zlib;
using UnityEngine;

public class MapPrefab : MonoBehaviour
{
	public static int MAP_SIZE_X = 16;

	public static int MAP_SIZE_Y = 4;

	public static int MAP_SIZE_Z = 16;

	public static int BLOCK_SIZE_X = MAP_SIZE_X * Map.CHUNK_SIZE_X;

	public static int BLOCK_SIZE_Y = MAP_SIZE_Y * Map.CHUNK_SIZE_Y;

	public static int BLOCK_SIZE_Z = MAP_SIZE_Z * Map.CHUNK_SIZE_Z;

	public static List<Map.CBlock> data = null;

	public static List<Map.CMapEnt> mapent = new List<Map.CMapEnt>();

	public static List<Prefab> p = new List<Prefab>();

	public static void Create(List<Map.CBlock> b)
	{
		data = new List<Map.CBlock>();
		int num = 9999;
		int num2 = 9999;
		int num3 = 9999;
		for (int i = 0; i < b.Count; i++)
		{
			if (b[i].x < num)
			{
				num = b[i].x;
			}
			if (b[i].y < num2)
			{
				num2 = b[i].y;
			}
			if (b[i].z < num3)
			{
				num3 = b[i].z;
			}
		}
		for (int j = 0; j < b.Count; j++)
		{
			data.Add(new Map.CBlock(b[j].x - num, b[j].y - num2, b[j].z - num3, b[j].block, b[j].color));
		}
	}

	public static void SaveBin(string prefabname)
	{
		if (data != null)
		{
			Console.Log("[ > ] SavePrefab " + prefabname);
			int num = 0;
			int mapsizex = BLOCK_SIZE_X / 16;
			int mapsizey = BLOCK_SIZE_Y / 16;
			int mapsizez = BLOCK_SIZE_Z / 16;
			int c = 0;
			List<Map.CZChunk> zs = new List<Map.CZChunk>();
			num = BuildChunks(ref zs, ref c, mapsizex, mapsizey, mapsizez);
			Console.Log("chunkcount " + c + " maplen " + num);
			byte[] headerdata = new byte[256];
			int num2 = 0;
			num2 = Map.BuildHeader(ref headerdata, mapent.Count, Vector3.zero, c, mapsizex, mapsizey, mapsizez);
			byte[] entzip = null;
			Map.BuildEntData(ref mapent, ref entzip);
			Map.CopyData(num, c, headerdata, num2, zs, entzip);
		}
	}

	public static int BuildChunks(ref List<Map.CZChunk> zs, ref int c, int mapsizex, int mapsizey, int mapsizez)
	{
		int num = 0;
		for (int i = 0; i < mapsizex; i++)
		{
			for (int j = 0; j < mapsizey; j++)
			{
				for (int k = 0; k < mapsizez; k++)
				{
					byte[] array = PackChunk(i, j, k);
					if (array != null)
					{
						zs.Add(new Map.CZChunk(i, j, k, array));
						c++;
						num += array.Length;
					}
				}
			}
		}
		return num;
	}

	public static byte[] PackChunk(int cx, int cy, int cz)
	{
		bool flag = true;
		NET.BEGIN_WRITE();
		for (int i = cx * 16; i < (cx + 1) * 16; i++)
		{
			for (int j = cz * 16; j < (cz + 1) * 16; j++)
			{
				for (int k = cy * 16; k < (cy + 1) * 16; k++)
				{
					Map.CBlock block = GetBlock(i, k, j);
					if (block == null)
					{
						NET.WRITE_BYTE(0);
						NET.WRITE_BYTE(0);
					}
					else
					{
						NET.WRITE_BYTE((byte)block.block);
						NET.WRITE_BYTE((byte)Palette.GetColor(block.color));
						flag = false;
					}
				}
			}
		}
		if (flag)
		{
			return null;
		}
		byte[] array = new byte[NET.WRITE_LEN()];
		Array.Copy(NET.WRITE_DATA(), array, NET.WRITE_LEN());
		byte[] result = GZipStream.CompressBuffer(array);
		array = null;
		return result;
	}

	private static Map.CBlock GetBlock(int x, int y, int z)
	{
		for (int i = 0; i < data.Count; i++)
		{
			if (data[i].x == x && data[i].y == y && data[i].z == z)
			{
				return data[i];
			}
		}
		return null;
	}

	public static void bin_prefab_load()
	{
		if (!Directory.Exists("Prefabs"))
		{
			Directory.CreateDirectory("Prefabs");
		}
		string[] files = Directory.GetFiles("Prefabs/", "*.bpobj");
		p.Clear();
		for (int i = 0; i < files.Length; i++)
		{
			string[] array = files[i].Split('/');
			string[] array2 = array[array.Length - 1].Split('.');
			p.Add(new Prefab(array2[0], files[i]));
		}
		Log.Add("prefab count : " + files.Length);
	}

	public static void LoadBin(string prefabname, ref List<Map.CBlock> b)
	{
	}

	public static void Rotate(ref List<Map.CBlock> b, bool forward)
	{
		List<Map.CBlock> list = new List<Map.CBlock>();
		float num = 90 * (forward ? 1 : (-1));
		float f = (float)Math.PI / 180f * num;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < b.Count; j++)
			{
				float f2 = (float)b[j].x * Mathf.Cos(f) + (float)b[j].z * Mathf.Sin(f);
				float f3 = b[j].y;
				float f4 = (float)b[j].z * Mathf.Cos(f) - (float)b[j].x * Mathf.Sin(f);
				if (i == 0)
				{
					Vector3 vector = new Vector3(Mathf.Round(f2), Mathf.Round(f3), Mathf.Round(f4));
					if (vector.x < zero.x)
					{
						zero.x = vector.x;
					}
					if (vector.z < zero.z)
					{
						zero.z = vector.z;
					}
				}
				else
				{
					Vector3 vector = new Vector3(Mathf.Round(f2) - zero.x, Mathf.Round(f3), Mathf.Round(f4) - zero.z);
					list.Add(new Map.CBlock((int)vector.x, (int)vector.y, (int)vector.z, b[j].block, b[j].color));
				}
			}
		}
		b.Clear();
		b = list;
	}
}
