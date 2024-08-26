using System.Collections.Generic;

namespace GameClass
{
	public class Quest
	{
		public QuestData data;

		public int param;

		public float progress;

		public int complete;

		public string sProgress;

		public string text;

		public static int[] qcount = new int[3];

		public Quest(QuestData data, int progress, int require, int complete)
		{
			this.data = data;
			if (data.id == 0 || data.id == 1)
			{
				this.progress = 0f;
				if (require != 0)
				{
					float num = Main.CalcExp(require);
					float num2 = progress;
					this.progress = num2 / num * 100f;
				}
			}
			else if (data.id == 2 || data.id == 3)
			{
				this.progress = 100f;
			}
			else
			{
				this.progress = progress;
			}
			if (this.progress > 100f)
			{
				this.progress = 100f;
			}
			sProgress = this.progress.ToString("0.00") + "%";
			text = data.text.Replace("%param%", require.ToString());
			this.complete = complete;
		}

		public static void CalcCount(List<Quest> qlist)
		{
			for (int i = 0; i < 3; i++)
			{
				qcount[i] = 0;
			}
			for (int j = 0; j < qlist.Count; j++)
			{
				int active = qlist[j].data.active;
				qcount[active]++;
			}
		}
	}
}
