using System.Collections.Generic;
using UnityEngine;

public class GeometryBuffer
{
	private class ObjectData
	{
		public string name;

		public List<GroupData> groups;

		public List<FaceIndices> allFaces;

		public int normalCount;

		public ObjectData()
		{
			groups = new List<GroupData>();
			allFaces = new List<FaceIndices>();
			normalCount = 0;
		}
	}

	private class GroupData
	{
		public string name;

		public string materialName;

		public List<FaceIndices> faces;

		public bool isEmpty
		{
			get
			{
				return faces.Count == 0;
			}
		}

		public GroupData()
		{
			faces = new List<FaceIndices>();
		}
	}

	private List<ObjectData> objects;

	public List<Vector3> vertices;

	public List<Vector2> uvs;

	public List<Vector3> normals;

	public int unnamedGroupIndex = 1;

	private ObjectData current;

	private GroupData curgr;

	public static int MAX_VERTICES_LIMIT_FOR_A_MESH = 64999;

	public int numObjects
	{
		get
		{
			return objects.Count;
		}
	}

	public bool isEmpty
	{
		get
		{
			return vertices.Count == 0;
		}
	}

	public bool hasUVs
	{
		get
		{
			return uvs.Count > 0;
		}
	}

	public bool hasNormals
	{
		get
		{
			return normals.Count > 0;
		}
	}

	public GeometryBuffer()
	{
		objects = new List<ObjectData>();
		ObjectData objectData = new ObjectData
		{
			name = "default"
		};
		objects.Add(objectData);
		current = objectData;
		GroupData item = new GroupData
		{
			name = "default"
		};
		objectData.groups.Add(item);
		curgr = item;
		vertices = new List<Vector3>();
		uvs = new List<Vector2>();
		normals = new List<Vector3>();
	}

	public void PushObject(string name)
	{
		string materialName = current.groups[current.groups.Count - 1].materialName;
		if (isEmpty)
		{
			objects.Remove(current);
		}
		ObjectData objectData = new ObjectData();
		objectData.name = name;
		objects.Add(objectData);
		GroupData groupData = new GroupData();
		groupData.materialName = materialName;
		groupData.name = "default";
		objectData.groups.Add(groupData);
		curgr = groupData;
		current = objectData;
	}

	public void PushGroup(string name)
	{
		string materialName = current.groups[current.groups.Count - 1].materialName;
		if (curgr.isEmpty)
		{
			current.groups.Remove(curgr);
		}
		GroupData groupData = new GroupData();
		groupData.materialName = materialName;
		if (name == null)
		{
			name = "Unnamed-" + unnamedGroupIndex;
			unnamedGroupIndex++;
		}
		groupData.name = name;
		current.groups.Add(groupData);
		curgr = groupData;
	}

	public void PushMaterialName(string name)
	{
		if (!curgr.isEmpty)
		{
			PushGroup(name);
		}
		if (curgr.name == "default")
		{
			curgr.name = name;
		}
		curgr.materialName = name;
	}

	public void PushVertex(Vector3 v)
	{
		vertices.Add(v);
	}

	public void PushUV(Vector2 v)
	{
		uvs.Add(v);
	}

	public void PushNormal(Vector3 v)
	{
		normals.Add(v);
	}

	public void PushFace(FaceIndices f)
	{
		curgr.faces.Add(f);
		current.allFaces.Add(f);
		if (f.vn >= 0)
		{
			current.normalCount++;
		}
	}

	public void Trace()
	{
		Debug.Log("OBJ has " + objects.Count + " object(s)");
		Debug.Log("OBJ has " + vertices.Count + " vertice(s)");
		Debug.Log("OBJ has " + uvs.Count + " uv(s)");
		Debug.Log("OBJ has " + normals.Count + " normal(s)");
		foreach (ObjectData @object in objects)
		{
			Debug.Log(@object.name + " has " + @object.groups.Count + " group(s)");
			foreach (GroupData group in @object.groups)
			{
				Debug.Log(@object.name + "/" + group.name + " has " + group.faces.Count + " faces(s)");
			}
		}
	}

	public void PopulateMeshes(GameObject[] gs, Dictionary<string, Material> mats)
	{
		if (gs.Length != numObjects)
		{
			return;
		}
		Debug.Log("PopulateMeshes GameObjects count:" + gs.Length);
		for (int i = 0; i < gs.Length; i++)
		{
			ObjectData objectData = objects[i];
			bool flag = hasNormals && objectData.normalCount > 0;
			if (objectData.name != "default")
			{
				gs[i].name = objectData.name;
			}
			Debug.Log("PopulateMeshes object name:" + objectData.name);
			Vector3[] array = new Vector3[objectData.allFaces.Count];
			Vector2[] array2 = new Vector2[objectData.allFaces.Count];
			Vector3[] array3 = new Vector3[objectData.allFaces.Count];
			int num = 0;
			foreach (FaceIndices allFace in objectData.allFaces)
			{
				if (num >= MAX_VERTICES_LIMIT_FOR_A_MESH)
				{
					Debug.LogWarning("maximum vertex number for a mesh exceeded for object:" + gs[i].name);
					break;
				}
				array[num] = vertices[allFace.vi];
				if (hasUVs)
				{
					array2[num] = uvs[allFace.vu];
				}
				if (hasNormals && allFace.vn >= 0)
				{
					array3[num] = normals[allFace.vn];
				}
				num++;
			}
			Mesh mesh = (gs[i].GetComponent(typeof(MeshFilter)) as MeshFilter).mesh;
			mesh.vertices = array;
			if (hasUVs)
			{
				mesh.uv = array2;
			}
			if (flag)
			{
				mesh.normals = array3;
			}
			if (objectData.groups.Count == 1)
			{
				Debug.Log("PopulateMeshes only one group: " + objectData.groups[0].name);
				GroupData groupData = objectData.groups[0];
				string text = ((groupData.materialName != null) ? groupData.materialName : "default");
				if (mats.ContainsKey(text))
				{
					gs[i].GetComponent<Renderer>().material = mats[text];
					Debug.Log("PopulateMeshes mat:" + text + " set.");
				}
				else
				{
					Debug.LogWarning("PopulateMeshes mat:" + text + " not found.");
				}
				int[] array4 = new int[groupData.faces.Count];
				for (int j = 0; j < array4.Length; j++)
				{
					array4[j] = j;
				}
				mesh.triangles = array4;
			}
			else
			{
				int count = objectData.groups.Count;
				Material[] array5 = new Material[count];
				mesh.subMeshCount = count;
				int num2 = 0;
				Debug.Log("PopulateMeshes group count:" + count);
				for (int k = 0; k < count; k++)
				{
					string text2 = ((objectData.groups[k].materialName != null) ? objectData.groups[k].materialName : "default");
					if (mats.ContainsKey(text2))
					{
						array5[k] = mats[text2];
						Console.Log("PopulateMeshes mat:" + text2 + " set.");
					}
					else
					{
						Console.Log("PopulateMeshes mat:" + text2 + " not found.");
						array5[k] = new Material(ContentLoader_.LoadMaterial("vertex_color"));
					}
					int[] array6 = new int[objectData.groups[k].faces.Count];
					int num3 = objectData.groups[k].faces.Count + num2;
					int num4 = 0;
					while (num2 < num3)
					{
						array6[num4] = num2;
						num2++;
						num4++;
					}
					mesh.SetTriangles(array6, k);
				}
				gs[i].GetComponent<Renderer>().materials = array5;
			}
			if (!flag)
			{
				mesh.RecalculateNormals();
			}
		}
	}
}
