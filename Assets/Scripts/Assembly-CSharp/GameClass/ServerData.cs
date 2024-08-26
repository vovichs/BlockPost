using UnityEngine;

namespace GameClass
{
	public class ServerData
	{
		public string ip;

		public int port;

		public int players;

		public int maxplayers;

		public int idx;

		public int gamemode;

		public int status;

		public int privategame;

		public int level;

		public string slevel;

		public string formatplayers;

		public Rect[] r;

		public string sPOS;

		public string sSERVER;

		public ServerData(string ip, int port, int players, int gamemode, int status, int privategame, int level, int maxplayers)
		{
			this.ip = ip;
			this.port = port;
			this.players = players;
			int num = port / 10000;
			idx = port - num * 10000;
			this.gamemode = gamemode;
			this.status = status;
			this.privategame = privategame;
			this.level = level;
			slevel = level.ToString();
			this.maxplayers = maxplayers;
			formatplayers = players + "/" + maxplayers;
		}
	}
}
