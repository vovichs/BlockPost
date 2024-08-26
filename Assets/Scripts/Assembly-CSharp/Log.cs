using System.Collections.Generic;
using GameClass;
using UnityEngine;

public class Log : MonoBehaviour
{
	public static bool active = false;

	private static List<LogData> loglist = new List<LogData>();

	private static Rect r;

	private static List<LogData> mainloglist = new List<LogData>();

	public static void Add(string msg)
	{
		if (active)
		{
			float time = Time.time;
			float num = Mathf.Floor(time / 60f);
			float num2 = Mathf.RoundToInt(time % 60f);
			string msg2 = "[ " + num.ToString("00") + ":" + num2.ToString("00") + " ] " + msg;
			loglist.Add(new LogData(msg2));
			if (loglist.Count > 25)
			{
				loglist.RemoveAt(0);
			}
		}
	}

	private void Start()
	{
	}

	private void OnGUI()
	{
		if (active)
		{
			GUI.depth = -10;
			int num = loglist.Count - 25;
			if (num < 0)
			{
				num = 0;
			}
			int num2 = 0;
			for (int i = num; i < loglist.Count; i++)
			{
				r.Set(GUIM.YRES(8f), GUIM.YRES(200f) + GUIM.YRES(16f) * (float)num2, GUIM.YRES(256f), GUIM.YRES(16f));
				GUIM.DrawText(r, loglist[i].msg, TextAnchor.MiddleLeft, BaseColor.White, 1, 12, true);
				num2++;
			}
		}
	}

	public static void AddMainLog(string msg)
	{
		float time = Time.time;
		float num = Mathf.Floor(time / 60f);
		float num2 = Mathf.RoundToInt(time % 60f);
		string msg2 = "[ " + num.ToString("00") + ":" + num2.ToString("00") + " ] " + msg;
		mainloglist.Add(new LogData(msg2));
		if (mainloglist.Count > 8)
		{
			mainloglist.RemoveAt(0);
		}
	}

	public static void DrawLog()
	{
		if (mainloglist.Count != 0)
		{
			int count = mainloglist.Count;
			for (int i = 0; i < count; i++)
			{
				r.Set(GUIM.YRES(8f), GUIM.YRES(400f) + GUIM.YRES(26f) * (float)i, GUIM.YRES(256f), GUIM.YRES(26f));
				GUIM.DrawText(r, mainloglist[i].msg, TextAnchor.MiddleLeft, BaseColor.White, 1, 20, true);
			}
		}
	}
}
