using System.Collections.Generic;
using UnityEngine;

public class Greedy : MonoBehaviour
{
	private static List<Vector3> vertices = new List<Vector3>();

	private static List<int> elements = new List<int>();

	private static Map.CChunk chunk = null;

	private static Vector3[] vx = new Vector3[4];

	private static bool done = false;

	private static int[] x = new int[3];

	private static int[] q = new int[3];

	private static bool[] mask = new bool[Map.CHUNK_SIZE_X * Map.CHUNK_SIZE_X];

	private static int[,,] voxelData = null;

	private static int[,,] voxelDataSide = null;

	private static Color[,,] voxelDataColor = null;

	private static int SOUTH = 0;

	private static int NORTH = 1;

	private static int EAST = 2;

	private static int WEST = 3;

	private static int TOP = 4;

	private static int BOTTOM = 5;

	private static bool stop = false;

	private void Start()
	{
		Mesh sharedMesh = Build(null);
		GameObject obj = new GameObject();
		obj.AddComponent<MeshFilter>().sharedMesh = sharedMesh;
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = new Material(Shader.Find("Standard (Vertex Color)"));
		meshRenderer.sharedMaterial.SetTexture("_MainTex", TEX.GetTextureByName("gray"));
	}

	public static Mesh Build(int[,,] block, Color[,,] color)
	{
		voxelData = block;
		voxelDataColor = color;
		return Build();
	}

	public static Mesh Build(Map.CChunk c)
	{
		if (c == null)
		{
			return null;
		}
		chunk = c;
		voxelData = chunk.block;
		voxelDataColor = chunk.color;
		return Build();
	}

