namespace GameClass
{
	public class TopPlayer
	{
		public int gid;

		public string n;

		public int pos;

		public int exp;

		public int f;

		public int d;

		public int a;

		public int h;

		public string sPos;

		public string sKD;

		public string sF;

		public string sD;

		public string sA;

		public string sH;

		public string sEXP;

		public string sLV;

		public TopPlayer(int pos, string n, int exp, int f, int d, int a, int h)
		{
			this.pos = pos;
			this.n = n;
			this.exp = exp;
			this.f = f;
			this.d = d;
			this.a = a;
			this.h = h;
			sPos = (pos + 1).ToString();
			sKD = ((d != 0) ? ((float)f / (float)d).ToString("0.00") : "0.0");
			sF = f.ToString();
			sD = d.ToString();
			sA = a.ToString();
			sH = h.ToString();
			sEXP = exp.ToString();
			sLV = "Lv." + Main.CalcLevel(exp);
		}
	}
}
