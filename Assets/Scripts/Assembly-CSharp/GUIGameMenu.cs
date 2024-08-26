using UnityEngine;

public class GUIGameMenu : MonoBehaviour
{
	public static bool show;

	private static Texture2D tLogo;

	private static Texture2D tVig;

	private static Texture2D tBlack;

	private static Texture2D tOptions;

	private static Texture2D tWhite;

	private static Rect rScreen;

	private static Rect rOptions;

	private Vector2 pOptions;

	private static Rect rFull;

	private float angle;

	private Vector2 pFull;

	private float scale = 1f;

	private int scaleforward = 1;

	private static Texture2D tFullScreen;

	private Color cg = new Color(0.8f, 0.8f, 0.8f, 1f);

	public static bool createmap;

	public static bool savemap;

	private Rect rBottom;

	private Rect rLeft;

	private Rect rRight;

	public static void SetActive(bool val)
	{
		show = val;
		Crosshair.SetCursor(show);
		Controll.SetLockAttack(show);
		Controll.SetLockLook(show);
		GUIGameExit.SetActive(show);
		HUD.OnResize_Score();
		if (GUIAdmin.cs != null && GUIAdmin.cs.show)
		{
			GUIAdmin.cs.SetActive(false);
		}
	}

	public static void Toggle()
	{
		SetActive(!show);
	}

	private void LoadEnd()
	{
		tVig = ContentLoader_.LoadTexture("vig") as Texture2D;
		tBlack = TEX.GetTextureByName("gray0");
		tWhite = TEX.GetTextureByName("white");
		tOptions = ContentLoader_.LoadTexture("options") as Texture2D;
		tFullScreen = Resources.Load("fullscreen") as Texture2D;
	}

	private void OnResize()
	{
		rScreen = new Rect(0f, 0f, Screen.width, Screen.height);
		rOptions = new Rect((float)Screen.width - GUIM.YRES(64f), GUIM.YRES(12f), GUIM.YRES(40f), GUIM.YRES(40f));
		pOptions = new Vector2(rOptions.xMin + rOptions.width * 0.5f, rOptions.yMin + rOptions.height * 0.5f);
		rFull = new Rect((float)Screen.width - GUIM.YRES(120f), GUIM.YRES(12f), GUIM.YRES(40f), GUIM.YRES(40f));
		pFull = new Vector2(rFull.xMin + rFull.width * 0.5f, rFull.yMin + rFull.height * 0.5f);
		rBottom = new Rect(0f, (float)Screen.height - GUIM.YRES(32f), Screen.width, GUIM.YRES(32f));
		rLeft = new Rect(GUIM.YRES(16f), (float)Screen.height - GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(32f));
		rRight = new Rect((float)Screen.width - GUIM.YRES(216f), (float)Screen.height - GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(32f));
	}

	private void OnGUI()
	{
		if (show)
		{
			GUI.color = Color.black;
			GUI.DrawTexture(rScreen, tVig);
			GUI.color = Color.white;
			GUIM.DrawBox(new Rect(0f, 0f, Screen.width, GUIM.YRES(64f)), tBlack);
			if (Lang.lang == 0)
			{
				GUI.DrawTexture(Main.rLogoMini, Main.tLogo_mini);
			}
			else
			{
				GUI.DrawTexture(Main.rLogoMini, Main.tLogoEn_mini);
			}
			Matrix4x4 matrix2 = GUI.matrix;
			if (GUIM.Contains(rOptions))
			{
				GUI.color = Color.yellow;
				angle += 64f * Time.smoothDeltaTime;
			}
			else
			{
				GUI.color = cg;
			}
			if (angle > 45f)
			{
				angle -= 45f;
			}
			float angle2 = angle;
			Matrix4x4 matrix = GUI.matrix;
			GUIUtility.RotateAroundPivot(angle, pOptions);
			GUI.DrawTexture(rOptions, tOptions);
			GUI.matrix = matrix;
			if (GUIM.HideButton(rOptions))
			{
				GUIGameExit.SetActive(false);
				Main.SelectMenu(7, 0);
				GUIOptions.SetActive(true);
				GUIOptions.showingame = true;
			}
			GUIM.DrawBox(rBottom, tBlack);
			GUIM.DrawText(rLeft, Main.sLeftMsg, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
			GUIM.DrawText(rRight, Main.sRightMsg, TextAnchor.MiddleRight, BaseColor.White, 0, 20, true);
		}
	}
}