	public static Mesh Build()
	{
		if (voxelDataSide == null)
		{
			voxelDataSide = new int[Map.CHUNK_SIZE_X, Map.CHUNK_SIZE_Y, Map.CHUNK_SIZE_Z];
		}
		for (int i = 0; i < Map.CHUNK_SIZE_X; i++)
		{
			for (int j = 0; j < Map.CHUNK_SIZE_Y; j++)
			{
				for (int k = 0; k < Map.CHUNK_SIZE_Z; k++)
				{
					voxelDataSide[i, j, k] = 0;
				}
			}
		}
		MeshBuilder.Create();
		bool flag = true;
		for (bool flag2 = false; flag2 != flag; flag2 = !flag2)
		{
			for (int l = 0; l < 3; l++)
			{
				int num = (l + 1) % 3;
				int num2 = (l + 2) % 3;
				int side = 0;
				x[0] = 0;
				x[1] = 0;
				x[2] = 0;
				q[0] = 0;
				q[1] = 0;
				q[2] = 0;
				q[l] = 1;
				switch (l)
				{
				case 0:
					side = (flag ? WEST : EAST);
					break;
				case 1:
					side = (flag ? BOTTOM : TOP);
					break;
				case 2:
					side = (flag ? SOUTH : NORTH);
					break;
				}
				x[l] = -1;
				while (x[l] < Map.CHUNK_SIZE_X)
				{
					int num3 = 0;
					x[num2] = 0;
					while (x[num2] < Map.CHUNK_SIZE_X)
					{
						x[num] = 0;
						while (x[num] < Map.CHUNK_SIZE_X)
						{
							bool flag3 = x[l] >= 0 && data(x[0], x[1], x[2], side);
							bool flag4 = x[l] < Map.CHUNK_SIZE_X - 1 && data(x[0] + q[0], x[1] + q[1], x[2] + q[2], side);
							mask[num3++] = flag3 != flag4 && (flag ? flag4 : flag3);
							x[num]++;
						}
						x[num2]++;
					}
					x[l]++;
					num3 = 0;
					for (int m = 0; m < Map.CHUNK_SIZE_X; m++)
					{
						int num4 = 0;
						while (num4 < Map.CHUNK_SIZE_X)
						{
							if (mask[num3])
							{
								int n;
								for (n = 1; num4 + n < Map.CHUNK_SIZE_X && mask[num3 + n]; n++)
								{
								}
								done = false;
								int num5;
								for (num5 = 1; m + num5 < Map.CHUNK_SIZE_X; num5++)
								{
									for (int num6 = 0; num6 < n; num6++)
									{
										if (!mask[num3 + num6 + num5 * Map.CHUNK_SIZE_X])
										{
											done = true;
											break;
										}
									}
									if (done)
									{
										break;
									}
								}
								x[num] = num4;
								x[num2] = m;
								int[] array = new int[3];
								int[] array2 = new int[3];
								array[num] = n;
								array2[num2] = num5;
								vx[0].Set(x[0], x[1], x[2]);
								vx[1].Set(x[0] + array[0], x[1] + array[1], x[2] + array[2]);
								vx[2].Set(x[0] + array[0] + array2[0], x[1] + array[1] + array2[1], x[2] + array[2] + array2[2]);
								vx[3].Set(x[0] + array2[0], x[1] + array2[1], x[2] + array2[2]);
								MeshBuilder.AddTexCoords(Atlas.GetUV(40, 0));
								MeshBuilder.AddFaceIndicesGreedy(0, flag);
								MeshBuilder.AddVertices(vx, Vector3.zero);
								switch (l)
								{
								case 0:
									if (flag)
									{
										MeshBuilder.AddColor(dataColor(x[0], x[1], x[2]));
										MeshBuilder.AddColor(dataColor(x[0] + array[0], x[1] + array[1] - 1, x[2] + array[2]));
										MeshBuilder.AddColor(dataColor(x[0] + array[0] + array2[0], x[1] + array[1] + array2[1] - 1, x[2] + array[2] + array2[2] - 1));
										MeshBuilder.AddColor(dataColor(x[0] + array2[0], x[1] + array2[1], x[2] + array2[2] - 1));
									}
									else
									{
										MeshBuilder.AddColor(dataColor(x[0] - 1, x[1], x[2]));
										MeshBuilder.AddColor(dataColor(x[0] + array[0] - 1, x[1] + array[1] - 1, x[2] + array[2]));
										MeshBuilder.AddColor(dataColor(x[0] + array[0] + array2[0] - 1, x[1] + array[1] + array2[1] - 1, x[2] + array[2] + array2[2] - 1));
										MeshBuilder.AddColor(dataColor(x[0] + array2[0] - 1, x[1] + array2[1], x[2] + array2[2] - 1));
									}
									break;
								case 1:
									if (flag)
									{
										MeshBuilder.AddColor(dataColor(x[0], x[1], x[2]));
										MeshBuilder.AddColor(dataColor(x[0] + array[0], x[1] + array[1], x[2] + array[2] - 1));
										MeshBuilder.AddColor(dataColor(x[0] + array[0] + array2[0] - 1, x[1] + array[1] + array2[1], x[2] + array[2] + array2[2] - 1));
										MeshBuilder.AddColor(dataColor(x[0] + array2[0] - 1, x[1] + array2[1], x[2] + array2[2]));
									}
									else
									{
										MeshBuilder.AddColor(dataColor(x[0], x[1] - 1, x[2]));
										MeshBuilder.AddColor(dataColor(x[0] + array[0], x[1] + array[1] - 1, x[2] + array[2] - 1));
										MeshBuilder.AddColor(dataColor(x[0] + array[0] + array2[0] - 1, x[1] + array[1] + array2[1] - 1, x[2] + array[2] + array2[2] - 1));
										MeshBuilder.AddColor(dataColor(x[0] + array2[0] - 1, x[1] + array2[1] - 1, x[2] + array2[2]));
									}
									break;
								case 2:
									if (flag)
									{
										MeshBuilder.AddColor(dataColor(x[0], x[1], x[2]));
										MeshBuilder.AddColor(dataColor(x[0] + array[0] - 1, x[1] + array[1], x[2] + array[2]));
										MeshBuilder.AddColor(dataColor(x[0] + array[0] + array2[0] - 1, x[1] + array[1] + array2[1] - 1, x[2] + array[2] + array2[2]));
										MeshBuilder.AddColor(dataColor(x[0] + array2[0], x[1] + array2[1] - 1, x[2] + array2[2]));
									}
									else
									{
										MeshBuilder.AddColor(dataColor(x[0], x[1], x[2] - 1));
										MeshBuilder.AddColor(dataColor(x[0] + array[0] - 1, x[1] + array[1], x[2] + array[2] - 1));
										MeshBuilder.AddColor(dataColor(x[0] + array[0] + array2[0] - 1, x[1] + array[1] + array2[1] - 1, x[2] + array[2] + array2[2] - 1));
										MeshBuilder.AddColor(dataColor(x[0] + array2[0], x[1] + array2[1] - 1, x[2] + array2[2] - 1));
									}
									break;
								}
								for (int num7 = 0; num7 < num5; num7++)
								{
									for (int num6 = 0; num6 < n; num6++)
									{
										mask[num3 + num6 + num7 * Map.CHUNK_SIZE_X] = false;
									}
								}
								num4 += n;
								num3 += n;
							}
							else
							{
								num4++;
								num3++;
							}
						}
					}
				}
			}
			flag = flag && flag2;
		}
		Mesh mesh = MeshBuilder.ToMesh();
		mesh.RecalculateNormals();
		return mesh;
	}

	private static bool data(int x, int y, int z)
	{
		return voxelData[x, y, z] > 0;
	}

	private static bool data(int x, int y, int z, int side)
	{
		voxelDataSide[x, y, z] = side;
		return voxelData[x, y, z] > 0;
	}

	private static Color dataColor(int x, int y, int z)
	{
		if (x < 0)
		{
			x = 0;
		}
		if (y < 0)
		{
			y = 0;
		}
		if (z < 0)
		{
			z = 0;
		}
		if (x >= Map.CHUNK_SIZE_X)
		{
			return Color.white;
		}
		if (y >= Map.CHUNK_SIZE_Y)
		{
			return Color.white;
		}
		if (z >= Map.CHUNK_SIZE_Z)
		{
			return Color.white;
		}
		if (voxelData[x, y, z] == 0)
		{
			return Color.black;
		}
		return voxelDataColor[x, y, z];
	}
}
