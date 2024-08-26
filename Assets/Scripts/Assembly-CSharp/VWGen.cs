using System;
using System.Globalization;
using Player;
using UnityEngine;
using UnityEngine.Rendering;

public class VWGen : MonoBehaviour
{
	private static Texture2D weapon_tex;

	private static Texture2D weapon_mask;

	private static Texture2D weapon_mask_extra;

	private static int w = 0;

	private static int h = 0;

	private static Texture2D weapon_add;

	private static Texture2D weapon_add_mask;

	private static Texture2D weapon_add_point;

	private static Vector3 rhand = Vector3.zero;

	private static Vector3 trigger = Vector3.zero;

	private static Vector3 lhand = Vector3.zero;

	private static Vector3 muzzle = Vector3.zero;

	private static Vector3 scope = Vector3.zero;

	public static GameObject goWeaponMain = null;

	public static GameObject goWeapon = null;

	public static GameObject goMuzzle = null;

	public static GameObject goBigFinger = null;

	public static GameObject goLBone = null;

	public static GameObject goRBone = null;

	public static GameObject goBoneRHand = null;

	public static GameObject goBoneLHand = null;

	public static GameObject goBoneMuzzle = null;

	public static GameObject goBoneScope = null;

	public static GameObject goBolt0 = null;

	private static Material[] pMuzzle = new Material[3];

