using System.Collections.Generic;
using UnityEngine;

public class DM : MonoBehaviour
{
	public class DestroyData
	{
		private GameObject go;

		private float deltime;

		public DestroyData(GameObject go, float deltime)
		{
			this.go = go;
			this.deltime = Time.time + deltime;
		}
	}

	private static float t = 0f;

	public static List<DestroyData> destroylist = new List<DestroyData>();

	private static void Add(GameObject go, float time)
	{
		destroylist.Add(new DestroyData(go, time));
	}

	private void Update()
	{
		if (!(Time.time < t))
		{
			t = Time.time + 0.2f;
		}
	}
}
