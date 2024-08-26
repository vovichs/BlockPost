using UnityEngine;

public class VInput : MonoBehaviour
{
	public static float h;

	public static float v;

	public GameObject go;

	private void Update()
	{
		InputMove();
		if (!(go == null))
		{
			CharacterController component = go.GetComponent<CharacterController>();
			base.transform.Rotate(0f, h * 1f, 0f);
			Vector3 vector = go.transform.TransformDirection(Vector3.forward);
			float num = 20f * v;
			component.SimpleMove(vector * num);
		}
	}

	private void InputMove()
	{
		h = 0f;
		v = 0f;
		if (Input.GetKey(KeyCode.W))
		{
			v += 1f;
		}
		if (Input.GetKey(KeyCode.S))
		{
			v -= 1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			h += 1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			h -= 1f;
		}
	}
}