	public static void FindBones()
	{
		rhand = Vector3.zero;
		trigger = Vector3.zero;
		lhand = Vector3.zero;
		muzzle = Vector3.zero;
		scope = Vector3.zero;
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
					scope = new Vector3(j, i, 0f);
				}
				else if (pixel.g != 0f)
				{
					if (pixel.g < 0.2f)
					{
						rhand = new Vector3(j, i, 0f);
					}
					else if (pixel.g < 0.4f)
					{
						trigger = new Vector3(j, i, 0f);
					}
					else if (pixel.g < 0.6f)
					{
						lhand = new Vector3(j, i, 0f);
					}
					else if (pixel.g < 0.8f)
					{
						muzzle = new Vector3(j, i, 0f);
					}
				}
			}
		}
	}

	public static WeaponData BuildWeaponFPS(PlayerData pl, string weaponname, WeaponInfo wi, int scopeid = 0)
	{
		if (wi.id == 107 || wi.id == 108 || wi.id == 109 || wi.id == 110 || weaponname == "grenade33")
		{
			WeaponData weaponData = new WeaponData();
			weaponData.weaponname = weaponname;
			weaponData.tex = Resources.Load("Textures/skin_backpack_" + weaponname) as Texture2D;
			PLH.AddWeapon(pl, weaponData);
			return weaponData;
		}
		LoadWeaponBase(wi.id, weaponname);
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
		Transform transform = null;
		if (GameObject.Find("Camera") != null)
		{
			transform = GameObject.Find("Camera").transform;
		}
		GameObject gameObject = new GameObject();
		gameObject.name = "weapon_" + weaponname + "_" + gameObject.GetInstanceID();
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		FindBones();
		Material material = new Material(ContentLoader_.LoadMaterial("vertex_color"));
		material.SetTexture("_MainTex", weapon_tex);
		CreateWeaponObject(weaponname, transform, gameObject, material);
		VCGen.BuildFPSArm(gameObject.transform, pl);
		goRBone = CreateBone(pl.goFPSRArm.transform, "bone", new Vector3(19.5f, 2f, 1f));
		goLBone = CreateBone(pl.goFPSLArm.transform, "bone", new Vector3(-19.5f, -0.5f, -2f));
		BuildMuzzle(goWeapon.transform, wi);
		gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		goWeaponMain = gameObject;
		Texture2D texture2D = ContentLoader_.LoadTexture(weaponname + "_icon") as Texture2D;
		if (texture2D == null)
		{
			texture2D = weapon_tex;
		}
		Texture2D texture2D2 = new Texture2D(weapon_tex.width, weapon_tex.height, TextureFormat.RGBA32, false);
		texture2D2.filterMode = FilterMode.Point;
		Color black = Color.black;
		Color color = new Color(0f, 0f, 0f, 0f);
		Color color2 = new Color(0.9f, 0.9f, 0.9f, 1f);
		Color color3 = new Color(0f, 0f, 0f, 0f);
		for (int i = 0; i < texture2D2.width; i++)
		{
			for (int j = 0; j < texture2D2.height; j++)
			{
				bool flag = false;
				if (weapon_mask.GetPixel(i, j).a != 0f)
				{
					texture2D2.SetPixel(i, j, color);
					flag = true;
				}
				Color pixel = texture2D.GetPixel(i, j);
				if (pixel.a != 0f)
				{
					if (pixel.r == 0f && pixel.g == 0f && pixel.b == 0f)
					{
						texture2D2.SetPixel(i, j, color2);
					}
					else if (!flag)
					{
						texture2D2.SetPixel(i, j, color);
					}
				}
				else
				{
					texture2D2.SetPixel(i, j, color3);
				}
			}
		}
		texture2D2.Apply(false);
		GameObject goScope = null;
		if (scopeid != 0)
		{
			goScope = ScopeGen.BuildScope(scopeid, goWeapon, goBoneScope);
		}
		if (wi.name == "panzerfaust")
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("prefabs/weapon_" + weaponname) as GameObject, goWeapon.transform);
			obj.name = "model";
			UnityEngine.Object.Destroy(goWeapon.GetComponent<MeshRenderer>());
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			if (wi.name == "panzerfaust")
			{
				GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load("prefabs/weapon_" + weaponname + "_drop") as GameObject, goWeapon.transform);
				obj2.transform.localPosition = Vector3.zero;
				obj2.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
				obj2.name = "drop";
				GameObject.Find(gameObject.name + "/" + goWeapon.name + "/drop/Model").GetComponent<MeshRenderer>().enabled = false;
			}
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(goWeapon);
		gameObject2.name = "p_weapon_" + weaponname + "_" + pl.idx + "_fps";
		gameObject2.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
		GameObject goPMuzzle = GameObject.Find(gameObject2.name + "/muzzle");
		gameObject2.SetActive(false);
		WeaponData weaponData2 = new WeaponData();
		weaponData2.goScope = goScope;
		weaponData2.weaponname = weaponname;
		weaponData2.go = goWeaponMain;
		weaponData2.goWeapon = goWeapon;
		weaponData2.mat = material;
		weaponData2.goMuzzle = goMuzzle;
		weaponData2.goLHand = pl.goFPSLArm;
		weaponData2.goRHand = pl.goFPSRArm;
		weaponData2.goLBone = goLBone;
		weaponData2.goRBone = goRBone;
		weaponData2.goBoneRHand = goBoneRHand;
		weaponData2.goBoneLHand = goBoneLHand;
		weaponData2.goBoneMuzzle = goBoneMuzzle;
		weaponData2.goBoneScope = goBoneScope;
		weaponData2.snd = Resources.Load("sounds/s_" + weaponname) as AudioClip;
		if (weaponData2.snd == null)
		{
			weaponData2.snd = ContentLoader_.LoadAudio("s_" + weaponname);
		}
		weaponData2.tex = weapon_tex;
		weaponData2.hudicon = texture2D2;
		weaponData2.vtc = GUIGameSet.CalcSize(weapon_tex);
		weaponData2.shelltype = 1;
		weaponData2.goBolt0 = goBolt0;
		weaponData2.goPW = gameObject2;
		weaponData2.goPMuzzle = goPMuzzle;
		gameObject.SetActive(false);
		weaponData2.goMuzzle.SetActive(false);
		weaponData2.goPMuzzle.SetActive(false);
		weaponData2.vPosition = goWeapon.transform.localPosition;
		TextAsset textAsset = Resources.Load("params/p_" + weaponname) as TextAsset;
		if (textAsset == null)
		{
			textAsset = ContentLoader_.LoadTextAsset("p_" + weaponname);
		}
		if (textAsset != null)
		{
			string text = Environment.NewLine + "\n\r";
			string[] array = textAsset.text.Split(text.ToCharArray(), StringSplitOptions.None);
			for (int k = 0; k < array.Length; k++)
			{
				string[] array2 = array[k].Split(' ');
				if (array2[0] == "position")
				{
					float result = 0f;
					float result2;
					float.TryParse(array2[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result2);
					float result3;
					float.TryParse(array2[2], NumberStyles.Any, CultureInfo.InvariantCulture, out result3);
					float.TryParse(array2[3], NumberStyles.Any, CultureInfo.InvariantCulture, out result);
					weaponData2.vPosition = new Vector3(result2, result3, result);
					goWeapon.transform.localPosition = weaponData2.vPosition;
				}
				else if (array2[0] == "scope")
				{
					int result4 = 0;
					int.TryParse(array2[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result4);
					if (weaponData2.scope == null)
					{
						weaponData2.scope = new ScopeData[1];
						weaponData2.scope[0] = new ScopeData(result4, array2[2]);
					}
					else
					{
						Array.Resize(ref weaponData2.scope, weaponData2.scope.Length + 1);
						weaponData2.scope[weaponData2.scope.Length - 1] = new ScopeData(result4, array2[2]);
					}
				}
				else if (array2[0] == "lrotation")
				{
					float result5 = 0f;
					float result6;
					float.TryParse(array2[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result6);
					float result7;
					float.TryParse(array2[2], NumberStyles.Any, CultureInfo.InvariantCulture, out result7);
					float.TryParse(array2[3], NumberStyles.Any, CultureInfo.InvariantCulture, out result5);
					weaponData2.vLRotation = new Vector3(result6, result7, result5);
				}
				else if (array2[0] == "lposition")
				{
					float result8 = 0f;
					float result9;
					float.TryParse(array2[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result9);
					float result10;
					float.TryParse(array2[2], NumberStyles.Any, CultureInfo.InvariantCulture, out result10);
					float.TryParse(array2[3], NumberStyles.Any, CultureInfo.InvariantCulture, out result8);
					weaponData2.goBoneLHand.transform.localPosition = new Vector3(result9, result10, result8);
				}
				else if (array2[0] == "rposition")
				{
					float result11 = 0f;
					float result12;
					float.TryParse(array2[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result12);
					float result13;
					float.TryParse(array2[2], NumberStyles.Any, CultureInfo.InvariantCulture, out result13);
					float.TryParse(array2[3], NumberStyles.Any, CultureInfo.InvariantCulture, out result11);
					weaponData2.goBoneRHand.transform.localPosition = new Vector3(result12, result13, result11);
				}
				else if (array2[0] == "zscale")
				{
					float result14 = 0f;
					float.TryParse(array2[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result14);
					weaponData2.go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f * result14);
				}
				else if (array2[0] == "firetype")
				{
					int result15 = 0;
					int.TryParse(array2[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result15);
					if (wi != null)
					{
						wi.firetype = result15;
					}
				}
				else if (array2[0] == "animation")
				{
					if (weaponData2.ani == null)
					{
						weaponData2.ani = weaponData2.go.AddComponent<Animation>();
					}
					weaponData2.wield = Resources.Load(weaponname + "_wield") as AnimationClip;
					if (weaponData2.wield == null)
					{
						weaponData2.wield = Resources.Load("animation/" + weaponname + "_wield") as AnimationClip;
					}
					if (weaponData2.wield != null)
					{
						weaponData2.ani.AddClip(weaponData2.wield, weaponData2.wield.name);
					}
					weaponData2.shoot = Resources.Load(weaponname + "_shoot") as AnimationClip;
					if (weaponData2.shoot == null)
					{
						weaponData2.shoot = Resources.Load("animation/" + weaponname + "_shoot") as AnimationClip;
					}
					if (weaponData2.shoot != null)
					{
						weaponData2.ani.AddClip(weaponData2.shoot, weaponData2.shoot.name);
					}
					weaponData2.shoot2 = Resources.Load(weaponname + "_shoot2") as AnimationClip;
					if (weaponData2.shoot2 == null)
					{
						weaponData2.shoot2 = Resources.Load("animation/" + weaponname + "_shoot2") as AnimationClip;
					}
					if (weaponData2.shoot2 != null)
					{
						weaponData2.ani.AddClip(weaponData2.shoot2, weaponData2.shoot2.name);
					}
					weaponData2.ani.clip = weaponData2.wield;
				}
				else if (array2[0] == "changeparent")
				{
					weaponData2.goWeapon.transform.parent = weaponData2.goRHand.transform;
				}
				else if (array2[0] == "hidden")
				{
					weaponData2.goPW.GetComponent<MeshRenderer>().enabled = false;
				}
				else if (array2[0] == "double")
				{
					weaponData2.goWeaponDouble = UnityEngine.Object.Instantiate(weaponData2.goWeapon);
					weaponData2.goWeaponDouble.transform.parent = weaponData2.goLHand.transform;
					weaponData2.goWeaponDouble.transform.localPosition = Vector3.zero;
					weaponData2.goWeaponDouble.transform.localScale = Vector3.one;
					weaponData2.goWeaponDouble.name = "weaponmodeldouble";
				}
				else if (array2[0] == "weaponscale")
				{
					float result16 = 0f;
					float.TryParse(array2[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result16);
					weaponData2.goWeapon.transform.localScale = new Vector3(result16, result16, result16);
				}
			}
		}
		if (wi != null)
		{
			if (wi.slot == 2 || wi.slot == 3)
			{
				weaponData2.goPMuzzle = null;
				weaponData2.goMuzzle = null;
				weaponData2.shelltype = 0;
			}
			if (wi.id == 149)
			{
				weaponData2.shelltype = 0;
			}
		}
		weaponData2.wi = wi;
		VWPos.Load(weaponData2);
		return weaponData2;
	}

	private static void CreateWeaponObject(string weaponname, Transform trParent, GameObject goW, Material mat)
	{
		goWeapon = new GameObject();
		MeshFilter meshFilter = goWeapon.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = goWeapon.AddComponent<MeshRenderer>();
		meshRenderer.material = mat;
		goWeapon.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
		goWeapon.transform.parent = trParent;
		goWeapon.transform.localPosition = new Vector3(1f, -1f, 1.8f);
		goWeapon.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
		goWeapon.name = "weaponmodel";
		goWeapon.transform.parent = goW.transform;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		MeshBuilder.Create();
		BlockBuilder.SetPivot(trigger);
		BlockBuilder.BeginTexBuild(weapon_tex, weapon_mask);
		VWGen2.BuildTex(w, h, weapon_tex, 0);
		VWGen2.BuildTex(w, h, weapon_mask, 1);
		if (weapon_mask_extra != null)
		{
			VWGen2.BuildTex(w, h, weapon_mask_extra, 2);
			VWGen2.BuildTex(w, h, weapon_mask_extra, 3);
			VWGen2.BuildTex(w, h, weapon_mask_extra, 4);
		}
		meshFilter.sharedMesh = MeshBuilder.ToMesh();
		if (weapon_add != null)
		{
			VWGen2.BuildAdd(weapon_add, weapon_add_mask, weapon_add_point, goWeapon.transform, trigger);
		}
		Vector3 vector = trigger - Vector3.forward * 5f;
		goBolt0 = new GameObject();
		goBolt0.name = "bone_bolt0";
		goBolt0.transform.parent = goWeapon.transform;
		goBolt0.transform.localPosition = vector - trigger;
		goBolt0.transform.localEulerAngles = Vector3.zero;
		goBolt0.transform.localScale = Vector3.one;
		goBoneRHand = new GameObject();
		goBoneRHand.name = "bone_rhand";
		goBoneRHand.transform.parent = goWeapon.transform;
		goBoneRHand.transform.localPosition = new Vector3(rhand.x - trigger.x, rhand.y - trigger.y, rhand.z - trigger.z);
		goBoneRHand.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		goBoneRHand.transform.localScale = new Vector3(1f, 1f, 1f);
		goBoneLHand = new GameObject();
		goBoneLHand.name = "bone_lhand";
		goBoneLHand.transform.parent = goWeapon.transform;
		goBoneLHand.transform.localPosition = new Vector3(lhand.x - trigger.x, lhand.y - trigger.y, lhand.z - trigger.z);
		goBoneLHand.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		goBoneLHand.transform.localScale = new Vector3(1f, 1f, 1f);
		goBoneMuzzle = new GameObject();
		goBoneMuzzle.name = "bone_muzzle";
		goBoneMuzzle.transform.parent = goWeapon.transform;
		goBoneMuzzle.transform.localPosition = new Vector3(muzzle.x - trigger.x, muzzle.y - trigger.y, muzzle.z - trigger.z);
		goBoneMuzzle.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		goBoneMuzzle.transform.localScale = new Vector3(1f, 1f, 1f);
		goBoneScope = new GameObject();
		goBoneScope.name = "bone_scope";
		goBoneScope.transform.parent = goWeapon.transform;
		goBoneScope.transform.localPosition = new Vector3(scope.x - trigger.x, scope.y - trigger.y, scope.z - trigger.z);
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
			goWeapon.transform.localPosition = new Vector3(1f, -1.3f, 1.8f);
			goWeapon.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
			goWeapon.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			break;
		case "block":
			UnityEngine.Object.Destroy(meshFilter.sharedMesh);
			meshRenderer.sharedMaterial = new Material(Atlas.mat);
			meshRenderer.sharedMaterial.SetTexture("_MainTex", Atlas.texmark);
			MeshBuilder.Create();
			BlockBuilder.SetPivot(Vector3.zero);
			BlockBuilder.BuildFree(40, Vector3.zero, Palette.GetColor());
			meshFilter.sharedMesh = MeshBuilder.ToMesh();
			goWeapon.transform.localPosition = new Vector3(1.5f, -1.5f, 2.5f);
			goWeapon.transform.localEulerAngles = new Vector3(5f, 240f, 10f);
			goWeapon.transform.localScale = Vector3.one;
			goBoneLHand.transform.localPosition = new Vector3(0.5f, 0f, 1f);
			goBoneRHand.transform.localPosition = new Vector3(0f, 0f, -0.14f);
			break;
		}
	}

	private static void LoadWeaponBase(int wid, string weaponname)
	{
		weapon_tex = ContentLoader_.LoadTexture(weaponname) as Texture2D;
		if (weapon_tex == null)
		{
			weapon_tex = Resources.Load("weapons/" + weaponname) as Texture2D;
		}
		if (wid == 120)
		{
			weapon_add = Resources.Load("weapons/" + weaponname + "_add") as Texture2D;
			weapon_add_mask = Resources.Load("weapons/" + weaponname + "_add_mask") as Texture2D;
			weapon_add_point = Resources.Load("weapons/" + weaponname + "_add_point") as Texture2D;
		}
		else
		{
			weapon_add = null;
		}
		if (wid == 119 || wid == 133)
		{
			weapon_mask_extra = Resources.Load("weapons/" + weaponname + "_mask_extra") as Texture2D;
		}
		else
		{
			weapon_mask_extra = null;
		}
		weapon_mask = ContentLoader_.LoadTexture(weaponname + "_mask") as Texture2D;
		if (weapon_mask == null)
		{
			weapon_mask = Resources.Load("weapons/" + weaponname + "_mask") as Texture2D;
		}
	}

	private static GameObject CreateBone(Transform p, string name, Vector3 pos)
	{
		GameObject obj = new GameObject();
		obj.name = "bone";
		obj.transform.parent = p;
		obj.transform.localPosition = pos;
		obj.transform.localEulerAngles = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		return obj;
	}

	private static void BuildMuzzle(Transform p, WeaponInfo wi)
	{
		if (pMuzzle[0] == null)
		{
			pMuzzle[0] = ContentLoader_.LoadMaterial("muzzle");
		}
		if (pMuzzle[1] == null)
		{
			pMuzzle[1] = Resources.Load("Materials/muzzle_pistol") as Material;
		}
		if (pMuzzle[2] == null)
		{
			pMuzzle[2] = Resources.Load("Materials/muzzle_taser") as Material;
		}
		float num = 2f;
		int num2 = 0;
		if (wi.slot == 1)
		{
			num2 = 1;
		}
		if (wi.id == 170)
		{
			num2 = 2;
		}
		GameObject obj = new GameObject();
		obj.name = "muzzle";
		obj.transform.parent = p;
		obj.transform.position = goBoneMuzzle.transform.position;
		obj.transform.localEulerAngles = Vector3.zero;
		obj.transform.localScale = new Vector3(num, num, 1f);
		MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.material = new Material(pMuzzle[num2]);
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(0.5f, 0.5f, 0f));
		BlockBuilder.SetTexCoord(0, 1, 1);
		BlockBuilder.BuildFaceBlock(1, 1, Vector3.zero, Color.white, 1f, 1f, 0f);
		meshFilter.sharedMesh = MeshBuilder.ToMesh();
		if (weapon_add != null)
		{
			VWGen2.BuildAdd(weapon_add, weapon_add_mask, weapon_add_point, goWeapon.transform, trigger);
		}
		goMuzzle = obj;
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
