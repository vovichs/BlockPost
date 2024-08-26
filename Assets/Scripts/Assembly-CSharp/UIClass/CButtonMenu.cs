using UnityEngine;

namespace UIClass
{
	internal class CButtonMenu
	{
		public int idx;

		public string codename;

		public string text;

		public Rect r;

		public float width;

		public string[] text2;

		public Rect[] r2;

		public Rect[] r2_text;

		public int newitem;

		public CButtonMenu(int idx, string codename, string t, string[] t2)
		{
			text = t;
			text2 = t2;
			if (t2 != null)
			{
				r2 = new Rect[t2.Length];
				r2_text = new Rect[t2.Length];
			}
			this.idx = idx;
			this.codename = codename;
		}

		~CButtonMenu()
		{
		}
	}
}
