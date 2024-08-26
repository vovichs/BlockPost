using Player;
using UnityEngine;
using UnityEngine.Rendering;

public class VCGen : MonoBehaviour
{
	private static float char_scale = 0.045f;

	private static Texture2D[] skin_org = new Texture2D[6];

	public static Texture2D tHair = null;

	public static Texture2D tEye = null;

	public static Texture2D tBeard = null;

	public static Texture2D tHat = null;

	public static Texture2D tBody = null;

	public static Color skin_a = new Color(47f / 51f, 63f / 85f, 52f / 85f);

	public static Color skin_b = new Color(43f / 51f, 0.6745098f, 0.5568628f);

	public static Color eye_a = new Color(0.3019608f, 54f / 85f, 13f / 15f);

	public static Color eye_b = new Color(0.1254902f, 21f / 85f, 1f / 3f);

	public static Color hair_a = new Color(16f / 85f, 0.1254902f, 0f);

	public static Color hair_b = new Color(8f / 85f, 0.0627451f, 0f);

	public static Color skin_zombie_a = new Color(31f / 51f, 0.5882353f, 20f / 51f);

	public static Color skin_zombie_b = new Color(0.3529412f, 0.3529412f, 11f / 51f);

	public static int hair = 1;

	public static int eye = 0;

	public static int beard = 1;

	public static int hat = 0;

	public static int body = 0;

	public static int pants = 0;

	public static int boots = 0;

	public static int team = 0;

	public static MeshRenderer lastmr = null;

	public static MeshFilter lastmf = null;

	private static BoxCollider lastbc = null;

	public static void SetColor(int _s, int _e, int _h)
	{
		skin_a = UtilChar.colorskin[_s].a;
		skin_b = UtilChar.colorskin[_s].b;
		eye_a = UtilChar.coloreye[_e].a;
		eye_b = UtilChar.coloreye[_e].b;
		hair_a = UtilChar.colorhair[_h].a;
		hair_b = UtilChar.colorhair[_h].b;
	}

	public static void SetData(int _hair, int _eye, int _beard, int _hat, int _body, int _pants, int _boots, int _team)
	{
		hair = _hair;
		eye = _eye;
		beard = _beard;
		hat = _hat;
		body = _body;
		pants = _pants;
		boots = _boots;
		team = _team;
	}

	public static void LoadAddonTex(int t)
	{
		tHair = LoadSkin("", "hair", hair, t);
		tEye = LoadSkin("", "eye", eye, t);
		tBeard = LoadSkin("", "beard", beard, t);
		tHat = LoadSkin("", "hat", hat, t);
	}

	public static Texture2D LoadSkin(string prefix, string skinname, int val, int team)
	{
		string text = val.ToString();
		if (val < 0)
		{
			text = "";
		}
		Texture2D texture2D = ContentLoader_.LoadTexture(prefix + "skin_" + skinname + text) as Texture2D;
		if (texture2D == null)
		{
			texture2D = ContentLoader_.LoadTexture(prefix + "skin_" + skinname + text + "_" + team) as Texture2D;
		}
		if (texture2D == null)
		{
			texture2D = Resources.Load("Textures/skins/" + prefix + "skin_" + skinname + text) as Texture2D;
		}
		if (texture2D == null)
		{
			texture2D = Resources.Load("Textures/skins/" + prefix + "skin_" + skinname + text + "_" + team) as Texture2D;
		}
		text = "0";
		if (texture2D == null)
		{
			texture2D = ContentLoader_.LoadTexture(prefix + "skin_" + skinname + text) as Texture2D;
		}
		if (texture2D == null)
		{
			texture2D = ContentLoader_.LoadTexture(prefix + "skin_" + skinname + text + "_" + team) as Texture2D;
		}
		if (texture2D == null)
		{
			texture2D = Resources.Load("Textures/skins/" + prefix + "skin_" + skinname + text) as Texture2D;
		}
		if (texture2D == null)
		{
			texture2D = Resources.Load("Textures/skins/" + prefix + "skin_" + skinname + text + "_" + team) as Texture2D;
		}
		if (texture2D == null)
		{
			Debug.Log("error load: " + prefix + "_" + skinname + text);
			Debug.Log("error load: " + prefix + "_" + skinname + text + "_" + team);
		}
		return texture2D;
	}

	public static void ReBuildSkin(PlayerData p)
	{
		SetColor(p.cp[0], p.cp[1], p.cp[2]);
		SetData(p.cp[3], p.cp[4], p.cp[5], p.cp[6], p.cp[7], p.cp[8], p.cp[9], p.team);
		BuildSkin(p);
	}

