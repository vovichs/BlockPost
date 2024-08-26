using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class VKWeb : MonoBehaviour
{
	public static VKWeb cs;

	private void Awake()
	{
		cs = this;
	}

	public void GetAuth(string token)
	{
		StartCoroutine(cGetAuth(token));
	}

	private IEnumerator cGetAuth(string token)
	{
		string uri = "https://playblockpost.com/bpm/web_auth.php?client_token=" + token;
		UnityWebRequest www = UnityWebRequest.Get(uri);
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			MainManager.error = true;
			MainManager.error_msg = "ОШИБКА АВТОРИЗАЦИИ (" + www.error + ")";
			yield break;
		}
		Log.AddMainLog(www.downloadHandler.text);
		string[] array = www.downloadHandler.text.Split('_');
		if (array[0] == "err")
		{
			if (array.Length == 2)
			{
				if (array[1] == "15")
				{
					Console.Log("session expired");
					Main.mauth = false;
				}
				else
				{
					MainManager.error = true;
					MainManager.error_msg = "ОШИБКА АВТОРИЗАЦИИ (ERRORCODE " + array[1] + ")";
				}
			}
			else
			{
				MainManager.error = true;
				MainManager.error_msg = "ОШИБКА АВТОРИЗАЦИИ";
			}
		}
		else if (www.downloadHandler.text.Split('|')[0] == "AUTH")
		{
			string[] array2 = www.downloadHandler.text.Split('|');
			MasterClient.vk_id = array2[1];
			MasterClient.vk_key = array2[2];
			MasterClient.IP = MainConfig.DEFAULT_IP;
			MasterClient.cs.Connect();
		}
		else
		{
			MainManager.error = true;
			MainManager.error_msg = "ОШИБКА АВТОРИЗАЦИИ (" + www.downloadHandler.text + ")";
		}
	}
}
