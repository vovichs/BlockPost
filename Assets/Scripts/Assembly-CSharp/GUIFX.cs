using UnityEngine;

public class GUIFX : MonoBehaviour
{
	private static bool fx;

	private static float fxtime;

	private static Vector3 vPos;

	private static Vector3 vScale;

	public static void Set()
	{
		fx = true;
		fxtime = Time.time;
	}

	public static void Begin()
	{
		if (fx)
		{
			float num = (Time.time - fxtime) * 5f;
			num += 0.5f;
			if (num > 1f)
			{
				fx = false;
				num = 1f;
			}
			vPos.Set((float)Screen.width * (1f - num) / 2f, (float)Screen.height * (1f - num) / 2f, 0f);
			vScale.Set(num, num, 1f);
			Matrix4x4 identity = Matrix4x4.identity;
			identity.SetTRS(vPos, Quaternion.identity, vScale);
			GUI.matrix = identity;
		}
	}

	public static void End()
	{
		if (fx)
		{
			GUI.matrix = Matrix4x4.identity;
		}
	}
}
