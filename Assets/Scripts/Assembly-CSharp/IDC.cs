using System.Runtime.InteropServices;
using UnityEngine;

public class IDC : MonoBehaviour
{
	[DllImport("idclib_32.dll", CharSet = CharSet.Ansi)]
	public static extern int getAccessData(int gameid, string token, string uuid, ref int iduser, [Out] byte[] _usercrc, [Out] byte[] _language, [Out] byte[] _country, [Out] byte[] _currency);

	public static bool idcfunctions(string t, string u)
	{
		int num = 0;
		int iduser = -1;
		byte[] usercrc = new byte[1024];
		byte[] language = new byte[3];
		byte[] country = new byte[3];
		byte[] currency = new byte[3];
		num = getAccessData(1002, t, u, ref iduser, usercrc, language, country, currency);
		Debug.Log("IDC getAccessData RES " + num + " userid " + iduser);
		if (num == 0)
		{
			MasterClient.vk_id = iduser.ToString();
			MasterClient.vk_key = t;
			MasterClient.vk_reserve = u;
			return true;
		}
		return false;
	}
}
