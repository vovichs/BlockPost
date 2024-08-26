using UnityEngine;

public class FBlock : MonoBehaviour
{
	private void OnCollisionEnter(Collision col)
	{
		OnCollisionStay(col);
	}

	private void OnCollisionStay(Collision col)
	{
		GOP.Create(3, base.transform.position, Vector3.zero, Vector3.zero, Color.white);
		int uID = GOP.GetUID(base.gameObject);
		base.gameObject.SetActive(false);
		Vector3 position = base.transform.position;
		if (uID >= 0)
		{
			HitData component = col.transform.GetComponent<HitData>();
			if (!(component == null))
			{
				int idx = component.idx;
				int box = component.box;
				Client.cs.send_receivethrow(uID, position, idx);
			}
		}
	}
}
