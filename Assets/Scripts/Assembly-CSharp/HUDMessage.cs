using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class HUDMessage : MonoBehaviour
{
	public class DeathData
	{
		public int vid;

		public int aid;

		public int ateam;

		public int vteam;

		public string aname;

		public string vname;

		public string weaponname;

		public float time;

		public int hs;

		public Texture2D tex;

		public Vector2 v;

		public int ihs;

		public BaseColor ca;

		public BaseColor cv;

		public DeathData(int _aid, int _vid, int _ateam, int _vteam, string _aname, string _vname, string _weaponname, int _hs, Texture2D _tex, Vector2 _v, BaseColor ca, BaseColor cv)
		{
			vid = _vid;
			ateam = _ateam;
			vteam = _vteam;
			aid = _aid;
			aname = _aname;
			vname = _vname;
			weaponname = _weaponname;
			time = Time.time;
			hs = _hs;
			v = _v;
			tex = _tex;
			ihs = UnityEngine.Random.Range(0, 3);
			this.ca = ca;
			this.cv = cv;
		}

		~DeathData()
		{
		}
	}

	public class ChatData
	{
		public BaseColor c;

		public string name;

		public string msg;

		public float time;

		public int teamchat;

		public ChatData(BaseColor c, int teamchat, string name, string msg)
		{
			this.name = name;
			this.msg = msg;
			this.c = c;
			this.teamchat = teamchat;
			time = Time.time;
		}

		~ChatData()
		{
		}
	}

	public class PointData
	{
		public int msgid;

		public string pts;

		public Color c;

		public float time;

		public PointData(int msgid, int pts)
		{
			c = Color.white;
			this.msgid = msgid;
			this.pts = "+" + pts;
			time = Time.time;
		}
	}

	public static bool show = true;

	public static bool showchatedit = false;

	private static string sEdit = "";

	private static int teamchat = 0;

	public static int chatenable = 1;

	public static int chatfilter = 1;

	private static Rect rChat;

	private static Rect rChat2;

	private static Rect rChat3;

	private static Rect rChat4;

	private static Rect rEdit;

	private static Rect rEditText;

	private Texture2D tBlack;

	private Texture2D tGray;

	private Texture2D tYellow;

	private Texture2D tRed;

	private Texture2D tBlue;

	private Texture2D[] tHS = new Texture2D[3];

	private Texture2D[] tChatBack = new Texture2D[3];

	private static Color a = new Color(1f, 1f, 1f, 0.5f);

	private static Color a2 = new Color(1f, 1f, 1f, 0.75f);

	private static TouchScreenKeyboard keyboard;

	private static string[] point_msg = new string[5] { "FRAG", "HEADSHOT", "ASSIST", "CLANFRAG", "INFECTED" };

	public static List<ChatData> chat = new List<ChatData>();

	public static List<DeathData> death = new List<DeathData>();

	public static List<PointData> point = new List<PointData>();

	private static string sToTeam;

	private static string sServer;

	private Rect r;

	private Rect r_a;

	private Rect r_w;

	private Rect r_h;

	private Rect r_v;

	private Rect rp0;

	private Rect rp1;

	private float pdif;

	private float psize;

	private float ppos;

	private static Rect rc;

	private static Rect rc_;

	public void LoadLang()
	{
		sToTeam = Lang.GetString("_TO_TEAM");
		sServer = Lang.GetString("_SERVER");
	}

	public static void SetActive(bool val)
	{
		show = val;
	}

	public static void SetChatEdit(bool val, int t = 0)
	{
		showchatedit = val;
		sEdit = "";
		teamchat = t;
		if (val)
		{
			keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
		}
		if (keyboard != null)
		{
			keyboard.text = "";
		}
	}

	public static void AddDeath(int aid, int vid, int slot, int hs)
	{
		if (PLH.player[aid] == null || PLH.player[vid] == null)
		{
			return;
		}
		PlayerData playerData = PLH.player[aid];
		PlayerData playerData2 = PLH.player[vid];
		if (playerData.wset[playerData.currset] == null || playerData.wset[playerData.currset].w[slot] == null)
		{
			return;
		}
		WeaponInfo wi = playerData.wset[playerData.currset].w[slot].wi;
		if (wi != null)
		{
			LoadWeaponIcon(wi);
			BaseColor ca = ((playerData.team == 0) ? BaseColor.Red : BaseColor.Blue);
			BaseColor cv = ((playerData2.team == 0) ? BaseColor.Red : BaseColor.Blue);
			if (Controll.gamemode == 2 || Controll.gamemode == 8)
			{
				ca = BaseColor.Green;
				cv = BaseColor.Green;
			}
			else if (Controll.gamemode == 5)
			{
				ca = ((playerData.skinstate == 0) ? BaseColor.Yellow : BaseColor.Green);
				cv = ((playerData2.skinstate == 0) ? BaseColor.Yellow : BaseColor.Green);
			}
			DeathData item = new DeathData(aid, vid, PLH.player[aid].team, PLH.player[vid].team, PLH.player[aid].name, PLH.player[vid].name, wi.fullname, hs, wi.tIconDMSG, wi.vIconDMSG, ca, cv);
			death.Add(item);
			if (death.Count > 10)
			{
				death.RemoveAt(0);
			}
			if (DemoRec.isDemo())
			{
				DemoRec.stats_AddDeath(playerData.name, hs);
			}
		}
	}

	public static void AddDeath2(int aid, int vid, int wid, int hs)
	{
		if (PLH.player[aid] == null || PLH.player[vid] == null)
		{
			return;
		}
		PlayerData playerData = PLH.player[aid];
		PlayerData playerData2 = PLH.player[vid];
		WeaponInfo weaponInfo = GUIInv.winfo[wid];
		if (weaponInfo != null)
		{
			LoadWeaponIcon(weaponInfo);
			BaseColor ca = ((playerData.team == 0) ? BaseColor.Red : BaseColor.Blue);
			BaseColor cv = ((playerData2.team == 0) ? BaseColor.Red : BaseColor.Blue);
			if (Controll.gamemode == 2 || Controll.gamemode == 8)
			{
				ca = BaseColor.Green;
				cv = BaseColor.Green;
			}
			else if (Controll.gamemode == 5)
			{
				ca = ((playerData.skinstate == 0) ? BaseColor.Yellow : BaseColor.Green);
				cv = ((playerData2.skinstate == 0) ? BaseColor.Yellow : BaseColor.Green);
			}
			DeathData item = new DeathData(aid, vid, PLH.player[aid].team, PLH.player[vid].team, PLH.player[aid].name, PLH.player[vid].name, weaponInfo.fullname, hs, weaponInfo.tIconDMSG, weaponInfo.vIconDMSG, ca, cv);
			death.Add(item);
			if (death.Count > 10)
			{
				death.RemoveAt(0);
			}
		}
	}

	public static void LoadWeaponIcon(WeaponInfo wi)
	{
		if (!(wi.tIconDMSG != null))
		{
			if (wi.tIconDMSG == null)
			{
				wi.tIconDMSG = ContentLoader_.LoadTexture(wi.name + "_dmsg") as Texture2D;
			}
			if (wi.tIconDMSG == null)
			{
				wi.tIconDMSG = Resources.Load("weapons/" + wi.name + "_dmsg") as Texture2D;
			}
			if (wi.tIconDMSG == null)
			{
				wi.tIconDMSG = ContentLoader_.LoadTexture(wi.name + "_icon") as Texture2D;
			}
			if (wi.tIconDMSG == null)
			{
				wi.tIconDMSG = Resources.Load("weapons/" + wi.name + "_icon") as Texture2D;
			}
			if (wi.tIconDMSG == null)
			{
				wi.tIconDMSG = ContentLoader_.LoadTexture(wi.name) as Texture2D;
			}
			if (wi.tIconDMSG == null)
			{
				wi.tIconDMSG = Resources.Load("weapons/" + wi.name) as Texture2D;
			}
			if (wi.tIconDMSG == null)
			{
				wi.tIconDMSG = TEX.GetTextureByName("red");
			}
			if (wi.vIconDMSG == Vector2.zero)
			{
				wi.vIconDMSG = GUIGameSet.CalcSize(wi.tIconDMSG);
			}
		}
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
		tYellow = TEX.GetTextureByName("yellow");
		tRed = TEX.GetTextureByName("red");
		tGray = TEX.GetTextureByName("gray");
		tBlue = TEX.GetTextureByName("blue");
		tHS[0] = Resources.Load("hs_0") as Texture2D;
		tHS[1] = Resources.Load("hs_1") as Texture2D;
		tHS[2] = Resources.Load("hs_2") as Texture2D;
		tChatBack = new Texture2D[3];
		tChatBack[0] = tRed;
		tChatBack[1] = tBlue;
		tChatBack[2] = tGray;
	}

	private void OnGUI()
	{
		if (show)
		{
			GUI.depth = -1;
			DrawChat();
			DrawChatEdit();
			DrawDeath();
			DrawPoint();
		}
	}

	public static void AddPoint(int msgid, int pts)
	{
		PointData item = new PointData(msgid, pts);
		point.Add(item);
	}

	public static void AddChat(string n, string msg)
	{
		if (chatenable != 0)
		{
			chat.Add(new ChatData(BaseColor.Green, 0, n + " (клан)", msg));
			if (chat.Count > 15)
			{
				chat.RemoveAt(0);
			}
		}
	}

	public static void AddChat(int id, string msg, int teamchat)
	{
		if (id == 255)
		{
			chat.Add(new ChatData(BaseColor.Green, teamchat, sServer, msg));
			if (chat.Count > 15)
			{
				chat.RemoveAt(0);
			}
		}
		else
		{
			if (chatenable == 0 || PLH.player[id] == null)
			{
				return;
			}
			BaseColor c = BaseColor.White;
			if (Controll.gamemode == 2 || Controll.gamemode == 8)
			{
				c = BaseColor.Green;
			}
			else if (Controll.gamemode == 5)
			{
				c = ((PLH.player[id].skinstate != 1) ? BaseColor.Yellow : BaseColor.Green);
			}
			else if (PLH.player[id].team == 0)
			{
				c = BaseColor.Red;
			}
			else if (PLH.player[id].team == 1)
			{
				c = BaseColor.Blue;
			}
			if (chatfilter == 1)
			{
				string text = msg.ToLower();
				text = text.Replace("подстрахуй", "_strah_");
				text = text.Replace("рубля", "_rub_");
				text = text.Replace("хуй", "***");
				text = text.Replace("пизда", "*****");
				text = text.Replace("сука", "с*ка");
				text = text.Replace("пидор", "*****");
				text = text.Replace("бля", "***");
				text = text.Replace("ебал", "****");
				text = text.Replace("ебан", "****");
				text = text.Replace("сучки", "с**ки");
				text = text.Replace("_strah_", "подстрахуй");
				text = text.Replace("_rub_", "рубля");
				char[] array = msg.ToCharArray();
				int i = 0;
				for (int length = msg.Length; i < length; i++)
				{
					if (text[i] == '*')
					{
						array[i] = '*';
					}
				}
				msg = new string(array);
			}
			chat.Add(new ChatData(c, teamchat, PLH.player[id].name, msg));
			if (chat.Count > 15)
			{
				chat.RemoveAt(0);
			}
		}
	}

	private static void DrawChat()
	{
		int num = 0;
		for (int num2 = chat.Count - 1; num2 >= 0; num2--)
		{
			if (showchatedit)
			{
				if (Time.time > chat[num2].time + 8f)
				{
					GUI.color = a;
				}
				else
				{
					GUI.color = Color.white;
				}
			}
			else if (Time.time > chat[num2].time + 8f)
			{
				continue;
			}
			int index = num2;
			rChat.Set(GUIM.YRES(16f), GUIM.YRES(460f) - GUIM.YRES(20f) * (float)num, GUIM.YRES(400f), GUIM.YRES(20f));
			if (chat[index].teamchat == 0)
			{
				Vector2 vector = GUIM.CalcSize(chat[index].name, 1, 14);
				GUIM.DrawText(rChat, chat[index].name, TextAnchor.MiddleLeft, chat[index].c, 1, 14, true);
				rChat2.Set(rChat.x + vector.x, rChat.y, rChat.width, rChat.height);
				GUIM.DrawText(rChat2, " : " + chat[index].msg, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, true);
			}
			else
			{
				Vector2 vector2 = GUIM.CalcSize(sToTeam + " ", 1, 14);
				Vector2 vector = GUIM.CalcSize(chat[index].name, 1, 14);
				GUIM.DrawText(rChat, sToTeam + " ", TextAnchor.MiddleLeft, BaseColor.Yellow, 1, 14, true);
				rChat3.Set(rChat.x + vector2.x, rChat.y, rChat.width, rChat.height);
				rChat4.Set(rChat.x + vector2.x + vector.x, rChat.y, rChat.width, rChat.height);
				GUIM.DrawText(rChat3, chat[index].name, TextAnchor.MiddleLeft, chat[index].c, 1, 14, true);
				GUIM.DrawText(rChat4, " : " + chat[index].msg, TextAnchor.MiddleLeft, BaseColor.White, 1, 14, true);
			}
			num++;
		}
		GUI.color = Color.white;
	}

	private void DrawChatEdit()
	{
		if (showchatedit && keyboard != null && !keyboard.active && (keyboard.done || keyboard.wasCanceled))
		{
			sEdit = keyboard.text;
			ValidateAndSend();
			SetChatEdit(false);
		}
	}

	private static void ValidateAndSend()
	{
		if (sEdit.Length > 0)
		{
			sEdit = string.Join(" ", sEdit.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries));
			sEdit = sEdit.Replace("<", "");
			sEdit = sEdit.Replace(">", "");
			sEdit = sEdit.Replace(":", "");
			sEdit = sEdit.Replace("/", "");
			sEdit = sEdit.Replace(".", "");
			if (sEdit.Length > 127)
			{
				sEdit = sEdit.Substring(0, 127);
			}
			if (teamchat == 2)
			{
				MasterClient.cs.send_clanchatsend(sEdit);
			}
			else
			{
				Client.cs.send_chatmsg(teamchat, sEdit);
			}
		}
	}

	private void DrawDeath()
	{
		if (death.Count == 0)
		{
			return;
		}
		for (int i = 0; i < death.Count; i++)
		{
			if (!(Time.time < death[i].time + 5f))
			{
				death.RemoveAt(i);
				break;
			}
		}
		float num = Screen.width;
		for (int num2 = death.Count - 1; num2 >= 0; num2--)
		{
			r.Set(num - GUIM.YRES(16f), GUIM.YRES(GUIGameMenu.show ? 80 : 16) + (float)num2 * GUIM.YRES(20f), GUIM.YRES(200f), GUIM.YRES(20f));
			float x = GUIM.CalcSize(death[num2].aname, 1, 14).x;
			float num3 = GUIM.YRES(72f);
			float num4 = 0f;
			if (death[num2].hs == 1)
			{
				num4 = GUIM.YRES(32f);
			}
			float x2 = GUIM.CalcSize(death[num2].vname, 1, 14).x;
			r_a.Set(r.x - (x + x2 + num3 + num4), r.y, x, r.height);
			r_w.Set(r.x - (x2 + num3 + num4), r.y, num3, r.height);
			r_h.Set(r.x - (x2 + num4), r.y - GUIM.YRES(8f), GUIM.YRES(32f), GUIM.YRES(32f));
			r_v.Set(r.x - x2, r.y, x2, r.height);
			GUIM.DrawText(r_a, death[num2].aname, TextAnchor.MiddleLeft, death[num2].ca, 1, 14, true);
			DrawWeaponIcon(r_w, death[num2].tex, death[num2].v);
			if (death[num2].hs == 1)
			{
				GUI.DrawTexture(r_h, tHS[death[num2].ihs]);
			}
			GUIM.DrawText(r_v, death[num2].vname, TextAnchor.MiddleLeft, death[num2].cv, 1, 14, true);
			if (death[num2].aid == Controll.pl.idx)
			{
				r.Set(num - GUIM.YRES(2f), r.y, GUIM.YRES(2f), r.height);
				GUI.DrawTexture(r, tYellow);
			}
			else if (death[num2].vid == Controll.pl.idx)
			{
				r.Set(num - GUIM.YRES(2f), r.y, GUIM.YRES(2f), r.height);
				GUI.DrawTexture(r, tRed);
			}
		}
	}

	private void DrawPoint()
	{
		if (point.Count == 0)
		{
			return;
		}
		for (int i = 0; i < point.Count; i++)
		{
			if (!(Time.time < point[i].time + 5f))
			{
				point.RemoveAt(i);
				break;
			}
		}
		ppos = 0f;
		for (int num = point.Count - 1; num >= 0; num--)
		{
			rp0.Set((float)Screen.width / 2f - GUIM.YRES(55f), GUIM.YRES(400f) + ppos * GUIM.YRES(20f), GUIM.YRES(50f), GUIM.YRES(20f));
			rp1.Set((float)Screen.width / 2f + GUIM.YRES(15f), GUIM.YRES(400f) + ppos * GUIM.YRES(20f), GUIM.YRES(120f), GUIM.YRES(20f));
			pdif = Time.time - point[num].time;
			if (pdif > 0.1f)
			{
				pdif = 0.1f;
			}
			psize = (0.1f - pdif) * 20f;
			pdif = Time.time - point[num].time;
			if (pdif > 4f)
			{
				point[num].c.a = 5f - pdif;
			}
			GUI.color = point[num].c;
			GUIM.DrawText(rp0, point_msg[point[num].msgid], TextAnchor.MiddleRight, BaseColor.White, 1, (int)(14f * psize), true);
			GUIM.DrawText(rp1, point[num].pts, TextAnchor.MiddleLeft, BaseColor.Yellow, 1, (int)(14f * psize), true);
			ppos += 1f;
		}
		GUI.color = Color.white;
	}

	public static void DrawWeaponIcon(Rect r, Texture2D tex, Vector2 tc)
	{
		if (!(tex == null))
		{
			float num = GUIM.YRES(8f);
			float num2 = tc.x / tc.y;
			float num3 = r.width - num;
			float height = r.height;
			if (num3 / num2 > height)
			{
				num3 = height * num2;
			}
			float num4 = (r.width - num3) / 2f;
			float num5 = (r.height - height) / 2f;
			float num6 = tc.y / tc.x * num3;
			float num7 = (height - num6) / 2f;
			rc.Set(0f, 1f - tc.y / (float)tex.height, tc.x / (float)tex.width, tc.y / (float)tex.height);
			rc_.Set(r.x + num4, r.y + num5 + num7, num3, num6);
			GUI.DrawTextureWithTexCoords(rc_, tex, rc);
		}
	}
}
