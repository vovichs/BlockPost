using UnityEngine;

public class FXTracer : MonoBehaviour
{
	private Transform tr;

	private float speed = 300f;

	public float maxdist = 256f;

	private Vector3 origin;

	private void Awake()
	{
		tr = base.gameObject.transform;
		speed = Random.Range(250, 300);
		origin = tr.position;
		RaycastHit hit;
		if (Controll.Raycast(origin, tr.forward, out hit))
		{
			maxdist = hit.distance - 8f;
		}
		if (maxdist < 16f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (!(base.gameObject == null))
		{
			Vector3 vector = tr.transform.forward * Time.deltaTime * speed;
			if (Vector3.Distance(tr.transform.position + vector, origin) > maxdist)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				tr.transform.position += vector;
			}
		}
	}
}
