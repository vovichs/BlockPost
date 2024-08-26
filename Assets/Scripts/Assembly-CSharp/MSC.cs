using System.Collections.Generic;
using UnityEngine;

public class MSC : MonoBehaviour
{
	public static List<Map.CChunk> cfull = new List<Map.CChunk>();

	public static List<Map.CChunk> cmesh = new List<Map.CChunk>();

	private void Update()
	{
		if (Map.chunk == null)
		{
			return;
		}
		TEX.GetTextureByName("green");
		int height = Screen.height;
		Vector3 vector = new Vector3(4f, 4f, 4f);
		cfull.Clear();
		cmesh.Clear();
		for (int i = 0; i < Map.MAP_SIZE_X; i++)
		{
			for (int j = 0; j < Map.MAP_SIZE_Y; j++)
			{
				for (int k = 0; k < Map.MAP_SIZE_Z; k++)
				{
					if (Map.chunk[i, j, k] == null)
					{
						continue;
					}
					Map.CChunk cChunk = Map.chunk[i, j, k];
					if (cChunk.count == 512)
					{
						Vector3 position = new Vector3(cChunk.x * Map.CHUNK_SIZE_X + 4, cChunk.y * Map.CHUNK_SIZE_Y + 4, cChunk.z * Map.CHUNK_SIZE_Z + 4);
						position = Controll.csCam.WorldToScreenPoint(position);
						cChunk.sp = position;
						if (cChunk.sp.z >= 0f)
						{
							cfull.Add(cChunk);
						}
					}
					else if (!(cChunk.go == null))
					{
						cChunk.go.SetActive(true);
						Vector3 position = cChunk.go.transform.localPosition + vector;
						position = Controll.csCam.WorldToScreenPoint(position);
						cChunk.sp = position;
						if (cChunk.sp.z >= 0f)
						{
							cmesh.Add(cChunk);
						}
						else
						{
							cChunk.go.SetActive(false);
						}
					}
				}
			}
		}
		int count = cfull.Count;
		int count2 = cmesh.Count;
		for (int l = 0; l < count; l++)
		{
			Map.CChunk cChunk2 = cfull[l];
			for (int m = 0; m < count2; m++)
			{
				Map.CChunk cChunk3 = cmesh[m];
				if (InRadius(cChunk2.sp, cChunk3.sp))
				{
					cChunk3.go.SetActive(false);
				}
			}
		}
	}

	private void OnGUI()
	{
		if (Map.chunk != null)
		{
			GUIM.DrawText(new Rect(120f, 120f, 200f, 30f), "MESH: " + cmesh.Count + "FULL : " + cfull.Count, TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, true);
		}
	}

	private bool InRadius(Vector3 p1, Vector3 p2)
	{
		p1.y = (float)Screen.height - p1.y;
		p2.y = (float)Screen.height - p2.y;
		float num = 0f;
		float num2 = 0f;
		num = ((!(p1.x > p2.x)) ? (p2.x - p1.x) : (p1.x - p2.x));
		num2 = ((!(p1.y > p2.y)) ? (p2.y - p1.y) : (p1.y - p2.y));
		if (num < 4f && num2 < 4f)
		{
			return true;
		}
		return false;
	}
}
