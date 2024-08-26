using System.Collections.Generic;
using GameClass;
using UnityEngine;

public class GUIAdminUpload : MonoBehaviour
{
	public static GUIAdminUpload cs;

	public bool show = true;

	private List<MapData> maplist;

	public static List<MapData> maplist_sv;

	private MapData setmap;

	private MapData delmap;

	private int maplen;

	private byte[] mapdata;

	private Rect rBack;

	private Rect rScrollView;

	private Rect rScroll;

	private Rect[] rList;

	private Rect[] rListText;

	private Rect[] rListContain;

	private Rect rWebUpload;

	private Rect rUpload;

	private Rect rDelete;

	private Rect rCol0;

	private Rect rCol1;

	private Vector2 sv = Vector2.zero;

	private Rect rScrollView_sv;

	private Rect rScroll_sv;

	private Rect[] rList_sv;

	private Rect[] rListText_sv;

	private Rect[] rListContain_sv;

	private Vector2 sv2 = Vector2.zero;

	private Texture2D tBlack;

	private Texture2D tWhite;

	private Color[] cList;

	private FileSender fs;

	private string sLocalMaps;

	private string sServerMaps;

	private string sMapDel;

	private string sMapRequestDel;

	private string sMapSend;

	private void Awake()
	{
		cs = this;
		OnResize();
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		cList = new Color[4];
		cList[0] = new Color(0.1f, 0.1f, 0.1f, 1f);
		cList[1] = new Color(0.2f, 0.2f, 0.2f, 1f);
		cList[2] = new Color(0.3f, 0.3f, 0.3f, 1f);
		cList[3] = new Color(0.1f, 0.2f, 0.4f, 1f);
	}

	private void Start()
	{
		LoadLang();
	}

	private void LoadLang()
	{
		sLocalMaps = Lang.GetString("_LOCAL_MAPS");
		sServerMaps = Lang.GetString("_MAPS_ON_SERVER");
		sMapDel = Lang.GetString("_REMOVE_MAP");
		sMapRequestDel = Lang.GetString("_REQUEST_REMOVE_MAP");
		sMapSend = Lang.GetString("_SEND_TO_SERVER");
	}

