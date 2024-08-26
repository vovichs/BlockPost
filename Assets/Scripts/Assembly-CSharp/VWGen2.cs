using Player;
using UnityEngine;
using UnityEngine.Rendering;

public class VWGen2 : MonoBehaviour
{
	private static Texture2D weapon_tex;

	private static Texture2D weapon_mask;

	private static Texture2D weapon_mask_extra;

	private static Texture2D weapon_ls;

	private static Texture2D weapon_add;

	private static Texture2D weapon_add_mask;

	private static Texture2D weapon_add_point;

	private static int w;

	private static int h;

	private static Color c;

	private static Vector3 point_rhand = Vector3.zero;

	private static Vector3 point_trigger = Vector3.zero;

	private static Vector3 point_lhand = Vector3.zero;

	private static Vector3 point_muzzle = Vector3.zero;

	private static Vector3 point_scope = Vector3.zero;

	private static Material currMat = null;

	private static GameObject goBoneMuzzle;

	private static GameObject goBoneScope;

	private static GameObject goBoneRHand;

	private static GameObject goMuzzle;

	private static string[] muzzlehack = new string[24]
	{
		"kukri", "katana", "karambit_gold", "karambit_blood", "bayonet", "machete", "strider", "tomahawk", "victorinox", "shadow_dagger",
		"say", "kama", "gut", "falchion", "cleaver", "6x4", "shovel", "block", "zombiehands", "balisong",
		"biker", "bowie", "extractor", "hatchet"
	};

	public static WeaponData BuildWeaponPreview(string weaponname)
	{
		weapon_tex = LoadWeaponTexture(weaponname);
		weapon_mask = LoadWeaponMask(weaponname);
		if (weapon_tex == null || weapon_mask == null)
		{
			return null;
		}
		if (weaponname == "sten")
		{
			LoadAdd(weaponname);
		}
		else
		{
			weapon_add = null;
		}
		switch (weaponname)
		{
		case "ppsh":
			LoadExtraMask(weaponname);
			break;
		case "mg42":
			LoadExtraMask(weaponname);
			break;
		case "m249":
			LoadExtraMask(weaponname);
			break;
		default:
			weapon_mask_extra = null;
			break;
		}
		w = weapon_tex.width;
		h = weapon_tex.height;
		GameObject gameObject = CreateWeaponRoot(weaponname);
		GameObject goWeapon = CreateWeaponModel(weaponname, gameObject.transform);
		gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		return new WeaponData
		{
			weaponname = weaponname,
			go = gameObject,
			goWeapon = goWeapon,
			mat = currMat,
			tex = weapon_tex
		};
	}

