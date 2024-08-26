using System.Collections.Generic;
using GameClass;
using Player;
using UnityEngine;
using UnityEngine.Rendering;

public class GOP : MonoBehaviour
{
	public static GameObject goPull = null;

	public static List<PullObject> po = new List<PullObject>();

	private static int debug_reused = 0;

	private static Bounds bounds = default(Bounds);

	private static Vector3 v05 = new Vector3(0.5f, 0.5f, 0.5f);

	private static bool[] checkbounds = new bool[32];

	public static void Init()
	{
		checkbounds[0] = true;
		checkbounds[1] = false;
		checkbounds[2] = false;
		checkbounds[3] = true;
		checkbounds[4] = true;
		checkbounds[5] = true;
		checkbounds[6] = true;
		checkbounds[7] = true;
		checkbounds[8] = false;
		checkbounds[9] = false;
		checkbounds[10] = false;
		checkbounds[11] = false;
		checkbounds[12] = false;
		checkbounds[13] = false;
		checkbounds[14] = false;
		checkbounds[15] = false;
		checkbounds[16] = true;
		checkbounds[17] = true;
		checkbounds[18] = true;
		checkbounds[19] = true;
		checkbounds[20] = false;
		checkbounds[21] = false;
		checkbounds[22] = false;
		checkbounds[23] = false;
	}

	public static void Clear()
	{
		for (int i = 0; i < po.Count; i++)
		{
			if (!(po[i].go == null))
			{
				Object.Destroy(po[i].go);
			}
		}
		po.Clear();
	}

