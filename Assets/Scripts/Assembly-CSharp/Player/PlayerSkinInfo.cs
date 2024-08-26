using UnityEngine;

namespace Player
{
	public class PlayerSkinInfo
	{
		public int idx;

		public string fullname;

		public int hairid;

		public int hatid;

		public int bodyid;

		public int pantsid;

		public int bootsid;

		public int cost;

		public string scost;

		public Texture2D img;

		public int shop;

		public PlayerSkinInfo(int idx, string fullname, int hairid, int hatid, int bodyid, int pantsid, int bootsid, int cost)
		{
			this.idx = idx;
			this.fullname = fullname;
			this.hairid = hairid;
			this.hatid = hatid;
			this.bodyid = bodyid;
			this.pantsid = pantsid;
			this.bootsid = bootsid;
			this.cost = cost;
			scost = cost.ToString();
			img = Resources.Load("Textures/skins/skin_" + idx + "_icon") as Texture2D;
			if (img == null)
			{
				img = ContentLoader_.LoadTexture("skin_" + idx + "_icon") as Texture2D;
			}
			shop = 0;
		}
	}
}
