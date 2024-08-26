using Player;
using UnityEngine;

public class GUIAdminPlayers : MonoBehaviour
{
	public static GUIAdminPlayers cs;

	public bool show;

	private Texture2D tBlack;

	private Texture2D tWhite;

	private Color[] cList;

	private Rect rBack;

	private Rect rCol0;

	private Rect rScrollView;

	private Rect rScroll;

	private Rect[] rList;

	private Rect[] rListText;

	private Rect[] rListContain;

	private Rect rKick;

	private Rect rMute;

	private Rect rBuild;

	private Rect rBan;

	private Vector2 sv = Vector2.zero;

	private PlayerData currplayer;

	private string sPlayers;

	private string sKick;

	private string sChat;

	private string sBuild;

	private string sRequestKick;

	private string sRequestChat;

	private string sRequestBuild;

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
		sPlayers = Lang.GetString("_PLAYERS");
		sKick = Lang.GetString("_KICK_FROM_SERVER");
		sChat = Lang.GetString("_PERMISSION_CHAT");
		sBuild = Lang.GetString("_PERMISSION_BUILDING");
		sRequestKick = Lang.GetString("_REQUEST_FOR_KICK_PLAYER");
		sRequestChat = Lang.GetString("_REQUEST_SILENCE_PLAYER");
		sRequestBuild = Lang.GetString("_REQUEST_BUILD_FOR_PLAYER");
	}

	public void OnResize()
	{
		int num = (int)GUIM.YRES(16f);
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(200f), (float)Screen.height / 2f - GUIM.YRES(200f), GUIM.YRES(600f), GUIM.YRES(400f));
		rCol0 = new Rect(rBack.x + GUIM.YRES(16f), rBack.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(26f));
		rScrollView = new Rect(rBack.x + (float)num, rBack.y + (float)num + GUIM.YRES(26f), GUIM.YRES(240f), rBack.height - (float)(num * 2) - GUIM.YRES(64f));
		rScroll = new Rect(0f, 0f, rScrollView.width - GUIM.YRES(20f), 40f * GUIM.YRES(26f));
		rList = new Rect[40];
		rListText = new Rect[40];
		rListContain = new Rect[40];
		for (int i = 0; i < 40; i++)
		{
			rList[i] = new Rect(0f, (float)i * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
			rListText[i] = new Rect(GUIM.YRES(8f), (float)i * GUIM.YRES(26f), GUIM.YRES(240f), GUIM.YRES(26f));
			rListContain[i] = new Rect(0f, (float)i * GUIM.YRES(26f), GUIM.YRES(220f), GUIM.YRES(26f));
		}
		rKick = new Rect(rBack.x + (float)num + GUIM.YRES(256f), rBack.y + rBack.height - (float)num - GUIM.YRES(32f), GUIM.YRES(240f), GUIM.YRES(32f));
		rMute = new Rect(rKick.x, rKick.y - GUIM.YRES(48f), rKick.width, rKick.height);
		rBuild = new Rect(rKick.x, rKick.y - GUIM.YRES(84f), rKick.width, rKick.height);
	}

	private void OnGUI()
	{
		if (show)
		{
			GUI.depth = -2;
			GUI.DrawTexture(rBack, tBlack);
			DrawPlayers();
			DrawPlayerMenu();
		}
	}

	private void DrawPlayers()
	{
		GUIM.DrawText(rCol0, sPlayers, TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		sv = GUIM.BeginScrollView(rScrollView, sv, rScroll);
		int num = 0;
		for (int i = 0; i < 40; i++)
		{
			if (PLH.player[i] != null)
			{
				PlayerData playerData = PLH.player[i];
				GUI.color = ((i % 2 == 0) ? cList[0] : cList[1]);
				if (GUIM.Contains(rListContain[num]))
				{
					GUI.color = cList[2];
				}
				if (currplayer == playerData)
				{
					GUI.color = cList[3];
				}
				GUI.DrawTexture(rList[num], tWhite);
				GUI.color = Color.white;
				GUIM.DrawText(rListText[num], playerData.formatname, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
				if (GUIM.HideButton(rList[num]))
				{
					currplayer = playerData;
				}
				num++;
			}
		}
		GUIM.EndScrollView();
	}

	private void DrawPlayerMenu()
	{
		bool flag = currplayer != null;
		if (GUIM.Button(rKick, flag ? BaseColor.Yellow : BaseColor.Block, sKick, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && flag)
		{
			Client.cs.send_customop(7, currplayer.idx, 0);
			GUIAdmin.cs.Log("> " + sRequestKick + " (" + currplayer.name + ")");
			currplayer = null;
		}
		if (GUIM.Button(rMute, flag ? BaseColor.Yellow : BaseColor.Block, sChat, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && flag)
		{
			Client.cs.send_customop(9, currplayer.idx, 0);
			GUIAdmin.cs.Log("> " + sRequestChat + " (" + currplayer.name + ")");
			currplayer = null;
		}
		if (GUIM.Button(rBuild, flag ? BaseColor.Yellow : BaseColor.Block, sBuild, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && flag)
		{
			Client.cs.send_customop(10, currplayer.idx, 0);
			GUIAdmin.cs.Log("> " + sRequestBuild + " (" + currplayer.name + ")");
			currplayer = null;
		}
	}
}
