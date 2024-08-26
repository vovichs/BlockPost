using UnityEngine;

namespace GameClass
{
	public class ClanPlayer
	{
		public int id;

		public string n;

		public int rank;

		public int p0;

		public int p1;

		public int p2;

		public int p3;

		public int p4;

		public int p5;

		public int p6;

		public int p7;

		public int p8;

		public int p9;

		public int exp;

		public int f;

		public int d;

		public string sKD;

		public string sF;

		public string sD;

		public string sLV;

		public Texture2D tAvatar;

		public int exp7;

		public int f7;

		public int d7;

		public int exp30;

		public int f30;

		public int d30;

		public int full_exp;

		public int full_f;

		public int full_d;

		public string full_fd;

		public int online;

		public ClanPlayer(int id, string n, int rank, int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8, int p9, int exp, int f, int d)
		{
			this.id = id;
			this.n = n;
			this.rank = rank;
			this.p0 = p0;
			this.p1 = p1;
			this.p2 = p2;
			this.p3 = p3;
			this.p4 = p4;
			this.p5 = p5;
			this.p6 = p6;
			this.p7 = p7;
			this.p8 = p8;
			this.p9 = p9;
			this.exp = exp;
			this.f = f;
			this.d = d;
			sKD = ((float)f / (float)d).ToString("0.00");
			sF = f.ToString();
			sD = d.ToString();
			sLV = "Lv." + Main.CalcLevel(exp);
			online = 0;
		}

		public void SetStats(int exp7, int f7, int d7, int exp30, int f30, int d30, int exp, int f, int d)
		{
			this.exp7 = exp7;
			this.f7 = f7;
			this.d7 = d7;
			this.exp30 = exp30;
			this.f30 = f30;
			this.d30 = d30;
			full_exp = exp;
			full_f = f;
			full_d = d;
			if (d != 0)
			{
				full_fd = ((float)f / (float)d).ToString("0.00");
			}
		}
	}
}