	public static void Create(int c, Vector3 pos, Vector3 rot, Vector3 force, Color color, int iparam = 0, int iparam2 = 0)
	{
		if (checkbounds[c])
		{
			bounds.SetMinMax(pos - v05, pos + v05);
			if (!GeometryUtility.TestPlanesAABB(Controll.camplanes, bounds))
			{
				return;
			}
		}
		for (int i = 0; i < po.Count; i++)
		{
			PullObject pullObject = po[i];
			if (pullObject.c != c || pullObject.go.activeSelf)
			{
				continue;
			}
			pullObject.go.transform.position = pos;
			switch (c)
			{
			case 0:
				pullObject.ps.startColor = color;
				break;
			case 1:
				pullObject.go.transform.eulerAngles = rot;
				pullObject.ps.maxParticles = Random.Range(0, 6);
				break;
			case 2:
				pullObject.go.transform.eulerAngles = rot;
				break;
			case 3:
				pullObject.ps.startColor = color;
				break;
			case 4:
			case 5:
			case 6:
			case 7:
				pullObject.go.transform.LookAt(Controll.trCamera.position);
				pullObject.go.transform.eulerAngles = new Vector3(pullObject.go.transform.eulerAngles.x, pullObject.go.transform.eulerAngles.y, Random.Range(0, 360));
				if (iparam == 1)
				{
					pullObject.go.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
				}
				else
				{
					pullObject.go.transform.localScale = Vector3.one;
				}
				break;
			default:
				switch (c)
				{
				case 8:
				{
					pullObject.go.transform.position = pos;
					pullObject.go.transform.eulerAngles = rot;
					pullObject.speed = Random.Range(250, 300);
					pullObject.origin = pos;
					pullObject.maxdist = 256f;
					RaycastHit hit;
					if (Controll.Raycast(pos, pullObject.go.transform.forward, out hit))
					{
						pullObject.maxdist = hit.distance - 8f;
					}
					if (pullObject.maxdist < 16f)
					{
						return;
					}
					break;
				}
				case 9:
					pullObject.go.transform.position = pos;
					pullObject.mat.color = new Color(1f, 0f, 0f, 0.5f);
					pullObject.s.volume = ((iparam == 1) ? 0f : GUIOptions.gamevolume);
					break;
				case 10:
				{
					Transform transform = Controll.pl.currweapon.goWeapon.transform;
					pullObject.go.transform.position = transform.position + transform.right * 0.02f + transform.up * 0.04f - transform.forward * 0.02f;
					pullObject.go.transform.eulerAngles = new Vector3(Random.Range(80, 100), transform.eulerAngles.y + (float)Random.Range(75, 115), Random.Range(0, 360));
					pullObject.rb.velocity = Vector3.zero;
					pullObject.rb.angularVelocity = Vector3.zero;
					pullObject.rb.rotation = Quaternion.Euler(pullObject.go.transform.eulerAngles);
					pullObject.rb.position = pullObject.go.transform.position;
					pullObject.go.SetActive(true);
					pullObject.rb.AddForce(-transform.forward * Random.Range(100, 125) + transform.up * Random.Range(40, 60));
					break;
				}
				case 11:
					pullObject.go.transform.position = pos;
					pullObject.s.volume = ((iparam == 1) ? 0f : GUIOptions.gamevolume);
					break;
				case 12:
					pullObject.go.transform.localPosition = pos;
					pullObject.goObj.transform.localEulerAngles = rot;
					pullObject.goObj.SetActive(false);
					pullObject.rb.velocity = Vector3.zero;
					pullObject.rb.angularVelocity = Vector3.zero;
					pullObject.go.SetActive(true);
					pullObject.rb.AddForce(force * 650f + Vector3.up * 100f);
					pullObject.uid = iparam;
					pullObject.ownerid = iparam2;
					pullObject.status = false;
					pullObject.maxdist = 0f;
					UpdateCollision(pullObject.col, iparam2);
					break;
				case 13:
					pullObject.go.transform.localPosition = pos;
					pullObject.goObj.transform.localEulerAngles = rot;
					pullObject.goObj.SetActive(false);
					pullObject.rb.velocity = Vector3.zero;
					pullObject.rb.angularVelocity = Vector3.zero;
					pullObject.go.SetActive(true);
					pullObject.rb.AddForce(force * 250f + Vector3.up * 50f);
					pullObject.uid = iparam;
					pullObject.maxdist = 0f;
					break;
				case 14:
					pullObject.go.transform.localPosition = pos;
					pullObject.goObj.transform.localEulerAngles = rot;
					pullObject.goObj.SetActive(false);
					pullObject.rb.velocity = Vector3.zero;
					pullObject.rb.angularVelocity = Vector3.zero;
					pullObject.go.SetActive(true);
					pullObject.rb.AddForce(force * 250f + Vector3.up * 50f);
					pullObject.uid = iparam;
					pullObject.maxdist = 0f;
					break;
				case 15:
					pullObject.go.transform.localPosition = pos;
					break;
				case 16:
				case 17:
				case 18:
				case 19:
					pullObject.go.transform.LookAt(Controll.trCamera.position);
					pullObject.go.transform.eulerAngles = new Vector3(pullObject.go.transform.eulerAngles.x, pullObject.go.transform.eulerAngles.y, Random.Range(0, 360));
					if (iparam == 1)
					{
						pullObject.go.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
					}
					else
					{
						pullObject.go.transform.localScale = Vector3.one;
					}
					break;
				default:
					switch (c)
					{
					case 20:
						pullObject.go.transform.position = pos;
						pullObject.go.transform.eulerAngles = rot;
						pullObject.origin.x = (int)force.x;
						pullObject.origin.y = (int)force.y;
						pullObject.origin.z = (int)force.z;
						break;
					case 21:
						pullObject.go.transform.position = pos;
						pullObject.go.transform.eulerAngles = rot;
						pullObject.origin.x = (int)force.x;
						pullObject.origin.y = (int)force.y;
						pullObject.origin.z = (int)force.z;
						break;
					case 22:
						pullObject.go.transform.eulerAngles = rot;
						break;
					case 23:
						pullObject.go.transform.localPosition = pos;
						pullObject.goObj.transform.localEulerAngles = rot;
						pullObject.goObj.SetActive(false);
						pullObject.rb.velocity = Vector3.zero;
						pullObject.rb.angularVelocity = Vector3.zero;
						pullObject.go.SetActive(true);
						pullObject.rb.AddForce(force * 800f + Vector3.up * 50f);
						pullObject.uid = iparam;
						pullObject.ownerid = iparam2;
						pullObject.status = false;
						pullObject.maxdist = 0f;
						UpdateCollision(pullObject.col, iparam2);
						break;
					case 26:
						pullObject.go.transform.localPosition = pos;
						pullObject.goObj.transform.localEulerAngles = rot;
						pullObject.goObj.SetActive(false);
						pullObject.rb.velocity = Vector3.zero;
						pullObject.rb.angularVelocity = Vector3.zero;
						pullObject.go.SetActive(true);
						pullObject.rb.AddForce(force * 2000f + Vector3.up * 50f);
						pullObject.uid = iparam;
						pullObject.ownerid = iparam2;
						pullObject.status = false;
						pullObject.maxdist = 0f;
						UpdateCollision(pullObject.col, iparam2);
						break;
					case 27:
						pullObject.go.transform.localPosition = pos;
						pullObject.goObj.transform.localEulerAngles = rot;
						pullObject.goObj.SetActive(false);
						pullObject.rb.velocity = Vector3.zero;
						pullObject.rb.angularVelocity = Vector3.zero;
						pullObject.go.SetActive(true);
						pullObject.rb.AddForce(force * 250f + Vector3.up * 50f);
						pullObject.uid = iparam;
						pullObject.maxdist = 0f;
						break;
					case 28:
						pullObject.go.transform.localPosition = pos;
						pullObject.goObj.transform.localEulerAngles = Vector3.zero;
						pullObject.goObj.SetActive(false);
						pullObject.rb.velocity = Vector3.zero;
						pullObject.rb.angularVelocity = Vector3.zero;
						pullObject.go.SetActive(true);
						pullObject.rb.AddForce(force * 250f + Vector3.up * 50f);
						pullObject.uid = 0;
						pullObject.maxdist = 0f;
						break;
					case 29:
						pullObject.go.transform.localPosition = pos;
						pullObject.goObj.transform.localEulerAngles = rot;
						pullObject.goObj.SetActive(false);
						pullObject.rb.velocity = Vector3.zero;
						pullObject.rb.angularVelocity = Vector3.zero;
						pullObject.go.SetActive(true);
						pullObject.rb.AddForce(force * 800f + Vector3.up * 75f);
						pullObject.uid = iparam;
						pullObject.ownerid = iparam2;
						pullObject.status = false;
						pullObject.maxdist = 0f;
						UpdateCollision(pullObject.col, iparam2);
						break;
					}
					break;
				}
				break;
			}
			pullObject.t = Time.time + pullObject.dt;
			pullObject.go.SetActive(true);
			debug_reused++;
			return;
		}
		GameObject gameObject = null;
		ParticleSystem ps = null;
		switch (c)
		{
		case 0:
		{
			gameObject = Object.Instantiate(Controll.pgoDebris);
			gameObject.transform.position = pos;
			ps = gameObject.GetComponent<ParticleSystem>();
			ps.startColor = color;
			PullObject item2 = new PullObject(c, gameObject, ps, 2f);
			po.Add(item2);
			return;
		}
		case 1:
			if (GUIOptions.particles != 0)
			{
				gameObject = Object.Instantiate(Controll.pgoDebris_muzzle);
				gameObject.transform.position = pos;
				gameObject.transform.eulerAngles = rot;
				ps = gameObject.GetComponent<ParticleSystem>();
				ps.maxParticles = Random.Range(0, 6);
				PullObject item5 = new PullObject(c, gameObject, ps, 0.2f);
				po.Add(item5);
			}
			return;
		case 2:
			if (GUIOptions.particles != 0)
			{
				gameObject = Object.Instantiate(Controll.pgoDebris_smoke);
				gameObject.transform.position = pos;
				gameObject.transform.eulerAngles = rot;
				ps = gameObject.GetComponent<ParticleSystem>();
				PullObject item3 = new PullObject(c, gameObject, ps, 0.3f);
				po.Add(item3);
			}
			return;
		case 3:
			if (GUIOptions.particles != 0)
			{
				gameObject = Object.Instantiate(Controll.pgoDebris_smoke_big);
				gameObject.transform.position = pos;
				ps = gameObject.GetComponent<ParticleSystem>();
				ps.startColor = color;
				PullObject item4 = new PullObject(c, gameObject, ps, 3f);
				po.Add(item4);
			}
			return;
		case 4:
		case 5:
		case 6:
		case 7:
		{
			gameObject = Object.Instantiate(Controll.pgoBloodSplat[c - 4]);
			gameObject.transform.position = pos;
			gameObject.transform.LookAt(Controll.trCamera.position);
			gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, Random.Range(0, 360));
			if (iparam == 1)
			{
				gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
			}
			PullObject item = new PullObject(c, gameObject, ps, 0.25f);
			po.Add(item);
			return;
		}
		}
		switch (c)
		{
		case 8:
		{
			gameObject = Object.Instantiate(Controll.pgoTracer);
			gameObject.transform.position = pos;
			gameObject.transform.eulerAngles = rot;
			gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
			PullObject pullObject5 = new PullObject(c, gameObject, ps, 0.5f);
			pullObject5.speed = Random.Range(250, 300);
			pullObject5.origin = pos;
			pullObject5.maxdist = 256f;
			RaycastHit hit2;
			if (Controll.Raycast(pos, gameObject.transform.forward, out hit2))
			{
				pullObject5.maxdist = hit2.distance - 8f;
			}
			po.Add(pullObject5);
			if (pullObject5.maxdist < 16f)
			{
				pullObject5.go.SetActive(false);
			}
			return;
		}
		case 9:
		{
			gameObject = Object.Instantiate(Controll.pgoDamageBlock);
			gameObject.transform.position = pos;
			PullObject pullObject7 = new PullObject(c, gameObject, ps, 0.3f);
			pullObject7.mat = gameObject.GetComponent<MeshRenderer>().material;
			pullObject7.mat.color = new Color(1f, 0f, 0f, 0.5f);
			pullObject7.s = gameObject.AddComponent<AudioSource>();
			pullObject7.s.clip = ContentLoader_.LoadAudio("deadblock");
			pullObject7.s.volume = ((iparam == 1) ? 0f : GUIOptions.gamevolume);
			pullObject7.s.spatialBlend = 1f;
			pullObject7.s.minDistance = 1f;
			pullObject7.s.maxDistance = 32f;
			pullObject7.s.playOnAwake = true;
			po.Add(pullObject7);
			return;
		}
		case 10:
		{
			Transform transform2 = Controll.pl.currweapon.goWeapon.transform;
			gameObject = Object.Instantiate(Controll.pgoShell);
			gameObject.transform.position = transform2.position + transform2.right * 0.02f + transform2.up * 0.04f - transform2.forward * 0.02f;
			gameObject.transform.eulerAngles = new Vector3(Random.Range(80, 100), transform2.eulerAngles.y + (float)Random.Range(75, 115), Random.Range(0, 360));
			PullObject pullObject6 = new PullObject(c, gameObject, ps, 0.5f);
			pullObject6.rb = gameObject.AddComponent<Rigidbody>();
			pullObject6.rb.interpolation = RigidbodyInterpolation.Interpolate;
			pullObject6.rb.AddForce(-transform2.forward * Random.Range(100, 125) + transform2.up * Random.Range(40, 60));
			po.Add(pullObject6);
			return;
		}
		case 11:
		{
			gameObject = new GameObject();
			gameObject.transform.position = pos;
			PullObject pullObject2 = new PullObject(c, gameObject, ps, 0.3f);
			pullObject2.s = gameObject.AddComponent<AudioSource>();
			pullObject2.s.clip = ContentLoader_.LoadAudio("setblock");
			pullObject2.s.volume = ((iparam == 1) ? 0f : GUIOptions.gamevolume);
			pullObject2.s.spatialBlend = 1f;
			pullObject2.s.minDistance = 1f;
			pullObject2.s.maxDistance = 32f;
			pullObject2.s.playOnAwake = true;
			po.Add(pullObject2);
			return;
		}
		case 12:
		{
			gameObject = Object.Instantiate(Controll.pgoGrenade);
			gameObject.transform.localPosition = pos;
			gameObject.name = "grenade_" + gameObject.GetInstanceID();
			GameObject gameObject2 = GameObject.Find(gameObject.name + "/obj");
			gameObject2.transform.localEulerAngles = rot;
			gameObject2.SetActive(false);
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			gameObject.layer = 10;
			rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(0.2f, 0.2f, 0.2f);
			rigidbody.freezeRotation = true;
			rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody.mass = 0.5f;
			rigidbody.AddForce(force * 650f + Vector3.up * 100f);
			TrailRenderer trailRenderer = gameObject.AddComponent<TrailRenderer>();
			trailRenderer.shadowCastingMode = ShadowCastingMode.Off;
			trailRenderer.useLightProbes = false;
			Material material = new Material(ContentLoader_.LoadMaterial("unlit_transparent"));
			material.SetTexture("_MainTex", TEX.GetTextureByName("white15"));
			trailRenderer.sharedMaterial = material;
			trailRenderer.startWidth = 0.2f;
			trailRenderer.endWidth = 0.01f;
			trailRenderer.time = 1f;
			PullObject pullObject3 = new PullObject(c, gameObject, null, 5f);
			pullObject3.rb = rigidbody;
			pullObject3.uid = iparam;
			pullObject3.ownerid = iparam2;
			pullObject3.goObj = gameObject2;
			pullObject3.status = false;
			pullObject3.maxdist = 0f;
			pullObject3.col = boxCollider;
			po.Add(pullObject3);
			UpdateCollision(boxCollider, iparam2);
			return;
		}
		case 13:
		{
			gameObject = Object.Instantiate(Controll.pgoMedkit);
			gameObject.transform.localPosition = pos;
			gameObject.name = "medkit_" + gameObject.GetInstanceID();
			GameObject gameObject4 = GameObject.Find(gameObject.name + "/obj");
			gameObject4.transform.localEulerAngles = rot;
			gameObject4.SetActive(false);
			Rigidbody rigidbody3 = gameObject.AddComponent<Rigidbody>();
			gameObject.layer = 10;
			rigidbody3.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			gameObject.AddComponent<BoxCollider>().size = new Vector3(1f, 0.8f, 1f);
			rigidbody3.freezeRotation = true;
			rigidbody3.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody3.mass = 1f;
			rigidbody3.AddForce(force * 250f + Vector3.up * 50f);
			PullObject pullObject9 = new PullObject(c, gameObject, null, 20f);
			pullObject9.rb = rigidbody3;
			pullObject9.uid = iparam;
			pullObject9.goObj = gameObject4;
			pullObject9.maxdist = 0f;
			po.Add(pullObject9);
			return;
		}
		case 14:
		{
			gameObject = Object.Instantiate(Controll.pgoAmmokit);
			gameObject.transform.localPosition = pos;
			gameObject.name = "ammokit_" + gameObject.GetInstanceID();
			GameObject gameObject3 = GameObject.Find(gameObject.name + "/obj");
			gameObject3.transform.localEulerAngles = rot;
			gameObject3.SetActive(false);
			Rigidbody rigidbody2 = gameObject.AddComponent<Rigidbody>();
			gameObject.layer = 10;
			rigidbody2.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			gameObject.AddComponent<BoxCollider>().size = new Vector3(1f, 0.8f, 1f);
			rigidbody2.freezeRotation = true;
			rigidbody2.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody2.mass = 1f;
			rigidbody2.AddForce(force * 250f + Vector3.up * 50f);
			PullObject pullObject8 = new PullObject(c, gameObject, null, 10f);
			pullObject8.rb = rigidbody2;
			pullObject8.uid = iparam;
			pullObject8.goObj = gameObject3;
			pullObject8.maxdist = 0f;
			po.Add(pullObject8);
			return;
		}
		case 15:
		{
			gameObject = Object.Instantiate(Controll.pgoExplode);
			gameObject.transform.localPosition = pos;
			PullObject pullObject4 = new PullObject(c, gameObject, null, 1f);
			pullObject4.s = gameObject.AddComponent<AudioSource>();
			pullObject4.s.clip = Resources.Load("sounds/explode") as AudioClip;
			pullObject4.s.volume = ((iparam == 1) ? 0f : GUIOptions.gamevolume);
			pullObject4.s.spatialBlend = 1f;
			pullObject4.s.minDistance = 4f;
			pullObject4.s.maxDistance = 32f;
			pullObject4.s.playOnAwake = true;
			pullObject4.s.Play();
			po.Add(pullObject4);
			return;
		}
		case 16:
		case 17:
		case 18:
		case 19:
		{
			gameObject = Object.Instantiate(Controll.pgoBloodSplatGreen[c - 16]);
			gameObject.transform.position = pos;
			gameObject.transform.LookAt(Controll.trCamera.position);
			gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, Random.Range(0, 360));
			if (iparam == 1)
			{
				gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
			}
			PullObject item6 = new PullObject(c, gameObject, ps, 0.25f);
			po.Add(item6);
			return;
		}
		}
		switch (c)
		{
		case 20:
		{
			gameObject = Object.Instantiate(Controll.pgoHoleBlood);
			gameObject.transform.position = pos;
			gameObject.transform.eulerAngles = rot;
			PullObject pullObject10 = new PullObject(c, gameObject, null, 5f);
			pullObject10.origin = new Vector3((int)force.x, (int)force.y, (int)force.z);
			po.Add(pullObject10);
			return;
		}
		case 21:
		{
			gameObject = Object.Instantiate(Controll.pgoHole);
			gameObject.transform.position = pos;
			gameObject.transform.eulerAngles = rot;
			PullObject pullObject11 = new PullObject(c, gameObject, null, 5f);
			pullObject11.origin = new Vector3((int)force.x, (int)force.y, (int)force.z);
			po.Add(pullObject11);
			return;
		}
		case 22:
		{
			gameObject = Object.Instantiate(Controll.pgoDebris_flame);
			gameObject.transform.position = pos;
			gameObject.transform.eulerAngles = rot;
			ps = gameObject.GetComponent<ParticleSystem>();
			PullObject item8 = new PullObject(c, gameObject, ps, 1f);
			po.Add(item8);
			return;
		}
		case 23:
		{
			gameObject = Object.Instantiate(Controll.pgoFBlock);
			gameObject.transform.localPosition = pos;
			gameObject.name = "fblock_" + gameObject.GetInstanceID();
			GameObject gameObject5 = GameObject.Find(gameObject.name + "/obj");
			gameObject5.transform.localEulerAngles = rot;
			gameObject5.SetActive(false);
			gameObject.AddComponent<FBlock>();
			Rigidbody rigidbody4 = gameObject.AddComponent<Rigidbody>();
			gameObject.layer = 10;
			rigidbody4.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			BoxCollider boxCollider2 = gameObject.AddComponent<BoxCollider>();
			boxCollider2.size = new Vector3(0.4f, 0.4f, 0.4f);
			rigidbody4.freezeRotation = true;
			rigidbody4.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody4.mass = 0.5f;
			rigidbody4.AddForce(force * 800f + Vector3.up * 50f);
			TrailRenderer trailRenderer2 = gameObject.AddComponent<TrailRenderer>();
			trailRenderer2.shadowCastingMode = ShadowCastingMode.Off;
			trailRenderer2.useLightProbes = false;
			Material material2 = new Material(ContentLoader_.LoadMaterial("unlit_transparent"));
			material2.SetTexture("_MainTex", TEX.GetTextureByName("white15"));
			trailRenderer2.sharedMaterial = material2;
			trailRenderer2.startWidth = 0.2f;
			trailRenderer2.endWidth = 0.01f;
			trailRenderer2.time = 0.1f;
			PullObject pullObject12 = new PullObject(c, gameObject, null, 2f);
			pullObject12.rb = rigidbody4;
			pullObject12.uid = iparam;
			pullObject12.ownerid = iparam2;
			pullObject12.goObj = gameObject5;
			pullObject12.status = false;
			pullObject12.maxdist = 0f;
			pullObject12.col = boxCollider2;
			po.Add(pullObject12);
			UpdateCollision(boxCollider2, iparam2);
			return;
		}
		case 24:
		case 25:
		{
			gameObject = Object.Instantiate(Controll.pgoEnergySplat[c - 24]);
			gameObject.transform.position = pos;
			gameObject.transform.LookAt(Controll.trCamera.position);
			gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, Random.Range(0, 360));
			if (iparam == 1)
			{
				gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
			}
			PullObject item7 = new PullObject(c, gameObject, ps, 0.25f);
			po.Add(item7);
			return;
		}
		}
		switch (c)
		{
		case 26:
		{
			if (Controll.pgoFaust == null)
			{
				Controll.pgoFaust = Resources.Load("prefabs/weapon_panzerfaust_rocket") as GameObject;
			}
			gameObject = Object.Instantiate(Controll.pgoFaust);
			gameObject.transform.localPosition = pos;
			gameObject.name = "faust_" + gameObject.GetInstanceID();
			GameObject gameObject8 = GameObject.Find(gameObject.name + "/Model");
			gameObject8.transform.localEulerAngles = rot;
			gameObject8.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
			gameObject8.SetActive(false);
			Faust faust = gameObject.AddComponent<Faust>();
			Rigidbody rigidbody7 = gameObject.AddComponent<Rigidbody>();
			gameObject.layer = 10;
			rigidbody7.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			BoxCollider boxCollider4 = gameObject.AddComponent<BoxCollider>();
			boxCollider4.size = new Vector3(0.4f, 0.4f, 0.4f);
			rigidbody7.freezeRotation = true;
			rigidbody7.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody7.mass = 0.5f;
			rigidbody7.AddForce(force * 2000f + Vector3.up * 50f);
			TrailRenderer trailRenderer4 = gameObject.AddComponent<TrailRenderer>();
			trailRenderer4.shadowCastingMode = ShadowCastingMode.Off;
			trailRenderer4.useLightProbes = false;
			Material material4 = new Material(ContentLoader_.LoadMaterial("unlit_transparent"));
			material4.SetTexture("_MainTex", TEX.GetTextureByName("yellow15"));
			trailRenderer4.sharedMaterial = material4;
			trailRenderer4.startWidth = 0.2f;
			trailRenderer4.endWidth = 0.01f;
			trailRenderer4.time = 0.5f;
			PullObject pullObject15 = new PullObject(c, gameObject, null, 5f)
			{
				rb = rigidbody7,
				uid = iparam,
				ownerid = iparam2,
				goObj = gameObject8,
				status = false,
				maxdist = 0f,
				col = boxCollider4
			};
			po.Add(pullObject15);
			UpdateCollision(boxCollider4, iparam2);
			faust.p = pullObject15;
			break;
		}
		case 27:
		{
			gameObject = Object.Instantiate(Controll.pgoRocketkit);
			gameObject.transform.localPosition = pos;
			gameObject.name = "rocketkit_" + gameObject.GetInstanceID();
			GameObject gameObject7 = GameObject.Find(gameObject.name + "/obj");
			gameObject7.transform.localEulerAngles = rot;
			gameObject7.SetActive(false);
			Rigidbody rigidbody6 = gameObject.AddComponent<Rigidbody>();
			gameObject.layer = 10;
			rigidbody6.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			gameObject.AddComponent<BoxCollider>().size = new Vector3(1f, 0.8f, 1f);
			rigidbody6.freezeRotation = true;
			rigidbody6.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody6.mass = 1f;
			rigidbody6.AddForce(force * 250f + Vector3.up * 50f);
			PullObject pullObject14 = new PullObject(c, gameObject, null, 30f);
			pullObject14.rb = rigidbody6;
			pullObject14.uid = iparam;
			pullObject14.goObj = gameObject7;
			pullObject14.maxdist = 0f;
			po.Add(pullObject14);
			break;
		}
		case 28:
		{
			if (Controll.pgoFaustDrop == null)
			{
				Controll.pgoFaustDrop = Resources.Load("prefabs/weapon_panzerfaust_drop") as GameObject;
			}
			gameObject = Object.Instantiate(Controll.pgoFaustDrop);
			gameObject.transform.localPosition = pos;
			gameObject.name = "rocketkit_" + gameObject.GetInstanceID();
			GameObject gameObject9 = GameObject.Find(gameObject.name + "/Model");
			gameObject9.transform.localEulerAngles = Vector3.zero;
			gameObject9.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
			gameObject9.SetActive(false);
			Rigidbody rigidbody8 = gameObject.AddComponent<Rigidbody>();
			gameObject.layer = 10;
			rigidbody8.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			BoxCollider boxCollider5 = gameObject.AddComponent<BoxCollider>();
			boxCollider5.size = new Vector3(1.25f, 0.15f, 0.15f);
			boxCollider5.center = new Vector3(0f, -0.075f, 0f);
			rigidbody8.freezeRotation = false;
			rigidbody8.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody8.mass = 1f;
			rigidbody8.AddForce(force * 250f + Vector3.up * 50f);
			rigidbody8.AddTorque(new Vector3(50f, 250f, 0f));
			PullObject pullObject16 = new PullObject(c, gameObject, null, 5f);
			pullObject16.rb = rigidbody8;
			pullObject16.uid = iparam;
			pullObject16.goObj = gameObject9;
			pullObject16.maxdist = 0f;
			po.Add(pullObject16);
			break;
		}
		case 29:
		{
			if (Controll.pgoGrenade33 == null)
			{
				Controll.pgoGrenade33 = Resources.Load("prefabs/bp_grenade_stielhandgranate") as GameObject;
			}
			gameObject = Object.Instantiate(Controll.pgoGrenade33);
			gameObject.transform.localPosition = pos;
			gameObject.name = "grenade_" + gameObject.GetInstanceID();
			GameObject gameObject6 = GameObject.Find(gameObject.name + "/Model");
			gameObject6.transform.localEulerAngles = rot;
			gameObject6.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
			gameObject6.SetActive(false);
			Rigidbody rigidbody5 = gameObject.AddComponent<Rigidbody>();
			gameObject.layer = 10;
			rigidbody5.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			BoxCollider boxCollider3 = gameObject.AddComponent<BoxCollider>();
			boxCollider3.size = new Vector3(0.2f, 0.2f, 0.2f);
			rigidbody5.freezeRotation = true;
			rigidbody5.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody5.mass = 0.5f;
			rigidbody5.AddForce(force * 800f + Vector3.up * 75f);
			TrailRenderer trailRenderer3 = gameObject.AddComponent<TrailRenderer>();
			trailRenderer3.shadowCastingMode = ShadowCastingMode.Off;
			trailRenderer3.useLightProbes = false;
			Material material3 = new Material(ContentLoader_.LoadMaterial("unlit_transparent"));
			material3.SetTexture("_MainTex", TEX.GetTextureByName("yellow15"));
			trailRenderer3.sharedMaterial = material3;
			trailRenderer3.startWidth = 0.2f;
			trailRenderer3.endWidth = 0.01f;
			trailRenderer3.time = 1f;
			PullObject pullObject13 = new PullObject(c, gameObject, null, 5f);
			pullObject13.rb = rigidbody5;
			pullObject13.uid = iparam;
			pullObject13.ownerid = iparam2;
			pullObject13.goObj = gameObject6;
			pullObject13.status = false;
			pullObject13.maxdist = 0f;
			pullObject13.col = boxCollider3;
			po.Add(pullObject13);
			UpdateCollision(boxCollider3, iparam2);
			break;
		}
		}
	}

	private void Update()
	{
		float time = Time.time;
		PullObject pullObject = null;
		int i = 0;
		for (int count = po.Count; i < count; i++)
		{
			pullObject = po[i];
			if (!pullObject.go.activeSelf)
			{
				continue;
			}
			if (time < pullObject.t)
			{
				if (pullObject.c >= 4 && pullObject.c <= 7)
				{
					pullObject.go.transform.localScale *= 1f + 2f * Time.deltaTime;
				}
				else if (pullObject.c == 8)
				{
					Vector3 vector = pullObject.go.transform.forward * Time.deltaTime * pullObject.speed;
					if (Vector3.Distance(pullObject.go.transform.position + vector, pullObject.origin) > pullObject.maxdist)
					{
						pullObject.go.SetActive(false);
					}
					else
					{
						pullObject.go.transform.position += vector;
					}
				}
				else if (pullObject.c == 9)
				{
					float num = Time.deltaTime * 2f;
					Color color = pullObject.mat.color;
					color.a -= num;
					pullObject.mat.color = color;
				}
				else if (pullObject.c == 12 || pullObject.c == 29)
				{
					if (!pullObject.status && pullObject.t - time < 3f)
					{
						pullObject.status = true;
						if (pullObject.ownerid == Controll.pl.idx)
						{
							Client.cs.send_entpos(pullObject.uid, pullObject.rb.position);
						}
					}
					if (pullObject.maxdist == 0f && pullObject.t - time < 4.9f)
					{
						pullObject.maxdist = 1f;
						pullObject.goObj.SetActive(true);
					}
				}
				else if (pullObject.c == 13)
				{
					if (pullObject.maxdist == 0f && pullObject.t - time < 19.9f)
					{
						pullObject.maxdist = 1f;
						pullObject.goObj.SetActive(true);
					}
				}
				else if (pullObject.c == 14)
				{
					if (pullObject.maxdist == 0f && pullObject.t - time < 9.9f)
					{
						pullObject.maxdist = 1f;
						pullObject.goObj.SetActive(true);
					}
				}
				else if (pullObject.c == 23)
				{
					if (pullObject.maxdist == 0f && pullObject.t - time < 1.9f)
					{
						pullObject.maxdist = 1f;
						pullObject.goObj.SetActive(true);
					}
				}
				else if (pullObject.c == 26)
				{
					if (pullObject.maxdist == 0f && pullObject.t - time < 4.9f)
					{
						pullObject.maxdist = 1f;
						pullObject.goObj.SetActive(true);
					}
				}
				else if (pullObject.c == 27)
				{
					if (pullObject.maxdist == 0f && pullObject.t - time < 29.9f)
					{
						pullObject.maxdist = 1f;
						pullObject.goObj.SetActive(true);
					}
				}
				else if (pullObject.c == 28 && pullObject.maxdist == 0f && pullObject.t - time < 4.9f)
				{
					pullObject.maxdist = 1f;
					pullObject.goObj.SetActive(true);
				}
			}
			else
			{
				pullObject.go.SetActive(false);
			}
		}
	}

	public static void KillbyUID(int uid)
	{
		PullObject pullObject = null;
		int i = 0;
		for (int count = po.Count; i < count; i++)
		{
			pullObject = po[i];
			if (pullObject.go.activeSelf)
			{
				if (pullObject.c == 12 && pullObject.uid == uid)
				{
					pullObject.t = 0f;
				}
				if (pullObject.c == 29 && pullObject.uid == uid)
				{
					pullObject.t = 0f;
				}
			}
		}
	}

	public static void KillbyVector(int x, int y, int z)
	{
		PullObject pullObject = null;
		int i = 0;
		for (int count = po.Count; i < count; i++)
		{
			pullObject = po[i];
			if (pullObject.go.activeSelf && (pullObject.c == 20 || pullObject.c == 21) && (int)pullObject.origin.x == x && (int)pullObject.origin.y == y && (int)pullObject.origin.z == z)
			{
				pullObject.go.SetActive(false);
			}
		}
	}

	public static int GetUID(GameObject go)
	{
		PullObject pullObject = null;
		int i = 0;
		for (int count = po.Count; i < count; i++)
		{
			pullObject = po[i];
			if (pullObject.go.activeSelf && pullObject.go == go)
			{
				return pullObject.uid;
			}
		}
		return -1;
	}

	private static void UpdateCollision(Collider col, int id)
	{
		if (col == null)
		{
			return;
		}
		for (int i = 0; i < 40; i++)
		{
			if (PLH.player[i] != null && !(PLH.player[i].go == null))
			{
				PlayerData obj = PLH.player[i];
				bool ignore = ((i == id) ? true : false);
				Physics.IgnoreCollision(obj.bcBody, col, ignore);
				Physics.IgnoreCollision(obj.bcHead, col, ignore);
				Physics.IgnoreCollision(obj.bcLArm[0], col, ignore);
				Physics.IgnoreCollision(obj.bcLArm[1], col, ignore);
				Physics.IgnoreCollision(obj.bcRArm[0], col, ignore);
				Physics.IgnoreCollision(obj.bcRArm[1], col, ignore);
				Physics.IgnoreCollision(obj.bcLLeg[0], col, ignore);
				Physics.IgnoreCollision(obj.bcLLeg[1], col, ignore);
				Physics.IgnoreCollision(obj.bcLLeg[2], col, ignore);
				Physics.IgnoreCollision(obj.bcRLeg[0], col, ignore);
				Physics.IgnoreCollision(obj.bcRLeg[1], col, ignore);
				Physics.IgnoreCollision(obj.bcRLeg[2], col, ignore);
			}
		}
	}
}
