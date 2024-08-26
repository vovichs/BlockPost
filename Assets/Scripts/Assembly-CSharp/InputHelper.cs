using System.Collections.Generic;
using UnityEngine;

public class InputHelper : MonoBehaviour
{
	private static TouchCreator lastFakeTouch;

	public static List<Touch> GetTouches()
	{
		List<Touch> list = new List<Touch>();
		list.AddRange(Input.touches);
		return list;
	}
}
