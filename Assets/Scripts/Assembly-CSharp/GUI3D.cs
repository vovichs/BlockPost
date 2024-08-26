using UnityEngine;

public class GUI3D : MonoBehaviour
{
	public static Camera csCam;

	private static int fx;

	private static float tStart;

	private void Awake()
	{
		csCam = base.gameObject.AddComponent<Camera>();
		csCam.enabled = false;
		csCam.clearFlags = CameraClearFlags.Depth;
		csCam.cullingMask = 512;
		csCam.fieldOfView = 20f;
	}

	public static void SetFX(int val)
	{
		fx = val;
		tStart = Time.time;
	}

	public static void Draw(Rect r, GameObject go, Vector3 pos, Vector3 rot)
	{
		if (go == null || r.y + r.height - GUIM.offsety2 < 0f)
		{
			return;
		}
		csCam.rect = new Rect((r.x + GUIM.offsetx) / (float)Screen.width, 1f - (r.y + r.height + GUIM.offsety - GUIM.offsety2) / (float)Screen.height, r.width / (float)Screen.width, r.height / (float)Screen.height);
		bool activeSelf = go.activeSelf;
		Vector3 position = go.transform.position;
		Vector3 eulerAngles = go.transform.eulerAngles;
		go.SetActive(true);
		go.transform.position = pos;
		go.transform.eulerAngles = rot;
		if (fx == 1)
		{
			Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
			if ((double)Time.time < (double)tStart + 0.1)
			{
				Renderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material.SetColor("_Color", Color.black);
				}
			}
			else
			{
				Renderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material.SetColor("_Color", Color.white);
				}
			}
		}
		csCam.Render();
		go.SetActive(activeSelf);
		go.transform.position = position;
		go.transform.eulerAngles = eulerAngles;
	}
}
