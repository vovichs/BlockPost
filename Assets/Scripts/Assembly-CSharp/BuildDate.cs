using System;
using System.Reflection;
using UnityEngine;

public class BuildDate : MonoBehaviour
{
	[Tooltip("Date/time format.")]
	private string format = "g";

	public static Version Version()
	{
		return Assembly.GetExecutingAssembly().GetName().Version;
	}

	public static DateTime Date()
	{
		Version version = Version();
		DateTime dateTime = new DateTime(2000, 1, 1, 0, 0, 0);
		TimeSpan value = new TimeSpan(version.Build, 0, 0, version.Revision * 2);
		return dateTime.Add(value);
	}

	public static string ToString(string format = null)
	{
		return Date().ToString(format);
	}
}
