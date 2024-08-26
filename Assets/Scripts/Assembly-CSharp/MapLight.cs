using UnityEngine;

public class MapLight : MonoBehaviour
{
	public static int[,] data;

	private static Texture2D tDebug;

	public static void GenerateLight()
	{
		data = new int[Map.BLOCK_SIZE_X, Map.BLOCK_SIZE_Z];
		for (int i = 0; i < Map.BLOCK_SIZE_X; i++)
		{
			for (int j = 0; j < Map.BLOCK_SIZE_Z; j++)
			{
				data[i, j] = GetHighBlock(i, j);
			}
		}
	}

	public static void ClearData()
	{
		data = null;
	}

	private static int GetHighBlock(int x, int z)
	{
		for (int num = Map.BLOCK_SIZE_Y - 1; num >= 0; num--)
		{
			if (Map.GetBlock(x, num, z) > 0)
			{
				return num;
			}
		}
		return 0;
	}
}
