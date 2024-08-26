using System;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	public class Logger
	{
		private static bool debugLogEnabled = false;

		private static bool warningLogEnabled = true;

		public static bool DebugLogEnabled
		{
			get
			{
				return debugLogEnabled;
			}
			set
			{
				debugLogEnabled = value;
			}
		}

		public static bool WarningLogEnabled
		{
			get
			{
				return warningLogEnabled;
			}
			set
			{
				warningLogEnabled = value;
			}
		}

		public static void d(string msg)
		{
			if (debugLogEnabled)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.Log(ToLogMessage(string.Empty, "DEBUG", msg));
				});
			}
		}

		public static void w(string msg)
		{
			if (warningLogEnabled)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.LogWarning(ToLogMessage("!!!", "WARNING", msg));
				});
			}
		}

		public static void e(string msg)
		{
			if (warningLogEnabled)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.LogWarning(ToLogMessage("***", "ERROR", msg));
				});
			}
		}

		public static string describe(byte[] b)
		{
			if (b != null)
			{
				return "byte[" + b.Length + "]";
			}
			return "(null)";
		}

		private static string ToLogMessage(string prefix, string logType, string msg)
		{
			string text = null;
			try
			{
				text = DateTime.Now.ToString("MM/dd/yy H:mm:ss zzz");
			}
			catch (Exception)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.LogWarning("*** [Play Games Plugin 0.10.09] ERROR: Failed to format DateTime.Now");
				});
				text = string.Empty;
			}
			return string.Format("{0} [Play Games Plugin 0.10.09] {1} {2}: {3}", prefix, text, logType, msg);
		}
	}
}
