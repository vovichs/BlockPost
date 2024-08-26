using UnityEngine;

public class MapLoader : MonoBehaviour
{
	public static void GenerateTerrain()
	{
		for (int i = 0; i < 128; i++)
		{
			for (int j = 0; j < 128; j++)
			{
				TMap.SetBlock(i, Random.Range(0, 4), j, 1, Color.gray);
			}
		}
		TMap.FixHeight();
		TMap.RenderAll(true);
	}

	public static void GenerateMap()
	{
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 256; j++)
			{
				Map.SetBlock(i, 0, j, Color.white, 1);
			}
		}
		for (int k = 0; k < 32; k++)
		{
			int x = Random.Range(0, 256);
			int y = 2;
			int z = Random.Range(0, 256);
			int x2 = Random.Range(0, 32);
			int y2 = Random.Range(0, 32);
			int z2 = Random.Range(0, 32);
			BuildBox(x, y, z, x2, y2, z2);
		}
		Map.SetBlock(8, 0, 8, Color.yellow, 1);
		Map.SetBlock(8, 1, 8, Color.yellow, 1);
		Map.SetBlock(8, 2, 8, Color.yellow, 1);
		Map.SetBlock(8, 3, 8, Color.yellow, 1);
		for (int l = 0; l < 10; l++)
		{
			Map.SetBlock(0, l, 0, Color.red, 1);
			Map.SetBlock(255, l, 0, Color.red, 1);
			Map.SetBlock(0, l, 255, Color.red, 1);
			Map.SetBlock(255, l, 255, Color.red, 1);
		}
		Map.RenderAll();
	}

	public static void BuildBox(int x1, int y1, int z1, int x2, int y2, int z2)
	{
	}

	public static void GenerateClearMap(int flag, int color)
	{
		Color color2 = Palette.GetColor(color);
		for (int i = 0; i < Map.BLOCK_SIZE_X; i++)
		{
			for (int j = 0; j < Map.BLOCK_SIZE_Z; j++)
			{
				Map.SetBlock(i, 0, j, color2, flag);
			}
		}
		Map.RenderAll();
	}
}
