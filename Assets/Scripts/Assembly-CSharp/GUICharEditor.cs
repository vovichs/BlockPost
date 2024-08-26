using System.Collections.Generic;
using System.Globalization;
using GameClass;
using Player;
using RuntimeGizmos;
using UnityEngine;

public class GUICharEditor : MonoBehaviour
{
	public static GUICharEditor cs = null;

	private bool show = true;

	private TransformGizmo csrg;

	private Spectator spec;

	private MouseLook mlook;

	private AudioListener csal;

	private FXAA csfxaa;

	private AmplifyOcclusionEffect csaoe;

	private Texture2D[] tArrow = new Texture2D[2];

	public List<SceneObject> objlist = new List<SceneObject>();

	public static List<string> objfilelist = new List<string>();

	public static List<string> pdatafilelist = new List<string>();

	private string[] team = new string[3] { "RED", "BLUE", "GREEN" };

	private string[] skinstate = new string[3] { "NORMAL", "ZOMBIE", "DAMAGED" };

	private string[] skinlist = new string[27]
	{
		"Default", "Paratrooper", "Police", "Rebel", "Professional", "Jacket", "Skullcap", "Smoking", "Sniper", "Mummy",
		"Pumpkin", "Forester", "Winter", "Clown", "Tankman", "Specops", "Punk", "Dracula", "Pirate", "Drifter",
		"Elf", "Biker", "Seaman", "Cyborg", "Nutcracker", "Hazmat", "Guerilla"
	};

	private string[] colorskinlist = new string[3] { "COLOR_SKIN0", "COLOR_SKIN1", "COLOR_SKIN2" };

	private string[] coloreyelist = new string[3] { "COLOR_EYE0", "COLORS_EYE1", "COLOR_EYE2" };

	private string[] colorhairlist = new string[4] { "COLOR_HAIR0", "COLORS_HAIR1", "COLOR_HAIR2", "COLOR_HAIR3" };

	private string[] eyelist = new string[3] { "EYE0", "EYE1", "EYE2" };

	private string[] beardlist = new string[4] { "BEARD0", "BEARD1", "BEARD2", "BEARD3" };

	private string[] hairlist = new string[5] { "HAIR0", "HAIR1", "HAIR2", "HAIR3", "HAIR4" };

	private Rect rBack;

	private Rect rFov;

	private Rect rColor;

	private Rect rRotate;

	private Rect rSpectator;

	private Rect rHideGUI;

	public Rect rTop;

	private Rect rTopLine;

	private Rect rFile;

	private Rect rView;

	private Rect rObject;

	private Rect rFileMenu;

	private Rect rViewMenu;

	public Rect rObjectMenu;

	private Rect rHide;

	private Rect[] rFileSlot = new Rect[4];

	private Rect[] rViewSlot = new Rect[4];

	private Rect[] rObjectSlot = new Rect[4];

	private Rect rLeft;

	private Rect rRight;

	private Rect rItem;

	private Rect[] rSlot = new Rect[10];

	private Rect[] rSlotArrowLeft = new Rect[10];

	private Rect[] rSlotArrowRight = new Rect[10];

	private int toolmode;

	private Rect[] rTool = new Rect[3];

	private Rect[] rToolKey = new Rect[3];

	private bool res;

	public int menuid = -1;

	public int objectmenuid = -1;

	private int itemtype;

	private int curritem = 1;

	private int scopeitem = 1;

	private int currskinstate;

	private string[] itemname = new string[6] { "CHAR", "CASE", "WEAPON", "MAP", "OBJ", "FWEAPON" };

	private int currskinid;

	private int currcolorskin;

	private int currcoloreye;

	private int currcolorhair;

	private int curreye;

	private int currbeard;

	private int currhair;

	public static string[] varfweapon = new string[3] { "ak47", "m4a1", "zombiehands" };

	private Vector3 wpos = Vector3.zero;

	private Vector3 lpos = Vector3.zero;

	private Vector3 lrot = Vector3.zero;

	private WeaponData wd;

	private Rect[] rv;

	private Vector2 sv = Vector2.zero;

	private int currpdata = -1;

	private string filename = "newfile";

	public Rect rMenuSave;

	private void Awake()
	{
		cs = this;
		csrg = base.gameObject.AddComponent<TransformGizmo>();
		csrg.space = TransformSpace.Local;
		csrg.SetMoveType = KeyCode.Alpha1;
		csrg.SetRotateType = KeyCode.Alpha2;
		csrg.SetScaleType = KeyCode.Alpha3;
		csrg.SetSpaceToggle = KeyCode.None;
		csrg.type = TransformType.Rotate;
		spec = base.gameObject.AddComponent<Spectator>();
		mlook = base.gameObject.AddComponent<MouseLook>();
		mlook.frameCounter = 2f;
		csfxaa = base.gameObject.AddComponent<FXAA>();
		csaoe = base.gameObject.AddComponent<AmplifyOcclusionEffect>();
		tArrow[0] = ContentLoader_.LoadTexture("arrow_l") as Texture2D;
		tArrow[1] = ContentLoader_.LoadTexture("arrow_r") as Texture2D;
		csal = base.gameObject.AddComponent<AudioListener>();
		OnResize();
	}

