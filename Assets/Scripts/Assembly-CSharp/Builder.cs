using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
	public static Builder cs = null;

	public static bool active = false;

	public static int toolmode = 0;

	public static int current = 0;

	public static int currblock = 0;

	public static Vector3 blockCursor = Vector3.zero;

	public static GameObject goCursor = null;

	public static GameObject goCursor2 = null;

	private Vector3 a;

	private Vector3 b;

	private static Vector3 lastpos = Vector3.zero;

	private static Vector3 dir = Vector3.zero;

	private static bool firstblock = false;

	private static float buildtime = 0f;

	public static List<Map.CBlock> copyblock = new List<Map.CBlock>();

	public static Vector3 offset;

	public static GameObject goFillZone = null;

	public static GameObject goFillZone2 = null;

	public static int prefabpos = -1;

	public static void SetActive(bool val)
	{
		active = val;
		toolmode = 0;
	}

	private void Update()
	{
		if (active && !Main.show && !GUIGameMenu.show)
		{
			UpdateTab();
			UpdateDev();
			if (toolmode == 0)
			{
				UpdateCursor();
				UpdatePicker();
				UpdateBuildMode0();
				UpdateBuildMode1();
				UpdateBuildMode2();
				UpdateBuildMode3();
				UpdateBuildMode4();
				UpdateBlockSelect();
				UpdateToolSelect();
			}
			else
			{
				UpdateEntRotation();
				UpdateCursorEnt();
				UpdateEntSelect();
				UpdateEnt();
			}
		}
	}

	private void UpdateDev()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			Controll.freefly = !Controll.freefly;
			if (Controll.freefly)
			{
				Controll.SetPos(Controll.trControll.position + Controll.trControll.up);
			}
			else
			{
				Controll.SetPos(Controll.trControll.position);
			}
		}
	}

	public static void UpdateTab()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (toolmode == 0)
			{
				toolmode = 1;
			}
			else
			{
				toolmode = 0;
			}
			BuildCursor(false, -1);
		}
	}

	public static void UpdateFlyMode()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			Controll.speed = 20f;
		}
		else
		{
			Controll.speed = 10f;
		}
		if (Input.GetKey(GUIOptions.keyCrouch))
		{
			Controll.trControll.position = Controll.trControll.position + Controll.trControll.up * -1f * Controll.speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			Controll.trControll.position = Controll.trControll.position + Controll.trControll.up * Controll.speed * Time.deltaTime;
		}
		float num = 0f;
		float num2 = 0f;
		if (Input.GetKey(GUIOptions.keyForward) || Input.GetKey(KeyCode.UpArrow))
		{
			num = 1f;
		}
		else if (Input.GetKey(GUIOptions.keyBackward) || Input.GetKey(KeyCode.DownArrow))
		{
			num = -1f;
		}
		if (Input.GetKey(GUIOptions.keyStrafeRight) || Input.GetKey(KeyCode.RightArrow))
		{
			num2 = 1f;
		}
		else if (Input.GetKey(GUIOptions.keyStrafeLeft) || Input.GetKey(KeyCode.LeftArrow))
		{
			num2 = -1f;
		}
		Vector3 normalized = (Controll.trControll.forward * num + Controll.trControll.right * num2).normalized;
		Controll.nextPos = Controll.trControll.position + normalized * Controll.speed * Time.deltaTime;
		Controll.trControll.position = Controll.ClampPosMap(Controll.nextPos);
	}

	private void UpdatePicker()
	{
		if (!Input.GetKey(KeyCode.F))
		{
			return;
		}
		Vector3 pos = Vector3.zero;
		if (GetBlock(ref pos, false))
		{
			int block = Map.GetBlock(pos);
			if (block > 0)
			{
				currblock = block - 1;
				Palette.SetColor(Map.GetLastColor());
				BuildCursor(false, -1);
			}
		}
	}

	private void UpdateEnt()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (goCursor2 == null)
			{
				return;
			}
			if (Time.time > buildtime)
			{
				buildtime = Time.time + 0.1f;
				if (Map.GetBlock(blockCursor) != 0)
				{
					return;
				}
				Map.SetEnt((int)blockCursor.x, (int)blockCursor.y, (int)blockCursor.z, (int)goCursor2.transform.eulerAngles.y, current + 1);
			}
		}
		if (Input.GetKey(KeyCode.Mouse1))
		{
			int ent = Map.GetEnt((int)blockCursor.x, (int)blockCursor.y, (int)blockCursor.z);
			if (ent >= 0)
			{
				Map.DelEnt(ent);
			}
		}
	}

	private void UpdateEntRotation()
	{
		if (Input.GetKeyUp(KeyCode.Q))
		{
			float num = goCursor2.transform.eulerAngles.y + 90f;
			if (num > 360f)
			{
				num -= 360f;
			}
			goCursor2.transform.eulerAngles = new Vector3(0f, num, 0f);
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			float num2 = goCursor2.transform.eulerAngles.y - 90f;
			if (num2 < 0f)
			{
				num2 += 360f;
			}
			goCursor2.transform.eulerAngles = new Vector3(0f, num2, 0f);
		}
	}

	private void UpdateBuildMode0()
	{
		if (Controll.trControll == null || Controll.toolmode != 0 || Controll.lockAttack)
		{
			return;
		}
		if (Input.GetKey(KeyCode.Mouse0) && Time.time > buildtime)
		{
			firstblock = false;
			if (buildtime == 0f)
			{
				buildtime = Time.time + 0.3f;
				firstblock = true;
			}
			else
			{
				buildtime = Time.time + 0.025f;
			}
			Controll.buildmode = 1;
			if (firstblock)
			{
				dir = Controll.trControll.transform.position - blockCursor;
				dir = dir.normalized;
				if (dir.x < -0.75f)
				{
					dir.x = -1f;
				}
				else if (dir.x > 0.75f)
				{
					dir.x = 1f;
				}
				else
				{
					dir.x = 0f;
				}
				if (dir.y < -0.75f)
				{
					dir.y = -1f;
				}
				else if (dir.y > 0.75f)
				{
					dir.y = 1f;
				}
				else
				{
					dir.y = 0f;
				}
				if (dir.z < -0.75f)
				{
					dir.z = -1f;
				}
				else if (dir.z > 0.75f)
				{
					dir.z = 1f;
				}
				else
				{
					dir.z = 0f;
				}
				lastpos = blockCursor;
			}
			else
			{
				blockCursor = lastpos + dir;
				lastpos = new Vector3(blockCursor.x, blockCursor.y, blockCursor.z);
			}
			if (Map.GetBlock(blockCursor) == 0 && VUtil.isValidBlockPlace(Controll.trControll.position, 0.45f, Controll.boxheight, blockCursor))
			{
				Map.SetBlock(blockCursor.x, blockCursor.y, blockCursor.z, Palette.GetColor(), currblock + 1);
				Map.RenderBlock((int)blockCursor.x, (int)blockCursor.y, (int)blockCursor.z);
			}
			BuildCursor(false, -1);
		}
		if (Input.GetKey(KeyCode.Mouse1) && Time.time > buildtime && toolmode == 0)
		{
			if (buildtime == 0f)
			{
				buildtime = Time.time + 0.3f;
			}
			else
			{
				buildtime = Time.time + 0.025f;
			}
			Controll.buildmode = 0;
			if (Map.GetBlock(blockCursor) > 0)
			{
				Map.SetBlock(blockCursor.x, blockCursor.y, blockCursor.z, Color.white, 0);
				Map.RenderBlock((int)blockCursor.x, (int)blockCursor.y, (int)blockCursor.z);
			}
			BuildCursor(true, 40);
			goCursor2.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
			if (goCursor2.GetComponent<FXScale>() != null)
			{
				goCursor2.GetComponent<FXScale>().maxscale = 1.1f;
				goCursor2.GetComponent<FXScale>().minscale = 1.05f;
			}
		}
		if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1))
		{
			buildtime = 0f;
		}
	}

	private void UpdateBuildMode1()
	{
		if (Controll.toolmode != 1 || Controll.lockAttack)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 pos = Vector3.zero;
			if (!GetBlock(ref pos, true))
			{
				return;
			}
			if (a.x < 0f)
			{
				a = pos;
				CreateFillZone(pos, new Color(1f, 1f, 1f, 0.75f));
				DestroyCursor();
			}
			else
			{
				b = pos;
				int[] array = new int[2]
				{
					(int)a.x,
					(int)b.x
				};
				int[] array2 = new int[2]
				{
					(int)a.y,
					(int)b.y
				};
				int[] array3 = new int[2]
				{
					(int)a.z,
					(int)b.z
				};
				if (array[0] > array[1])
				{
					int num = array[0];
					array[0] = array[1];
					array[1] = num;
				}
				if (array2[0] > array2[1])
				{
					int num2 = array2[0];
					array2[0] = array2[1];
					array2[1] = num2;
				}
				if (array3[0] > array3[1])
				{
					int num3 = array3[0];
					array3[0] = array3[1];
					array3[1] = num3;
				}
				for (int i = array[0]; i <= array[1]; i++)
				{
					for (int j = array2[0]; j <= array2[1]; j++)
					{
						for (int k = array3[0]; k <= array3[1]; k++)
						{
							Map.SetBlock(i, j, k, Palette.GetColor(), currblock + 1);
						}
					}
				}
				Map.RenderDirty();
				a = new Vector3(-1f, 0f, 0f);
				DestroyFillZone();
				BuildCursor(true, -1);
			}
		}
		else if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			a = new Vector3(-1f, 0f, 0f);
			DestroyFillZone();
			BuildCursor(true, -1);
		}
		if (a.x >= 0f)
		{
			UpdateFillZone(blockCursor);
		}
	}

	private void UpdateBuildMode2()
	{
		if (Controll.toolmode != 2 || Controll.lockAttack)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 pos = Vector3.zero;
			if (!GetBlock(ref pos, false))
			{
				return;
			}
			if (a.x < 0f)
			{
				a = pos;
				CreateFillZone(pos, new Color(1f, 0f, 0f, 0.75f));
				DestroyCursor();
			}
			else
			{
				b = pos;
				int[] array = new int[2]
				{
					(int)a.x,
					(int)b.x
				};
				int[] array2 = new int[2]
				{
					(int)a.y,
					(int)b.y
				};
				int[] array3 = new int[2]
				{
					(int)a.z,
					(int)b.z
				};
				if (array[0] > array[1])
				{
					int num = array[0];
					array[0] = array[1];
					array[1] = num;
				}
				if (array2[0] > array2[1])
				{
					int num2 = array2[0];
					array2[0] = array2[1];
					array2[1] = num2;
				}
				if (array3[0] > array3[1])
				{
					int num3 = array3[0];
					array3[0] = array3[1];
					array3[1] = num3;
				}
				for (int i = array[0]; i <= array[1]; i++)
				{
					for (int j = array2[0]; j <= array2[1]; j++)
					{
						for (int k = array3[0]; k <= array3[1]; k++)
						{
							Map.SetBlock(i, j, k, Color.white, 0);
						}
					}
				}
				Map.RenderDirty();
				a = new Vector3(-1f, 0f, 0f);
				DestroyFillZone();
				BuildCursor(true, 40);
				goCursor2.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
				goCursor2.GetComponent<FXScale>().maxscale = 1.1f;
				goCursor2.GetComponent<FXScale>().minscale = 1.05f;
			}
		}
		else if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			a = new Vector3(-1f, 0f, 0f);
			DestroyFillZone();
			BuildCursor(true, 40);
			goCursor2.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
			goCursor2.GetComponent<FXScale>().maxscale = 1.1f;
			goCursor2.GetComponent<FXScale>().minscale = 1.05f;
		}
		if (a.x >= 0f)
		{
			UpdateFillZone(blockCursor);
		}
	}

	private void UpdateBuildMode3()
	{
		if (Controll.toolmode != 3 || Controll.lockAttack)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 pos = Vector3.zero;
			if (!GetBlock(ref pos, false))
			{
				return;
			}
			if (copyblock.Count != 0)
			{
				a = new Vector3(-1f, 0f, 0f);
				DestroyFillZone();
				BuildCursor(true, 40);
				goCursor2.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;
				goCursor2.GetComponent<FXScale>().maxscale = 1.1f;
				goCursor2.GetComponent<FXScale>().minscale = 1.05f;
				copyblock.Clear();
				Controll.buildmode = 0;
				return;
			}
			if (a.x < 0f)
			{
				a = pos;
				CreateFillZone(pos, new Color(1f, 0f, 0f, 0.75f));
				DestroyCursor();
			}
			else
			{
				b = pos;
				int[] array = new int[2]
				{
					(int)a.x,
					(int)b.x
				};
				int[] array2 = new int[2]
				{
					(int)a.y,
					(int)b.y
				};
				int[] array3 = new int[2]
				{
					(int)a.z,
					(int)b.z
				};
				if (array[0] > array[1])
				{
					int num = array[0];
					array[0] = array[1];
					array[1] = num;
				}
				if (array2[0] > array2[1])
				{
					int num2 = array2[0];
					array2[0] = array2[1];
					array2[1] = num2;
				}
				if (array3[0] > array3[1])
				{
					int num3 = array3[0];
					array3[0] = array3[1];
					array3[1] = num3;
				}
				copyblock.Clear();
				offset = new Vector3(array[0], array2[0], array3[0]);
				for (int i = array[0]; i <= array[1]; i++)
				{
					for (int j = array2[0]; j <= array2[1]; j++)
					{
						for (int k = array3[0]; k <= array3[1]; k++)
						{
							int block = Map.GetBlock(i, j, k);
							if (block != 0)
							{
								Color lastColor = Map.GetLastColor();
								copyblock.Add(new Map.CBlock(i, j, k, block, lastColor));
							}
						}
					}
				}
				a = new Vector3(-1f, 0f, 0f);
				DestroyFillZone();
				BuildCursorPrefab();
				RenderPrefab();
				Controll.buildmode = 1;
			}
		}
		else if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			int num4 = (int)blockCursor.x;
			int num5 = (int)blockCursor.y;
			int num6 = (int)blockCursor.z;
			for (int l = 0; l < copyblock.Count; l++)
			{
				Map.SetBlock((float)(copyblock[l].x + num4) - offset.x, (float)(copyblock[l].y + num5) - offset.y, (float)(copyblock[l].z + num6) - offset.z, copyblock[l].color, copyblock[l].block);
			}
			Map.RenderDirty();
		}
		if (a.x >= 0f)
		{
			UpdateFillZone(blockCursor);
		}
	}

	private void RenderPrefab()
	{
		MeshBuilder.Create();
		for (int i = 0; i < copyblock.Count; i++)
		{
			BlockBuilder.BuildBox(new Vector3((float)copyblock[i].x - offset.x, (float)copyblock[i].y - offset.y, (float)copyblock[i].z - offset.z), 1f, 1f, 1f, copyblock[i].color);
		}
		GameObject obj = new GameObject();
		obj.name = "prefab";
		obj.transform.parent = goCursor2.transform.parent;
		obj.transform.localPosition = Vector3.zero;
		MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = new Material(ContentLoader_.LoadMaterial("vertex_color"));
		meshRenderer.sharedMaterial.SetTexture("_MainTex", null);
		meshFilter.sharedMesh = MeshBuilder.ToMesh();
	}

	private void UpdateToolSelect()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Controll.toolmode = 0;
			Controll.buildmode = 1;
			a = new Vector3(-1f, 0f, 0f);
			DestroyFillZone();
			copyblock.Clear();
			BuildCursor(false, -1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Controll.toolmode = 1;
			Controll.buildmode = 1;
			a = new Vector3(-1f, 0f, 0f);
			DestroyFillZone();
			copyblock.Clear();
			BuildCursor(false, -1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Controll.toolmode = 2;
			Controll.buildmode = 0;
			a = new Vector3(-1f, 0f, 0f);
			DestroyFillZone();
			copyblock.Clear();
			BuildCursor(true, 40);
			goCursor2.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
			goCursor2.GetComponent<FXScale>().maxscale = 1.1f;
			goCursor2.GetComponent<FXScale>().minscale = 1.05f;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			Controll.toolmode = 3;
			Controll.buildmode = 0;
			a = new Vector3(-1f, 0f, 0f);
			DestroyFillZone();
			copyblock.Clear();
			BuildCursor(true, 40);
			goCursor2.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;
			goCursor2.GetComponent<FXScale>().maxscale = 1.1f;
			goCursor2.GetComponent<FXScale>().minscale = 1.05f;
		}
	}

	private void UpdateBlockSelect()
	{
		if (toolmode != 0 || Controll.toolmode != 0)
		{
			return;
		}
		float num = Input.GetAxis("Mouse ScrollWheel");
		if (Input.GetKeyUp(KeyCode.Z))
		{
			num = 1f;
		}
		if (Input.GetKeyUp(KeyCode.X))
		{
			num = -1f;
		}
		if (num > 0f)
		{
			currblock--;
			if (currblock < 0)
			{
				currblock = 0;
			}
			Controll.buildmode = 1;
			UpdateCursor();
			BuildCursor(false, -1);
		}
		else if (num < 0f)
		{
			currblock++;
			if (currblock >= 52)
			{
				currblock = 51;
			}
			Controll.buildmode = 1;
			UpdateCursor();
			BuildCursor(false, -1);
		}
	}

	private void UpdateEntSelect()
	{
		if (toolmode != 1)
		{
			return;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			current--;
			if (current < 0)
			{
				current = 0;
			}
			BuildCursor(false, -1);
		}
		else if (axis < 0f)
		{
			current++;
			if (current >= 2)
			{
				current = 1;
			}
			BuildCursor(false, -1);
		}
	}

	private bool GetBlock(ref Vector3 pos, bool outside)
	{
		if (Controll.trCamera == null)
		{
			return false;
		}
		RaycastHit hit;
		if (!Controll.Raycast(Controll.trCamera.transform.position, Controll.trCamera.transform.forward, out hit))
		{
			return false;
		}
		if (outside)
		{
			hit.point += hit.normal * 0.5f;
		}
		else
		{
			hit.point += hit.normal * -0.5f;
		}
		pos.x = (int)hit.point.x;
		pos.y = (int)hit.point.y;
		pos.z = (int)hit.point.z;
		return true;
	}

	private void UpdateCursor()
	{
		if (Controll.trCamera == null || Controll.lockAttack)
		{
			return;
		}
		RaycastHit hit;
		if (!Controll.Raycast(Controll.trCamera.transform.position, Controll.trCamera.transform.forward, out hit))
		{
			if (goCursor != null)
			{
				goCursor.transform.position = Vector3.zero;
			}
			return;
		}
		if (Controll.buildmode == 1)
		{
			hit.point += hit.normal * 0.5f;
		}
		else
		{
			hit.point += hit.normal * -0.5f;
		}
		float num = (int)hit.point.x;
		float num2 = (int)hit.point.y;
		float num3 = (int)hit.point.z;
		if (Controll.toolmode == 0 && !firstblock && buildtime != 0f && Controll.buildmode == 1)
		{
			num = lastpos.x;
			num2 = lastpos.y;
			num3 = lastpos.z;
		}
		if (Controll.buildmode == 0 && Map.GetBlock(new Vector3(num + 0.5f, num2 + 0.5f, num3 + 0.5f)) == 0)
		{
			if (goCursor != null)
			{
				goCursor.transform.position = Vector3.zero;
			}
			return;
		}
		if (!VUtil.isValidBlockPlace(Controll.trControll.transform.position, 0.45f, Controll.boxheight, new Vector3(num + 0.5f, num2 + 0.5f, num3 + 0.5f)))
		{
			if (goCursor != null)
			{
				goCursor.transform.position = Vector3.zero;
			}
			return;
		}
		if (goCursor != null)
		{
			goCursor.transform.position = new Vector3(num, num2, num3);
			if (Controll.buildmode == 0)
			{
				goCursor.transform.position = new Vector3(num - 0.01f, num2 - 0.01f, num3 - 0.01f);
			}
		}
		blockCursor = new Vector3(num, num2, num3);
	}

	public static void CreateFillZone(Vector3 pos, Color c)
	{
		if (goFillZone != null)
		{
			goFillZone.name = "dead_" + Time.time;
			Object.Destroy(goFillZone);
		}
		goFillZone = new GameObject();
		goFillZone.transform.position = pos;
		GameObject obj = new GameObject();
		obj.transform.parent = goFillZone.transform;
		obj.transform.localPosition = Vector3.zero;
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
		meshRenderer.sharedMaterial = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
		meshRenderer.sharedMaterial.color = c;
		MeshBuilder.Create();
		BlockBuilder.BuildFree(41, Vector3.zero, Palette.GetColor());
		meshFilter.sharedMesh = MeshBuilder.ToMesh();
		meshFilter.sharedMesh.RecalculateNormals(0f);
		TangentSolver.Solve(meshFilter.sharedMesh);
		goFillZone2 = obj;
	}

	public static void UpdateFillZone(Vector3 pos)
	{
		if (goFillZone == null)
		{
			return;
		}
		Vector3 vector = pos - goFillZone.transform.position;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < 3; i++)
		{
			if (vector[i] >= 0f)
			{
				vector[i] += 1f;
				vector[i] += 0.02f;
				zero[i] -= 0.01f;
			}
			else
			{
				vector[i] -= 1f;
				zero[i] += 1f;
				vector[i] -= 0.02f;
				zero[i] += 0.01f;
			}
		}
		goFillZone2.transform.localPosition = new Vector3(zero.x, zero.y, zero.z);
		goFillZone2.transform.localScale = new Vector3(vector.x, vector.y, vector.z);
	}

	public static void DestroyFillZone()
	{
		if (!(goFillZone == null))
		{
			goFillZone.name = "dead_" + Time.time;
			Object.Destroy(goFillZone);
		}
	}

	public static void BuildCursor(bool force, int block)
	{
		if (goFillZone != null && !force)
		{
			return;
		}
		if (block < 0)
		{
			block = currblock + 1;
		}
		MeshRenderer meshRenderer = null;
		if (goCursor != null)
		{
			Object.Destroy(goCursor.AddComponent<MeshFilter>().sharedMesh);
			goCursor.name = "dead_" + Time.time;
			Object.Destroy(goCursor);
		}
		goCursor = new GameObject();
		goCursor.name = "cursor_" + Time.time;
		goCursor.transform.position = new Vector3(blockCursor.x, blockCursor.y, blockCursor.z);
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = goCursor.transform;
		gameObject.transform.localPosition = Vector3.zero;
		if (toolmode == 0)
		{
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			meshRenderer.sharedMaterial = new Material(Atlas.mat);
			meshRenderer.sharedMaterial.SetTexture("_MainTex", Atlas.texmark);
			MeshBuilder.Create();
			BlockBuilder.BuildFree(block, Vector3.zero, Palette.GetColor());
			meshFilter.sharedMesh = MeshBuilder.ToMesh();
			gameObject.AddComponent<FXScale>();
		}
		else
		{
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
			MeshFilter meshFilter2 = gameObject.AddComponent<MeshFilter>();
			meshRenderer.sharedMaterial = new Material(ContentLoader_.LoadMaterial("vertex_color"));
			meshRenderer.sharedMaterial.SetTexture("_MainTex", null);
			MeshBuilder.Create();
			BlockBuilder.SetPivot(new Vector3(0.5f, 0f, 0.5f));
			gameObject.transform.localPosition = new Vector3(0.5f, 0f, 0.5f);
			if (current + 1 == 1)
			{
				BlockBuilder.BuildBox(Vector3.zero, 1f, 2.75f, 1f, Color.red);
			}
			else if (current + 1 == 2)
			{
				BlockBuilder.BuildBox(Vector3.zero, 1f, 2.75f, 1f, Color.blue);
			}
			BlockBuilder.BuildBox(new Vector3(0.375f, 2.375f, 1f), 0.25f, 0.25f, 0.25f, Color.yellow);
			meshFilter2.sharedMesh = MeshBuilder.ToMesh();
		}
		goCursor2 = gameObject;
	}

	public static void DestroyCursor()
	{
		if (!(goCursor == null))
		{
			goCursor.name = "dead_" + Time.time;
			Object.Destroy(goCursor);
		}
	}

	private void UpdateCursorEnt()
	{
		if (Controll.trCamera == null || Controll.lockAttack)
		{
			return;
		}
		RaycastHit hit;
		if (!Controll.Raycast(Controll.trCamera.transform.position, Controll.trCamera.transform.forward, out hit))
		{
			if (goCursor != null)
			{
				goCursor.transform.position = Vector3.zero;
			}
			return;
		}
		hit.point += hit.normal * 0.5f;
		float num = (int)hit.point.x;
		float num2 = (int)hit.point.y;
		float num3 = (int)hit.point.z;
		if (!VUtil.isValidBlockPlace(Controll.trControll.transform.position, 0.45f, Controll.boxheight, new Vector3(num + 0.5f, num2 + 0.5f, num3 + 0.5f)))
		{
			if (goCursor != null)
			{
				goCursor.transform.position = Vector3.zero;
			}
			return;
		}
		if (goCursor != null)
		{
			goCursor.transform.position = new Vector3(num, num2, num3);
		}
		blockCursor = new Vector3(num, num2, num3);
	}

	private void UpdateBuildMode4()
	{
		if (Controll.toolmode == 4 && !Controll.lockAttack)
		{
			UpdateLoadPrefab();
			UpdateSetPrefab();
			UpdateRotatePrefab();
		}
	}

	private void UpdateLoadPrefab()
	{
		float axis = Input.GetAxis("Mouse ScrollWheel");
		axis *= -1f;
		if (Input.GetKeyUp(KeyCode.Z))
		{
			axis = 1f;
		}
		if (Input.GetKeyUp(KeyCode.X))
		{
			axis = -1f;
		}
		bool flag = false;
		if (axis > 0f)
		{
			if (MapPrefab.p.Count == 0)
			{
				return;
			}
			prefabpos++;
			flag = true;
			if (prefabpos >= MapPrefab.p.Count)
			{
				prefabpos = MapPrefab.p.Count - 1;
			}
		}
		else if (axis < 0f)
		{
			if (MapPrefab.p.Count == 0)
			{
				return;
			}
			prefabpos--;
			flag = true;
			if (prefabpos < 0)
			{
				prefabpos = 0;
			}
		}
		if (flag)
		{
			MapPrefab.LoadBin(MapPrefab.p[prefabpos].prefabname, ref copyblock);
			offset = Vector3.zero;
			BuildCursorPrefab();
			RenderPrefab();
		}
	}

	private void BuildCursorPrefab()
	{
		BuildCursor(true, 40);
		goCursor2.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;
		goCursor2.GetComponent<FXScale>().maxscale = 1.1f;
		goCursor2.GetComponent<FXScale>().minscale = 1.05f;
	}

	private void UpdateSetPrefab()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			int num = (int)blockCursor.x;
			int num2 = (int)blockCursor.y;
			int num3 = (int)blockCursor.z;
			for (int i = 0; i < copyblock.Count; i++)
			{
				Map.SetBlock((float)(copyblock[i].x + num) - offset.x, (float)(copyblock[i].y + num2) - offset.y, (float)(copyblock[i].z + num3) - offset.z, copyblock[i].color, copyblock[i].block);
			}
			Map.RenderDirty();
		}
	}

	private void UpdateRotatePrefab()
	{
		if (Input.GetKeyUp(KeyCode.Q))
		{
			MapPrefab.Rotate(ref copyblock, false);
			offset = Vector3.zero;
			BuildCursorPrefab();
			RenderPrefab();
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			MapPrefab.Rotate(ref copyblock, true);
			offset = Vector3.zero;
			BuildCursorPrefab();
			RenderPrefab();
		}
	}
}
