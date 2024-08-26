using UnityEngine;

public class HUDIndicator : MonoBehaviour
{
	public static bool show = false;

	private static float[] time = new float[16];

	private static Vector3[] pos = new Vector3[16];

	private static int count = 0;

	private Color a = new Color(1f, 1f, 1f, 0.95f);

	private Vector2 v = Vector2.zero;

	private Rect r;

	private Texture2D tTex;

	private float width;

	public static void SetActive(bool val)
	{
		show = val;
	}

	private void LoadEnd()
	{
		tTex = ContentLoader_.LoadTexture("indicator") as Texture2D;
		OnResize();
	}

	public void OnResize()
	{
		r.Set((float)Screen.width / 2f - GUIM.YRES(210f), (float)Screen.height / 2f - GUIM.YRES(210f), GUIM.YRES(420f), GUIM.YRES(420f));
		v = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
		width = GUIM.YRES(420f);
	}

	public static void Set(Vector3 p)
	{
		time[count] = Time.time + 2.5f;
		pos[count] = p;
		count++;
		if (count >= 16)
		{
			count = 0;
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			GUI.color = a;
			for (int i = 0; i < 16; i++)
			{
				DrawInd(i);
			}
			GUI.color = Color.white;
		}
	}

	private void DrawInd(int i)
	{
		if (!(Time.time > time[i]))
		{
			float num = time[i] - Time.time;
			num = ((!(num > 2.35f)) ? 1f : (1f + (num - 2.35f) * 6.5f));
			float num2 = width * num;
			r.Set(((float)Screen.width - num2) / 2f, ((float)Screen.height - num2) / 2f, num2, num2);
			float angle = AngleSigned(Controll.trCamera.forward, pos[i] - Controll.trControll.position, Vector3.up);
			Matrix4x4 matrix = GUI.matrix;
			GUIUtility.RotateAroundPivot(angle, v);
			GUI.DrawTexture(r, tTex);
			GUI.matrix = matrix;
		}
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
	{
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}
}
