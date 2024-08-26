using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zlib;
using UnityEngine;

public class DemoRec : MonoBehaviour
{
	public class RecPlayerData
	{
		public string n = "";

		public int[] shootattack = new int[4];

		public int[] shoothit = new int[4];

		public int shooterror;

		public int shootbody;

		public int shoothead;

		public int shootother;

		public int f;

		public int d;

		public int pid_attack;

		public int gid;

		public int net;

		public RecPlayerData(string _n)
		{
			n = _n;
		}
	}

	public class RecData
	{
		public byte[] Buffer;

		public int Len;

		public float time;

		public int packetid;

		public RecData(byte[] _buffer, int _len)
		{
			Buffer = new byte[_len];
			for (int i = 0; i < _len; i++)
			{
				Buffer[i] = _buffer[i];
			}
			Len = _len;
			time = Time.time;
		}

		~RecData()
		{
			Buffer = null;
		}
	}

	public static bool active = false;

	private static bool record = false;

	private static bool play = false;

	private static bool stats = false;

	private static int personview = 0;

	private static int limitdemo = 10;

	private static string demoname = "";

	private static bool extract = false;

	private static List<RecPlayerData> recplayerlist = null;

	private static List<RecData> plist = null;

	private static int ppos = 0;

	private static float timeoffset = 0f;

	private static float starttime = 0f;

	private static int[] pid_attack = new int[40];

	private static bool pid_attackblock = false;

	private static FileStream rFileExtract;

	public static void AllowRecord(bool val)
	{
		active = val;
	}

	public static void StartRecord()
	{
		if (active)
		{
			if (plist != null)
			{
				Log.Add("previous demo not correct finished");
			}
			plist = new List<RecData>();
			Log.Add("StartRecord");
			record = true;
		}
	}

	public static void StopRecord()
	{
		if (active && record)
		{
			Save(DateTime.Now.ToString("yyyyMMdd_hhmmss"));
			Log.Add("StopRecord packets " + plist.Count);
			plist.Clear();
			plist = null;
			record = false;
		}
	}

	public static void AddPacket(byte[] buffer, int len)
	{
		if (active && record)
		{
			plist.Add(new RecData(buffer, len));
		}
	}

	public static void PlayDemo(string filename)
	{
		demoname = filename;
		Load(filename);
		if (plist == null || plist.Count < 1)
		{
			return;
		}
		timeoffset = plist[0].time;
		play = true;
		ppos = 0;
		starttime = Time.time;
		if (stats)
		{
			for (int i = 0; i < plist.Count; i++)
			{
				Client.buffer = plist[ppos].Buffer;
				Client.len = plist[ppos].Len;
				ppos++;
				Client.cs.DemoOnRecvPacket();
			}
			AutoStop();
		}
	}

	public static void StatsDemo(string filename)
	{
		stats = true;
		PlayDemo(filename);
	}

	public static bool isDemo()
	{
		return play;
	}

	public static void StopDemo()
	{
		if (!play)
		{
			return;
		}
		play = false;
		if (stats && !extract && recplayerlist != null)
		{
			for (int i = 0; i < recplayerlist.Count; i++)
			{
				if (recplayerlist[i].shootattack[0] != 0)
				{
					float num = (float)recplayerlist[i].shoothit[0] / (float)recplayerlist[i].shootattack[0];
				}
				if (recplayerlist[i].d != 0)
				{
					float num2 = (float)recplayerlist[i].f / (float)recplayerlist[i].d;
				}
				if (recplayerlist[i].shootbody + recplayerlist[i].shoothead != 0)
				{
					float num3 = (float)recplayerlist[i].shoothead / (float)(recplayerlist[i].shootbody + recplayerlist[i].shoothead);
				}
			}
			SaveStats(demoname);
			recplayerlist.Clear();
			recplayerlist = null;
		}
		stats = false;
	}

	public static void UpdateDemo()
	{
		if (play)
		{
			if (Time.time - starttime + timeoffset > plist[ppos].time)
			{
				Client.buffer = plist[ppos].Buffer;
				Client.len = plist[ppos].Len;
				ppos++;
				Client.cs.DemoOnRecvPacket();
			}
			AutoStop();
		}
	}

	public static void AutoStop()
	{
		if (ppos >= plist.Count)
		{
			StopDemo();
			GUIGameExit.ExitMainMenu();
		}
	}

	public static void ChangeView()
	{
		if (play)
		{
			personview++;
			if (personview > 1)
			{
				personview = 0;
			}
			if (personview != 0)
			{
				int personview2 = personview;
				int num = 1;
			}
		}
	}

