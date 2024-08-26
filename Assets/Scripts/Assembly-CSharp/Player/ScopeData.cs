using UnityEngine;

namespace Player
{
	public class ScopeData
	{
		public int level;

		public int id;

		public string name_;

		public string fullname;

		public string slevel;

		public Texture2D zoom;

		public Texture2D dot;

		public Texture2D glass;

		public int scopetype;

		public int fov;

		public ScopeData(int level, string name_)
		{
			this.level = level;
			id = -1;
			this.name_ = name_;
			fullname = "notfound_" + name_;
			scopetype = 0;
			for (int i = 0; i < ScopeGen.scope.Length; i++)
			{
				if (ScopeGen.scope[i] != null && ScopeGen.scope[i].name_.Equals(name_))
				{
					id = i;
					fullname = ScopeGen.scope[i].fullname;
					scopetype = ScopeGen.scope[i].scopetype;
					fov = ScopeGen.scope[i].fov;
					break;
				}
			}
			slevel = "Lv." + this.level;
			zoom = Resources.Load("scopes/" + name_ + "_zoom") as Texture2D;
			if (zoom == null)
			{
				zoom = ContentLoader_.LoadTexture(name_ + "_zoom") as Texture2D;
			}
			if (zoom == null)
			{
				zoom = Resources.Load("scopes/s_sniper_s_zoom") as Texture2D;
			}
			if (zoom == null)
			{
				zoom = ContentLoader_.LoadTexture("s_sniper_s_zoom") as Texture2D;
			}
			dot = Resources.Load("scopes/" + name_ + "_dot") as Texture2D;
			if (dot == null)
			{
				dot = ContentLoader_.LoadTexture(name_ + "_dot") as Texture2D;
			}
			if (dot == null)
			{
				dot = Resources.Load("scopes/tasco_s_dot") as Texture2D;
			}
			if (dot == null)
			{
				dot = ContentLoader_.LoadTexture("tasco_s_dot") as Texture2D;
			}
			glass = Resources.Load("scopes/" + name_ + "_glass") as Texture2D;
			if (glass == null)
			{
				glass = ContentLoader_.LoadTexture(name_ + "_glass") as Texture2D;
			}
		}
	}
}
