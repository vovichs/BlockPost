using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Rendering;

public class ScopeGen : MonoBehaviour
{
	public class CBlock
	{
		public int x;

		public int y;

		public int x2;

		public int y2;

		public int w;

		public int h;

		public CBlock(int x, int y, int w, int h)
		{
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
			x2 = x + w;
			y2 = y + h;
		}

		public bool Contain(int x, int y)
		{
			if (x >= this.x && x < x2 && y >= this.y && y < y2)
			{
				return true;
			}
			return false;
		}

		~CBlock()
		{
		}
	}

	public class CScope
	{
		public string name_;

		public string fullname;

		public int lensoffset;

		public int scopetype;

		public int fov;

		public CScope(string name_, string fullname, int scopetype, int fov, int lensoffset)
		{
			this.name_ = name_;
			this.fullname = fullname;
			this.scopetype = scopetype;
			this.lensoffset = lensoffset;
			this.fov = fov;
		}

		~CScope()
		{
		}
	}

	private static Texture2D tTex;

	private static Texture2D tMask;

	private static int width;

	private static int height;

	private static int depth;

	private static Color c;

	private static Color cm;

	private static MeshFilter mf;

	private static MeshRenderer mr;

	private static GameObject goPivot = null;

	public static List<CBlock> bside = new List<CBlock>();

	public static List<CBlock> bsidepoint = new List<CBlock>();

	public static List<CBlock> btop = new List<CBlock>();

	public static List<CBlock> bdown = new List<CBlock>();

	public static List<CBlock> blens = new List<CBlock>();

	public static CScope[] scope = null;

	private void Start()
	{
	}

	public static void Init()
	{
		if (scope == null)
		{
			scope = new CScope[16];
			scope[0] = null;
			scope[1] = new CScope("s_sniper_s", "PM", 1, 20, 0);
			scope[2] = new CScope("acog_s", "ACOG", 0, 35, 0);
			scope[3] = new CScope("p_pka_s", "PKA", 2, 42, 0);
			scope[4] = new CScope("eotech_s", "EOTECH", 0, 45, 2);
			scope[5] = new CScope("mars_s", "MARS", 0, 55, 0);
			scope[6] = new CScope("pso_s", "PSO", 1, 28, 0);
			scope[7] = new CScope("trijicon_rmr_s", "RMR", 0, 60, 2);
			scope[8] = new CScope("tasco_s", "TASCO", 0, 40, 0);
			scope[9] = new CScope("sightmark_s", "SIGHTMARK", 0, 50, 6);
			scope[10] = new CScope("s_barska_s", "BARSKA", 1, 15, 0);
			scope[11] = new CScope("s_ballistic_s", "BALLISTIC", 1, 25, 0);
			scope[12] = new CScope("s_m7_s", "M7", 1, 30, 0);
			scope[13] = null;
			scope[14] = null;
			scope[15] = new CScope("s_pu_s", "PU", 1, 27, 0);
		}
	}

	public static GameObject Build(int scopeid)
	{
		if (scope[scopeid] == null)
		{
			return null;
		}
		return Generate(scope[scopeid].name_, scope[scopeid].lensoffset);
	}

	public static GameObject GetPivot()
	{
		return goPivot;
	}

	public static void BuildScope(int scopeid, WeaponData wd)
	{
		if (!(wd.goWeapon == null))
		{
			GameObject gameObject = Build(scopeid);
			GameObject pivot = GetPivot();
			if ((bool)gameObject)
			{
				gameObject.transform.parent = wd.goWeapon.transform;
				gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				gameObject.transform.localPosition = wd.goBoneScope.transform.localPosition - pivot.transform.localPosition * 0.5f + Vector3.forward * 0.5f;
				gameObject.transform.localEulerAngles = Vector3.zero;
				wd.goScope = gameObject;
			}
		}
	}

	public static GameObject BuildScope(int scopeid, GameObject goWeapon, GameObject goBoneScope)
	{
		if (scopeid == 0)
		{
			return null;
		}
		GameObject gameObject = Build(scopeid);
		GameObject pivot = GetPivot();
		if ((bool)gameObject)
		{
			gameObject.transform.parent = goWeapon.transform;
			gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			gameObject.transform.localPosition = goBoneScope.transform.localPosition - pivot.transform.localPosition * 0.5f + Vector3.forward * 0.5f;
			gameObject.transform.localEulerAngles = Vector3.zero;
		}
		return gameObject;
	}

	private static GameObject Generate(string _name, int lensoffset = 0)
	{
		tTex = Resources.Load("scopes/" + _name) as Texture2D;
		if (tTex == null)
		{
			tTex = ContentLoader_.LoadTexture(_name) as Texture2D;
		}
		tMask = Resources.Load("scopes/" + _name + "_mask") as Texture2D;
		if (tMask == null)
		{
			tMask = ContentLoader_.LoadTexture(_name + "_mask") as Texture2D;
		}
		width = tTex.width;
		height = tTex.height;
		depth = 0;
		bside.Clear();
		btop.Clear();
		bdown.Clear();
		bsidepoint.Clear();
		blens.Clear();
		FindBlocks2(bside, Color.red);
		FindBlocks2(btop, new Color(1f, 1f, 0f), bside);
		FindBlocks2(bdown, Color.magenta, bside);
		FindBlocks2(bsidepoint, Color.green);
		FindBlocks2(blens, Color.blue);
		GameObject gameObject = CreateGameObject();
		mr.material.SetTexture("_MainTex", tTex);
		BlockBuilder.SetTexCoord(0, width, height);
		float num = height;
		for (int i = 0; i < bside.Count; i++)
		{
			if (_name == "s_barska_s")
			{
				BuildBlock(bside[i], btop[i], bdown[i], (i >= bside.Count - 5 && blens.Count >= 2) ? blens[1] : null, (i == lensoffset && blens.Count >= 1) ? blens[0] : null);
			}
			else
			{
				BuildBlock(bside[i], btop[i], bdown[i], (i == bside.Count - 1 && blens.Count >= 2) ? blens[1] : null, (i == lensoffset && blens.Count >= 1) ? blens[0] : null);
			}
			if ((float)bside[i].y < num)
			{
				num = bside[i].y;
			}
		}
		for (int j = 0; j < bsidepoint.Count; j++)
		{
			BuildBlock(bsidepoint[j], bsidepoint[j], bsidepoint[j], null, null);
			if ((float)bsidepoint[j].y < num)
			{
				num = bsidepoint[j].y;
			}
		}
		goPivot = new GameObject();
		goPivot.transform.localPosition = new Vector3(0f, num, (float)depth / 2f);
		goPivot.transform.parent = gameObject.transform;
		mf.sharedMesh = MeshBuilder.ToMesh();
		return gameObject;
	}

