using UnityEngine;

namespace Player
{
	public class CaseInfo
	{
		public int idx;

		public string name;

		public string fullname;

		public int keyid;

		public int cost;

		public int shop;

		public string scost;

		public int weaponcount;

		public WeaponInfo[] weapon;

		public Texture2D img;

		public CaseInfo(int idx, string _name)
		{
			this.idx = idx;
			name = _name;
			img = ContentLoader_.LoadTexture("case_" + _name + "_icon") as Texture2D;
			if (img == null)
			{
				img = Resources.Load("cases/case_" + _name + "_icon") as Texture2D;
			}
		}
	}
}
