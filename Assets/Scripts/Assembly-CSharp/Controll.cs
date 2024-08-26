using System;
using System.Collections.Generic;
using GameClass;
using Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.ImageEffects;

public class Controll : MonoBehaviour
{
	[Flags]
	public enum KeyBase
	{
		plus_x = 1,
		minus_x = 2,
		plus_z = 4,
		minus_z = 8,
		jump = 0x10,
		duck = 0x20,
		speed = 0x40,
		aim = 0x80
	}

	public class BulletHole
	{
		public GameObject go;

		public int x;

		public int y;

		public int z;

		public BulletHole(GameObject go, float x, float y, float z)
		{
			this.x = (int)x;
			this.y = (int)y;
			this.z = (int)z;
			this.go = go;
		}
	}

	public static Controll cs = null;

	public static FlareLayer flCamera = null;

	public static AmplifyMotionEffect ame = null;

	public static AmplifyOcclusionEffect aoe = null;

	public static FXAA fxaa = null;

	public static DepthOfField dof = null;

	public static ColorCorrectionCurves ccc = null;

	private static float default_speed = 0.5f;

	public static float speed = 0f;

	public static float camheight = 2.4f;

	public static float boxheight = 2.75f;

	public static float zombieslowdown = 0f;

	public static bool inSlow = false;

	public static bool invertmouse = false;

	public static float sens = 3f;

	public static float zoomsens = 3f;

	public static float rx = 0f;

	public static float ry = 0f;

	public static float movex = 0f;

	public static float movez = 0f;

	private static float radius = 0.4f;

	private float cl_fps = 0.05f;

	public static uint cl_time = 0u;

	public static uint sv_time = 0u;

	public static uint ping = 0u;

	public static float fcl_time = 0f;

	private static int prevweaponinput = -1;

	public static int currweaponinput = -1;

	private static GameObject goControll = null;

	public static GameObject goCamera = null;

	public static GameObject goRadarCam = null;

	public static GameObject goRadarCam2 = null;

	public static Transform trControll = null;

	public static Transform trCamera = null;

	public static Transform trRadarCam = null;

	public static Transform trRadarCam2 = null;

	public static Camera csCam = null;

	public static Camera csRadarCam = null;

	public static Camera csRadarCam2 = null;

	public static Plane[] camplanes;

	public static Vector3 velocity = Vector3.zero;

	public static Vector3 vel = Vector3.zero;

	public static bool inDuckKey = false;

	public static bool inJumpKey = false;

	public static bool inJumpKeyPressed = true;

	public static bool inDuck = false;

	public static bool inAir = false;

	public static bool inAttack = false;

	public static bool inZoom = false;

	public static bool inReload = false;

	public static bool inStuck = false;

	public static bool lockMove = false;

	public static bool lockLook = false;

	public static bool lockAttack = false;

	public static bool manualAttack = false;

	public static int burstAttack = 0;

	public static float tBurstAttack = 0f;

	public static int specid = -1;

	public static int specmode = 0;

	public static Vector3 prevPos = Vector3.zero;

	private Vector3 prevForward = Vector3.zero;

	private Vector3 prevRight = Vector3.zero;

	public static Vector3 nextPos = Vector3.zero;

	private Vector3 halfWayVector = Vector3.zero;

	private Vector3 nextPos0 = Vector3.zero;

	private Vector3 nextPos1 = Vector3.zero;

	private Vector3 jumpVector = Vector3.zero;

	private static int inAirFrame = 0;

	private float jumpPos;

	public static bool freefly = false;

	public static int buildmode = 1;

	public static int toolmode = 0;

	private float tFixedTimer;

	public static float currCamheight = 0f;

	public static Vector3 currPos = Vector3.zero;

	public static Vector3 Pos = Vector3.zero;

	private static float fcurrFrame = 0f;

	private static float fnextFrame = 0f;

	public static float fdeltatime = 0f;

	public static float fdifFrame = 0f;

	private AudioSource as_fs;

	private float fs_time;

	public static float specialtime_start = 0f;

	public static float specialtime_end = 0f;

	public static float specialtime_duration = 0f;

	public static bool inFreeze = false;

	public static float tFreeze = 0f;

	public static int iFreeze = 0;

	public static int demoidx = -1;

	public static PlayerData pl = new PlayerData();

	public static GameObject pgoDebris = null;

	public static GameObject pgoDebris_muzzle = null;

	public static GameObject pgoDebris_smoke = null;

	public static GameObject pgoDebris_smoke_big = null;

	public static GameObject pgoDebris_flame = null;

	public static GameObject[] pgoBloodSplat = new GameObject[4];

	public static GameObject[] pgoBloodSplatGreen = new GameObject[4];

	public static GameObject[] pgoEnergySplat = new GameObject[2];

	public static GameObject pgoTracer = null;

	public static GameObject pgoHole = null;

	public static GameObject pgoHoleBlood = null;

	public static GameObject pgoDamageBlock = null;

	public static GameObject[] pgoArrow = new GameObject[2];

	public static GameObject pgoShell = null;

	public static GameObject pgoGrenade = null;

	public static GameObject pgoMedkit = null;

	public static GameObject pgoAmmokit = null;

	public static GameObject pgoRocketkit = null;

	public static GameObject pgoExplode = null;

	public static GameObject[] pgoFreezeBlock = new GameObject[5];

	public static GameObject pgoFBlock = null;

	public static GameObject pgoFaust = null;

	public static GameObject pgoFaustDrop = null;

	public static GameObject pgoGrenade33 = null;

	public static List<BulletHole> holelist = new List<BulletHole>();

	public static float reload_start = 0f;

	public static float reload_end = 0f;

	public static float reload_end_msg = 0f;

	public static int reload_count = 0;

	public static float reload_active = 0f;

	public static float reload_result = 0f;

	public static float reload_forcept = 0f;

	public static Vector3 blockCursor = Vector3.zero;

	public static Vector3 nullvec = new Vector3(-2000f, -2000f, -2000f);

	private static Texture2D tBlack;

	private static Texture2D tRed;

	public static float forceunzoom = 0f;

	public static bool forceweapon = false;

	public static bool altrender = false;

	public static int layerMask = 1;

	public static List<AttackData> attacklist = new List<AttackData>();

	public static SpecialBlock[] sblock = null;

	private AudioClip sShovelBlock;

	private AudioClip sReloadEnd;

	private static AudioClip sReloadActive = null;

	private static AudioClip sReloadBad = null;

	private static AudioClip sReloadStart = null;

	private static AudioClip sThrow = null;

	private static AudioClip sHitmark = null;

	public static AudioClip sShieldmark = null;

	private static AudioClip sIceHit = null;

	public static AudioClip sSnowmark = null;

	public static int blockslot = 0;

	public static int gamemode = 0;

	private static ControllTouch csCT = null;

	private float overtime;

	private float overattack;

	private float overlock;

	private int flamestate = -1;

	private bool AutoReload;

	private float tAutoReload;

	public static uint iattacktime = 0u;

	public static bool manualZoom = false;

	private Texture2D tZoom;

	private Texture2D tDot;

	private Texture2D tGlass;

	private Color cglass = new Color(1f, 1f, 1f, 0f);

	private Rect rZoom;

	private Rect rDot;

	private float scope_scale = 1f;

	private int scope_type;

	private int scope_fov = 50;

	public static bool lockcursor = false;

	private bool hideobjects;

	private static int prevweaponkey = 0;

	public static int nextweaponkey = 0;

	private float twheelchange;

	private Vector3 jumpdir = Vector3.zero;

	private uint step;

	private bool MouseFilter = true;

	private int MouseLookSmoothSteps = 10;

	private float MouseLookSmoothWeight;

	protected List<Vector2> m_MouseLookSmoothBuffer = new List<Vector2>();

	protected Vector2 m_MouseLookSmoothMove = Vector2.zero;

	protected Vector2 m_MouseLookRawMove = Vector2.zero;

	protected Vector2 m_CurrentMouseLook = Vector2.zero;

	protected int m_LastMouseLookFrame = -1;

	private float tPhys;

	private Rect rCorner;

	private Rect rMoveTouch;

	private Color ca = new Color(1f, 1f, 1f, 0.25f);

	private static float intp = 0f;

	private Vector3 cam_offset = new Vector3(0f, 0.282f, 0f);

	private static float lasthitmarksound = 0f;

	private float recoilend;

	public void PostAwake()
	{
		cs = this;
		blockslot = 0;
		gamemode = 0;
		HUDTab.cs.SetScoreBar(0);
		goControll = new GameObject();
		goControll.transform.position = Vector3.zero;
		goControll.transform.eulerAngles = Vector3.zero;
		goControll.name = "Controll";
		trControll = goControll.transform;
		goCamera = new GameObject();
		goCamera.transform.position = Vector3.zero + new Vector3(0f, camheight, 0f);
		goCamera.transform.eulerAngles = Vector3.zero;
		goCamera.name = "Camera";
		csCam = goCamera.AddComponent<Camera>();
		csCam.nearClipPlane = 0.01f;
		csCam.farClipPlane = 128f;
		csCam.eventMask = 0;
		csCam.cullingMask = -257;
		trCamera = goCamera.transform;
		flCamera = goCamera.AddComponent<FlareLayer>();
		camplanes = GeometryUtility.CalculateFrustumPlanes(csCam);
		AddMotionCamera();
		AddSSAO();
		AddFXAA();
		if (altrender)
		{
			goCamera.AddComponent<DevDraw>();
		}
		goRadarCam = new GameObject();
		goRadarCam.transform.position = Vector3.zero + new Vector3(0f, camheight, 0f);
		goRadarCam.transform.eulerAngles = new Vector3(90f, 0f, 0f);
		goRadarCam.name = "RadarCam";
		csRadarCam = goRadarCam.AddComponent<Camera>();
		csRadarCam.nearClipPlane = 0.01f;
		csRadarCam.nearClipPlane = 0.1f;
		csRadarCam.farClipPlane = 128f;
		csRadarCam.clearFlags = CameraClearFlags.Nothing;
		csRadarCam.orthographic = true;
		csRadarCam.orthographicSize = 32f;
		csRadarCam.rect = new Rect(GUIM.YRES(16f) / (float)Screen.width, ((float)Screen.height - GUIM.YRES(16f) - GUIM.YRES(160f)) / (float)Screen.height, GUIM.YRES(200f) / (float)Screen.width, GUIM.YRES(160f) / (float)Screen.height);
		csRadarCam.cullingMask = 256;
		csRadarCam.depth = 1f;
		trRadarCam = goRadarCam.transform;
		goRadarCam.transform.parent = goControll.transform;
		goRadarCam2 = new GameObject();
		goRadarCam2.transform.position = Vector3.zero + new Vector3(0f, camheight, 0f);
		goRadarCam2.transform.eulerAngles = new Vector3(90f, 0f, 0f);
		goRadarCam2.name = "RadarCam_Overview";
		csRadarCam2 = goRadarCam2.AddComponent<Camera>();
		csRadarCam2.nearClipPlane = 0.01f;
		csRadarCam2.nearClipPlane = 0.1f;
		csRadarCam2.farClipPlane = 256f;
		csRadarCam2.clearFlags = CameraClearFlags.Nothing;
		csRadarCam2.orthographic = true;
		csRadarCam2.orthographicSize = 32f;
		csRadarCam2.rect = new Rect(((float)Screen.width / 2f - GUIM.YRES(150f)) / (float)Screen.width, ((float)Screen.height / 2f - GUIM.YRES(150f)) / (float)Screen.height, GUIM.YRES(300f) / (float)Screen.width, GUIM.YRES(300f) / (float)Screen.height);
		csRadarCam2.cullingMask = 256;
		csRadarCam2.depth = 1f;
		trRadarCam2 = goRadarCam2.transform;
		goRadarCam2.SetActive(false);
		csCam.fieldOfView = 50 + GUIOptions.mobilefov * 5;
		goCamera.transform.parent = goControll.transform;
		goCamera.AddComponent<AudioListener>();
		pl.asFS = goCamera.AddComponent<AudioSource>();
		pl.asW = goCamera.AddComponent<AudioSource>();
		pl.asFX = goCamera.AddComponent<AudioSource>();
		SetVolume(pl);
		pgoDebris = ContentLoader_.LoadGameObject("debris");
		pgoDebris_muzzle = Resources.Load("Prefabs/debris_muzzle") as GameObject;
		pgoDebris_smoke = Resources.Load("Prefabs/debris_smoke") as GameObject;
		pgoDebris_smoke_big = Resources.Load("Prefabs/debris_smoke_big") as GameObject;
		pgoExplode = Resources.Load("Prefabs/FX_Explosion_Fire") as GameObject;
		pgoDebris_flame = Resources.Load("Prefabs/debris_flame") as GameObject;
		GenerateBloodSplat();
		GenerateTracer();
		GenerateHole();
		GenerateDamageBlock();
		GenerateArrow();
		GenerateShell();
		GenerateGrenade();
		GenerateMedkit();
		GenerateAmmo();
		GenerateRocketkit();
		GenerateFreezeBlock();
		GenerateFBlock();
		GameObject obj = UnityEngine.Object.Instantiate(pgoArrow[0]);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.parent = goControll.transform;
		tBlack = TEX.GetTextureByName("black");
		tRed = TEX.GetTextureByName("red");
		csCT = GameObject.Find("GUI").AddComponent<ControllTouch>();
		GOpt.SetDistance();
	}

