using UnityEngine;

namespace GameClass
{
	public class PullObject
	{
		public int c;

		public GameObject go;

		public ParticleSystem ps;

		public float dt;

		public float t;

		public float speed;

		public float maxdist;

		public Vector3 origin;

		public Material mat;

		public AudioSource s;

		public Rigidbody rb;

		public int uid;

		public int ownerid;

		public GameObject goObj;

		public bool status;

		public float nextthink;

		public Collider col;

		public PullObject(int c, GameObject go, ParticleSystem ps, float dt)
		{
			this.c = c;
			this.go = go;
			this.ps = ps;
			this.dt = dt;
			t = Time.time + dt;
		}
	}
}
