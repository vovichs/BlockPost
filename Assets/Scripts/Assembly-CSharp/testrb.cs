using UnityEngine;

public class testrb : MonoBehaviour
{
	private Vector3 store;

	private void Awake()
	{
		store = base.transform.position;
	}

	private void Update()
	{
		float x = Random.Range(0.01f, 0.05f);
		float y = Random.Range(0.01f, 0.05f);
		float z = Random.Range(0.01f, 0.05f);
		base.transform.position = store + new Vector3(x, y, z);
	}
}
