using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using GameClass;
using Player;
using UnityEngine;

public class MasterClient : MonoBehaviour
{
	public class RecvData
	{
		public byte[] Buffer;

		public int Len;

		public RecvData(byte[] _buffer, int _len)
		{
			Buffer = new byte[_len];
			for (int i = 0; i < _len; i++)
			{
				Buffer[i] = _buffer[i];
			}
			Len = _len;
		}

		~RecvData()
		{
			Buffer = null;
		}
	}

	public static MasterClient cs = null;

	public static bool Lock = false;

	public static int Rate = 30000;

	public static string Map = "";

	public static bool InConnect = false;

	public static string vk_reserve = "NULL";

	public static string vk_id = "guest";

	public static string vk_key = "VK_KEY";

	public static string IP = "82.202.219.194";

	public static int PORT = 7778;

	public static string acckey = "-";

	public static int POLICYPORT = 843;

	public static string PASSWORD = "";

	public static bool Loaded = false;

	private bool Active;

	private TcpClient client;

	public byte[] sendbuffer;

	private static bool forcedisconnect = false;

	public static bool proxy_change = false;

	private List<RecvData> Tlist = new List<RecvData>();

	private static byte[] buffer = null;

	private static int len = 0;

	private int concount;

	private float sendtimer;

	private byte[] readBuffer = new byte[102400];

	private int SplitRead;

	private int BytesRead;

	private string _bl = "blockpost";

	private string _data = "_Data";

	private string _managed = "Managed";

	private string _ascs0 = "Assembly";

	private string _ascs1 = "-CSharp";

	private string _ascs2 = ".d";

	private string _ascs3 = "l";

	public static void Init()
	{
		forcedisconnect = false;
	}

	public void PostAwake()
	{
		cs = this;
		sendbuffer = new byte[2050];
		Init();
		Active = false;
		lock (Tlist)
		{
			Tlist.Clear();
		}
	}

	private IEnumerator StartReconnect()
	{
		yield return new WaitForSeconds(2f);
		Connect();
	}

	public void Connect()
	{
		if (client != null && client.Connected)
		{
			return;
		}
		try
		{
			client = new TcpClient();
			client.NoDelay = true;
			Log.Add("MASTER: " + IP + ":" + PORT);
			IAsyncResult asyncResult = client.BeginConnect(IP, PORT, null, null);
			asyncResult.AsyncWaitHandle.WaitOne(2000);
			if (!client.Connected)
			{
				Active = false;
				MainManager.error = true;
				MainManager.error_msg = Lang.GetString("_NOT_CONNECT_TIMEOUT");
			}
			else
			{
				client.EndConnect(asyncResult);
				client.GetStream().BeginRead(readBuffer, 0, Rate, DoRead, null);
				Active = true;
				Log.Add("Master connected to server " + IP + " " + PORT);
			}
		}
		catch
		{
			Log.Add("Master connection error (server is not active)");
			Active = false;
			MainManager.error = true;
			MainManager.error_msg = Lang.GetString("_NOT_CONNECT_TIMEOUT");
		}
		Lock = false;
		if (!Active)
		{
			if (concount < 3)
			{
				MainManager.error = true;
				MainManager.error_msg = Lang.GetString("_CONNECTION_ATTEMPT") + " " + (concount + 1);
				StartCoroutine(StartReconnect());
				concount++;
				return;
			}
			if (MainManager.mycom && !proxy_change)
			{
				proxy_change = true;
				StartProxy();
			}
			if (MainManager.steam && !proxy_change)
			{
				proxy_change = true;
				StartProxySteam();
			}
			return;
		}
		MainManager.error = false;
		if (GP.auth)
		{
			if (GP.force)
			{
				GP.force = false;
				send_auth_vk();
			}
			else
			{
				send_authgoogle2();
			}
		}
		else
		{
			send_auth_vk();
		}
	}

	private void StartProxy()
	{
		IP = MainConfig.MYCOM_IP_PROXY;
		PORT = MainConfig.MYCOM_PORT_PROXY;
		concount = 0;
		MainManager.error = true;
		MainManager.error_msg = Lang.GetString("_CONNECTION_ATTEMPT") + " " + (concount + 1);
		StartCoroutine(StartReconnect());
		concount++;
	}

	private void StartProxySteam()
	{
		IP = MainConfig.STEAM_IP_PROXY;
		PORT = MainConfig.STEAM_PORT_PROXY;
		concount = 0;
		MainManager.error = true;
		MainManager.error_msg = Lang.GetString("_CONNECTION_ATTEMPT") + " " + (concount + 1);
		StartCoroutine(StartReconnect());
		concount++;
	}

	private void Update()
	{
		lock (Tlist)
		{
			for (int i = 0; i < Tlist.Count; i++)
			{
				buffer = Tlist[i].Buffer;
				len = Tlist[i].Len;
				ProcessData();
			}
			Tlist.Clear();
		}
		if (forcedisconnect)
		{
			forcedisconnect = false;
		}
		float time = Time.time;
		if (sendtimer == 0f || time > sendtimer)
		{
			sendtimer = time + 60f;
			send_ping();
		}
	}

	private void OnApplicationQuit()
	{
		if (client != null)
		{
			client.Close();
		}
	}

	public void CloseClient()
	{
		if (client != null)
		{
			client.Close();
			client = null;
		}
	}

	private void DoRead(IAsyncResult ar)
	{
		try
		{
			BytesRead = client.GetStream().EndRead(ar);
			if (BytesRead < 1)
			{
				forcedisconnect = true;
				return;
			}
			SplitRead += BytesRead;
			while (SplitRead >= 4)
			{
				int num = NET.DecodeShort(readBuffer, 2);
				if (SplitRead < num)
				{
					break;
				}
				lock (Tlist)
				{
					Tlist.Add(new RecvData(readBuffer, num));
				}
				int num2 = 0;
				for (int i = num; i < SplitRead; i++)
				{
					readBuffer[num2] = readBuffer[i];
					num2++;
				}
				SplitRead -= num;
			}
			client.GetStream().BeginRead(readBuffer, SplitRead, Rate, DoRead, null);
		}
		catch (Exception)
		{
		}
	}

	public void AddMsg(byte[] buffer, int len)
	{
		lock (Tlist)
		{
			Tlist.Add(new RecvData(buffer, len));
		}
	}

	private void ProcessData()
	{
		if (len >= 2 && buffer[0] == 245)
		{
			switch (buffer[1])
			{
			case 0:
				recv_auth();
				break;
			case 1:
				recv_status();
				break;
			case 2:
				recv_play();
				break;
			case 3:
				recv_list();
				break;
			case 5:
				recv_customplay();
				break;
			case 6:
				recv_acq();
				break;
			case 10:
				recv_drop();
				break;
			case 30:
				recv_weaponfrags();
				break;
			case 41:
				recv_set();
				break;
			case 43:
				recv_stats();
				break;
			case 44:
				recv_options();
				break;
			case 46:
				recv_character();
				break;
			case 48:
				recv_name();
				break;
			case 49:
				recv_profile();
				break;
			case 50:
				recv_inv();
				break;
			case 51:
				recv_weaponinfo();
				break;
			case 53:
				recv_caseinfo();
				break;
			case 54:
				recv_keyinfo();
				break;
			case 55:
				recv_buy();
				break;
			case 56:
				recv_caseopen();
				break;
			case 57:
				recv_casecraft();
				break;
			case 58:
				recv_questlist();
				break;
			case 59:
				recv_questreward();
				break;
			case 60:
				recv_topplayer();
				break;
			case 62:
				recv_questload();
				break;
			case 64:
				recv_shop();
				break;
			case 65:
				recv_casesell();
				break;
			case 66:
				recv_casesellinfo();
				break;
			case 67:
				recv_bonus();
				break;
			case 68:
				recv_clanbase();
				break;
			case 69:
				recv_clanrestore();
				break;
			case 70:
				recv_clancreate();
				break;
			case 71:
				recv_clanlist();
				break;
			case 72:
				recv_clanfind();
				break;
			case 73:
				recv_clanplayer();
				break;
			case 75:
				recv_clanmessage();
				break;
			case 80:
				recv_clanadminop();
				break;
			case 82:
				recv_clanstats();
				break;
			case 83:
				recv_clanplayerstats();
				break;
			case 84:
				recv_clanchatlist();
				break;
			case 85:
				recv_clanchatsend();
				break;
			case 86:
				recv_topclan();
				break;
			case 87:
				recv_profile2();
				break;
			case 88:
				recv_playerskininfo();
				break;
			case 90:
				recv_mkey();
				break;
			case 111:
				recv_rcon();
				break;
			case 112:
				recv_error();
				break;
			case 210:
				recv_auth_google();
				break;
			case 211:
				recv_purchase();
				break;
			case 212:
				recv_orderlist();
				break;
			case 200:
				recv_auth_steam();
				break;
			case 202:
				recv_microtxnstate();
				break;
			case 220:
				recv_auth_idc();
				break;
			case 230:
				recv_auth_mycom();
				break;
			case 231:
				recv_payerequest_mycom();
				break;
			case 240:
				recv_nettest();
				break;
			}
		}
	}

