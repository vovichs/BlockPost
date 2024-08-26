using UnityEngine;

public class dbgNet : MonoBehaviour
{
	public static dbgNet cs;

	private Rect[] rLine;

	private Rect[] rFrame;

	private Texture2D tGreen;

	private Texture2D tYellow;

	private Texture2D tRed;

	private float tFrame;

	private int pFrame;

	private int hsize = 200;

	private int framecount = 512;

	private void Awake()
	{
		cs = this;
		rLine = new Rect[8];
		framecount = Screen.width - 40;
		rFrame = new Rect[framecount];
	}

	private void Start()
	{
		OnResize();
		tGreen = TEX.GetTextureByName("green");
		tYellow = TEX.GetTextureByName("yellow");
		tRed = TEX.GetTextureByName("red");
	}

	private void OnResize()
	{
		for (int i = 0; i < 8; i++)
		{
			rLine[i] = new Rect(GUIM.YRES(200f), GUIM.YRES(400f) + GUIM.YRES(20f) * (float)i, GUIM.YRES(200f), GUIM.YRES(20f));
		}
	}

	private void OnGUI()
	{
		if (Client.cs == null || Client.cs.client == null)
		{
			return;
		}
		for (int i = 0; i < pFrame; i++)
		{
			if (rFrame[i].height >= (float)hsize)
			{
				GUI.DrawTexture(rFrame[i], tRed);
			}
			else if (rFrame[i].height > 10f)
			{
				GUI.DrawTexture(rFrame[i], tYellow);
			}
			else
			{
				GUI.DrawTexture(rFrame[i], tGreen);
			}
		}
	}

	public void AddFrame()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = (realtimeSinceStartup - tFrame) * (float)hsize;
		if (num > (float)hsize)
		{
			num = hsize;
		}
		int num2 = (int)num;
		rFrame[pFrame].Set(20 + pFrame, Screen.height - 20 - num2, 1f, num2);
		pFrame++;
		tFrame = realtimeSinceStartup;
		if (pFrame >= framecount)
		{
			pFrame = 0;
		}
	}
}
