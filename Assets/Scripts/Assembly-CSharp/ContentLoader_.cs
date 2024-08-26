using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentLoader_
{
	public class LoadBundle
	{
		public string name;

		public bool loaded;

		public bool dontdestroy;

		public int version;

		public int size;

		public bool crypted;

		public LoadBundle(string name, int version, bool dontdestroy, bool crypted, int size = 1)
		{
			this.name = name;
			loaded = false;
			this.version = version;
			this.size = size;
			this.dontdestroy = dontdestroy;
			this.crypted = crypted;
		}
	}

	public static bool proceed = false;

	public static string host = "https://files.underdogs.ru/bp/android";

	private static string ext = "webp";

	private const int HEADER_SKIP = 124;

	public static string CURRPATH = "";

	public static List<UnityEngine.Object> materialList = new List<UnityEngine.Object>();

	public static List<UnityEngine.Object> gameobjectList = new List<UnityEngine.Object>();

	public static List<UnityEngine.Object> textureList = new List<UnityEngine.Object>();

	public static List<UnityEngine.Object> spriteList = new List<UnityEngine.Object>();

	public static List<UnityEngine.Object> audioList = new List<UnityEngine.Object>();

	public static List<UnityEngine.Object> fontList = new List<UnityEngine.Object>();

	public static List<UnityEngine.Object> textassetList = new List<UnityEngine.Object>();

	public static List<LoadBundle> LoadList = new List<LoadBundle>();

	public static LoadBundle currBundle = null;

	public static bool inDownload = false;

	public static int maxcontentcount = 0;

	public static WWW www = null;

	public static string[] assetdir = null;

	public static bool loadmap = false;

	private static int gv = 2;

	public static void Init()
	{
		loadmap = false;
		if (proceed)
		{
			BroadcastAll("LoadEnd", "");
			return;
		}
		textassetList.Clear();
		materialList.Clear();
		gameobjectList.Clear();
		spriteList.Clear();
		textureList.Clear();
		audioList.Clear();
		AddPack();
	}

	public static void AddPack()
	{
		LoadList.Clear();
		TextAsset textAsset = Resources.Load("m_res") as TextAsset;
		if (textAsset == null)
		{
			return;
		}
		string text = Environment.NewLine + "\n\r";
		string[] array = textAsset.text.Split(text.ToCharArray(), StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(' ');
			if (array2[0] == "file")
			{
				LoadList.Add(new LoadBundle(array2[1], gv, false, true));
			}
		}
		maxcontentcount = LoadList.Count;
	}

	public static void LoadLevel(string packname)
	{
		loadmap = true;
		LoadList.Clear();
		LoadList.Add(new LoadBundle(packname, gv, false, true));
		maxcontentcount = LoadList.Count;
	}

	public static IEnumerator cLoad(string svalue, int version, bool crypted)
	{
		System.GC.Collect();
		yield return null;
		string text = host + "/data/" + svalue + "." + ext;
		Debug.Log("dev android load file: android/" + svalue);
		TextAsset b = Resources.Load("android/" + svalue) as TextAsset;
		yield return null;
		AssetBundle bundle = AssetBundle.LoadFromMemory(b.bytes);
		yield return null;
		if (bundle == null)
		{
			inDownload = false;
			yield break;
		}
		if (currBundle.crypted)
		{
			TextAsset textAsset = bundle.LoadAsset(currBundle.name) as TextAsset;
			if (textAsset == null)
			{
				Debug.Log("error bundle " + currBundle.name);
				yield break;
			}
			byte[] binary = UnLoad(textAsset.bytes);
			AssetBundleCreateRequest acr = AssetBundle.LoadFromMemoryAsync(binary);
			yield return acr;
			if (!currBundle.dontdestroy)
			{
				bundle.Unload(false);
			}
			bundle = acr.assetBundle;
		}
		UnityEngine.Object[] objArray7 = bundle.LoadAllAssets(typeof(TextAsset));
		yield return null;
		for (int i = 0; i < objArray7.Length; i++)
		{
			textassetList.Add(objArray7[i]);
		}
		objArray7 = bundle.LoadAllAssets(typeof(Texture));
		yield return null;
		for (int j = 0; j < objArray7.Length; j++)
		{
			textureList.Add(objArray7[j]);
		}
		objArray7 = bundle.LoadAllAssets(typeof(Sprite));
		yield return null;
		for (int k = 0; k < objArray7.Length; k++)
		{
			spriteList.Add(objArray7[k]);
		}
		objArray7 = bundle.LoadAllAssets(typeof(Material));
		yield return null;
		for (int l = 0; l < objArray7.Length; l++)
		{
			materialList.Add(objArray7[l]);
		}
		objArray7 = bundle.LoadAllAssets(typeof(GameObject));
		yield return null;
		for (int m = 0; m < objArray7.Length; m++)
		{
			gameobjectList.Add(objArray7[m]);
		}
		objArray7 = bundle.LoadAllAssets(typeof(AudioClip));
		yield return null;
		for (int n = 0; n < objArray7.Length; n++)
		{
			audioList.Add(objArray7[n]);
		}
		objArray7 = bundle.LoadAllAssets(typeof(Font));
		yield return null;
		for (int num = 0; num < objArray7.Length; num++)
		{
			fontList.Add(objArray7[num]);
		}
		if (!currBundle.dontdestroy)
		{
			bundle.Unload(false);
		}
		if (LoadList.Count == 0)
		{
			if (loadmap)
			{
				BroadcastAll("LoadMap", "");
				loadmap = false;
			}
			else
			{
				BroadcastAll("LoadEnd", "");
			}
			proceed = true;
		}
		if (!proceed)
		{
			int count = LoadList.Count;
			int num2 = maxcontentcount - 1;
		}
		GameObject.Find("Core").GetComponent<MasterClient>().PostAwake();
		GUIM.LoadFont();
		BroadcastAll("LoadEndFirst", "");
		inDownload = false;
	}

	public static void BroadcastAll(string fun, object msg)
	{
		GameObject[] array = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject gameObject in array)
		{
			if ((bool)gameObject && gameObject.transform.parent == null)
			{
				gameObject.gameObject.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static TextAsset LoadTextAsset(string name)
	{
		foreach (TextAsset textasset in textassetList)
		{
			if (textasset.name == name)
			{
				return textasset;
			}
		}
		return null;
	}

	public static Material LoadMaterial(string name)
	{
		foreach (Material material in materialList)
		{
			if (material.name == name)
			{
				return material;
			}
		}
		return null;
	}

	public static GameObject LoadGameObject(string name)
	{
		foreach (GameObject gameobject in gameobjectList)
		{
			if (gameobject.name == name)
			{
				return gameobject;
			}
		}
		return null;
	}

	public static Texture LoadTexture(string name)
	{
		foreach (Texture texture in textureList)
		{
			if (string.Compare(texture.name, name) == 0)
			{
				return texture;
			}
		}
		return null;
	}

	public static AudioClip LoadAudio(string name)
	{
		foreach (AudioClip audio in audioList)
		{
			if (audio.name == name)
			{
				return audio;
			}
		}
		return null;
	}

	public static Font LoadFont(string name)
	{
		foreach (Font font in fontList)
		{
			if (font.name == name)
			{
				return font;
			}
		}
		return null;
	}

	public static Sprite LoadSprite(string name)
	{
		foreach (Sprite sprite in spriteList)
		{
			if (sprite.name == name)
			{
				return sprite;
			}
		}
		return null;
	}

	private static byte[] UnLoad(byte[] data)
	{
		if (data.Length < 124)
		{
			return null;
		}
		int num = data.Length / 2 * 2;
		for (int i = 0; i < num; i++)
		{
			if (i >= 124)
			{
				byte b = data[i];
				data[i] = data[i + 1];
				data[i + 1] = b;
				i++;
			}
		}
		return data;
	}
}
