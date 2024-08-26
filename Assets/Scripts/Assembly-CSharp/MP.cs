using System.Collections.Generic;
using UnityEngine;

public class MP : MonoBehaviour
{
	private static List<Mesh> meshlist = new List<Mesh>();

	public static Mesh Get(string meshname)
	{
		int i = 0;
		for (int count = meshlist.Count; i < count; i++)
		{
			if (meshlist[i].name == meshname)
			{
				return meshlist[i];
			}
		}
		return null;
	}

	public static void Add(Mesh mesh)
	{
		int i = 0;
		for (int count = meshlist.Count; i < count; i++)
		{
			if (meshlist[i].name == mesh.name)
			{
				return;
			}
		}
		meshlist.Add(mesh);
	}
}
