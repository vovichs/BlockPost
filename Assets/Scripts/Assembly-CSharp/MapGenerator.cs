using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	private bool showmenu;

	private static int width = 256;

	private static int height = 256;

	private static int mapheight = 64;

	private Texture2D tex;

	private Texture2D col;

	private Color[] tpix;

	private Color[] cpix;

	private static Color c;

	private static float[,] noiseMap = null;

	private float scale = 1f;

	private int octaves = 4;

	private float persistance = 0.5f;

	private float lacunarity = 1.5f;

	private Vector2 offset = Vector2.zero;

	private static bool changed = false;

	private static float forg;

	private static int iorg;

	private void Start()
	{
		width = GUIMap.arrmapsize[GUIMap.mapsize] * 16;
		height = GUIMap.arrmapsize[GUIMap.mapsize] * 16;
		tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Clamp;
		col = new Texture2D(width, height, TextureFormat.RGBA32, false);
		col.filterMode = FilterMode.Point;
		col.wrapMode = TextureWrapMode.Clamp;
		tpix = new Color[width * height];
		cpix = new Color[width * height];
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			showmenu = !showmenu;
			Crosshair.SetCursor(showmenu);
			Controll.SetLockLook(showmenu);
		}
	}

	private void OnGUI()
	{
		if (showmenu)
		{
			changed = false;
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), TEX.tBlack);
			GUI.color = Color.white;
			Rect r = new Rect(GUIM.YRES(220f), GUIM.YRES(120f), GUIM.YRES(120f), GUIM.YRES(32f));
			Rect r2 = new Rect(GUIM.YRES(220f) + GUIM.YRES(140f), GUIM.YRES(120f), GUIM.YRES(120f), GUIM.YRES(32f));
			Rect r3 = new Rect(GUIM.YRES(220f), GUIM.YRES(120f) + GUIM.YRES(40f), GUIM.YRES(120f), GUIM.YRES(32f));
			Rect r4 = new Rect(GUIM.YRES(220f), GUIM.YRES(120f) + GUIM.YRES(40f) * 2f, GUIM.YRES(120f), GUIM.YRES(32f));
			Rect r5 = new Rect(GUIM.YRES(220f), GUIM.YRES(120f) + GUIM.YRES(40f) * 3f, GUIM.YRES(120f), GUIM.YRES(32f));
			Rect r6 = new Rect(GUIM.YRES(220f), GUIM.YRES(120f) + GUIM.YRES(40f) * 4f, GUIM.YRES(120f), GUIM.YRES(32f));
			if (GUIM.Button(r, BaseColor.White, "GENERATE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 14, false))
			{
				Random.seed = (int)(Time.time * 1000f);
				noiseMap = NoiseMap.Generate(width, height, Random.seed, scale, octaves, persistance, lacunarity, offset);
				CreateTex();
			}
			if (GUIM.Button(r2, BaseColor.White, "VISUALIZE", TextAnchor.MiddleCenter, BaseColor.Black, 0, 14, false))
			{
				CreateMap();
			}
			if (GUIM.Button(new Rect(r2.x + r2.width + GUIM.YRES(8f), r2.y, GUIM.YRES(32f), GUIM.YRES(32f)), BaseColor.Blue, "PIC", TextAnchor.MiddleCenter, BaseColor.White, 0, 14, false))
			{
				Save();
			}
			DrawSlider(r3, "SCALE", 1, 200, ref scale);
			DrawSlider(r4, "OCTAVES", 0, 8, ref octaves);
			DrawSlider(r5, "PERSISTANCE", 0, 2, ref persistance);
			DrawSlider(r6, "LACUNARITY", 1, 10, ref lacunarity);
			Rect position = new Rect((float)Screen.width / 2f - GUIM.YRES(40f), GUIM.YRES(120f), GUIM.YRES(400f), GUIM.YRES(400f));
			if (tex == null)
			{
				GUI.DrawTexture(position, TEX.GetTextureByName("gray0"));
			}
			else
			{
				GUI.DrawTexture(position, tex);
			}
		}
	}

	private void CreateTex()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				tpix[i * height + j] = Color.Lerp(Color.black, Color.white, noiseMap[j, i]);
				if (noiseMap[j, i] < 0.4f)
				{
					cpix[i * height + j] = new Color32(70, 150, 210, byte.MaxValue);
				}
				else if (noiseMap[j, i] < 0.45f)
				{
					cpix[i * height + j] = new Color32(230, 220, 60, byte.MaxValue);
				}
				else if (noiseMap[j, i] < 0.55f)
				{
					cpix[i * height + j] = new Color32(100, 180, 20, byte.MaxValue);
				}
				else if (noiseMap[j, i] < 0.6f)
				{
					cpix[i * height + j] = new Color32(60, 120, 10, byte.MaxValue);
				}
				else if (noiseMap[j, i] < 0.7f)
				{
					cpix[i * height + j] = new Color32(100, 70, 30, byte.MaxValue);
				}
				else if (noiseMap[j, i] < 0.9f)
				{
					cpix[i * height + j] = new Color32(50, 30, 15, byte.MaxValue);
				}
				else
				{
					cpix[i * height + j] = new Color32(200, 200, 200, byte.MaxValue);
				}
			}
		}
		tex.SetPixels(cpix);
		tex.Apply(false);
	}

	private void CreateMap()
	{
		Map.Clear();
		Map.Create(width / 16, 4, height / 16);
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				int num = (int)(tpix[i * height + j].r * (float)mapheight) - 1;
				for (int k = 0; k < num; k++)
				{
					Map.SetBlock(i, k, j, cpix[i * height + j], 40);
				}
			}
		}
		Map.RenderAll();
	}

	private void DrawSlider(Rect r, string label, int min, int max, ref float param)
	{
		forg = param;
		GUIM.DrawBox(r, TEX.GetTextureByName("black"));
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y, GUIM.YRES(200f), GUIM.YRES(32f)), label, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, true);
		param = GUIM.DrawSlider(new Rect(r.x + GUIM.YRES(120f), r.y + GUIM.YRES(10f), GUIM.YRES(200f), GUIM.YRES(32f)), (int)GUIM.YRES(200f), min, max, param);
		param = (float)(int)(param * 10f + 0.05f) / 10f;
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(260f), r.y, GUIM.YRES(40f), GUIM.YRES(32f)), param.ToString("0.0"), TextAnchor.MiddleRight, BaseColor.White, 0, 20, true);
		if (param != forg)
		{
			changed = true;
		}
	}

	private void DrawSlider(Rect r, string label, int min, int max, ref int param)
	{
		iorg = param;
		GUIM.DrawBox(r, TEX.GetTextureByName("black"));
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(16f), r.y, GUIM.YRES(200f), GUIM.YRES(32f)), label, TextAnchor.MiddleLeft, BaseColor.Gray, 0, 20, true);
		forg = GUIM.DrawSlider(new Rect(r.x + GUIM.YRES(120f), r.y + GUIM.YRES(10f), GUIM.YRES(200f), GUIM.YRES(32f)), (int)GUIM.YRES(200f), min, max, param);
		forg = (float)(int)(forg * 10f + 0.05f) / 10f;
		GUIM.DrawText(new Rect(r.x + GUIM.YRES(260f), r.y, GUIM.YRES(40f), GUIM.YRES(32f)), forg.ToString("0"), TextAnchor.MiddleRight, BaseColor.White, 0, 20, true);
		param = (int)forg;
		if (param != iorg)
		{
			changed = true;
		}
	}

	private void Save()
	{
		bool flag = tex == null;
	}
}