	public void OnResize()
	{
		int num = (int)GUIM.YRES(16f);
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(200f), (float)Screen.height / 2f - GUIM.YRES(200f), GUIM.YRES(600f), GUIM.YRES(400f));
		rScrollView = new Rect(rBack.x + (float)num, rBack.y + (float)num + GUIM.YRES(26f), GUIM.YRES(240f), rBack.height - (float)(num * 2) - GUIM.YRES(64f));
		rScrollView_sv = new Rect(rBack.x + (float)num + GUIM.YRES(256f), rBack.y + (float)num + GUIM.YRES(26f), GUIM.YRES(240f), rBack.height - (float)(num * 2) - GUIM.YRES(64f));
		rCol0 = new Rect(rBack.x + GUIM.YRES(16f), rBack.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(26f));
		rCol1 = new Rect(rBack.x + GUIM.YRES(16f) + GUIM.YRES(256f), rBack.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(26f));
		if (maplist != null)
		{
			rScroll = new Rect(0f, 0f, rScrollView.width - GUIM.YRES(20f), (float)maplist.Count * GUIM.YRES(26f));
			if (maplist.Count > 0)
			{
				rList = new Rect[maplist.Count];
				rListText = new Rect[maplist.Count];
				rListContain = new Rect[maplist.Count];
			}
			for (int i = 0; i < maplist.Count; i++)
			{
				rList[i] = new Rect(0f, (float)i * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
				rListText[i] = new Rect(GUIM.YRES(8f), (float)i * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
				rListContain[i] = new Rect(0f, (float)i * GUIM.YRES(26f), GUIM.YRES(220f), GUIM.YRES(26f));
			}
		}
		if (maplist_sv != null)
		{
			rScroll_sv = new Rect(0f, 0f, rScrollView.width - GUIM.YRES(20f), (float)maplist_sv.Count * GUIM.YRES(26f));
			if (maplist_sv.Count > 0)
			{
				rList_sv = new Rect[maplist_sv.Count];
				rListText_sv = new Rect[maplist_sv.Count];
				rListContain_sv = new Rect[maplist_sv.Count];
			}
			for (int j = 0; j < maplist_sv.Count; j++)
			{
				rList_sv[j] = new Rect(0f, (float)j * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
				rListText_sv[j] = new Rect(GUIM.YRES(8f), (float)j * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
				rListContain_sv[j] = new Rect(0f, (float)j * GUIM.YRES(26f), GUIM.YRES(220f), GUIM.YRES(26f));
			}
		}
		rWebUpload = new Rect(rBack.x + (float)num, rBack.y + rBack.height - (float)num - GUIM.YRES(68f), GUIM.YRES(240f), GUIM.YRES(32f));
		rDelete = new Rect(rBack.x + (float)num + GUIM.YRES(256f), rBack.y + rBack.height - (float)num - GUIM.YRES(32f), GUIM.YRES(240f), GUIM.YRES(32f));
		rUpload = new Rect(rBack.x + (float)num, rBack.y + rBack.height - (float)num - GUIM.YRES(32f), GUIM.YRES(240f), GUIM.YRES(32f));
	}

	private void OnGUI()
	{
		if (show)
		{
			GUI.depth = -2;
			GUI.DrawTexture(rBack, tBlack);
			DrawLocalMaps();
			DrawServerMaps();
			DrawWebUpload();
			DrawDeleteButton();
			DrawUploadButton();
		}
	}

	private void DrawLocalMaps()
	{
		if (maplist == null)
		{
			return;
		}
		GUIM.DrawText(rCol0, sLocalMaps, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		sv = GUIM.BeginScrollView(rScrollView, sv, rScroll);
		for (int i = 0; i < maplist.Count; i++)
		{
			GUI.color = ((i % 2 == 0) ? cList[0] : cList[1]);
			if (GUIM.Contains(rListContain[i]))
			{
				GUI.color = cList[2];
			}
			if (setmap == maplist[i])
			{
				GUI.color = cList[3];
			}
			GUI.DrawTexture(rList[i], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rListText[i], maplist[i].title, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
			if (GUIM.HideButton(rList[i]))
			{
				setmap = maplist[i];
			}
		}
		GUIM.EndScrollView();
	}

	private void DrawServerMaps()
	{
		if (maplist_sv == null)
		{
			return;
		}
		GUIM.DrawText(rCol1, sServerMaps, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		sv2 = GUIM.BeginScrollView(rScrollView_sv, sv2, rScroll_sv);
		for (int i = 0; i < maplist_sv.Count; i++)
		{
			GUI.color = ((i % 2 == 0) ? cList[0] : cList[1]);
			if (GUIM.Contains(rListContain_sv[i]))
			{
				GUI.color = cList[2];
			}
			if (delmap == maplist_sv[i])
			{
				GUI.color = cList[3];
			}
			GUI.DrawTexture(rList_sv[i], tWhite);
			GUI.color = Color.white;
			GUIM.DrawText(rListText_sv[i], maplist_sv[i].title, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
			if (GUIM.HideButton(rList_sv[i]))
			{
				delmap = maplist_sv[i];
			}
		}
		GUIM.EndScrollView();
	}

	private void DrawWebUpload()
	{
	}

	private void DrawDeleteButton()
	{
		bool flag = delmap != null;
		if (GUIM.Button(rDelete, flag ? BaseColor.Yellow : BaseColor.Block, sMapDel, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && flag)
		{
			GUIAdmin.cs.Log("> " + sMapRequestDel + " (" + delmap.title + ")");
			Client.cs.send_customop(0, delmap.sv_index, 0);
			delmap = null;
		}
	}

	private void DrawUploadButton()
	{
		if (setmap != null || (fs != null && !fs.inSend))
		{
			if (GUIM.Button(rUpload, BaseColor.Yellow, "ОТПРАВИТЬ НА СЕРВЕР", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				if (fs == null)
				{
					fs = base.gameObject.AddComponent<FileSender>();
				}
				LoadMapFileByte(setmap.sv_index);
				if (mapdata != null)
				{
					fs.Send(0, setmap.title, mapdata, maplen);
				}
			}
		}
		else
		{
			GUIM.Button(rUpload, BaseColor.Block, "ОТПРАВИТЬ НА СЕРВЕР", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		}
	}

	public void LoadMapList()
	{
		if (MainManager.bytemaplist != null)
		{
			if (maplist == null)
			{
				maplist = new List<MapData>();
			}
			maplist.Clear();
			for (int i = 0; i < MainManager.bytemaplist.Count; i++)
			{
				maplist.Add(new MapData("0", "0", "0", "usermap" + i, "usermap" + i, i));
			}
			OnResize();
		}
	}

	private void LoadMapFileByte(int index)
	{
		if (MainManager.bytemaplist != null && index < MainManager.bytemaplist.Count)
		{
			mapdata = MainManager.bytemaplist[index];
			maplen = mapdata.Length;
			if (maplen < 4)
			{
				mapdata = null;
			}
		}
	}

	private void LoadMapFile(string mapname)
	{
		if (maplen < 4)
		{
			mapdata = null;
		}
	}
}
