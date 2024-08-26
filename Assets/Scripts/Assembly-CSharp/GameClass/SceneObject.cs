using UnityEngine;

namespace GameClass
{
	public class SceneObject
	{
		public int itemtype;

		public GameObject go;

		public SceneObject(GameObject go, int itemtype)
		{
			this.go = go;
			this.itemtype = itemtype;
		}
	}
}
