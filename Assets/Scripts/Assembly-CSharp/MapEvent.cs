using System.Collections.Generic;
using UnityEngine;

public class MapEvent : MonoBehaviour
{
	private static GameObject[] pgo;

	public static List<GameObject> golist;

	public static void Init()
	{
		if (pgo == null)
		{
			pgo = new GameObject[12];
			pgo[0] = Resources.Load("obj/bp_fir_tree1") as GameObject;
			pgo[1] = Resources.Load("obj/bp_fir_tree2") as GameObject;
			pgo[2] = Resources.Load("obj/bp_snowdrift1") as GameObject;
			pgo[3] = Resources.Load("obj/bp_snowdrift2") as GameObject;
			pgo[4] = Resources.Load("obj/bp_snowdrift3") as GameObject;
			pgo[5] = Resources.Load("obj/bp_snowdrift1") as GameObject;
			pgo[6] = Resources.Load("obj/bp_snowdrift2") as GameObject;
			pgo[7] = Resources.Load("obj/bp_snowdrift3") as GameObject;
			pgo[8] = Resources.Load("obj/bp_snowdrift4") as GameObject;
			pgo[9] = Resources.Load("obj/bp_snowman1") as GameObject;
			pgo[10] = Resources.Load("obj/bp_snowman2") as GameObject;
			pgo[11] = Resources.Load("obj/bp_snowman3") as GameObject;
		}
		if (golist == null)
		{
			golist = new List<GameObject>();
		}
	}

	public static void Set()
	{
		for (int i = 0; i < Map.MAP_SIZE_X; i++)
		{
			float x = (float)Random.Range(0, Map.BLOCK_SIZE_X - 1) + 0.5f;
			float z = (float)Random.Range(0, Map.BLOCK_SIZE_Z - 1) + 0.5f;
			int num = Random.Range(0, 12);
			RaycastHit hitInfo;
			if (Physics.Raycast(new Vector3(x, 64f, z), Vector3.down, out hitInfo, 64f, Controll.layerMask))
			{
				GameObject item = Object.Instantiate(pgo[num], hitInfo.point, Quaternion.identity);
				golist.Add(item);
			}
		}
	}

	public static void Clear()
	{
		if (golist != null)
		{
			for (int i = 0; i < golist.Count; i++)
			{
				Object.Destroy(golist[i]);
			}
			golist.Clear();
		}
	}
}