	private void OnResize()
	{
		rBack = new Rect(GUIM.YRES(32f), (float)Screen.height - GUIM.YRES(64f), GUIM.YRES(120f), GUIM.YRES(32f));
		rColor = new Rect(GUIM.YRES(32f), GUIM.YRES(32f), GUIM.YRES(120f), GUIM.YRES(32f));
		rFov = rColor;
		rFov.y += GUIM.YRES(40f);
		rRotate = rColor;
		rRotate.y += GUIM.YRES(40f) * 2f;
		rSpectator = new Rect(0f, (float)Screen.height - GUIM.YRES(64f), Screen.width, GUIM.YRES(32f));
		rHideGUI = new Rect(0f, (float)Screen.height - GUIM.YRES(64f) + GUIM.YRES(32f), Screen.width, GUIM.YRES(26f));
		rTop = new Rect(0f, 0f, Screen.width, GUIM.YRES(40f));
		rTopLine = new Rect(rTop.x, rTop.y + rTop.height - GUIM.YRES(1f), rTop.width, GUIM.YRES(1f));
		rFile = new Rect(0f, 0f, GUIM.YRES(100f), GUIM.YRES(40f));
		rView = new Rect(GUIM.YRES(100f), 0f, GUIM.YRES(100f), GUIM.YRES(40f));
		rObject = new Rect(GUIM.YRES(200f), 0f, GUIM.YRES(100f), GUIM.YRES(40f));
		rHide = new Rect(0f, GUIM.YRES(40f), Screen.width, (float)Screen.height - GUIM.YRES(40f));
		rFileMenu = new Rect(0f, 0f, GUIM.YRES(100f), GUIM.YRES(160f));
		rViewMenu = new Rect(GUIM.YRES(100f), 0f, GUIM.YRES(100f), GUIM.YRES(200f));
		rObjectMenu = new Rect(GUIM.YRES(200f), 0f, GUIM.YRES(100f), GUIM.YRES(120f));
		rFileSlot[0] = new Rect(rFileMenu.x, rFileMenu.y + GUIM.YRES(40f) * 1f, rFileMenu.width, GUIM.YRES(40f));
		rFileSlot[1] = new Rect(rFileMenu.x, rFileMenu.y + GUIM.YRES(40f) * 2f, rFileMenu.width, GUIM.YRES(40f));
		rFileSlot[2] = new Rect(rFileMenu.x, rFileMenu.y + GUIM.YRES(40f) * 3f, rFileMenu.width, GUIM.YRES(40f));
		rFileSlot[3] = new Rect(rFileMenu.x, rFileMenu.y + GUIM.YRES(40f) * 4f, rFileMenu.width, GUIM.YRES(40f));
		rViewSlot[0] = new Rect(rViewMenu.x, rViewMenu.y + GUIM.YRES(40f) * 1f, rViewMenu.width, GUIM.YRES(40f));
		rViewSlot[1] = new Rect(rViewMenu.x, rViewMenu.y + GUIM.YRES(40f) * 2f, rViewMenu.width, GUIM.YRES(40f));
		rViewSlot[2] = new Rect(rViewMenu.x, rViewMenu.y + GUIM.YRES(40f) * 3f, rViewMenu.width, GUIM.YRES(40f));
		rViewSlot[3] = new Rect(rViewMenu.x, rViewMenu.y + GUIM.YRES(40f) * 4f, rViewMenu.width, GUIM.YRES(40f));
		rObjectSlot[0] = new Rect(rObjectMenu.x, rObjectMenu.y + GUIM.YRES(40f) * 1f, rObjectMenu.width, GUIM.YRES(40f));
		rObjectSlot[1] = new Rect(rObjectMenu.x, rObjectMenu.y + GUIM.YRES(40f) * 2f, rObjectMenu.width, GUIM.YRES(40f));
		rObjectSlot[2] = new Rect(rObjectMenu.x, rObjectMenu.y + GUIM.YRES(40f) * 3f, rObjectMenu.width, GUIM.YRES(40f));
		rObjectSlot[3] = new Rect(rObjectMenu.x, rObjectMenu.y + GUIM.YRES(40f) * 4f, rObjectMenu.width, GUIM.YRES(40f));
		rLeft = new Rect(GUIM.YRES(32f), GUIM.YRES(240f), GUIM.YRES(32f), GUIM.YRES(32f));
		rRight = new Rect(GUIM.YRES(192f), GUIM.YRES(240f), GUIM.YRES(32f), GUIM.YRES(32f));
		rItem = new Rect(GUIM.YRES(72f), GUIM.YRES(240f), GUIM.YRES(112f), GUIM.YRES(32f));
		for (int i = 0; i < rSlot.Length; i++)
		{
			rSlot[i] = new Rect(rItem.x, rItem.y + GUIM.YRES(40f) * (float)(i + 1), rItem.width, rItem.height);
			rSlotArrowLeft[i] = new Rect(rSlot[i].x - GUIM.YRES(40f), rSlot[i].y, GUIM.YRES(32f), GUIM.YRES(32f));
			rSlotArrowRight[i] = new Rect(rSlot[i].x + rSlot[i].width + GUIM.YRES(8f), rSlot[i].y, GUIM.YRES(32f), GUIM.YRES(32f));
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			DrawTools();
			DrawItem();
			DrawSystem();
			GUIM.DrawText(rSpectator, "PRESS Z TO CAMERA VIEW", TextAnchor.MiddleCenter, BaseColor.Yellow, 0, 20, false);
			GUIM.DrawText(rHideGUI, "PRESS H TO HIDE GUI", TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, false);
			DrawLoadMenu();
			DrawSaveMenu();
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Z))
		{
			bool active = spec.active;
			spec.active = !active;
			mlook.active = !active;
		}
		if (Input.GetKeyUp(KeyCode.H))
		{
			show = !show;
		}
		if (Input.GetKeyUp(KeyCode.Delete))
		{
			if (csrg.target != null)
			{
				if (csrg.target.gameObject.name == "Map")
				{
					return;
				}
				for (int i = 0; i < objlist.Count; i++)
				{
					if (csrg.target == objlist[i].go.transform)
					{
						objlist.RemoveAt(i);
						break;
					}
				}
				Object.Destroy(csrg.target.gameObject);
			}
			csrg.target = null;
		}
		if (Input.GetKeyUp(KeyCode.Q))
		{
			csrg.selectparent = !csrg.selectparent;
		}
		if (Input.GetKeyUp(KeyCode.R))
		{
			bool flag = true;
			spec.active = !flag;
			mlook.active = !flag;
			base.transform.localPosition = Vector3.zero;
			base.transform.localEulerAngles = Vector3.zero;
		}
		if (itemtype != 5)
		{
			return;
		}
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			GameObject gameObject = FindGameObjectByItemtype(200);
			if (!(gameObject == null))
			{
				Animation animation = gameObject.GetComponent<Animation>();
				if (animation == null)
				{
					animation = gameObject.AddComponent<Animation>();
				}
				AnimationClip animationClip = Resources.Load(varfweapon[curritem] + "_wield") as AnimationClip;
				if (animationClip == null)
				{
					animationClip = Resources.Load("animation/" + varfweapon[curritem] + "_wield") as AnimationClip;
				}
				if (!(animationClip == null))
				{
					animation.AddClip(animationClip, varfweapon[curritem] + "_wield");
					animation.clip = animation.GetClip(varfweapon[curritem] + "_wield");
					animation.Stop();
					animation.Play();
				}
			}
		}
		else if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			GameObject gameObject2 = FindGameObjectByItemtype(200);
			if (gameObject2 == null)
			{
				return;
			}
			Animation animation2 = gameObject2.GetComponent<Animation>();
			if (animation2 == null)
			{
				animation2 = gameObject2.AddComponent<Animation>();
			}
			AnimationClip animationClip2 = Resources.Load(varfweapon[curritem] + "_shoot") as AnimationClip;
			if (animationClip2 == null)
			{
				animationClip2 = Resources.Load("animation/" + varfweapon[curritem] + "_shoot") as AnimationClip;
			}
			if (!(animationClip2 == null))
			{
				animation2.AddClip(animationClip2, varfweapon[curritem] + "_shoot");
				animation2.clip = animation2.GetClip(varfweapon[curritem] + "_shoot");
				animation2.Stop();
				animation2.Play();
				AudioSource audioSource = gameObject2.GetComponent<AudioSource>();
				if (audioSource == null)
				{
					audioSource = gameObject2.AddComponent<AudioSource>();
				}
				audioSource.spatialBlend = 0f;
				AudioClip clip = Resources.Load("sounds/s_" + varfweapon[curritem]) as AudioClip;
				audioSource.PlayOneShot(clip);
			}
		}
		else
		{
			if (!Input.GetKeyUp(KeyCode.Alpha3))
			{
				return;
			}
			GameObject gameObject3 = FindGameObjectByItemtype(200);
			if (gameObject3 == null)
			{
				return;
			}
			Animation animation3 = gameObject3.GetComponent<Animation>();
			if (animation3 == null)
			{
				animation3 = gameObject3.AddComponent<Animation>();
			}
			AnimationClip animationClip3 = Resources.Load(varfweapon[curritem] + "_shoot2") as AnimationClip;
			if (animationClip3 == null)
			{
				animationClip3 = Resources.Load("animation/" + varfweapon[curritem] + "_shoot2") as AnimationClip;
			}
			if (!(animationClip3 == null))
			{
				animation3.AddClip(animationClip3, varfweapon[curritem] + "_shoot2");
				animation3.clip = animation3.GetClip(varfweapon[curritem] + "_shoot2");
				animation3.Stop();
				animation3.Play();
				AudioSource audioSource2 = gameObject3.GetComponent<AudioSource>();
				if (audioSource2 == null)
				{
					audioSource2 = gameObject3.AddComponent<AudioSource>();
				}
				audioSource2.spatialBlend = 0f;
				AudioClip clip2 = Resources.Load("sounds/s_" + varfweapon[curritem]) as AudioClip;
				audioSource2.PlayOneShot(clip2);
			}
		}
	}

	private void DrawTools()
	{
		Rect r = new Rect(GUIM.YRES(32f) + GUIM.YRES(40f) * 1f, GUIM.YRES(100f), GUIM.YRES(32f), GUIM.YRES(32f));
		GUIM.DrawText(new Rect(GUIM.YRES(32f) + GUIM.YRES(40f) * 1f, GUIM.YRES(100f) - GUIM.YRES(40f), GUIM.YRES(32f), GUIM.YRES(32f)), "Q", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		res = GUIM.Button(r, (!csrg.selectparent) ? BaseColor.White : BaseColor.Yellow, "FULL", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res)
		{
			csrg.selectparent = !csrg.selectparent;
		}
		rTool[0] = new Rect(GUIM.YRES(32f) + GUIM.YRES(40f) * 1f, GUIM.YRES(200f), GUIM.YRES(32f), GUIM.YRES(32f));
		rTool[1] = new Rect(GUIM.YRES(32f) + GUIM.YRES(40f) * 2f, GUIM.YRES(200f), GUIM.YRES(32f), GUIM.YRES(32f));
		rTool[2] = new Rect(GUIM.YRES(32f) + GUIM.YRES(40f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f), GUIM.YRES(32f));
		rToolKey[0] = new Rect(GUIM.YRES(32f) + GUIM.YRES(40f) * 1f, GUIM.YRES(200f) - GUIM.YRES(32f), GUIM.YRES(32f), GUIM.YRES(32f));
		rToolKey[1] = new Rect(GUIM.YRES(32f) + GUIM.YRES(40f) * 2f, GUIM.YRES(200f) - GUIM.YRES(32f), GUIM.YRES(32f), GUIM.YRES(32f));
		rToolKey[2] = new Rect(GUIM.YRES(32f) + GUIM.YRES(40f) * 3f, GUIM.YRES(200f) - GUIM.YRES(32f), GUIM.YRES(32f), GUIM.YRES(32f));
		GUIM.DrawText(rToolKey[0], "1", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		GUIM.DrawText(rToolKey[1], "2", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		GUIM.DrawText(rToolKey[2], "3", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		res = GUIM.Button(rTool[0], (csrg.type != 0) ? BaseColor.White : BaseColor.Yellow, "MOVE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res)
		{
			csrg.type = TransformType.Move;
		}
		res = GUIM.Button(rTool[1], (csrg.type != TransformType.Rotate) ? BaseColor.White : BaseColor.Yellow, "ROT", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res)
		{
			csrg.type = TransformType.Rotate;
		}
		res = GUIM.Button(rTool[2], (csrg.type != TransformType.Scale) ? BaseColor.White : BaseColor.Yellow, "SCALE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res)
		{
			csrg.type = TransformType.Scale;
		}
	}

	private void DrawSystem()
	{
		GUI.DrawTexture(rTop, TEX.tGray);
		GUI.DrawTexture(rTopLine, TEX.tBlack);
		GUIM.DrawText(rFile, "FILE", TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, false);
		GUIM.DrawText(rView, "VIEW", TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, false);
		GUIM.DrawText(rObject, "OBJECT", TextAnchor.MiddleCenter, BaseColor.LightGray2, 0, 20, false);
		if (GUIM.HideButton(rFile))
		{
			menuid = 0;
		}
		if (GUIM.HideButton(rView))
		{
			menuid = 1;
		}
		if (GUIM.HideButton(rObject))
		{
			menuid = 2;
		}
		if (menuid == 0)
		{
			GUI.DrawTexture(rFileMenu, TEX.tLightGray);
			GUIM.DrawText(rFile, "FILE", TextAnchor.MiddleCenter, BaseColor.Gray, 0, 20, false);
			if (GUIM.Contains(rFileSlot[0]))
			{
				GUI.DrawTexture(rFileSlot[0], TEX.tYellow);
			}
			GUIM.DrawText(rFileSlot[0], "OPEN", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			GUIM.HideButton(rFileSlot[0]);
			if (GUIM.Contains(rFileSlot[1]))
			{
				GUI.DrawTexture(rFileSlot[1], TEX.tYellow);
			}
			GUIM.DrawText(rFileSlot[1], "SAVE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			GUIM.HideButton(rFileSlot[1]);
			if (GUIM.Contains(rFileSlot[2]))
			{
				GUI.DrawTexture(rFileSlot[2], TEX.tYellow);
			}
			GUIM.DrawText(rFileSlot[2], "EXIT", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			if (GUIM.HideButton(rFileSlot[2]))
			{
				Exit();
			}
		}
		else if (menuid == 1)
		{
			GUI.DrawTexture(rViewMenu, TEX.tLightGray);
			GUIM.DrawText(rView, "VIEW", TextAnchor.MiddleCenter, BaseColor.Gray, 0, 20, false);
			if (GUIM.Contains(rViewSlot[0]))
			{
				GUI.DrawTexture(rViewSlot[0], TEX.tYellow);
			}
			GUIM.DrawText(rViewSlot[0], "CHROMAKEY", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			if (GUIM.HideButton(rViewSlot[0]))
			{
				if (GUI3D.csCam.backgroundColor != Color.gray)
				{
					GUI3D.csCam.backgroundColor = Color.gray;
				}
				else
				{
					GUI3D.csCam.backgroundColor = Color.green;
				}
			}
			if (GUIM.Contains(rViewSlot[1]))
			{
				GUI.DrawTexture(rViewSlot[1], TEX.tYellow);
			}
			GUIM.DrawText(rViewSlot[1], "CHANGEFOV", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			if (GUIM.HideButton(rViewSlot[1]))
			{
				if (GUI3D.csCam.fieldOfView == 65f)
				{
					GUI3D.csCam.fieldOfView = 40f;
				}
				else
				{
					GUI3D.csCam.fieldOfView = 65f;
				}
			}
			if (GUIM.Contains(rViewSlot[2]))
			{
				GUI.DrawTexture(rViewSlot[2], TEX.tYellow);
			}
			GUIM.DrawText(rViewSlot[2], "CASECAM", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			if (GUIM.HideButton(rViewSlot[2]))
			{
				GUI3D.csCam.fieldOfView = 10f;
				GameObject gameObject = CaseGen.Build("case_newyear2020");
				gameObject.transform.position = new Vector3(0f, -0.35f, 8f);
				gameObject.transform.eulerAngles = new Vector3(350f, 45f, 350f);
				GameObject.Find("Dlight").transform.eulerAngles = new Vector3(15f, 15f, 0f);
				gameObject.layer = 0;
				GameObject gameObject2 = GameObject.Find(gameObject.name + "/box");
				GameObject obj = GameObject.Find(gameObject.name + "/cover");
				gameObject2.layer = 0;
				obj.layer = 0;
				gameObject2.AddComponent<BoxCollider>();
				obj.AddComponent<BoxCollider>();
				objlist.Add(new SceneObject(gameObject, 255));
			}
			if (GUIM.Contains(rViewSlot[3]))
			{
				GUI.DrawTexture(rViewSlot[3], TEX.tYellow);
			}
			GUIM.DrawText(rViewSlot[3], "TOGGLE SKY", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			if (GUIM.HideButton(rViewSlot[3]))
			{
				if (GUI3D.csCam.clearFlags == CameraClearFlags.Color)
				{
					GUI3D.csCam.clearFlags = CameraClearFlags.Skybox;
				}
				else
				{
					GUI3D.csCam.clearFlags = CameraClearFlags.Color;
				}
			}
		}
		else if (menuid == 2)
		{
			GUI.DrawTexture(rObjectMenu, TEX.tLightGray);
			GUIM.DrawText(rObject, "OBJECT", TextAnchor.MiddleCenter, BaseColor.Gray, 0, 20, false);
			if (GUIM.Contains(rObjectSlot[0]))
			{
				GUI.DrawTexture(rObjectSlot[0], TEX.tYellow);
			}
			if (csrg.target == null)
			{
				GUI.DrawTexture(rObjectSlot[1], TEX.tRed);
			}
			GUIM.DrawText(rObjectSlot[0], "OPEN", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			if (GUIM.HideButton(rObjectSlot[0]))
			{
				menuid = -1;
				objectmenuid = 0;
				currpdata = -1;
				bin_bpdata_load();
			}
			if (GUIM.Contains(rObjectSlot[1]))
			{
				GUI.DrawTexture(rObjectSlot[1], TEX.tYellow);
			}
			if (csrg.target == null)
			{
				GUI.DrawTexture(rObjectSlot[1], TEX.tRed);
			}
			GUIM.DrawText(rObjectSlot[1], "SAVE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			if (GUIM.HideButton(rObjectSlot[1]) && csrg.target != null)
			{
				menuid = -1;
				objectmenuid = 1;
				filename = "newfile";
			}
		}
		if (menuid >= 0 && GUIM.HideButton(rHide))
		{
			menuid = -1;
		}
	}

	private void DrawMap()
	{
	}

	private void DrawItem()
	{
		GUI.DrawTexture(rLeft, tArrow[0]);
		if (GUIM.HideButton(rLeft))
		{
			itemtype--;
			if (itemtype < 0)
			{
				itemtype = 0;
			}
			curritem = 1;
		}
		GUI.DrawTexture(rRight, tArrow[1]);
		if (GUIM.HideButton(rRight))
		{
			curritem = 1;
			itemtype++;
			if (itemtype > 2)
			{
				itemtype = 2;
			}
		}
		res = GUIM.Button(rItem, BaseColor.White, itemname[itemtype], TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (itemtype == 0)
		{
			DrawItemChar();
		}
		else if (itemtype == 1)
		{
			DrawItemCase();
		}
		else if (itemtype == 2)
		{
			DrawItemWeapon();
		}
		else if (itemtype == 3)
		{
			DrawItemMap();
		}
		else if (itemtype == 4)
		{
			DrawItemObj();
		}
		else if (itemtype == 5)
		{
			DrawItemFWeapon();
		}
	}

	private void DrawItemChar()
	{
		DrawSelector(0, ref curritem, ref team);
		DrawSelector(1, ref currskinstate, ref skinstate);
		DrawSelector(2, ref currskinid, ref skinlist);
		DrawSelector(3, ref currcolorskin, ref colorskinlist);
		DrawSelector(4, ref currcoloreye, ref coloreyelist);
		DrawSelector(5, ref currcolorhair, ref colorhairlist);
		DrawSelector(6, ref curreye, ref eyelist);
		DrawSelector(7, ref currbeard, ref beardlist);
		DrawSelector(8, ref currhair, ref hairlist);
		res = GUIM.Button(rSlot[9], BaseColor.White, "SPAWN", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res)
		{
			SpawnCharacter();
		}
	}

	private void DrawSelector(int slot, ref int val, ref string[] list)
	{
		GUIM.DrawText(rSlot[slot], list[val], TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		GUI.DrawTexture(rSlotArrowLeft[slot], tArrow[0]);
		GUI.DrawTexture(rSlotArrowRight[slot], tArrow[1]);
		if (GUIM.HideButton(rSlotArrowLeft[slot]))
		{
			val--;
			if (val < 0)
			{
				val = 0;
			}
		}
		if (GUIM.HideButton(rSlotArrowRight[slot]))
		{
			val++;
			if (val >= list.Length)
			{
				val = list.Length - 1;
			}
		}
	}

	private SceneObject SpawnCharacter()
	{
		PlayerData playerData = new PlayerData();
		playerData.team = curritem;
		playerData.skinstate = currskinstate;
		PlayerSkinInfo playerSkinInfo = GUIInv.psinfo[currskinid];
		if (playerSkinInfo != null)
		{
			if (playerSkinInfo.hatid > 0)
			{
				playerData.cp[6] = playerSkinInfo.hatid;
			}
			if (playerSkinInfo.bodyid > 0)
			{
				playerData.cp[7] = playerSkinInfo.bodyid;
			}
			if (playerSkinInfo.pantsid > 0)
			{
				playerData.cp[8] = playerSkinInfo.pantsid;
			}
			if (playerSkinInfo.bootsid > 0)
			{
				playerData.cp[9] = playerSkinInfo.bodyid;
			}
		}
		playerData.cp[0] = currcolorskin;
		playerData.cp[1] = currcoloreye;
		playerData.cp[2] = currcolorhair;
		playerData.cp[3] = currhair;
		playerData.cp[4] = curreye;
		playerData.cp[5] = currbeard;
		GameObject gameObject = VCGen.Build(playerData);
		gameObject.name = "char_" + gameObject.GetInstanceID();
		gameObject.transform.eulerAngles = new Vector3(0f, 210f, 0f);
		GameObject.Find(gameObject.name + "/body/head").transform.localEulerAngles = new Vector3(355f, 0f, 0f);
		GameObject.Find(gameObject.name + "/body/armhelp/LeftArmUp").transform.localPosition = new Vector3(-8.000148f, 0f, 0f);
		GameObject.Find(gameObject.name + "/body/armhelp/LeftArmUp").transform.localEulerAngles = new Vector3(-3.080189f, -1.88176f, 81.94669f);
		GameObject.Find(gameObject.name + "/body/armhelp/LeftArmUp/LeftArmDown").transform.localPosition = new Vector3(-12.00017f, 2.001597f, 0f);
		GameObject.Find(gameObject.name + "/body/armhelp/LeftArmUp/LeftArmDown").transform.localEulerAngles = new Vector3(358.6052f, 10.77333f, 12.44588f);
		GameObject.Find(gameObject.name + "/body/armhelp/RightArmUp").transform.localPosition = new Vector3(8.000176f, 0f, 0f);
		GameObject.Find(gameObject.name + "/body/armhelp/RightArmUp").transform.localEulerAngles = new Vector3(1.675015f, -1.381974f, 281.9428f);
		GameObject.Find(gameObject.name + "/body/armhelp/RightArmUp/RightArmDown").transform.localPosition = new Vector3(12.00018f, 2.000503f, 0f);
		GameObject.Find(gameObject.name + "/body/armhelp/RightArmUp/RightArmDown").transform.localEulerAngles = new Vector3(351.9885f, 4.819155f, 344.9216f);
		GameObject gameObject2 = GameObject.Find(gameObject.name + "/watersplash");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(false);
		}
		playerData.goArrow.SetActive(false);
		if (playerData.skinstate == 1)
		{
			VCGen.BuildZombieSkin(playerData);
			VCGen.UpdateTexturesZombie(playerData);
		}
		else if (playerData.skinstate == 2)
		{
			VCGen.BuildDamagedSkin(playerData);
			VCGen.UpdateTexturesDamaged(playerData);
		}
		SceneObject sceneObject = new SceneObject(gameObject, 0);
		objlist.Add(sceneObject);
		return sceneObject;
	}

	private void DrawItemCase()
	{
		if (GUIInv.cinfo[curritem] != null)
		{
			GUIM.DrawText(rSlot[0], GUIInv.cinfo[curritem].name, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		GUI.DrawTexture(rSlotArrowLeft[0], tArrow[0]);
		GUI.DrawTexture(rSlotArrowRight[0], tArrow[1]);
		if (GUIM.HideButton(rSlotArrowLeft[0]))
		{
			curritem--;
			if (curritem < 1)
			{
				curritem = 1;
			}
		}
		if (GUIM.HideButton(rSlotArrowRight[0]))
		{
			curritem++;
			if (GUIInv.cinfo[curritem] == null)
			{
				curritem = 1;
			}
		}
		res = GUIM.Button(rSlot[1], BaseColor.White, "SPAWN", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res && GUIInv.cinfo[curritem] != null)
		{
			GameObject gameObject = CaseGen.Build("case_" + GUIInv.cinfo[curritem].name);
			gameObject.layer = 0;
			GameObject gameObject2 = GameObject.Find(gameObject.name + "/box");
			GameObject obj = GameObject.Find(gameObject.name + "/cover");
			gameObject2.layer = 0;
			obj.layer = 0;
			gameObject2.AddComponent<BoxCollider>();
			obj.AddComponent<BoxCollider>();
			objlist.Add(new SceneObject(gameObject, 1));
		}
	}

	private void DrawItemWeapon()
	{
		GUIM.DrawText(rSlot[0], GUIPlay.varweapon[curritem], TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		GUI.DrawTexture(rSlotArrowLeft[0], tArrow[0]);
		GUI.DrawTexture(rSlotArrowRight[0], tArrow[1]);
		if (GUIM.HideButton(rSlotArrowLeft[0]))
		{
			curritem--;
			if (curritem < 0)
			{
				curritem = GUIPlay.varweapon.Length - 1;
			}
		}
		if (GUIM.HideButton(rSlotArrowRight[0]))
		{
			curritem++;
			if (curritem >= GUIPlay.varweapon.Length)
			{
				curritem = GUIPlay.varweapon.Length - 1;
			}
		}
		GUI.DrawTexture(rSlotArrowLeft[1], tArrow[0]);
		GUI.DrawTexture(rSlotArrowRight[1], tArrow[1]);
		if (GUIM.HideButton(rSlotArrowLeft[1]))
		{
			scopeitem--;
			if (scopeitem < 0)
			{
				scopeitem = 0;
			}
		}
		if (GUIM.HideButton(rSlotArrowRight[1]))
		{
			scopeitem++;
			if (scopeitem >= 12)
			{
				scopeitem = 11;
			}
		}
		if (scopeitem == 0)
		{
			GUIM.DrawText(rSlot[1], "NO SCOPE", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		else
		{
			GUIM.DrawText(rSlot[1], ScopeGen.scope[scopeitem].name_, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		res = GUIM.Button(rSlot[2], BaseColor.White, "SPAWN", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res)
		{
			WeaponData weaponData = VWGen2.BuildWeaponPlayer(GUIPlay.varweapon[curritem], scopeitem);
			weaponData.goPW.AddComponent<BoxCollider>();
			objlist.Add(new SceneObject(weaponData.goPW, 2));
		}
	}

	private void DrawItemMap()
	{
		if (GUIMap.maplist.Count > 0)
		{
			GUIM.DrawText(rSlot[0], GUIMap.maplist[curritem].title, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		GUI.DrawTexture(rSlotArrowLeft[0], tArrow[0]);
		GUI.DrawTexture(rSlotArrowRight[0], tArrow[1]);
		if (GUIM.HideButton(rSlotArrowLeft[0]))
		{
			curritem--;
			if (curritem < 0)
			{
				curritem = 0;
			}
		}
		if (GUIM.HideButton(rSlotArrowRight[0]))
		{
			curritem++;
			if (curritem >= GUIMap.maplist.Count)
			{
				curritem = GUIMap.maplist.Count - 1;
			}
			if (GUIMap.maplist.Count == 0)
			{
				curritem = 0;
			}
		}
		if (GUIMap.maplist.Count > 0)
		{
			res = GUIM.Button(rSlot[1], BaseColor.White, "SPAWN", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			if (res)
			{
				Map.Clear();
				Map.LoadBin(GUIMap.maplist[curritem].title);
			}
		}
	}

	private void DrawItemObj()
	{
		if (GUIMap.maplist.Count > 0)
		{
			GUIM.DrawText(rSlot[0], objfilelist[curritem], TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		}
		GUI.DrawTexture(rSlotArrowLeft[0], tArrow[0]);
		GUI.DrawTexture(rSlotArrowRight[0], tArrow[1]);
		if (GUIM.HideButton(rSlotArrowLeft[0]))
		{
			curritem--;
			if (curritem < 0)
			{
				curritem = 0;
			}
		}
		if (GUIM.HideButton(rSlotArrowRight[0]))
		{
			curritem++;
			if (curritem >= objfilelist.Count)
			{
				curritem = objfilelist.Count - 1;
			}
			if (objfilelist.Count == 0)
			{
				curritem = 0;
			}
		}
		if (objfilelist.Count > 0)
		{
			res = GUIM.Button(rSlot[1], BaseColor.White, "SPAWN", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
			if (res)
			{
				Console.cs.Command("obj " + objfilelist[curritem]);
			}
		}
	}

	private void DrawItemFWeapon()
	{
		GUIM.DrawText(rSlot[0], varfweapon[curritem], TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		GUI.DrawTexture(rSlotArrowLeft[0], tArrow[0]);
		GUI.DrawTexture(rSlotArrowRight[0], tArrow[1]);
		if (GUIM.HideButton(rSlotArrowLeft[0]))
		{
			curritem--;
			if (curritem < 0)
			{
				curritem = 0;
			}
		}
		if (GUIM.HideButton(rSlotArrowRight[0]))
		{
			curritem++;
			if (curritem >= varfweapon.Length)
			{
				curritem = varfweapon.Length - 1;
			}
		}
		res = GUIM.Button(rSlot[1], BaseColor.White, "SPAWN", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res)
		{
			GameObject obj = VCGen.Build(Controll.pl);
			wd = VWGen.BuildWeaponFPS(wi: new WeaponInfo
			{
				id = 0
			}, pl: Controll.pl, weaponname: varfweapon[curritem]);
			objlist.Add(new SceneObject(wd.go, 200));
			Object.Destroy(obj);
			Object.Destroy(wd.goPW);
			wd.go.SetActive(true);
			wd.go.transform.localScale = Vector3.one;
			VWIK.EditorUpdate(wd);
			VWIK.EditorUpdate(wd);
		}
		res = GUIM.Button(rSlot[2], BaseColor.White, "RESET", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res)
		{
			for (int i = 0; i < objlist.Count; i++)
			{
				if (objlist[i].itemtype == 200)
				{
					Object.Destroy(objlist[i].go);
				}
			}
		}
		GUIM.DrawText(rSlot[4], "W POSITION", TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(rSlot[5], "L POSITION", TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		GUIM.DrawText(rSlot[6], "L ROTATION", TextAnchor.MiddleLeft, BaseColor.Yellow, 0, 20, false);
		DrawVector3(new Rect(rSlot[4].x + rSlot[4].width, rSlot[4].y, rSlot[4].width, rSlot[4].height), ref wpos);
		DrawVector3(new Rect(rSlot[5].x + rSlot[5].width, rSlot[5].y, rSlot[5].width, rSlot[5].height), ref lpos);
		DrawVector3(new Rect(rSlot[5].x + rSlot[5].width, rSlot[5].y, rSlot[5].width, rSlot[5].height), ref lrot);
		res = GUIM.Button(rSlot[9], BaseColor.White, "UPDATE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		if (res && wd != null)
		{
			VWIK.EditorUpdate(wd);
		}
	}

	private bool DrawVector3(Rect r, ref Vector3 v)
	{
		if (rv == null)
		{
			rv = new Rect[3];
		}
		rv[0].Set(r.x, r.y, GUIM.YRES(60f), r.height);
		rv[1].Set(r.x + GUIM.YRES(70f), r.y, GUIM.YRES(60f), r.height);
		rv[2].Set(r.x + GUIM.YRES(140f), r.y, GUIM.YRES(60f), r.height);
		GUI.DrawTexture(rv[0], TEX.tBlack);
		GUI.DrawTexture(rv[1], TEX.tBlack);
		GUI.DrawTexture(rv[2], TEX.tBlack);
		GUIM.DrawText(rv[0], v.x.ToString("0.00"), TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		GUIM.DrawText(rv[1], v.y.ToString("0.00"), TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		GUIM.DrawText(rv[2], v.z.ToString("0.00"), TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		return false;
	}

	private GameObject FindGameObjectByItemtype(int t)
	{
		for (int i = 0; i < objlist.Count; i++)
		{
			if (objlist[i].itemtype == t)
			{
				return objlist[i].go;
			}
		}
		return null;
	}

	private SceneObject FindSceneObjectByTransform(Transform tr)
	{
		for (int i = 0; i < objlist.Count; i++)
		{
			if (objlist[i].go.transform == tr)
			{
				return objlist[i];
			}
		}
		return null;
	}

	private void Exit()
	{
		Object.Destroy(csfxaa);
		Object.Destroy(csaoe);
		Object.Destroy(spec);
		Object.Destroy(mlook);
		Object.Destroy(csrg);
		Object.Destroy(csal);
		Object.Destroy(this);
		Cursor.visible = true;
		Main.SetActive(true);
		GameObject.Find("GUI").transform.localPosition = Vector3.zero;
		GameObject.Find("GUI").transform.localEulerAngles = Vector3.zero;
		for (int i = 0; i < objlist.Count; i++)
		{
			if (objlist[i].go != null)
			{
				Object.Destroy(objlist[i].go);
			}
		}
		objlist.Clear();
		Map.Clear();
		GUI3D.csCam.clearFlags = CameraClearFlags.Depth;
		GUI3D.csCam.cullingMask = 512;
		GUI3D.csCam.fieldOfView = 20f;
		GUI3D.csCam.enabled = false;
		if (Controll.dof != null)
		{
			Object.Destroy(Controll.dof);
		}
		OutlineSystem component = GetComponent<OutlineSystem>();
		if (component != null)
		{
			Object.Destroy(component);
		}
		GameObject.Find("Map").transform.localPosition = Vector3.zero;
		GameObject.Find("Map").transform.localEulerAngles = Vector3.zero;
		GameObject.Find("Map").transform.localScale = Vector3.one;
		GameObject.Find("GUI").transform.localPosition = Vector3.zero;
		GameObject.Find("GUI").transform.localEulerAngles = Vector3.zero;
		GameObject.Find("GUI").transform.localScale = Vector3.one;
	}

	private void DrawLoadMenu()
	{
		if (objectmenuid != 0)
		{
			return;
		}
		Rect position = new Rect(0f, 0f, GUIM.YRES(300f), GUIM.YRES(500f));
		position.center = new Vector2(Screen.width, Screen.height) / 2f;
		Rect position2 = new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f);
		GUI.DrawTexture(position, TEX.tBlack);
		GUI.DrawTexture(position2, TEX.tGray);
		GUIM.DrawText(new Rect(position.x, position.y, position.width, GUIM.YRES(32f)), "LOAD OBJECT DATA", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		if (GUIM.Button(new Rect(position2.x + position2.width - GUIM.YRES(44f), position2.y + GUIM.YRES(4f), GUIM.YRES(40f), GUIM.YRES(28f)), BaseColor.Gray, "CLOSE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			objectmenuid = -1;
		}
		Rect rect = new Rect(position.x + GUIM.YRES(8f), position.y + GUIM.YRES(40f), position.width - GUIM.YRES(16f), position.height - GUIM.YRES(96f));
		GUI.DrawTexture(rect, TEX.tBlack);
		Rect scrollzone = new Rect(0f, 0f, rect.width - GUIM.YRES(20f), (float)pdatafilelist.Count * GUIM.YRES(26f));
		sv = GUIM.BeginScrollView(rect, sv, scrollzone);
		int num = 0;
		for (int i = 0; i < pdatafilelist.Count; i++)
		{
			Rect rect2 = new Rect(0f, (float)num * GUIM.YRES(26f), scrollzone.width, GUIM.YRES(26f));
			if (GUIM.HideButton(rect2))
			{
				currpdata = i;
			}
			if (i == currpdata)
			{
				GUI.DrawTexture(rect2, TEX.tBlue);
			}
			GUIM.DrawText(rect2, pdatafilelist[i], TextAnchor.MiddleLeft, BaseColor.White, 0, 20, false);
			num++;
		}
		GUIM.EndScrollView();
		if (GUIM.Button(new Rect(position.x + GUIM.YRES(8f), position.y + position.height - GUIM.YRES(48f), position.width - GUIM.YRES(16f), GUIM.YRES(40f)), BaseColor.White, "LOAD", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			objectmenuid = -1;
			LoadObject(pdatafilelist[currpdata]);
		}
	}

	private void DrawSaveMenu()
	{
		if (objectmenuid != 1 || csrg.target == null)
		{
			return;
		}
		rMenuSave = new Rect(0f, 0f, GUIM.YRES(300f), GUIM.YRES(134f));
		rMenuSave.center = new Vector2(Screen.width, Screen.height) / 2f;
		Rect position = new Rect(rMenuSave.x + 1f, rMenuSave.y + 1f, rMenuSave.width - 2f, rMenuSave.height - 2f);
		GUI.DrawTexture(rMenuSave, TEX.tBlack);
		GUI.DrawTexture(position, TEX.tGray);
		GUIM.DrawText(new Rect(rMenuSave.x, rMenuSave.y, rMenuSave.width, GUIM.YRES(32f)), "LOAD OBJECT DATA", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		if (GUIM.Button(new Rect(position.x + position.width - GUIM.YRES(44f), position.y + GUIM.YRES(4f), GUIM.YRES(40f), GUIM.YRES(28f)), BaseColor.Gray, "CLOSE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			objectmenuid = -1;
		}
		Rect rect = new Rect(rMenuSave.x + GUIM.YRES(8f), rMenuSave.y + GUIM.YRES(40f), rMenuSave.width - GUIM.YRES(16f), GUIM.YRES(40f));
		GUI.DrawTexture(rect, TEX.tBlack);
		GUIM.DrawEdit(rect, ref filename, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		if (GUIM.Button(new Rect(rMenuSave.x + GUIM.YRES(8f), rMenuSave.y + rMenuSave.height - GUIM.YRES(48f), rMenuSave.width - GUIM.YRES(16f), GUIM.YRES(40f)), BaseColor.White, "SAVE FILE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Debug.Log("> SAVE FILE " + csrg.target.gameObject.name);
			SceneObject sceneObject = FindSceneObjectByTransform(csrg.target);
			if (sceneObject == null)
			{
				Debug.Log("NOT FOUND");
				return;
			}
			Debug.Log("saveing " + sceneObject.go.name + " " + sceneObject.itemtype);
			SaveObject(filename, sceneObject);
			objectmenuid = -1;
		}
	}

	public static void bin_bpdata_load()
	{
	}

	public static void bin_bpscene_load()
	{
	}

	private void LoadObject(string filename)
	{
	}

	private void SaveObject(string filename, SceneObject obj)
	{
	}

	private static Vector3 ParsVector3(string[] d)
	{
		if (d.Length != 5)
		{
			return Vector3.zero;
		}
		float result = 0f;
		float result2;
		float.TryParse(d[2], NumberStyles.Any, CultureInfo.InvariantCulture, out result2);
		float result3;
		float.TryParse(d[3], NumberStyles.Any, CultureInfo.InvariantCulture, out result3);
		float.TryParse(d[4], NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		return new Vector3(result2, result3, result);
	}
}
