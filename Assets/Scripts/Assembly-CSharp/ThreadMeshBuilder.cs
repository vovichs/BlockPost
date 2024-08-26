using System;
using System.Collections.Generic;
using UnityEngine;

public class ThreadMeshBuilder
{
	public List<Vector3> vertices = new List<Vector3>(4096);

	private List<Vector2> uv = new List<Vector2>(4096);

	public List<Vector3> normals = new List<Vector3>(4096);

	private List<Color> colors = new List<Color>(4096);

	public List<int>[] indices = new List<int>[0];

	private List<Vector4> tangents = new List<Vector4>(4096);

	private int ind = 1;

	public Mesh mesh;

	public int max_vertices;

	public int max_colors;

	public int max_normals;

	public int max_uv;

	public int max_tangents;

	public int max_indices;

	public void AddVertices(Vector3[] vert, Vector3 offset)
	{
		foreach (Vector3 vector in vert)
		{
			vertices.Add(vector + offset);
			tangents.Add(new Vector4(0f, 0f, 1f, 1f));
		}
	}

	public void AddNormals(Vector3[] norm)
	{
		normals.AddRange(norm);
	}

	public void AddColor(Color color)
	{
		colors.Add(color);
	}

	public void AddFaceColor(Color color)
	{
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	public void AddFaceColor(Color color, float a, float b, float c, float d)
	{
		colors.Add(new Color(color.r * a, color.g * a, color.b * a));
		colors.Add(new Color(color.r * b, color.g * b, color.b * b));
		colors.Add(new Color(color.r * c, color.g * c, color.b * c));
		colors.Add(new Color(color.r * d, color.g * d, color.b * d));
	}

	public void AddColors(Color color, int count)
	{
		for (int i = 0; i < count; i++)
		{
			colors.Add(color);
		}
	}

	public void AddTexCoords(Vector2[] _uv)
	{
		uv.AddRange(_uv);
	}

	public void AddFaceIndices(int materialIndex)
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

	public void AddFaceIndices_flipped(int materialIndex)
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

	public void AddFaceIndicesGreedy(int materialIndex)
	{
		int count = vertices.Count;
		List<int> list = GetIndices(materialIndex);
		list.Add(count);
		list.Add(count + 1);
		list.Add(count + 2);
		list.Add(count + 3);
		list.Add(count);
		list.Add(count + 2);
		list.Add(count + 2);
		list.Add(count + 1);
		list.Add(count);
		list.Add(count + 2);
		list.Add(count);
		list.Add(count + 3);
	}

	public void AddIndices(int materialIndex, int[] indices)
	{
		int count = vertices.Count;
		List<int> list = GetIndices(materialIndex);
		foreach (int num in indices)
		{
			list.Add(num + count);
		}
	}

	public List<int> GetIndices(int index)
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

	public List<Color> GetColors()
	{
		return colors;
	}

	public void Clear()
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

	public void Create()
	{
		mesh = new Mesh();
		Clear();
	}

	public int GetVerticesCount()
	{
		return vertices.Count;
	}

	public Mesh ToMesh()
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
