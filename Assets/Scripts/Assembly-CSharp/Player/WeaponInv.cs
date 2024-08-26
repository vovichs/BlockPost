namespace Player
{
	public class WeaponInv
	{
		public ulong uid;

		public WeaponInfo wi;

		public int frags;

		public int level;

		public byte[] mod = new byte[3];

		public byte dublicate;

		public int scopeid;

		public int ammo;

		public int backpack;

		public string detail_level;

		public string detail_progress;

		public WeaponInv(ulong uid, WeaponInfo wi)
		{
			this.uid = uid;
			this.wi = wi;
		}

		public int GetLevel()
		{
			return frags / 100 + 1;
		}
	}
}
