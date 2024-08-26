using System.Collections;
using Player;
using UnityEngine;

public class SoundLoader : MonoBehaviour
{
	public static SoundLoader cs;

	private void Awake()
	{
		cs = this;
	}

	public static void _load(WeaponData wd)
	{
	}

	public static IEnumerator Load(WeaponData wd)
	{
		yield break;
	}
}
