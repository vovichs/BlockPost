using UnityEngine;

public class SaveMap : MonoBehaviour
{
	public static bool show;

	private Texture2D tblack;

	private Rect rBack;

	private string sSaved = "КАРТА СОХРАНЕНА";

	public static bool inSave;

	private string[] sSaving = new string[3] { "ИДЕТ СОХРАНЕНИЕ.", "ИДЕТ СОХРАНЕНИЕ..", "ИДЕТ СОХРАНЕНИЕ..." };

	public static bool Saved;

	private string sGameMenu;

	private string sMainMenu;

	public void LoadLang()
	{
		sSaved = Lang.GetString("_MAP_SAVED");
		sSaving[0] = Lang.GetString("_CONSERVATION") + ".";
		sSaving[1] = Lang.GetString("_CONSERVATION") + "..";
		sSaving[2] = Lang.GetString("_CONSERVATION") + "..";
		sGameMenu = Lang.GetString("_GAME_MENU");
		sMainMenu = Lang.GetString("_EXIT_MAIN_MENU");
	}

	public static void SetActive(bool val)
	{
		show = val;
		inSave = false;
		Saved = false;
	}

	public static void Toggle()
	{
		SetActive(!show);
	}

	private void LoadEnd()
	{
		tblack = TEX.GetTextureByName("black");
	}

	private void OnResize()
	{
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(400f), GUIM.YRES(120f), GUIM.YRES(800f), GUIM.YRES(400f));
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIM.DrawBox(rBack, tblack);
			GUIM.DrawText(new Rect(rBack.x + GUIM.YRES(16f), rBack.y + GUIM.YRES(8f), GUIM.YRES(200f), GUIM.YRES(32f)), sGameMenu, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
			if (GUIM.Button(new Rect(rBack.x + GUIM.YRES(300f), rBack.y + GUIM.YRES(60f) + GUIM.YRES(36f) * 8f, GUIM.YRES(200f), GUIM.YRES(32f)), BaseColor.Yellow, sMainMenu, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				Main.SetActive(true);
			}
		}
	}
}
