using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class UtilHash : MonoBehaviour
{
	public static string Calc(byte[] data)
	{
		byte[] array = new MD5CryptoServiceProvider().ComputeHash(data);
		string text = "";
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			text += b.ToString("x2");
		}
		return text;
	}

	public static string Calc(string strdata)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(strdata);
		byte[] array = new MD5CryptoServiceProvider().ComputeHash(bytes);
		string text = "";
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			text += b.ToString("x2");
		}
		return text;
	}
}
