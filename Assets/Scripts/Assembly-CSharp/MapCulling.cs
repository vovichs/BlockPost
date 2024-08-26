using UnityEngine;

public class MapCulling : MonoBehaviour
{
	private string[] sCull = new string[2] { "<b>CULL OFF</b>", "<b>CULL ON</b>" };

	private string sChunkObjects = "N/A";

	private int cullon;

	private void OnDrawGizmosSelected()
	{
		if (Map.chunk == null)
		{
			return;
		}
		int num = (int)(Controll.nextPos.x / 16f);
		int num2 = (int)(Controll.nextPos.y / 16f);
		int num3 = (int)(Controll.nextPos.z / 16f);
		for (int i = 0; i < Map.MAP_SIZE_X; i++)
		{
			for (int j = 0; j < Map.MAP_SIZE_Z; j++)
			{
				for (int k = 0; k < Map.MAP_SIZE_Y; k++)
				{
					if (Map.chunk[i, k, j] != null)
					{
						Map.CChunk cChunk = Map.chunk[i, k, j];
						if (cChunk.x == num && cChunk.y == num2 && cChunk.z == num3)
						{
							Gizmos.color = new Color(1f, 1f, 0f, 0.75f);
							Gizmos.DrawCube(new Vector3(cChunk.x * 16, cChunk.y * 16, cChunk.z * 16) + new Vector3(8f, 8f, 8f), new Vector3(16f, 16f, 16f));
						}
						if (cChunk.count == 4096)
						{
							Gizmos.color = new Color(1f, 0f, 0f, 0.75f);
							Gizmos.DrawCube(new Vector3(cChunk.x * 16, cChunk.y * 16, cChunk.z * 16) + new Vector3(8f, 8f, 8f), new Vector3(16f, 16f, 16f));
						}
					}
				}
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.L))
		{
			if (cullon > 0)
			{
				cullon = 0;
				CullOff();
				CalcChunksObjects();
			}
			else
			{
				cullon = 1;
				CullMap();
				CalcChunksObjects();
			}
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect((float)Screen.width / 2f - 40f, Screen.height - 100, 200f, 20f), sCull[cullon]);
		GUI.Label(new Rect((float)Screen.width / 2f - 40f, Screen.height - 80, 200f, 20f), sChunkObjects);
	}

	private void CullOff()
	{
		if (Map.chunk == null)
		{
			return;
		}
		for (int i = 0; i < Map.MAP_SIZE_X; i++)
		{
			for (int j = 0; j < Map.MAP_SIZE_Z; j++)
			{
				for (int k = 0; k < Map.MAP_SIZE_Y; k++)
				{
					if (Map.chunk[i, k, j] != null)
					{
						Map.CChunk cChunk = Map.chunk[i, k, j];
						if (cChunk.go != null)
						{
							cChunk.go.SetActive(true);
						}
					}
				}
			}
		}
	}

	private void CullMap()
	{
		if (Map.chunk == null)
		{
			return;
		}
		int num = (int)(Controll.nextPos.x / 16f);
		int num2 = (int)(Controll.nextPos.y / 16f);
		int num3 = (int)(Controll.nextPos.z / 16f);
		(new int[2])[1] = Map.MAP_SIZE_X - 1;
		(new int[2])[1] = Map.MAP_SIZE_Y - 1;
		int[] array = new int[2]
		{
			0,
			Map.MAP_SIZE_Z - 1
		};
		for (int i = num3 + 1; i < Map.MAP_SIZE_Z - 1; i++)
		{
			if (Map.chunk[num, num2, i] != null && Map.chunk[num, num2, i].count == 4096)
			{
				array[1] = i;
				break;
			}
		}
		for (int num4 = num3 - 1; num4 > 0; num4--)
		{
			if (Map.chunk[num, num2, num4] != null && Map.chunk[num, num2, num4].count == 4096)
			{
				array[0] = num4;
				break;
			}
		}
		for (int j = array[0]; j < array[1]; j++)
		{
			bool flag = false;
			for (int k = num + 1; k < Map.MAP_SIZE_X; k++)
			{
				if (Map.chunk[k, num2, j] == null)
				{
					continue;
				}
				if (flag)
				{
					if (Map.chunk[k, num2, j].go != null)
					{
						Map.chunk[k, num2, j].go.SetActive(false);
					}
				}
				else if (Map.chunk[k, num2, j].count == 4096)
				{
					flag = true;
				}
			}
		}
	}

	private void CalcChunksObjects()
	{
		int num = 0;
		for (int i = 0; i < Map.MAP_SIZE_X; i++)
		{
			for (int j = 0; j < Map.MAP_SIZE_Z; j++)
			{
				for (int k = 0; k < Map.MAP_SIZE_Y; k++)
				{
					if (Map.chunk[i, k, j] != null && Map.chunk[i, k, j].go != null && Map.chunk[i, k, j].go.activeSelf)
					{
						num++;
					}
				}
			}
		}
		sChunkObjects = "<color=green>CHUNK_OBJECTS: " + num + "</color>";
	}
}
