using System;
using UnityEngine;

public class GC : MonoBehaviour
{
	private static float t;

	private void Update()
	{
		if (Builder.active && !(Time.time < t))
		{
			t = Time.time + 60f;
			System.GC.Collect();
			Resources.UnloadUnusedAssets();
		}
	}

	public static void Force()
	{
		t = Time.time + 60f;
		System.GC.Collect();
		Resources.UnloadUnusedAssets();
	}
}
