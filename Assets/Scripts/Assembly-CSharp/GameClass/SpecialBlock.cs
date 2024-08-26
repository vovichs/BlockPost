namespace GameClass
{
	public class SpecialBlock
	{
		public int x;

		public int y;

		public int z;

		public SpecialBlock(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public bool inBlock(int px, int py, int pz)
		{
			int num = ((px > x) ? (px - x) : (x - px));
			int num2 = ((py >= y) ? (py - y) : (-1));
			int num3 = ((pz > z) ? (pz - z) : (z - pz));
			if (num2 < 0)
			{
				return false;
			}
			if (num2 > 4)
			{
				return false;
			}
			if (num > 1)
			{
				return false;
			}
			if (num3 > 1)
			{
				return false;
			}
			return true;
		}
	}
}
