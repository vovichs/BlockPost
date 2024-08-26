using UnityEngine;

public class TranslateObject : MonoBehaviour
{
	private void Update()
	{
		base.transform.position = new Vector3(Mathf.PingPong(Time.time * 2f, 14f) - 7f, base.transform.position.y, base.transform.position.z);
	}
}