	public static void AddMotionCamera()
	{
		if (!(goCamera == null) && GUIOptions.motion != 0)
		{
			ame = goCamera.AddComponent<AmplifyMotionEffect>();
			ame.AutoRegisterObjs = false;
			ame.MotionScale = 5f;
			if (GUIOptions.motion == 2)
			{
				ame.CameraMotionMult = 0.1f;
			}
			else
			{
				ame.CameraMotionMult = 0f;
			}
		}
	}

	public static void AddSSAO()
	{
		if (!(goCamera == null) && GUIOptions.ssao != 0 && GUIOptions.globalpreset != 0)
		{
			aoe = goCamera.AddComponent<AmplifyOcclusionEffect>();
			aoe.Radius = 0.5f;
			aoe.Intensity = 0.9f;
			aoe.Bias = 0.01f;
			aoe.PowerExponent = 2f;
		}
	}

	public static void AddFXAA()
	{
	}

	public static void Clear()
	{
		UnityEngine.Object.Destroy(Builder.goCursor);
		UnityEngine.Object.Destroy(goCamera);
		UnityEngine.Object.Destroy(goControll);
		UnityEngine.Object.Destroy(goRadarCam2);
		if (csCT != null)
		{
			UnityEngine.Object.Destroy(csCT);
		}
		speed = 0f;
		pl.wstate = 0;
		pl.bstate = 0;
		for (int i = 0; i < 4; i++)
		{
			pl.rw[i] = 0;
		}
		for (int j = 0; j < 40; j++)
		{
			pl.fragcount[j] = 0;
		}
		inFreeze = false;
		for (int k = 1000; k < 1008; k++)
		{
			GUIInv.winfo[k] = null;
		}
		specmode = 0;
	}

	public static void SetVolume(PlayerData pl)
	{
		pl.asFS.volume = GUIOptions.gamevolume * 0.5f;
		pl.asW.volume = GUIOptions.gamevolume;
		if (pl.asFX != null)
		{
			pl.asFX.volume = GUIOptions.gamevolume;
		}
	}

	public static void SpectatorSpawn()
	{
		int num = UnityEngine.Random.Range(0, 4);
		float x = 0f;
		float z = 0f;
		float y = 0f;
		switch (num)
		{
		case 0:
			x = (float)Map.BLOCK_SIZE_X / 2f;
			z = Map.BLOCK_SIZE_Z;
			y = 180f;
			break;
		case 1:
			x = Map.BLOCK_SIZE_X;
			z = (float)Map.BLOCK_SIZE_Z / 2f;
			y = 270f;
			break;
		case 2:
			x = (float)Map.BLOCK_SIZE_X / 2f;
			z = 0f;
			y = 0f;
			break;
		case 3:
			x = 0f;
			z = (float)Map.BLOCK_SIZE_Z / 2f;
			y = 90f;
			break;
		}
		SetPos(new Vector3(x, 32f, z));
		trCamera.localEulerAngles = new Vector3(25f, y, 0f);
	}

	public static void SetPos(float x, float y, float z)
	{
		SetPos(new Vector3(x, y, z));
	}

	public static void SetLockMove(bool val)
	{
		lockMove = val;
		if (val)
		{
			speed = 0f;
		}
	}

	public static void SetLockLook(bool val)
	{
		lockLook = val;
	}

	public static void SetLockAttack(bool val)
	{
		lockAttack = val;
	}

	public static void SetCameraSolid(bool val)
	{
		if (val)
		{
			csCam.clearFlags = CameraClearFlags.Color;
			csCam.backgroundColor = new Color(27f / 85f, 0.30980393f, 24f / 85f);
		}
		else
		{
			csCam.clearFlags = CameraClearFlags.Skybox;
		}
	}

	public static void SetPos(Vector3 p)
	{
		trControll.position = p;
		currPos = p;
		Pos = p;
		prevPos = p;
	}

	public static void SetKillMode(bool val)
	{
		specid = -1;
		if (val)
		{
			SetLockMove(true);
			SetLockLook(true);
			SetLockAttack(true);
			pl.currweapon = null;
			PLH.HideWeapon(pl);
		}
		else
		{
			goCamera.transform.parent = goControll.transform;
			goCamera.transform.localPosition = Vector3.zero + new Vector3(0f, camheight, 0f);
			SetLockMove(false);
			SetLockLook(false);
			SetLockAttack(false);
		}
	}

	public static void SetCamMode(int id)
	{
		if (id >= 0 && id < 40)
		{
			specid = -1;
			if (PLH.player[id] != null && !(PLH.player[id].go == null))
			{
				specid = id;
			}
		}
	}

	private void Update()
	{
		if (ControllTouch.cs != null)
		{
			ControllTouch.cs.UpdateTouch();
		}
		UpdateLook();
		UpdateKey();
		UpdateKeyMenu();
		UpdateKeyWeapon();
		UpdatePlayer(pl);
		VWIK.Update();
		UpdateAttack();
		UpdateZoom();
		UpdateReload();
		UpdateCursor();
		if (overtime > 0f && Time.time > overattack)
		{
			overtime -= 0.4f * Time.deltaTime;
			if (overtime < 0f)
			{
				overtime = 0f;
			}
		}
		if (flamestate >= 0 && Time.time > overattack + 0.1f)
		{
			int flamestate2 = flamestate;
			int num = 2;
			flamestate = -1;
		}
		if (freefly)
		{
			Builder.UpdateFlyMode();
			return;
		}
		if (Time.realtimeSinceStartup > fnextFrame)
		{
			fdifFrame = Time.realtimeSinceStartup - fcurrFrame - cl_fps;
			if (fdifFrame >= cl_fps)
			{
				fdifFrame = 0f;
			}
			UpdateMove2();
			UpdatePhysics();
			UpdateStuck();
			UpdateSendPos();
		}
		LerpCamera();
		LerpMove();
		BobCamera();
		VWIK.ShakeCamera();
		KillCamera();
		if (HUD.showcallvote)
		{
			if (Input.GetKeyUp(KeyCode.F1))
			{
				Client.cs.send_callvote(1, 1);
				HUD.showcallvote = false;
			}
			else if (Input.GetKeyUp(KeyCode.F2))
			{
				Client.cs.send_callvote(1, 0);
				HUD.showcallvote = false;
			}
		}
	}

	public static void UpdatePlayer(PlayerData p)
	{
		if (p.bstate != 5)
		{
			PLH.UpdateMuzzle(p);
			if (p == pl)
			{
				PLH.Audio_fs(p, speed, inAir | inDuck, freefly);
			}
			else if (p.bstate != 0 && p.bstate != 2 && p.bstate != 3 && p.bstate != 5)
			{
				PLH.Audio_fs(p, default_speed, false, false);
			}
		}
	}

	private void UpdateAttack()
	{
		inAttack = false;
		if (inFreeze || Builder.active || pl.currweapon == null || lockAttack || inReload || (ControllTouch.fingerKeys[0] < 0 && (GUIOptions.mobilesecondattack <= 0 || ControllTouch.fingerKeys[11] < 0)))
		{
			return;
		}
		WeaponInfo wi = pl.currweapon.wi;
		if (wi.firetype == 2 && burstAttack > 0)
		{
			if (Time.time < tBurstAttack)
			{
				return;
			}
		}
		else if (GetClientTime() < iattacktime)
		{
			return;
		}
		if (HUD.iHealth == 0)
		{
			return;
		}
		if (HUD.iAmmoClip == 0)
		{
			ReloadWeapon();
		}
		if (wi.slot == 0 || wi.slot == 1)
		{
			if (wi.firetype != 1 || !manualAttack)
			{
				UpdateWeaponAttack();
			}
		}
		else if (wi.slot == 2)
		{
			UpdateShovelAttack();
		}
		else if (wi.slot == 3)
		{
			UpdateBlockAttack();
		}
	}

	private void UpdateZoom()
	{
		if (forceunzoom != 0f && Time.time > forceunzoom)
		{
			forceunzoom = 0f;
			if (inZoom)
			{
				UnZoom();
				PlayAnimBolt();
			}
		}
		if (Builder.active || pl.currweapon == null || lockAttack || inReload || ControllTouch.fingerKeys[5] < 0 || manualZoom)
		{
			return;
		}
		WeaponInv weaponInv = PLH.GetWeaponInv(pl, pl.currweapon.weaponname);
		if (weaponInv.scopeid == 0)
		{
			return;
		}
		int scopeid = weaponInv.scopeid;
		if (pl.currweapon.scope == null)
		{
			return;
		}
		for (int i = 0; i < pl.currweapon.scope.Length; i++)
		{
			if (pl.currweapon.scope[i].id == scopeid)
			{
				tZoom = pl.currweapon.scope[i].zoom;
				tDot = pl.currweapon.scope[i].dot;
				tGlass = pl.currweapon.scope[i].glass;
				if (tDot != null)
				{
					scope_scale = (float)tDot.width / 64f;
				}
				scope_type = pl.currweapon.scope[i].scopetype;
				scope_fov = pl.currweapon.scope[i].fov;
			}
		}
		if (!(tZoom == null) && !(tDot == null) && (inZoom || (weaponInv.wi.id != 147 && weaponInv.wi.id != 148) || GetClientTime() >= iattacktime))
		{
			if (!manualZoom)
			{
				manualZoom = true;
			}
			inZoom = !inZoom;
			if (inZoom)
			{
				SetZoom(weaponInv.wi.id);
			}
			else
			{
				UnZoom();
			}
		}
	}

	private void SetZoom(int wid)
	{
		Crosshair.SetActive(false);
		csCam.fieldOfView = scope_fov;
		VWIK.SetOffset(new Vector3(0f, -2f, 0f), Time.time, 1000f);
	}

	public static void UnZoom()
	{
		if (!(csCam == null))
		{
			inZoom = false;
			csCam.fieldOfView = 50 + GUIOptions.mobilefov * 5;
			VWIK.ClearOffset();
			Crosshair.SetActive(true);
		}
	}

