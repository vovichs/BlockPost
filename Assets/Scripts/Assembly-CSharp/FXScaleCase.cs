using UnityEngine;

public class FXScaleCase : MonoBehaviour
{
	public float minscale = 0.9f;

	public float maxscale = 1f;

	public float speed = 1f;

	private float forward = 1f;

	private float currscale = 1f;

	private float offset;

	private void Update()
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
		base.transform.localScale = new Vector3(currscale, currscale, currscale);
		base.transform.localPosition = new Vector3(offset, offset - 0.4f, offset + 6f);
	}
}