	private static void BuildSkin(PlayerData p)
	{
		int t = team;
		if (Controll.gamemode == 2 || Controll.gamemode == 8)
		{
			t = 2;
		}
		LoadAddonTex(t);
		skin_org[0] = LoadSkin("", "head", -1, t);
		skin_org[1] = LoadSkin("", "body", body, t);
		skin_org[3] = LoadSkin("", "pants", pants, t);
		skin_org[4] = LoadSkin("", "boots", boots, t);
		skin_org[5] = ContentLoader_.LoadTexture("skin_backpack") as Texture2D;
		p.skin[0] = GenerateTextureHead(skin_org[0], skin_a, skin_b, tHair, hair_a, hair_b, tEye, eye_a, eye_b, tBeard, tHat);
		p.skin[1] = GenerateTexture(skin_org[1], skin_a, skin_b);
		p.skin[3] = GenerateTexture(skin_org[3], skin_a, skin_b);
		p.skin[4] = skin_org[4];
		p.skin[5] = skin_org[5];
		if (p.skin_zombie != null)
		{
			for (int i = 0; i < p.skin_zombie.Length; i++)
			{
				if (p.skin_zombie[i] != null)
				{
					p.skin_zombie[i] = null;
				}
			}
		}
		p.skin_zombie = null;
		if (p.skin_damaged != null)
		{
			for (int j = 0; j < p.skin_damaged.Length; j++)
			{
				if (p.skin_damaged[j] != null)
				{
					p.skin_damaged[j] = null;
				}
			}
		}
		p.skin_damaged = null;
	}

	public static void BuildZombieSkin(PlayerData p)
	{
		if (p.skin_zombie == null)
		{
			p.skin_zombie = new Texture2D[6];
			int t = p.team;
			if (Controll.gamemode == 2 || Controll.gamemode == 5 || Controll.gamemode == 8)
			{
				t = 2;
			}
			SetColor(p.cp[0], p.cp[1], p.cp[2]);
			SetData(p.cp[3], p.cp[4], p.cp[5], p.cp[6], p.cp[7], p.cp[8], p.cp[9], t);
			LoadAddonTex(t);
			skin_org[0] = LoadSkin("", "head", -1, t);
			skin_org[1] = LoadSkin("zombie_", "body", body, t);
			skin_org[3] = LoadSkin("zombie_", "pants", pants, t);
			skin_org[4] = LoadSkin("", "boots", boots, t);
			skin_org[5] = ContentLoader_.LoadTexture("skin_backpack") as Texture2D;
			skin_a = skin_zombie_a;
			skin_b = skin_zombie_b;
			tHat = ContentLoader_.LoadTexture("skin_beard0") as Texture2D;
			tBeard = ContentLoader_.LoadTexture("skin_beard0") as Texture2D;
			int num = p.idx - p.idx / 4 * 4 + 1;
			tEye = ContentLoader_.LoadTexture("skin_zombie_face_" + num) as Texture2D;
			p.skin_zombie[0] = GenerateTextureHead(skin_org[0], skin_a, skin_b, tHair, hair_a, hair_b, tEye, eye_a, eye_b, tBeard, tHat);
			p.skin_zombie[1] = GenerateTexture(skin_org[1], skin_a, skin_b);
			p.skin_zombie[3] = GenerateTexture(skin_org[3], skin_a, skin_b);
			p.skin_zombie[4] = skin_org[4];
			p.skin_zombie[5] = skin_org[5];
		}
	}

	public static void BuildDamagedSkin(PlayerData p)
	{
		if (p.skin_damaged == null)
		{
			p.skin_damaged = new Texture2D[6];
			int num = team;
			if (Controll.gamemode == 2 || Controll.gamemode == 8)
			{
				num = 2;
			}
			skin_org[1] = LoadSkin("zombie_", "body", body, num);
			skin_org[3] = LoadSkin("zombie_", "pants", pants, num);
			p.skin_damaged[1] = GenerateTexture(skin_org[1], skin_a, skin_b);
			p.skin_damaged[3] = GenerateTexture(skin_org[3], skin_a, skin_b);
		}
	}

	public static GameObject Build(PlayerData p)
	{
		SetColor(p.cp[0], p.cp[1], p.cp[2]);
		SetData(p.cp[3], p.cp[4], p.cp[5], p.cp[6], p.cp[7], p.cp[8], p.cp[9], p.team);
		BuildSkin(p);
		for (int i = 0; i < 5; i++)
		{
			p.mat[i] = new Material(ContentLoader_.LoadMaterial("vertex_color"));
			p.mat[i].SetTexture("_MainTex", null);
			p.mat[i].SetFloat("_Glossiness", 0f);
		}
		p.mat[0].name = "mat_head";
		p.mat[1].name = "mat_body";
		p.mat[2].name = "mat_pants";
		p.mat[3].name = "mat_boots";
		p.mat[4].name = "mat_backpack";
		GameObject gameObject = new GameObject();
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.isKinematic = true;
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		BuildBody(gameObject.transform, p);
		BuildHead(p.goBody.transform, p);
		p.goArmHelp = new GameObject();
		p.goArmHelp.name = "armhelp";
		p.goArmHelp.transform.parent = p.goBody.transform;
		p.goArmHelp.transform.localPosition = new Vector3(0f, 19f, 0f);
		p.goArmHelp.transform.localEulerAngles = Vector3.zero;
		p.goArmHelp.transform.localScale = Vector3.one;
		BuildWaterSplash(gameObject.transform, p);
		BuildLeftArmv2(p.goArmHelp.transform, p);
		BuildRightArmv2(p.goArmHelp.transform, p);
		BuildLeftLeg(gameObject.transform, p);
		BuildRightLeg(gameObject.transform, p);
		BuildBackPack(p.goBody.transform, p);
		gameObject.transform.position = new Vector3(0f, 0f, 5f);
		Controll.GenerateArrow();
		p.goArrow = Object.Instantiate(Controll.pgoArrow[1]);
		p.goArrow.transform.localPosition = Vector3.zero;
		p.goArrow.transform.parent = gameObject.transform;
		p.restoreHead = p.goHead.transform.localPosition;
		p.restoreLArm = p.goLArm[0].transform.localPosition;
		p.restoreRArm = p.goRArm[0].transform.localPosition;
		return gameObject;
	}

