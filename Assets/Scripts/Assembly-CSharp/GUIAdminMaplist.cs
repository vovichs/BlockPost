using System.Collections.Generic;
using GameClass;
using UnityEngine;

public class GUIAdminMaplist : MonoBehaviour
{
	public static GUIAdminMaplist cs;

	public bool show;

	public static List<MapData> offmaplist;

	public static List<MapData> rotmaplist;

	public MapData setmap;

	public MapData delmap;

	private int usermap;

	private Texture2D tBlack;

	private Texture2D tWhite;

	private Color[] cList;

	private Rect rBack;

	private Rect[] rList;

	private Rect[] rListText;

	private Rect[] rListContain;

	private Rect rScrollView;

	private Rect rScroll;

	private Rect rCol0;

	private Rect rCol1;

	private Rect[] rList2;

	private Rect[] rListText2;

	private Rect[] rListContain2;

	private Rect rScrollView2;

	private Rect rScroll2;

	private Vector2 sv = Vector2.zero;

	private Vector2 sv2 = Vector2.zero;

	private string sCol0 = "ДОСТУПНЫЕ КАРТЫ (0)";

	private string sCol1 = "РОТАЦИЯ КАРТ (0)";

	private Rect rAdd;

	private Rect rDel;

	private Rect rStart;

	private Rect rScrollOff;

	private Rect rScrollRot;

	private string sAddRotation;

	private string sMapRequestAdd;

	private string sMapRequestDel;

	private string sMapRequestStart;

	private string sMapDel;

	private string sMapStart;

	private void Awake()
	{
		cs = this;
		sCol0 = Lang.GetString("_MAPS_AVAILABLE") + " (0)";
		sCol1 = Lang.GetString("_MAPS_ROTATION") + " (0)";
		OnResize();
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		cList = new Color[4];
		cList[0] = new Color(0.1f, 0.1f, 0.1f, 1f);
		cList[1] = new Color(0.2f, 0.2f, 0.2f, 1f);
		cList[2] = new Color(0.3f, 0.3f, 0.3f, 1f);
		cList[3] = new Color(0.6f, 0.6f, 0.2f, 1f);
	}

	private void Start()
	{
		LoadLang();
	}

	private void LoadLang()
	{
		sAddRotation = Lang.GetString("_ADD_TO_ROTATION");
		sMapRequestAdd = Lang.GetString("_REQUEST_ADDING_TO_ROTATION");
		sMapRequestDel = Lang.GetString("_REQUEST_REMOVE_FROM_ROTATION");
		sMapRequestStart = Lang.GetString("_REQUEST_LAUNCH_MAP");
		sMapDel = Lang.GetString("_REMOVE_FROM_ROTATION");
		sMapStart = Lang.GetString("_LAUNCH_MAP");
	}

