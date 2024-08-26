using System.Collections.Generic;
using UnityEngine;

public class GUIDevDemoViewer : MonoBehaviour
{
	private List<string> demolist = new List<string>();

	private float leftx;

	private Rect rBack;

	private Rect rTypeHeader;

	private Vector2 sv = Vector2.zero;

	private Vector2 svs = Vector2.zero;

	private int currdemo = -1;

	private string[] statsdemo;

	private float statsdemo_rh;

	private void Start()
	{
		demo_loadlist();
		OnResize();
	}

	private void OnResize()
	{
		rBack = new Rect((float)Screen.width / 2f - GUIM.YRES(460f), GUIM.YRES(140f), GUIM.YRES(920f), GUIM.YRES(500f));
		rTypeHeader = new Rect(rBack.x + GUIM.YRES(16f), rBack.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(32f));
		leftx = rBack.x + GUIM.YRES(16f);
	}

	private void OnGUI()
	{
		Draw();
	}

	private void Draw()
	{
		GUIM.DrawBox(rBack, TEX.tBlack, 0.05f);
		DrawList();
		DrawStats();
		DrawButton();
		DrawDemoRec();
	}

	private void DrawList()
	{
		if (demolist == null)
		{
			return;
		}
		Rect r = new Rect(leftx + GUIM.YRES(32f), rBack.y + GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(32f));
		Rect rect = new Rect(leftx + GUIM.YRES(32f), rBack.y + GUIM.YRES(16f) + GUIM.YRES(36f), GUIM.YRES(200f), GUIM.YRES(400f));
		GUIM.DrawText(r, "DEMO LIST", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		GUI.DrawTexture(rect, TEX.tBlackAlpha);
		sv = GUIM.BeginScrollView(rect, sv, new Rect(0f, 0f, GUIM.YRES(180f), (float)demolist.Count * GUIM.YRES(26f)));
		for (int i = 0; i < demolist.Count; i++)
		{
			Rect r2 = new Rect(0f, 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(100f), GUIM.YRES(20f));
			BaseColor fontcolor = BaseColor.White;
			if (GUIM.Contains(r2))
			{
				fontcolor = BaseColor.Yellow;
			}
			if (currdemo == i)
			{
				fontcolor = BaseColor.Green;
			}
			GUIM.DrawText(new Rect(0f, 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(20f), GUIM.YRES(20f)), i.ToString(), TextAnchor.MiddleCenter, fontcolor, 1, 12, true);
			GUIM.DrawText(new Rect(GUIM.YRES(22f), 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(80f), GUIM.YRES(20f)), demolist[i], TextAnchor.MiddleLeft, fontcolor, 1, 12, true);
			if (GUIM.HideButton(r2))
			{
				currdemo = i;
				statsdemo = DemoRec.LoadStats(demolist[i]);
			}
		}
		GUIM.EndScrollView();
	}

	private void DrawStats()
	{
		Rect r = new Rect(leftx + GUIM.YRES(300f) + GUIM.YRES(32f), rBack.y + GUIM.YRES(16f), GUIM.YRES(500f), GUIM.YRES(32f));
		Rect rect = new Rect(leftx + GUIM.YRES(300f) + GUIM.YRES(32f), rBack.y + GUIM.YRES(16f) + GUIM.YRES(36f), GUIM.YRES(500f), GUIM.YRES(400f));
		GUIM.DrawText(r, "DEMO STATS", TextAnchor.MiddleCenter, BaseColor.White, 0, 20, false);
		GUI.DrawTexture(rect, TEX.tBlackAlpha);
		if (currdemo < 0)
		{
			return;
		}
		if (statsdemo == null)
		{
			Rect r2 = new Rect(rect.center.x - GUIM.YRES(196f) / 2f, rect.center.y - GUIM.YRES(28f) / 2f, GUIM.YRES(196f), GUIM.YRES(28f));
			GUIM.DrawText(new Rect(r2.x, r2.y - GUIM.YRES(32f), GUIM.YRES(196f), GUIM.YRES(28f)), "DEMO STATS NOT FOUND", TextAnchor.MiddleCenter, BaseColor.Orange, 1, 12, true);
			if (GUIM.Button(r2, BaseColor.White, "STATS DEMO", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
			{
				Console.cs.Command("statsdemo " + demolist[currdemo]);
			}
			return;
		}
		svs = GUIM.BeginScrollView(rect, svs, new Rect(0f, 0f, GUIM.YRES(180f), statsdemo_rh));
		int num = 0;
		for (int i = 0; i < statsdemo.Length; i++)
		{
			if (statsdemo[i].Length > 1)
			{
				new Rect(0f, 0f + (float)i * GUIM.YRES(26f), GUIM.YRES(500f), GUIM.YRES(20f));
				GUIM.DrawText(new Rect(GUIM.YRES(22f), 0f + (float)num * GUIM.YRES(26f), GUIM.YRES(80f), GUIM.YRES(20f)), statsdemo[i], TextAnchor.MiddleLeft, BaseColor.White, 1, 12, true);
				num++;
			}
		}
		GUIM.EndScrollView();
		statsdemo_rh = (float)num * GUIM.YRES(26f);
	}

	private void DrawButton()
	{
		if (currdemo >= 0 && GUIM.Button(new Rect(rBack.x + rBack.width - GUIM.YRES(208f), rBack.y + rBack.height - GUIM.YRES(40f), GUIM.YRES(196f), GUIM.YRES(28f)), BaseColor.White, "PLAY DEMO", TextAnchor.MiddleCenter, BaseColor.Black, 0, 20, false))
		{
			Console.cs.Command("playdemo " + demolist[currdemo]);
		}
	}

	public void demo_loadlist()
	{
	}

	private void DrawDemoRec()
	{
		Rect r = new Rect(rBack.x + rBack.width - GUIM.YRES(120f), rBack.y + GUIM.YRES(8f), GUIM.YRES(92f), GUIM.YRES(16f));
		Rect rect = new Rect(rBack.x + rBack.width - GUIM.YRES(24f), rBack.y + GUIM.YRES(8f), GUIM.YRES(16f), GUIM.YRES(16f));
		Rect position = new Rect(rect.x + 2f, rect.y + 2f, rect.width - 4f, rect.height - 4f);
		GUI.DrawTexture(rect, TEX.tBlack);
		if (DemoRec.active)
		{
			GUIM.DrawText(r, "DEMO RECORD ENABLED", TextAnchor.MiddleRight, BaseColor.Green, 0, 14, false);
			GUI.DrawTexture(position, TEX.tWhite);
		}
		else
		{
			GUIM.DrawText(r, "DEMO RECORD DISABLED", TextAnchor.MiddleRight, BaseColor.Orange, 0, 14, false);
		}
		if (GUIM.HideButton(rect))
		{
			if (DemoRec.active)
			{
				Console.cs.Command("demorec 0");
			}
			else
			{
				Console.cs.Command("demorec 1");
			}
		}
	}
}
