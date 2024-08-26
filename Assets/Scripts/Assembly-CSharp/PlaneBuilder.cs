using UnityEngine;

public class PlaneBuilder
{
	public static Vector3[] directions = new Vector3[6]
	{
		Vector3.forward,
		Vector3.back,
		Vector3.right,
		Vector3.left,
		Vector3.up,
		Vector3.down
	};

	public static Vector3[][] normals = new Vector3[8][]
	{
		new Vector3[4]
		{
			Vector3.forward,
			Vector3.forward,
			Vector3.forward,
			Vector3.forward
		},
		new Vector3[4]
		{
			Vector3.back,
			Vector3.back,
			Vector3.back,
			Vector3.back
		},
		new Vector3[4]
		{
			Vector3.right,
			Vector3.right,
			Vector3.right,
			Vector3.right
		},
		new Vector3[4]
		{
			Vector3.left,
			Vector3.left,
			Vector3.left,
			Vector3.left
		},
		new Vector3[4]
		{
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.up
		},
		new Vector3[4]
		{
			Vector3.down,
			Vector3.down,
			Vector3.down,
			Vector3.down
		},
		new Vector3[4]
		{
			new Vector3(-0.5f, 0f, 0.5f),
			new Vector3(-0.5f, 0f, 0.5f),
			new Vector3(-0.5f, 0f, 0.5f),
			new Vector3(-0.5f, 0f, 0.5f)
		},
		new Vector3[4]
		{
			new Vector3(-0.5f, 0f, -0.5f),
			new Vector3(-0.5f, 0f, -0.5f),
			new Vector3(-0.5f, 0f, -0.5f),
			new Vector3(-0.5f, 0f, -0.5f)
		}
	};

	private static Vector3[] verts = null;

	private static bool hardedge = false;

	private static int scale = 1;

	public static void SetHardEdge(bool val)
	{
		hardedge = val;
	}

	public static void Build(int flag, Vector3 localpos, Vector3 worldpos, Color c, int step, int dir = -1)
	{
		float num = TMap.GetBlockHeight((int)worldpos.x, (int)worldpos.z);
		float num2 = TMap.GetBlockHeight((int)worldpos.x + step, (int)worldpos.z);
		float num3 = TMap.GetBlockHeight((int)worldpos.x, (int)worldpos.z + step);
		float num4 = TMap.GetBlockHeight((int)worldpos.x + step, (int)worldpos.z + step);
		float num5 = scale * step;
		verts = new Vector3[4]
		{
			new Vector3(num5 + localpos.x, 0f + num2 * 0.25f, 0f + localpos.z),
			new Vector3(num5 + localpos.x, 0f + num4 * 0.25f, num5 + localpos.z),
			new Vector3(0f + localpos.x, 0f + num3 * 0.25f, num5 + localpos.z),
			new Vector3(0f + localpos.x, 0f + num * 0.25f, 0f + localpos.z)
		};
		MeshBuilder.AddFaceIndices(0);
		MeshBuilder.AddVertices(verts, Vector3.zero);
		MeshBuilder.AddNormals(normals[4]);
		MeshBuilder.AddTexCoords(Atlas.GetUV(flag));
		MeshBuilder.AddFaceColor(c);
	}

