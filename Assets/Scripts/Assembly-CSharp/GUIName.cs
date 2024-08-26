using UnityEngine;

public class GUIName : MonoBehaviour
{
	public static bool show = false;

	private Texture2D tBlack;

	public static string sEdit = "";

	private static bool inEdit = false;

	public static string msg = "";

	public static BaseColor msg_color = BaseColor.White;

	private static Color c8 = new Color(1f, 1f, 1f, 0.65f);

	private Rect rBack;

	private Rect rBack2;

	private Rect rBack3;

	private Rect rEditText;

	private Rect rButton;

	private Rect rEditText2;

	private string sChangeName;

	private string sSaving;

	private string sPurchaseChange;

	private string sBuy;

	private string sAttentName;

	public void LoadLang()
	{
		sChangeName = Lang.GetString("_CHANGE_NAME");
		sSaving = Lang.GetString("_SAVING");
		sPurchaseChange = Lang.GetString("_PURCHASE_CHANGE_NAME");
		sBuy = Lang.GetString("_BUY");
		sAttentName = Lang.GetString("_ATTENTION_NAME");
	}

	public static void SetActive(bool val)
	{
		show = val;
		if (show)
		{
			sEdit = GUIOptions.playername;
			inEdit = true;
			msg = "";
			msg_color = BaseColor.White;
			GUIFX.Set();
		}
		else
		{
			GUIFX.End();
		}
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
	}

	private void OnResize()
	{
		rBack.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(160f));
		rBack2.Set((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f) + GUIM.YRES(168f), GUIM.YRES(920f), GUIM.YRES(120f));
		rBack3.Set(rBack2.x, rBack2.y + rBack2.height + GUIM.YRES(8f), GUIM.YRES(920f), GUIM.YRES(60f));
		rEditText.Set(rBack.x + GUIM.YRES(360f), rBack.y + GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(32f));
		rButton.Set(rBack.x + GUIM.YRES(360f), rBack.y + GUIM.YRES(72f), GUIM.YRES(200f), GUIM.YRES(32f));
		rEditText2.Set(rBack.x + GUIM.YRES(360f) + GUIM.YRES(216f), rBack.y + GUIM.YRES(32f), GUIM.YRES(200f), GUIM.YRES(32f));
	}

	private void OnGUI()
	{
		if (!show)
		{
			return;
		}
		GUIFX.Begin();
		if (inEdit)
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < sEdit.Length; i++)
			{
				char c = sEdit[i];
				if ((c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я'))
				{
					flag2 = true;
					break;
				}
				if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
				{
					flag = true;
					break;
				}
			}
			char character = Event.current.character;
			if ((character < 'а' || character > 'я') && (character < 'А' || character > 'Я') && (character < 'a' || character > 'z') && (character < 'A' || character > 'Z') && (character < '0' || character > '9') && character != '_' && character != '.')
			{
				Event.current.character = '\0';
			}
			if (flag && ((character >= 'а' && character <= 'я') || (character >= 'А' && character <= 'Я')))
			{
				sEdit = "";
			}
			if (flag2 && ((character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z')))
			{
				sEdit = "";
			}
			if (sEdit.Length > 16)
			{
				Event.current.character = '\0';
			}
		}
		GUIM.DrawBox(rBack, tBlack, 0.05f);
		GUI.DrawTexture(rEditText, tBlack);
		if (inEdit && GUIOptions.NameCount > 0)
		{
			GUIM.DrawEdit(rEditText, ref sEdit, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, true);
		}
		else
		{
			GUIM.DrawText(rEditText, GUIOptions.playername, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, true);
			if (GUIM.HideButton(rEditText))
			{
				inEdit = true;
				msg = "";
				msg_color = BaseColor.White;
			}
		}
		GUIM.DrawText(rEditText2, msg, TextAnchor.MiddleLeft, msg_color, 1, 14, true);
		if (GUIOptions.NameCount == 0)
		{
			GUIM.Button(rButton, BaseColor.Block, sChangeName, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false);
		}
		else if (GUIM.Button(rButton, BaseColor.White, sChangeName, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false) && sEdit.Length >= 3)
		{
			GUIOptions.playername = sEdit;
			inEdit = false;
			msg = sSaving;
			msg_color = BaseColor.White;
			MasterClient.cs.send_name(GUIOptions.playername);
		}
		GUIM.DrawText(new Rect(rBack.x, rBack.y + GUIM.YRES(120f), rBack.width, GUIM.YRES(20f)), GUIOptions.sNameCount2, TextAnchor.MiddleCenter, BaseColor.Yellow, 1, 14, true);
		GUIM.DrawBox(rBack2, tBlack, 0.05f);
		GUIM.DrawText(rBack2, sAttentName, TextAnchor.MiddleCenter, BaseColor.LightGray2, 1, 14, true);
		if (GUIOptions.NameCount == 0)
		{
			GUIM.DrawBox(rBack3, tBlack, 0.05f);
			GUIM.DrawText(new Rect(rBack3.x + GUIM.YRES(280f), rBack3.y + GUIM.YRES(16f), GUIM.YRES(120f), GUIM.YRES(28f)), sPurchaseChange, TextAnchor.MiddleCenter, BaseColor.White, 1, 14, true);
			GUI.color = c8;
			GUI.DrawTexture(new Rect(rBack3.x + GUIM.YRES(460f), rBack3.y + GUIM.YRES(16f), GUIM.YRES(60f), GUIM.YRES(28f)), tBlack);
			GUI.color = Color.white;
			GUIM.DrawText(new Rect(rBack3.x + GUIM.YRES(460f), rBack3.y + GUIM.YRES(20f), GUIM.YRES(40f), GUIM.YRES(28f)), "5", TextAnchor.MiddleCenter, BaseColor.White, 1, 20, false);
			GUI.DrawTexture(new Rect(rBack3.x + GUIM.YRES(492f), rBack3.y + GUIM.YRES(20f), GUIM.YRES(20f), GUIM.YRES(20f)), Main.tCoin);
			if (GUIM.Button(new Rect(rBack3.x + GUIM.YRES(530f), rBack3.y + GUIM.YRES(16f), GUIM.YRES(80f), GUIM.YRES(28f)), BaseColor.White, sBuy, TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				MasterClient.cs.send_buyname();
			}
		}
		GUIFX.End();
	}
}
