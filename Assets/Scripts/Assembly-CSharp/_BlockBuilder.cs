using UnityEngine;

public class _BlockBuilder
{
	public static float SCALE = 1f;

	public static bool fullview = true;

	public static Vector3[] directions = new Vector3[6]
	{
		Vector3.forward,
		Vector3.back,
		Vector3.right,
		Vector3.left,
		Vector3.up,
		Vector3.down
	};

	public static Vector3[][] vertices = new Vector3[8][]
	{
		new Vector3[4]
		{
			new Vector3(0f, 0f, SCALE),
			new Vector3(0f, SCALE, SCALE),
			new Vector3(SCALE, SCALE, SCALE),
			new Vector3(SCALE, 0f, SCALE)
		},
		new Vector3[4]
		{
			new Vector3(SCALE, 0f, 0f),
			new Vector3(SCALE, SCALE, 0f),
			new Vector3(0f, SCALE, 0f),
			new Vector3(0f, 0f, 0f)
		},
		new Vector3[4]
		{
			new Vector3(SCALE, 0f, SCALE),
			new Vector3(SCALE, SCALE, SCALE),
			new Vector3(SCALE, SCALE, 0f),
			new Vector3(SCALE, 0f, 0f)
		},
		new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, SCALE, 0f),
			new Vector3(0f, SCALE, SCALE),
			new Vector3(0f, 0f, SCALE)
		},
		new Vector3[4]
		{
			new Vector3(SCALE, SCALE, 0f),
			new Vector3(SCALE, SCALE, SCALE),
			new Vector3(0f, SCALE, SCALE),
			new Vector3(0f, SCALE, 0f)
		},
		new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, SCALE),
			new Vector3(SCALE, 0f, SCALE),
			new Vector3(SCALE, 0f, 0f)
		},
		new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, SCALE, 0f),
			new Vector3(SCALE, SCALE, SCALE),
			new Vector3(SCALE, 0f, SCALE)
		},
		new Vector3[4]
		{
			new Vector3(0f, 0f, SCALE),
			new Vector3(0f, SCALE, SCALE),
			new Vector3(SCALE, SCALE, SCALE),
			new Vector3(SCALE, 0f, SCALE)
		}
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

	public static Vector3[][] ao = new Vector3[6][]
	{
		new Vector3[12]
		{
			new Vector3(-1f, 0f, 1f),
			new Vector3(-1f, -1f, 1f),
			new Vector3(0f, -1f, 1f),
			new Vector3(0f, 1f, 1f),
			new Vector3(-1f, 1f, 1f),
			new Vector3(-1f, 0f, 1f),
			new Vector3(1f, 0f, 1f),
			new Vector3(1f, 1f, 1f),
			new Vector3(0f, 1f, 1f),
			new Vector3(1f, 0f, 1f),
			new Vector3(1f, -1f, 1f),
			new Vector3(0f, -1f, 1f)
		},
		new Vector3[12]
		{
			new Vector3(1f, 0f, -1f),
			new Vector3(1f, -1f, -1f),
			new Vector3(0f, -1f, -1f),
			new Vector3(1f, 0f, -1f),
			new Vector3(1f, 1f, -1f),
			new Vector3(0f, 1f, -1f),
			new Vector3(0f, 1f, -1f),
			new Vector3(-1f, 1f, -1f),
			new Vector3(-1f, 0f, -1f),
			new Vector3(-1f, 0f, -1f),
			new Vector3(-1f, -1f, -1f),
			new Vector3(0f, -1f, -1f)
		},
		new Vector3[12]
		{
			new Vector3(1f, 0f, 1f),
			new Vector3(1f, -1f, 1f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, 1f, 0f),
			new Vector3(1f, 1f, 1f),
			new Vector3(1f, 0f, 1f),
			new Vector3(1f, 1f, 0f),
			new Vector3(1f, 1f, -1f),
			new Vector3(1f, 0f, -1f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, -1f, -1f),
			new Vector3(1f, 0f, -1f)
		},
		new Vector3[12]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(-1f, -1f, -1f),
			new Vector3(-1f, 0f, -1f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(-1f, 1f, -1f),
			new Vector3(-1f, 0f, -1f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(-1f, 1f, 1f),
			new Vector3(-1f, 0f, 1f),
			new Vector3(-1f, 0f, 1f),
			new Vector3(-1f, -1f, 1f),
			new Vector3(-1f, -1f, 0f)
		},
		new Vector3[12]
		{
			new Vector3(1f, 1f, 0f),
			new Vector3(1f, 1f, -1f),
			new Vector3(0f, 1f, -1f),
			new Vector3(1f, 1f, 0f),
			new Vector3(1f, 1f, 1f),
			new Vector3(0f, 1f, 1f),
			new Vector3(0f, 1f, 1f),
			new Vector3(-1f, 1f, 1f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(-1f, 1f, -1f),
			new Vector3(0f, 1f, -1f)
		},
		new Vector3[12]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(-1f, -1f, -1f),
			new Vector3(0f, -1f, -1f),
			new Vector3(0f, -1f, 1f),
			new Vector3(-1f, -1f, 1f),
			new Vector3(-1f, -1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, -1f, 1f),
			new Vector3(0f, -1f, 1f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, -1f, -1f),
			new Vector3(0f, -1f, -1f)
		}
	};

	private static Vector3 _nearpos;

	private static float[] _a = new float[4];

	private static int _face;

	private static int _p;

	private static int _v;

	private static bool _side1;

	private static bool _corner;

	private static bool _side2;

	private static Map map;

	private static bool[,,] array;

	private static int matidx = 0;

	private static int width = 0;

	private static int height = 0;

	private static int depth = 0;

	private static Color c;

	private static Vector3 pivot = Vector3.zero;

	public static void SetMap(Map map)
	{
		_BlockBuilder.map = map;
	}

	public static void Build(int flag, Vector3 localpos, Vector3 worldpos, Color color)
	{
		for (_face = 0; _face < 6; _face++)
		{
			_nearpos = worldpos + directions[_face];
			if (!IsFaceVisible())
			{
				_v = 0;
				for (_p = 0; _p < 4; _p++)
				{
					_side1 = IsFaceVisible(worldpos + ao[_face][_v]);
					_corner = IsFaceVisible(worldpos + ao[_face][_v + 1]);
					_side2 = IsFaceVisible(worldpos + ao[_face][_v + 2]);
					_a[_p] = VertexAO(_side1, _side2, _corner) / 3f;
					_v += 3;
				}
				BuildFaceAO(flag, _face, localpos, color, _a[0], _a[1], _a[2], _a[3]);
			}
		}
	}

	private static float VertexAO(bool side1, bool side2, bool corner)
	{
		if (side1 && side2)
		{
			return 0f;
		}
		return 3 - ((side1 ? 1 : 0) + (side2 ? 1 : 0) + (corner ? 1 : 0));
	}

	private static bool IsFaceVisible(Vector3 nearpos)
	{
		if (nearpos.x < 0f || nearpos.y < 0f || nearpos.z < 0f)
		{
			return !fullview;
		}
		if (nearpos.x >= (float)Map.BLOCK_SIZE_X || nearpos.y >= (float)Map.BLOCK_SIZE_Y || nearpos.z >= (float)Map.BLOCK_SIZE_Z)
		{
			return !fullview;
		}
		if (Map.GetBlock(nearpos) == 0)
		{
			return false;
		}
		return true;
	}

	private static bool IsFaceVisible()
	{
		if (_nearpos.x < 0f || _nearpos.y < 0f || _nearpos.z < 0f)
		{
			return !fullview;
		}
		if (_nearpos.x >= (float)Map.BLOCK_SIZE_X || _nearpos.y >= (float)Map.BLOCK_SIZE_Y || _nearpos.z >= (float)Map.BLOCK_SIZE_Z)
		{
			return !fullview;
		}
		if (Map.GetBlock(_nearpos) == 0)
		{
			return false;
		}
		return true;
	}

	private static void BuildFace(int flag, int face, Vector3 localpos, Color color)
	{
		_MeshBuilder.AddFaceIndices(0);
		_MeshBuilder.AddVertices(vertices[face], localpos);
		_MeshBuilder.AddNormals(normals[face]);
		_MeshBuilder.AddTexCoords(Atlas.GetUV(flag, face));
		_MeshBuilder.AddFaceColor(color);
	}

	private static void BuildFaceAO(int flag, int face, Vector3 localpos, Color color, float a, float b, float c, float d)
	{
		if (a + c < b + d)
		{
			_MeshBuilder.AddFaceIndices_flipped(0);
		}
		else
		{
			_MeshBuilder.AddFaceIndices(0);
		}
		_MeshBuilder.AddVertices(vertices[face], localpos);
		_MeshBuilder.AddNormals(normals[face]);
		_MeshBuilder.AddTexCoords(Atlas.GetUV(flag, face));
		_MeshBuilder.AddFaceColor(color, a, b, c, d);
	}

	private static void BuildFaceLine(int flag, int face, Vector3 localpos, Color color, int sizex, int sizez = 1)
	{
		Vector3[] array = vertices[face];
		Vector3[] vert = new Vector3[4]
		{
			new Vector3(array[0].x * (float)sizex, array[0].y, array[0].z * (float)sizez),
			new Vector3(array[1].x * (float)sizex, array[1].y, array[1].z * (float)sizez),
			new Vector3(array[2].x * (float)sizex, array[2].y, array[2].z * (float)sizez),
			new Vector3(array[3].x * (float)sizex, array[3].y, array[3].z * (float)sizez)
		};
		float num = localpos.x / (float)width;
		float num2 = (localpos.x + (float)sizex) / (float)width;
		float y = localpos.y / (float)height;
		float y2 = (localpos.y + 1f) / (float)height;
		localpos -= pivot;
		switch (face)
		{
		case 0:
		{
			float num3 = num;
			num = num2;
			num2 = num3;
			break;
		}
		default:
			num = 0 / width;
			num2 = 1 / width;
			y = 0 / height;
			y2 = 1 / height;
			color = Color.black;
			break;
		case 1:
			break;
		}
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(num2, y),
			new Vector2(num2, y2),
			new Vector2(num, y2),
			new Vector2(num, y)
		};
		_MeshBuilder.AddFaceIndices(0);
		_MeshBuilder.AddVertices(vert, localpos);
		_MeshBuilder.AddNormals(normals[face]);
		_MeshBuilder.AddTexCoords(uv);
		_MeshBuilder.AddFaceColor(color);
	}

	public static void BuildGrass(int flag, Vector3 localpos, Vector3 worldpos)
	{
	}

	public static void BeginTexBuild(Texture2D tex, Texture2D mask)
	{
		width = tex.width;
		height = tex.height;
		depth = 3;
		array = new bool[width, height, depth];
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				for (int k = 0; k < depth; k++)
				{
					array[i, j, k] = false;
					if (k == 1)
					{
						c = tex.GetPixel(i, j);
					}
					else
					{
						c = mask.GetPixel(i, j);
					}
					if (c.a != 0f)
					{
						array[i, j, k] = true;
					}
				}
			}
		}
	}

	public static void BuildLine(Vector3 pos, int w, int d = 1)
	{
		for (int i = 0; i < 6; i++)
		{
			int face = i;
			Vector3 vector = directions[i];
			Vector3 vector2 = pos + vector;
			BuildFaceLine(1, face, pos, Color.white, w, d);
		}
	}

	public static void BuildFree(int flag, Vector3 pos, Color color)
	{
		for (int i = 0; i < 6; i++)
		{
			int face = i;
			Vector3 vector = directions[i];
			Vector3 vector2 = pos + vector;
			if (!IsFaceVisibleTex((int)vector2.x, (int)vector2.y, (int)vector2.z))
			{
				BuildFace(flag, face, pos * SCALE, color);
			}
		}
	}

	public static void SetPivot(Vector3 pos)
	{
		pivot = pos;
	}

	public static void BuildBox(Vector3 pos, float sizex, float sizey, float sizez, Color c)
	{
		for (int i = 0; i < 6; i++)
		{
			int face = i;
			Vector3 vector = directions[i];
			Vector3 vector2 = pos + vector;
			BuildFaceBlock(1, face, pos, c, sizex, sizey, sizez);
		}
	}

	public static void SetTexCoord(int m, int w, int h)
	{
		matidx = m;
		width = w;
		height = h;
	}

	public static void BuildFaceBlock(int flag, int face, Vector3 localpos, Color color, float sizex, float sizey, float sizez)
	{
		Vector3[] array = vertices[face];
		Vector3[] vert = new Vector3[4]
		{
			new Vector3(array[0].x * sizex, array[0].y * sizey, array[0].z * sizez),
			new Vector3(array[1].x * sizex, array[1].y * sizey, array[1].z * sizez),
			new Vector3(array[2].x * sizex, array[2].y * sizey, array[2].z * sizez),
			new Vector3(array[3].x * sizex, array[3].y * sizey, array[3].z * sizez)
		};
		float x = 0 / width;
		float x2 = 1 / width;
		float y = 0 / height;
		float y2 = 1 / height;
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(x2, y),
			new Vector2(x2, y2),
			new Vector2(x, y2),
			new Vector2(x, y)
		};
		localpos -= pivot;
		_MeshBuilder.AddFaceIndices(0);
		_MeshBuilder.AddVertices(vert, localpos);
		_MeshBuilder.AddNormals(normals[face]);
		_MeshBuilder.AddTexCoords(uv);
		_MeshBuilder.AddFaceColor(color);
	}

	public static void BuildFaceBlockTex(int face, Vector3 localpos, Color color, float sizex, float sizey, float sizez, float x0, float y0, float w, float h, bool rotate = false)
	{
		Vector3[] array = vertices[face];
		Vector3[] vert = new Vector3[4]
		{
			new Vector3(array[0].x * sizex, array[0].y * sizey, array[0].z * sizez),
			new Vector3(array[1].x * sizex, array[1].y * sizey, array[1].z * sizez),
			new Vector3(array[2].x * sizex, array[2].y * sizey, array[2].z * sizez),
			new Vector3(array[3].x * sizex, array[3].y * sizey, array[3].z * sizez)
		};
		float x = x0 / (float)width;
		float x2 = (x0 + w) / (float)width;
		float y = ((float)height - (y0 + h)) / (float)height;
		float y2 = ((float)height - y0) / (float)height;
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(x2, y),
			new Vector2(x2, y2),
			new Vector2(x, y2),
			new Vector2(x, y)
		};
		if (rotate)
		{
			uv = new Vector2[4]
			{
				new Vector2(x2, y2),
				new Vector2(x, y2),
				new Vector2(x, y),
				new Vector2(x2, y)
			};
		}
		localpos -= pivot;
		_MeshBuilder.AddFaceIndices(matidx);
		_MeshBuilder.AddVertices(vert, localpos);
		_MeshBuilder.AddNormals(normals[face]);
		_MeshBuilder.AddTexCoords(uv);
		_MeshBuilder.AddFaceColor(color);
	}

	private static bool IsFaceVisibleTex(int x, int y, int z)
	{
		if (x < 0 || y < 0 || z < 0)
		{
			return false;
		}
		if (x >= width || y >= height || z >= depth)
		{
			return false;
		}
		return array[x, y, z];
	}
}
