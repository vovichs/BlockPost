namespace GameClass
{
	public class TopClan
	{
		public int gid;

		public string n;

		public int pos;

		public int exp;

		public int slots;

		public int players;

		public string sPos;

		public string sEXP;

		public string sLV;

		public string sPlayers;

		public string sSlots;

		public string sPLSL;

		public TopClan(int pos, string n, int exp, int slots, int players)
		{
			this.pos = pos;
			this.n = n;
			this.exp = exp;
			this.players = players;
			this.slots = slots;
			sPos = (pos + 1).ToString();
			sEXP = exp.ToString();
			sLV = "Lv." + Main.CalcClanLevel(exp);
			sSlots = slots.ToString();
			sPlayers = players.ToString();
		}
	}
}
