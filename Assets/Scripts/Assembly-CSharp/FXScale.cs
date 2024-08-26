using UnityEngine;

public class FXScale : MonoBehaviour
{
	public float minscale = 0.9f;

	public float maxscale = 1f;

	public float speed = 1f;

	private float forward = 1f;

	private float currscale = 1f;

	private float offset;

	private Transform tr;

	private Vector3 pos;

	private void Update()
	{
		if (tr == null)
		{
			tr = base.gameObject.transform;
		}
		pos = tr.localPosition;
		if (!(pos.x < 0f) && !(pos.y < 0f) && !(pos.z < 0f))
		{
			currscale += 0.5f * forward * Time.deltaTime * speed;
			if (currscale > maxscale)
			{
				currscale = maxscale;
				forward = -1f;
			}
			else if (currscale < minscale)
			{
				currscale = minscale;
				forward = 1f;
			}
			offset = (1f - currscale) / 2f;
			base.transform.localScale = new Vector3(currscale, currscale, currscale);
			base.transform.localPosition = new Vector3(offset, offset, offset);
		}
	}
}
