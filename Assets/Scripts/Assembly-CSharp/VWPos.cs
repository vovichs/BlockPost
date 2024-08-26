using System;
using System.Globalization;
using Player;
using UnityEngine;

public class VWPos : MonoBehaviour
{
	public static void Load(WeaponData wd)
	{
		LoadHandPos(wd, "base");
		LoadHandPos(wd, "menu");
		LoadWeaponPos(wd);
	}

	public static void LoadHandPos(WeaponData wd, string posname)
	{
		TextAsset textAsset = Resources.Load("params/p_" + wd.weaponname + "_" + posname) as TextAsset;
		if (textAsset == null)
		{
			textAsset = ContentLoader_.LoadTextAsset("p_" + wd.weaponname + "_" + posname);
		}
		if (textAsset == null)
		{
			return;
		}
		WeaponPosHands weaponPosHands = new WeaponPosHands();
		string[] array = textAsset.text.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(' ');
			if (array2.Length > 1)
			{
				string object_name = array2[0];
				string transform_type = array2[1];
				Vector3 v = ParsVector3(array2);
				SetVector3(0, weaponPosHands, "RightArmUp", object_name, transform_type, v);
				SetVector3(1, weaponPosHands, "RightArmDown", object_name, transform_type, v);
				SetVector3(2, weaponPosHands, "LeftArmUp", object_name, transform_type, v);
				SetVector3(3, weaponPosHands, "LeftArmDown", object_name, transform_type, v);
			}
		}
		if (posname == "base")
		{
			wd.pBase = weaponPosHands;
		}
		else if (posname == "menu")
		{
			wd.pMenu = weaponPosHands;
		}
	}

	private static Vector3 ParsVector3(string[] d)
	{
		if (d.Length != 5)
		{
			return Vector3.zero;
		}
		float result = 0f;
		float result2;
		float.TryParse(d[2], NumberStyles.Any, CultureInfo.InvariantCulture, out result2);
		float result3;
		float.TryParse(d[3], NumberStyles.Any, CultureInfo.InvariantCulture, out result3);
		float.TryParse(d[4], NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		return new Vector3(result2, result3, result);
	}

	private static void SetVector3(int id, WeaponPosHands ph, string orgname, string object_name, string transform_type, Vector3 v)
	{
		if (!(orgname != object_name))
		{
			if (transform_type == "r")
			{
				ph.r[id] = v;
			}
			else if (transform_type == "p")
			{
				ph.p[id] = v;
			}
		}
	}

	public static void LoadWeaponPos(WeaponData wd)
	{
		TextAsset textAsset = Resources.Load("params/p_" + wd.weaponname) as TextAsset;
		if (textAsset == null)
		{
			textAsset = ContentLoader_.LoadTextAsset("p_" + wd.weaponname);
		}
		if (textAsset == null)
		{
			return;
		}
		WeaponPos weaponPos = null;
		WeaponPos weaponPos2 = null;
		string[] array = textAsset.text.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(' ');
			if (array2.Length <= 1)
			{
				continue;
			}
			string text = array2[0];
			string text2 = array2[1];
			Vector3 vector = ParsVector3(array2);
			if (text == "backpack")
			{
				if (weaponPos == null)
				{
					weaponPos = new WeaponPos();
				}
				switch (text2)
				{
				case "p":
					weaponPos.p = vector;
					break;
				case "r":
					weaponPos.r = vector;
					break;
				case "s":
					weaponPos.s = vector;
					break;
				}
			}
			else if (text == "weapon")
			{
				if (weaponPos2 == null)
				{
					weaponPos2 = new WeaponPos();
				}
				switch (text2)
				{
				case "p":
					weaponPos2.p = vector;
					break;
				case "r":
					weaponPos2.r = vector;
					break;
				case "s":
					weaponPos2.s = vector;
					break;
				}
			}
		}
		if (weaponPos != null)
		{
			wd.pBackpack = weaponPos;
		}
		if (weaponPos2 != null)
		{
			wd.pWeapon = weaponPos2;
		}
	}
}
