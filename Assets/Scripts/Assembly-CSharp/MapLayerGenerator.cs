using UnityEngine;

public class MapLayerGenerator : MonoBehaviour
{
	private Texture2D[] layer = new Texture2D[5];

	private int width = 256;

	private int height = 256;

	private void Start()
	{
		layer[0] = Resources.Load("generator/layer0") as Texture2D;
		layer[1] = Resources.Load("generator/layer1") as Texture2D;
		layer[2] = Resources.Load("generator/layer2") as Texture2D;
		layer[3] = Resources.Load("generator/layer3") as Texture2D;
		layer[4] = Resources.Load("generator/layer4") as Texture2D;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.H))
		{
			Generate();
		}
	}

	private void Generate()
	{
		Map.Clear();
		Map.Create(width / Map.CHUNK_SIZE_X, 4, height / Map.CHUNK_SIZE_Z);
		GenerateLayer(layer[0], 0, 41, Palette.GetColor(33), 1);
		GenerateLayer(layer[1], 1, 9, Color.white, 14);
		GenerateLayer(layer[1], 15, 3, Color.white, 1);
		GenerateLayer(layer[2], 16, 9, Color.white, 15);
		GenerateLayer(layer[2], 31, 3, Color.white, 1);
		Map.RenderAll();
	}

	private void GenerateLayer(Texture2D tex, int y, int flag, Color cc, int h)
	{
		if (tex == null)
		{
			Debug.Log("GenerateLayer texture is null");
			return;
		}
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Color pixel = tex.GetPixel(i, j);
				if (pixel.a != 0f && (pixel.r != 1f || pixel.g != 1f || pixel.b != 1f))
				{
					for (int k = y; k < y + h; k++)
					{
						Map.SetBlock(i, k, j, cc, flag);
					}
				}
			}
		}
	}
}
