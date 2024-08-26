using UnityEngine;

public class DelayTransparent : MonoBehaviour
{
	public float starttime;

	public float timer = 0.5f;

	public Material mat;

	public float cr = 1f;

	public float cg;

	public float cb;

	private void Awake()
	{
		timer += Time.time;
		starttime = Time.time;
		mat = base.gameObject.GetComponent<MeshRenderer>().material;
	}

	private void Update()
	{
		if (mat != null)
		{
			float num = timer - Time.time;
			float num2 = timer - starttime;
			if (num > 0f)
			{
				mat.color = new Color(cr, cg, cb, num / num2 * 0.5f);
			}
		}
		if (!(Time.time < timer) && !(base.gameObject == null))
		{
			Object.Destroy(base.gameObject);
		}
	}
}