	private static GameObject CreateGameObject()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "scope_" + gameObject.GetInstanceID();
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		mf = gameObject.AddComponent<MeshFilter>();
		mr = gameObject.AddComponent<MeshRenderer>();
		mr.material = new Material(ContentLoader_.LoadMaterial("vertex_color"));
		mr.shadowCastingMode = ShadowCastingMode.Off;
		mr.material.SetTexture("_MainTex", null);
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(0f, 0f, 0f));
		BlockBuilder.SetTexCoord(0, 1, 1);
		return gameObject;
	}

	private static void FindBlocks2(List<CBlock> b, Color maskcolor, List<CBlock> borg = null)
	{
		bool[] array = new bool[height];
		bool[] array2 = new bool[height];
		int num = -1;
		int y = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = -1;
		int num5 = 0;
		bool flag = true;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				array2[j] = false;
				c = tMask.GetPixel(i, j);
				if (c.a != 0f && c.r == maskcolor.r && c.g == maskcolor.g && c.b == maskcolor.b)
				{
					if (num < 0)
					{
						num = i;
						y = j;
						num2 = 1;
						flag = true;
					}
					if (num4 < 0)
					{
						num4 = i;
						num5 = j;
					}
					array2[j] = true;
				}
			}
			if (!flag)
			{
				bool flag2 = true;
				for (int k = 0; k < height; k++)
				{
					if (array[k] != array2[k])
					{
						flag2 = false;
						break;
					}
				}
				if (borg != null && b.Count < borg.Count && num2 == borg[b.Count].w)
				{
					flag2 = false;
				}
				if (flag2)
				{
					num2++;
				}
				else
				{
					for (int l = 0; l < height; l++)
					{
						if (array[l])
						{
							num3++;
						}
					}
					b.Add(new CBlock(num, y, num2, num3));
					if (num3 > depth)
					{
						depth = num3;
					}
					num = num4;
					y = num5;
					num2 = 1;
					num3 = 0;
				}
			}
			for (int m = 0; m < height; m++)
			{
				array[m] = array2[m];
			}
			num4 = -1;
			flag = false;
		}
	}

	private static bool isDublicate(List<CBlock> b, int x, int y)
	{
		for (int i = 0; i < b.Count; i++)
		{
			if (b[i].Contain(x, y))
			{
				return true;
			}
		}
		return false;
	}

	private static bool isDublicateFull(int x, int y)
	{
		for (int i = 0; i < bside.Count; i++)
		{
			if (bside[i].Contain(x, y))
			{
				return true;
			}
		}
		for (int j = 0; j < btop.Count; j++)
		{
			if (btop[j].Contain(x, y))
			{
				return true;
			}
		}
		for (int k = 0; k < bdown.Count; k++)
		{
			if (bdown[k].Contain(x, y))
			{
				return true;
			}
		}
		for (int l = 0; l < blens.Count; l++)
		{
			if (blens[l].Contain(x, y))
			{
				return true;
			}
		}
		for (int m = 0; m < bsidepoint.Count; m++)
		{
			if (bsidepoint[m].Contain(x, y))
			{
				return true;
			}
		}
		return false;
	}

	private static void BuildBlock(CBlock b, CBlock top, CBlock down, CBlock front, CBlock back)
	{
		if (front == null)
		{
			front = b;
		}
		if (back == null)
		{
			back = b;
		}
		float sizex = b.w;
		float sizey = b.h;
		float num = top.h;
		if (num < 2f)
		{
			num = 2f;
		}
		Vector3 localpos = new Vector3(b.x, b.y, ((float)depth - num) / 2f);
		BlockBuilder.BuildFaceBlockTex(0, localpos, Color.white, sizex, sizey, num, b.x + b.w, height - (b.y + b.h), -b.w, b.h);
		BlockBuilder.BuildFaceBlockTex(1, localpos, Color.white, sizex, sizey, num, b.x, height - (b.y + b.h), b.w, b.h);
		BlockBuilder.BuildFaceBlockTex(2, localpos, Color.white, sizex, sizey, num, front.x, height - (front.y + front.h), front.w, front.h);
		BlockBuilder.BuildFaceBlockTex(3, localpos, Color.white, sizex, sizey, num, back.x, height - (back.y + back.h), back.w, back.h);
		BlockBuilder.BuildFaceBlockTex(4, localpos, Color.white, sizex, sizey, num, top.x, height - (top.y + top.h), top.w, top.h);
		BlockBuilder.BuildFaceBlockTex(5, localpos, Color.white, sizex, sizey, num, down.x, height - (down.y + down.h), down.w, down.h);
	}
}