	private static void Save(string filename)
	{
		if (!active || plist == null || plist.Count < 10)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < plist.Count; i++)
		{
			num += plist[i].Len;
			num += 4;
		}
		byte[] array = new byte[num];
		NET._BEGIN_WRITE(array, 0);
		for (int j = 0; j < plist.Count; j++)
		{
			NET._WRITE_FLOAT(plist[j].time);
			for (int k = 0; k < plist[j].Len; k++)
			{
				NET._WRITE_BYTE(plist[j].Buffer[k]);
			}
		}
		byte[] array2 = GZipStream.CompressBuffer(array);
		int num2 = array2.Length;
		byte[] array3 = new byte[5 + num2];
		NET._BEGIN_WRITE(array3, 0);
		NET._WRITE_BYTE(0);
		NET._WRITE_LONG(0);
		for (int l = 0; l < num2; l++)
		{
			NET._WRITE_BYTE(array2[l]);
		}
		if (!Directory.Exists("Demos"))
		{
			Directory.CreateDirectory("Demos");
		}
		FileStream fileStream = new FileStream("Demos/" + filename + ".bpdm0", FileMode.Create);
		fileStream.Write(array3, 0, array3.Length);
		fileStream.Close();
		LimitDemoFiles();
	}

	public static void LimitDemo(int val)
	{
		limitdemo = val;
	}

	public static void LimitDemoFiles()
	{
		if (limitdemo == 0)
		{
			return;
		}
		FileInfo[] array = Enumerable.ToArray(Enumerable.OrderBy(new DirectoryInfo("Demos").GetFiles("*.bpdm*"), (FileInfo p) => p.CreationTime));
		if (array != null)
		{
			for (int i = 0; i < array.Length - limitdemo; i++)
			{
				File.Delete(array[i].FullName);
			}
		}
	}

	public static void Load(string filename)
	{
		if (!File.Exists("Demos/" + filename + ".bpdm0"))
		{
			return;
		}
		byte[] array = null;
		int num = 0;
		FileStream fileStream = new FileStream("Demos/" + filename + ".bpdm0", FileMode.Open, FileAccess.Read);
		try
		{
			num = (int)fileStream.Length;
			array = new byte[num];
			fileStream.Read(array, 0, num);
		}
		finally
		{
			fileStream.Close();
		}
		NET.BEGIN_READ(array, num, 0);
		NET.READ_BYTE();
		NET.READ_LONG();
		byte[] array2 = new byte[num - 5];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = (byte)NET.READ_BYTE();
		}
		byte[] array3 = GZipStream.UncompressBuffer(array2);
		plist = new List<RecData>();
		NET.BEGIN_READ(array3, array3.Length, 0);
		bool flag = NET.READ_ERROR();
		while (!flag)
		{
			float time = NET.READ_FLOAT();
			NET.READ_BYTE();
			int packetid = NET.READ_BYTE();
			int num2 = NET.READ_SHORT();
			NET.READ_POS_SET(NET.READ_POS() - 4);
			byte[] array4 = new byte[num2];
			for (int j = 0; j < num2; j++)
			{
				array4[j] = (byte)NET.READ_BYTE();
			}
			RecData recData = new RecData(array4, num2);
			recData.time = time;
			recData.packetid = packetid;
			plist.Add(recData);
			flag = NET.READ_ERROR();
		}
		Log.Add("packet count " + plist.Count);
	}

	public static void stats_AddPlayer(string n)
	{
		if (!stats)
		{
			return;
		}
		if (recplayerlist == null)
		{
			recplayerlist = new List<RecPlayerData>();
		}
		for (int i = 0; i < recplayerlist.Count; i++)
		{
			if (recplayerlist[i].n == n)
			{
				return;
			}
		}
		recplayerlist.Add(new RecPlayerData(n));
	}

	public static void stats_AddPlayerHiddenInfo(string n, int gid, int net)
	{
		if (!stats)
		{
			return;
		}
		if (recplayerlist == null)
		{
			recplayerlist = new List<RecPlayerData>();
		}
		for (int i = 0; i < recplayerlist.Count; i++)
		{
			if (recplayerlist[i].n == n)
			{
				recplayerlist[i].gid = gid;
				recplayerlist[i].net = net;
			}
		}
	}

	public static void stats_AddDeath(string n, int hs)
	{
		if (!stats)
		{
			return;
		}
		if (recplayerlist == null)
		{
			recplayerlist = new List<RecPlayerData>();
		}
		for (int i = 0; i < recplayerlist.Count; i++)
		{
			if (recplayerlist[i].n == n)
			{
				switch (hs)
				{
				case 0:
					recplayerlist[i].shootbody++;
					break;
				case 1:
					recplayerlist[i].shoothead++;
					break;
				}
			}
		}
	}

	public static void stats_Attack(int id, string n, int slot)
	{
		if (!stats || recplayerlist == null)
		{
			return;
		}
		for (int i = 0; i < recplayerlist.Count; i++)
		{
			if (recplayerlist[i].n == n)
			{
				if (slot < 0)
				{
					recplayerlist[i].shooterror++;
				}
				else
				{
					recplayerlist[i].shootattack[slot]++;
				}
			}
		}
	}

	public static void stats_Damage(string n, int vbox, int slot)
	{
		if (!stats || recplayerlist == null)
		{
			return;
		}
		for (int i = 0; i < recplayerlist.Count; i++)
		{
			if (recplayerlist[i].n == n)
			{
				if (slot < 0)
				{
					recplayerlist[i].shooterror++;
				}
				else
				{
					recplayerlist[i].shoothit[slot]++;
				}
				switch (vbox)
				{
				case 0:
					recplayerlist[i].shootbody++;
					break;
				case 1:
					recplayerlist[i].shoothead++;
					break;
				}
			}
		}
	}

	public static void stats_PlayerStatsFrag(string n, int frags)
	{
		if (!stats || recplayerlist == null)
		{
			return;
		}
		for (int i = 0; i < recplayerlist.Count; i++)
		{
			if (recplayerlist[i].n == n)
			{
				recplayerlist[i].f = frags;
			}
		}
	}

	public static void stats_PlayerStatsDeaths(string n, int deaths)
	{
		if (!stats || recplayerlist == null)
		{
			return;
		}
		for (int i = 0; i < recplayerlist.Count; i++)
		{
			if (recplayerlist[i].n == n)
			{
				recplayerlist[i].d = deaths;
			}
		}
	}

	public static void stats_PlayerAttackBlock(int x, int y, int z)
	{
		if (stats && recplayerlist != null)
		{
			pid_attackblock = true;
		}
	}

	public static void SaveStats(string filename)
	{
		if (!Directory.Exists("Demos"))
		{
			Directory.CreateDirectory("Demos");
		}
		FileStream fileStream = new FileStream("Demos/" + filename + ".bpstats", FileMode.OpenOrCreate, FileAccess.Write);
		for (int i = 0; i < recplayerlist.Count; i++)
		{
			float num = ((recplayerlist[i].shootattack[0] == 0) ? 0f : ((float)recplayerlist[i].shoothit[0] / (float)recplayerlist[i].shootattack[0]));
			float num2 = ((recplayerlist[i].d == 0) ? 0f : ((float)recplayerlist[i].f / (float)recplayerlist[i].d));
			float num3 = ((recplayerlist[i].shootbody + recplayerlist[i].shoothead == 0) ? 0f : ((float)recplayerlist[i].shoothead / (float)(recplayerlist[i].shootbody + recplayerlist[i].shoothead)));
			string text = recplayerlist[i].n + ": AIM " + num.ToString("0.00") + " K/D " + num2.ToString("0.00") + " FRAGS " + recplayerlist[i].f + " DEATHS " + recplayerlist[i].d + " HS " + num3.ToString("0.00");
			if (MainManager.mod)
			{
				text = text + " GID " + recplayerlist[i].gid + "/" + recplayerlist[i].net;
			}
			WriteString(fileStream, text.Replace(",", "."));
		}
		fileStream.Close();
	}

	private static void WriteString(FileStream fs, string v)
	{
		string newLine = Environment.NewLine;
		string s = v + newLine;
		byte[] bytes = new UTF8Encoding(true).GetBytes(s);
		fs.Write(bytes, 0, bytes.Length);
	}

	public static string[] LoadStats(string filename)
	{
		if (!Directory.Exists("Demos"))
		{
			return null;
		}
		string text = null;
		if (!File.Exists("Demos/" + filename + ".bpstats"))
		{
			return null;
		}
		FileStream fileStream = new FileStream("Demos/" + filename + ".bpstats", FileMode.Open, FileAccess.Read);
		try
		{
			int num = (int)fileStream.Length;
			byte[] array = new byte[num];
			fileStream.Read(array, 0, num);
			text = Encoding.Default.GetString(array);
			fileStream.Close();
		}
		finally
		{
			fileStream.Close();
		}
		if (text == null)
		{
			return null;
		}
		string text2 = Environment.NewLine + "\n\r";
		return text.Split(text2.ToCharArray(), StringSplitOptions.None);
	}

	public static void ExtractDemoStart(string filename)
	{
	}

	public static void ExtractDemoEnd()
	{
	}

	public static void ExtractWrite(string s)
	{
	}
}
