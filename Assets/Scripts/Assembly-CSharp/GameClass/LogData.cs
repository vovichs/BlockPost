using UnityEngine;

namespace GameClass
{
	internal class LogData
	{
		public string msg;

		public float time;

		public LogData(string msg)
		{
			this.msg = msg;
			time = Time.time;
		}
	}
}
