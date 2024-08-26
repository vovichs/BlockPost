using System;
using System.Collections.Generic;
using Ionic.Zlib;
using UnityEngine;
using UnityEngine.Rendering;

public class Map : MonoBehaviour
{
	public class CMapEnt
	{
		public GameObject go;

		public int type;

		public int x;

		public int y;

		public int z;

		public int ry;

		public CMapEnt(int x, int y, int z, int ry, int type)
		{
			entindex++;
			this.x = x;
			this.y = y;
			this.z = z;
			this.type = type;
			this.ry = ry;
			if (type != 0)
			{
				go = UnityEngine.Object.Instantiate(goEnt[type - 1]);
				go.name = "mapent_" + entindex;
				go.transform.position = new Vector3(x, y, z);
				GameObject gameObject = GameObject.Find(go.name + "/object");
				if (gameObject != null)
				{
					gameObject.transform.localEulerAngles = new Vector3(0f, ry, 0f);
				}
				else
				{
					go.transform.localEulerAngles = new Vector3(0f, ry, 0f);
				}
			}
		}

		~CMapEnt()
		{
		}
	}

	public class CBlock
	{
		public int x;

		public int y;

		public int z;

		public int block;

		public Color color;

		public CBlock(int x, int y, int z, int block, Color color)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.block = block;
			this.color = color;
		}
	}

	public class CChunk
	{
		public bool dirty;

		public int x;

		public int y;

		public int z;

		public int[,,] block;

		public Color[,,] color;

		public int count;

		public GameObject go;

		public MeshFilter mf;

		public MeshRenderer mr;

		public MeshCollider mc;

		public Vector3 sp;

		public CChunk(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			block = new int[CHUNK_SIZE_X, CHUNK_SIZE_Y, CHUNK_SIZE_Z];
			color = new Color[CHUNK_SIZE_X, CHUNK_SIZE_Y, CHUNK_SIZE_Z];
			go = null;
			mf = null;
			mc = null;
			dirty = false;
			count = 0;
		}

		~CChunk()
		{
			block = null;
			color = null;
		}
	}

	public class CZChunk
	{
		public int x;

		public int y;

		public int z;

		public byte[] data;

		public CZChunk(int x, int y, int z, byte[] data)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.data = data;
		}
	}

	public const int SERVER_CHUNK_SIZE = 16;

	public static int MAP_SIZE_X = 16;

	public static int MAP_SIZE_Y = 4;

	public static int MAP_SIZE_Z = 16;

	public static int CHUNK_SIZE_X = 8;

	public static int CHUNK_SIZE_Y = 8;

	public static int CHUNK_SIZE_Z = 8;

	public static int CHUNK_SIZE_ALL = CHUNK_SIZE_X * CHUNK_SIZE_Y * CHUNK_SIZE_Z;

	public static int BLOCK_SIZE_X = MAP_SIZE_X * CHUNK_SIZE_X;

	public static int BLOCK_SIZE_Y = MAP_SIZE_Y * CHUNK_SIZE_Y;

	public static int BLOCK_SIZE_Z = MAP_SIZE_Z * CHUNK_SIZE_Z;

	public static float WATER_LEVEL = 0f;

	public static CChunk[,,] chunk = null;

	public static byte[] mapdata = null;

	private static int maplen = 0;

	public static GameObject goMap = null;

	public static Transform trMap = null;

	private static int lastFlag;

	private static Color lastColor;

	public static Vector3 sunpos = Vector3.zero;

	public static Vector3[] clip = new Vector3[2];

	public static int entindex = 0;

	public static GameObject[] goEnt = new GameObject[2];

	public static List<CMapEnt> mapent = new List<CMapEnt>();

	public static int avgcolcount = 0;

	public static float avgcolms = 0f;

	public static int avgmeshcount = 0;

	public static float avgmeshms = 0f;

	public static byte[] zs = null;

	public static bool skipplayerclip = false;

	private static int cx;

	private static int cy;

	private static int cz;

	private static GameObject goMapWater = null;

	private static GameObject goMapPlatform = null;

	public static Transform trMapPlatform = null;

	public static void GenerateEntPrefab()
	{
		if (goEnt[0] == null)
		{
			goEnt[0] = new GameObject();
			goEnt[0].name = "prefab_ent_0";
			goEnt[0].transform.position = new Vector3(-2000f, -2000f, -2000f);
			GameObject obj = new GameObject();
			obj.name = "object";
			obj.transform.parent = goEnt[0].transform;
			obj.transform.localPosition = Vector3.zero;
			MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
			MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
			meshRenderer.sharedMaterial = new Material(ContentLoader_.LoadMaterial("vertex_color"));
			meshRenderer.sharedMaterial.SetTexture("_MainTex", null);
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 1, 1);
			BlockBuilder.SetPivot(new Vector3(0.5f, 0f, 0.5f));
			obj.transform.localPosition = new Vector3(0.5f, 0f, 0.5f);
			BlockBuilder.BuildBox(Vector3.zero, 1f, 2.75f, 1f, Color.red);
			BlockBuilder.BuildBox(new Vector3(0.375f, 2.375f, 1f), 0.25f, 0.25f, 0.25f, Color.yellow);
			meshFilter.sharedMesh = MeshBuilder.ToMesh();
		}
		if (goEnt[1] == null)
		{
			goEnt[1] = new GameObject();
			goEnt[1].name = "prefab_ent_1";
			goEnt[1].transform.position = new Vector3(-2000f, -2000f, -2000f);
			GameObject obj2 = new GameObject();
			obj2.name = "object";
			obj2.transform.parent = goEnt[1].transform;
			obj2.transform.localPosition = Vector3.zero;
			MeshRenderer meshRenderer2 = obj2.AddComponent<MeshRenderer>();
			MeshFilter meshFilter2 = obj2.AddComponent<MeshFilter>();
			meshRenderer2.sharedMaterial = new Material(ContentLoader_.LoadMaterial("vertex_color"));
			meshRenderer2.sharedMaterial.SetTexture("_MainTex", null);
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 1, 1);
			BlockBuilder.SetPivot(new Vector3(0.5f, 0f, 0.5f));
			obj2.transform.localPosition = new Vector3(0.5f, 0f, 0.5f);
			BlockBuilder.BuildBox(Vector3.zero, 1f, 2.75f, 1f, Color.blue);
			BlockBuilder.BuildBox(new Vector3(0.375f, 2.375f, 1f), 0.25f, 0.25f, 0.25f, Color.yellow);
			meshFilter2.sharedMesh = MeshBuilder.ToMesh();
		}
	}

	public static void Create(int x, int y, int z)
	{
		if (chunk == null)
		{
			if (goMap == null)
			{
				goMap = GameObject.Find("Map");
				trMap = goMap.transform;
			}
			MAP_SIZE_X = x * 16 / CHUNK_SIZE_X;
			MAP_SIZE_Y = y * 16 / CHUNK_SIZE_Y;
			MAP_SIZE_Z = z * 16 / CHUNK_SIZE_Z;
			BLOCK_SIZE_X = MAP_SIZE_X * CHUNK_SIZE_X;
			BLOCK_SIZE_Y = MAP_SIZE_Y * CHUNK_SIZE_Y;
			BLOCK_SIZE_Z = MAP_SIZE_Z * CHUNK_SIZE_Z;
			chunk = new CChunk[MAP_SIZE_X, MAP_SIZE_Y, MAP_SIZE_Z];
			GenerateEntPrefab();
			mapent.Clear();
			entindex = 0;
			clip[0] = new Vector3(0f, 0f, 0f);
			clip[1] = new Vector3(BLOCK_SIZE_X, BLOCK_SIZE_Y, BLOCK_SIZE_Z);
			GOpt.SetDistance();
		}
	}

	public static void RenderDirty()
	{
		if (chunk == null)
		{
			return;
		}
		for (int i = 0; i < MAP_SIZE_X; i++)
		{
			for (int j = 0; j < MAP_SIZE_Y; j++)
			{
				for (int k = 0; k < MAP_SIZE_Z; k++)
				{
					if (chunk[i, j, k] == null || !chunk[i, j, k].dirty)
					{
						continue;
					}
					if (chunk[i, j, k].count == 0)
					{
						if (chunk[i, j, k].go != null)
						{
							UnityEngine.Object.Destroy(chunk[i, j, k].go);
						}
					}
					else if (chunk[i, j, k].go == null)
					{
						Render(i, j, k);
					}
					else
					{
						if (GUIOptions.mobilegreedy == 1)
						{
							RenderMeshGreedy(chunk[i, j, k]);
						}
						else
						{
							RenderMesh(chunk[i, j, k]);
						}
						RenderCollider(chunk[i, j, k]);
					}
					chunk[i, j, k].dirty = false;
				}
			}
		}
	}

	public static void RenderBlock(int x, int y, int z)
	{
		int num = x / CHUNK_SIZE_X;
		int num2 = y / CHUNK_SIZE_Y;
		int num3 = z / CHUNK_SIZE_Z;
		if (num >= MAP_SIZE_X || num2 >= MAP_SIZE_Y || num3 >= MAP_SIZE_Z || chunk == null || chunk[num, num2, num3] == null || !chunk[num, num2, num3].dirty)
		{
			return;
		}
		if (chunk[num, num2, num3].count == 0)
		{
			if (chunk[num, num2, num3].go != null)
			{
				UnityEngine.Object.Destroy(chunk[num, num2, num3].go);
			}
		}
		else if (chunk[num, num2, num3].go == null)
		{
			Render(num, num2, num3);
		}
		else
		{
			if (GUIOptions.mobilegreedy == 1)
			{
				RenderMeshGreedy(chunk[num, num2, num3]);
			}
			else
			{
				RenderMesh(chunk[num, num2, num3]);
			}
			RenderCollider(chunk[num, num2, num3]);
		}
		chunk[num, num2, num3].dirty = false;
		int num4 = x - num * CHUNK_SIZE_X;
		int num5 = y - num2 * CHUNK_SIZE_Y;
		int num6 = z - num3 * CHUNK_SIZE_Z;
		if (num4 == 0)
		{
			RenderBlockNear(num - 1, num2, num3);
		}
		if (num5 == 0)
		{
			RenderBlockNear(num, num2 - 1, num3);
		}
		if (num6 == 0)
		{
			RenderBlockNear(num, num2, num3 - 1);
		}
		if (num4 == CHUNK_SIZE_X - 1)
		{
			RenderBlockNear(num + 1, num2, num3);
		}
		if (num5 == CHUNK_SIZE_Y - 1)
		{
			RenderBlockNear(num, num2 + 1, num3);
		}
		if (num6 == CHUNK_SIZE_Z - 1)
		{
			RenderBlockNear(num, num2, num3 + 1);
		}
	}

	public static void RenderBlockNear(int cx, int cy, int cz)
	{
		if (cx < 0 || cy < 0 || cz < 0 || cx >= MAP_SIZE_X || cy >= MAP_SIZE_Y || cz >= MAP_SIZE_Z || chunk[cx, cy, cz] == null)
		{
			return;
		}
		if (chunk[cx, cy, cz].count == 0)
		{
			if (chunk[cx, cy, cz].go != null)
			{
				UnityEngine.Object.Destroy(chunk[cx, cy, cz].go);
			}
			return;
		}
		if (chunk[cx, cy, cz].go == null)
		{
			Render(cx, cy, cz);
			return;
		}
		if (GUIOptions.mobilegreedy == 1)
		{
			RenderMeshGreedy(chunk[cx, cy, cz]);
		}
		else
		{
			RenderMesh(chunk[cx, cy, cz]);
		}
		RenderCollider(chunk[cx, cy, cz]);
	}

	public static void SetEnt(int x, int y, int z, int ry, int type)
	{
		mapent.Add(new CMapEnt(x, y, z, ry, type));
	}

	public static int GetEnt(int x, int y, int z)
	{
		for (int i = 0; i < mapent.Count; i++)
		{
			if (mapent[i].x == x && mapent[i].y == y && mapent[i].z == z)
			{
				return i;
			}
		}
		return -1;
	}

	public static void DelEnt(int i)
	{
		if (mapent[i].go != null)
		{
			UnityEngine.Object.Destroy(mapent[i].go);
		}
		mapent.RemoveAt(i);
	}

	public static bool SetBlock(float x, float y, float z, Color c, int flag)
	{
		return SetBlock((int)x, (int)y, (int)z, c, flag);
	}

	public static bool SetBlock(int x, int y, int z, Color c, int flag)
	{
		if (x < 0)
		{
			return false;
		}
		if (y < 0)
		{
			return false;
		}
		if (z < 0)
		{
			return false;
		}
		int num = x / CHUNK_SIZE_X;
		int num2 = y / CHUNK_SIZE_Y;
		int num3 = z / CHUNK_SIZE_Z;
		if (num >= MAP_SIZE_X)
		{
			return false;
		}
		if (num2 >= MAP_SIZE_Y)
		{
			return false;
		}
		if (num3 >= MAP_SIZE_Z)
		{
			return false;
		}
		if (chunk == null)
		{
			return false;
		}
		if (chunk[num, num2, num3] == null)
		{
			chunk[num, num2, num3] = new CChunk(num, num2, num3);
		}
		int num4 = x - num * CHUNK_SIZE_X;
		int num5 = y - num2 * CHUNK_SIZE_Y;
		int num6 = z - num3 * CHUNK_SIZE_Z;
		chunk[num, num2, num3].block[num4, num5, num6] = flag;
		chunk[num, num2, num3].color[num4, num5, num6] = c;
		chunk[num, num2, num3].dirty = true;
		if (flag == 0)
		{
			chunk[num, num2, num3].count--;
		}
		else
		{
			chunk[num, num2, num3].count++;
		}
		return true;
	}

	public static void SetBlockNearDirtyUpdate(int x, int y, int z)
	{
		int num = x / CHUNK_SIZE_X;
		int num2 = y / CHUNK_SIZE_Y;
		int num3 = z / CHUNK_SIZE_Z;
		if (num < MAP_SIZE_X && num2 < MAP_SIZE_Y && num3 < MAP_SIZE_Z)
		{
			int num4 = x - num * CHUNK_SIZE_X;
			int num5 = y - num2 * CHUNK_SIZE_Y;
			int num6 = z - num3 * CHUNK_SIZE_Z;
			if (num4 == 0)
			{
				SetDirty(num - 1, num2, num3);
			}
			if (num5 == 0)
			{
				SetDirty(num, num2 - 1, num3);
			}
			if (num6 == 0)
			{
				SetDirty(num, num2, num3 - 1);
			}
			if (num4 == CHUNK_SIZE_X - 1)
			{
				SetDirty(num + 1, num2, num3);
			}
			if (num5 == CHUNK_SIZE_Y - 1)
			{
				SetDirty(num, num2 + 1, num3);
			}
			if (num6 == CHUNK_SIZE_Z - 1)
			{
				SetDirty(num, num2, num3 + 1);
			}
		}
	}

	private static void SetDirty(int cx, int cy, int cz)
	{
		if (cx >= 0 && cy >= 0 && cz >= 0 && cx < MAP_SIZE_X && cy < MAP_SIZE_Y && cz < MAP_SIZE_Z && chunk != null && chunk[cx, cy, cz] != null)
		{
			chunk[cx, cy, cz].dirty = true;
		}
	}

	public static int GetBlock(Vector3 pos)
	{
		return GetBlock((int)pos.x, (int)pos.y, (int)pos.z);
	}

	public static int GetBlock(int x, int y, int z)
	{
		if (x < 0)
		{
			return -1;
		}
		if (y < 0)
		{
			return -1;
		}
		if (z < 0)
		{
			return -1;
		}
		cx = x / CHUNK_SIZE_X;
		cy = y / CHUNK_SIZE_Y;
		cz = z / CHUNK_SIZE_Z;
		if (cx >= MAP_SIZE_X)
		{
			return -1;
		}
		if (cy >= MAP_SIZE_Y)
		{
			return -1;
		}
		if (cz >= MAP_SIZE_Z)
		{
			return -1;
		}
		if (chunk == null)
		{
			return 0;
		}
		if (chunk[cx, cy, cz] == null)
		{
			return 0;
		}
		int num = x - cx * CHUNK_SIZE_X;
		int num2 = y - cy * CHUNK_SIZE_Y;
		int num3 = z - cz * CHUNK_SIZE_Z;
		lastFlag = chunk[cx, cy, cz].block[num, num2, num3];
		lastColor = chunk[cx, cy, cz].color[num, num2, num3];
		return lastFlag;
	}

	public static Color GetLastColor()
	{
		return lastColor;
	}

	public static Color GetLastColorFixed()
	{
		if (lastFlag == 1)
		{
			return new Color(0.56078434f, 0.8235294f, 29f / 85f, 1f);
		}
		if (lastFlag == 2)
		{
			return new Color(0.24f, 0.54f, 0f, 1f);
		}
		if (lastFlag == 3)
		{
			return new Color(0.73f, 0.84f, 0f, 1f);
		}
		if (lastFlag == 4)
		{
			return new Color(0.89f, 0.97f, 1f, 1f);
		}
		if (lastFlag == 9)
		{
			return new Color(32f / 51f, 20f / 51f, 0f, 1f);
		}
		if (lastFlag == 5 || lastFlag == 6 || lastFlag == 7)
		{
			return Color.gray;
		}
		return lastColor;
	}

	public static void Render(int cx, int cy, int cz)
	{
		if (chunk[cx, cy, cz] != null)
		{
			chunk[cx, cy, cz].go = new GameObject("chunk_" + cx + "_" + cy + "_" + cz, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
			if (goMap == null)
			{
				goMap = GameObject.Find("Map");
			}
			if (goMap != null)
			{
				chunk[cx, cy, cz].go.transform.parent = goMap.transform;
			}
			chunk[cx, cy, cz].go.transform.localPosition = new Vector3(cx * CHUNK_SIZE_X, cy * CHUNK_SIZE_Y, cz * CHUNK_SIZE_Z) * BlockBuilder.SCALE;
			chunk[cx, cy, cz].go.transform.localRotation = Quaternion.identity;
			chunk[cx, cy, cz].go.transform.localScale = Vector3.one;
			chunk[cx, cy, cz].mf = chunk[cx, cy, cz].go.GetComponent<MeshFilter>();
			chunk[cx, cy, cz].mr = chunk[cx, cy, cz].go.GetComponent<MeshRenderer>();
			chunk[cx, cy, cz].mc = chunk[cx, cy, cz].go.GetComponent<MeshCollider>();
			chunk[cx, cy, cz].mr.sharedMaterial = Atlas.mat;
			if (GUIOptions.mobilegreedy == 1)
			{
				RenderMeshGreedy(chunk[cx, cy, cz]);
			}
			else
			{
				RenderMesh(chunk[cx, cy, cz]);
			}
			RenderCollider(chunk[cx, cy, cz]);
			if (Controll.altrender)
			{
				UnityEngine.Object.Destroy(chunk[cx, cy, cz].mr);
			}
		}
	}

	public static void RenderMeshGreedy(CChunk c)
	{
		c.mf.sharedMesh = Greedy.Build(c);
		if (MeshBuilder.GetVerticesCount() == 0)
		{
			if (MeshBuilder.mesh != null)
			{
				UnityEngine.Object.Destroy(MeshBuilder.mesh);
			}
			UnityEngine.Object.Destroy(c.go);
		}
	}

	public static void RenderMesh(CChunk c)
	{
		MeshBuilder.Create();
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		for (int i = 0; i < CHUNK_SIZE_Z; i++)
		{
			for (int j = 0; j < CHUNK_SIZE_Y; j++)
			{
				for (int k = 0; k < CHUNK_SIZE_X; k++)
				{
					int num = c.block[k, j, i];
					if (num != 0)
					{
						int num2 = k + c.x * CHUNK_SIZE_X;
						int num3 = j + c.y * CHUNK_SIZE_Y;
						int num4 = i + c.z * CHUNK_SIZE_Z;
						zero.Set(k, j, i);
						zero2.Set(num2, num3, num4);
						BlockBuilder.Build(num, zero, zero2, c.color[k, j, i]);
					}
				}
			}
		}
		if (MeshBuilder.GetVerticesCount() == 0)
		{
			if (MeshBuilder.mesh != null)
			{
				UnityEngine.Object.Destroy(MeshBuilder.mesh);
			}
			UnityEngine.Object.Destroy(c.go);
		}
		else
		{
			c.mf.sharedMesh = MeshBuilder.ToMesh();
		}
	}

	public static void RenderCollider(CChunk c)
	{
		if (c.go != null)
		{
			c.mc.sharedMesh = c.mf.sharedMesh;
		}
	}

	public static void RenderAll()
	{
		for (int i = 0; i < MAP_SIZE_X; i++)
		{
			for (int j = 0; j < MAP_SIZE_Y; j++)
			{
				for (int k = 0; k < MAP_SIZE_Z; k++)
				{
					Render(i, j, k);
					if (chunk[i, j, k] != null)
					{
						chunk[i, j, k].dirty = false;
					}
				}
			}
		}
	}

	public static void SaveBin(string mapname)
	{
		Console.Log("[ > ] SaveMap " + mapname);
		mapdata = null;
		maplen = 0;
		int mapsizex = BLOCK_SIZE_X / 16;
		int mapsizey = BLOCK_SIZE_Y / 16;
		int mapsizez = BLOCK_SIZE_Z / 16;
		int c = 0;
		List<CZChunk> list = new List<CZChunk>();
		maplen = BuildChunks(ref list, ref c, mapsizex, mapsizey, mapsizez);
		Console.Log("chunkcount " + c + " maplen " + maplen);
		byte[] headerdata = new byte[256];
		int num = 0;
		num = BuildHeader(ref headerdata, mapent.Count, sunpos, c, mapsizex, mapsizey, mapsizez);
		byte[] entzip = null;
		BuildEntData(ref mapent, ref entzip);
		mapdata = CopyData(maplen, c, headerdata, num, list, entzip);
		entzip = null;
		string text = Convert.ToBase64String(mapdata);
		Application.ExternalCall("savefilemap", text);
		mapdata = null;
		maplen = 0;
		headerdata = null;
		num = 0;
		Console.Log("save map " + mapname + ".map complete");
	}

	public static int BuildHeader(ref byte[] headerdata, int entcount, Vector3 sunpos, int c, int mapsizex, int mapsizey, int mapsizez)
	{
		NET._BEGIN_WRITE(headerdata, 0);
		NET._WRITE_BYTE(0);
		NET._WRITE_LONG(0);
		NET._WRITE_BYTE((byte)mapsizex);
		NET._WRITE_BYTE((byte)mapsizey);
		NET._WRITE_BYTE((byte)mapsizez);
		NET._WRITE_BYTE((byte)mapsizex);
		NET._WRITE_BYTE((byte)mapsizey);
		NET._WRITE_BYTE((byte)mapsizez);
		NET._WRITE_LONG(c);
		NET._WRITE_LONG(entcount);
		NET._WRITE_BYTE(0);
		NET._WRITE_FLOAT(sunpos.x);
		NET._WRITE_FLOAT(sunpos.y);
		NET._WRITE_BYTE(0);
		NET._WRITE_BYTE(0);
		NET._WRITE_BYTE(0);
		NET._WRITE_BYTE(0);
		NET._WRITE_BYTE(0);
		NET._WRITE_BYTE(0);
		int num = NET._WRITE_LEN();
		Console.Log("header len = " + num);
		return num;
	}

	public static int BuildEntData(ref List<CMapEnt> entlist, ref byte[] entzip)
	{
		byte[] array = new byte[mapent.Count * 8];
		NET._BEGIN_WRITE(array, 0);
		for (int i = 0; i < mapent.Count; i++)
		{
			NET._WRITE_BYTE((byte)mapent[i].type);
			NET._WRITE_SHORT((short)mapent[i].x);
			NET._WRITE_SHORT((short)mapent[i].y);
			NET._WRITE_SHORT((short)mapent[i].z);
			NET._WRITE_BYTE((byte)((float)mapent[i].ry / 90f));
		}
		entzip = GZipStream.CompressBuffer(array);
		if (entzip != null)
		{
			return entzip.Length;
		}
		return 0;
	}

	public static int BuildChunks(ref List<CZChunk> zs, ref int c, int mapsizex, int mapsizey, int mapsizez)
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
						zs.Add(new CZChunk(i, j, k, array));
						c++;
						num += array.Length;
					}
				}
			}
		}
		return num;
	}

	public static byte[] CopyData(int maplen, int chunkcount, byte[] headerdata, int headerlen, List<CZChunk> zs, byte[] entzip)
	{
		int num = 4;
		int num2 = 5;
		byte[] array = new byte[maplen + num + num2 * chunkcount + headerlen + ((entzip != null) ? entzip.Length : 0) + 2];
		NET._BEGIN_WRITE(array, 0);
		NET._WRITE_BYTE(10);
		NET._WRITE_BYTE(11);
		NET._WRITE_SHORT((short)headerlen);
		for (int i = 0; i < headerlen; i++)
		{
			NET._WRITE_BYTE(headerdata[i]);
		}
		for (int j = 0; j < zs.Count; j++)
		{
			int num3 = zs[j].data.Length;
			NET._WRITE_BYTE((byte)zs[j].x);
			NET._WRITE_BYTE((byte)zs[j].y);
			NET._WRITE_BYTE((byte)zs[j].z);
			NET._WRITE_SHORT((short)num3);
			for (int k = 0; k < num3; k++)
			{
				NET._WRITE_BYTE(zs[j].data[k]);
			}
		}
		if (entzip == null || entzip.Length == 0)
		{
			NET._WRITE_SHORT(0);
		}
		else
		{
			NET._WRITE_SHORT((short)((entzip != null) ? entzip.Length : 0));
			for (int l = 0; l < ((entzip != null) ? entzip.Length : 0); l++)
			{
				NET._WRITE_BYTE(entzip[l]);
			}
		}
		return array;
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
					int block = GetBlock(i, k, j);
					int color = Palette.GetColor(GetLastColor());
					NET.WRITE_BYTE((byte)block);
					NET.WRITE_BYTE((byte)color);
					if (block > 0)
					{
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

	public static void LoadBin(string mapname, byte[] mapdata = null, int maplen = 0)
	{
		if (maplen == 0)
		{
			Debug.Log("LoadBin.ERROR maplen==0");
			return;
		}
		if (maplen < 4)
		{
			Debug.Log("LoadBin.ERROR maplen==0");
			return;
		}
		Clear();
		NET.BEGIN_READ(mapdata, maplen, 0);
		if (NET.READ_BYTE() != 10 || NET.READ_BYTE() != 11)
		{
			return;
		}
		int num = NET.READ_SHORT();
		if (maplen < num + 4)
		{
			return;
		}
		NET.READ_BYTE();
		NET.READ_LONG();
		int num2 = NET.READ_BYTE();
		int num3 = NET.READ_BYTE();
		int num4 = NET.READ_BYTE();
		NET.READ_BYTE();
		NET.READ_BYTE();
		NET.READ_BYTE();
		int num5 = NET.READ_LONG();
		int num6 = NET.READ_LONG();
		NET.READ_BYTE();
		float num7 = NET.READ_FLOAT();
		float num8 = NET.READ_FLOAT();
		NET.READ_BYTE();
		NET.READ_BYTE();
		NET.READ_BYTE();
		NET.READ_BYTE();
		NET.READ_BYTE();
		NET.READ_BYTE();
		Console.Log("[ > ] loading map " + mapname);
		Console.Log("mapsize " + num2 + " " + num3 + " " + num4);
		Console.Log("chunkcount " + num5);
		for (int i = NET.READ_POS(); i < 4 + num; i++)
		{
			NET.READ_BYTE();
		}
		Create(num2, num3, num4);
		for (int j = 0; j < num5; j++)
		{
			int num9 = NET.READ_BYTE();
			int num10 = NET.READ_BYTE();
			int num11 = NET.READ_BYTE();
			int num12 = NET.READ_SHORT();
			zs = new byte[num12];
			for (int k = 0; k < num12; k++)
			{
				zs[k] = (byte)NET.READ_BYTE();
			}
			UncompressAndSetBlock(num9 * 16, num10 * 16, num11 * 16);
		}
		NET.READ_POS();
		int num13 = NET.READ_SHORT();
		if (num13 > 0)
		{
			byte[] array = new byte[num13];
			for (int l = 0; l < num13; l++)
			{
				array[l] = (byte)NET.READ_BYTE();
			}
			byte[] array2 = GZipStream.UncompressBuffer(array);
			NET.BEGIN_READ(array2, array2.Length, 0);
			for (int m = 0; m < num6; m++)
			{
				int type = NET.READ_BYTE();
				int x = NET.READ_SHORT();
				int y = NET.READ_SHORT();
				int z = NET.READ_SHORT();
				int ry = NET.READ_BYTE() * 90;
				SetEnt(x, y, z, ry, type);
			}
			array = null;
		}
		mapdata = null;
		maplen = 0;
		RenderAll();
		Console.cs.Command("sun " + num7 + " " + num8);
		LoadBackground();
		Controll.pl.team = 0;
	}

	public static void LoadBackground()
	{
		WATER_LEVEL = 0f;
		switch (GetBlock(0, 0, 0))
		{
		case 40:
			WATER_LEVEL = 0.8f;
			break;
		case 41:
			WATER_LEVEL = -1f;
			break;
		}
		GenerateBackground(WATER_LEVEL, GetLastColorFixed());
	}

	public static void UncompressAndSetBlock(int ox, int oy, int oz, bool netversion = false)
	{
		byte[] array = GZipStream.UncompressBuffer(zs);
		int num = 0;
		if (netversion)
		{
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					for (int k = 0; k < 16; k++)
					{
						int num2 = array[num];
						int c = array[num + 4096];
						num++;
						if ((!skipplayerclip || num2 != 52) && num2 > 0)
						{
							SetBlock(ox + i, oy + j, oz + k, Palette.GetColor(c), num2);
						}
					}
				}
			}
			return;
		}
		for (int l = 0; l < 16; l++)
		{
			for (int m = 0; m < 16; m++)
			{
				for (int n = 0; n < 16; n++)
				{
					int num2 = array[num];
					int c = array[num + 1];
					num += 2;
					if (num2 > 0)
					{
						SetBlock(ox + l, oy + n, oz + m, Palette.GetColor(c), num2);
					}
				}
			}
		}
	}

	public static void UncompressAndSetBlockCustom(int ox, int oy, int oz)
	{
		byte[] array = new byte[8192];
		int num = 0;
		int num2 = 0;
		byte b = 0;
		int num3 = 0;
		while (num < zs.Length)
		{
			if (zs[num] == byte.MaxValue && num + 2 < zs.Length)
			{
				b = zs[num + 1];
				for (num3 = zs[num + 2]; num3 > 0; num3--)
				{
					array[num2] = b;
					num2++;
				}
				num += 3;
			}
			else
			{
				array[num2] = zs[num];
				num++;
				num2++;
			}
		}
		int num4 = 0;
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					int num5 = array[num4];
					int c = array[num4 + 4096];
					num4++;
					if (num5 > 0)
					{
						SetBlock(ox + i, oy + j, oz + k, Palette.GetColor(c), num5);
					}
				}
			}
		}
	}

	public static void SaveText()
	{
	}

	public static void Clear()
	{
		if (goMapWater != null)
		{
			UnityEngine.Object.Destroy(goMapWater);
		}
		if (goMapPlatform != null)
		{
			UnityEngine.Object.Destroy(goMapPlatform);
		}
		if (chunk == null)
		{
			return;
		}
		for (int i = 0; i < MAP_SIZE_X; i++)
		{
			for (int j = 0; j < MAP_SIZE_Y; j++)
			{
				for (int k = 0; k < MAP_SIZE_Z; k++)
				{
					if (chunk[i, j, k] != null)
					{
						UnityEngine.Object.Destroy(chunk[i, j, k].go);
						chunk[i, j, k] = null;
					}
				}
			}
		}
		chunk = null;
		for (int l = 0; l < mapent.Count; l++)
		{
			if (!(mapent[l].go == null))
			{
				UnityEngine.Object.Destroy(mapent[l].go);
			}
		}
		mapent.Clear();
		entindex = 0;
		MapLight.ClearData();
		MapEvent.Clear();
	}

	private void LoadEnd()
	{
		Atlas.Init();
	}

	public static void GenerateBackground(float y, Color c)
	{
		if (goMapWater != null)
		{
			UnityEngine.Object.Destroy(goMapWater);
		}
		goMapWater = new GameObject();
		goMapWater.name = "MapBackWater";
		goMapWater.transform.localPosition = new Vector3(0f, y, 0f);
		goMapWater.transform.localEulerAngles = Vector3.zero;
		MeshFilter meshFilter = goMapWater.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = goMapWater.AddComponent<MeshRenderer>();
		meshRenderer.material = new Material(ContentLoader_.LoadMaterial("standard"));
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.material.SetTexture("_MainTex", ContentLoader_.LoadTexture("mapback") as Texture2D);
		meshRenderer.material.SetColor("_Color", c);
		meshRenderer.material.SetFloat("_Metallic", 0.5f);
		meshRenderer.material.SetFloat("_Glossiness", 0f);
		MeshBuilder.Create();
		float num = 640f;
		BlockBuilder.SetPivot(new Vector3((num - (float)BLOCK_SIZE_X) / 2f, 0f, (num - (float)BLOCK_SIZE_Z) / 2f));
		BlockBuilder.SetTexCoord(0, 1, 1);
		BlockBuilder.BuildFaceBlock(1, 4, new Vector3(0f, 0f, 0f), c, num, 0f, num);
		meshFilter.mesh = MeshBuilder.ToMesh();
		if (goMapPlatform != null)
		{
			UnityEngine.Object.Destroy(goMapPlatform);
		}
		goMapPlatform = UnityEngine.Object.Instantiate(goMapWater);
		goMapPlatform.name = "MapBackPlatform";
		meshRenderer = goMapPlatform.GetComponent<MeshRenderer>();
		if (y < 0f)
		{
			Texture value = Resources.Load("groundblock") as Texture;
			meshRenderer.material.SetColor("_Color", c);
			meshRenderer.material.SetTexture("_MainTex", value);
			meshRenderer.material.SetTextureScale("_MainTex", new Vector2(640f, 640f));
		}
		else
		{
			meshRenderer.material.SetColor("_Color", Color.gray);
		}
		goMapPlatform.transform.localPosition = Vector3.zero;
		goMapPlatform.AddComponent<BoxCollider>();
		trMapPlatform = goMapPlatform.transform;
		if (y <= 0f)
		{
			UnityEngine.Object.Destroy(goMapWater);
		}
	}
}
