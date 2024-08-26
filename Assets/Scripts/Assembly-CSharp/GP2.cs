using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GP2 : MonoBehaviour
{
	public static GP2 cs = null;

	public static string email = "_";

	public static string token_part0 = "";

	public static string token_part1 = "";

	private void Awake()
	{
		cs = this;
	}

	private void Start()
	{
		CheckPermission();
	}

	private void CheckPermission()
	{
	}

	public void SignInNoSilent()
	{
	}

	public void ClearData()
	{
	}

	public static string[] Chop(string token_string, int length)
	{
		int num = token_string.Length;
		int num2 = (num + length - 1) / length;
		string[] array = new string[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = token_string.Substring(i * length, Mathf.Min(length, num));
			num -= length;
		}
		return array;
	}

	public void Auth()
	{
		Log.AddMainLog("START AUTH");
		PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestIdToken().Build());
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		if (Social.localUser.authenticated)
		{
			bool flag = Social.localUser.image != null;
		}
		else
		{
			SignIn();
		}
	}

	public void SignIn()
	{
		Social.localUser.Authenticate(delegate(bool success, string error)
		{
			if (success)
			{
				Log.AddMainLog("Authentication ok");
				SendAuth();
			}
			else
			{
				Log.AddMainLog("Authentication failed");
				Log.AddMainLog("Error: " + error);
			}
		});
	}

	public void SendAuth()
	{
		string text = PlayGamesPlatform.Instance.GetIdToken().ToString();
		string text2 = "";
		string text3 = "";
		if (text.Length <= 1000)
		{
			text2 = text;
		}
		else
		{
			text2 = text.Substring(0, 1000);
			text3 = text.Substring(1000, text.Length - 1000);
		}
		token_part0 = text2;
		token_part1 = text3;
		GP.auth = true;
		MasterClient.IP = MainConfig.DEFAULT_IP;
		MasterClient.PORT = 7780;
		MasterClient.cs.Connect();
	}
}
