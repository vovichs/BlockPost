using UnityEngine;

namespace GameClass
{
	public class QuestData
	{
		public int id;

		public string label;

		public string text;

		public Texture2D icon;

		public Texture2D rewardicon;

		public int c;

		public int active;

		public static int[] qcount = new int[3];

		public QuestData(int id, string label, string text, Texture2D icon, Texture2D rewardicon, int c, int active)
		{
			this.id = id;
			this.label = label;
			this.text = text;
			this.icon = icon;
			this.rewardicon = rewardicon;
			this.c = c;
			this.active = active;
		}

		public static void CalcCount(QuestData[] qlist)
		{
			for (int i = 0; i < 3; i++)
			{
				qcount[i] = 0;
			}
			if (qlist == null)
			{
				return;
			}
			for (int j = 0; j < qlist.Length; j++)
			{
				if (qlist[j] != null)
				{
					int num = qlist[j].active;
					qcount[num]++;
				}
			}
		}
	}
}
