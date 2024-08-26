using System;
using System.Collections.Generic;
using System.Net.Sockets;
using GameClass;
using Player;
using UnityEngine;

public class Client : MonoBehaviour
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

	public static Client cs = null;

	public static int Rate = 30000;

	public static string IP = "192.168.1.201";

	public static int PORT = 5555;

	public static string PASSWORD = "";

	public static bool Loaded = false;

	private bool Active;

	public TcpClient client;

	private byte[] sendbuffer;

	private static bool forcedisconnect = false;

	public static byte[] buffer = null;

	public static int len = 0;

	private List<RecvData> Tlist = new List<RecvData>();

	public static byte[] blockpacket = new byte[256];

	private byte[] readBuffer = new byte[102400];

	private int SplitRead;

	private int BytesRead;

	private int cc;

	private int cclen;

	public static bool isConnected()
	{
		if (cs.client == null)
		{
			return false;
		}
		return cs.client.Connected;
	}

	public static void Init()
	{
		forcedisconnect = false;
		for (int i = 0; i < 256; i++)
		{
			blockpacket[i] = 0;
		}
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

	public void Connect()
	{
		Active = false;
		if (client != null && client.Connected)
		{
			client.GetStream().Close();
			client.Close();
			client = null;
		}
		try
		{
			client = new TcpClient();
			client.NoDelay = true;
			IAsyncResult asyncResult = client.BeginConnect(IP, PORT, null, null);
			asyncResult.AsyncWaitHandle.WaitOne(2000);
			if (!client.Connected)
			{
				Active = false;
			}
			else
			{
				client.EndConnect(asyncResult);
				client.GetStream().BeginRead(readBuffer, 0, Rate, DoRead, null);
				Active = true;
			}
		}
		catch
		{
			Active = false;
		}
		if (Active)
		{
			send_auth();
		}
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
	}

	public void OnApplicationQuit()
	{
		Disconnect();
	}

	public void Disconnect()
	{
		if (client != null)
		{
			client.Close();
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

	public void DemoOnRecvPacket()
	{
		if (len >= 4)
		{
			ProcessData();
		}
	}

	private void ProcessData()
	{
		if (len < 2 || buffer[0] != 245)
		{
			return;
		}
		int num = buffer[1];
		if (blockpacket[num] != 1)
		{
			switch (buffer[1])
			{
			case 0:
				recv_auth();
				break;
			case 1:
				recv_playerinfo();
				break;
			case 2:
				recv_playerteam();
				break;
			case 3:
				recv_pos();
				break;
			case 4:
				recv_attack();
				break;
			case 5:
				recv_health();
				break;
			case 6:
				recv_attackblock();
				break;
			case 7:
				recv_buildblock();
				break;
			case 8:
				recv_weaponinfo();
				break;
			case 10:
				recv_spawn();
				break;
			case 11:
				recv_kill();
				break;
			case 12:
				recv_deathmsg();
				break;
			case 13:
				recv_chatmsg();
				break;
			case 14:
				recv_playerset();
				break;
			case 15:
				recv_weaponselect();
				break;
			case 16:
				recv_reload();
				break;
			case 18:
				recv_playerfullstats();
				break;
			case 19:
				recv_playerstats();
				break;
			case 20:
				recv_scoretime();
				break;
			case 21:
				recv_playerpoint();
				break;
			case 22:
				recv_attackspecial();
				break;
			case 24:
				recv_detonate();
				break;
			case 25:
				recv_attackblockmulti();
				break;
			case 26:
				recv_hudeffect();
				break;
			case 39:
				recv_gameend();
				break;
			case 40:
				recv_disconnect();
				break;
			case 41:
				recv_specialblock();
				break;
			case 49:
				recv_mapinfo();
				break;
			case 50:
				recv_zchunk();
				break;
			case 51:
				recv_zchunk_finish();
				break;
			case 52:
				recv_netinfo();
				break;
			case 54:
				recv_custom_admin();
				break;
			case 55:
				recv_custom_log();
				break;
			case 56:
				recv_custom_maplist();
				break;
			case 58:
				recv_clientdisconnect();
				break;
			case 59:
				recv_restorepos();
				break;
			case 60:
				recv_damageshield();
				break;
			case 62:
				recv_blockslot();
				break;
			case 63:
				recv_gamemode();
				break;
			case 64:
				recv_devmsg();
				break;
			case 65:
				recv_msg();
				break;
			case 66:
				recv_spectator();
				break;
			case 67:
				recv_freezetime();
				break;
			case 68:
				recv_resetspecial();
				break;
			case 69:
				recv_blocklimit();
				break;
			case 70:
				recv_blockammo();
				break;
			case 71:
				recv_zmmessage();
				break;
			case 72:
				recv_setzombie();
				break;
			case 73:
				recv_forceweapon();
				break;
			case 74:
				recv_mapinfo2();
				break;
			case 75:
				recv_healthbig();
				break;
			case 76:
				recv_deathmsg2();
				break;
			case 78:
				recv_callvote();
				break;
			case 79:
				recv_callvote_result();
				break;
			case 80:
				recv_result();
				break;
			case 81:
				recv_clienttime();
				break;
			case 82:
				recv_clienttimeres();
				break;
			case 83:
				recv_blockfreeze();
				break;
			case 84:
				recv_replaceweapon();
				break;
			case 87:
				recv_damagethrow();
				break;
			case 88:
				recv_gg_nextweapon();
				break;
			case 89:
				recv_gg_playsound();
				break;
			case 90:
				recv_gg_winner();
				break;
			case 91:
				recv_gg_frags();
				break;
			case 92:
				recv_attack_repeat();
				break;
			case 93:
				recv_hiddeninfo();
				break;
			case 94:
				recv_dropanimation();
				break;
			case 95:
				recv_forceselect();
				break;
			case 9:
			case 17:
			case 23:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 42:
			case 43:
			case 44:
			case 45:
			case 46:
			case 47:
			case 48:
			case 53:
			case 57:
			case 61:
			case 77:
			case 85:
			case 86:
				break;
			}
		}
	}

	public void cl_send()
	{
		if (!Active || client == null || !client.Connected)
		{
			return;
		}
		NetworkStream stream = client.GetStream();
		try
		{
			stream.Write(NET.WRITE_DATA(), 0, NET.WRITE_LEN());
		}
		catch (Exception ex)
		{
			Debug.Log("Exception | host dropped connection (" + ex.Message + ")");
			Active = false;
		}
	}

	public void send_auth()
	{
		NET.BEGIN_WRITE(245, 0);
		NET.WRITE_LONG(GUIOptions.gid);
		NET.WRITE_STRING(GUIOptions.authkey);
		NET.WRITE_STRING(GUIOptions.playername);
		NET.WRITE_BYTE((byte)Controll.pl.cp[0]);
		NET.WRITE_BYTE((byte)Controll.pl.cp[1]);
		NET.WRITE_BYTE((byte)Controll.pl.cp[2]);
		NET.WRITE_BYTE((byte)Controll.pl.cp[3]);
		NET.WRITE_BYTE((byte)Controll.pl.cp[4]);
		NET.WRITE_BYTE((byte)Controll.pl.cp[5]);
		NET.WRITE_BYTE((byte)Controll.pl.cp[6]);
		NET.WRITE_LONG(GUIOptions.exp);
		NET.WRITE_BYTE((byte)Controll.pl.cp[7]);
		NET.WRITE_BYTE((byte)Controll.pl.cp[8]);
		NET.WRITE_BYTE((byte)Controll.pl.cp[9]);
		NET.WRITE_STRING(GUIProfile.hash_stats);
		NET.WRITE_STRING(GUIOptions.namekey);
		NET.WRITE_LONG(GUIClan.clanid);
		NET.WRITE_STRING(GUIClan.clanname);
		NET.WRITE_STRING(GUIClan.clankey);
		if (GP.auth)
		{
			NET.WRITE_BYTE(1);
		}
		else
		{
			NET.WRITE_BYTE(0);
		}
		NET.WRITE_STRING(MasterClient.vk_id);
		NET.WRITE_STRING(MasterClient.acckey);
		NET.END_WRITE();
		cl_send();
	}

	public void send_changeteam(int team)
	{
		NET.BEGIN_WRITE(245, 2);
		NET.WRITE_BYTE((byte)team);
		NET.END_WRITE();
		cl_send();
	}

	public void send_attack(Vector3 pos, uint time, List<AttackData> adlist)
	{
		NET.BEGIN_WRITE(245, 4);
		if (adlist != null && adlist.Count != 0)
		{
			NET.WRITE_FLOAT(pos.x);
			NET.WRITE_FLOAT(pos.y);
			NET.WRITE_FLOAT(pos.z);
			NET.WRITE_LONG((int)time);
			for (int i = 0; i < adlist.Count; i++)
			{
				NET.WRITE_BYTE((byte)adlist[i].vid);
				NET.WRITE_BYTE((byte)adlist[i].hitbox);
				NET.WRITE_FLOAT(adlist[i].hitpos.x);
				NET.WRITE_FLOAT(adlist[i].hitpos.y);
				NET.WRITE_FLOAT(adlist[i].hitpos.z);
			}
		}
		NET.END_WRITE();
		cl_send();
	}

	public void send_attackblock(int x, int y, int z)
	{
		NET.BEGIN_WRITE(245, 6);
		NET.WRITE_SHORT((short)x);
		NET.WRITE_SHORT((short)y);
		NET.WRITE_SHORT((short)z);
		NET.END_WRITE();
		cl_send();
	}

	public void send_buildblock(int x, int y, int z)
	{
		NET.BEGIN_WRITE(245, 7);
		NET.WRITE_SHORT((short)x);
		NET.WRITE_SHORT((short)y);
		NET.WRITE_SHORT((short)z);
		NET.END_WRITE();
		cl_send();
	}

	public void send_weaponinfo(WeaponInfo wi)
	{
		NET.BEGIN_WRITE(245, 8);
		NET.WRITE_LONG(wi.id);
		NET.WRITE_STRING(wi.name);
		NET.WRITE_STRING(wi.fullname);
		NET.WRITE_BYTE((byte)wi.ammo);
		NET.WRITE_SHORT((short)wi.backpack);
		NET.WRITE_BYTE((byte)wi.damage);
		NET.WRITE_SHORT((short)wi.firerate);
		NET.WRITE_BYTE((byte)wi.distance);
		NET.WRITE_BYTE((byte)wi.accuracy);
		NET.WRITE_SHORT((short)wi.reload);
		NET.WRITE_BYTE((byte)wi.recoil);
		NET.WRITE_BYTE((byte)wi.piercing);
		NET.WRITE_BYTE((byte)wi.mobility);
		NET.WRITE_BYTE((byte)wi.slot);
		NET.WRITE_STRING(wi.key);
		NET.END_WRITE();
		cl_send();
	}

	public void send_weapondata(List<WeaponInv> awlist)
	{
		NET.BEGIN_WRITE(245, 9);
		NET.WRITE_BYTE((byte)awlist.Count);
		for (int i = 0; i < awlist.Count; i++)
		{
			NET.WRITE_LONG((int)awlist[i].uid);
			NET.WRITE_SHORT((short)awlist[i].wi.id);
			NET.WRITE_BYTE((byte)awlist[i].scopeid);
			NET.WRITE_BYTE((byte)awlist[i].GetLevel());
			NET.WRITE_BYTE(awlist[i].mod[0]);
			NET.WRITE_BYTE(awlist[i].mod[1]);
			NET.WRITE_BYTE(awlist[i].mod[2]);
		}
		for (int j = 0; j < 3; j++)
		{
			for (int k = 0; k < 5; k++)
			{
				if (GUIInv.wset[j].w[k] == null)
				{
					NET.WRITE_LONG(0);
				}
				else
				{
					NET.WRITE_LONG((int)GUIInv.wset[j].w[k].uid);
				}
			}
		}
		NET.END_WRITE();
		cl_send();
	}

	public void send_weapondata_steam(List<WeaponInv> awlist)
	{
		NET.BEGIN_WRITE(245, 9);
		NET.WRITE_BYTE((byte)awlist.Count);
		for (int i = 0; i < awlist.Count; i++)
		{
			NET.WRITE_ULONG64(awlist[i].uid);
			NET.WRITE_SHORT((short)awlist[i].wi.id);
			NET.WRITE_BYTE((byte)awlist[i].scopeid);
			NET.WRITE_BYTE((byte)awlist[i].GetLevel());
			NET.WRITE_BYTE(awlist[i].mod[0]);
			NET.WRITE_BYTE(awlist[i].mod[1]);
			NET.WRITE_BYTE(awlist[i].mod[2]);
		}
		for (int j = 0; j < 3; j++)
		{
			for (int k = 0; k < 5; k++)
			{
				if (GUIInv.wset[j].w[k] == null)
				{
					NET.WRITE_ULONG64(0uL);
				}
				else
				{
					NET.WRITE_ULONG64(GUIInv.wset[j].w[k].uid);
				}
			}
		}
		NET.END_WRITE();
		cl_send();
	}

	public void send_playerset(int set)
	{
		NET.BEGIN_WRITE(245, 14);
		NET.WRITE_BYTE((byte)set);
		NET.END_WRITE();
		cl_send();
	}

	public void send_chatmsg(int teamchat, string msg)
	{
		NET.BEGIN_WRITE(245, 13);
		NET.WRITE_BYTE((byte)teamchat);
		NET.WRITE_STRING(msg);
		NET.END_WRITE();
		cl_send();
	}

	public void send_weaponselect(int slot)
	{
		NET.BEGIN_WRITE(245, 15);
		NET.WRITE_BYTE((byte)slot);
		NET.END_WRITE();
		cl_send();
	}

	public void send_reload()
	{
		NET.BEGIN_WRITE(245, 16);
		NET.END_WRITE();
		cl_send();
	}

	public void send_reloadactive()
	{
		NET.BEGIN_WRITE(245, 17);
		NET.END_WRITE();
		cl_send();
	}

	public void send_pos_dev(float x, float y, float z, float rx, float ry, byte bitmask, uint time)
	{
		if (Controll.pl.idx >= 0)
		{
			NET.BEGIN_WRITE(245, 45);
			NET.WRITE_FLOAT(x);
			NET.WRITE_FLOAT(y);
			NET.WRITE_FLOAT(z);
			NET.WRITE_FLOAT(rx);
			NET.WRITE_FLOAT(ry);
			NET.WRITE_BYTE(bitmask);
			NET.WRITE_LONG((int)time);
			NET.END_WRITE();
			cl_send();
		}
	}

	public void send_attackspecial(Vector3 pos, Vector3 force, float rotation)
	{
		NET.BEGIN_WRITE(245, 22);
		NET.WRITE_FLOAT(pos.x);
		NET.WRITE_FLOAT(pos.y);
		NET.WRITE_FLOAT(pos.z);
		NET.WRITE_FLOAT(force.x);
		NET.WRITE_FLOAT(force.y);
		NET.WRITE_FLOAT(force.z);
		NET.WRITE_FLOAT(rotation);
		NET.END_WRITE();
		cl_send();
	}

	public void send_entpos(int uid, Vector3 pos)
	{
		NET.BEGIN_WRITE(245, 23);
		NET.WRITE_LONG(uid);
		NET.WRITE_FLOAT(pos.x);
		NET.WRITE_FLOAT(pos.y);
		NET.WRITE_FLOAT(pos.z);
		NET.END_WRITE();
		cl_send();
	}

	public void send_cmd(string cmd)
	{
		NET.BEGIN_WRITE(245, 27);
		NET.WRITE_STRING(cmd);
		NET.END_WRITE();
		cl_send();
	}

	public void send_file_start(int filetype, string filename, int filesize)
	{
		NET.BEGIN_WRITE(245, 53);
		NET.WRITE_BYTE(0);
		NET.WRITE_BYTE((byte)filetype);
		NET.WRITE_STRING(filename);
		NET.WRITE_LONG(filesize);
		NET.END_WRITE();
		cl_send();
	}

	public void send_file_continue(byte[] b, int l)
	{
		NET.BEGIN_WRITE(245, 53);
		NET.WRITE_BYTE(1);
		for (int i = 0; i < l; i++)
		{
			NET.WRITE_BYTE(b[i]);
		}
		NET.END_WRITE();
		cl_send();
	}

	public void send_file_end()
	{
		NET.BEGIN_WRITE(245, 53);
		NET.WRITE_BYTE(2);
		NET.END_WRITE();
		cl_send();
	}

	public void send_customop(int cmd, int index, int param)
	{
		NET.BEGIN_WRITE(245, 57);
		NET.WRITE_BYTE((byte)cmd);
		NET.WRITE_BYTE((byte)index);
		NET.WRITE_BYTE((byte)param);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clc(int c)
	{
		NET.BEGIN_WRITE(245, 61);
		NET.WRITE_BYTE((byte)c);
		NET.END_WRITE();
		cl_send();
	}

	public void send_spectator()
	{
		NET.BEGIN_WRITE(245, 66);
		NET.END_WRITE();
		cl_send();
	}

	public void send_callvote(int code, int id)
	{
		NET.BEGIN_WRITE(245, 78);
		NET.WRITE_BYTE((byte)code);
		NET.WRITE_BYTE((byte)id);
		NET.END_WRITE();
		cl_send();
	}

	public void send_result(int c, string s)
	{
		NET.BEGIN_WRITE(245, 80);
		NET.WRITE_BYTE((byte)c);
		NET.WRITE_STRING(s);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clienttime(int p0, uint p1, string p2)
	{
		NET.BEGIN_WRITE(245, 81);
		NET.WRITE_BYTE((byte)p0);
		NET.WRITE_LONG((int)p1);
		NET.WRITE_STRING(p2);
		NET.END_WRITE();
		cl_send();
	}

	public void send_clienttimeres(int p0, string p1, string p2)
	{
		NET.BEGIN_WRITE(245, 82);
		NET.WRITE_BYTE((byte)p0);
		NET.WRITE_STRING(p1);
		NET.WRITE_STRING(p2);
		NET.END_WRITE();
		cl_send();
	}

	public void send_attackthrow(Vector3 pos, Vector3 force, float rotation)
	{
		NET.BEGIN_WRITE(245, 85);
		NET.WRITE_FLOAT(pos.x);
		NET.WRITE_FLOAT(pos.y);
		NET.WRITE_FLOAT(pos.z);
		NET.WRITE_FLOAT(force.x);
		NET.WRITE_FLOAT(force.y);
		NET.WRITE_FLOAT(force.z);
		NET.WRITE_FLOAT(rotation);
		NET.END_WRITE();
		cl_send();
	}

	public void send_receivethrow(int uid, Vector3 pos, int id)
	{
		NET.BEGIN_WRITE(245, 86);
		NET.WRITE_LONG(uid);
		NET.WRITE_FLOAT(pos.x);
		NET.WRITE_FLOAT(pos.y);
		NET.WRITE_FLOAT(pos.z);
		NET.WRITE_BYTE((byte)id);
		NET.END_WRITE();
		cl_send();
	}

	public void send_damagethrow(int p)
	{
		NET.BEGIN_WRITE(245, 87);
		NET.WRITE_BYTE((byte)p);
		NET.END_WRITE();
		cl_send();
	}

	private void recv_auth()
	{
		NET.BEGIN_READ(buffer, len, 4);
		Controll.pl.idx = NET.READ_BYTE();
		Controll.fcl_time = Time.realtimeSinceStartup;
		Controll.cl_time = Controll.GetClientTime();
		Controll.sv_time = NET.READ_ULONG();
		PLH.g_protecttime = NET.READ_BYTE();
		if (DemoRec.isDemo())
		{
			Controll.demoidx = Controll.pl.idx;
			Controll.pl.idx = -1;
		}
		PLH.Clear();
		GUIPlay.StartPoligon();
		GUIPlay.CreateControll();
		GUIMap.SPAWN();
		List<int> list = new List<int>();
		list.Clear();
		List<WeaponInv> list2 = new List<WeaponInv>();
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (GUIInv.wset[i].w[j] != null)
				{
					WeaponInfo wi = GUIInv.wset[i].w[j].wi;
					for (int k = 0; k < list.Count && list[k] != wi.id; k++)
					{
					}
					send_weaponinfo(wi);
					list.Add(wi.id);
					list2.Add(GUIInv.wset[i].w[j]);
				}
			}
		}
		if (MainManager.steam)
		{
			send_weapondata_steam(list2);
		}
		else
		{
			send_weapondata(list2);
		}
	}

	private void recv_playerinfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_BYTE();
		string text = NET.READ_STRING();
		int team = NET.READ_BYTE();
		int cp = NET.READ_BYTE();
		int cp2 = NET.READ_BYTE();
		int cp3 = NET.READ_BYTE();
		int cp4 = NET.READ_BYTE();
		int cp5 = NET.READ_BYTE();
		int cp6 = NET.READ_BYTE();
		int cp7 = NET.READ_BYTE();
		int pexp = NET.READ_LONG();
		int cp8 = NET.READ_BYTE();
		int cp9 = NET.READ_BYTE();
		int cp10 = NET.READ_BYTE();
		int cid = NET.READ_LONG();
		string cname = NET.READ_STRING();
		PLH.Add(id, text, team, cp, cp2, cp3, cp4, cp5, cp6, cp7, cp8, cp9, cp10, pexp, cid, cname);
	}

	private void recv_playerteam()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_BYTE();
		int team = NET.READ_BYTE();
		PLH.UpdateTeam(id, team);
	}

	private void recv_pos()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = (len - 4) / 10;
		PLH.CacheRealTime();
		for (int i = 0; i < num; i++)
		{
			int id = NET.READ_BYTE();
			float x = NET.READ_COORD();
			int i_coord = NET.i_coord;
			float y = NET.READ_COORD();
			int i_coord2 = NET.i_coord;
			float z = NET.READ_COORD();
			int i_coord3 = NET.i_coord;
			float rx = NET.READ_ANGLE();
			int i_angle = NET.i_angle;
			float ry = NET.READ_ANGLE();
			int i_angle2 = NET.i_angle;
			int bitmask = NET.READ_BYTE();
			PLH.Pos(id, x, y, z, rx, ry, bitmask, i_coord, i_coord2, i_coord3, i_angle, i_angle2);
		}
		if (dbgNet.cs != null)
		{
			dbgNet.cs.AddFrame();
		}
	}

	private void recv_attack()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_BYTE();
		PLH.Attack(id);
		int num = (len - 5) / 2;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				int vid = NET.READ_BYTE();
				int vbox = NET.READ_BYTE();
				PLH.AttackDamage(id, vid, vbox);
			}
		}
	}

	private void recv_health()
	{
		NET.BEGIN_READ(buffer, len, 4);
		HUD.SetHealth(NET.READ_BYTE());
	}

	private void recv_attackblock()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_SHORT();
		int num2 = NET.READ_SHORT();
		int num3 = NET.READ_SHORT();
		Map.GetBlock(num, num2, num3);
		Color lastColorFixed = Map.GetLastColorFixed();
		Map.SetBlock(num, num2, num3, Color.white, 0);
		Map.RenderBlock(num, num2, num3);
		GOP.KillbyVector(num, num2, num3);
		GOP.Create(9, new Vector3(num, num2, num3), Vector3.zero, Vector3.zero, Color.black);
		GOP.Create(3, new Vector3((float)num + 0.5f, (float)num2 + 0.5f, (float)num3 + 0.5f), Vector3.zero, Vector3.zero, lastColorFixed);
		if (DemoRec.isDemo())
		{
			DemoRec.stats_PlayerAttackBlock(num, num2, num3);
		}
	}

	private void recv_buildblock()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_SHORT();
		int num2 = NET.READ_SHORT();
		int num3 = NET.READ_SHORT();
		int c = NET.READ_BYTE();
		int flag = NET.READ_BYTE();
		if (NET.READ_ERROR())
		{
			Map.SetBlock(num, num2, num3, Color.white, 40);
		}
		else
		{
			Map.SetBlock(num, num2, num3, Palette.GetColor(c), flag);
		}
		Map.RenderBlock(num, num2, num3);
		GOP.Create(11, new Vector3(num, num2, num3), Vector3.zero, Vector3.zero, Color.black);
	}

	private void recv_weaponinfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_SHORT();
		string text = NET.READ_STRING();
		string fullname = NET.READ_STRING();
		int ammo = NET.READ_BYTE();
		int backpack = NET.READ_SHORT();
		int damage = NET.READ_BYTE();
		int firerate = NET.READ_SHORT();
		int distance = NET.READ_BYTE();
		int accuracy = NET.READ_BYTE();
		int reload = NET.READ_SHORT();
		int recoil = NET.READ_BYTE();
		int piercing = NET.READ_BYTE();
		int mobility = NET.READ_BYTE();
		int slot = NET.READ_BYTE();
		if (GUIInv.winfo[num] == null)
		{
			WeaponInfo weaponInfo = new WeaponInfo();
			weaponInfo.id = num;
			weaponInfo.name = text;
			weaponInfo.fullname = fullname;
			weaponInfo.ammo = ammo;
			weaponInfo.backpack = backpack;
			weaponInfo.damage = damage;
			weaponInfo.firerate = firerate;
			weaponInfo.distance = distance;
			weaponInfo.accuracy = accuracy;
			weaponInfo.reload = reload;
			weaponInfo.recoil = recoil;
			weaponInfo.piercing = piercing;
			weaponInfo.mobility = mobility;
			weaponInfo.slot = slot;
			weaponInfo.GenerateIcon();
			GUIInv.winfo[weaponInfo.id] = weaponInfo;
		}
	}

	private void recv_spawn()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_BYTE();
		int x = NET.READ_SHORT();
		int y = NET.READ_SHORT();
		int z = NET.READ_SHORT();
		float ry = NET.READ_ANGLE();
		int weaponset = NET.READ_BYTE();
		PLH.Spawn(id, x, y, z, ry, weaponset);
	}

	private void recv_kill()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_BYTE();
		int aid = NET.READ_BYTE();
		int hitbox = NET.READ_BYTE();
		PLH.Kill(id, aid, hitbox);
	}

	private void recv_deathmsg()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		int slot = NET.READ_BYTE();
		int num3 = NET.READ_BYTE();
		HUDMessage.AddDeath(num, num2, slot, num3);
		if (num == Controll.pl.idx && num != num2)
		{
			HUD.AddMedal(num3);
			Controll.pl.fragcount[num2]++;
		}
		if (num2 == Controll.pl.idx && num != Controll.pl.idx)
		{
			HUDKiller.cs.Set(num, num2, slot);
		}
	}

	private void recv_chatmsg()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_BYTE();
		int teamchat = NET.READ_BYTE();
		string msg = NET.READ_STRING();
		HUDMessage.AddChat(id, msg, teamchat);
	}

	private void recv_playerset()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		if (PLH.player[num] == null)
		{
			return;
		}
		PLH.player[num].wset[num2] = new WeaponSet(num2);
		for (int i = 0; i < 5; i++)
		{
			int num3 = NET.READ_SHORT();
			if (num3 != 0)
			{
				string text = NET.READ_STRING();
				string fullname = NET.READ_STRING();
				int slot = NET.READ_BYTE();
				WeaponInfo weaponInfo = new WeaponInfo();
				weaponInfo.id = num3;
				weaponInfo.name = text;
				weaponInfo.fullname = fullname;
				weaponInfo.slot = slot;
				int scopeid = NET.READ_BYTE();
				int level = NET.READ_BYTE();
				int num4 = NET.READ_BYTE();
				int num5 = NET.READ_BYTE();
				int num6 = NET.READ_BYTE();
				WeaponInv weaponInv = new WeaponInv(0uL, weaponInfo);
				weaponInv.level = level;
				weaponInv.mod[0] = (byte)num4;
				weaponInv.mod[1] = (byte)num5;
				weaponInv.mod[2] = (byte)num6;
				weaponInv.scopeid = scopeid;
				PLH.player[num].wset[num2].w[i] = weaponInv;
			}
		}
	}

	private void recv_weaponselect()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		if (PLH.player[num] == null || PLH.player[num] == Controll.pl)
		{
			return;
		}
		PlayerData playerData = PLH.player[num];
		WeaponInv weaponInv = null;
		if (playerData.rw[num2] > 0)
		{
			weaponInv = playerData.GetWeaponInvExtra(playerData.rw[num2]);
		}
		else
		{
			if (playerData.wset[playerData.currset] == null || playerData.wset[playerData.currset].w[num2] == null)
			{
				return;
			}
			weaponInv = playerData.wset[playerData.currset].w[num2];
		}
		if (weaponInv != null)
		{
			PLH.SelectWeapon(PLH.player[num], weaponInv.wi.name, true);
		}
	}

	private void recv_reload()
	{
		NET.BEGIN_READ(buffer, len, 4);
		Controll.ReloadWeaponEnd(NET.READ_BYTE());
	}

	private void recv_playerfullstats()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int f = NET.READ_SHORT();
		int d = NET.READ_SHORT();
		int a = NET.READ_SHORT();
		int exp = NET.READ_SHORT();
		if (PLH.player[num] != null)
		{
			PlayerData playerData = PLH.player[num];
			playerData.f = f;
			playerData.d = d;
			playerData.a = a;
			playerData.exp = exp;
			playerData.sFDA = f + " / " + d + " / " + a;
			playerData.sEXP = exp.ToString();
		}
	}

	private void recv_playerstats()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		int num3 = NET.READ_SHORT();
		int exp = NET.READ_SHORT();
		if (PLH.player[num] == null)
		{
			return;
		}
		PlayerData playerData = PLH.player[num];
		switch (num2)
		{
		case 0:
			playerData.f = num3;
			break;
		case 1:
			playerData.d = num3;
			break;
		case 2:
			playerData.a = num3;
			break;
		}
		playerData.exp = exp;
		playerData.sFDA = playerData.f + " / " + playerData.d + " / " + playerData.a;
		playerData.sEXP = exp.ToString();
		if (DemoRec.isDemo())
		{
			switch (num2)
			{
			case 0:
				DemoRec.stats_PlayerStatsFrag(playerData.name, playerData.f);
				break;
			case 1:
				DemoRec.stats_PlayerStatsDeaths(playerData.name, playerData.d);
				break;
			}
		}
	}

	private void recv_scoretime()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int score = NET.READ_SHORT();
		int score2 = NET.READ_SHORT();
		int sec = NET.READ_SHORT();
		HUD.SetScoreTime(score, score2, sec);
	}

	private void recv_playerpoint()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int msgid = NET.READ_BYTE();
		int pts = NET.READ_BYTE();
		HUDMessage.AddPoint(msgid, pts);
	}

	private void recv_attackspecial()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int iparam = NET.READ_BYTE();
		int iparam2 = NET.READ_LONG();
		int num = NET.READ_BYTE();
		float x = NET.READ_FLOAT();
		float y = NET.READ_FLOAT();
		float z = NET.READ_FLOAT();
		float x2 = NET.READ_FLOAT();
		float y2 = NET.READ_FLOAT();
		float z2 = NET.READ_FLOAT();
		float num2 = NET.READ_FLOAT();
		switch (num)
		{
		case 0:
			GOP.Create(12, new Vector3(x, y, z), new Vector3(0f, num2, 90f), new Vector3(x2, y2, z2), Color.black, iparam2, iparam);
			break;
		case 1:
			GOP.Create(13, new Vector3(x, y, z), new Vector3(0f, num2, 0f), new Vector3(x2, y2, z2), Color.black, iparam2, iparam);
			break;
		case 2:
			GOP.Create(14, new Vector3(x, y, z), new Vector3(0f, num2, 0f), new Vector3(x2, y2, z2), Color.black, iparam2, iparam);
			break;
		case 3:
			GOP.Create(23, new Vector3(x, y, z), new Vector3(0f, num2, 0f), new Vector3(x2, y2, z2), Color.black, iparam2, iparam);
			break;
		case 4:
			GOP.Create(26, new Vector3(x, y, z), new Vector3(0f, num2 + 90f, 0f), new Vector3(x2, y2, z2), Color.black, iparam2, iparam);
			break;
		case 5:
			GOP.Create(29, new Vector3(x, y, z), new Vector3(90f, num2, 0f), new Vector3(x2, y2, z2), Color.black, iparam2, iparam);
			break;
		case 7:
			GOP.Create(27, new Vector3(x, y, z), new Vector3(0f, num2, 0f), new Vector3(x2, y2, z2), Color.black, iparam2, iparam);
			break;
		}
	}

	private void recv_detonate()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int uid = NET.READ_LONG();
		float x = NET.READ_FLOAT();
		float y = NET.READ_FLOAT();
		float z = NET.READ_FLOAT();
		GOP.KillbyUID(uid);
		Vector3 b = new Vector3(x, y, z);
		GOP.Create(15, new Vector3(x, y, z), Vector3.zero, Vector3.zero, Color.black);
		float num = Vector3.Distance(Controll.currPos, b);
		if (num < 32f)
		{
			VWIK.SetCameraShake((num < 16f) ? 0.4f : 0.2f, 1f);
		}
	}

	private void recv_attackblockmulti()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = (len - 4) / 3;
		for (int i = 0; i < num; i++)
		{
			int num2 = NET.READ_BYTE();
			int num3 = NET.READ_BYTE();
			int num4 = NET.READ_BYTE();
			Map.GetBlock(num2, num3, num4);
			Color lastColorFixed = Map.GetLastColorFixed();
			Map.SetBlock(num2, num3, num4, Color.white, 0);
			if (num == 1)
			{
				Map.RenderBlock(num2, num3, num4);
			}
			else
			{
				Map.SetBlockNearDirtyUpdate(num2, num3, num4);
			}
			GOP.Create(9, new Vector3(num2, num3, num4), Vector3.zero, Vector3.zero, Color.black);
			GOP.Create(3, new Vector3((float)num2 + 0.5f, (float)num3 + 0.5f, (float)num4 + 0.5f), Vector3.zero, Vector3.zero, lastColorFixed);
		}
		if (num != 1)
		{
			Map.RenderDirty();
		}
	}

	private void recv_hudeffect()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		int num3 = NET.READ_BYTE();
		if (DemoRec.isDemo())
		{
			return;
		}
		switch (num)
		{
		case 0:
			HUD.SetFade(Color.green, 0.1f, 0.1f);
			HUD.SetHealth(num3);
			Controll.pl.asFX.PlayOneShot(HUD.sMedkit);
			break;
		case 1:
			if (Controll.pl.wset[Controll.pl.currset].w[num2] != null)
			{
				WeaponInv obj = Controll.pl.wset[Controll.pl.currset].w[num2];
				HUD.SetFade(Color.yellow, 0.1f, 0.1f);
				obj.backpack += num3;
				HUD.SetBackPack(obj.backpack);
				Controll.pl.asFX.PlayOneShot(HUD.sAmmokit);
			}
			break;
		}
	}

	private void recv_gameend()
	{
		Controll.SetLockMove(true);
		Controll.SetLockAttack(true);
		Controll.SetLockLook(true);
		HUDGameEnd.SetActive(true);
		Controll.UnZoom();
	}

	private void recv_disconnect()
	{
		NET.BEGIN_READ(buffer, len, 4);
		PLH.Delete(NET.READ_BYTE());
	}

	private void recv_specialblock()
	{
		int num = (len - 4) / 3;
		Controll.sblock = new SpecialBlock[num];
		NET.BEGIN_READ(buffer, len, 4);
		for (int i = 0; i < num; i++)
		{
			int x = NET.READ_BYTE();
			int y = NET.READ_BYTE();
			int z = NET.READ_BYTE();
			Controll.sblock[i] = new SpecialBlock(x, y, z);
		}
	}

	private void recv_mapinfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int x = NET.READ_BYTE();
		int y = NET.READ_BYTE();
		int z = NET.READ_BYTE();
		string text = NET.READ_STRING();
		NET.READ_STRING();
		Map.Clear();
		Map.Create(x, y, z);
		cc = 0;
		cclen = 0;
		if (!(Controll.trControll != null))
		{
			return;
		}
		bool flag = false;
		if (Main.winter)
		{
			UnityEngine.Random.seed = PORT + text.GetHashCode();
			if (UnityEngine.Random.Range(0, 3) == 0)
			{
				flag = true;
			}
		}
		if (text == "map17" || flag)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("prefabs/gamesnow") as GameObject);
			obj.transform.parent = Controll.trControll;
			obj.transform.localPosition = new Vector3(0f, 32f, 0f);
		}
	}

	private void recv_zchunk()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		int num3 = NET.READ_BYTE();
		int num4 = len - 7;
		Map.zs = new byte[num4];
		for (int i = 0; i < num4; i++)
		{
			Map.zs[i] = (byte)NET.READ_BYTE();
		}
		Map.skipplayerclip = true;
		Map.UncompressAndSetBlock(num * 16, num2 * 16, num3 * 16, true);
		Map.skipplayerclip = false;
		Map.zs = null;
	}

	private void recv_zchunk_finish()
	{
		NET.BEGIN_READ(buffer, len, 4);
		if (GUIOptions.directlight == 0)
		{
			MapLight.GenerateLight();
		}
		Map.RenderAll();
		if (Main.winter)
		{
			MapEvent.Init();
			MapEvent.Set();
		}
		Radar.SetActive(true);
		Radar.GenerateRadar();
		Map.LoadBackground();
		DateTime dateTime = new DateTime(2017, 10, 31);
		if (DateTime.Today == dateTime)
		{
			Console.cs.Command("sun " + UnityEngine.Random.Range(340, 350) + " " + UnityEngine.Random.Range(0, 360));
		}
	}

	private void recv_netinfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		uint num = NET.READ_ULONG();
		uint sv_time = NET.READ_ULONG();
		Controll.ping = Controll.GetServerTime() - num;
		Controll.fcl_time = Time.realtimeSinceStartup;
		Controll.cl_time = Controll.GetClientTime();
		Controll.sv_time = sv_time;
	}

	private void recv_custom_admin()
	{
		NET.BEGIN_READ(buffer, len, 4);
		if (NET.READ_BYTE() == Controll.pl.idx)
		{
			if (GUIAdmin.cs == null)
			{
				GameObject.Find("GUI").AddComponent<GUIAdmin>().showhint = true;
			}
			else
			{
				GUIAdmin.cs.showhint = true;
			}
		}
	}

	private void recv_custom_log()
	{
	}

	private void recv_custom_maplist()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		switch (num)
		{
		case 0:
			if (GUIAdminUpload.maplist_sv == null)
			{
				GUIAdminUpload.maplist_sv = new List<MapData>();
			}
			GUIAdminUpload.maplist_sv.Clear();
			break;
		case 1:
			if (GUIAdminMaplist.offmaplist == null)
			{
				GUIAdminMaplist.offmaplist = new List<MapData>();
			}
			GUIAdminMaplist.offmaplist.Clear();
			break;
		case 2:
			if (GUIAdminMaplist.rotmaplist == null)
			{
				GUIAdminMaplist.rotmaplist = new List<MapData>();
			}
			GUIAdminMaplist.rotmaplist.Clear();
			break;
		}
		for (int i = 0; i < num2; i++)
		{
			string title = NET.READ_STRING();
			switch (num)
			{
			case 0:
				GUIAdminUpload.maplist_sv.Add(new MapData("null", "null", "null", title, "null", i));
				break;
			case 1:
				GUIAdminMaplist.offmaplist.Add(new MapData("null", "null", "null", title, "null", i));
				break;
			case 2:
				GUIAdminMaplist.rotmaplist.Add(new MapData("null", "null", "null", title, "null", i));
				break;
			}
		}
		if (GUIAdminUpload.cs != null)
		{
			GUIAdminUpload.cs.OnResize();
		}
		if (GUIAdminMaplist.cs != null)
		{
			GUIAdminMaplist.cs.OnResize();
		}
	}

	private void recv_clientdisconnect()
	{
		NET.BEGIN_READ(buffer, len, 4);
		NET.READ_BYTE();
		GUIGameExit.ExitMainMenu();
	}

	private void recv_restorepos()
	{
		NET.BEGIN_READ(buffer, len, 4);
		float x = NET.READ_COORD();
		float num = NET.READ_COORD();
		float z = NET.READ_COORD();
		Controll.pl.Pos.x = x;
		Controll.pl.Pos.y = num + Controll.camheight;
		Controll.pl.Pos.z = z;
		Controll.pl.prevPos = Controll.pl.Pos;
		Controll.velocity = Vector3.zero;
		Controll.vel = Vector3.zero;
		Controll.SetPos(Controll.pl.Pos - Vector3.up * Controll.camheight);
		Controll.cs.CheckMove();
	}

	private void recv_damageshield()
	{
		NET.BEGIN_READ(buffer, len, 4);
		if (NET.READ_BYTE() == Controll.pl.idx)
		{
			HUD.show_armor = false;
			HUD.SetFade(Color.blue, 0.3f, 0.5f);
			return;
		}
		if (Controll.gamemode == 6)
		{
			if (Controll.sSnowmark == null)
			{
				Controll.sSnowmark = Resources.Load("sounds/snowhit") as AudioClip;
			}
			Controll.pl.asFX.PlayOneShot(Controll.sSnowmark);
		}
		Crosshair.SetMark(Color.blue);
	}

	private void recv_blockslot()
	{
		NET.BEGIN_READ(buffer, len, 4);
		Controll.blockslot = NET.READ_BYTE();
	}

	private void recv_gamemode()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = (Controll.gamemode = NET.READ_BYTE());
		HUD.SetMessage(GUIPlay.sGameMode[num], 10);
		if (Controll.gamemode == 2 || Controll.gamemode == 5 || Controll.gamemode == 8)
		{
			HUDTab.cs.SetScoreBar(1);
		}
		if (Controll.gamemode == 5)
		{
			HUD.maxhealth = 5000f;
		}
		else
		{
			HUD.maxhealth = 100f;
		}
		HUD.OnResize_Health();
	}

	private void recv_devmsg()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int sec = NET.READ_LONG();
		int fragscount = NET.READ_LONG();
		int livetime = NET.READ_LONG();
		string killername = NET.READ_STRING();
		Main.SetDevMsg(sec, fragscount, livetime, killername);
	}

	private void recv_msg()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		int num3 = NET.READ_BYTE();
		if (num == 0)
		{
			HUD.SetMessage2(GUIPlay.sRound + " " + num2);
		}
		if (num == 1)
		{
			if (num2 == 1)
			{
				HUD.SetMessage(GUIPlay.sRoundWon, 5);
				if (num3 == 0)
				{
					HUD.SetMessage2(GUIPlay.sEnemyEliminated);
				}
			}
			else if (num3 == 0)
			{
				HUD.SetMessage(GUIPlay.sRoundLose, 5);
			}
			if (num3 == 1)
			{
				HUD.SetMessage2(GUIPlay.sRountTimeEnd);
			}
		}
		if (num == 2)
		{
			switch (num2)
			{
			case 0:
				HUD.SetMessage(GUIPlay.sHumansWin, 5);
				break;
			case 1:
				HUD.SetMessage(GUIPlay.sZombiesWin, 5);
				break;
			}
			if (num3 == 1)
			{
				HUD.SetMessage2(GUIPlay.sRountTimeEnd);
			}
		}
	}

	private void recv_spectator()
	{
		NET.BEGIN_READ(buffer, len, 4);
		Controll.SetCamMode(NET.READ_BYTE());
	}

	private void recv_freezetime()
	{
		NET.BEGIN_READ(buffer, len, 4);
		PLH.g_freezetime = NET.READ_BYTE();
	}

	private void recv_resetspecial()
	{
		NET.BEGIN_READ(buffer, len, 4);
		Controll.specialtime_end = 0f;
	}

	private void recv_blocklimit()
	{
		NET.BEGIN_READ(buffer, len, 4);
		PLH.g_blocklimit = NET.READ_BYTE();
	}

	private void recv_blockammo()
	{
		NET.BEGIN_READ(buffer, len, 4);
	}

	private void recv_zmmessage()
	{
		NET.BEGIN_READ(buffer, len, 4);
		NET.READ_BYTE();
		HUD.cs.SetZMessage(0);
	}

	private void recv_setzombie()
	{
		NET.BEGIN_READ(buffer, len, 4);
		PLH.SetZombie(NET.READ_BYTE());
	}

	private void recv_forceweapon()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_BYTE();
		int wid = NET.READ_SHORT();
		PLH.ForceSelectWeapon(id, wid);
	}

	private void recv_mapinfo2()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		int num3 = NET.READ_BYTE();
		int num4 = NET.READ_BYTE();
		int num5 = NET.READ_BYTE();
		int custom_fogstart = NET.READ_SHORT();
		int custom_fogend = NET.READ_SHORT();
		float custom_intensity = (float)NET.READ_BYTE() / 255f;
		GOpt.custom_render = true;
		GOpt.custom_fog = new Color((float)num3 / 255f, (float)num4 / 255f, (float)num5 / 255f, 1f);
		GOpt.custom_fogstart = custom_fogstart;
		GOpt.custom_fogend = custom_fogend;
		GOpt.custom_intensity = custom_intensity;
		Console.cs.Command("sun " + num + " " + num2);
		GOpt.SetDistance();
	}

	private void recv_healthbig()
	{
		NET.BEGIN_READ(buffer, len, 4);
		HUD.SetHealth(NET.READ_SHORT());
	}

	private void recv_deathmsg2()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int aid = NET.READ_BYTE();
		int vid = NET.READ_BYTE();
		int wid = NET.READ_SHORT();
		int hs = NET.READ_BYTE();
		HUDMessage.AddDeath2(aid, vid, wid, hs);
	}

	private void recv_callvote()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int code = NET.READ_BYTE();
		int id = NET.READ_BYTE();
		HUD.SetCallVote(code, id);
	}

	private void recv_callvote_result()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int v = NET.READ_BYTE();
		int v2 = NET.READ_BYTE();
		HUD.SetCallVoteResult(v, v2);
	}

	private void recv_result()
	{
	}

	private void recv_clienttime()
	{
	}

	private void recv_clienttimeres()
	{
	}

	private void recv_blockfreeze()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		if (PLH.player[num] == null || PLH.player[num].go == null)
		{
			return;
		}
		if ((bool)PLH.player[num].goAttach)
		{
			UnityEngine.Object.Destroy(PLH.player[num].goAttach);
			if (Controll.pl.idx == num)
			{
				Controll.inFreeze = false;
			}
		}
		if (num2 > 0)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Controll.pgoFreezeBlock[num2 - 1]);
			gameObject.name = "f_" + gameObject.GetInstanceID();
			gameObject.transform.parent = PLH.player[num].go.transform;
			gameObject.transform.localPosition = new Vector3(0f, -2.1f, 0f);
			PLH.player[num].goAttach = gameObject;
			if (Controll.pl.idx == num)
			{
				Controll.inFreeze = true;
				Controll.tFreeze = Time.time + 999f;
			}
		}
	}

	private void recv_replaceweapon()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_SHORT();
		int num3 = NET.READ_BYTE();
		int scopeid = NET.READ_BYTE();
		if (GUIInv.winfo[num2] == null)
		{
			return;
		}
		int slot = GUIInv.winfo[num2].slot;
		if (PLH.player[num] == null)
		{
			return;
		}
		if (num3 > 0)
		{
			PLH.player[num].rw[slot] = num2;
			PLH.BuildWeaponExtra(PLH.player[num], num2, scopeid);
			if (num == Controll.pl.idx)
			{
				PLH.RefillWeapons();
				Controll.ChangeWeapon(0);
			}
		}
		else
		{
			PLH.player[num].rw[slot] = 0;
		}
	}

	private void recv_damagethrow()
	{
	}

	private void recv_gg_nextweapon()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_SHORT();
		int num2 = NET.READ_BYTE();
		int num3 = NET.READ_BYTE();
		HUD.ggrank = "LEVEL " + num2 + "/" + num3;
		HUD.ggw = GUIInv.winfo[num];
		HUD.iggrank = num2;
		HUD.iggmaxrank = num3;
	}

	private void recv_gg_playsound()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int sid = NET.READ_BYTE();
		HUD.cs.PlayCustomSound(sid);
	}

	private void recv_gg_winner()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		recv_gameend();
		HUDGameEnd.sWin = HUDGameEnd.sWon + " " + ((PLH.player[num] == null) ? ("PLAYER " + num) : PLH.player[num].name);
	}

	private void recv_gg_frags()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int num2 = NET.READ_BYTE();
		HUD.ggfrags = "FRAGS " + num + "/" + num2;
	}

	private void recv_attack_repeat()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_BYTE();
		int vid = NET.READ_BYTE();
		int vbox = NET.READ_BYTE();
		int bloodid = NET.READ_BYTE();
		int soundid = NET.READ_BYTE();
		int affectid = NET.READ_BYTE();
		PLH.AttackDamageRepeat(id, vid, vbox, bloodid, soundid, affectid);
	}

	private void recv_hiddeninfo()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		int gid = NET.READ_LONG();
		int net = NET.READ_BYTE();
		if (DemoRec.isDemo() && PLH.player[num] != null)
		{
			DemoRec.stats_AddPlayerHiddenInfo(PLH.player[num].name, gid, net);
		}
	}

	private void recv_dropanimation()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int num = NET.READ_BYTE();
		NET.READ_SHORT();
		if (Controll.pl.idx == num)
		{
			VWIK.SetDrop();
			VWIK.AddAngle(new Vector3(45f, 0f, 0f), Time.time + 0.4f, 0.2f);
			GOP.Create(28, Controll.trCamera.position, new Vector3(0f, Controll.rx, 0f), Controll.trCamera.forward, Color.black);
		}
	}

	private void recv_forceselect()
	{
		NET.BEGIN_READ(buffer, len, 4);
		int id = NET.READ_BYTE();
		VWIK.inDrop = false;
		Controll.ChangeWeapon(id);
	}
}