	public static WeaponData BuildWeaponPlayer(string weaponname, int scopeid = 0)
	{
		weapon_tex = LoadWeaponTexture(weaponname);
		weapon_mask = LoadWeaponMask(weaponname);
		if (weapon_tex == null || weapon_mask == null)
		{
			return null;
		}
		if (weaponname == "sten")
		{
			LoadAdd(weaponname);
		}
		else
		{
			weapon_add = null;
		}
		if (weaponname == "ppsh")
		{
			LoadExtraMask(weaponname);
		}
		else
		{
			weapon_mask_extra = null;
		}
		if (weaponname == "mg42")
		{
			LoadExtraMask(weaponname);
		}
		else
		{
			weapon_mask_extra = null;
		}
		if (weaponname == "m249")
		{
			LoadExtraMask(weaponname);
		}
		else
		{
			weapon_mask_extra = null;
		}
		w = weapon_tex.width;
		h = weapon_tex.height;
		GameObject gameObject = CreateWeaponModel(weaponname);
		if (weaponname == "zombiehands")
		{
			gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
		gameObject.name = "p_weapon_" + weaponname + "_" + gameObject.GetInstanceID();
		gameObject.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
		BuildMuzzle(weaponname, gameObject.transform);
		GameObject goScope = ScopeGen.BuildScope(scopeid, gameObject, goBoneScope);
		if (weaponname == "panzerfaust")
		{
			GameObject obj = Object.Instantiate(Resources.Load("prefabs/weapon_" + weaponname) as GameObject, gameObject.transform);
			obj.name = "model";
			Object.Destroy(gameObject.GetComponent<MeshRenderer>());
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			if (weaponname == "panzerfaust")
			{
				GameObject obj2 = Object.Instantiate(Resources.Load("prefabs/weapon_" + weaponname + "_drop") as GameObject, gameObject.transform);
				obj2.transform.localPosition = Vector3.zero;
				obj2.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
				obj2.name = "drop";
				GameObject.Find(gameObject.name + "/drop/Model").GetComponent<MeshRenderer>().enabled = false;
			}
		}
		WeaponData weaponData = new WeaponData();
		weaponData.goScope = goScope;
		weaponData.weaponname = weaponname;
		weaponData.goPW = gameObject;
		weaponData.mat = currMat;
		weaponData.goPMuzzle = goMuzzle;
		weaponData.goBoneMuzzle = ((goMuzzle == null) ? null : goBoneMuzzle);
		weaponData.goBoneScope = goBoneScope;
		weaponData.goBoneRHand = goBoneRHand;
		weaponData.snd = ContentLoader_.LoadAudio("s_" + weaponname);
		if (weaponData.snd == null)
		{
			weaponData.snd = Resources.Load("sounds/s_" + weaponname) as AudioClip;
		}
		VWPos.Load(weaponData);
		return weaponData;
	}

	private static Texture2D LoadWeaponTexture(string weaponname)
	{
		Texture2D texture2D = ContentLoader_.LoadTexture(weaponname) as Texture2D;
		if (texture2D == null)
		{
			texture2D = Resources.Load("weapons/" + weaponname) as Texture2D;
		}
		return texture2D;
	}

	private static Texture2D LoadWeaponMask(string weaponname)
	{
		Texture2D texture2D = ContentLoader_.LoadTexture(weaponname + "_mask") as Texture2D;
		if (texture2D == null)
		{
			texture2D = Resources.Load("weapons/" + weaponname + "_mask") as Texture2D;
		}
		return texture2D;
	}

	private static GameObject CreateWeaponRoot(string weaponname)
	{
		GameObject obj = new GameObject();
		obj.name = "weapon_" + weaponname;
		obj.transform.localPosition = new Vector3(0f, 0f, 0f);
		obj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		return obj;
	}

	private static GameObject CreateWeaponModel(string weaponname, Transform tr = null, bool debug = false)
	{
		FindBones();
		GameObject gameObject = new GameObject();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		currMat = new Material(ContentLoader_.LoadMaterial("vertex_color"));
		currMat.SetTexture("_MainTex", weapon_tex);
		meshRenderer.material = currMat;
		gameObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
		gameObject.transform.localPosition = new Vector3(1f, -1f, 1.8f);
		gameObject.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
		gameObject.name = "weaponmodel_" + weaponname;
		if (tr != null)
		{
			gameObject.transform.parent = tr;
		}
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		MeshBuilder.Create();
		BlockBuilder.SetPivot(point_trigger);
		BlockBuilder.BeginTexBuild(weapon_tex, weapon_mask);
		BuildTex(w, h, weapon_tex, 0);
		BuildTex(w, h, weapon_mask, 1);
		if (weapon_mask_extra != null)
		{
			BuildTex(w, h, weapon_mask_extra, 2);
			BuildTex(w, h, weapon_mask_extra, 3);
			BuildTex(w, h, weapon_mask_extra, 4);
		}
		if (debug)
		{
			BlockBuilder.BuildBox(point_rhand, 1f, 1f, 3f, Color.green);
			BlockBuilder.BuildBox(point_lhand, 1f, 1f, 3f, Color.blue);
			BlockBuilder.BuildBox(point_muzzle, 1f, 1f, 1f, Color.red);
			BlockBuilder.BuildBox(point_trigger, 1f, 1f, 1f, Color.cyan);
		}
		meshFilter.sharedMesh = MeshBuilder.ToMesh();
		if (weapon_add != null)
		{
			BuildAdd(weapon_add, weapon_add_mask, weapon_add_point, gameObject.transform, point_trigger);
		}
		goBoneRHand = new GameObject();
		goBoneRHand.name = "bone_rhand";
		goBoneRHand.transform.parent = gameObject.transform;
		goBoneRHand.transform.localPosition = new Vector3(point_rhand.x - point_trigger.x, point_rhand.y - point_trigger.y, point_rhand.z - point_trigger.z);
		goBoneRHand.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		goBoneRHand.transform.localScale = new Vector3(1f, 1f, 1f);
		GameObject gameObject2 = new GameObject();
		gameObject2.name = "bone_lhand";
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = new Vector3(point_lhand.x - point_trigger.x, point_lhand.y - point_trigger.y, point_lhand.z - point_trigger.z);
		gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		goBoneMuzzle = new GameObject();
		goBoneMuzzle.name = "bone_muzzle";
		goBoneMuzzle.transform.parent = gameObject.transform;
		goBoneMuzzle.transform.localPosition = new Vector3(point_muzzle.x - point_trigger.x, point_muzzle.y - point_trigger.y, point_muzzle.z - point_trigger.z);
		goBoneMuzzle.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		goBoneMuzzle.transform.localScale = new Vector3(1f, 1f, 1f);
		goBoneScope = new GameObject();
		goBoneScope.name = "bone_scope";
		goBoneScope.transform.parent = gameObject.transform;
		goBoneScope.transform.localPosition = new Vector3(point_scope.x - point_trigger.x, point_scope.y - point_trigger.y, point_scope.z - point_trigger.z);
		goBoneScope.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		goBoneScope.transform.localScale = new Vector3(1f, 1f, 1f);
		switch (weaponname)
		{
		case "shovel":
		case "karambit_gold":
		case "katana":
		case "kukri":
		case "karambit_blood":
		case "bayonet":
		case "machete":
		case "strider":
		case "tomahawk":
		case "sapper_shovel":
			gameObject.transform.localPosition = new Vector3(1f, -1.3f, 1.8f);
			gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
			gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			break;
		case "block":
			Object.Destroy(meshFilter.sharedMesh);
			meshRenderer.sharedMaterial = new Material(Atlas.mat);
			meshRenderer.sharedMaterial.SetTexture("_MainTex", Atlas.texmark);
			MeshBuilder.Create();
			BlockBuilder.SetPivot(Vector3.zero);
			BlockBuilder.BuildFree(40, Vector3.zero, Palette.GetColor());
			meshFilter.sharedMesh = MeshBuilder.ToMesh();
			gameObject.transform.localPosition = new Vector3(1.5f, -1.5f, 2.5f);
			gameObject.transform.localEulerAngles = new Vector3(5f, 240f, 10f);
			gameObject.transform.localScale = Vector3.one;
			gameObject2.transform.localPosition = new Vector3(0.5f, 0f, 1f);
			goBoneRHand.transform.localPosition = new Vector3(0f, 0f, -0.14f);
			break;
		}
		return gameObject;
	}

	public static void FindBones()
	{
		point_rhand = Vector3.zero;
		point_trigger = Vector3.zero;
		point_lhand = Vector3.zero;
		point_muzzle = Vector3.zero;
		point_scope = Vector3.zero;
		for (int i = 0; i < h; i++)
		{
			for (int j = 0; j < w; j++)
			{
				Color pixel = weapon_mask.GetPixel(j, i);
				if (pixel.a == 0f)
				{
					continue;
				}
				if (pixel.b == 1f)
				{
					point_scope = new Vector3(j, i, 0f);
				}
				else if (pixel.g != 0f)
				{
					if (pixel.g < 0.2f)
					{
						point_rhand = new Vector3(j, i, 0f);
					}
					else if (pixel.g < 0.4f)
					{
						point_trigger = new Vector3(j, i, 0f);
					}
					else if (pixel.g < 0.6f)
					{
						point_lhand = new Vector3(j, i, 0f);
					}
					else if (pixel.g < 0.8f)
					{
						point_muzzle = new Vector3(j, i, 0f);
					}
				}
			}
		}
	}

	public static void BuildTex(int w, int h, Texture2D t, int level)
	{
		int num = -1;
		for (int i = 0; i < h; i++)
		{
			num = -1;
			for (int j = 0; j < w; j++)
			{
				c = t.GetPixel(j, i);
				bool flag = false;
				if (c.a == 0f)
				{
					flag = true;
				}
				if (!flag && level == 1 && !(c.r > 0.75f))
				{
					flag = true;
				}
				if (!flag && level == 2 && !(c.r > 0.75f))
				{
					flag = true;
				}
				if (!flag && level == 3 && !(c.g > 0.75f))
				{
					flag = true;
				}
				if (!flag && level == 4 && !(c.b > 0.75f))
				{
					flag = true;
				}
				if (flag)
				{
					if (num >= 0)
					{
						switch (level)
						{
						case 1:
							BuildLine(num, i, -1, j, 3);
							break;
						case 2:
							BuildLine(num, i, -3, j, 7);
							break;
						case 3:
							BuildLine(num, i, -4, j, 9);
							break;
						case 4:
							BuildLine(num, i, -5, j, 11);
							break;
						default:
							BuildLine(num, i, 0, j);
							break;
						}
						num = -1;
					}
				}
				else if (num < 0)
				{
					num = j;
				}
			}
			if (num >= 0)
			{
				switch (level)
				{
				case 1:
					BuildLine(num, i, -1, w);
					break;
				case 2:
					BuildLine(num, i, -3, w, 7);
					break;
				case 3:
					BuildLine(num, i, -4, w, 9);
					break;
				case 4:
					BuildLine(num, i, -5, w, 11);
					break;
				default:
					BuildLine(num, i, 0, w);
					break;
				}
			}
		}
	}

	private static void BuildLine(int x, int y, int z, int x2, int depth = 1)
	{
		BlockBuilder.BuildLine(new Vector3(x, y, z), x2 - x, depth);
	}

	private static void BuildMuzzle(string weaponname, Transform p)
	{
		goMuzzle = null;
		for (int i = 0; i < muzzlehack.Length; i++)
		{
			if (muzzlehack[i] == weaponname)
			{
				return;
			}
		}
		float num = 2f;
		GameObject obj = new GameObject();
		obj.name = "muzzle";
		obj.transform.parent = p;
		obj.transform.position = goBoneMuzzle.transform.position;
		obj.transform.localEulerAngles = Vector3.zero;
		obj.transform.localScale = new Vector3(num, num, 1f);
		MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.material = new Material(ContentLoader_.LoadMaterial("muzzle"));
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(0.5f, 0.5f, 0f));
		BlockBuilder.SetTexCoord(0, 1, 1);
		BlockBuilder.BuildFaceBlock(1, 1, Vector3.zero, Color.white, 1f, 1f, 0f);
		meshFilter.sharedMesh = MeshBuilder.ToMesh();
		goMuzzle = obj;
		goMuzzle.SetActive(false);
	}

