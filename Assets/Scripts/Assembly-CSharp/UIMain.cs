using UIClass;
using UnityEngine;

public class UIMain : MonoBehaviour
{
	private static bool ENABLE_UI;

	private GameObject goEventSystem;

	private GameObject goCanvasMenu;

	private GameObject goCurrMenu;

	private void Start()
	{
		goEventSystem = GameObject.Find("EventSystem");
		goCanvasMenu = GameObject.Find("CanvasMenu");
		goEventSystem.SetActive(false);
		goCanvasMenu.SetActive(false);
	}

	private void Update()
	{
	}

	private void HideMenu()
	{
		if (goCurrMenu != null)
		{
			Object.Destroy(goCurrMenu);
		}
	}

	private void SelectMenu(CButtonMenu b)
	{
		if (ENABLE_UI)
		{
			Log.Add("UIMain.SelectMenu idx " + b.idx + " codename " + b.codename);
			GameObject gameObject = Resources.Load("prefabs/ui/" + b.codename) as GameObject;
			if (!(gameObject == null))
			{
				goCanvasMenu.SetActive(true);
				goCurrMenu = Object.Instantiate(gameObject, goCanvasMenu.transform);
			}
		}
	}
}
