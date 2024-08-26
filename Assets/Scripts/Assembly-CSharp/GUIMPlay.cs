using System.Collections.Generic;
using GameClass;
using UnityEngine;

public class GUIMPlay : MonoBehaviour
{
	public static GUIMPlay cs = null;

	public static bool show = false;

	private Texture2D tBlack;

	private Texture2D tGray;

	private Texture2D tOrange;

	private Rect rBack;

	private Rect rScroll;

	private Rect rScrollZone;

	private Rect[] rHeader;

	private Rect rConnect;

	private Rect rConnectButton;

	private Vector2 scroll = Vector2.zero;

	private static ServerData currServer = null;

	private string sGameMode = "КОМАНДНЫЙ БОЙ";

	private float movex;

	public static List<ServerData> srvlist = new List<ServerData>();

	public static void SetActive(bool val)
	{
		show = val;
		currServer = null;
		if (val)
		{
			MasterClient.cs.send_list(0);
		}
	}

	private void Awake()
	{
		cs = this;
		OnResize();
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tGray = TEX.GetTextureByName("gray");
		tOrange = TEX.GetTextureByName("orange");
	}

	public void OnResize()
	{
		int num = (int)GUIM.YRES(64f);
		int num2 = (int)GUIM.YRES(2f);
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(300f), GUIM.YRES(102f), GUIM.YRES(600f), GUIM.YRES(450f));
		rScroll = new Rect(rBack.x + GUIM.YRES(4f), rBack.y + GUIM.YRES(48f), rBack.width - GUIM.YRES(8f), rBack.height - GUIM.YRES(64f));
		rScrollZone = new Rect(0f, 0f, 0f, srvlist.Count * (num + num2));
		if (rHeader == null)
		{
			rHeader = new Rect[8];
		}
		rHeader[0] = new Rect(rBack.x + GUIM.YRES(4f), rBack.y + GUIM.YRES(4f), GUIM.YRES(60f), GUIM.YRES(48f));
		rHeader[1] = rHeader[0];
		rHeader[1].x += rHeader[0].width + (float)num2;
		rHeader[1].width = GUIM.YRES(132f);
		rHeader[2] = rHeader[1];
		rHeader[2].x += rHeader[1].width + (float)num2;
		rHeader[2].width = GUIM.YRES(132f);
		rHeader[3] = rHeader[2];
		rHeader[3].x += rHeader[2].width + (float)num2;
		rHeader[3].width = GUIM.YRES(132f);
		rHeader[4] = rHeader[3];
		rHeader[4].x += rHeader[3].width + (float)num2;
		rHeader[4].width = GUIM.YRES(132f);
		for (int i = 0; i < srvlist.Count; i++)
		{
			ServerData serverData = srvlist[i];
			if (serverData.r == null)
			{
				serverData.r = new Rect[8];
			}
			serverData.r[0] = new Rect(0f, (num + num2) * i, rBack.width, num);
			serverData.r[1] = new Rect(0f, serverData.r[0].y, rHeader[0].width, num);
			serverData.r[2] = new Rect(rHeader[0].width, serverData.r[0].y, rHeader[1].width, num);
			serverData.r[3] = new Rect(serverData.r[2].x + rHeader[1].width, serverData.r[0].y, rHeader[2].width, num);
			serverData.r[4] = new Rect(serverData.r[3].x + rHeader[2].width, serverData.r[0].y, rHeader[3].width, num);
			serverData.r[5] = new Rect(serverData.r[4].x + rHeader[3].width, serverData.r[0].y, rHeader[4].width, num);
			serverData.sPOS = (serverData.idx + 1).ToString();
			serverData.sSERVER = "СЕРВЕР #" + (serverData.idx + 1);
		}
		_OnResize_connect();
	}

	private void _OnResize_connect()
	{
		int num = (int)GUIM.YRES(64f);
		int num2 = (int)GUIM.YRES(2f);
		rConnect = new Rect((float)Screen.width - GUIM.YRES(232f) + movex, (float)Screen.height - GUIM.YRES(260f), GUIM.YRES(200f), num + num2 * 2);
		rConnectButton = new Rect(rConnect.x + (float)num2, rConnect.y + (float)num2, rConnect.width - (float)(num2 * 2), num);
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIM.DrawBox(rBack, tBlack, 0.1f);
			DrawHeader();
			DrawServers();
			DrawConnect();
		}
	}

	private void DrawHeader()
	{
		GUI.DrawTexture(rHeader[0], tGray);
		GUI.DrawTexture(rHeader[1], tGray);
		GUI.DrawTexture(rHeader[2], tGray);
		GUI.DrawTexture(rHeader[3], tGray);
		GUI.DrawTexture(rHeader[4], tGray);
		GUIM.DrawText(rHeader[0], "#", TextAnchor.MiddleCenter, BaseColor.White, 0, 24, false);
		GUIM.DrawText(rHeader[1], "СЕРВЕР", TextAnchor.MiddleCenter, BaseColor.White, 0, 24, false);
		GUIM.DrawText(rHeader[2], "РЕЖИМ", TextAnchor.MiddleCenter, BaseColor.White, 0, 24, false);
		GUIM.DrawText(rHeader[3], "ИГРОКИ", TextAnchor.MiddleCenter, BaseColor.White, 0, 24, false);
		GUIM.DrawText(rHeader[4], "УРОВЕНЬ", TextAnchor.MiddleCenter, BaseColor.White, 0, 24, false);
	}

	private void DrawServers()
	{
		scroll = GUIM.BeginScrollView(rScroll, scroll, rScrollZone);
		for (int i = 0; i < srvlist.Count; i++)
		{
			DrawServer(srvlist[i]);
		}
		GUIM.EndScrollView();
	}

	private void DrawServer(ServerData s)
	{
		bool flag = currServer == s;
		GUI.DrawTexture(s.r[0], flag ? tOrange : tBlack);
		GUIM.DrawText(s.r[1], s.sPOS, TextAnchor.MiddleCenter, BaseColor.White, 0, 24, flag ? true : false);
		GUIM.DrawText(s.r[2], s.sSERVER, TextAnchor.MiddleCenter, BaseColor.White, 0, 24, flag ? true : false);
		GUIM.DrawText(s.r[3], sGameMode, TextAnchor.MiddleCenter, BaseColor.White, 0, 24, flag ? true : false);
		GUIM.DrawText(s.r[4], s.formatplayers, TextAnchor.MiddleCenter, BaseColor.White, 0, 24, flag ? true : false);
		GUIM.DrawText(s.r[5], s.slevel, TextAnchor.MiddleCenter, BaseColor.White, 0, 24, flag ? true : false);
		if (GUIM.HideButton(s.r[0]))
		{
			currServer = s;
			movex = GUIM.YRES(240f);
		}
	}

	private void DrawConnect()
	{
		if (currServer == null)
		{
			return;
		}
		if (movex > 0f)
		{
			movex -= GUIM.YRES(64f) * Time.deltaTime * 15f;
			if (movex < 0f)
			{
				movex = 0f;
			}
			_OnResize_connect();
		}
		GUIM.DrawBox(rConnect, tBlack);
		GUI.DrawTexture(rConnectButton, tOrange);
		GUIM.DrawText(rConnectButton, "ПОДКЛЮЧИТЬСЯ", TextAnchor.MiddleCenter, BaseColor.White, 0, 32, true);
		if (GUIM.HideButton(rConnect))
		{
			GUIInv.CheckNullSet();
			Client.IP = currServer.ip;
			Client.PORT = currServer.port;
			Client.cs.Connect();
		}
	}
}
