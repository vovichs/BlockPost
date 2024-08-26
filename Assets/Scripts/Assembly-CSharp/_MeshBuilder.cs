using System;
using System.Collections.Generic;
using UnityEngine;

public class _MeshBuilder
{
	public static List<Vector3> vertices = new List<Vector3>(4096);

	private static List<Vector2> uv = new List<Vector2>(4096);

	public static List<Vector3> normals = new List<Vector3>(4096);

	private static List<Color> colors = new List<Color>(4096);

	public static List<int>[] indices = new List<int>[0];

	private static List<Vector4> tangents = new List<Vector4>(4096);

	private static int ind = 1;

	public static Mesh mesh = null;

	public static int max_vertices = 0;

	public static int max_colors = 0;

	public static int max_normals = 0;

	public static int max_uv = 0;

	public static int max_tangents = 0;

	public static int max_indices = 0;

	public static void AddVertices(Vector3[] vert, Vector3 offset)
	{
		foreach (Vector3 vector in vert)
		{
			vertices.Add(vector + offset);
			tangents.Add(new Vector4(0f, 0f, 1f, 1f));
		}
	}

	public static void AddNormals(Vector3[] norm)
	{
		normals.AddRange(norm);
	}

	public static void AddColor(Color color)
	{
		colors.Add(color);
	}

	public static void AddFaceColor(Color color)
	{
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	public static void AddFaceColor(Color color, float a, float b, float c, float d)
	{
		colors.Add(new Color(color.r * a, color.g * a, color.b * a));
		colors.Add(new Color(color.r * b, color.g * b, color.b * b));
		colors.Add(new Color(color.r * c, color.g * c, color.b * c));
		colors.Add(new Color(color.r * d, color.g * d, color.b * d));
	}

	public static void AddColors(Color color, int count)
	{
		for (int i = 0; i < count; i++)
		{
			colors.Add(color);
		}
	}

	public static void AddTexCoords(Vector2[] _uv)
	{
		uv.AddRange(_uv);
	}

	public static void AddFaceIndices(int materialIndex)
	{
		int count = vertices.Count;
		List<int> list = GetIndices(materialIndex);
		list.Add(count + 2);
		list.Add(count + 1);
		list.Add(count);
		list.Add(count + 3);
		list.Add(count + 2);
		list.Add(count);
	}

	public static void AddFaceIndices_flipped(int materialIndex)
	{
		int count = vertices.Count;
		List<int> list = GetIndices(materialIndex);
		list.Add(count + 3);
		list.Add(count + 1);
		list.Add(count);
		list.Add(count + 3);
		list.Add(count + 2);
		list.Add(count + 1);
	}

	public static void AddFaceIndicesGreedy(int materialIndex)
	{
		int count = vertices.Count;
		List<int> list = GetIndices(materialIndex);
		list.Add(count + 2);
		list.Add(count + 1);
		list.Add(count);
		list.Add(count + 3);
		list.Add(count + 2);
		list.Add(count);
		list.Add(count);
		list.Add(count + 1);
		list.Add(count + 2);
		list.Add(count);
		list.Add(count + 2);
		list.Add(count + 3);
	}

	public static void AddFaceIndicesGreedy(int materialIndex, bool backface)
	{
		int count = vertices.Count;
		List<int> list = GetIndices(materialIndex);
		if (backface)
		{
			list.Add(count + 2);
			list.Add(count + 1);
			list.Add(count);
			list.Add(count + 3);
			list.Add(count + 2);
			list.Add(count);
		}
		else
		{
			list.Add(count);
			list.Add(count + 1);
			list.Add(count + 2);
			list.Add(count);
			list.Add(count + 2);
			list.Add(count + 3);
		}
	}

	public static void AddIndicesNull(int materialIndex, int[] indices)
	{
		int num = 0;
		List<int> list = GetIndices(materialIndex);
		foreach (int num2 in indices)
		{
			list.Add(num2 + num);
		}
	}

	public static void AddIndices(int materialIndex, int[] indices)
	{
		int count = vertices.Count;
		List<int> list = GetIndices(materialIndex);
		foreach (int num in indices)
		{
			list.Add(num + count);
		}
	}

	public static List<int> GetIndices(int index)
	{
		if (index >= indices.Length)
		{
			int num = indices.Length;
			Array.Resize(ref indices, index + 1);
			for (int i = num; i < indices.Length; i++)
			{
				indices[i] = new List<int>(8192);
			}
		}
		if (index + 1 > ind)
		{
			ind = index + 1;
		}
		return indices[index];
	}

	public static List<Color> GetColors()
	{
		return colors;
	}

	public static void Clear()
	{
		vertices.Clear();
		uv.Clear();
		normals.Clear();
		colors.Clear();
		tangents.Clear();
		List<int>[] array = indices;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Clear();
		}
		ind = 1;
	}

	public static void Create()
	{
		mesh = new Mesh();
		Clear();
	}

	public static int GetVerticesCount()
	{
		return vertices.Count;
	}

	public static Mesh ToMesh()
	{
		mesh.vertices = vertices.ToArray();
		mesh.colors = colors.ToArray();
		mesh.normals = normals.ToArray();
		mesh.uv = uv.ToArray();
		mesh.tangents = tangents.ToArray();
		mesh.subMeshCount = ind;
		for (int i = 0; i < ind; i++)
		{
			mesh.SetTriangles(indices[i].ToArray(), i);
		}
		return mesh;
	}
}
