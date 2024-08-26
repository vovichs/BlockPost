using UnityEngine;

public class FXBloodSplat : MonoBehaviour
{
	private Transform tr;

	private void Awake()
	{
		tr = base.gameObject.transform;
	}

	private void Update()
	{
		tr.localScale *= 1f + 2f * Time.deltaTime;
	}
}