	public void OnResize()
	{
		int num = (int)GUIM.YRES(16f);
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(200f), (float)Screen.height / 2f - GUIM.YRES(200f), GUIM.YRES(600f), GUIM.YRES(400f));
		rCol0 = new Rect(rBack.x + GUIM.YRES(16f), rBack.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(26f));
		rCol1 = new Rect(rBack.x + GUIM.YRES(16f) + GUIM.YRES(256f), rBack.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(26f));
		rScrollView = new Rect(rBack.x + (float)num, rBack.y + (float)num + GUIM.YRES(26f), GUIM.YRES(240f), rBack.height - (float)(num * 2) - GUIM.YRES(64f));
		rScrollView2 = new Rect(rBack.x + (float)num + GUIM.YRES(256f), rBack.y + (float)num + GUIM.YRES(26f), GUIM.YRES(240f), rBack.height - (float)(num * 2) - GUIM.YRES(64f));
		if (offmaplist != null || GUIAdminUpload.maplist_sv != null)
		{
			int num2 = ((offmaplist != null) ? offmaplist.Count : 0) + ((GUIAdminUpload.maplist_sv != null) ? GUIAdminUpload.maplist_sv.Count : 0);
			rScrollOff = new Rect(0f, 0f, rScrollView.width - GUIM.YRES(20f), (float)num2 * GUIM.YRES(26f));
			if (num2 > 0)
			{
				rList = new Rect[num2];
				rListText = new Rect[num2];
				rListContain = new Rect[num2];
			}
			for (int i = 0; i < num2; i++)
			{
				rList[i] = new Rect(0f, (float)i * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
				rListText[i] = new Rect(GUIM.YRES(8f), (float)i * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
				rListContain[i] = new Rect(0f, (float)i * GUIM.YRES(26f), GUIM.YRES(220f), GUIM.YRES(26f));
			}
			sCol0 = Lang.GetString("_MAPS_AVAILABLE") + "  (" + num2 + ")";
		}
		if (rotmaplist != null)
		{
			int count = rotmaplist.Count;
			rScrollRot = new Rect(0f, 0f, rScrollView.width - GUIM.YRES(20f), (float)count * GUIM.YRES(26f));
			if (count > 0)
			{
				rList2 = new Rect[count];
				rListText2 = new Rect[count];
				rListContain2 = new Rect[count];
			}
			for (int j = 0; j < count; j++)
			{
				rList2[j] = new Rect(0f, (float)j * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
				rListText2[j] = new Rect(GUIM.YRES(8f), (float)j * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
				rListContain2[j] = new Rect(0f, (float)j * GUIM.YRES(26f), GUIM.YRES(220f), GUIM.YRES(26f));
			}
			sCol1 = Lang.GetString("_MAPS_ROTATION") + "  (" + count + ")";
		}
		rAdd = new Rect(rBack.x + (float)num, rBack.y + rBack.height - (float)num - GUIM.YRES(32f), GUIM.YRES(240f), GUIM.YRES(32f));
		rDel = new Rect(rBack.x + (float)num + GUIM.YRES(256f), rBack.y + rBack.height - (float)num - GUIM.YRES(32f), GUIM.YRES(240f), GUIM.YRES(32f));
		rStart = new Rect(rBack.x + (float)num + GUIM.YRES(256f), rBack.y + rBack.height - (float)num - GUIM.YRES(68f), GUIM.YRES(240f), GUIM.YRES(32f));
	}

	private void OnGUI()
	{
		if (show)
		{
			GUI.depth = -2;
			GUI.DrawTexture(rBack, tBlack);
			DrawMapList();
			DrawMapListFile();
			DrawButtons();
		}
	}

	private void DrawMapList()
	{
		GUIM.DrawText(rCol0, sCol0, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		if (offmaplist == null)
		{
			return;
		}
		sv = GUIM.BeginScrollView(rScrollView, sv, rScrollOff);
		for (int i = 0; i < offmaplist.Count; i++)
		{
			GUI.color = ((i % 2 == 0) ? cList[0] : cList[1]);
			if (GUIM.Contains(rListContain[i]))
			{
				GUI.color = cList[2];
			}
			if (setmap == offmaplist[i])
			{
				GUI.color = cList[3];
			}
			GUI.DrawTexture(rList[i], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rListText[i], offmaplist[i].title, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
			if (GUIM.HideButton(rList[i]))
			{
				setmap = offmaplist[i];
				delmap = null;
				usermap = 0;
			}
		}
		if (GUIAdminUpload.maplist_sv != null)
		{
			int num = offmaplist.Count;
			for (int j = 0; j < GUIAdminUpload.maplist_sv.Count; j++)
			{
				GUI.color = ((num % 2 == 0) ? cList[0] : cList[1]);
				if (GUIM.Contains(rListContain[num]))
				{
					GUI.color = cList[2];
				}
				if (setmap == GUIAdminUpload.maplist_sv[j])
				{
					GUI.color = cList[3];
				}
				GUI.DrawTexture(rList[num], tWhite);
				GUI.color = Color.white;
				GUIM.DrawText(rListText[num], GUIAdminUpload.maplist_sv[j].title, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
				if (GUIM.HideButton(rList[num]))
				{
					setmap = GUIAdminUpload.maplist_sv[j];
					delmap = null;
					usermap = 1;
				}
				num++;
			}
		}
		GUIM.EndScrollView();
	}

	private void DrawMapListFile()
	{
		GUIM.DrawText(rCol1, sCol1, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		if (rotmaplist == null)
		{
			return;
		}
		sv2 = GUIM.BeginScrollView(rScrollView2, sv2, rScroll2);
		for (int i = 0; i < rotmaplist.Count; i++)
		{
			GUI.color = ((i % 2 == 0) ? cList[0] : cList[1]);
			if (GUIM.Contains(rListContain2[i]))
			{
				GUI.color = cList[2];
			}
			if (delmap == rotmaplist[i])
			{
				GUI.color = cList[3];
			}
			GUI.DrawTexture(rList2[i], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rListText2[i], rotmaplist[i].title, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
			if (GUIM.HideButton(rList2[i]))
			{
				delmap = rotmaplist[i];
				setmap = null;
			}
		}
		GUIM.EndScrollView();
	}

	private void DrawButtons()
	{
		bool flag = setmap != null;
		bool flag2 = delmap != null;
		if (GUIM.Button(rAdd, flag ? BaseColor.Yellow : BaseColor.Block, sAddRotation, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && flag)
		{
			Client.cs.send_customop(1, setmap.sv_index, usermap);
			GUIAdmin.cs.Log("> " + sMapRequestAdd + " (" + setmap.title + ")");
			setmap = null;
		}
		if (GUIM.Button(rDel, flag2 ? BaseColor.Yellow : BaseColor.Block, sMapDel, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && flag2)
		{
			Client.cs.send_customop(2, delmap.sv_index, 0);
			GUIAdmin.cs.Log("> " + sMapRequestDel + " (" + delmap.title + ")");
			delmap = null;
		}
		if (GUIM.Button(rStart, flag2 ? BaseColor.Green : BaseColor.Block, sMapStart, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && flag2)
		{
			Client.cs.send_customop(3, delmap.sv_index, 0);
			GUIAdmin.cs.Log("> " + sMapRequestStart + " (" + delmap.title + ")");
			delmap = null;
		}
	}
}