	private void UpdateWeaponAttack()
	{
		if (inStuck)
		{
			return;
		}
		WeaponInv weaponInv = PLH.GetWeaponInv(pl, pl.currweapon.weaponname);
		if (weaponInv == null)
		{
			return;
		}
		WeaponInfo weaponInfo = GUIInv.GetWeaponInfo(pl.currweapon.weaponname);
		if (weaponInv.ammo == 0)
		{
			iattacktime = GetClientTime() + 250;
			HUD.SetNoAmmo();
			burstAttack = 0;
			return;
		}
		if (weaponInv.wi.firetype == 2)
		{
			if (burstAttack > 0)
			{
				burstAttack--;
			}
			else
			{
				burstAttack = 2;
			}
			tBurstAttack = Time.time + 0.03f;
		}
		if (weaponInv.wi.id == 133)
		{
			if (Time.time < overlock)
			{
				return;
			}
			overtime += 0.024f;
			overattack = Time.time + 0.5f;
			if (overtime > 1f)
			{
				overtime = 1f;
				HUD.SetOverHeat();
				overlock = Time.time + 1.5f;
				return;
			}
		}
		if (weaponInfo.id == 149)
		{
			if (flamestate < 0)
			{
				flamestate = 1;
				overattack = Time.time + 0.55f;
				PLH.Audio_Fire_Flame(pl, 0);
				return;
			}
			if (Time.time > overattack)
			{
				flamestate = 2;
				overattack = Time.time + 0.55f;
				PLH.Audio_Fire_Flame(pl, 1);
			}
			else if (flamestate == 1)
			{
				return;
			}
		}
		float num = (float)weaponInfo.firerate / 1000f;
		iattacktime = GetClientTime() + (uint)weaponInfo.firerate;
		if (pl.currweapon.ani != null)
		{
			pl.currweapon.ani.Stop();
			if (pl.currweapon.shoot2 != null && pl.currweapon.ani.clip == pl.currweapon.shoot)
			{
				pl.currweapon.ani.clip = pl.currweapon.shoot2;
			}
			else
			{
				pl.currweapon.ani.clip = pl.currweapon.shoot;
			}
			pl.currweapon.ani.Play();
		}
		weaponInv.ammo--;
		HUD.SetAmmo(weaponInv.ammo, weaponInfo.ammo);
		VWIK.AddOffset(new Vector3(0f, 0.001f, -0.01f), Time.time, num * (float)weaponInfo.recoil * 0.25f);
		VWIK.AddAngle(new Vector3(-0.1f, 0f, 0f), Time.time, num * (float)weaponInfo.recoil * 0.25f);
		if (weaponInfo.id != 149)
		{
			PLH.Audio_Fire(pl);
		}
		VWIK.CameraAddOffset(new Vector3(0f, 0f, -0.05f), Time.time, num);
		VWIK.CameraAddAngle(new Vector3(0f, 0f, (float)UnityEngine.Random.Range(-5, 5) * 0.05f), Time.time, num);
		float num2 = num * (float)weaponInfo.recoil;
		if (num2 > 0.5f)
		{
			num2 = 0.5f;
		}
		VWIK.CameraAddAngle(new Vector3((float)(-weaponInfo.recoil) * 0.05f, 0f, 0f), Time.time, num2);
		if (weaponInfo.id == 67 || weaponInfo.id == 71 || weaponInfo.id == 62)
		{
			SetRecoil(1, 1f);
		}
		else if (weaponInfo.id == 49)
		{
			SetRecoil(1, 2f);
		}
		else if (weaponInfo.id == 12 || weaponInfo.id == 57)
		{
			SetRecoil(1, 2f);
		}
		else if (weaponInfo.id == 66)
		{
			SetRecoil(4, 1f);
		}
		else if (weaponInfo.id == 51)
		{
			SetRecoil(1, 3f);
		}
		else if (weaponInfo.id == 14)
		{
			SetRecoil(2, 1f);
		}
		else if (weaponInfo.id == 54)
		{
			SetRecoil(2, 1.25f);
		}
		else if (weaponInfo.id == 22 || weaponInfo.id == 161 || weaponInfo.id == 162)
		{
			SetRecoil(1, 3f);
		}
		else if (weaponInfo.id == 93 || weaponInfo.id == 113)
		{
			SetRecoil(2, 2f);
		}
		else if (weaponInfo.id == 31)
		{
			SetRecoil(2, 6f);
		}
		else if (weaponInfo.id == 11 || weaponInfo.id == 13)
		{
			SetRecoil(2, 1f);
		}
		else if (weaponInfo.id == 80)
		{
			SetRecoil(2, 0.5f);
		}
		else if (weaponInfo.id == 146)
		{
			SetRecoil(2, 0.25f);
		}
		else if (weaponInfo.id == 90)
		{
			SetRecoil(2, 0.2f);
		}
		else if (weaponInfo.id == 85)
		{
			SetRecoil(2, 0.75f);
		}
		else if (weaponInfo.id == 87)
		{
			SetRecoil(2, 2f);
		}
		else if (weaponInfo.id == 79)
		{
			SetRecoil(2, 1.5f);
		}
		else if (weaponInfo.id == 76)
		{
			SetRecoil(5, 0.2f);
		}
		else if (weaponInfo.id == 129)
		{
			SetRecoil(2, 1.24f);
		}
		else if (weaponInfo.id == 133)
		{
			SetRecoil(5, 0.1f);
		}
		else if (weaponInfo.id == 134)
		{
			SetRecoil(2, 1.1f);
		}
		else if (weaponInfo.id == 84)
		{
			SetRecoil(4, 0.05f);
			SetRecoil(2, 0.15f);
		}
		else if (weaponInfo.id == 91)
		{
			SetRecoil(4, 0.1f);
			SetRecoil(2, 0.1f);
		}
		if (pl.currweapon.goMuzzle != null && weaponInfo.id != 20 && weaponInfo.id != 40 && weaponInfo.id != 14 && weaponInfo.id != 88 && weaponInfo.id != 143 && weaponInfo.id != 149 && weaponInfo.id != 42)
		{
			if (weaponInfo.id == 170)
			{
				pl.currweapon.goMuzzle.transform.localEulerAngles = new Vector3(0f, 90f, UnityEngine.Random.Range(0, 2) * 180);
			}
			else
			{
				pl.currweapon.goMuzzle.transform.localEulerAngles = new Vector3(0f, 90f, UnityEngine.Random.Range(0, 360));
			}
			float num3 = UnityEngine.Random.Range(30f, 27f);
			pl.currweapon.goMuzzle.transform.localScale = new Vector3(num3, num3, 0f);
			pl.currweapon.goMuzzle.SetActive(true);
			pl.tM = Time.time + 0.03f;
		}
		Vector3 pos = pl.currweapon.goMuzzle.transform.position;
		Vector3 eulerAngles = pl.currweapon.goMuzzle.transform.eulerAngles;
		if (inZoom)
		{
			pos = trCamera.transform.position + trCamera.transform.forward - Vector3.up * 0.25f;
			eulerAngles = trCamera.transform.eulerAngles;
		}
		GOP.Create(1, pos, eulerAngles, Vector3.zero, Color.black);
		Vector3 pos2 = pl.currweapon.goMuzzle.transform.position;
		Vector3 eulerAngles2 = pl.currweapon.goMuzzle.transform.eulerAngles;
		if (inZoom)
		{
			pos2 = trCamera.transform.position + trCamera.transform.forward - Vector3.up * 0.25f;
			eulerAngles2 = trCamera.transform.eulerAngles;
		}
		GOP.Create(2, pos2, eulerAngles2, Vector3.zero, Color.black);
		if (weaponInfo.id == 149)
		{
			Vector3 pos3 = pl.currweapon.goMuzzle.transform.position;
			Vector3 rot = pl.currweapon.goMuzzle.transform.eulerAngles + Vector3.up * 90f + trCamera.transform.localEulerAngles;
			if (inZoom)
			{
				pos3 = trCamera.transform.position + trCamera.transform.forward - Vector3.up * 0.25f;
				rot = trCamera.transform.eulerAngles + Vector3.up * 90f;
			}
			GOP.Create(22, pos3, rot, Vector3.zero, Color.black);
		}
		inAttack = true;
		if (pl.currweapon.shelltype > 0)
		{
			GOP.Create(10, Vector3.zero, Vector3.zero, Vector3.zero, Color.black);
		}
		float num4 = (float)(100 - weaponInfo.accuracy) / 100f;
		if ((weaponInfo.id == 80 || weaponInfo.id == 146) && Time.time >= recoilend)
		{
			num4 *= 0.5f;
		}
		float num5 = (float)(100 - weaponInfo.distance) / 100f;
		if (num5 < 0f)
		{
			num5 = 0.01f;
		}
		float num6 = num4 * num5;
		float dist = 256f;
		int num7 = 1;
		if (weaponInfo.id == 15 || weaponInfo.id == 16 || weaponInfo.id == 52)
		{
			num7 = 6;
			num6 = 0.5f;
			SetRecoil(3, 2f);
			dist = 28f;
		}
		else if (weaponInfo.id == 94)
		{
			num7 = 5;
			num6 = 0.4f;
			SetRecoil(2, 2f);
			dist = 32f;
		}
		else if (weaponInfo.id == 81 || weaponInfo.id == 173)
		{
			num7 = 5;
			num6 = 0.6f;
			SetRecoil(2, 2f);
			dist = 26f;
		}
		else if (weaponInfo.id == 64)
		{
			num7 = 5;
			num6 = 0.45f;
			SetRecoil(2, 2f);
			dist = 32f;
		}
		else if (weaponInfo.id == 164)
		{
			num7 = 5;
			num6 = 0.55f;
			SetRecoil(3, 2f);
			dist = 32f;
		}
		else if (weaponInfo.id == 170)
		{
			dist = 12f;
		}
		if ((weaponInfo.id == 12 || weaponInfo.id == 66 || weaponInfo.id == 51 || weaponInfo.id == 57) && !inZoom)
		{
			num6 *= 1000f;
			PlayAnimBolt();
		}
		if (weaponInfo.id == 56)
		{
			if (!inZoom)
			{
				num6 *= 500f;
			}
			else if (movex != 0f || movez != 0f)
			{
				num6 *= 500f;
			}
		}
		if (weaponInfo.id == 13 || weaponInfo.id == 72)
		{
			if (inZoom)
			{
				forceunzoom = Time.time + 0.3f;
			}
			else
			{
				PlayAnimBolt();
			}
		}
		if (weaponInfo.id == 147 || weaponInfo.id == 148 || weaponInfo.id == 79)
		{
			if (inZoom)
			{
				forceunzoom = Time.time + 0.01f;
			}
			else
			{
				num6 *= 1000f;
			}
		}
		if (weaponInfo.id == 149)
		{
			dist = 10f;
		}
		Vector3 position = trCamera.transform.position;
		attacklist.Clear();
		if (gamemode == 6 && weaponInfo.name == "icegun")
		{
			Client.cs.send_attackthrow(trCamera.position, trCamera.forward, rx);
			Client.cs.send_attack(position, GetServerTime(), attacklist);
			return;
		}
		if (weaponInfo.id >= 1000 && weaponInfo.name == "panzerfaust")
		{
			Client.cs.send_attackthrow(trCamera.position, trCamera.forward, rx);
			Client.cs.send_attack(position, GetServerTime(), attacklist);
			VWIK.SetCameraShake(0.5f, 3f);
			GameObject gameObject = GameObject.Find(pl.currweapon.go.name + "/" + pl.currweapon.goWeapon.name + "/drop/Model");
			GameObject gameObject2 = GameObject.Find(pl.currweapon.go.name + "/" + pl.currweapon.goWeapon.name + "/model/Model");
			if (gameObject != null && gameObject2 != null)
			{
				gameObject.GetComponent<MeshRenderer>().enabled = true;
				gameObject2.GetComponent<MeshRenderer>().enabled = false;
			}
			return;
		}
		for (int i = 0; i < num7; i++)
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(0f - num6, num6), UnityEngine.Random.Range(0f - num6, num6), UnityEngine.Random.Range(0f - num6, num6));
			if (weaponInfo.id != 149 && weaponInfo.id != 170)
			{
				CreateTracer(vector);
			}
			bool flag = false;
			RaycastHit hit;
			flag = ((weaponInfo.id != 66 && weaponInfo.id != 48 && weaponInfo.id != 78 && weaponInfo.id != 53 && weaponInfo.id != 72 && weaponInfo.id != 79) ? Raycast(position, trCamera.transform.forward + vector * 0.1f, out hit, dist) : Bulletcast(position, trCamera.transform.forward + vector * 0.1f, out hit));
			if (flag && !CheckPlayerDamage(hit, weaponInfo))
			{
				Vector3 vector2 = hit.point + hit.normal * -0.5f;
				if (Map.GetBlock(vector2) > 0)
				{
					GOP.Create(0, hit.point, Vector3.zero, Vector3.zero, Map.GetLastColorFixed());
					CreateHole(hit, false, vector2);
					Client.cs.send_attackblock((int)vector2.x, (int)vector2.y, (int)vector2.z);
				}
			}
		}
		for (int j = 0; j < attacklist.Count; j++)
		{
			int num8 = (int)Vector3.Distance(position, attacklist[j].hitpos);
			Vector3 normalized = (attacklist[j].hitpos - position).normalized;
			RaycastHit hit2;
			if (VoxCast.RayCast(position, normalized, out hit2, num8))
			{
				return;
			}
		}
		Client.cs.send_attack(position, GetServerTime(), attacklist);
	}

	private void UpdateShovelAttack()
	{
		if (DemoRec.isDemo() || PLH.GetWeaponInv(pl, pl.currweapon.weaponname) == null)
		{
			return;
		}
		WeaponInfo weaponInfo = GUIInv.GetWeaponInfo(pl.currweapon.weaponname);
		iattacktime = GetClientTime() + (uint)weaponInfo.firerate;
		VWIK.shovel_attack = Time.time;
		VWIK.shovel_hit = false;
		inAttack = true;
		PLH.Audio_Fire(pl);
		if (pl.currweapon.ani != null)
		{
			pl.currweapon.ani.Stop();
			if (pl.currweapon.shoot2 != null && pl.currweapon.ani.clip == pl.currweapon.shoot)
			{
				pl.currweapon.ani.clip = pl.currweapon.shoot2;
			}
			else
			{
				pl.currweapon.ani.clip = pl.currweapon.shoot;
			}
			pl.currweapon.ani.Play();
		}
		Vector3 position = trCamera.transform.position;
		RaycastHit hit;
		if (!Raycast(position, trCamera.transform.forward, out hit, 3f))
		{
			Client.cs.send_attack(Vector3.zero, GetServerTime(), null);
			return;
		}
		attacklist.Clear();
		if (CheckPlayerDamage(hit, weaponInfo))
		{
			Client.cs.send_attack(position, GetServerTime(), attacklist);
			VWIK.shovel_hit = true;
			return;
		}
		Vector3 pos = hit.point + hit.normal * -0.5f;
		if (Map.GetBlock(pos) > 0)
		{
			GOP.Create(0, hit.point, Vector3.zero, Vector3.zero, Map.GetLastColorFixed());
			Client.cs.send_attackblock((int)pos.x, (int)pos.y, (int)pos.z);
			Client.cs.send_attack(Vector3.zero, GetServerTime(), null);
			if (sShovelBlock == null)
			{
				sShovelBlock = ContentLoader_.LoadAudio("shovelblock");
			}
			pl.asFX.clip = sShovelBlock;
			pl.asFX.Play();
			GOP.Create(9, new Vector3((int)pos.x, (int)pos.y, (int)pos.z), Vector3.zero, Vector3.zero, Color.black, 1);
			VWIK.shovel_hit = true;
		}
	}

	private void UpdateBlockAttack()
	{
		if (inStuck || blockCursor.x < 0f)
		{
			return;
		}
		WeaponInv weaponInv = PLH.GetWeaponInv(pl, pl.currweapon.weaponname);
		if (weaponInv == null || weaponInv.backpack <= 0)
		{
			return;
		}
		iattacktime = GetClientTime() + 200;
		VWIK.shovel_attack = Time.time;
		inAttack = true;
		int num = (int)blockCursor.x;
		int num2 = (int)blockCursor.y;
		int num3 = (int)blockCursor.z;
		if (sblock != null)
		{
			int i = 0;
			for (int num4 = sblock.Length; i < num4; i++)
			{
				if (sblock[i].inBlock(num, num2, num3))
				{
					return;
				}
			}
		}
		weaponInv.backpack--;
		HUD.SetBackPack(weaponInv.backpack);
		Client.cs.send_buildblock(num, num2, num3);
	}

	private void UpdateReload()
	{
		if (inReload && Time.time > reload_end)
		{
			inReload = false;
			reload_result = 0f;
			if (sReloadEnd == null)
			{
				sReloadEnd = ContentLoader_.LoadAudio("reload_end");
			}
			if (pl != null && pl.asFX != null)
			{
				pl.asFX.PlayOneShot(sReloadEnd);
			}
		}
	}

	private void UpdateKeyMenu()
	{
		if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.BackQuote)) && MainManager.state == 1)
		{
			GUIGameMenu.Toggle();
		}
		if (Input.GetKeyUp(KeyCode.F9))
		{
			SetFullScreen();
		}
		if (Input.GetKeyDown(GUIOptions.keyChangeSet) && !Builder.active && pl.idx >= 0)
		{
			GUIGameSet.Toggle();
			HUD.SetHint(-1);
		}
		if (Input.GetKeyDown(GUIOptions.keyChangeTeam) && !Builder.active)
		{
			int team = ((pl.team == 0) ? 1 : 0);
			Client.cs.send_changeteam(team);
		}
		if (Input.GetKeyDown(KeyCode.Tab) && ((!Builder.active && pl.idx >= 0) || DemoRec.isDemo()))
		{
			HUDTab.SetActive(true);
		}
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			HUDTab.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.H) && !Builder.active && pl.idx >= 0)
		{
			HUDKiller.SetActive(true);
			HUDKiller.tautoclose = Time.time + 30f;
		}
		if (Input.GetKeyUp(KeyCode.H))
		{
			HUDKiller.SetActive(false);
		}
		if (!Builder.active)
		{
			if (Input.GetKeyUp(GUIOptions.keyChat))
			{
				HUDMessage.SetChatEdit(true);
			}
			if (Input.GetKeyUp(GUIOptions.keyTeamChat))
			{
				HUDMessage.SetChatEdit(true, 1);
			}
			if (Input.GetKeyUp(GUIOptions.keyClanChat) && GUIClan.clanid > 0)
			{
				HUDMessage.SetChatEdit(true, 2);
			}
		}
		if (lockcursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		if (Input.GetKeyDown(GUIOptions.keySpecial))
		{
			UseSpecial();
		}
	}

	public static void UseSpecial()
	{
		if (forceweapon || pl.bstate == 5 || pl.wset[pl.currset] == null || pl.wset[pl.currset].w[4] == null || pl.wset[pl.currset].w[4].wi.id == 110 || !(Time.time > specialtime_end))
		{
			return;
		}
		Client.cs.send_attackspecial(trCamera.position, trCamera.forward, rx);
		specialtime_duration = GUIInv.GetWeaponInfo(pl.wset[pl.currset].w[4].wi.name).firerate;
		specialtime_start = Time.time;
		specialtime_end = specialtime_start + specialtime_duration;
		if (pl.wset[pl.currset].w[4].wi.id == 108)
		{
			VWIK.SetGrenade();
			VWIK.AddAngle(new Vector3(45f, 0f, 0f), Time.time + 0.25f, 0.2f);
			if (sThrow == null)
			{
				sThrow = Resources.Load("sounds/throw") as AudioClip;
			}
			if (pl != null && pl.asFX != null)
			{
				pl.asFX.PlayOneShot(sThrow);
			}
		}
		else if (pl.wset[pl.currset].w[4].wi.id == 107 || pl.wset[pl.currset].w[4].wi.id == 109)
		{
			VWIK.SetDrop();
			VWIK.AddAngle(new Vector3(45f, 0f, 0f), Time.time + 0.4f, 0.2f);
		}
	}

	private static void PlayAnimBolt()
	{
		Vector3 vector = new Vector3(0.25f, 0.25f, 0.75f);
		VWIK.WeaponAddAngle(new Vector3(25f, 0f, 0f), Time.time, 0.1f, true);
		VWIK.WeaponSetAngle(new Vector3(25f, 0f, 0f), Time.time + 0.1f, 0.5f);
		VWIK.WeaponAddAngle(new Vector3(25f, 0f, 0f), Time.time + 0.6f, 0.1f);
		VWIK.AddAngle(new Vector3(-10f, 0f, 0f), Time.time, 0.3f, true);
		VWIK.SetAngle(new Vector3(-10f, 0f, 0f), Time.time + 0.3f, 0.3f);
		VWIK.AddAngle(new Vector3(-10f, 0f, 0f), Time.time + 0.6f, 0.1f);
		VWIK.AddOffset(new Vector3(0f, -0.05f, 0f), Time.time, 0.3f, true);
		VWIK.SetOffset(new Vector3(0f, -0.05f, 0f), Time.time + 0.3f, 0.3f);
		VWIK.AddOffset(new Vector3(0f, -0.05f, 0f), Time.time + 0.6f, 0.1f);
		VWIK.CameraAddAngle(new Vector3(-1f, 1f, 0f), Time.time, 0.3f, true);
		VWIK.CameraSetAngle(new Vector3(-1f, 1f, 0f), Time.time + 0.3f, 0.3f);
		VWIK.CameraAddAngle(new Vector3(-1f, 1f, 0f), Time.time + 0.6f, 0.1f);
		VWIK.BoltAddOffset(vector, new Vector3(0.25f, 0.25f, -0.25f), Time.time + 0.1f, 0.15f);
		VWIK.BoltAddOffset(new Vector3(0.25f, 0.25f, -0.25f), new Vector3(0.25f, 0.25f, -0.25f), Time.time + 0.25f, 0.1f);
		VWIK.BoltAddOffset(new Vector3(0.25f, 0.25f, -0.25f), vector, Time.time + 0.35f, 0.15f);
		VWIK.BoltAddOffset(vector, new Vector3(0.25f, 0f, 0f), Time.time + 0.5f, 0.05f);
	}

	public static void SetFullScreen()
	{
		if (Screen.fullScreen)
		{
			Screen.fullScreen = false;
			return;
		}
		if (GUIOptions.res < 0 || GUIOptions.res > Screen.resolutions.Length - 1)
		{
			GUIOptions.res = Screen.resolutions.Length - 1;
		}
		Resolution resolution = Screen.resolutions[GUIOptions.res];
		Screen.SetResolution(resolution.width, resolution.height, true);
	}

	private void UpdateKeyWeapon()
	{
		if (!Builder.active && pl.bstate != 5)
		{
			if (Input.GetKeyDown(GUIOptions.keyPrevWeapon))
			{
				ChangeWeapon(prevweaponinput);
			}
			if (Input.GetKeyDown(GUIOptions.keyPrimary))
			{
				ChangeWeapon(0);
			}
			if (Input.GetKeyDown(GUIOptions.keySecondary))
			{
				ChangeWeapon(1);
			}
			if (Input.GetKeyDown(GUIOptions.keyShovel))
			{
				ChangeWeapon(2);
			}
			if (Input.GetKeyDown(GUIOptions.keyBlock))
			{
				ChangeWeapon(3);
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				ReloadWeapon();
			}
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis > 0f && Time.time > twheelchange)
			{
				ChangeWeapon(nextweaponkey);
				twheelchange = Time.time + 0.1f;
			}
			else if (axis < 0f && Time.time > twheelchange)
			{
				twheelchange = Time.time + 0.1f;
				ChangeWeapon(prevweaponkey);
			}
		}
	}

	public static bool ChangeWeapon(int id)
	{
		if (pl.team == 2)
		{
			return false;
		}
		if (forceweapon)
		{
			return false;
		}
		if (blockslot > 0)
		{
			if (blockslot == 1 && (id == 0 || id == 1))
			{
				id = 2;
			}
			else if (blockslot == 2 && id == 1)
			{
				id = 0;
			}
			else if (blockslot == 3 && id == 0)
			{
				id = 1;
			}
		}
		prevweaponkey = id - 1;
		if (prevweaponkey < 0)
		{
			prevweaponkey = 0;
		}
		nextweaponkey = id + 1;
		if (nextweaponkey > 3)
		{
			nextweaponkey = 3;
		}
		if (id < 0)
		{
			return false;
		}
		if (pl.rw[id] > 0)
		{
			int num = pl.rw[id];
			if (pl.currweapon != null && pl.currweapon.wi.id == num)
			{
				return false;
			}
			prevweaponinput = currweaponinput;
			currweaponinput = id;
			UnZoom();
			forceunzoom = 0f;
			PLH.SelectWeapon(pl, GUIInv.winfo[num].name);
		}
		else
		{
			WeaponSet weaponSet = pl.wset[pl.currset];
			if (weaponSet == null)
			{
				return false;
			}
			if (weaponSet.w[id] == null)
			{
				return false;
			}
			if (pl.currweapon != null && pl.currweapon.weaponname == weaponSet.w[id].wi.name)
			{
				return false;
			}
			prevweaponinput = currweaponinput;
			currweaponinput = id;
			UnZoom();
			forceunzoom = 0f;
			PLH.SelectWeapon(pl, weaponSet.w[id].wi.name);
		}
		if (pl.currweapon != null && pl.currweapon.ani != null)
		{
			pl.currweapon.ani.Stop();
			pl.currweapon.ani.clip = pl.currweapon.wield;
			pl.currweapon.ani.Play();
		}
		return true;
	}

	public static void ReloadWeapon()
	{
		if (DemoRec.isDemo() || pl.currweapon == null || pl.bstate == 5)
		{
			return;
		}
		if (inReload)
		{
			if (Time.time < reload_start + 0.5f || reload_result > 0f)
			{
				return;
			}
			float num = GUIM.YRES(296f);
			float num2 = GUIM.YRES(16f);
			float num3 = (Time.time - reload_start) / (reload_end - reload_start);
			num3 *= num3;
			float num4 = num * num3;
			float num5 = num * reload_active;
			if (num4 >= num5 && num4 <= num5 + num2)
			{
				reload_result = 1f;
				reload_end = Time.time + 0.1f;
				reload_end_msg = reload_end + 0.3f;
				reload_count++;
				if (reload_count > 3)
				{
					reload_count = 0;
				}
				VWIK.ClearOffset();
				VWIK.AddOffset(new Vector3(0.2f, -0.1f, 0f), Time.time, 0.1f);
				VWIK.AddAngle(new Vector3(0f, -60f, 0f), Time.time, 0.1f);
				if (sReloadActive == null)
				{
					sReloadActive = ContentLoader_.LoadAudio("reload_active");
				}
				pl.asFX.PlayOneShot(sReloadActive);
				Client.cs.send_reloadactive();
			}
			else
			{
				reload_result = 2f;
				reload_count = 0;
				reload_end_msg = 0f;
				if (sReloadBad == null)
				{
					sReloadBad = ContentLoader_.LoadAudio("reload_bad");
				}
				pl.asFX.PlayOneShot(sReloadBad);
			}
			reload_forcept = num4 / num;
		}
		else
		{
			if (Time.time < reload_end + 0.25f)
			{
				return;
			}
			WeaponInv weaponInv = PLH.GetWeaponInv(pl, pl.currweapon.weaponname);
			WeaponInfo weaponInfo = GUIInv.GetWeaponInfo(pl.currweapon.weaponname);
			if (weaponInv.ammo != weaponInfo.ammo && weaponInv.backpack != 0)
			{
				if (pl.currweapon != null && pl.currweapon.ani != null)
				{
					pl.currweapon.ani.clip = null;
				}
				UnZoom();
				inReload = true;
				reload_start = Time.time;
				reload_end = reload_start + 2f;
				reload_active = (float)UnityEngine.Random.Range(25, 50) / 100f;
				Client.cs.send_reload();
				burstAttack = 0;
				VWIK.AddOffset(new Vector3(0.2f, -0.1f, 0f), Time.time, 0.2f, true);
				VWIK.AddAngle(new Vector3(0f, -60f, 0f), Time.time, 0.2f, true);
				VWIK.SetOffset(new Vector3(0.2f, -0.1f, 0f), Time.time + 0.2f, 1.7f);
				VWIK.SetAngle(new Vector3(0f, -60f, 0f), Time.time + 0.2f, 1.7f);
				VWIK.AddOffset(new Vector3(0.2f, -0.1f, 0f), Time.time + 1.9f, 0.1f);
				VWIK.AddAngle(new Vector3(0f, -60f, 0f), Time.time + 1.9f, 0.1f);
				if (sReloadStart == null)
				{
					sReloadStart = ContentLoader_.LoadAudio("reload_start");
				}
				pl.asFX.PlayOneShot(sReloadStart);
			}
		}
	}

	public static void ReloadWeaponEnd(int slot)
	{
		if (DemoRec.isDemo())
		{
			return;
		}
		WeaponInv weaponInv = null;
		WeaponInfo weaponInfo = null;
		if (pl.rw[slot] > 0)
		{
			int wid = pl.rw[slot];
			weaponInv = pl.GetWeaponInvExtra(wid);
			weaponInfo = GUIInv.GetWeaponInfo(wid);
		}
		else
		{
			if (pl.wset[pl.currset].w[slot] == null)
			{
				return;
			}
			weaponInv = pl.wset[pl.currset].w[slot];
			weaponInfo = GUIInv.GetWeaponInfo(weaponInv.wi.name);
		}
		if (weaponInv != null && weaponInfo != null)
		{
			int ammo = weaponInfo.ammo;
			int num = weaponInv.ammo + weaponInv.backpack;
			if (num < weaponInfo.ammo)
			{
				ammo = num;
				num = 0;
			}
			else
			{
				num -= weaponInfo.ammo;
			}
			weaponInv.ammo = ammo;
			weaponInv.backpack = num;
			if (pl.currweapon != null && pl.currweapon.weaponname == weaponInv.wi.name)
			{
				HUD.SetAmmo(weaponInv.ammo, weaponInfo.ammo);
				HUD.SetBackPack(weaponInv.backpack);
			}
		}
	}

	private void UpdateKey()
	{
		if (Input.GetKeyDown(GUIOptions.keyCrouch))
		{
			inDuckKey = true;
		}
		else if (Input.GetKeyUp(GUIOptions.keyCrouch))
		{
			inDuckKey = false;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			inJumpKey = true;
			inJumpKeyPressed = false;
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			inJumpKey = false;
		}
		if (Input.GetKeyUp(KeyCode.L) && !HUDMessage.showchatedit)
		{
			if (Cursor.lockState == CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
		if (!DemoRec.isDemo())
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			int num = specid;
			for (int i = num + 1; i < 40; i++)
			{
				if (PLH.player[i] != null && !(PLH.player[i].go == null) && PLH.player[i].bstate != 5 && PLH.player[i].team != 2)
				{
					num = i;
					break;
				}
			}
			if (num == specid)
			{
				for (int j = 0; j < num; j++)
				{
					if (PLH.player[j] != null && !(PLH.player[j].go == null) && PLH.player[j].bstate != 5 && PLH.player[j].team != 2)
					{
						num = j;
						break;
					}
				}
			}
			SetCamMode(num);
		}
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			int num2 = specid;
			for (int num3 = num2 - 1; num3 >= 0; num3--)
			{
				if (PLH.player[num3] != null && !(PLH.player[num3].go == null) && PLH.player[num3].bstate != 5 && PLH.player[num3].team != 2)
				{
					num2 = num3;
					break;
				}
			}
			if (num2 == specid)
			{
				for (int num4 = 39; num4 > num2; num4--)
				{
					if (PLH.player[num4] != null && !(PLH.player[num4].go == null) && PLH.player[num4].bstate != 5 && PLH.player[num4].team != 2)
					{
						num2 = num4;
						break;
					}
				}
			}
			SetCamMode(num2);
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			specmode++;
			if (specmode > 1)
			{
				specmode = 0;
			}
		}
	}

	private void UpdateMove2()
	{
		if (trControll == null || lockMove || pl.team == 2)
		{
			return;
		}
		UpdateMoveSpeed();
		UpdateMoveKey();
		if (inFreeze)
		{
			if (Time.time > tFreeze)
			{
				inFreeze = false;
				if (ccc != null)
				{
					ccc.saturation = 1f;
				}
			}
			movex = 0f;
			movez = 0f;
		}
		prevPos.Set(Pos.x, Pos.y, Pos.z);
		fdeltatime = fnextFrame - fcurrFrame;
		Vector3 forward = trControll.forward;
		Vector3 right = trControll.right;
		Vector3 normalized = (forward * movex + right * movez).normalized;
		if (inAir)
		{
			int inAirFrame2 = inAirFrame;
			int num = 1;
			vel = Movement.MoveAir(Vector3.zero, vel);
		}
		else
		{
			vel = Movement.MoveGround(normalized, vel);
		}
		if (vel.magnitude * speed < 0.01f)
		{
			speed = 0f;
		}
		bool flag = false;
		if (speed != 0f)
		{
			nextPos0 = Pos + new Vector3(vel.x, 0f, 0f) * speed;
			nextPos1 = Pos + new Vector3(0f, 0f, vel.z) * speed;
			nextPos = Pos + vel * speed;
			if (VUtil.isValidBBox(nextPos, radius, boxheight))
			{
				Pos = nextPos;
			}
			else if (!inAir && VUtil.isValidBBox(nextPos + Vector3.up, radius, boxheight))
			{
				Pos = nextPos + Vector3.up;
				flag = true;
			}
			else if (VUtil.isValidBBox(nextPos0, radius, boxheight))
			{
				Pos = nextPos0;
			}
			else if (!inAir && VUtil.isValidBBox(nextPos0 + Vector3.up, radius, boxheight))
			{
				Pos = nextPos0 + Vector3.up;
				flag = true;
			}
			else if (VUtil.isValidBBox(nextPos1, radius, boxheight))
			{
				Pos = nextPos1;
			}
			else if (!inAir && VUtil.isValidBBox(nextPos1 + Vector3.up, radius, boxheight))
			{
				Pos = nextPos1 + Vector3.up;
				flag = true;
			}
		}
		if (!flag)
		{
			UpdateMoveJump();
		}
		fcurrFrame = Time.realtimeSinceStartup - fdifFrame;
		fnextFrame = fcurrFrame + cl_fps;
		Pos = ClampPosClip(Pos);
	}

	private void UpdateSendPos()
	{
		if (!(trControll == null) && pl.bstate != 5 && pl.team != 2)
		{
			float x = trCamera.localEulerAngles.x;
			float y = trControll.eulerAngles.y;
			KeyBase keyBase = (KeyBase)0;
			if (movex > 0f)
			{
				keyBase |= KeyBase.plus_x;
			}
			if (movex < 0f)
			{
				keyBase |= KeyBase.minus_x;
			}
			if (movez > 0f)
			{
				keyBase |= KeyBase.plus_z;
			}
			if (movez < 0f)
			{
				keyBase |= KeyBase.minus_z;
			}
			if (inAir)
			{
				keyBase |= KeyBase.jump;
			}
			if (inDuck)
			{
				keyBase |= KeyBase.duck;
			}
			Client.cs.send_pos_dev(Pos.x, Pos.y, Pos.z, x, y, (byte)keyBase, GetServerTime());
		}
	}

	private void UpdateMoveSpeed()
	{
		if (inDuckKey && !inDuck)
		{
			inDuck = true;
			camheight = 1.65f;
			boxheight = 1.9f;
		}
		else if (!inDuckKey && inDuck && VUtil.isValidBBox(trControll.position, radius, 2.75f))
		{
			inDuck = false;
			camheight = 2.4f;
			boxheight = 2.75f;
		}
		if (inDuck)
		{
			speed = default_speed / 2f;
		}
		else
		{
			speed = default_speed;
		}
		if (Pos.y < Map.WATER_LEVEL)
		{
			speed *= 0.5f;
		}
		if (inSlow)
		{
			if (zombieslowdown > Time.time)
			{
				speed *= 0.5f;
			}
			else
			{
				inSlow = false;
			}
		}
		if (pl.currweapon != null && !DemoRec.isDemo())
		{
			WeaponInfo weaponInfo = GUIInv.GetWeaponInfo(pl.currweapon.weaponname);
			speed *= (float)weaponInfo.mobility * 0.01f;
		}
	}

	private void UpdateMoveKey()
	{
		movex = ControllTouch.movex;
		movez = ControllTouch.movez;
		if (Input.GetKey(GUIOptions.keyForward) || Input.GetKey(KeyCode.UpArrow))
		{
			movex = 1f;
		}
		else if (Input.GetKey(GUIOptions.keyBackward) || Input.GetKey(KeyCode.DownArrow))
		{
			movex = -1f;
		}
		if (Input.GetKey(GUIOptions.keyStrafeRight) || Input.GetKey(KeyCode.RightArrow))
		{
			movez = 1f;
		}
		else if (Input.GetKey(GUIOptions.keyStrafeLeft) || Input.GetKey(KeyCode.LeftArrow))
		{
			movez = -1f;
		}
	}

	private void UpdateMoveJump()
	{
		if (inJumpKey && !inAir && !inJumpKeyPressed && velocity.y <= 0f)
		{
			velocity = new Vector3(0f, 2.35f, 0f);
			inJumpKeyPressed = true;
			VWIK.AddOffset(Vector3.down * 0.002f, Time.time, 0.5f);
		}
	}

	private void UpdateLook()
	{
		if (trControll == null || lockLook || pl.team == 2)
		{
			return;
		}
		float num = 1f;
		float num2 = 50 + GUIOptions.mobilefov * 5;
		if (csCam.fieldOfView != num2)
		{
			num = csCam.fieldOfView / num2 * zoomsens / 3f;
		}
		if (MouseFilter)
		{
			m_MouseLookSmoothMove.x = ControllTouch.tx;
			m_MouseLookSmoothMove.y = ControllTouch.ty;
		}
		else
		{
			rx += ControllTouch.tx * sens * num;
			ry -= ControllTouch.ty * sens * num;
		}
		if (MouseFilter)
		{
			if (m_LastMouseLookFrame == Time.frameCount)
			{
				return;
			}
			m_LastMouseLookFrame = Time.frameCount;
			while (m_MouseLookSmoothBuffer.Count > MouseLookSmoothSteps)
			{
				m_MouseLookSmoothBuffer.RemoveAt(0);
			}
			m_MouseLookSmoothBuffer.Add(m_MouseLookSmoothMove);
			float num3 = 1f;
			Vector2 zero = Vector2.zero;
			float num4 = 0f;
			for (int num5 = m_MouseLookSmoothBuffer.Count - 1; num5 > 0; num5--)
			{
				zero += m_MouseLookSmoothBuffer[num5] * num3;
				num4 += 1f * num3;
				num3 *= MouseLookSmoothWeight / Time.deltaTime * 60f;
			}
			num4 = Mathf.Max(1f, num4);
			m_CurrentMouseLook = NaNSafeVector2(zero / num4);
			rx += m_CurrentMouseLook.x * sens * num;
			ry -= m_CurrentMouseLook.y * sens * num;
		}
		rx = ClampAngle(rx, -360f, 360f);
		ry = ClampAngle(ry, -90f, 90f);
		trControll.eulerAngles = new Vector3(0f, rx, 0f);
		trCamera.localEulerAngles = new Vector3(ry, 0f, 0f);
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		angle %= 360f;
		if (angle >= -360f && angle <= 360f)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
		}
		return Mathf.Clamp(angle, min, max);
	}

	public static Vector3 ClampPosMap(Vector3 pos)
	{
		float num = pos.x;
		float num2 = pos.y;
		float num3 = pos.z;
		if (num < 0.5f)
		{
			num = 0.5f;
		}
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		if (num3 < 0.5f)
		{
			num3 = 0.5f;
		}
		if (num > (float)Map.BLOCK_SIZE_X - 0.5f)
		{
			num = (float)Map.BLOCK_SIZE_X - 0.5f;
		}
		if (num2 > (float)Map.BLOCK_SIZE_Y - boxheight)
		{
			num2 = (float)Map.BLOCK_SIZE_Y - boxheight;
		}
		if (num3 > (float)Map.BLOCK_SIZE_Z - 0.5f)
		{
			num3 = (float)Map.BLOCK_SIZE_Z - 0.5f;
		}
		return new Vector3(num, num2, num3);
	}

	public static Vector3 ClampPosClip(Vector3 pos)
	{
		float num = pos.x;
		float num2 = pos.y;
		float num3 = pos.z;
		if (num < Map.clip[0].x + 0.5f)
		{
			num = Map.clip[0].x + 0.5f;
		}
		if (num2 < Map.clip[0].y)
		{
			num2 = Map.clip[0].y;
		}
		if (num3 < Map.clip[0].z + 0.5f)
		{
			num3 = Map.clip[0].z + 0.5f;
		}
		if (num > Map.clip[1].x - 0.5f)
		{
			num = Map.clip[1].x - 0.5f;
		}
		if (num2 > Map.clip[1].y - boxheight)
		{
			num2 = Map.clip[1].y - boxheight;
		}
		if (num3 > Map.clip[1].z - 0.5f)
		{
			num3 = Map.clip[1].z - 0.5f;
		}
		return new Vector3(num, num2, num3);
	}

	private void UpdatePhysics()
	{
		if (trControll == null || freefly || pl.team == 2)
		{
			return;
		}
		float num = 0f;
		float num2 = 1f;
		if (velocity.y > 0f)
		{
			num = 0.8f * velocity.y;
			velocity.y -= 0.3f;
		}
		Vector3 pos = Pos;
		Vector3 pos2 = new Vector3(pos.x, pos.y + num - num2, pos.z);
		if (VUtil.isValidBBox(pos2, radius, boxheight))
		{
			Pos = pos2;
			inAir = true;
			inAirFrame++;
			return;
		}
		if (VUtil.isHeadContact())
		{
			velocity.y = 0f;
		}
		if (VUtil.isGroundContact())
		{
			if (inAir)
			{
				VWIK.AddOffset(Vector3.down * 0.004f, Time.time, 0.5f);
			}
			velocity.y = 0f;
			inAir = false;
			inAirFrame = 0;
		}
		Pos.y = (int)Pos.y;
		if (Map.GetBlock(Pos) != 0 && VUtil.isValidBBox(Pos + Vector3.up, radius, boxheight))
		{
			Pos.y += 1f;
		}
	}

	private void UpdateStuck()
	{
		if (VUtil.isValidBBox(Pos, radius, boxheight))
		{
			inStuck = false;
		}
		else
		{
			inStuck = true;
		}
	}

	private void UpdateCursor()
	{
		if (DemoRec.isDemo() || pl.currweapon == null || GUIInv.GetWeaponInfo(pl.currweapon.weaponname).slot != 3)
		{
			return;
		}
		if (Builder.goCursor == null)
		{
			Builder.BuildCursor(false, 40);
		}
		RaycastHit hit;
		if (!Raycast(trCamera.transform.position, trCamera.transform.forward, out hit, 10f))
		{
			HideCursor();
			return;
		}
		if (hit.transform != Map.goMap.transform && hit.transform != Map.trMapPlatform)
		{
			HideCursor();
			return;
		}
		hit.point += hit.normal * 0.5f;
		float num = (int)hit.point.x;
		float num2 = (int)hit.point.y;
		float num3 = (int)hit.point.z;
		float num4 = (int)trControll.position.x - (int)hit.point.x;
		float num5 = (int)trControll.position.y - (int)hit.point.y;
		float num6 = (int)trControll.position.z - (int)hit.point.z;
		if ((num4 >= 0f && num4 > 3f) || (num4 < 0f && num4 < -4f) || (num5 >= 0f && num5 > 2f) || (num5 < 0f && num5 < -5f) || (num6 >= 0f && num6 > 3f) || (num6 < 0f && num6 < -4f))
		{
			HideCursor();
			return;
		}
		if (!VUtil.isValidBlockPlace(trControll.transform.position, 0.45f, boxheight, new Vector3(num + 0.5f, num2 + 0.5f, num3 + 0.5f)))
		{
			HideCursor();
			return;
		}
		if (Builder.goCursor != null)
		{
			Builder.goCursor.transform.position = new Vector3(num, num2, num3);
		}
		blockCursor = new Vector3(num, num2, num3);
	}

	public static void HideCursor()
	{
		if (Builder.goCursor != null)
		{
			Builder.goCursor.transform.position = Vector3.zero;
		}
		blockCursor = nullvec;
	}

	private void OnGUI()
	{
		if (trControll == null)
		{
			return;
		}
		if (inZoom)
		{
			float num = VWIK.vLagRot.y * 10f * GUIM.YRES(32f);
			float num2 = VWIK.vBobPos.x * 250f * GUIM.YRES(32f);
			float num3 = VWIK.vBobPos.y * 200f * GUIM.YRES(32f);
			float num4 = 0f;
			if (iattacktime > GetClientTime())
			{
				num4 = (float)(iattacktime - GetClientTime()) / 1000f;
			}
			float num5 = (num + num2) * 0.25f;
			float num6 = num3 * 0.25f;
			float num7 = GUIM.YRES(48f * scope_scale);
			float num8 = (float)Screen.height / 32f * 2f;
			float num9 = num4 * GUIM.YRES(64f);
			if (scope_type == 1 || scope_type == 2)
			{
				num8 = (float)Screen.height / 32f * 4f;
				float x = num + num2 + (float)(Screen.width - Screen.height) / 2f + num8 - num9;
				float y = num3 + num8 - num9;
				float num10 = (float)Screen.height - num8 * 2f + num9 * 2f;
				float height = num10;
				rZoom.Set(x, y, num10, height);
				num8 = ((scope_type != 1) ? (num8 + GUIM.YRES(248f)) : (num8 - GUIM.YRES(64f)));
				x = num + num2 + (float)(Screen.width - Screen.height) / 2f + num8;
				y = num3 + num8;
				num10 = (float)Screen.height - num8 * 2f;
				height = num10;
				rDot.Set(x, y, num10, height);
				GUI.DrawTexture(rZoom, tZoom);
				GUI.DrawTexture(rDot, tDot);
				int num11 = 2;
				rCorner.Set(-num11, -num11, rZoom.x + (float)(num11 * 2), Screen.height + num11 * 2);
				GUI.DrawTexture(rCorner, tBlack);
				rCorner.Set(rZoom.x + rZoom.width - (float)num11, -num11, (float)Screen.width - (rZoom.x + rZoom.width) + (float)(num11 * 2), Screen.height + num11 * 2);
				GUI.DrawTexture(rCorner, tBlack);
				rCorner.Set(-num11, -num11, Screen.width + num11 * 2, rZoom.y + (float)(num11 * 2));
				GUI.DrawTexture(rCorner, tBlack);
				rCorner.Set(-num11, rZoom.y + rZoom.height - (float)num11, Screen.width + num11 * 2, (float)Screen.height - (rZoom.y + rZoom.height) + (float)(num11 * 2));
				GUI.DrawTexture(rCorner, tBlack);
			}
			else
			{
				rZoom.Set((float)(Screen.width - Screen.height) / 2f + num + num2 - num8 - num9, num3 - num8 - num9, (float)Screen.height + (num8 + num9) * 2f, (float)Screen.height + (num8 + num9) * 2f);
				rDot.Set(((float)Screen.width - num7) / 2f + num5, ((float)Screen.height - num7) / 2f + num6, num7, num7);
				GUI.DrawTexture(rDot, tDot);
				if (tGlass != null)
				{
					cglass.a = (rx + 360f) / 1000f;
					if (cglass.a > 0.2f)
					{
						cglass.a = 0.2f;
					}
					if (cglass.a < 0.02f)
					{
						cglass.a = 0.02f;
					}
					GUI.color = cglass;
					GUI.DrawTexture(rZoom, tGlass);
					GUI.color = Color.white;
				}
				GUI.DrawTexture(rZoom, tZoom);
			}
		}
		if (overtime > 0f)
		{
			Rect position = new Rect((float)Screen.width / 2f - GUIM.YRES(40f), GUIM.YRES(390f), GUIM.YRES(80f), GUIM.YRES(4f));
			Rect position2 = new Rect(position.x, position.y, GUIM.YRES(80f) * overtime, position.height);
			GUI.color = new Color(1f, 1f, 1f, overtime);
			GUI.DrawTexture(position, TEX.tBlack);
			if (overtime > 0.7f)
			{
				GUI.DrawTexture(position2, TEX.tRed);
			}
			else if (overtime > 0.4f)
			{
				GUI.DrawTexture(position2, TEX.tYellow);
			}
			else
			{
				GUI.DrawTexture(position2, TEX.tWhite);
			}
			GUI.color = Color.white;
		}
	}

	private void LerpMove()
	{
		if (!(trControll == null) && !lockMove)
		{
			float num = fnextFrame - Time.realtimeSinceStartup;
			if (num < 0f)
			{
				num = 0f;
			}
			if (num > cl_fps)
			{
				num = cl_fps;
			}
			float num2 = (cl_fps - num) / cl_fps;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			intp = num2;
			currPos = Vector3.Lerp(prevPos, Pos, num2);
			trControll.transform.position = currPos;
			pl.currPos = currPos;
			pl.currPos.y += PLH.pyofs;
		}
	}

	private void LerpCamera()
	{
		if (currCamheight == camheight || trCamera == null || lockLook)
		{
			return;
		}
		float num = 15f * Time.deltaTime;
		if (currCamheight < camheight)
		{
			currCamheight += num;
			if (currCamheight > camheight)
			{
				currCamheight = camheight;
			}
		}
		else
		{
			currCamheight -= num;
			if (currCamheight < camheight)
			{
				currCamheight = camheight;
			}
		}
		trCamera.localPosition = new Vector3(0f, currCamheight, 0f);
	}

	private void BobCamera()
	{
		if (!(trControll == null) && !lockMove)
		{
			float time = Time.time;
			float num = 10f;
			float num2 = 0.05f;
			float num3 = vel.magnitude * speed * 5000f;
			float x = Mathf.Sin(time * num) * num3 * num2 / 100f * Time.deltaTime;
			float num4 = Mathf.Sin(2f * time * num) * num3 * num2 / 400f * Time.deltaTime;
			trCamera.localPosition = new Vector3(x, trCamera.localPosition.y + num4, 0f);
		}
	}

	private void KillCamera()
	{
		if (trControll == null)
		{
			return;
		}
		if ((pl.team == 2 || pl.bstate == 5) && Input.GetMouseButtonUp(0))
		{
			Client.cs.send_spectator();
		}
		if (specid >= 0)
		{
			if (PLH.player[specid] == null)
			{
				specid = -1;
			}
			else if (PLH.player[specid].go == null)
			{
				specid = -1;
			}
			else if (specmode == 0 || PLH.player[specid].bstate == 5)
			{
				Vector3 vector = PLH.player[specid].tr.position + Vector3.up * 0.5f - PLH.player[specid].tr.forward * 0.5f;
				Vector3 vector2 = vector + PLH.player[specid].tr.right * 1f - PLH.player[specid].tr.forward * 2.5f;
				RaycastHit hitInfo;
				if (Physics.Linecast(vector, vector2, out hitInfo))
				{
					vector2 = hitInfo.point;
				}
				trCamera.parent = PLH.player[specid].tr;
				trCamera.position = vector2;
				trCamera.localEulerAngles = Vector3.zero;
			}
			else
			{
				trCamera.parent = PLH.player[specid].tr;
				trCamera.localPosition = cam_offset;
				trCamera.localEulerAngles = new Vector3(PLH.player[specid].currRot[0], 0f, 0f);
			}
		}
		else if (pl.bstate == 5 && !(pl.goBody == null) && !(pl.go == null))
		{
			Vector3 position = pl.goBody.transform.position + Vector3.up * 2f + pl.go.transform.forward * -7f;
			trCamera.position = position;
			trCamera.localEulerAngles = Vector3.zero;
		}
	}

	private void ChangeLayer(PlayerData p, int val)
	{
		p.go.layer = val;
		p.goHead.layer = val;
		p.goBody.layer = val;
		p.goBackPack.layer = val;
		p.goLArm[0].layer = val;
		p.goLArm[1].layer = val;
		p.goRArm[0].layer = val;
		p.goRArm[1].layer = val;
		p.goLLeg[0].layer = val;
		p.goLLeg[1].layer = val;
		p.goLLeg[2].layer = val;
		p.goRLeg[0].layer = val;
		p.goRLeg[1].layer = val;
		p.goRLeg[2].layer = val;
	}

	public static void GenerateBloodSplat()
	{
		if (!(pgoBloodSplat[0] != null))
		{
			float num = 1f;
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 1, 1);
			BlockBuilder.SetPivot(new Vector3(num / 2f, num / 2f, 0f));
			BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, num, num, 0f, 0f, 0f, 1f, 1f);
			Mesh sharedMesh = MeshBuilder.ToMesh();
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "bloodsplat_" + i;
				gameObject.transform.position = new Vector3(-2000f, -2000f, -2000f);
				MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				meshRenderer.material = new Material(ContentLoader_.LoadMaterial("unlit_transparent"));
				meshRenderer.material.SetTexture("_MainTex", ContentLoader_.LoadTexture("blood_splat" + i) as Texture2D);
				meshRenderer.material.SetFloat("_Glossiness", 0f);
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				meshRenderer.receiveShadows = false;
				meshFilter.sharedMesh = sharedMesh;
				pgoBloodSplat[i] = gameObject;
			}
			for (int j = 0; j < 4; j++)
			{
				pgoBloodSplatGreen[j] = UnityEngine.Object.Instantiate(pgoBloodSplat[j]);
				pgoBloodSplatGreen[j].name = "bloodsplatgreen_" + j;
				pgoBloodSplatGreen[j].transform.position = new Vector3(-2000f, -2000f, -2000f);
				MeshRenderer component = pgoBloodSplatGreen[j].GetComponent<MeshRenderer>();
				Texture2D value = Resources.Load("blood_splatgreen" + j) as Texture2D;
				component.material.SetTexture("_MainTex", value);
			}
			for (int k = 0; k < 2; k++)
			{
				pgoEnergySplat[k] = UnityEngine.Object.Instantiate(pgoBloodSplat[k]);
				pgoEnergySplat[k].name = "energysplat_" + k;
				pgoEnergySplat[k].transform.position = new Vector3(-2000f, -2010f, -2000f);
				MeshRenderer component2 = pgoEnergySplat[k].GetComponent<MeshRenderer>();
				Texture2D value2 = Resources.Load("Textures/energy_splat" + k) as Texture2D;
				component2.material.SetTexture("_MainTex", value2);
			}
		}
	}

	public static void GenerateShell()
	{
		if (!(pgoShell != null))
		{
			GameObject obj = new GameObject();
			obj.name = "shell";
			obj.transform.localPosition = new Vector3(-2000f, -2000f, -2000f);
			obj.transform.localEulerAngles = Vector3.zero;
			obj.transform.localScale = new Vector3(0.009f, 0.009f, 0.009f);
			MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
			meshRenderer.material = new Material(ContentLoader_.LoadMaterial("vertex_color"));
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.material.SetTexture("_MainTex", null);
			MeshBuilder.Create();
			BlockBuilder.SetPivot(new Vector3(1.5f, 0f, 0f));
			BlockBuilder.BuildBox(new Vector3(0f, 0f, 0f), 1f, 3f, 1f, Color.yellow);
			meshFilter.sharedMesh = MeshBuilder.ToMesh();
			pgoShell = obj;
			if (GUIOptions.motion > 0)
			{
				pgoShell.AddComponent<AmplifyMotionObject>();
			}
		}
	}

	public static void GenerateGrenade()
	{
		if (!(pgoGrenade != null))
		{
			Texture2D value = Resources.Load("Textures/grenade_tex") as Texture2D;
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 8, 8);
			BlockBuilder.SetPivot(new Vector3(2f, 3f, 2f));
			BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 4f, 6f, 4f, 0f, 0f, 4f, 6f);
			BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 4f, 6f, 4f, 0f, 0f, 4f, 6f);
			BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 4f, 6f, 4f, 0f, 0f, 4f, 6f);
			BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 4f, 6f, 4f, 0f, 0f, 4f, 6f);
			BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 4f, 6f, 4f, 4f, 0f, 4f, 4f);
			BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 4f, 6f, 4f, 4f, 0f, 4f, 4f);
			BlockBuilder.BuildFaceBlockTex(1, new Vector3(1f, 6f, 1f), Color.white, 2f, 1f, 2f, 0f, 6f, 2f, 2f);
			BlockBuilder.BuildFaceBlockTex(0, new Vector3(1f, 6f, 1f), Color.white, 2f, 1f, 2f, 0f, 6f, 2f, 2f);
			BlockBuilder.BuildFaceBlockTex(2, new Vector3(1f, 6f, 1f), Color.white, 2f, 1f, 2f, 0f, 6f, 2f, 2f);
			BlockBuilder.BuildFaceBlockTex(3, new Vector3(1f, 6f, 1f), Color.white, 2f, 1f, 2f, 0f, 6f, 2f, 2f);
			BlockBuilder.BuildFaceBlockTex(4, new Vector3(1f, 6f, 1f), Color.white, 2f, 1f, 2f, 0f, 6f, 2f, 2f);
			BlockBuilder.BuildFaceBlockTex(5, new Vector3(1f, 6f, 1f), Color.white, 2f, 1f, 2f, 0f, 6f, 2f, 2f);
			Mesh sharedMesh = MeshBuilder.ToMesh();
			MeshFilter meshFilter = null;
			GameObject gameObject = new GameObject();
			gameObject.name = "grenade";
			gameObject.transform.localPosition = new Vector3(-2000f, -2000f, -2000f);
			GameObject obj = new GameObject();
			obj.name = "obj";
			obj.transform.parent = gameObject.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			meshFilter = obj.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
			meshRenderer.material = new Material(ContentLoader_.LoadMaterial("vertex_color"));
			meshRenderer.material.SetTexture("_MainTex", value);
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshFilter.sharedMesh = sharedMesh;
			pgoGrenade = gameObject;
		}
	}

	public static void GenerateMedkit()
	{
		if (!(pgoMedkit != null))
		{
			Texture2D value = Resources.Load("Textures/skin_box_medkit") as Texture2D;
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 64, 64);
			BlockBuilder.SetPivot(new Vector3(16f, 8f, 8f));
			BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 32f, 16f, 16f, 0f, 16f, 32f, 16f);
			BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 32f, 16f, 16f, 0f, 16f, 32f, 16f);
			BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 32f, 16f, 16f, 32f, 16f, 16f, 16f);
			BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 32f, 16f, 16f, 32f, 16f, 16f, 16f);
			BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 32f, 16f, 16f, 0f, 0f, 32f, 16f);
			BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 32f, 16f, 16f, 0f, 32f, 32f, 16f);
			Mesh sharedMesh = MeshBuilder.ToMesh();
			MeshFilter meshFilter = null;
			GameObject gameObject = new GameObject();
			gameObject.name = "medkit";
			gameObject.transform.localPosition = new Vector3(-2000f, -2000f, -2000f);
			GameObject obj = new GameObject();
			obj.name = "obj";
			obj.transform.parent = gameObject.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			meshFilter = obj.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
			meshRenderer.material = new Material(ContentLoader_.LoadMaterial("vertex_color"));
			meshRenderer.material.SetTexture("_MainTex", value);
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshFilter.sharedMesh = sharedMesh;
			pgoMedkit = gameObject;
		}
	}

	public static void GenerateFreezeBlock()
	{
		if (!(pgoFreezeBlock[0] != null))
		{
			Texture2D value = Resources.Load("Textures/freezebox_01") as Texture2D;
			GameObject gameObject = new GameObject();
			gameObject.name = "freezeblock_01";
			gameObject.transform.localPosition = new Vector3(-2000f, -2000f, -2000f);
			gameObject.transform.localScale = new Vector3(0.09f, 0.09f, 0.09f);
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 64, 64);
			BlockBuilder.SetPivot(new Vector3(8f, 0f, 8f));
			BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 16f, 32f, 16f, 0f, 16f, 16f, 32f);
			BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 16f, 32f, 16f, 32f, 16f, 16f, 32f);
			BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 16f, 32f, 16f, 16f, 16f, 16f, 32f);
			BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 16f, 32f, 16f, 48f, 16f, 16f, 32f);
			BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 16f, 32f, 16f, 0f, 0f, 16f, 16f);
			BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 16f, 32f, 16f, 16f, 64f, -16f, -16f);
			Mesh sharedMesh = MeshBuilder.ToMesh();
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.material = new Material(MAT.Get("unlit_alpha_co"));
			meshRenderer.material.SetTexture("_MainTex", value);
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshFilter.sharedMesh = sharedMesh;
			pgoFreezeBlock[0] = gameObject;
			for (int i = 1; i < 5; i++)
			{
				pgoFreezeBlock[i] = UnityEngine.Object.Instantiate(pgoFreezeBlock[0]);
				value = Resources.Load("Textures/freezebox_0" + (i + 1)) as Texture2D;
				pgoFreezeBlock[i].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", value);
				pgoFreezeBlock[i].name = "freezeblock_0" + (i + 1);
			}
		}
	}

	public static void GenerateAmmo()
	{
		Texture2D value = Resources.Load("Textures/skin_box_ammo") as Texture2D;
		pgoAmmokit = UnityEngine.Object.Instantiate(pgoMedkit);
		pgoAmmokit.name = "ammo_" + pgoAmmokit.GetInstanceID();
		pgoAmmokit.transform.localPosition = new Vector3(-2000f, -2000f, -2000f);
		GameObject.Find(pgoAmmokit.name + "/obj").GetComponent<MeshRenderer>().material.SetTexture("_MainTex", value);
	}

	public static void GenerateRocketkit()
	{
		Texture2D value = Resources.Load("Textures/skin_box_rocket") as Texture2D;
		pgoRocketkit = UnityEngine.Object.Instantiate(pgoMedkit);
		pgoRocketkit.name = "rocket_" + pgoRocketkit.GetInstanceID();
		pgoRocketkit.transform.localPosition = new Vector3(-2000f, -2000f, -2000f);
		GameObject.Find(pgoRocketkit.name + "/obj").GetComponent<MeshRenderer>().material.SetTexture("_MainTex", value);
		GameObject obj = UnityEngine.Object.Instantiate(pgoArrow[0], pgoRocketkit.transform);
		obj.transform.localPosition = Vector3.zero;
		MeshRenderer component = obj.GetComponent<MeshRenderer>();
		component.material.SetColor("_Color", Color.white);
		Texture2D value2 = Resources.Load("rocket_radar") as Texture2D;
		component.material.SetTexture("_MainTex", value2);
	}

	public static void GenerateTracer()
	{
		if (!(pgoTracer != null))
		{
			Texture2D value = ContentLoader_.LoadTexture("tracer") as Texture2D;
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 16, 16);
			BlockBuilder.SetPivot(new Vector3(0.5f, 0.5f, 0f));
			BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 1f, 1f, 16f, 1f, 5f, 1f, 1f);
			BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 1f, 1f, 16f, 13f, 5f, 1f, 1f);
			BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 1f, 1f, 16f, 1f, 1f, 14f, 1f);
			BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 1f, 1f, 16f, 15f, 1f, -14f, 1f);
			BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 1f, 1f, 16f, 15f, 1f, -14f, 1f, true);
			BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 1f, 1f, 16f, 15f, 1f, -14f, 1f, true);
			Mesh sharedMesh = MeshBuilder.ToMesh();
			MeshFilter meshFilter = null;
			GameObject obj = new GameObject();
			obj.name = "tracer";
			obj.transform.position = new Vector3(-2000f, -2000f, -2000f);
			meshFilter = obj.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
			meshRenderer.material = new Material(ContentLoader_.LoadMaterial("unlit_transparent"));
			meshRenderer.material.SetTexture("_MainTex", value);
			meshRenderer.material.SetFloat("_Glossiness", 0f);
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshFilter.sharedMesh = sharedMesh;
			pgoTracer = obj;
		}
	}

	public static void GenerateFBlock()
	{
		if (!(pgoFBlock != null))
		{
			Texture2D value = Resources.Load("Textures/fblock_tex") as Texture2D;
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 8, 8);
			BlockBuilder.SetPivot(new Vector3(2f, 3f, 2f));
			BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 4f, 4f, 4f, 0f, 0f, 4f, 4f);
			BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 4f, 4f, 4f, 0f, 0f, 4f, 4f);
			BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 4f, 4f, 4f, 0f, 0f, 4f, 4f);
			BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 4f, 4f, 4f, 0f, 0f, 4f, 4f);
			BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 4f, 4f, 4f, 4f, 0f, 4f, 4f);
			BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 4f, 4f, 4f, 4f, 0f, 4f, 4f);
			Mesh sharedMesh = MeshBuilder.ToMesh();
			MeshFilter meshFilter = null;
			GameObject gameObject = new GameObject();
			gameObject.name = "fblock";
			gameObject.transform.localPosition = new Vector3(-2000f, -2000f, -2000f);
			GameObject obj = new GameObject();
			obj.name = "obj";
			obj.transform.parent = gameObject.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			meshFilter = obj.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
			meshRenderer.material = new Material(ContentLoader_.LoadMaterial("vertex_color"));
			meshRenderer.material.SetTexture("_MainTex", value);
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshFilter.sharedMesh = sharedMesh;
			pgoFBlock = gameObject;
		}
	}

	public static void GenerateHole()
	{
		if (!(pgoHole != null))
		{
			Texture2D textureByName = TEX.GetTextureByName("white");
			float num = 0.2f;
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 1, 1);
			BlockBuilder.SetPivot(new Vector3(num / 2f, num / 2f, 0f));
			BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, num, num, 0f, 0f, 0f, 1f, 1f);
			Mesh sharedMesh = MeshBuilder.ToMesh();
			MeshFilter meshFilter = null;
			GameObject obj = new GameObject();
			obj.transform.position = new Vector3(-2000f, -2000f, -2000f);
			obj.name = "hole";
			meshFilter = obj.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
			meshRenderer.material = ContentLoader_.LoadMaterial("legacy_transparent");
			meshRenderer.material.color = new Color(0f, 0f, 0f, 0.5f);
			meshRenderer.material.SetTexture("_MainTex", textureByName);
			meshRenderer.material.SetFloat("_Glossiness", 0f);
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshFilter.sharedMesh = sharedMesh;
			pgoHole = obj;
			pgoHoleBlood = UnityEngine.Object.Instantiate(pgoHole);
			pgoHoleBlood.name = "hole_blood";
			pgoHoleBlood.GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 0f, 0.5f);
		}
	}

	public static void CheckHole(int x, int y, int z)
	{
		int num = holelist.Count;
		for (int i = 0; i < num; i++)
		{
			if (holelist[i].go == null)
			{
				holelist.RemoveAt(i);
				i--;
				num--;
			}
			else if (holelist[i].x == x && holelist[i].y == y && holelist[i].z == z)
			{
				UnityEngine.Object.Destroy(holelist[i].go);
				holelist.RemoveAt(i);
				i--;
				num--;
			}
		}
	}

	public static void CreateHole(RaycastHit hit, bool blood, Vector3 block)
	{
		if (blood)
		{
			return;
		}
		float num = 0.2f;
		float num2 = 5f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		if (!blood)
		{
			if (hit.normal.x != 0f)
			{
				num3 = hit.point.x;
				num4 = (float)(int)(hit.point.y / num) / num2;
				num5 = (float)(int)(hit.point.z / num) / num2;
				if ((int)(hit.point.z - num) != (int)hit.point.z)
				{
					num5 += num;
				}
				if ((int)(hit.point.y - num) != (int)hit.point.y)
				{
					num4 += num;
				}
			}
			else if (hit.normal.y != 0f)
			{
				num3 = (float)(int)(hit.point.x / num) / num2;
				num4 = hit.point.y;
				num5 = (float)(int)(hit.point.z / num) / num2;
				if ((int)(hit.point.x - num) != (int)hit.point.x)
				{
					num3 += num;
				}
				if ((int)(hit.point.z - num) != (int)hit.point.z)
				{
					num5 += num;
				}
			}
			else if (hit.normal.z != 0f)
			{
				num3 = (float)(int)(hit.point.x / num) / num2;
				num4 = (float)(int)(hit.point.y / num) / num2;
				num5 = hit.point.z;
				if ((int)(hit.point.x - num) != (int)hit.point.x)
				{
					num3 += num;
				}
				if ((int)(hit.point.y - num) != (int)hit.point.y)
				{
					num4 += num;
				}
			}
		}
		else
		{
			num3 = hit.point.x;
			num4 = hit.point.y;
			num5 = hit.point.z;
		}
		Vector3 pos = new Vector3(num3, num4, num5) + hit.normal * 0.01f;
		Vector3 eulerAngles = (Quaternion.LookRotation(hit.normal) * Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360))).eulerAngles;
		if (blood)
		{
			GOP.Create(20, pos, eulerAngles, Vector3.zero, Color.white);
		}
		else
		{
			GOP.Create(21, pos, eulerAngles, block, Color.white);
		}
	}

	private static void GenerateDamageBlock()
	{
		if (!(pgoDamageBlock != null))
		{
			float num = 0.01f;
			GameObject obj = new GameObject
			{
				name = "damageblock"
			};
			MeshBuilder.Create();
			BlockBuilder.SetTexCoord(0, 1, 1);
			BlockBuilder.SetPivot(new Vector3(num, num, num));
			BlockBuilder.BuildBox(Vector3.zero, 1f + num * 2f, 1f + num * 2f, 1f + num * 2f, Color.white);
			Mesh sharedMesh = MeshBuilder.ToMesh();
			MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
			meshRenderer.material = ContentLoader_.LoadMaterial("legacy_transparent");
			meshRenderer.material.SetTexture("_MainTex", TEX.GetTextureByName("white"));
			meshRenderer.material.SetFloat("_Glossiness", 0f);
			meshRenderer.material.color = new Color(0f, 0f, 0f, 0f);
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshFilter.sharedMesh = sharedMesh;
			pgoDamageBlock = obj;
		}
	}

	public static void GenerateArrow()
	{
		if (!(pgoArrow[0] != null))
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "Arrow";
			gameObject.transform.localPosition = new Vector3(-2000f, -2000f, -2000f);
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.layer = 8;
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.material = new Material(ContentLoader_.LoadMaterial("legacy_transparent"));
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			Texture2D value = ContentLoader_.LoadTexture("arrow_radar") as Texture2D;
			meshRenderer.material.SetTexture("_MainTex", value);
			meshRenderer.material.SetColor("_Color", Color.yellow);
			MeshBuilder.Create();
			BlockBuilder.SetPivot(new Vector3(5f, 0f, 5f));
			BlockBuilder.SetTexCoord(0, 1, 1);
			BlockBuilder.BuildFaceBlock(1, 4, Vector3.zero, Color.white, 10f, 0f, 10f);
			meshFilter.mesh = MeshBuilder.ToMesh();
			pgoArrow[0] = gameObject;
			pgoArrow[1] = UnityEngine.Object.Instantiate(pgoArrow[0]);
			pgoArrow[1].transform.localPosition = new Vector3(-2000f, -2000f, -2000f);
			MeshRenderer component = pgoArrow[1].GetComponent<MeshRenderer>();
			component.material.SetColor("_Color", Color.white);
			value = Resources.Load("arrow_radar2") as Texture2D;
			component.material.SetTexture("_MainTex", value);
		}
	}

	private static bool CheckPlayerDamage(RaycastHit hit, WeaponInfo wi)
	{
		HitData component = hit.transform.GetComponent<HitData>();
		if (component == null)
		{
			return false;
		}
		int idx = component.idx;
		int box = component.box;
		if (gamemode == 6 && PLH.player[idx].goAttach != null)
		{
			attacklist.Add(new AttackData(idx, 0, hit.point));
			if (sIceHit == null)
			{
				sIceHit = Resources.Load("sounds/icehit") as AudioClip;
			}
			if (Time.time > lasthitmarksound)
			{
				pl.asW.PlayOneShot(sIceHit);
			}
			lasthitmarksound = Time.time + 0.05f;
			return true;
		}
		if (PLH.player[idx].bstate == 5)
		{
			return false;
		}
		if (gamemode != 2 && gamemode != 5 && gamemode != 8 && pl.team == PLH.player[idx].team)
		{
			return false;
		}
		attacklist.Add(new AttackData(idx, box, hit.point));
		Crosshair.SetMark(Color.red);
		if (sHitmark == null)
		{
			sHitmark = ContentLoader_.LoadAudio("hitmark");
		}
		if (Time.time > lasthitmarksound)
		{
			pl.asW.PlayOneShot(sHitmark);
		}
		lasthitmarksound = Time.time + 0.05f;
		if (wi.id == 170)
		{
			CreateBlood(hit.point, box, 2);
		}
		else
		{
			CreateBlood(hit.point, box, (PLH.player[idx].skinstate == 1) ? 1 : 0);
		}
		if (PLH.player[idx].skinstate == 1)
		{
			if (PLH.sZombieHit == null)
			{
				PLH.sZombieHit = Resources.Load("sounds/zombiehit") as AudioClip;
			}
			if (Time.time > lasthitmarksound)
			{
				pl.asW.PlayOneShot(PLH.sZombieHit);
			}
		}
		return true;
	}

	public static void CreateBlood(Vector3 pos, int vb, int bloodtype = 0)
	{
		switch (bloodtype)
		{
		case 1:
			GOP.Create(16 + UnityEngine.Random.Range(0, 4), pos, Vector3.zero, Vector3.zero, Color.white, vb);
			GOP.Create(3, pos, Vector3.zero, Vector3.zero, Color.green);
			GOP.Create(0, pos, Vector3.zero, Vector3.zero, Color.green);
			break;
		case 2:
			GOP.Create(24 + UnityEngine.Random.Range(0, 2), pos, Vector3.zero, Vector3.zero, Color.white, vb);
			GOP.Create(3, pos, Vector3.zero, Vector3.zero, Color.cyan);
			GOP.Create(0, pos, Vector3.zero, Vector3.zero, Color.cyan);
			break;
		default:
			GOP.Create(4 + UnityEngine.Random.Range(0, 4), pos, Vector3.zero, Vector3.zero, Color.white, vb);
			GOP.Create(3, pos, Vector3.zero, Vector3.zero, Color.red);
			GOP.Create(0, pos, Vector3.zero, Vector3.zero, Color.red);
			break;
		}
	}

	public static bool Raycast(Vector3 pos, Vector3 dir, out RaycastHit hit, float dist = 1024f)
	{
		return Physics.Raycast(pos, dir, out hit, dist, layerMask);
	}

	public static bool Bulletcast(Vector3 pos, Vector3 dir, out RaycastHit hit, float dist = 128f)
	{
		Vector3 vector = pos + dir * dist + Quaternion.AngleAxis(1f, trControll.right) * dir * (256f - dist);
		if (Physics.Raycast(pos, dir, out hit, dist, layerMask))
		{
			return true;
		}
		return Physics.Raycast(pos + dir * dist, Quaternion.AngleAxis(1f, trControll.right) * dir, out hit, 256f - dist, layerMask);
	}

	private void SetRecoil(int recoiltype, float force)
	{
		float num = 1f;
		if (inZoom)
		{
			num = 0.5f;
		}
		switch (recoiltype)
		{
		case 1:
			VWIK.CameraAddAngle(new Vector3((0f - force) * num, force * 0.5f * num, 0f), Time.time, 0.1f, true);
			VWIK.CameraAddAngle(new Vector3((0f - force) * num, force * 0.5f * num, 0f), Time.time + 0.1f, 0.2f);
			break;
		case 2:
		{
			if (Time.time > recoilend)
			{
				SetRecoil(1, force);
				return;
			}
			float num3 = UnityEngine.Random.Range(0, 2);
			if (num3 == 0f)
			{
				num3 = -1f;
			}
			VWIK.CameraAddAngle(new Vector3((0f - force) * num, force * 0.5f * num * num3, 0f), Time.time, 0.1f, true);
			VWIK.CameraAddAngle(new Vector3((0f - force) * num, force * 0.5f * num * num3, 0f), Time.time + 0.1f, 0.2f);
			break;
		}
		case 3:
			VWIK.CameraAddAngle(new Vector3((0f - force) * num, force * 0.1f * num, 0f), Time.time, 0.3f);
			break;
		case 4:
			VWIK.CameraAddAngle(new Vector3((0f - force) * num, 0f, 0f), Time.time, 0.2f, true);
			VWIK.CameraAddAngle(new Vector3((0f - force) * num, 0f, 0f), Time.time + 0.2f, 0.3f);
			break;
		case 5:
		{
			float num2 = UnityEngine.Random.Range(0, 2);
			if (num2 == 0f)
			{
				num2 = -1f;
			}
			VWIK.CameraAddAngle(new Vector3(0f, force * num * num2, 0f), Time.time, 0.01f, true);
			VWIK.CameraAddAngle(new Vector3(0f, force * num * num2, 0f), Time.time + 0.01f, 0.2f);
			break;
		}
		}
		recoilend = Time.time + 0.3f;
	}

	public void CheckMove()
	{
	}

	public static void SetFreeze(float sec)
	{
		inFreeze = true;
		tFreeze = Time.time + sec;
		iFreeze = (int)sec;
		if (ccc == null && goCamera != null)
		{
			ccc = goCamera.AddComponent<ColorCorrectionCurves>();
			ccc.colorCorrectionCurvesShader = Shader.Find("Hidden/ColorCorrectionCurves");
			ccc.simpleColorCorrectionCurvesShader = Shader.Find("Hidden/ColorCorrectionCurvesSimple");
			ccc.colorCorrectionSelectiveShader = Shader.Find("Hidden/ColorCorrectionSelective");
			ccc.redChannel = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			ccc.greenChannel = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			ccc.blueChannel = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		}
		if (ccc != null)
		{
			ccc.saturation = 0f;
		}
	}

	private void CreateTracer(Vector3 spread)
	{
		if (pl.currweapon != null && pl.currweapon.goBoneMuzzle != null)
		{
			Vector3 pos = pl.currweapon.goBoneMuzzle.transform.position;
			if (inZoom)
			{
				pos = trCamera.transform.position - Vector3.up * 0.25f;
			}
			Vector3 rot = trCamera.transform.eulerAngles + spread;
			GOP.Create(8, pos, rot, Vector3.zero, Color.black);
		}
	}

	public static uint GetServerTime()
	{
		return sv_time + (uint)((Time.realtimeSinceStartup - fcl_time) * 1000f);
	}

	public static uint GetClientTime()
	{
		return (uint)Environment.TickCount & 0x7FFFFFFFu;
	}

	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		vector.x = (double.IsNaN(vector.x) ? prevVector.x : vector.x);
		vector.y = (double.IsNaN(vector.y) ? prevVector.y : vector.y);
		return vector;
	}
}
