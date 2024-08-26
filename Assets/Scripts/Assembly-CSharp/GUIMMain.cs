using UnityEngine;
using UnityEngine.Rendering;

public class GUIMMain : MonoBehaviour
{
	public static GUIMMain cs = null;

	public static bool show = true;

	private Rect rVig;

	private Rect rMenuBack;

	private Rect rTopBack;

	private Rect[] rItem;

	private Rect rAvatar;

	private Rect rNameText;

	private Rect rCoin;

	private Rect rCoinText;

	private Texture2D tVig;

	private Texture2D tBlack;

	private Texture2D tWhite;

	private Texture2D tIconInv;

	private Texture2D tIconOptions;

	private Texture2D tIconPlay;

	private Texture2D tIconProfile;

	private Texture2D tIconShop;

	private Texture2D tIconWork;

	private Texture2D tCoin;

	private Color ca = new Color(1f, 1f, 1f, 0.5f);

	public static void Init()
	{
		RenderSettings.skybox = null;
		RenderSettings.ambientMode = AmbientMode.Flat;
		RenderSettings.ambientSkyColor = Color.white;
	}

	private void Awake()
	{
		cs = this;
	}

	private void LoadEnd()
	{
		tVig = ContentLoader_.LoadTexture("vig") as Texture2D;
		tBlack = TEX.GetTextureByName("black");
		tWhite = TEX.GetTextureByName("white");
		tIconInv = ContentLoader_.LoadTexture("inventory_icon") as Texture2D;
		tIconOptions = ContentLoader_.LoadTexture("options_icon") as Texture2D;
		tIconPlay = ContentLoader_.LoadTexture("play_icon") as Texture2D;
		tIconProfile = ContentLoader_.LoadTexture("profile_icon") as Texture2D;
		tIconShop = ContentLoader_.LoadTexture("shop_icon") as Texture2D;
		tIconWork = ContentLoader_.LoadTexture("workshop_icon") as Texture2D;
		tCoin = ContentLoader_.LoadTexture("coin") as Texture2D;
		UtilChar.InitColors();
	}

	private void OnResize()
	{
		rVig = new Rect(0f, 0f, Screen.width, Screen.height);
		rMenuBack = new Rect(0f, (float)Screen.height - GUIM.YRES(160f), Screen.width, GUIM.YRES(160f));
		rTopBack = new Rect(0f, 0f, Screen.width, GUIM.YRES(92f));
		rAvatar = new Rect((float)Screen.width / 2f + GUIM.YRES(200f), GUIM.YRES(22f), GUIM.YRES(48f), GUIM.YRES(48f));
		rNameText = new Rect(rAvatar.x + GUIM.YRES(60f), GUIM.YRES(34f), GUIM.YRES(160f), GUIM.YRES(32f));
		rCoin = new Rect((float)Screen.width / 2f + GUIM.YRES(400f), GUIM.YRES(22f), GUIM.YRES(48f), GUIM.YRES(48f));
		rCoinText = new Rect((float)Screen.width / 2f + GUIM.YRES(460f), GUIM.YRES(34f), GUIM.YRES(100f), GUIM.YRES(32f));
		ResizeMenu();
	}

	private void ResizeMenu()
	{
		rItem = new Rect[6];
		int num = (int)GUIM.YRES(26f);
		int num2 = (int)GUIM.YRES(112f);
		int num3 = (int)GUIM.YRES(48f);
		int num4 = (int)GUIM.YRES(60f);
		int num5 = (int)GUIM.YRES(100f);
		rItem[2] = new Rect((float)Screen.width / 2f - (float)num2 / 2f, rMenuBack.y + (float)num, num2, num2);
		rItem[1] = new Rect(rItem[2].x - (float)num5 - (float)num4, rMenuBack.y + (float)num3, num4, num4);
		rItem[0] = new Rect(rItem[1].x - (float)num5 - (float)num4, rMenuBack.y + (float)num3, num4, num4);
		rItem[3] = new Rect(rItem[2].x + (float)num5 + (float)num2, rMenuBack.y + (float)num3, num4, num4);
		rItem[4] = new Rect(rItem[3].x + (float)num5 + (float)num4, rMenuBack.y + (float)num3, num4, num4);
	}

	public void _OnGUI()
	{
		GUI.color = Color.black;
		GUI.DrawTexture(rVig, tVig);
		GUI.color = Color.white;
		DrawTop();
		DrawMenu();
	}

	private void DrawTop()
	{
		GUI.color = ca;
		GUI.DrawTexture(rTopBack, tBlack);
		GUI.color = Color.white;
		DrawName();
		DrawMoney();
	}

	private void DrawMenu()
	{
		GUI.color = ca;
		GUI.DrawTexture(rMenuBack, tBlack);
		GUI.color = Color.white;
		GUI.DrawTexture(rItem[0], tIconWork);
		GUI.DrawTexture(rItem[1], tIconInv);
		GUI.DrawTexture(rItem[2], tIconPlay);
		GUI.DrawTexture(rItem[3], tIconShop);
		GUI.DrawTexture(rItem[4], tIconProfile);
		if (GUIM.HideButton(rItem[2]))
		{
			GUIMPlay.SetActive(true);
		}
	}

	private void DrawName()
	{
		GUI.color = ca;
		GUI.DrawTexture(rAvatar, tBlack);
		GUI.color = Color.white;
		if (Main.tAvatar != null)
		{
			GUI.DrawTexture(rAvatar, Main.tAvatar);
		}
		GUIM.DrawText(rNameText, GUIOptions.playername, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 24, true);
	}

	private void DrawMoney()
	{
		GUI.DrawTexture(rCoin, tCoin);
		GUIM.DrawText(rCoinText, GUIOptions.sGold, TextAnchor.MiddleLeft, BaseColor.LightGray2, 1, 24, true);
	}
}