	public static void BuildAdd(Texture2D tex, Texture2D mask, Texture2D tex_point, Transform trParent, Vector3 trigger)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "add_point";
		gameObject.transform.parent = trParent;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
		int i = 0;
		for (int width = tex_point.width; i < width; i++)
		{
			int j = 0;
			for (int height = tex_point.height; j < height; j++)
			{
				c = tex_point.GetPixel(i, j);
				if (c.a != 0f)
				{
					gameObject.transform.localPosition = new Vector3(i, j, 0f) - trigger;
					break;
				}
			}
		}
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		currMat = new Material(ContentLoader_.LoadMaterial("vertex_color"));
		currMat.SetTexture("_MainTex", tex);
		meshRenderer.material = currMat;
		MeshBuilder.Create();
		BlockBuilder.SetPivot(Vector3.zero);
		BlockBuilder.BeginTexBuild(tex, mask);
		BuildTex(tex.width, tex.height, tex, 0);
		BuildTex(tex.width, tex.height, mask, 1);
		meshFilter.sharedMesh = MeshBuilder.ToMesh();
	}

	private static void LoadAdd(string weaponname)
	{
		weapon_add = Resources.Load("weapons/" + weaponname + "_add") as Texture2D;
		weapon_add_mask = Resources.Load("weapons/" + weaponname + "_add_mask") as Texture2D;
		weapon_add_point = Resources.Load("weapons/" + weaponname + "_add_point") as Texture2D;
		if (weapon_add == null)
		{
			weapon_add = ContentLoader_.LoadTexture(weaponname + "_add") as Texture2D;
		}
		if (weapon_add_mask == null)
		{
			weapon_add_mask = ContentLoader_.LoadTexture(weaponname + "_add_mask") as Texture2D;
		}
		if (weapon_add_point == null)
		{
			weapon_add_point = ContentLoader_.LoadTexture(weaponname + "_add_point") as Texture2D;
		}
	}

	private static void LoadExtraMask(string weaponname)
	{
		weapon_mask_extra = Resources.Load("weapons/" + weaponname + "_mask_extra") as Texture2D;
		if (weapon_mask_extra == null)
		{
			weapon_mask_extra = ContentLoader_.LoadTexture(weaponname + "_mask_extra") as Texture2D;
		}
	}
}
