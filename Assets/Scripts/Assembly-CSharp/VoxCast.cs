using UnityEngine;

public class VoxCast : MonoBehaviour
{
	private static float maxvalue = 1000f;

	public static int Sign(float x)
	{
		if (!(x > 0f))
		{
			if (!(x < 0f))
			{
				return 0;
			}
			return -1;
		}
		return 1;
	}

	public static float Frac0(float x)
	{
		return x - Mathf.Floor(x);
	}

	public static float Frac1(float x)
	{
		return 1f - x + Mathf.Floor(x);
	}

	public static bool RayCast(Vector3 pos, Vector3 dir, out RaycastHit hit, float dist)
	{
		hit = default(RaycastHit);
		if (dir[0] == 0f && dir[1] == 0f && dir[2] == 0f)
		{
			return false;
		}
		Vector3 vector = pos + dir * dist;
		int[] array = new int[3];
		float[] array2 = new float[3];
		float[] array3 = new float[3];
		float[] array4 = new float[3];
		int num = (int)pos[0];
		int num2 = (int)pos[1];
		int num3 = (int)pos[2];
		for (int i = 0; i < 3; i++)
		{
			array[i] = Sign(dir[i]);
			if (array[i] != 0)
			{
				array2[i] = Mathf.Min((float)array[i] / dir[i], maxvalue);
			}
			else
			{
				array2[i] = maxvalue;
			}
			if (array[i] > 0)
			{
				array3[i] = array2[i] * Frac1(pos[i]);
			}
			else
			{
				array3[i] = array2[i] * Frac0(pos[i]);
			}
			array4[i] = pos[i] - (float)(int)pos[i] + dist;
		}
		int num4 = 0;
		do
		{
			if (array3[0] < array3[1])
			{
				if (array3[0] < array3[2])
				{
					num += array[0];
					array3[0] += array2[0];
				}
				else
				{
					num3 += array[2];
					array3[2] += array2[2];
				}
			}
			else if (array3[1] < array3[2])
			{
				num2 += array[1];
				array3[1] += array2[1];
			}
			else
			{
				num3 += array[2];
				array3[2] += array2[2];
			}
			if (array3[0] > dist && array3[1] > dist && array3[2] > dist)
			{
				return false;
			}
			num4 = Map.GetBlock(num, num2, num3);
			if (num4 < 0)
			{
				return false;
			}
		}
		while (num4 == 0);
		hit.point = new Vector3(num, num2, num3);
		return true;
	}
}