	public static Texture2D GenerateTextureHead(Texture2D org, Color a, Color b, Texture2D hair, Color ha, Color hb, Texture2D eye, Color ea, Color eb, Texture2D beard, Texture2D hat)
	{
		Color[] pixels = hair.GetPixels();
		Color[] pixels2 = eye.GetPixels();
		Color[] pixels3 = beard.GetPixels();
		Color[] pixels4 = hat.GetPixels();
		Texture2D texture2D = new Texture2D(org.width, org.height, TextureFormat.RGBA32, false);
		texture2D.filterMode = FilterMode.Point;
		Color[] pixels5 = org.GetPixels();
		for (int i = 0; i < pixels5.Length; i++)
		{
			if (pixels5[i].r == 1f && pixels5[i].g == 1f && pixels5[i].b == 1f && pixels[i].a == 0f)
			{
				continue;
			}
			if (pixels4[i].a != 0f)
			{
				pixels5[i] = pixels4[i];
				continue;
			}
			if (pixels[i].a != 0f)
			{
				if (pixels[i].r == 1f && pixels[i].g == 0f)
				{
					pixels5[i] = ha;
				}
				else if (pixels[i].g == 1f && pixels[i].r == 0f)
				{
					pixels5[i] = hb;
				}
				else if (pixels[i].b == 1f && pixels[i].g == 0f)
				{
					pixels5[i].a = 0f;
				}
				else
				{
					pixels5[i] = pixels[i];
				}
			}
			else if (pixels2[i].a != 0f)
			{
				if (pixels2[i].r == 1f && pixels2[i].g == 0f)
				{
					pixels5[i] = ea;
				}
				else if (pixels2[i].g == 1f && pixels2[i].r == 0f)
				{
					pixels5[i] = eb;
				}
				else if (pixels2[i].b == 1f && pixels2[i].r == 0f && pixels2[i].g == 0f)
				{
					pixels5[i] = b;
				}
				else
				{
					pixels5[i] = pixels2[i];
				}
			}
			if (pixels3[i].a != 0f)
			{
				if (pixels3[i].r == 1f && pixels3[i].g == 0f)
				{
					pixels5[i] = ha;
				}
				else if (pixels3[i].g == 1f && pixels3[i].r == 0f)
				{
					pixels5[i] = hb;
				}
				else
				{
					pixels5[i] = pixels3[i];
				}
			}
			else if (pixels5[i].r == 1f && pixels5[i].g == 0f)
			{
				pixels5[i] = a;
			}
			else if (pixels5[i].g == 1f && pixels5[i].r == 0f)
			{
				pixels5[i] = b;
			}
		}
		texture2D.SetPixels(pixels5);
		texture2D.Apply(false);
		return texture2D;
	}

