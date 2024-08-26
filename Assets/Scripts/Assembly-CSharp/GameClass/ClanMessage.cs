using System;

namespace GameClass
{
	public class ClanMessage
	{
		public int id;

		public int t;

		public int v0;

		public int v1;

		public string msg;

		public int dt;

		public int st;

		public string datetime;

		public ClanMessage(int id, int t, int v0, int v1, string msg, int dt, int st)
		{
			this.id = id;
			this.t = t;
			this.v0 = v0;
			this.v1 = v1;
			if (t == 0)
			{
				this.msg = Lang.GetString("_REQUEST_PLAYER_TO_CLAN") + msg;
			}
			this.dt = dt;
			this.st = st;
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(dt);
			datetime = string.Format("{0:dd MMM hh:mm}", dateTime);
		}
	}
}
