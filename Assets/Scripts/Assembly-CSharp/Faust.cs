using GameClass;
using UnityEngine;

public class Faust : MonoBehaviour
{
	public PullObject p;

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
		if (uID >= 0 && p.ownerid == Controll.pl.idx)
		{
			Client.cs.send_entpos(p.uid, position);
		}
	}
}