	private static Texture2D GenerateTexture(Texture2D org, Color a, Color b)
	{
		Texture2D texture2D = new Texture2D(org.width, org.height, TextureFormat.RGBA32, false);
		texture2D.filterMode = FilterMode.Point;
		Color[] pixels = org.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			if (pixels[i].r == 1f && pixels[i].g == 0f)
			{
				pixels[i] = a;
			}
			else if (pixels[i].g == 1f && pixels[i].r == 0f)
			{
				pixels[i] = b;
			}
		}
		texture2D.SetPixels(pixels);
		texture2D.Apply(false);
		return texture2D;
	}

	public static void UpdateTexturesNormal(PlayerData p)
	{
		p.mat[0].SetTexture("_MainTex", p.skin[0]);
		p.mat[1].SetTexture("_MainTex", p.skin[1]);
		p.mat[2].SetTexture("_MainTex", p.skin[3]);
	}

	public static void UpdateTexturesZombie(PlayerData p)
	{
		p.mat[0].SetTexture("_MainTex", p.skin_zombie[0]);
		p.mat[1].SetTexture("_MainTex", p.skin_zombie[1]);
		p.mat[2].SetTexture("_MainTex", p.skin_zombie[3]);
	}

	public static void UpdateTexturesDamaged(PlayerData p)
	{
		p.mat[1].SetTexture("_MainTex", p.skin_damaged[1]);
		p.mat[2].SetTexture("_MainTex", p.skin_damaged[3]);
	}

	private static void BuildHead(Transform p, PlayerData pl)
	{
		pl.goHead = CreateGameObject(p, "head", new Vector3(0f, 20f, 0f), 1f, pl.mat[0]);
		MeshRenderer component = pl.goHead.GetComponent<MeshRenderer>();
		MeshFilter component2 = pl.goHead.GetComponent<MeshFilter>();
		component.sharedMaterial.SetTexture("_MainTex", pl.skin[0]);
		MeshBuilder.Create();
		BlockBuilder.SetTexCoord(0, pl.skin[0].width, pl.skin[0].height);
		BlockBuilder.SetPivot(new Vector3(7f, 0f, 7f));
		BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 14f, 14f, 14f, 42f, 14f, 14f, 14f);
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 14f, 14f, 14f, 28f, 14f, -14f, -14f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 14f, 14f, 14f, 14f, 28f, 14f, 14f);
		BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 14f, 14f, 14f, 14f, 14f, 14f, 14f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 14f, 14f, 14f, 0f, 14f, 14f, 14f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 14f, 14f, 14f, 28f, 14f, 14f, 14f);
		component2.sharedMesh = MeshBuilder.ToMesh();
		pl.bcHead = pl.goHead.AddComponent<BoxCollider>();
		pl.rbHead = pl.goHead.AddComponent<Rigidbody>();
		pl.rbHead.isKinematic = true;
		HitData hitData = pl.goHead.AddComponent<HitData>();
		hitData.idx = pl.idx;
		hitData.box = 1;
		if (pl.cp[3] == 4)
		{
			BuildHeadHair(pl.goHead.transform, pl);
		}
	}

	private static void BuildHeadHair(Transform p, PlayerData pl)
	{
		if (GUIOptions.globalpreset == 0)
		{
			Material mat = MAT.Get("unlit_alpha_co");
			pl.goHeadHair = CreateGameObject(p, "hair", new Vector3(0f, 0f, -21f), 1f, mat);
		}
		else
		{
			Material mat2 = MAT.Get("vertex_alpha_co");
			pl.goHeadHair = CreateGameObject(p, "hair", new Vector3(0f, 0f, -21f), 1f, mat2);
		}
		MeshRenderer component = pl.goHeadHair.GetComponent<MeshRenderer>();
		MeshFilter component2 = pl.goHeadHair.GetComponent<MeshFilter>();
		component.sharedMaterial.SetTexture("_MainTex", pl.skin[0]);
		MeshBuilder.Create();
		BlockBuilder.SetTexCoord(0, pl.skin[0].width, pl.skin[0].height);
		BlockBuilder.SetPivot(new Vector3(0f, 0f, 0f));
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 0f, 28f, 28f, 56f, 28f, -28f, 28f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 0f, 28f, 28f, 28f, 28f, 28f, 28f);
		component2.sharedMesh = MeshBuilder.ToMesh();
	}

	private static void BuildBody(Transform p, PlayerData pl)
	{
		pl.goBody = CreateGameObject(p, "body", Vector3.zero, char_scale, pl.mat[1]);
		MeshRenderer component = pl.goBody.GetComponent<MeshRenderer>();
		MeshFilter component2 = pl.goBody.GetComponent<MeshFilter>();
		component.sharedMaterial.SetTexture("_MainTex", pl.skin[1]);
		pl.goBody.transform.localPosition = new Vector3(0f, -20f * char_scale, 0f);
		MeshBuilder.Create();
		BlockBuilder.SetTexCoord(0, pl.skin[1].width, pl.skin[1].height);
		BlockBuilder.SetPivot(new Vector3(8f, 0f, 5f));
		BlockBuilder.BuildFaceBlockTex(1, new Vector3(-0.1f, 0f, 0f), Color.white, 16.2f, 20f, 10f, 36f, 10f, 16f, 20f);
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(-0.1f, 0f, 0f), Color.white, 16.2f, 20f, 10f, 26f, 10f, -16f, -10f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3(-0.1f, 0f, 0f), Color.white, 16.2f, 20f, 10f, 10f, 30f, 16f, 10f);
		BlockBuilder.BuildFaceBlockTex(0, new Vector3(-0.1f, 0f, 0f), Color.white, 16.2f, 20f, 10f, 10f, 10f, 16f, 20f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(-0.1f, 0f, 0f), Color.white, 16.2f, 20f, 10f, 0f, 10f, 10f, 20f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(-0.1f, 0f, 0f), Color.white, 16.2f, 20f, 10f, 26f, 10f, 10f, 20f);
		component2.sharedMesh = MeshBuilder.ToMesh();
		pl.bcBody = pl.goBody.AddComponent<BoxCollider>();
		pl.rbBody = pl.goBody.AddComponent<Rigidbody>();
		pl.rbBody.isKinematic = true;
		HitData hitData = pl.goBody.AddComponent<HitData>();
		hitData.idx = pl.idx;
		hitData.box = 0;
	}

	private static void BuildLeftLeg(Transform p, PlayerData pl)
	{
		float num = 0.2f;
		pl.goLLeg[0] = CreateGameObject(p, "LeftLegUp", new Vector3(-4f, -20f, 0f), char_scale, pl.mat[2]);
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[3]);
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(4f, 12f, 4f));
		BlockBuilder.SetTexCoord(0, pl.skin[3].width, pl.skin[3].height);
		BlockBuilder.BuildFaceBlockTex(0, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 8f, 22f, 8f, 12f);
		BlockBuilder.BuildFaceBlockTex(1, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 24f, 22f, 8f, 12f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 0f, 22f, 8f, 12f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 16f, 22f, 8f, 12f);
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 33f, 22f, 8f, 8f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 41f, 22f, 8f, 8f);
		lastmf.sharedMesh = MeshBuilder.ToMesh();
		pl.goLLeg[1] = CreateGameObject(pl.goLLeg[0].transform, "LeftLegDown", new Vector3(0f, -12f, 0f), 1f, pl.mat[2]);
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[3]);
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(4f, 9f, 4f));
		BlockBuilder.SetTexCoord(0, pl.skin[3].width, pl.skin[3].height);
		BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 8f, 34f, 8f, 9f);
		BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 24f, 34f, 8f, 9f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 0f, 34f, 8f, 9f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 16f, 34f, 8f, 9f);
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 33f, 31f, 8f, 8f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 41f, 31f, 8f, 8f);
		lastmf.sharedMesh = MeshBuilder.ToMesh();
		float num2 = 0.2f;
		pl.goLLeg[2] = CreateGameObject(pl.goLLeg[1].transform, "LeftLegBoot", new Vector3(0f, -9f, -4f), 1f, pl.mat[3]);
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[4]);
		MeshBuilder.Create();
		BlockBuilder.SetTexCoord(0, pl.skin[4].width, pl.skin[4].height);
		BlockBuilder.SetPivot(new Vector3(4f, 3f, 0f));
		BlockBuilder.BuildFaceBlockTex(0, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 0f, 13f, 8f, 3f);
		BlockBuilder.BuildFaceBlockTex(1, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 0f, 0f, 8f, 3f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 8f, 0f, 10f, 3f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 18f, 0f, 10f, 3f);
		BlockBuilder.BuildFaceBlockTex(4, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 0f, 3f, 8f, 10f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 8f, 13f, 8f, -10f);
		lastmf.sharedMesh = MeshBuilder.ToMesh();
		pl.bcLLeg[0] = pl.goLLeg[0].AddComponent<BoxCollider>();
		pl.bcLLeg[1] = pl.goLLeg[1].AddComponent<BoxCollider>();
		pl.bcLLeg[2] = pl.goLLeg[2].AddComponent<BoxCollider>();
		pl.rbLLeg[0] = pl.goLLeg[0].AddComponent<Rigidbody>();
		pl.rbLLeg[0].isKinematic = true;
		pl.rbLLeg[1] = pl.goLLeg[1].AddComponent<Rigidbody>();
		pl.rbLLeg[1].isKinematic = true;
		pl.rbLLeg[2] = pl.goLLeg[2].AddComponent<Rigidbody>();
		pl.rbLLeg[2].isKinematic = true;
		HitData hitData = pl.goLLeg[0].AddComponent<HitData>();
		hitData.idx = pl.idx;
		hitData.box = 0;
		HitData hitData2 = pl.goLLeg[1].AddComponent<HitData>();
		hitData2.idx = pl.idx;
		hitData2.box = 0;
		HitData hitData3 = pl.goLLeg[2].AddComponent<HitData>();
		hitData3.idx = pl.idx;
		hitData3.box = 0;
	}

	private static void BuildRightLeg(Transform p, PlayerData pl)
	{
		float num = 0.2f;
		pl.goRLeg[0] = CreateGameObject(p, "RightLegUp", new Vector3(4f, -20f, 0f), char_scale, pl.mat[2]);
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[3]);
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(4f, 12f, 4f));
		BlockBuilder.SetTexCoord(0, pl.skin[3].width, pl.skin[3].height);
		BlockBuilder.BuildFaceBlockTex(0, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 8f, 0f, 8f, 12f);
		BlockBuilder.BuildFaceBlockTex(1, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 24f, 0f, 8f, 12f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 0f, 0f, 8f, 12f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 16f, 0f, 8f, 12f);
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 33f, 22f, 8f, 8f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3(num / 2f, 0f, 0f), Color.white, 8f - num, 12f, 8f, 41f, 22f, 8f, 8f);
		lastmf.sharedMesh = MeshBuilder.ToMesh();
		pl.goRLeg[1] = CreateGameObject(pl.goRLeg[0].transform, "RightLegDown", new Vector3(0f, -12f, 0f), 1f, pl.mat[2]);
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[3]);
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(4f, 9f, 4f));
		BlockBuilder.SetTexCoord(0, pl.skin[3].width, pl.skin[3].height);
		BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 8f, 12f, 8f, 9f);
		BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 24f, 12f, 8f, 9f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 0f, 12f, 8f, 9f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 16f, 12f, 8f, 9f);
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 33f, 31f, 8f, 8f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 8f, 9f, 8f, 41f, 31f, 8f, 8f);
		lastmf.sharedMesh = MeshBuilder.ToMesh();
		float num2 = 0.2f;
		pl.goRLeg[2] = CreateGameObject(pl.goRLeg[1].transform, "RightLegBoot", new Vector3(0f, -9f, -4f), 1f, pl.mat[3]);
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[4]);
		MeshBuilder.Create();
		BlockBuilder.SetTexCoord(0, pl.skin[4].width, pl.skin[4].height);
		BlockBuilder.SetPivot(new Vector3(4f, 3f, 0f));
		BlockBuilder.BuildFaceBlockTex(0, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 0f, 13f, 8f, 3f);
		BlockBuilder.BuildFaceBlockTex(1, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 0f, 0f, 8f, 3f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 8f, 0f, 10f, 3f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 18f, 0f, 10f, 3f);
		BlockBuilder.BuildFaceBlockTex(4, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 0f, 3f, 8f, 10f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3((0f - num2) / 2f, 0f, 0f), Color.white, 8f + num2, 3f, 10f, 8f, 13f, 8f, -10f);
		lastmf.sharedMesh = MeshBuilder.ToMesh();
		pl.bcRLeg[0] = pl.goRLeg[0].AddComponent<BoxCollider>();
		pl.bcRLeg[1] = pl.goRLeg[1].AddComponent<BoxCollider>();
		pl.bcRLeg[2] = pl.goRLeg[2].AddComponent<BoxCollider>();
		pl.rbRLeg[0] = pl.goRLeg[0].AddComponent<Rigidbody>();
		pl.rbRLeg[0].isKinematic = true;
		pl.rbRLeg[1] = pl.goRLeg[1].AddComponent<Rigidbody>();
		pl.rbRLeg[1].isKinematic = true;
		pl.rbRLeg[2] = pl.goRLeg[2].AddComponent<Rigidbody>();
		pl.rbRLeg[2].isKinematic = true;
		HitData hitData = pl.goRLeg[0].AddComponent<HitData>();
		hitData.idx = pl.idx;
		hitData.box = 0;
		HitData hitData2 = pl.goRLeg[1].AddComponent<HitData>();
		hitData2.idx = pl.idx;
		hitData2.box = 0;
		HitData hitData3 = pl.goRLeg[2].AddComponent<HitData>();
		hitData3.idx = pl.idx;
		hitData3.box = 0;
	}

	private static void BuildBackPack(Transform p, PlayerData pl)
	{
		pl.goBackPack = CreateGameObject(p, "backpack", new Vector3(3f, 2f, -6f), 1f, pl.mat[4]);
		MeshRenderer component = pl.goBackPack.GetComponent<MeshRenderer>();
		MeshFilter component2 = pl.goBackPack.GetComponent<MeshFilter>();
		component.sharedMaterial.SetTexture("_MainTex", pl.skin[5]);
		MeshBuilder.Create();
		BlockBuilder.SetTexCoord(0, pl.skin[5].width, pl.skin[5].height);
		BlockBuilder.SetPivot(new Vector3(8f, 0f, 5f));
		BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 10f, 16f, 6f, 22f, 6f, 10f, 16f);
		BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 10f, 16f, 6f, 6f, 6f, 10f, 16f);
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 10f, 16f, 6f, 6f, 0f, 10f, 6f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 10f, 16f, 6f, 16f, 28f, -10f, -6f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 10f, 16f, 6f, 0f, 6f, 6f, 16f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 10f, 16f, 6f, 16f, 6f, 6f, 16f);
		component2.sharedMesh = MeshBuilder.ToMesh();
	}

	private static void BuildDebugPlatform(Transform p, PlayerData pl)
	{
		pl.goDebug = CreateGameObject(p, "platform", Vector3.zero, char_scale);
		pl.goDebug.transform.localPosition = new Vector3(0f, -45f * char_scale, 0f);
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(32f, 0f, 32f));
		BlockBuilder.BuildBox(Vector3.zero, 64f, 1f, 64f, Color.gray);
		lastmf.sharedMesh = MeshBuilder.ToMesh();
	}

	private static void BuildWaterSplash(Transform p, PlayerData pl)
	{
		pl.goWaterSplash = CreateGameObject(p, "watersplash", Vector3.zero, char_scale);
		pl.goWaterSplash.transform.localPosition = new Vector3(0f, 0f, 0f);
		MeshBuilder.Create();
		BlockBuilder.SetTexCoord(0, 1, 1);
		BlockBuilder.SetPivot(new Vector3(16f, 0f, 16f));
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 32f, 0f, 32f, 0f, 0f, 1f, 1f);
		lastmf.sharedMesh = MeshBuilder.ToMesh();
		Material material = new Material(ContentLoader_.LoadMaterial("standard_fade"));
		Texture2D value = ContentLoader_.LoadTexture("water_splash") as Texture2D;
		material.SetTexture("_MainTex", value);
		material.SetColor("_Color", Color.white);
		material.SetFloat("_Metallic", 0f);
		lastmr.material = material;
		pl.matWaterSplash = material;
		pl.frameWaterSplash = 0;
		pl.goWaterSplash.SetActive(false);
	}

	public static GameObject CreateGameObject(Transform p, string name, Vector3 offset, float scale, Material mat = null)
	{
		GameObject obj = new GameObject();
		obj.name = name;
		obj.transform.parent = p;
		obj.transform.localPosition = new Vector3(offset.x * scale, offset.y * scale, offset.z * scale);
		obj.transform.localEulerAngles = Vector3.zero;
		obj.transform.localScale = new Vector3(scale, scale, scale);
		lastmf = obj.AddComponent<MeshFilter>();
		lastmr = obj.AddComponent<MeshRenderer>();
		if (mat == null)
		{
			lastmr.material = new Material(ContentLoader_.LoadMaterial("vertex_color"));
			lastmr.material.SetTexture("_MainTex", null);
			lastmr.material.SetFloat("_Glossiness", 0f);
		}
		else
		{
			lastmr.sharedMaterial = mat;
		}
		lastmr.shadowCastingMode = ShadowCastingMode.On;
		return obj;
	}

	public static void BuildLeftArmv2(Transform tr, PlayerData pl)
	{
		pl.goLArm[0] = CreateGameObject(tr, "LeftArmUp", new Vector3(-8f, 0f, 0f), 1f, pl.mat[1]);
		pl.goLArm[0].transform.localEulerAngles = new Vector3(0f, 0f, 60f);
		pl.goLArm[0].GetComponent<MeshRenderer>();
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[1]);
		_MeshBuilder.Create();
		_BlockBuilder.SetPivot(new Vector3(12f, 1f, 4f));
		_BlockBuilder.SetTexCoord(0, pl.skin[1].width, pl.skin[1].height);
		_BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 27f, 39f, 12f, 8f);
		_BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 39f, 55f, -12f, 8f);
		_BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 0f, 41f, 6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 6f, 41f, 6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 39f, 47f, -12f, 8f);
		_BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 27f, 31f, 12f, 8f);
		pl.goLArm[0].GetComponent<MeshFilter>().sharedMesh = _MeshBuilder.ToMesh();
		pl.goLArm[1] = CreateGameObject(pl.goLArm[0].transform, "LeftArmDown", new Vector3(-12f, 2f, 0f), 1f, pl.mat[1]);
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[1]);
		_MeshBuilder.Create();
		_BlockBuilder.SetPivot(new Vector3(6f, 3f, 4f));
		_BlockBuilder.SetTexCoord(0, pl.skin[1].width, pl.skin[1].height);
		_BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 39f, 39f, 6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 45f, 55f, -6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 12f, 41f, 6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 45f, 47f, -6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 39f, 31f, 6f, 8f);
		_BlockBuilder.SetTexCoord(0, pl.skin[1].width, pl.skin[1].height);
		_BlockBuilder.BuildFaceBlockTex(0, new Vector3(-6f, 0f, 0f), Color.white, 6f, 6f, 8f, 0f, 64f, 6f, -6f);
		_BlockBuilder.BuildFaceBlockTex(1, new Vector3(-6f, 0f, 0f), Color.white, 6f, 6f, 8f, 14f, 64f, 6f, -6f);
		_BlockBuilder.BuildFaceBlockTex(3, new Vector3(-6f, 0f, 0f), Color.white, 6f, 6f, 8f, 6f, 64f, 8f, -6f);
		_BlockBuilder.BuildFaceBlockTex(4, new Vector3(-6f, 0f, 0f), Color.white, 6f, 6f, 8f, 33f, 0f, 6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(5, new Vector3(-6f, 0f, 0f), Color.white, 6f, 6f, 8f, 27f, 0f, 6f, 8f);
		pl.goLArm[1].GetComponent<MeshFilter>().sharedMesh = _MeshBuilder.ToMesh();
		pl.bcLArm[0] = pl.goLArm[0].AddComponent<BoxCollider>();
		pl.bcLArm[1] = pl.goLArm[1].AddComponent<BoxCollider>();
		pl.rbLArm[0] = pl.goLArm[0].AddComponent<Rigidbody>();
		pl.rbLArm[0].isKinematic = true;
		pl.rbLArm[1] = pl.goLArm[1].AddComponent<Rigidbody>();
		pl.rbLArm[1].isKinematic = true;
		HitData hitData = pl.goLArm[0].AddComponent<HitData>();
		hitData.idx = pl.idx;
		hitData.box = 0;
		HitData hitData2 = pl.goLArm[1].AddComponent<HitData>();
		hitData2.idx = pl.idx;
		hitData2.box = 0;
	}

	public static void BuildRightArmv2(Transform tr, PlayerData pl)
	{
		pl.goRArm[0] = CreateGameObject(tr, "RightArmUp", new Vector3(8f, 0f, 0f), 1f, pl.mat[1]);
		pl.goRArm[0].transform.localEulerAngles = new Vector3(0f, 0f, -60f);
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[1]);
		_MeshBuilder.Create();
		_BlockBuilder.SetPivot(new Vector3(0f, 1f, 4f));
		_BlockBuilder.SetTexCoord(0, pl.skin[1].width, pl.skin[1].height);
		_BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 58f, 39f, -12f, 8f);
		_BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 46f, 55f, 12f, 8f);
		_BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 6f, 41f, 6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 0f, 41f, 6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 46f, 47f, 12f, 8f);
		_BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 12f, 6f, 8f, 58f, 31f, -12f, 8f);
		lastmf.sharedMesh = _MeshBuilder.ToMesh();
		pl.goRArm[1] = CreateGameObject(pl.goRArm[0].transform, "RightArmDown", new Vector3(12f, 2f, 0f), 1f, pl.mat[1]);
		lastmr.sharedMaterial.SetTexture("_MainTex", pl.skin[1]);
		_MeshBuilder.Create();
		_BlockBuilder.SetPivot(new Vector3(0f, 3f, 4f));
		_BlockBuilder.SetTexCoord(0, pl.skin[1].width, pl.skin[1].height);
		_BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 45f, 39f, -6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 39f, 55f, 6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 18f, 41f, -6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 39f, 47f, 6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 6f, 6f, 8f, 45f, 31f, -6f, 8f);
		_BlockBuilder.SetTexCoord(0, pl.skin[1].width, pl.skin[1].height);
		_BlockBuilder.BuildFaceBlockTex(0, new Vector3(6f, 0f, 0f), Color.white, 6f, 6f, 8f, 6f, 64f, -6f, -6f);
		_BlockBuilder.BuildFaceBlockTex(1, new Vector3(6f, 0f, 0f), Color.white, 6f, 6f, 8f, 20f, 64f, -6f, -6f);
		_BlockBuilder.BuildFaceBlockTex(2, new Vector3(6f, 0f, 0f), Color.white, 6f, 6f, 8f, 6f, 64f, 8f, -6f);
		_BlockBuilder.BuildFaceBlockTex(4, new Vector3(6f, 0f, 0f), Color.white, 6f, 6f, 8f, 39f, 0f, -6f, 8f);
		_BlockBuilder.BuildFaceBlockTex(5, new Vector3(6f, 0f, 0f), Color.white, 6f, 6f, 8f, 33f, 0f, -6f, 8f);
		lastmf = pl.goRArm[1].GetComponent<MeshFilter>();
		lastmf.sharedMesh = _MeshBuilder.ToMesh();
		pl.bcRArm[0] = pl.goRArm[0].AddComponent<BoxCollider>();
		pl.bcRArm[1] = pl.goRArm[1].AddComponent<BoxCollider>();
		pl.rbRArm[0] = pl.goRArm[0].AddComponent<Rigidbody>();
		pl.rbRArm[0].isKinematic = true;
		pl.rbRArm[1] = pl.goRArm[1].AddComponent<Rigidbody>();
		pl.rbRArm[1].isKinematic = true;
		HitData hitData = pl.goRArm[0].AddComponent<HitData>();
		hitData.idx = pl.idx;
		hitData.box = 0;
		HitData hitData2 = pl.goRArm[1].AddComponent<HitData>();
		hitData2.idx = pl.idx;
		hitData2.box = 0;
	}

	public static void BuildFPSArm(Transform p, PlayerData pl)
	{
		pl.goFPSLArm = Object.Instantiate(pl.goLArm[0]);
		pl.goFPSRArm = Object.Instantiate(pl.goRArm[0]);
		pl.goFPSLArm.name = "LeftArmUp";
		pl.goFPSRArm.name = "RightArmUp";
		CharacterJoint component = pl.goFPSLArm.GetComponent<CharacterJoint>();
		Rigidbody component2 = pl.goFPSLArm.GetComponent<Rigidbody>();
		BoxCollider component3 = pl.goFPSLArm.GetComponent<BoxCollider>();
		Object.Destroy(component);
		Object.Destroy(component2);
		Object.Destroy(component3);
		component = pl.goFPSRArm.GetComponent<CharacterJoint>();
		component2 = pl.goFPSRArm.GetComponent<Rigidbody>();
		BoxCollider component4 = pl.goFPSRArm.GetComponent<BoxCollider>();
		Object.Destroy(component);
		Object.Destroy(component2);
		Object.Destroy(component4);
		pl.goFPSLArm.transform.parent = p;
		pl.goFPSLArm.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
		pl.goFPSLArm.transform.localPosition = new Vector3(0f, -2000f, 0f);
		pl.goFPSRArm.transform.parent = p;
		pl.goFPSRArm.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
		pl.goFPSRArm.transform.localPosition = new Vector3(0f, -2000f, 0f);
		MeshRenderer meshRenderer = null;
		Transform[] componentsInChildren = pl.goFPSLArm.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name == "LeftArmDown")
			{
				BuildBigFingerv2(componentsInChildren[i], pl);
				componentsInChildren[i].localEulerAngles = Vector3.zero;
				component = componentsInChildren[i].gameObject.GetComponent<CharacterJoint>();
				component2 = componentsInChildren[i].gameObject.GetComponent<Rigidbody>();
				BoxCollider component5 = componentsInChildren[i].gameObject.GetComponent<BoxCollider>();
				Object.Destroy(component);
				Object.Destroy(component2);
				Object.Destroy(component5);
			}
			meshRenderer = componentsInChildren[i].gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			}
		}
		componentsInChildren = pl.goFPSRArm.GetComponentsInChildren<Transform>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			meshRenderer = componentsInChildren[j].gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			}
			if (componentsInChildren[j].name == "RightArmDown")
			{
				componentsInChildren[j].localEulerAngles = Vector3.zero;
				component = componentsInChildren[j].gameObject.GetComponent<CharacterJoint>();
				component2 = componentsInChildren[j].gameObject.GetComponent<Rigidbody>();
				BoxCollider component6 = componentsInChildren[j].gameObject.GetComponent<BoxCollider>();
				Object.Destroy(component);
				Object.Destroy(component2);
				Object.Destroy(component6);
			}
		}
	}

	private static void BuildBigFingerv2(Transform p, PlayerData pl)
	{
		GameObject obj = CreateGameObject(p, "LeftBigFinger", new Vector3(-7f, -3f, 4.01f), 1f, pl.mat[1]);
		obj.transform.localPosition = new Vector3(-7f, -3f, 4.01f);
		obj.transform.localEulerAngles = new Vector3(0f, 180f, 310f);
		obj.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
		_MeshBuilder.Create();
		_BlockBuilder.SetPivot(new Vector3(0f, 0f, 0f));
		_BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 3f, 2f, 2f, 51f, 0f, 3f, 2f);
		_BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 3f, 2f, 2f, 51f, 2f, 3f, 2f);
		_BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 3f, 2f, 2f, 54f, 4f, -3f, 2f);
		_BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 3f, 2f, 2f, 54f, 6f, -3f, 2f);
		_BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 3f, 2f, 2f, 54f, 0f, 2f, 2f);
		_BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 3f, 2f, 2f, 54f, 2f, 2f, 2f);
		lastmf.sharedMesh = _MeshBuilder.ToMesh();
	}
}