	public static void GetOffset(Vector3 worldpos, Vector3 localpos, int size, int dir, ref float h, ref float hx, ref float hz, ref float hd, ref float vh, ref float vhx, ref float vhz, ref float vhd)
	{
		float min = -0.15f;
		float max = 0.15f;
		float num = 0f;
		float num2 = 0f;
		int num3 = (int)localpos.x;
		int num4 = (int)localpos.z;
		bool flag = false;
		if (size == 1 && num3 % 2 == 0)
		{
			flag = true;
		}
		else if (size == 2 && num3 != 2 && num3 != 6 && num3 != 10 && num3 != 14 && num3 != 18 && num3 != 22 && num3 != 26 && num3 != 30)
		{
			flag = true;
		}
		else if (size == 4 && num3 != 4 && num3 != 12 && num3 != 20 && num3 != 28)
		{
			flag = true;
		}
		else if (size == 8 && num3 != 8 && num3 != 24)
		{
			flag = true;
		}
		if (dir == 7 || dir == 0 || dir == 1)
		{
			if (flag)
			{
				float num5 = 0f;
				float num6 = 0f;
				num5 = TMap.GetBlockHeight((int)worldpos.x, (int)worldpos.z + size);
				num6 = TMap.GetBlockHeight((int)worldpos.x + size * 2, (int)worldpos.z + size);
				Random.seed = (int)worldpos.x + ((int)worldpos.z + size) * 100;
				num = Random.Range(min, max);
				Random.seed = (int)worldpos.x + size + ((int)worldpos.z + size * 2) * 100;
				num2 = Random.Range(min, max);
				hd = (num5 + num6) / 2f;
				vhd = (num + num2) / 2f;
			}
			else
			{
				float num7 = 0f;
				float num8 = 0f;
				num7 = TMap.GetBlockHeight((int)worldpos.x - size, (int)worldpos.z + size);
				num8 = TMap.GetBlockHeight((int)worldpos.x + size, (int)worldpos.z + size);
				Random.seed = (int)worldpos.x - size + ((int)worldpos.z + size) * 100;
				num = Random.Range(min, max);
				Random.seed = (int)worldpos.x + size + ((int)worldpos.z + size) * 100;
				num2 = Random.Range(min, max);
				hz = (num7 + num8) / 2f;
				vhd = (num + num2) / 2f;
			}
		}
		if (dir == 5 || dir == 4 || dir == 3)
		{
			if (flag)
			{
				float num9 = 0f;
				float num10 = 0f;
				num9 = TMap.GetBlockHeight((int)worldpos.x, (int)worldpos.z);
				num10 = TMap.GetBlockHeight((int)worldpos.x + size * 2, (int)worldpos.z);
				Random.seed = (int)worldpos.x + ((int)worldpos.z + size) * 100;
				num = Random.Range(min, max);
				Random.seed = (int)worldpos.x + size + ((int)worldpos.z + size * 2) * 100;
				num2 = Random.Range(min, max);
				hx = (num9 + num10) / 2f;
				vhx = (num + num2) / 2f;
			}
			else
			{
				float num11 = 0f;
				float num12 = 0f;
				num11 = TMap.GetBlockHeight((int)worldpos.x - size, (int)worldpos.z);
				num12 = TMap.GetBlockHeight((int)worldpos.x + size, (int)worldpos.z);
				Random.seed = (int)worldpos.x - size + (int)worldpos.z * 100;
				num = Random.Range(min, max);
				Random.seed = (int)worldpos.x + size + (int)worldpos.z * 100;
				num2 = Random.Range(min, max);
				h = (num11 + num12) / 2f;
				vhd = (num + num2) / 2f;
			}
		}
		flag = false;
		if (num4 % 2 == 0 && size == 1)
		{
			flag = true;
		}
		else if (size == 2 && num4 != 2 && num4 != 6 && num4 != 10 && num4 != 14 && num4 != 18 && num4 != 22 && num4 != 26 && num4 != 30)
		{
			flag = true;
		}
		else if (size == 4 && num4 != 4 && num4 != 12 && num4 != 20 && num4 != 28)
		{
			flag = true;
		}
		else if (size == 8 && num4 != 8 && num4 != 24)
		{
			flag = true;
		}
		if (dir == 7 || dir == 6 || dir == 5)
		{
			if (flag)
			{
				float num13 = 0f;
				float num14 = 0f;
				num13 = TMap.GetBlockHeight((int)worldpos.x, (int)worldpos.z + size * 2);
				num14 = TMap.GetBlockHeight((int)worldpos.x, (int)worldpos.z);
				Random.seed = (int)worldpos.x - size + ((int)worldpos.z + size) * 100;
				num = Random.Range(min, max);
				Random.seed = (int)worldpos.x + size + ((int)worldpos.z + size) * 100;
				num2 = Random.Range(min, max);
				hz = (num13 + num14) / 2f;
				vhd = (num + num2) / 2f;
			}
			else
			{
				float num15 = 0f;
				float num16 = 0f;
				num15 = TMap.GetBlockHeight((int)worldpos.x, (int)worldpos.z + size);
				num16 = TMap.GetBlockHeight((int)worldpos.x, (int)worldpos.z - size);
				Random.seed = (int)worldpos.x - size + ((int)worldpos.z + size) * 100;
				num = Random.Range(min, max);
				Random.seed = (int)worldpos.x + size + ((int)worldpos.z + size) * 100;
				num2 = Random.Range(min, max);
				h = (num15 + num16) / 2f;
				vhd = (num + num2) / 2f;
			}
		}
		if (dir == 1 || dir == 2 || dir == 3)
		{
			if (flag)
			{
				float num17 = 0f;
				float num18 = 0f;
				num17 = TMap.GetBlockHeight((int)worldpos.x + size, (int)worldpos.z + size * 2);
				num18 = TMap.GetBlockHeight((int)worldpos.x + size, (int)worldpos.z);
				Random.seed = (int)worldpos.x - size + ((int)worldpos.z + size) * 100;
				num = Random.Range(min, max);
				Random.seed = (int)worldpos.x + size + ((int)worldpos.z + size) * 100;
				num2 = Random.Range(min, max);
				hd = (num17 + num18) / 2f;
				vhd = (num + num2) / 2f;
			}
			else
			{
				float num19 = 0f;
				float num20 = 0f;
				num19 = TMap.GetBlockHeight((int)worldpos.x + size, (int)worldpos.z + size);
				num20 = TMap.GetBlockHeight((int)worldpos.x + size, (int)worldpos.z - size);
				Random.seed = (int)worldpos.x - size + ((int)worldpos.z + size) * 100;
				num = Random.Range(min, max);
				Random.seed = (int)worldpos.x + size + ((int)worldpos.z + size) * 100;
				num2 = Random.Range(min, max);
				hx = (num19 + num20) / 2f;
				vhx = (num + num2) / 2f;
			}
		}
	}
}
