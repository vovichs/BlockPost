using UnityEngine;

public class GP : MonoBehaviour
{
	public static GP cs = null;

	public static bool auth = false;

	public static string email = "";

	public static string token = "";

	public static bool tokenloaded = false;

	public static bool force = false;

	public static bool connect = false;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void LoadEndFirst()
	{
		CheckPermission();
	}

	private void CheckPermission()
	{
	}
}
