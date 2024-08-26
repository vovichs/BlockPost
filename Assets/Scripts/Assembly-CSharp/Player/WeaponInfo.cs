using UnityEngine;

namespace Player
{
	public class WeaponInfo
	{
		public int id;

		public string name;

		public string fullname;

		public int ammo;

		public int backpack;

		public int damage;

		public int firerate;

		public int distance;

		public int accuracy;

		public int reload;

		public int recoil;

		public int piercing;

		public int mobility;

		public int slot;

		public string key;

		public Texture2D tIcon;

		public Vector2 vIcon;

		public Texture2D tIconDMSG;

		public Vector2 vIconDMSG;

		public int p = -1;

		public int firetype;

		public string localname;

		public void GenerateIcon()
		{
			tIcon = ContentLoader_.LoadTexture(name + "_icon") as Texture2D;
			if (tIcon == null)
			{
				tIcon = Resources.Load("weapons/" + name + "_icon") as Texture2D;
			}
			if (tIcon == null)
			{
				tIcon = ContentLoader_.LoadTexture(name) as Texture2D;
			}
			if (tIcon == null)
			{
				tIcon = Resources.Load("weapons/" + name) as Texture2D;
			}
			if (tIcon == null)
			{
				tIcon = TEX.GetTextureByName("red");
			}
			vIcon = GUIGameSet.CalcSize(tIcon);
		}
	}
}
