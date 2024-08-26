namespace Player
{
	public class WeaponSet
	{
		public int idx;

		public WeaponInv[] w;

		public WeaponSet(int idx)
		{
			this.idx = idx;
			w = new WeaponInv[5];
		}
	}
}
