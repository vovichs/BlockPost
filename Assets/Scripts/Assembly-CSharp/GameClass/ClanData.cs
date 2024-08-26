namespace GameClass
{
	public class ClanData
	{
		public int id;

		public string n;

		public string slots;

		public int status;

		public string owner;

		public int players;

		public ClanData(int id, string n, string owner, int sl, int st, int players)
		{
			this.id = id;
			this.n = n;
			this.owner = owner;
			slots = players + "/" + ((sl + 1) * 8).ToString();
			status = st;
			this.players = players;
		}
	}
}
