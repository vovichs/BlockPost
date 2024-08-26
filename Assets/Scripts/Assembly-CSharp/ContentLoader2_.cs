using UnityEngine;

public class ContentLoader2_ : MonoBehaviour
{
	public static float currprogress;

	public static float progress;

	public static Texture2D tLogo;

	public static Texture2D tLogoEn;

	private void Update()
	{
		if (ContentLoader_.LoadList.Count != 0 && !ContentLoader_.inDownload)
		{
			Load(ContentLoader_.LoadList[0].name, ContentLoader_.LoadList[0].version, ContentLoader_.LoadList[0].crypted);
			ContentLoader_.currBundle = ContentLoader_.LoadList[0];
			ContentLoader_.LoadList.RemoveAt(0);
			progress = (float)(ContentLoader_.maxcontentcount - ContentLoader_.LoadList.Count) / (float)ContentLoader_.maxcontentcount;
		}
	}

	private void OnGUI()
	{
		if (ContentLoader_.inDownload || ContentLoader_.LoadList.Count > 0 || currprogress < 0.99f)
		{
			GUI.depth = -2;
			float num = 0f;
			if (ContentLoader_.LoadList.Count == ContentLoader_.maxcontentcount - 1)
			{
				num = 1f;
			}
			else
			{
				num = 1f - currprogress;
				num *= num;
			}
			GUI.color = new Color(1f, 1f, 1f, num);
			if (tLogo == null)
			{
				tLogo = Resources.Load("preload/gamelogo_start") as Texture2D;
			}
			if (tLogoEn == null)
			{
				tLogoEn = Resources.Load("preload/gamelogo_start_en") as Texture2D;
			}
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), TEX.GetTextureByName("gray0"));
			if (Lang.lang == 0)
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - GUIM.YRES(160f), GUIM.YRES(80f), GUIM.YRES(320f), GUIM.YRES(160f)), tLogo);
			}
			else
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - GUIM.YRES(160f), GUIM.YRES(80f), GUIM.YRES(320f), GUIM.YRES(160f)), tLogoEn);
			}
			GUI.color = Color.white;
			currprogress = Mathf.Lerp(currprogress, progress, Time.deltaTime * 1f);
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - GUIM.YRES(200f), (float)Screen.height - GUIM.YRES(80f), GUIM.YRES(400f), GUIM.YRES(16f)), TEX.GetTextureByName("gray2"));
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - GUIM.YRES(200f) + GUIM.YRES(4f), (float)Screen.height - GUIM.YRES(80f) + GUIM.YRES(4f), GUIM.YRES(392f) * currprogress, GUIM.YRES(8f)), TEX.GetTextureByName("white"));
			Rect position = new Rect((float)Screen.width / 2f - GUIM.YRES(16f), (float)Screen.height - GUIM.YRES(150f), GUIM.YRES(400f), GUIM.YRES(60f));
			GUI.color = Color.black;
			GUI.Label(new Rect(position.x + 1f, position.y + 1f, position.width, position.height), "<size=30><b>" + (currprogress * 100f).ToString("0") + "%</b></size>");
			GUI.color = Color.white;
			GUI.Label(position, "<size=30><b>" + (currprogress * 100f).ToString("0") + "%</b></size>");
		}
	}

	public void Load(string svalue, int version, bool crypted)
	{
		ContentLoader_.inDownload = true;
		StartCoroutine(ContentLoader_.cLoad(svalue, version, crypted));
	}

	public void LoadEnd()
	{
	}
}
