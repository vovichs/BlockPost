using UnityEngine;

public class DistanceDraw : MonoBehaviour
{
	private static int[] radius = new int[4] { 4, 8, 16, 32 };

	private static int RADIUS_RENDER = 32;

	private static int RADIUS_DATA = RADIUS_RENDER + 1;

	private static Transform tr;

	private static int offsetx = 0;

	private static int offsetz = 0;

	private static string s0 = "";

	private static string s1 = "";

	private Rect r0 = new Rect(8f, 8f, 512f, 32f);

	private Rect r1 = new Rect(8f, 40f, 512f, 32f);

	private Texture2D tRed;

	private void Awake()
	{
		tr = base.gameObject.transform;
		HMap.LoadTextureMap();
	}

	private void Update()
	{
		int num = 512;
		float num2 = tr.position.x;
		float num3 = tr.position.z;
		bool flag = false;
		if (num2 > (float)num)
		{
			num2 -= (float)num;
			offsetx -= num;
			flag = true;
		}
		else if (num2 < (float)(-num))
		{
			num2 += (float)num;
			offsetx += num;
			flag = true;
		}
		if (num3 > (float)num)
		{
			num3 -= (float)num;
			offsetz -= num;
			flag = true;
		}
		else if (num3 < (float)(-num))
		{
			num3 += (float)num;
			offsetz += num;
			flag = true;
		}
		if (flag)
		{
			tr.position = new Vector3(num2, tr.position.y, num3);
			GameObject.Find("Map").transform.position = new Vector3(offsetx, 0f, offsetz);
		}
		bool flag2 = false;
		int num4 = (int)((tr.position.x - (float)offsetx) / 64f);
		int num5 = (int)((tr.position.z - (float)offsetz) / 64f);
		for (int i = num4 - RADIUS_DATA; i < num4 + RADIUS_DATA; i++)
		{
			for (int j = num5 - RADIUS_DATA; j < num5 + RADIUS_DATA; j++)
			{
				if (i >= 0 && j >= 0 && i < 2 && j < 2 && TMap.chunk[i, j] == null)
				{
					TMap.LoadData(i, j);
				}
			}
		}
		for (int k = num4 - RADIUS_RENDER; k < num4 + RADIUS_RENDER; k++)
		{
			if (k < 0)
			{
				continue;
			}
			if (flag2)
			{
				break;
			}
			for (int l = num5 - RADIUS_RENDER; l < num5 + RADIUS_RENDER; l++)
			{
				if (k < 0 || l < 0 || k >= 2 || l >= 2 || TMap.chunk[k, l] == null)
				{
					continue;
				}
				int num6 = 4;
				for (int m = 0; m < radius.Length; m++)
				{
					if (Mathf.Abs(num4 - k) <= radius[m] && Mathf.Abs(num5 - l) <= radius[m])
					{
						num6 = m;
						break;
					}
				}
				int num7 = -1;
				if (num6 < 4)
				{
					if (l - num5 == radius[num6] && k - num4 == -radius[num6])
					{
						num7 = 7;
					}
					else if (l - num5 == radius[num6] && k - num4 == radius[num6])
					{
						num7 = 1;
					}
					else if (l - num5 == -radius[num6] && k - num4 == -radius[num6])
					{
						num7 = 5;
					}
					else if (l - num5 == -radius[num6] && k - num4 == radius[num6])
					{
						num7 = 3;
					}
					else if (l - num5 == radius[num6] && Mathf.Abs(num4 - k) < radius[num6])
					{
						num7 = 0;
					}
					else if (l - num5 == -radius[num6] && Mathf.Abs(num4 - k) < radius[num6])
					{
						num7 = 4;
					}
					else if (k - num4 == radius[num6] && Mathf.Abs(num5 - l) < radius[num6])
					{
						num7 = 2;
					}
					else if (k - num4 == -radius[num6] && Mathf.Abs(num5 - l) < radius[num6])
					{
						num7 = 6;
					}
				}
				if (TMap.chunk[k, l].go != null)
				{
					if (TMap.chunk[k, l].lodlevel != num6 || TMap.chunk[k, l].direction != num7)
					{
						if ((bool)TMap.chunk[k, l].mf.sharedMesh)
						{
							Object.Destroy(TMap.chunk[k, l].mf.sharedMesh);
							TMap.chunk[k, l].mf.sharedMesh = null;
						}
						if ((bool)TMap.chunk[k, l].mc.sharedMesh)
						{
							Object.Destroy(TMap.chunk[k, l].mc.sharedMesh);
							TMap.chunk[k, l].mc.sharedMesh = null;
						}
						if ((bool)TMap.chunk[k, l].currMesh)
						{
							Object.Destroy(TMap.chunk[k, l].currMesh);
							TMap.chunk[k, l].currMesh = null;
						}
						if (num7 >= 0)
						{
							TMap.CreateLODMerge(TMap.chunk[k, l], num6, num7);
						}
						else
						{
							TMap.CreateLOD(TMap.chunk[k, l], num6);
						}
						flag2 = true;
					}
				}
				else
				{
					TMap.Render(k, l);
					if (num7 >= 0)
					{
						TMap.CreateLODMerge(TMap.chunk[k, l], num6, num7);
					}
					else
					{
						TMap.CreateLOD(TMap.chunk[k, l], num6);
					}
					flag2 = true;
				}
			}
		}
		for (int n = num4 - (RADIUS_DATA + 1); n < num4 + (RADIUS_DATA + 1); n++)
		{
			if (n < 0)
			{
				continue;
			}
			for (int num8 = num5 - (RADIUS_DATA + 1); num8 < num5 + (RADIUS_DATA + 1); num8++)
			{
				if ((n < num4 - RADIUS_DATA || n >= num4 + RADIUS_DATA || num8 < num5 - RADIUS_DATA || num8 >= num5 + RADIUS_DATA) && n >= 0 && num8 >= 0 && n < 2 && num8 < 2 && TMap.chunk[n, num8] != null)
				{
					if (TMap.chunk[n, num8].mf != null)
					{
						Object.Destroy(TMap.chunk[n, num8].mf.sharedMesh);
						TMap.chunk[n, num8].mf.sharedMesh = null;
					}
					if (TMap.chunk[n, num8].mc != null)
					{
						Object.Destroy(TMap.chunk[n, num8].mc.sharedMesh);
						TMap.chunk[n, num8].mc.sharedMesh = null;
					}
					if ((bool)TMap.chunk[n, num8].currMesh)
					{
						Object.Destroy(TMap.chunk[n, num8].currMesh);
						TMap.chunk[n, num8].currMesh = null;
					}
					if ((bool)TMap.chunk[n, num8].go)
					{
						Object.Destroy(TMap.chunk[n, num8].go);
						TMap.chunk[n, num8].go = null;
					}
					TMap.chunk[n, num8] = null;
				}
			}
		}
		if (Time.frameCount % 30 == 0)
		{
			s0 = "chunk count: " + TMap.stats[0];
			s1 = "player position: " + (tr.position.x - (float)offsetx).ToString("0.00") + " " + tr.position.y.ToString("0.00") + " " + (tr.position.z - (float)offsetz).ToString("0.00");
		}
	}
}
