using UnityEngine;

public class ENT : MonoBehaviour
{
	private static float cl_fps = 0.1f;

	private static float grforce = 9.81f;

	private static GameObject pgoDefault = null;

	private static float g_realtimeSinceStartup = 0f;

	private static GameObject p = null;

	private static Vector3 addforce = Vector3.zero;

	private static float tStart = 0f;

	private static float tNextThink = 0f;

	private static Vector3 prevpos;

	private static Vector3 currpos;

	private static Vector3 nextpos;

	private void LoadEnd()
	{
		pgoDefault = Controll.pgoShell;
	}

	private void Update()
	{
		g_realtimeSinceStartup = Time.realtimeSinceStartup;
		Think();
		Lerp();
	}

	public static void Create(Vector3 pos, Vector3 force)
	{
		p = Object.Instantiate(Controll.pgoShell);
		p.transform.localPosition = pos;
		p.transform.localScale = new Vector3(0.4f, 0.1f, 0.4f);
		addforce = force;
		tStart = Time.realtimeSinceStartup;
		tNextThink = tStart + cl_fps;
		prevpos = pos;
		nextpos = pos;
	}

	public static void Think()
	{
		if (p == null || g_realtimeSinceStartup < tNextThink)
		{
			return;
		}
		tNextThink = g_realtimeSinceStartup + cl_fps;
		float num = g_realtimeSinceStartup - tStart;
		float y = grforce * num * 1f;
		prevpos = nextpos;
		nextpos = prevpos + addforce - new Vector3(0f, y, 0f);
		RaycastHit hitInfo;
		if (nextpos.y < 1f)
		{
			nextpos.y = 1f;
			tNextThink += 999f;
			p.transform.localPosition = nextpos;
			p = null;
		}
		else if (Physics.Linecast(prevpos, nextpos, out hitInfo))
		{
			nextpos = hitInfo.point;
			addforce.x = 0f;
			addforce.z = 0f;
			addforce.y /= 2f;
			if (addforce.y < 0.01f)
			{
				addforce.y = 0f;
			}
		}
	}

	public static void Lerp()
	{
		if (!(p == null))
		{
			float num = tNextThink - g_realtimeSinceStartup;
			if (num < 0f)
			{
				num = 0f;
			}
			float t = (cl_fps - num) / cl_fps;
			currpos = Vector3.Lerp(prevpos, nextpos, t);
			p.transform.localPosition = currpos;
		}
	}

	public static void CreateDev(int uid, int type, Vector3 pos, Vector3 force, float ry)
	{
		p = Object.Instantiate(Controll.pgoMedkit);
		p.transform.localPosition = pos;
		p.name = "grenade_" + p.GetInstanceID();
		GameObject.Find(p.name + "/obj").transform.localEulerAngles = new Vector3(0f, ry, 0f);
		Rigidbody rigidbody = p.AddComponent<Rigidbody>();
		p.layer = 9;
		rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		p.AddComponent<BoxCollider>().size = new Vector3(1f, 0.8f, 1f);
		rigidbody.freezeRotation = true;
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		rigidbody.mass = 1f;
		rigidbody.AddForce(force * 250f);
	}
}
