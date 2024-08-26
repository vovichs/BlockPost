using UnityEngine;

namespace GameClass
{
	public class AttackData
	{
		public int vid;

		public int hitbox;

		public Vector3 hitpos;

		public AttackData(int vid, int hitbox, Vector3 hitpos)
		{
			this.vid = vid;
			this.hitbox = hitbox;
			this.hitpos = hitpos;
		}
	}
}
