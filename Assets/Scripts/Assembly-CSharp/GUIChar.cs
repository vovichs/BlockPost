using UnityEngine;

public class GUIChar : MonoBehaviour
{
	public static bool show;

	private static Texture2D tblack;

	private static Rect rBack;

	private static GameObject goChar;

	private int bstate;

	private int wstate;

	private float start_x;

	public static void SetActive(bool val)
	{
		show = val;
		if (show)
		{
			goChar = VCGen.Build(Controll.pl);
			goChar.transform.position = new Vector3(0.5f, 0.5f, 5f);
		}
		else if (goChar != null)
		{
			Object.Destroy(goChar);
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			int num = wstate;
			int num2 = bstate;
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 0f, GUIM.YRES(200f), GUIM.YRES(32f)), (bstate != 0) ? BaseColor.White : BaseColor.Green, "STAY", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				bstate = 0;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 1f, GUIM.YRES(200f), GUIM.YRES(32f)), (bstate != 1) ? BaseColor.White : BaseColor.Green, "RUN", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				bstate = 1;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 2f, GUIM.YRES(200f), GUIM.YRES(32f)), (bstate != 2) ? BaseColor.White : BaseColor.Green, "(CROUCH)STAY", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				bstate = 2;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 3f, GUIM.YRES(200f), GUIM.YRES(32f)), (bstate != 3) ? BaseColor.White : BaseColor.Green, "(CROUCH)CROUCH", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				bstate = 3;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 4f, GUIM.YRES(200f), GUIM.YRES(32f)), (bstate != 4) ? BaseColor.White : BaseColor.Green, "JUMP", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				bstate = 4;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 5f, GUIM.YRES(200f), GUIM.YRES(32f)), (bstate != 5) ? BaseColor.White : BaseColor.Green, "DEAD", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				bstate = 5;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 7f, GUIM.YRES(200f), GUIM.YRES(32f)), (wstate == 0) ? BaseColor.Green : BaseColor.Yellow, "NO WEAPON", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				wstate = 0;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 8f, GUIM.YRES(200f), GUIM.YRES(32f)), (wstate == 1) ? BaseColor.Green : BaseColor.Yellow, "WEAPON PRIMARY", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				wstate = 1;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 9f, GUIM.YRES(200f), GUIM.YRES(32f)), (wstate == 2) ? BaseColor.Green : BaseColor.Yellow, "WEAPON SECONDARY", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				wstate = 2;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 10f, GUIM.YRES(200f), GUIM.YRES(32f)), (wstate == 3) ? BaseColor.Green : BaseColor.Yellow, "WEAPON MELLE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				wstate = 3;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 11f, GUIM.YRES(200f), GUIM.YRES(32f)), (wstate == 4) ? BaseColor.Green : BaseColor.Yellow, "WEAPON BLOCK", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				wstate = 4;
			}
			if (GUIM.Button(new Rect(GUIM.YRES(20f), GUIM.YRES(120f) + GUIM.YRES(40f) * 12f, GUIM.YRES(200f), GUIM.YRES(32f)), (wstate == 5) ? BaseColor.Green : BaseColor.Yellow, "WEAPON GRENADE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				wstate = 5;
			}
			if (num != wstate)
			{
				Controll.pl.wstate = wstate;
			}
			if (num2 != bstate)
			{
				Controll.pl.bstate = bstate;
			}
		}
	}

	private void Update()
	{
		if (show)
		{
			if (Input.GetMouseButtonDown(0))
			{
				start_x = Input.mousePosition.x;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				start_x = -1f;
			}
			if (start_x > 0f)
			{
				float x = Input.mousePosition.x;
				float num = x - start_x;
				start_x = x;
				goChar.transform.eulerAngles = new Vector3(0f, goChar.transform.eulerAngles.y - num, 0f);
			}
		}
	}
}