	private void recv_auth()
	{
		NET.BEGIN_READ(buffer, len, 4);
		NET.READ_BYTE();
		MainManager.authed = true;
	}

	private void recv_status()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_LONG();
		Main.sRightMsg = Main.sOnline + num;
		if (MainManager.idc || MainManager.mycom)
		{
			Main.sRightMsg = "";
		}
	}

	private void recv_play()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int value = NET.READ_LONG();
		int num = NET.READ_SHORT();
		string iP = new IPAddress(BitConverter.GetBytes(value)).ToString();
		short num2 = IPAddress.HostToNetworkOrder((short)num);
		Client.IP = iP;
		Client.PORT = (ushort)num2;
	}

	private void recv_list()
	{
		NET.BEGIN_READ(buffer, len, 4);
		if (NET.READ_BYTE() == 1)
		{
			GUIPlay.srvlist.Clear();
		}
		int num = (len - 5) / 11;
		for (int i = 0; i < num; i++)
		{
			int value = NET.READ_LONG();
			int num2 = NET.READ_SHORT();
			int players = NET.READ_BYTE();
			int gamemode = NET.READ_BYTE();
			int status = NET.READ_BYTE();
			int level = NET.READ_BYTE();
			int maxplayers = NET.READ_BYTE();
			int privategame = 0;
			string ip = new IPAddress(BitConverter.GetBytes(value)).ToString();
			int num3 = IPAddress.HostToNetworkOrder((short)num2);
			GUIPlay.srvlist.Add(new ServerData(ip, (ushort)num3, players, gamemode, status, privategame, level, maxplayers));
		}
		if (GUIPlay.quickplay)
		{
			GUIPlay.quickplay = false;
			GUIPlay.QuickConnect();
		}
	}

	private void recv_customplay()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int value = NET.READ_LONG();
		int num2 = NET.READ_SHORT();
		if (num > 0)
		{
			string iP = new IPAddress(BitConverter.GetBytes(value)).ToString();
			short num3 = IPAddress.HostToNetworkOrder((short)num2);
			Client.IP = iP;
			Client.PORT = (ushort)num3;
			Client.cs.Connect();
		}
	}

	private void recv_acq()
	{
		NET.BEGIN_READ(buffer, len, 4);
		NET.READ_BYTE();
		NET.READ_STRING();
	}

	private void recv_drop()
	{
		cs.CloseClient();
	}

	private void recv_weaponfrags()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_LONG();
		int frags = NET.READ_LONG();
		GUIInv.UpdateWeaponFrags((ulong)num, frags);
	}

	private void recv_set()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_LONG();
		int num2 = NET.READ_LONG();
		int value = NET.READ_LONG();
		int value2 = NET.READ_LONG();
		int value3 = NET.READ_LONG();
		int num3 = NET.READ_LONG();
		int num4 = NET.READ_LONG();
		int value4 = NET.READ_LONG();
		int value5 = NET.READ_LONG();
		int value6 = NET.READ_LONG();
		int num5 = NET.READ_LONG();
		int num6 = NET.READ_LONG();
		int value7 = NET.READ_LONG();
		int value8 = NET.READ_LONG();
		int value9 = NET.READ_LONG();
		int value10 = NET.READ_BYTE();
		int value11 = NET.READ_BYTE();
		int value12 = NET.READ_BYTE();
		int value13 = NET.READ_BYTE();
		int value14 = NET.READ_BYTE();
		int value15 = NET.READ_BYTE();
		int value16 = NET.READ_BYTE();
		int value17 = NET.READ_BYTE();
		int value18 = NET.READ_BYTE();
		int value19 = NET.READ_BYTE();
		int value20 = NET.READ_BYTE();
		int value21 = NET.READ_BYTE();
		PlayerPrefs.SetInt("bp_set", 1);
		PlayerPrefs.SetInt("set_0_slot_0_uid", num);
		PlayerPrefs.SetInt("set_0_slot_1_uid", num2);
		PlayerPrefs.SetInt("set_0_slot_2_uid", value);
		PlayerPrefs.SetInt("set_0_slot_3_uid", value2);
		PlayerPrefs.SetInt("set_0_slot_4_uid", value3);
		PlayerPrefs.SetInt("set_1_slot_0_uid", num3);
		PlayerPrefs.SetInt("set_1_slot_1_uid", num4);
		PlayerPrefs.SetInt("set_1_slot_2_uid", value4);
		PlayerPrefs.SetInt("set_1_slot_3_uid", value5);
		PlayerPrefs.SetInt("set_1_slot_4_uid", value6);
		PlayerPrefs.SetInt("set_2_slot_0_uid", num5);
		PlayerPrefs.SetInt("set_2_slot_1_uid", num6);
		PlayerPrefs.SetInt("set_2_slot_2_uid", value7);
		PlayerPrefs.SetInt("set_2_slot_3_uid", value8);
		PlayerPrefs.SetInt("set_2_slot_4_uid", value9);
		PlayerPrefs.SetInt("wi_" + num + "_scopeid", value10);
		PlayerPrefs.SetInt("wi_" + num2 + "_scopeid", value11);
		PlayerPrefs.SetInt("wi_" + num3 + "_scopeid", value12);
		PlayerPrefs.SetInt("wi_" + num4 + "_scopeid", value13);
		PlayerPrefs.SetInt("wi_" + num5 + "_scopeid", value14);
		PlayerPrefs.SetInt("wi_" + num6 + "_scopeid", value15);
		PlayerPrefs.SetInt("wi_" + num + "_silid", value16);
		PlayerPrefs.SetInt("wi_" + num2 + "_silid", value17);
		PlayerPrefs.SetInt("wi_" + num3 + "_silid", value18);
		PlayerPrefs.SetInt("wi_" + num4 + "_silid", value19);
		PlayerPrefs.SetInt("wi_" + num5 + "_silid", value20);
		PlayerPrefs.SetInt("wi_" + num6 + "_silid", value21);
		GUIInv.LoadSet();
		for (int i = 0; i < GUIInv.wlist.Count; i++)
		{
			WeaponInv weaponInv = GUIInv.wlist[i];
			weaponInv.scopeid = PlayerPrefs.GetInt("wi_" + weaponInv.uid + "_scopeid", 0);
		}
	}

	private void recv_stats()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_LONG();
		int num2 = NET.READ_LONG();
		int num3 = NET.READ_LONG();
		int num4 = NET.READ_LONG();
		int num5 = NET.READ_LONG();
		string hash_stats = NET.READ_STRING();
		int level = Main.CalcLevel(num);
		int num6 = Main.CalcExp(level);
		int num7 = Main.CalcNextExp(level);
		GUIOptions.exp = num;
		GUIOptions.level = level;
		GUIProfile.sLevel = GUIProfile.sLeveltext + " " + level;
		GUIProfile.sExp = num.ToString();
		GUIProfile.fLevelProgress = (float)(num - num6) / (float)(num7 - num6);
		GUIProfile.sLevelProgress = (GUIProfile.fLevelProgress * 100f).ToString("0.00") + "%";
		GUIProfile.sExpProgress = num + "/" + num7;
		GUIProfile.sF = num2.ToString();
		GUIProfile.sD = num3.ToString();
		GUIProfile.sA = num4.ToString();
		GUIProfile.sH = num5.ToString();
		GUIProfile.hash_stats = hash_stats;
		if (num3 != 0)
		{
			GUIProfile.sFD = ((float)num2 / (float)num3).ToString("0.00");
		}
	}

	private void recv_options()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int value = NET.READ_BYTE();
		int value2 = NET.READ_BYTE();
		int value3 = NET.READ_BYTE();
		int value4 = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		int num3 = NET.READ_BYTE();
		int num4 = NET.READ_BYTE();
		PlayerPrefs.SetInt("bp_options", 1);
		if (num != 255)
		{
			PlayerPrefs.SetInt("crosshair_type", num);
			PlayerPrefs.SetInt("crosshair_color", value);
			PlayerPrefs.SetInt("health_position", value2);
			PlayerPrefs.SetInt("res", value3);
			PlayerPrefs.SetInt("directlight", value4);
			PlayerPrefs.SetFloat("sens", (float)num2 / 10f);
			PlayerPrefs.SetFloat("gamevolume", (float)num3 / 10f);
			PlayerPrefs.SetFloat("musicvolume", (float)num4 / 10f);
		}
		GUIOptions.Load();
	}

	private void recv_character()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int value = NET.READ_BYTE();
		int value2 = NET.READ_BYTE();
		int value3 = NET.READ_BYTE();
		int value4 = NET.READ_BYTE();
		int value5 = NET.READ_BYTE();
		int value6 = NET.READ_BYTE();
		int value7 = NET.READ_BYTE();
		int value8 = NET.READ_BYTE();
		int value9 = NET.READ_BYTE();
		int value10 = NET.READ_BYTE();
		PlayerPrefs.SetInt("bp_char" + vk_id, 1);
		PlayerPrefs.SetInt("cp0" + vk_id, value);
		PlayerPrefs.SetInt("cp1" + vk_id, value2);
		PlayerPrefs.SetInt("cp2" + vk_id, value3);
		PlayerPrefs.SetInt("cp3" + vk_id, value4);
		PlayerPrefs.SetInt("cp4" + vk_id, value5);
		PlayerPrefs.SetInt("cp5" + vk_id, value6);
		PlayerPrefs.SetInt("cp6" + vk_id, value7);
		PlayerPrefs.SetInt("cp7" + vk_id, value8);
		PlayerPrefs.SetInt("cp8" + vk_id, value9);
		PlayerPrefs.SetInt("cp9" + vk_id, value10);
		GUIInv.LoadChar();
	}

	private void recv_name()
	{
		NET.BEGIN_READ(buffer, len, 4);
		string playername = NET.READ_STRING();
		int nameCount = NET.READ_BYTE();
		int num = NET.READ_BYTE();
		string namekey = NET.READ_STRING();
		GUIOptions.playername = playername;
		GUIOptions.NameCount = nameCount;
		GUIOptions.sNameCount2 = GUIOptions.sNameCount + nameCount;
		GUIName.sEdit = GUIOptions.playername;
		GUIOptions.namekey = namekey;
		switch (num)
		{
		case 0:
			GUIName.msg = Lang.GetString("_PRESERVED");
			GUIName.msg_color = BaseColor.Green;
			break;
		case 1:
			GUIName.msg = Lang.GetString("_INCORRECT_LENGTH");
			GUIName.msg_color = BaseColor.Red;
			break;
		case 2:
			GUIName.msg = Lang.GetString("_MIX_LANGUAGES");
			GUIName.msg_color = BaseColor.Red;
			break;
		case 3:
			GUIName.msg = Lang.GetString("_NAME_BUSY");
			GUIName.msg_color = BaseColor.Red;
			break;
		}
	}

	private void recv_profile()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int gid = NET.READ_LONG();
		string authkey = NET.READ_STRING();
		int gold = NET.READ_LONG();
		GUIOptions.gid = gid;
		GUIOptions.authkey = authkey;
		GUIOptions.sGold = gold.ToString();
		GUIOptions.Gold = gold;
		Main.gid = gid;
	}

	private void recv_inv()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int clear = NET.READ_BYTE();
		int c = NET.READ_BYTE();
		recv_inv_weapon_pass(clear, c);
		recv_inv_case_pass(clear, c);
		recv_inv_key_pass(clear, c);
		recv_inv_playerskin_pass(clear, c);
	}

	private void recv_inv_weapon_pass(int clear, int c)
	{
		if (c != 0)
		{
			return;
		}
		if (clear == 1)
		{
			GUIInv.wlist.Clear();
		}
		int num = (len - 6) / 14;
		if (MainManager.steam)
		{
			num = (len - 6) / 18;
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = NET.READ_SHORT();
			ulong num3 = 0uL;
			num3 = ((!MainManager.steam) ? ((ulong)NET.READ_LONG()) : NET.READ_ULONG64());
			int frags = NET.READ_LONG();
			byte b = (byte)NET.READ_BYTE();
			byte b2 = (byte)NET.READ_BYTE();
			byte b3 = (byte)NET.READ_BYTE();
			byte dublicate = (byte)NET.READ_BYTE();
			if (GUIInv.winfo[num2] != null)
			{
				int @int = PlayerPrefs.GetInt("wi_" + num3 + "_scopeid", 0);
				WeaponInv weaponInv = new WeaponInv(num3, GUIInv.winfo[num2]);
				weaponInv.frags = frags;
				weaponInv.mod[0] = b;
				weaponInv.mod[1] = b2;
				weaponInv.mod[2] = b3;
				weaponInv.dublicate = dublicate;
				weaponInv.scopeid = @int;
				GUIInv.wlist.Add(weaponInv);
			}
		}
		GUIInv.LoadSet();
	}

	private void recv_inv_case_pass(int clear, int c)
	{
		if (c != 1)
		{
			return;
		}
		if (clear == 1)
		{
			GUIInv.clist.Clear();
		}
		int num = (len - 6) / 6;
		if (MainManager.steam)
		{
			num = (len - 6) / 10;
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = NET.READ_SHORT();
			ulong num3 = 0uL;
			num3 = ((!MainManager.steam) ? ((ulong)NET.READ_LONG()) : NET.READ_ULONG64());
			if (GUIInv.cinfo[num2] != null)
			{
				GUIInv.clist.Add(new CaseInv(num3, GUIInv.cinfo[num2]));
			}
		}
	}

	private void recv_inv_playerskin_pass(int clear, int c)
	{
		if (c != 3)
		{
			return;
		}
		if (clear == 1)
		{
			GUIInv.pslist.Clear();
		}
		int num = (len - 6) / 2;
		for (int i = 0; i < num; i++)
		{
			int num2 = NET.READ_SHORT();
			if (GUIInv.psinfo[num2] != null)
			{
				GUIInv.pslist.Add(new PlayerSkinInv(0, GUIInv.psinfo[num2]));
			}
		}
		recv_inv_playerskin_calc();
	}

	private void recv_inv_playerskin_calc()
	{
		GUIInv.hairlock = new int[GUIInv.tHair.Length];
		GUIInv.hatlock = new int[GUIInv.tHat.Length];
		GUIInv.bodylock = new int[GUIInv.tBody.Length];
		GUIInv.pantslock = new int[GUIInv.tPants.Length];
		GUIInv.bootslock = new int[GUIInv.tBoots.Length];
		for (int i = 0; i < GUIInv.hairlock.Length; i++)
		{
			GUIInv.hairlock[i] = 0;
			if (i >= 3)
			{
				GUIInv.hairlock[i] = 1;
			}
		}
		for (int j = 0; j < GUIInv.hatlock.Length; j++)
		{
			GUIInv.hatlock[j] = 0;
			if (j > 0)
			{
				GUIInv.hatlock[j] = 1;
			}
		}
		for (int k = 0; k < GUIInv.bodylock.Length; k++)
		{
			GUIInv.bodylock[k] = 0;
			if (k >= 4)
			{
				GUIInv.bodylock[k] = 1;
			}
		}
		for (int l = 0; l < GUIInv.pantslock.Length; l++)
		{
			GUIInv.pantslock[l] = 0;
			if (l >= 3)
			{
				GUIInv.pantslock[l] = 1;
			}
		}
		for (int m = 0; m < GUIInv.bootslock.Length; m++)
		{
			GUIInv.bootslock[m] = 0;
			if (m >= 1)
			{
				GUIInv.bootslock[m] = 1;
			}
		}
		for (int n = 0; n < GUIInv.pslist.Count; n++)
		{
			PlayerSkinInfo psi = GUIInv.pslist[n].psi;
			if (psi.hairid >= 0)
			{
				GUIInv.hairlock[psi.hairid] = 0;
			}
			if (psi.hatid >= 0)
			{
				GUIInv.hatlock[psi.hatid] = 0;
			}
			if (psi.bodyid >= 0)
			{
				GUIInv.bodylock[psi.bodyid] = 0;
			}
			if (psi.pantsid >= 0)
			{
				GUIInv.pantslock[psi.pantsid] = 0;
			}
			if (psi.bootsid >= 0)
			{
				GUIInv.bootslock[psi.bootsid] = 0;
			}
		}
		if (GUIInv.hatlock[22] == 0)
		{
			GUIInv.hatlock[23] = 0;
		}
		GUIInv.hatlock[1] = 0;
		GUIInv.hatlock[2] = 1;
		GUIInv.hatlock[12] = 0;
		GUIInv.hatlock[26] = 0;
		GUIInv.hatlock[27] = 0;
		GUIInv.hatlock[28] = 1;
		GUIInv.bodylock[3] = 1;
		GUIInv.pantslock[2] = 1;
		GUIInv.hairlock[5] = 0;
		GUIInv.hairlock[6] = 0;
		GUIInv.hairlock[7] = 0;
		GUIInv.hairlock[8] = 0;
	}

	private void recv_weaponinfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		WeaponInfo weaponInfo = new WeaponInfo();
		weaponInfo.id = NET.READ_LONG();
		weaponInfo.name = NET.READ_STRING();
		weaponInfo.fullname = NET.READ_STRING();
		weaponInfo.ammo = NET.READ_BYTE();
		weaponInfo.backpack = NET.READ_SHORT();
		weaponInfo.damage = NET.READ_BYTE();
		weaponInfo.firerate = NET.READ_SHORT();
		weaponInfo.distance = NET.READ_BYTE();
		weaponInfo.accuracy = NET.READ_BYTE();
		weaponInfo.reload = NET.READ_SHORT();
		weaponInfo.recoil = NET.READ_BYTE();
		weaponInfo.piercing = NET.READ_BYTE();
		weaponInfo.mobility = NET.READ_BYTE();
		weaponInfo.slot = NET.READ_BYTE();
		weaponInfo.key = NET.READ_STRING();
		weaponInfo.GenerateIcon();
		GUIInv.winfo[weaponInfo.id] = weaponInfo;
	}

	private void recv_inv_key_pass(int clear, int c)
	{
		if (c != 2)
		{
			return;
		}
		if (clear == 1)
		{
			GUIInv.klist.Clear();
		}
		int num = (len - 6) / 6;
		if (MainManager.steam)
		{
			num = (len - 6) / 10;
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = NET.READ_SHORT();
			ulong num3 = 0uL;
			num3 = ((!MainManager.steam) ? ((ulong)NET.READ_LONG()) : NET.READ_ULONG64());
			if (GUIInv.kinfo[num2] != null)
			{
				GUIInv.klist.Add(new KeyInv(num3, GUIInv.kinfo[num2]));
			}
		}
	}

	private void recv_caseinfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int idx = NET.READ_SHORT();
		string text = NET.READ_STRING();
		CaseInfo caseInfo = new CaseInfo(idx, text);
		caseInfo.fullname = NET.READ_STRING();
		caseInfo.keyid = NET.READ_SHORT();
		caseInfo.cost = NET.READ_BYTE();
		caseInfo.shop = NET.READ_BYTE();
		caseInfo.weaponcount = NET.READ_BYTE();
		caseInfo.weapon = new WeaponInfo[caseInfo.weaponcount];
		for (int i = 0; i < caseInfo.weaponcount; i++)
		{
			WeaponInfo weaponInfo = new WeaponInfo();
			weaponInfo.id = NET.READ_SHORT();
			weaponInfo.name = NET.READ_STRING();
			weaponInfo.fullname = NET.READ_STRING();
			weaponInfo.p = NET.READ_BYTE();
			weaponInfo.tIcon = ContentLoader_.LoadTexture(weaponInfo.name + "_icon") as Texture2D;
			if (weaponInfo.tIcon == null)
			{
				weaponInfo.tIcon = ContentLoader_.LoadTexture(weaponInfo.name) as Texture2D;
			}
			if (weaponInfo.tIcon == null)
			{
				weaponInfo.tIcon = Resources.Load("weapons/" + weaponInfo.name) as Texture2D;
			}
			if (weaponInfo.tIcon == null)
			{
				weaponInfo.tIcon = TEX.GetTextureByName("red");
			}
			weaponInfo.vIcon = GUIGameSet.CalcSize(weaponInfo.tIcon);
			caseInfo.weapon[i] = weaponInfo;
			if (GUIInv.winfo[weaponInfo.id] != null && GUIInv.winfo[weaponInfo.id].p < 0)
			{
				GUIInv.winfo[weaponInfo.id].p = weaponInfo.p;
			}
		}
		caseInfo.scost = caseInfo.cost.ToString();
		if (MainManager.steam)
		{
			caseInfo.scost = ((float)caseInfo.cost * 0.01f).ToString("0.00$");
		}
		GUIInv.cinfo[caseInfo.idx] = caseInfo;
	}

	private void recv_keyinfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		KeyInfo keyInfo = new KeyInfo();
		keyInfo.idx = NET.READ_SHORT();
		keyInfo.name = NET.READ_STRING();
		keyInfo.fullname = NET.READ_STRING();
		keyInfo.cost = NET.READ_BYTE();
		keyInfo.shop = NET.READ_BYTE();
		keyInfo.scost = keyInfo.cost.ToString();
		keyInfo.img = Resources.Load("cases/key_" + keyInfo.name) as Texture2D;
		if (keyInfo.img == null)
		{
			keyInfo.img = ContentLoader_.LoadTexture("key_" + keyInfo.name) as Texture2D;
		}
		if (keyInfo.img == null)
		{
			Debug.Log("error load key img " + keyInfo.name);
		}
		if (MainManager.steam)
		{
			keyInfo.scost = ((float)keyInfo.cost * 0.01f).ToString("0.00$");
		}
		GUIInv.kinfo[keyInfo.idx] = keyInfo;
	}

	private void recv_buy()
	{
		NET.BEGIN_READ(buffer, len, 4);
		if ((GUIShop.buycode = NET.READ_BYTE()) == 1)
		{
			GUIShop.inlock = false;
		}
	}

	private void recv_caseopen()
	{
		NET.BEGIN_READ(buffer, len, 4);
		GUICase.StartRoll(NET.READ_BYTE());
	}

	private void recv_casecraft()
	{
		NET.BEGIN_READ(buffer, len, 4);
		GUICraft.CraftEnd(NET.READ_LONG());
	}

	private void recv_questlist()
	{
		int num = (len - 5) / 10;
		NET.BEGIN_READ(buffer, len, 4);
		if (NET.READ_BYTE() == 1)
		{
			GUIObj.qlist.Clear();
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = NET.READ_BYTE();
			int progress = NET.READ_LONG();
			int complete = NET.READ_BYTE();
			int require = NET.READ_LONG();
			if (GUIObj.qinfo[num2] != null)
			{
				GUIObj.qlist.Add(new Quest(GUIObj.qinfo[num2], progress, require, complete));
			}
		}
		Quest.CalcCount(GUIObj.qlist);
		GUIObj.OnResize_zone();
	}

	private void recv_questreward()
	{
		Main.SetMenuNewItem(1);
	}

	private void recv_rcon()
	{
		NET.BEGIN_READ(buffer, len, 4);
		Console.Log(NET.READ_STRING());
	}

	private void recv_error()
	{
		Log.Add("recv_error");
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_LONG();
		string text = NET.READ_STRING();
		MainManager.error = true;
		MainManager.error_msg = text + " (errorcode " + num + ")";
	}

	private void recv_auth_steam()
	{
		if (MainManager.steam)
		{
			NET.BEGIN_READ(buffer, len, 4);
			vk_key = NET.READ_STRING();
			send_auth_vk();
		}
	}

	private void recv_microtxnstate()
	{
		PlayerPrefs.DeleteKey("steam_order");
	}

	private void recv_auth_idc()
	{
		if (MainManager.idc)
		{
			NET.BEGIN_READ(buffer, len, 4);
			vk_key = NET.READ_STRING();
			send_auth_vk();
		}
	}

	private void recv_auth_mycom()
	{
		if (MainManager.mycom)
		{
			NET.BEGIN_READ(buffer, len, 4);
			string text = NET.READ_STRING();
			vk_id = MainManager.sz_pers_id;
			vk_key = text;
			send_auth_vk();
		}
	}

	private void recv_payerequest_mycom()
	{
		if (MainManager.mycom)
		{
			NET.BEGIN_READ(buffer, len, 4);
			string text = NET.READ_STRING();
			string text2 = NET.READ_STRING();
			string s = text + text2;
			GUIGold.cs.OpenUrl(s);
		}
	}

	private void recv_topplayer()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_SHORT();
		int num2 = NET.READ_BYTE();
		if (NET.READ_BYTE() == 1)
		{
		}
		while (NET.READ_POS() < len)
		{
			int num3 = NET.READ_LONG();
			string n = NET.READ_STRING();
			int exp = NET.READ_LONG();
			int f = NET.READ_LONG();
			int d = NET.READ_LONG();
			int a = NET.READ_LONG();
			int h = NET.READ_LONG();
			GUIRank.tp[num3] = new TopPlayer(num3, n, exp, f, d, a, h);
		}
		GUIRank.position = num;
		if (num == 9999)
		{
			GUIRank.progress = (float)num2 * 0.01f;
			GUIRank.sProgress = num2 + "%";
			return;
		}
		GUIRank.sProgress = GUIRank.sYouTake + " " + (num + 1) + " " + GUIRank.sPlaceRating;
	}

	private void recv_questload()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_LONG();
		PlayerPrefs.SetInt("bp_quest", num);
		GUIObj.currqid = num;
	}

	private void recv_shop()
	{
		int num = (len - 4) / 8;
		NET.BEGIN_READ(buffer, len, 4);
		for (int i = 0; i < num; i++)
		{
			int num2 = NET.READ_SHORT();
			int num3 = NET.READ_SHORT();
			int sec = NET.READ_LONG();
			for (int j = 0; j < GUIShop.cshop.Count; j++)
			{
				if (GUIShop.cshop[j].ci.idx == num2)
				{
					if (num3 >= 1000)
					{
						GUIShop.cshop[j].count = "999+";
					}
					else
					{
						GUIShop.cshop[j].count = num3.ToString();
					}
					GUIShop.cshop[j].icount = num3;
					GUIShop.cshop[j].sec = sec;
					GUIShop.cshop[j].time = Time.time;
					break;
				}
			}
		}
	}

	private void recv_casesell()
	{
		NET.BEGIN_READ(buffer, len, 4);
		switch (NET.READ_BYTE())
		{
		case 1:
			GUICraft.SellEnd();
			GUICraft.cs_sec = 14400;
			GUICraft.cs_time = Time.time;
			break;
		case 0:
			GUICraft.clicklock = false;
			break;
		}
	}

	private void recv_casesellinfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		GUICraft.cs_sec = NET.READ_LONG();
		GUICraft.cs_time = Time.time;
	}

	private void recv_bonus()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int v = NET.READ_SHORT();
		int v2 = NET.READ_LONG();
		GUIBonus.cs.Set(num, v, v2);
		if (num == 1)
		{
			GUIBonus.cs.shownext = true;
		}
	}

	private void recv_clanbase()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_LONG();
		string clanname = NET.READ_STRING();
		int clanoid = NET.READ_LONG();
		int num2 = NET.READ_BYTE();
		int clanpcount = NET.READ_BYTE();
		string clankey = NET.READ_STRING();
		if (num == 0)
		{
			GUIClan.clanname = "";
			GUIClan.clanid = 0;
			GUIClan.clanoid = 0;
			GUIClan.clanpcount = 0;
			GUIClan.clanslots = 0;
			GUIClan.sClanslots = "";
			GUIClan.clankey = "";
		}
		else
		{
			GUIClan.clanname = clanname;
			GUIClan.clanid = num;
			GUIClan.clanoid = clanoid;
			GUIClan.clanpcount = clanpcount;
			GUIClan.clanslots = (num2 + 1) * 8;
			GUIClan.sClanslots = GUIClan.clanpcount + "/" + GUIClan.clanslots;
			GUIClan.clankey = clankey;
		}
		if (GUIClan.show)
		{
			GUIClan.SetActive(false);
			GUIClan.SetActive(true);
			if (GUIClan.clanid > 0)
			{
				cs.send_clanplayers();
			}
		}
	}

	private void recv_clanrestore()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int clanid_owner = NET.READ_LONG();
		string clanname_owner = NET.READ_STRING();
		GUIClan.clanid_owner = clanid_owner;
		GUIClan.clanname_owner = clanname_owner;
	}

	private void recv_clancreate()
	{
		NET.BEGIN_READ(buffer, len, 4);
		switch (NET.READ_BYTE())
		{
		case 0:
			GUIClan.msg = Lang.GetString("_PRESERVED");
			GUIClan.msg_color = BaseColor.Green;
			break;
		case 1:
			GUIClan.msg = Lang.GetString("_INCORRECT_LENGTH");
			GUIClan.msg_color = BaseColor.Red;
			break;
		case 2:
			GUIClan.msg = Lang.GetString("_MIX_LANGUAGES");
			GUIClan.msg_color = BaseColor.Red;
			break;
		case 3:
			GUIClan.msg = Lang.GetString("_NAME_BUSY");
			GUIClan.msg_color = BaseColor.Red;
			break;
		}
	}

	private void recv_clanlist()
	{
		NET.BEGIN_READ(buffer, len, 4);
		if (NET.READ_BYTE() == 1)
		{
		}
		while (NET.READ_POS() < len)
		{
			int num = NET.READ_BYTE();
			int id = NET.READ_LONG();
			string n = NET.READ_STRING();
			int sl = NET.READ_BYTE();
			int st = NET.READ_BYTE();
			string owner = NET.READ_STRING();
			int players = NET.READ_BYTE();
			GUIClan.cl[num] = new ClanData(id, n, owner, sl, st, players);
		}
	}

	private void recv_clanfind()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int id = NET.READ_LONG();
		string n = NET.READ_STRING();
		int sl = NET.READ_BYTE();
		int st = NET.READ_BYTE();
		string owner = NET.READ_STRING();
		int players = NET.READ_BYTE();
		switch (num)
		{
		case 1:
			GUIClan.sFind = Lang.GetString("_INCORRECT_CLAN_NAME");
			return;
		case 2:
			GUIClan.sFind = Lang.GetString("_INCORRECT_CLAN_NAME");
			return;
		case 3:
			GUIClan.sFind = Lang.GetString("_CLAN_NOT_FOUND");
			return;
		}
		GUIClan.sFind = Lang.GetString("_CLAN_FOUND");
		GUIClan.NullClans();
		GUIClan.cl[0] = new ClanData(id, n, owner, sl, st, players);
	}

	private void recv_clanplayer()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int id = NET.READ_LONG();
		string n = NET.READ_STRING();
		int rank = NET.READ_BYTE();
		int p = NET.READ_BYTE();
		int p2 = NET.READ_BYTE();
		int p3 = NET.READ_BYTE();
		int p4 = NET.READ_BYTE();
		int p5 = NET.READ_BYTE();
		int p6 = NET.READ_BYTE();
		int p7 = NET.READ_BYTE();
		int p8 = NET.READ_BYTE();
		int p9 = NET.READ_BYTE();
		int p10 = NET.READ_BYTE();
		int exp = NET.READ_LONG();
		int f = NET.READ_LONG();
		int d = NET.READ_LONG();
		if (num == 0)
		{
			if (GUIClan.playerlist == null)
			{
				GUIClan.playerlist = new List<ClanPlayer>();
			}
			GUIClan.currplayer = null;
			GUIClan.playerlist.Clear();
		}
		GUIClan.playerlist.Add(new ClanPlayer(id, n, rank, p, p2, p3, p4, p5, p6, p7, p8, p9, p10, exp, f, d));
	}

	private void recv_clanmessage()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int id = NET.READ_LONG();
		int t = NET.READ_BYTE();
		int v = NET.READ_LONG();
		int v2 = NET.READ_LONG();
		string msg = NET.READ_STRING();
		int dt = NET.READ_LONG();
		int st = NET.READ_BYTE();
		if (num == 0)
		{
			if (GUIClan.messagelist == null)
			{
				GUIClan.messagelist = new List<ClanMessage>();
			}
			GUIClan.messagelist.Clear();
		}
		GUIClan.messagelist.Add(new ClanMessage(id, t, v, v2, msg, dt, st));
	}

	private void recv_clanadminop()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		if (num == 0)
		{
			GUIClan.clannamecount = num2;
		}
		if (num == 1)
		{
			GUIClan.clanstatus = num2;
		}
	}

	private void recv_nettest()
	{
		NET.BEGIN_READ(buffer, len, 4);
		NET.READ_BYTE();
		NET.READ_LONG();
		NET.READ_ULONG64();
	}

	private void recv_clanstats()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_LONG();
		int num2 = NET.READ_LONG();
		int num3 = NET.READ_LONG();
		if (num3 != 0)
		{
			GUIClan.sFD = ((float)num2 / (float)num3).ToString("0.00");
		}
		GUIClan.sF = num2.ToString();
		GUIClan.sD = num3.ToString();
		int level = Main.CalcClanLevel(num);
		int num4 = Main.CalcClanExp(level);
		int num5 = Main.CalcClanNextExp(level);
		GUIClan.sLevel = level.ToString();
		GUIClan.sExp = num.ToString();
		GUIClan.fLevelProgress = (float)(num - num4) / (float)(num5 - num4);
		GUIClan.sLevelProgress = (GUIClan.fLevelProgress * 100f).ToString("0.00") + "%";
		GUIClan.sExpProgress = num + "/" + num5;
	}

	private void recv_clanplayerstats()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_LONG();
		int exp = NET.READ_LONG();
		int f = NET.READ_LONG();
		int d = NET.READ_LONG();
		int exp2 = NET.READ_LONG();
		int f2 = NET.READ_LONG();
		int d2 = NET.READ_LONG();
		int exp3 = NET.READ_LONG();
		int f3 = NET.READ_LONG();
		int d3 = NET.READ_LONG();
		for (int i = 0; i < GUIClan.playerlist.Count; i++)
		{
			if (GUIClan.playerlist[i].id == num)
			{
				GUIClan.playerlist[i].SetStats(exp, f, d, exp2, f2, d2, exp3, f3, d3);
				break;
			}
		}
	}

	private void recv_clanchatlist()
	{
		for (int i = 0; i < GUIClan.playerlist.Count; i++)
		{
			GUIClan.playerlist[i].online = 0;
		}
		GUIClan.playerlistonline = 0;
		NET.BEGIN_READ(buffer, len, 4);
		while (!NET.READ_ERROR())
		{
			int num = NET.READ_LONG();
			for (int j = 0; j < GUIClan.playerlist.Count; j++)
			{
				if (GUIClan.playerlist[j].id == num)
				{
					GUIClan.playerlist[j].online = 1;
					GUIClan.playerlistonline++;
					break;
				}
			}
		}
	}

	private void recv_clanchatsend()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_LONG();
		string n = NET.READ_STRING();
		string msg = NET.READ_STRING();
		if (GUIClan.chatlist == null)
		{
			GUIClan.chatlist = new List<ClanChatMessage>();
		}
		GUIClan.chatlist.Add(new ClanChatMessage(id, n, msg));
		if (GUIClan.chatlist.Count >= 15)
		{
			GUIClan.chatlist.RemoveAt(0);
		}
		if (!Main.show)
		{
			HUDMessage.AddChat(n, msg);
		}
	}

	private void recv_topclan()
	{
		NET.BEGIN_READ(buffer, len, 4);
		if (NET.READ_BYTE() == 1)
		{
		}
		while (NET.READ_POS() < len)
		{
			int num = NET.READ_LONG();
			string n = NET.READ_STRING();
			int exp = NET.READ_LONG();
			int slots = NET.READ_BYTE();
			int players = NET.READ_BYTE();
			GUIRank.tc[num] = new TopClan(num, n, exp, slots, players);
		}
	}

	private void recv_profile2()
	{
		NET.BEGIN_READ(buffer, len, 4);
		acckey = NET.READ_STRING();
	}

	private void recv_auth_google()
	{
		NET.BEGIN_READ(buffer, len, 4);
		string text = NET.READ_STRING();
		string text2 = NET.READ_STRING();
		Console.Log("auth google: " + text + " " + text2);
		vk_id = text;
		vk_key = text2;
		PlayerPrefs.SetString("bp_mobile_auth_id", text);
		PlayerPrefs.SetString("bp_mobile_auth_key", text2);
		send_auth_vk();
	}

	private void recv_purchase()
	{
		NET.BEGIN_READ(buffer, len, 4);
		NET.READ_BYTE();
	}

	private void recv_orderlist()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		string gpa = NET.READ_STRING();
		int net = NET.READ_BYTE();
		if (num != 2)
		{
			GUIGold.cs.UpdateItem(gpa, net);
		}
	}

	private void recv_playerskininfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_SHORT();
		int cost = NET.READ_BYTE();
		int shop = NET.READ_BYTE();
		if (num < GUIInv.psinfo.Length && GUIInv.psinfo[num] != null)
		{
			GUIInv.psinfo[num].cost = cost;
			GUIInv.psinfo[num].scost = cost.ToString();
			GUIInv.psinfo[num].shop = shop;
		}
	}

	private void recv_mkey()
	{
		NET.BEGIN_READ(buffer, len, 4);
		MainManager.mkey = NET.READ_STRING();
		MainManager.mod = true;
	}

	public void send_play(int gamemode)
	{
		NET.BEGIN_WRITE();
		NET.WRITE_BYTE(245);
		NET.WRITE_BYTE(2);
		NET.WRITE_BYTE(0);
		NET.WRITE_BYTE(0);
		NET.WRITE_BYTE((byte)gamemode);
		NET.END_WRITE();
		cl_send();
	}

	public void send_list(int pull)
	{
		NET.BEGIN_WRITE();
		NET.WRITE_BYTE(245);
		NET.WRITE_BYTE(3);
		NET.WRITE_BYTE(0);
		NET.WRITE_BYTE(0);
		NET.WRITE_BYTE((byte)pull);
		NET.END_WRITE();
		cl_send();
	}

	public void send_customplay(int pull)
	{
		NET.BEGIN_WRITE(245, 5);
		NET.WRITE_BYTE((byte)pull);
		NET.END_WRITE();
		cl_send();
	}

	public void send_auth_vk()
	{
		NET.BEGIN_WRITE();
		NET.WRITE_BYTE(245);
		NET.WRITE_BYTE(100);
		NET.WRITE_BYTE(0);
		NET.WRITE_BYTE(0);
		NET.WRITE_STRING(vk_id);
		NET.WRITE_STRING(vk_key);
		NET.WRITE_LONG(115);
		NET.WRITE_BYTE(0);
		NET.END_WRITE();
		cl_send();
	}

	public void send_auth_steam()
	{
		NET.BEGIN_WRITE();
		NET.WRITE_BYTE(245);
		NET.WRITE_BYTE(200);
		NET.WRITE_BYTE(0);
		NET.WRITE_BYTE(0);
		NET.WRITE_STRING(vk_id);
		NET.WRITE_STRING(vk_key);
		NET.END_WRITE();
		cl_send();
	}

	public void send_microtxn(int idx)
	{
		NET.BEGIN_WRITE(245, 201);
		NET.WRITE_BYTE((byte)idx);
		NET.WRITE_STRING(MainManager.steamlang);
		NET.END_WRITE();
		cl_send();
	}

	public void send_microtxnstate(byte bstate, ulong orderid)
	{
		NET.BEGIN_WRITE(245, 202);
		NET.WRITE_BYTE(bstate);
		NET.WRITE_ULONG64(orderid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_auth_idc()
	{
		NET.BEGIN_WRITE(245, 220);
		NET.WRITE_STRING(vk_id);
		NET.WRITE_STRING(vk_key);
		NET.WRITE_STRING(vk_reserve);
		NET.END_WRITE();
		cl_send();
	}

	public void send_auth_mycom()
	{
		NET.BEGIN_WRITE(245, 230);
		NET.WRITE_STRING(MainManager.sz_pers_id);
		NET.WRITE_STRING(MainManager.sz_token);
		NET.END_WRITE();
		cl_send();
	}

	public void send_payrequest_mycom(int i, string l)
	{
		NET.BEGIN_WRITE(245, 231);
		NET.WRITE_LONG(i);
		NET.WRITE_STRING(l);
		NET.END_WRITE();
		cl_send();
	}

	public void send_weaponfrags(int wuid)
	{
		NET.BEGIN_WRITE(245, 30);
		NET.WRITE_LONG(wuid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_weaponfrags_steam(ulong wuid)
	{
		NET.BEGIN_WRITE(245, 30);
		NET.WRITE_ULONG64(wuid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_set()
	{
		NET.BEGIN_WRITE(245, 41);
		NET.END_WRITE();
		cl_send();
	}

	public void send_saveset()
	{
		NET.BEGIN_WRITE(245, 42);
		int @int = PlayerPrefs.GetInt("set_0_slot_0_uid", 0);
		int int2 = PlayerPrefs.GetInt("set_0_slot_1_uid", 0);
		int int3 = PlayerPrefs.GetInt("set_1_slot_0_uid", 0);
		int int4 = PlayerPrefs.GetInt("set_1_slot_1_uid", 0);
		int int5 = PlayerPrefs.GetInt("set_2_slot_0_uid", 0);
		int int6 = PlayerPrefs.GetInt("set_2_slot_1_uid", 0);
		NET.WRITE_LONG(@int);
		NET.WRITE_LONG(int2);
		NET.WRITE_LONG(PlayerPrefs.GetInt("set_0_slot_2_uid", 0));
		NET.WRITE_LONG(PlayerPrefs.GetInt("set_0_slot_3_uid", 0));
		NET.WRITE_LONG(PlayerPrefs.GetInt("set_0_slot_4_uid", 0));
		NET.WRITE_LONG(int3);
		NET.WRITE_LONG(int4);
		NET.WRITE_LONG(PlayerPrefs.GetInt("set_1_slot_2_uid", 0));
		NET.WRITE_LONG(PlayerPrefs.GetInt("set_1_slot_3_uid", 0));
		NET.WRITE_LONG(PlayerPrefs.GetInt("set_1_slot_4_uid", 0));
		NET.WRITE_LONG(int5);
		NET.WRITE_LONG(int6);
		NET.WRITE_LONG(PlayerPrefs.GetInt("set_2_slot_2_uid", 0));
		NET.WRITE_LONG(PlayerPrefs.GetInt("set_2_slot_3_uid", 0));
		NET.WRITE_LONG(PlayerPrefs.GetInt("set_2_slot_4_uid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + @int + "_scopeid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int2 + "_scopeid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int3 + "_scopeid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int4 + "_scopeid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int5 + "_scopeid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int6 + "_scopeid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + @int + "_silid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int2 + "_silid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int3 + "_silid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int4 + "_silid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int5 + "_silid", 0));
		NET.WRITE_BYTE((byte)PlayerPrefs.GetInt("wi_" + int6 + "_silid", 0));
		NET.END_WRITE();
		cl_send();
	}

	public void send_stats()
	{
		NET.BEGIN_WRITE(245, 43);
		NET.END_WRITE();
		cl_send();
	}

	public void send_options()
	{
		NET.BEGIN_WRITE(245, 44);
		NET.END_WRITE();
		cl_send();
	}

	public void send_saveoptions()
	{
		NET.BEGIN_WRITE(245, 45);
		NET.WRITE_BYTE(0);
		NET.WRITE_BYTE((byte)Crosshair.crosshair_color);
		NET.WRITE_BYTE((byte)GUIOptions.health_position);
		NET.WRITE_BYTE((byte)GUIOptions.res);
		NET.WRITE_BYTE((byte)GUIOptions.directlight);
		NET.WRITE_BYTE((byte)(Controll.sens * 10f));
		NET.WRITE_BYTE((byte)(GUIOptions.gamevolume * 10f));
		NET.WRITE_BYTE((byte)(GUIOptions.musicvolume * 10f));
		NET.END_WRITE();
		cl_send();
	}

	public void send_char()
	{
		NET.BEGIN_WRITE(245, 46);
		NET.END_WRITE();
		cl_send();
	}

	public void send_savechar()
	{
		NET.BEGIN_WRITE(245, 47);
		NET.WRITE_BYTE((byte)GUIInv.currcolorskin);
		NET.WRITE_BYTE((byte)GUIInv.currcoloreye);
		NET.WRITE_BYTE((byte)GUIInv.currcolorhair);
		NET.WRITE_BYTE((byte)GUIInv.currhair);
		NET.WRITE_BYTE((byte)GUIInv.curreye);
		NET.WRITE_BYTE((byte)GUIInv.currbeard);
		NET.WRITE_BYTE((byte)GUIInv.currhat);
		NET.WRITE_BYTE((byte)GUIInv.currbody);
		NET.WRITE_BYTE((byte)GUIInv.currpants);
		NET.WRITE_BYTE((byte)GUIInv.currboots);
		NET.END_WRITE();
		cl_send();
	}

	public void send_inv()
	{
		NET.BEGIN_WRITE(245, 50);
		NET.END_WRITE();
		cl_send();
	}

	public void send_name(string _name)
	{
		NET.BEGIN_WRITE(245, 48);
		NET.WRITE_STRING(_name);
		NET.END_WRITE();
		cl_send();
	}

	public void send_reload()
	{
		NET.BEGIN_WRITE(245, 52);
		NET.END_WRITE();
		cl_send();
	}

	public void send_buy(int c, int id)
	{
		NET.BEGIN_WRITE(245, 55);
		NET.WRITE_BYTE((byte)c);
		NET.WRITE_LONG(id);
		NET.END_WRITE();
		cl_send();
	}

	public void send_caseopen(int emu, int uid)
	{
		NET.BEGIN_WRITE(245, 56);
		NET.WRITE_BYTE((byte)emu);
		NET.WRITE_LONG(uid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_caseopen_steam(int emu, ulong uid)
	{
		NET.BEGIN_WRITE(245, 56);
		NET.WRITE_BYTE((byte)emu);
		NET.WRITE_ULONG64(uid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_casecraft(int uid0, int uid1, int uid2)
	{
		NET.BEGIN_WRITE(245, 57);
		NET.WRITE_LONG(uid0);
		NET.WRITE_LONG(uid1);
		NET.WRITE_LONG(uid2);
		NET.END_WRITE();
		cl_send();
	}

	public void send_casecraft_steam(ulong uid0, ulong uid1, ulong uid2)
	{
		NET.BEGIN_WRITE(245, 57);
		NET.WRITE_ULONG64(uid0);
		NET.WRITE_ULONG64(uid1);
		NET.WRITE_ULONG64(uid2);
		NET.END_WRITE();
		cl_send();
	}

	public void send_questlist()
	{
		NET.BEGIN_WRITE(245, 58);
		NET.END_WRITE();
		cl_send();
	}

	public void send_questreward(int qid)
	{
		NET.BEGIN_WRITE(245, 59);
		NET.WRITE_BYTE((byte)qid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_rank(int listid)
	{
		NET.BEGIN_WRITE(245, 60);
		NET.WRITE_BYTE((byte)listid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_rcon(string cmd)
	{
		NET.BEGIN_WRITE(245, 111);
		NET.WRITE_STRING(cmd);
		NET.END_WRITE();
		cl_send();
	}

	public void send_buyname()
	{
		NET.BEGIN_WRITE(245, 61);
		NET.END_WRITE();
		cl_send();
	}

	public void send_loadquest(int qid)
	{
		NET.BEGIN_WRITE(245, 62);
		NET.WRITE_LONG(qid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_savequest(int qid)
	{
		NET.BEGIN_WRITE(245, 63);
		NET.WRITE_LONG(qid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_shop()
	{
		NET.BEGIN_WRITE(245, 64);
		NET.END_WRITE();
		cl_send();
	}

	public void send_casesell(int uid)
	{
		NET.BEGIN_WRITE(245, 65);
		NET.WRITE_LONG(uid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_casesellinfo()
	{
		NET.BEGIN_WRITE(245, 66);
		NET.END_WRITE();
		cl_send();
	}

	public void send_bonus(int cmd)
	{
		NET.BEGIN_WRITE(245, 67);
		NET.WRITE_BYTE((byte)cmd);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanbase()
	{
		NET.BEGIN_WRITE(245, 68);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanrestore()
	{
		NET.BEGIN_WRITE(245, 69);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanlist()
	{
		NET.BEGIN_WRITE(245, 71);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clancreate(string n)
	{
		NET.BEGIN_WRITE(245, 70);
		NET.WRITE_STRING(n);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanfind(string n)
	{
		NET.BEGIN_WRITE(245, 72);
		NET.WRITE_STRING(n);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanplayers()
	{
		NET.BEGIN_WRITE(245, 73);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clandelete()
	{
		NET.BEGIN_WRITE(245, 74);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanmessage()
	{
		NET.BEGIN_WRITE(245, 75);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clannewmessage(int t, int v0)
	{
		NET.BEGIN_WRITE(245, 76);
		NET.WRITE_BYTE((byte)t);
		NET.WRITE_LONG(v0);
		NET.END_WRITE();
		cl_send();
	}

	public void send_claninviteop(int opid, int msgid)
	{
		NET.BEGIN_WRITE(245, 77);
		NET.WRITE_BYTE((byte)opid);
		NET.WRITE_LONG(msgid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanplayerop(int opid, int msgid)
	{
		NET.BEGIN_WRITE(245, 78);
		NET.WRITE_BYTE((byte)opid);
		NET.WRITE_LONG(msgid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanleave()
	{
		NET.BEGIN_WRITE(245, 79);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanadminop(int opid, int val)
	{
		NET.BEGIN_WRITE(245, 80);
		NET.WRITE_BYTE((byte)opid);
		NET.WRITE_BYTE((byte)val);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanrename(string n)
	{
		NET.BEGIN_WRITE(245, 81);
		NET.WRITE_STRING(n);
		NET.END_WRITE();
		cl_send();
	}

	public void send_nettest()
	{
		NET.BEGIN_WRITE(245, 240);
		NET.WRITE_BYTE(123);
		NET.WRITE_ULONG64(ulong.MaxValue);
		NET.WRITE_ULONG64(18446744073709551614uL);
		NET.WRITE_ULONG64(18446744073709551613uL);
		NET.WRITE_ULONG64(18446744073709551612uL);
		NET.WRITE_ULONG64(18446744073709551611uL);
		NET.WRITE_LONG(123456);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanstats()
	{
		NET.BEGIN_WRITE(245, 82);
		NET.END_WRITE();
		cl_send();
	}

	public void send_rankclan(int listid)
	{
		NET.BEGIN_WRITE(245, 86);
		NET.WRITE_BYTE((byte)listid);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanchatlist()
	{
		NET.BEGIN_WRITE(245, 84);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clanchatsend(string msg)
	{
		NET.BEGIN_WRITE(245, 85);
		NET.WRITE_STRING(msg);
		NET.END_WRITE();
		cl_send();
	}

	public void send_authgoogle()
	{
		NET.BEGIN_WRITE(245, 210);
		NET.WRITE_STRING(GP.email);
		NET.WRITE_STRING(GP.token);
		NET.END_WRITE();
		cl_send();
	}

	public void send_authgoogle2()
	{
		NET.BEGIN_WRITE(245, 213);
		NET.WRITE_STRING(GP2.token_part0);
		NET.WRITE_STRING(GP2.token_part1);
		NET.END_WRITE();
		cl_send();
	}

	public void send_purchasegoogle(string product, string token)
	{
		NET.BEGIN_WRITE(245, 211);
		NET.WRITE_STRING(product);
		NET.WRITE_STRING(token);
		NET.END_WRITE();
		cl_send();
	}

	public void send_orderlist()
	{
		NET.BEGIN_WRITE(245, 212);
		NET.END_WRITE();
		cl_send();
	}

	public void send_buyclanname()
	{
		NET.BEGIN_WRITE(245, 89);
		NET.END_WRITE();
		cl_send();
	}

	public void send_moderator(string cmd)
	{
		NET.BEGIN_WRITE(245, 91);
		NET.WRITE_STRING(cmd);
		NET.END_WRITE();
		cl_send();
	}

	public void cl_send()
	{
		if (!Active)
		{
			return;
		}
		try
		{
			client.GetStream().Write(NET.WRITE_DATA(), 0, NET.WRITE_LEN());
		}
		catch (Exception)
		{
			drop();
		}
	}

	public void drop()
	{
		if (Builder.active || GUICharEditor.cs != null || GUISkinEditor.cs != null)
		{
			MainManager.delaydrop = true;
			return;
		}
		GUIGameExit.ExitMainMenu();
		MainManager.error = true;
		MainManager.error_msg = "НЕ УДАЛОСЬ ПОДКЛЮЧИТЬСЯ К СЕРВЕРУ (LOST CONNECTION)";
		Main.state = 0;
		Active = false;
		GUIPlay.SetActive(false);
	}

	public void send_ping()
	{
		NET.BEGIN_WRITE(245, 101);
		NET.END_WRITE();
		cl_send();
	}
}
