namespace Player
{
	public class CaseInv
	{
		public ulong uid;

		public CaseInfo ci;

		public string count;

		public float time;

		public int sec;

		public int icount;

		public CaseInv(ulong uid, CaseInfo ci)
		{
			if (ci != null)
			{
				this.uid = uid;
				this.ci = ci;
				count = null;
			}
		}
	}
}
