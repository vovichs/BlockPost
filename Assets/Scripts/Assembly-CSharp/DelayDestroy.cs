using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
	public float timer = 5f;

	private void Awake()
	{
		timer += Time.time;
	}

	private void Update()
	{
		if (!(Time.time < timer) && !(base.gameObject == null))
		{
			Object.Destroy(base.gameObject);
		}
	}
}
