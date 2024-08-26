using UnityEngine;

public class RotateObject : MonoBehaviour
{
	public bool rotate = true;

	private void Start()
	{
		rotate = true;
	}

	private void Update()
	{
		if (rotate)
		{
			base.transform.Rotate(Vector3.up * (7f * Time.deltaTime));
		}
	}
}
