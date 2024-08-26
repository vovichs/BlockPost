using System;
using UnityEngine;

public static class NoiseMap
{
	public static float[,] Generate(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
	{
		float[,] array = new float[mapWidth, mapHeight];
		System.Random random = new System.Random(seed);
		Vector2[] array2 = new Vector2[octaves];
		for (int i = 0; i < octaves; i++)
		{
			float x = (float)random.Next(-100000, 100000) + offset.x;
			float y = (float)random.Next(-100000, 100000) + offset.y;
			array2[i] = new Vector2(x, y);
		}
		if (scale <= 0f)
		{
			scale = 0.0001f;
		}
		float num = float.MinValue;
		float num2 = float.MaxValue;
		float num3 = (float)mapWidth / 2f;
		float num4 = (float)mapHeight / 2f;
		for (int j = 0; j < mapHeight; j++)
		{
			for (int k = 0; k < mapWidth; k++)
			{
				float num5 = 1f;
				float num6 = 1f;
				float num7 = 0f;
				for (int l = 0; l < octaves; l++)
				{
					float x2 = ((float)k - num3) / scale * num6 + array2[l].x;
					float y2 = ((float)j - num4) / scale * num6 + array2[l].y;
					float num8 = Mathf.PerlinNoise(x2, y2) * 2f - 1f;
					num7 += num8 * num5;
					num5 *= persistance;
					num6 *= lacunarity;
				}
				if (num7 > num)
				{
					num = num7;
				}
				else if (num7 < num2)
				{
					num2 = num7;
				}
				array[k, j] = num7;
			}
		}
		for (int m = 0; m < mapHeight; m++)
		{
			for (int n = 0; n < mapWidth; n++)
			{
				array[n, m] = Mathf.InverseLerp(num2, num, array[n, m]);
			}
		}
		return array;
	}
}
