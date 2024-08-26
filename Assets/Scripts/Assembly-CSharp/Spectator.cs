using UnityEngine;

public class Spectator : MonoBehaviour
{
	public bool active;

	public int speed = 20;

	private void Update()
	{
		if (active)
		{
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				speed = 40;
			}
			else
			{
				speed = 20;
			}
			if (Input.GetKey(KeyCode.A))
			{
				base.transform.position = base.transform.position + base.transform.right * -1f * speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.S))
			{
				base.transform.position = base.transform.position + base.transform.forward * -1f * speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.D))
			{
				base.transform.position = base.transform.position + base.transform.right * speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.W))
			{
				base.transform.position = base.transform.position + base.transform.forward * speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.Space))
			{
				base.transform.position = base.transform.position + base.transform.up * speed * Time.deltaTime;
			}
			if (Input.GetKey(GUIOptions.keyCrouch))
			{
				base.transform.position = base.transform.position + base.transform.up * -1f * speed * Time.deltaTime;
			}
		}
	}
}
