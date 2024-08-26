using UnityEngine;

public class CLC : MonoBehaviour
{
	private void Awake()
	{
		cpt();
		Object.Destroy(this);
	}

	private void cpt()
	{
		if (Controll.pl != null && !(Controll.pl.goBody == null) && (double)Controll.pl.goBody.transform.localScale.y > 0.05)
		{
			Client.cs.send_clc(1);
		}
	}
}
