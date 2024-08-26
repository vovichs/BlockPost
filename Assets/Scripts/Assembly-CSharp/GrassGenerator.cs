using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
	public static void Generate()
	{
		Random.seed = 986565;
		for (int i = 0; i < 10000; i++)
		{
			int x = Random.Range(512, 1500);
			int z = Random.Range(512, 1500);
			int h = (int)((float)HMap.GetHeight(x, z) * 0.25f) + 1;
			VMap.SetBlock(x, h, z, 2);
		}
	}
}
