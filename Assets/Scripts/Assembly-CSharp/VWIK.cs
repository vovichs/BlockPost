using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class VWIK : MonoBehaviour
{
	public class WeaponOffset
	{
		public Vector3 v;

		public Vector3 v1;

		public float ts;

		public float te;

		public float t;

		public int type;

		public bool reverse;

		public WeaponOffset(Vector3 v, float ts, float te, int type, bool rev)
		{
			this.v = v;
			this.ts = ts;
			this.te = te;
			t = te - ts;
			this.type = type;
			reverse = rev;
		}

		public WeaponOffset(Vector3 v0, Vector3 v1, float ts, float te, int type, bool rev)
		{
			v = v0;
			this.v1 = v1;
			this.ts = ts;
			this.te = te;
			t = te - ts;
			this.type = type;
			reverse = rev;
		}
	}

	private static GameObject goRHand;

	private static GameObject goLHand;

	private static GameObject goRBone;

	private static GameObject goLBone;

	private static GameObject goBoneRHand;

	private static GameObject goBoneLHand;

	private static GameObject goWeaponMain;

	private static GameObject goWeapon;

	private static GameObject goBolt0;

	public static Vector3 ar = new Vector3(275f, 270f, 0f);

	public static Vector3 al = new Vector3(350f, 310f, 210f);

	private static Vector3 weapon_def_rot = new Vector3(0f, 270f, 0f);

	public static List<WeaponOffset> woffs = new List<WeaponOffset>();

	private static Vector3 roffset;

	private static Vector3 loffset;

	private static float amount = 0.02f;

	private static float maxAmount = 0.04f;

	private static float Smooth = 3f;

	private static float SmoothRotation = 3f;

	private static float tiltAngle = 1f;

	private static Vector3 def = new Vector3(0f, 0f, -0.02f);

	private static Vector3 vLagPos = Vector3.zero;

	public static Quaternion vLagRot = default(Quaternion);

	private static float timer = 0f;

	private static float bobbingSpeed = 0.045f;

	private static float bobbingAmount = 0.0025f;

	private static float waveslice = 0f;

	private static float horizontal = 0f;

	private static float vertical = 0f;

	private static float totalAxes = 0f;

	private static float translateChange = 0f;

	public static Vector3 vBobPos = Vector3.zero;

	private static Vector3 ofspos;

	private static Vector3 ofsrot;

	private static Vector3 campos;

	private static Vector3 camrot;

	private static Vector3 wofsrot;

	private static Vector3 boltpos;

	private static float zmin = 87f;

	private static float zmax = 93f;

	private static float zforward = 1f;

	public static float shovel_attack = 0f;

	public static bool shovel_hit = false;

	private static float shovel_anim = 0.2f;

	private static Vector3 weapon_pos = new Vector3(1.69f, -1.2f, 2.1f);

	private static Vector3 weapon_rot = new Vector3(299.8f, 172.97f, 5.96f);

	private static Vector3 rhand_pos = new Vector3(0.84f, -1.51f, 0.75f);

	private static Vector3 rhand_rot = new Vector3(359.69f, 258.62f, 13.54f);

	private static Vector3 lhand_pos = new Vector3(-1.42f, -1.72f, 0.96f);

	private static Vector3 lhand_rot = new Vector3(353.55f, 122.8f, 341.99f);

	private static Vector3 weapon_pos_att = new Vector3(0.56f, -0.85f, 2.98f);

	private static Vector3 weapon_rot_att = new Vector3(300.16f, 141.98f, 352.84f);

	private static Vector3 rhand_pos_att = new Vector3(0.58f, -1.26f, 1.45f);

	private static Vector3 rhand_rot_att = new Vector3(6.22f, 209.39f, 12.68f);

	private static Vector3 lhand_pos_att = new Vector3(-0.9f, -1.9f, 0.37f);

	private static Vector3 lhand_rot_att = new Vector3(353.55f, 122.8f, 341.99f);

	private static Vector3 kukri_weapon_pos = new Vector3(14.2f, 1.5f, -5.6f);

	private static Vector3 kukri_weapon_rot = new Vector3(273f, 270f, 0f);

	private static Vector3 kukri_rhand_pos = new Vector3(0.86f, -1.38f, 1.11f);

	private static Vector3 kukri_rhand_rot = new Vector3(341.46f, 270f, 1.35f);

	private static Vector3 kukri_lhand_pos = new Vector3(-1.05f, -1.4f, 1.07f);

	private static Vector3 kukri_lhand_rot = new Vector3(336.7f, 105.19f, 1.11f);

	private static Vector3 kukri_rhand_rot1_att = new Vector3(354f, 200f, 17f);

	private static Vector3 kukri_rhand_rot2_att = new Vector3(352f, 339f, 343f);

	private static Vector3 kukri_lhand_rot2_att = new Vector3(336f, 105f, 54f);

	private static Vector3 katana_weapon_pos = new Vector3(11.8f, 4.2f, -6.1f);

	private static Vector3 katana_weapon_rot = new Vector3(287f, 90f, 180f);

	private static Vector3 katana_rhand_pos = new Vector3(1.25f, -1.22f, 0.87f);

	private static Vector3 katana_rhand_rot = new Vector3(286.37f, 60.1f, 185.07f);

	private static Vector3 katana_lhand_pos = new Vector3(0.03f, -1.07f, 0.83f);

	private static Vector3 katana_lhand_rot = new Vector3(290.57f, 63.34f, 54.61f);

	private static Vector3 katana_rhand_rot1_att = new Vector3(300f, 123f, 125f);

	private static Vector3 katana_rhand_rot2_att = new Vector3(337f, 342f, 252f);

	private static bool inGrenade = false;

	private static float tGrenadeEnd = 0f;

	public static bool inDrop = false;

	private static float tDropEnd = 0f;

	public static Vector2 ShakeOffset = Vector2.zero;

	public static float ShakeForce = 0f;

	public static float ShakeSpeed = 1f;

	public static void Update()
	{
		if (Controll.trCamera == null || Controll.pl.currweapon == null)
		{
			return;
		}
		goRHand = Controll.pl.currweapon.goRHand;
		goLHand = Controll.pl.currweapon.goLHand;
		goRBone = Controll.pl.currweapon.goRBone;
		goLBone = Controll.pl.currweapon.goLBone;
		goBoneRHand = Controll.pl.currweapon.goBoneRHand;
		goBoneLHand = Controll.pl.currweapon.goBoneLHand;
		goWeaponMain = Controll.pl.currweapon.go;
		goBolt0 = Controll.pl.currweapon.goBolt0;
		if (!(goWeaponMain == null) && !(goRHand == null))
		{
			if (Controll.pl.currweapon.ani == null || Controll.pl.currweapon.ani.clip == null)
			{
				UpdateReset();
				UpdateHands();
				UpdateWeaponOfs();
			}
			else
			{
				goWeaponMain.transform.localPosition = Vector3.zero;
				goWeaponMain.transform.localEulerAngles = Vector3.zero;
			}
			UpdateWeaponBob();
			UpdateWeaponLag();
			UpdateWeaponShovel();
			UpdateWeaponBlock();
			UpdateWeaponKarambit();
			UpdateWeaponKukri();
			UpdateWeaponKatana();
			UpdateGrenade();
			UpdateDrop();
		}
	}

	public static void EditorUpdate(WeaponData wd)
	{
		goRHand = wd.goRHand;
		goLHand = wd.goLHand;
		goRBone = wd.goRBone;
		goLBone = wd.goLBone;
		goBoneRHand = wd.goBoneRHand;
		goBoneLHand = wd.goBoneLHand;
		goWeaponMain = wd.go;
		goBolt0 = wd.goBolt0;
		UpdateHandsEditor(wd);
	}

	private static void UpdateHandsEditor(WeaponData wd)
	{
		if (!(goRHand == null))
		{
			roffset = goRHand.transform.position - goRBone.transform.position;
			goRHand.transform.position = goBoneRHand.transform.position + roffset;
			goRHand.transform.localEulerAngles = ar;
			loffset = goLHand.transform.position - goLBone.transform.position;
			goLHand.transform.position = goBoneLHand.transform.position + loffset;
			if (wd.vLRotation != Vector3.zero)
			{
				goLHand.transform.localEulerAngles = wd.vLRotation;
			}
			else
			{
				goLHand.transform.localEulerAngles = al;
			}
		}
	}

	private static void UpdateReset()
	{
		if (!(goWeaponMain == null) && !(goWeapon == null))
		{
			goWeaponMain.transform.localPosition = Vector3.zero;
			goWeaponMain.transform.localEulerAngles = Vector3.zero;
			if (goWeapon.transform.localScale.x != 0.05f && goWeapon.transform.localScale.x != 1f)
			{
				goWeapon.transform.localEulerAngles = weapon_def_rot;
			}
			goWeapon.transform.localPosition = Controll.pl.currweapon.vPosition;
		}
	}

	private static void UpdateHands()
	{
		if (!(goRHand == null))
		{
			roffset = goRHand.transform.position - goRBone.transform.position;
			goRHand.transform.position = goBoneRHand.transform.position + roffset;
			goRHand.transform.localEulerAngles = ar;
			loffset = goLHand.transform.position - goLBone.transform.position;
			goLHand.transform.position = goBoneLHand.transform.position + loffset;
			if (Controll.pl.currweapon.vLRotation != Vector3.zero)
			{
				goLHand.transform.localEulerAngles = Controll.pl.currweapon.vLRotation;
			}
			else
			{
				goLHand.transform.localEulerAngles = al;
			}
		}
	}

	private static void UpdateWeaponLag()
	{
	}

	private static void UpdateWeaponBob()
	{
		waveslice = 0f;
		horizontal = ControllTouch.movex;
		vertical = ControllTouch.movez;
		if (Mathf.Abs(horizontal) == 0f && Mathf.Abs(vertical) == 0f)
		{
			timer = 0f;
		}
		else
		{
			waveslice = Mathf.Sin(timer);
			timer += bobbingSpeed * Time.deltaTime * 200f;
			if (timer > (float)Math.PI * 2f)
			{
				timer -= (float)Math.PI * 2f;
			}
		}
		translateChange = waveslice * bobbingAmount * Controll.speed * 2f;
		totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
		totalAxes = Mathf.Clamp(totalAxes, 0f, 1f);
		translateChange = totalAxes * translateChange;
		float num = translateChange;
		if (num < 0f)
		{
			num *= -1f;
		}
		vBobPos = new Vector3(translateChange, num, vBobPos.z);
		goWeaponMain.transform.localPosition += vBobPos;
	}

	private static void UpdateWeaponOfs()
	{
		if (woffs.Count == 0 || goWeapon == null)
		{
			return;
		}
		float time = Time.time;
		int num = woffs.Count;
		ofspos = Vector3.zero;
		ofsrot = Vector3.zero;
		campos = Vector3.zero;
		camrot = Vector3.zero;
		wofsrot = Vector3.zero;
		boltpos = Vector3.zero;
		for (int i = 0; i < num; i++)
		{
			if (time < woffs[i].ts)
			{
				continue;
			}
			if (time >= woffs[i].te)
			{
				woffs.RemoveAt(i);
				i--;
				num--;
				continue;
			}
			ofspos = Vector3.zero;
			ofsrot = Vector3.zero;
			campos = Vector3.zero;
			camrot = Vector3.zero;
			wofsrot = Vector3.zero;
			if (woffs[i].type == 0)
			{
				float num2 = (Time.time - woffs[i].ts) / woffs[i].t;
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				if (woffs[i].reverse)
				{
					ofspos = Vector3.Lerp(Vector3.zero, woffs[i].v, num2);
				}
				else
				{
					ofspos = Vector3.Lerp(woffs[i].v, Vector3.zero, num2);
				}
			}
			else if (woffs[i].type == 1)
			{
				ofspos = woffs[i].v;
			}
			else if (woffs[i].type == 2)
			{
				float num3 = (Time.time - woffs[i].ts) / woffs[i].t;
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				if (woffs[i].reverse)
				{
					ofsrot = Vector3.Lerp(Vector3.zero, woffs[i].v, num3);
				}
				else
				{
					ofsrot = Vector3.Lerp(woffs[i].v, Vector3.zero, num3);
				}
			}
			else if (woffs[i].type == 3)
			{
				ofsrot = woffs[i].v;
			}
			if (woffs[i].type == 4)
			{
				float num4 = (Time.time - woffs[i].ts) / woffs[i].t;
				if (num4 > 1f)
				{
					num4 = 1f;
				}
				if (woffs[i].reverse)
				{
					campos = Vector3.Lerp(Vector3.zero, woffs[i].v, num4);
				}
				else
				{
					campos = Vector3.Lerp(woffs[i].v, Vector3.zero, num4);
				}
			}
			else if (woffs[i].type == 5)
			{
				campos = woffs[i].v;
			}
			else if (woffs[i].type == 6)
			{
				float num5 = (Time.time - woffs[i].ts) / woffs[i].t;
				if (num5 > 1f)
				{
					num5 = 1f;
				}
				if (woffs[i].reverse)
				{
					camrot = Vector3.Lerp(Vector3.zero, woffs[i].v, num5);
				}
				else
				{
					camrot = Vector3.Lerp(woffs[i].v, Vector3.zero, num5);
				}
			}
			else if (woffs[i].type == 7)
			{
				camrot = woffs[i].v;
			}
			else if (woffs[i].type == 8)
			{
				float num6 = (Time.time - woffs[i].ts) / woffs[i].t;
				if (num6 > 1f)
				{
					num6 = 1f;
				}
				if (woffs[i].reverse)
				{
					wofsrot = Vector3.Lerp(Vector3.zero, woffs[i].v, num6);
				}
				else
				{
					wofsrot = Vector3.Lerp(woffs[i].v, Vector3.zero, num6);
				}
			}
			else if (woffs[i].type == 9)
			{
				wofsrot = woffs[i].v;
			}
			else if (woffs[i].type == 10)
			{
				float num7 = (Time.time - woffs[i].ts) / woffs[i].t;
				if (num7 > 1f)
				{
					num7 = 1f;
				}
				if (woffs[i].reverse)
				{
					boltpos = Vector3.Lerp(woffs[i].v1, woffs[i].v, num7);
				}
				else
				{
					boltpos = Vector3.Lerp(woffs[i].v, woffs[i].v1, num7);
				}
			}
			goWeaponMain.transform.localPosition += ofspos;
			goWeaponMain.transform.localEulerAngles += ofsrot;
			Controll.trCamera.localPosition += campos;
			Controll.trCamera.localEulerAngles += camrot;
			goWeapon.transform.localEulerAngles += wofsrot;
			goRHand.transform.localPosition += boltpos;
			goLHand.transform.localEulerAngles += wofsrot;
			goRHand.transform.localEulerAngles += wofsrot;
		}
	}

	private static void UpdateWeaponShovel()
	{
		if (Controll.pl.currweapon == null || (Controll.pl.currweapon.wi.id != 68 && Controll.pl.currweapon.wi.id != 171))
		{
			return;
		}
		goWeapon = Controll.pl.currweapon.goWeapon;
		float num = goWeapon.transform.localEulerAngles.z + 35f * Time.deltaTime * zforward * Controll.speed;
		if (num > zmax)
		{
			zforward = -1f;
			num = zmax;
		}
		if (num < zmin)
		{
			zforward = 1f;
			num = zmin;
		}
		goWeapon.transform.localEulerAngles = new Vector3(0f, 0f, num);
		if (shovel_attack != 0f)
		{
			float num2 = Time.time - shovel_attack;
			if (num2 > shovel_anim)
			{
				shovel_attack = 0f;
				return;
			}
			float num3 = 1f - num2 * 1f / shovel_anim;
			goWeapon.transform.localEulerAngles = new Vector3(90f * num3, 0f, 90f);
			goWeaponMain.transform.localEulerAngles = new Vector3(8f * num3, 0f, 0f);
		}
	}

	private static void UpdateWeaponBlock()
	{
		goWeapon = Controll.pl.currweapon.goWeapon;
		if (goWeapon.transform.localScale.x != 1f)
		{
			return;
		}
		float num = goWeapon.transform.localEulerAngles.z + 35f * Time.deltaTime * zforward * Controll.speed;
		if (num > 13f)
		{
			zforward = -1f;
			num = 13f;
		}
		if (num < 7f)
		{
			zforward = 1f;
			num = 7f;
		}
		goWeapon.transform.localEulerAngles = new Vector3(5f, 240f, num);
		if (shovel_attack != 0f)
		{
			float num2 = Time.time - shovel_attack;
			if (num2 > shovel_anim)
			{
				shovel_attack = 0f;
				return;
			}
			float num3 = 1f - num2 * 1f / shovel_anim;
			goWeapon.transform.localEulerAngles = new Vector3(5f - num3 * 25f, 240f + num3 * 25f, 10f - num3 * 25f);
			goWeaponMain.transform.localEulerAngles = new Vector3(30f * num3, 0f, 0f);
		}
	}

	private static void UpdateWeaponKarambit()
	{
		if (Controll.pl.currweapon == null || Controll.pl.currweapon.wi.id != 106)
		{
			return;
		}
		goWeapon = Controll.pl.currweapon.goWeapon;
		goWeapon.transform.localPosition = weapon_pos;
		goWeapon.transform.localEulerAngles = weapon_rot;
		goRHand.transform.localPosition = rhand_pos;
		goRHand.transform.localEulerAngles = rhand_rot;
		goLHand.transform.localPosition = lhand_pos;
		goLHand.transform.localEulerAngles = lhand_rot;
		if (shovel_attack == 0f)
		{
			return;
		}
		float num = Time.time - shovel_attack;
		if (num > shovel_anim)
		{
			shovel_attack = 0f;
			return;
		}
		float t = num / shovel_anim;
		if (shovel_hit && num < 0.1f)
		{
			t = 0f;
		}
		Vector3 localPosition = Vector3.Lerp(rhand_pos_att, rhand_pos, t);
		Vector3 localEulerAngles = default(Vector3);
		localEulerAngles.x = Mathf.LerpAngle(rhand_rot_att.x, rhand_rot.x, t);
		localEulerAngles.y = Mathf.LerpAngle(rhand_rot_att.y, rhand_rot.y, t);
		localEulerAngles.z = Mathf.LerpAngle(rhand_rot_att.z, rhand_rot.z, t);
		Vector3 localPosition2 = Vector3.Lerp(lhand_pos_att, lhand_pos, t);
		Vector3 localEulerAngles2 = default(Vector3);
		localEulerAngles2.x = Mathf.LerpAngle(lhand_rot_att.x, lhand_rot.x, t);
		localEulerAngles2.y = Mathf.LerpAngle(lhand_rot_att.y, lhand_rot.y, t);
		localEulerAngles2.z = Mathf.LerpAngle(lhand_rot_att.z, lhand_rot.z, t);
		goWeapon.transform.parent = goRHand.transform;
		goRHand.transform.localPosition = localPosition;
		goRHand.transform.localEulerAngles = localEulerAngles;
		goLHand.transform.localPosition = localPosition2;
		goLHand.transform.localEulerAngles = localEulerAngles2;
		goWeapon.transform.parent = goWeaponMain.transform;
	}

	private static void UpdateWeaponKukri()
	{
		if (Controll.pl.currweapon == null || Controll.pl.currweapon.wi.id != 104)
		{
			return;
		}
		goWeapon = Controll.pl.currweapon.goWeapon;
		goWeapon.transform.parent = goRHand.transform;
		goWeapon.transform.localPosition = kukri_weapon_pos;
		goWeapon.transform.localEulerAngles = kukri_weapon_rot;
		goRHand.transform.localPosition = kukri_rhand_pos;
		goRHand.transform.localEulerAngles = kukri_rhand_rot;
		goLHand.transform.localPosition = kukri_lhand_pos;
		goLHand.transform.localEulerAngles = kukri_lhand_rot;
		goWeapon.transform.parent = goWeaponMain.transform;
		if (shovel_attack == 0f)
		{
			return;
		}
		float num = Time.time - shovel_attack;
		if (num > 0.35f)
		{
			shovel_attack = 0f;
			return;
		}
		float num2 = num / 0.35f;
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		if (num2 <= 0.2f)
		{
			zero = kukri_rhand_rot1_att;
		}
		else if (num2 <= 0.4f)
		{
			num2 = (num2 - 0.2f) * 5f;
			zero.x = Mathf.LerpAngle(kukri_rhand_rot1_att.x, kukri_rhand_rot2_att.x, num2);
			zero.y = Mathf.LerpAngle(kukri_rhand_rot1_att.y, kukri_rhand_rot2_att.y, num2);
			zero.z = Mathf.LerpAngle(kukri_rhand_rot1_att.z, kukri_rhand_rot2_att.z, num2);
			zero2 = kukri_lhand_rot2_att;
		}
		else if (num2 <= 0.6f)
		{
			num2 = (num2 - 0.4f) * 5f;
			zero = kukri_rhand_rot2_att;
			zero2 = kukri_lhand_rot2_att;
		}
		else
		{
			num2 = (num2 - 0.6f) * 2.5f;
			zero.x = Mathf.LerpAngle(kukri_rhand_rot2_att.x, kukri_rhand_rot.x, num2);
			zero.y = Mathf.LerpAngle(kukri_rhand_rot2_att.y, kukri_rhand_rot.y, num2);
			zero.z = Mathf.LerpAngle(kukri_rhand_rot2_att.z, kukri_rhand_rot.z, num2);
			zero2.x = Mathf.LerpAngle(kukri_lhand_rot2_att.x, kukri_lhand_rot.x, num2);
			zero2.y = Mathf.LerpAngle(kukri_lhand_rot2_att.y, kukri_lhand_rot.y, num2);
			zero2.z = Mathf.LerpAngle(kukri_lhand_rot2_att.z, kukri_lhand_rot.z, num2);
		}
		goWeapon.transform.parent = goRHand.transform;
		goRHand.transform.localEulerAngles = zero;
		goLHand.transform.localEulerAngles = zero2;
		goWeapon.transform.parent = goWeaponMain.transform;
	}

	private static void UpdateWeaponKatana()
	{
		if (Controll.pl.currweapon == null || Controll.pl.currweapon.wi.id != 105)
		{
			return;
		}
		goWeapon = Controll.pl.currweapon.goWeapon;
		goWeapon.transform.parent = goRHand.transform;
		goWeapon.transform.localPosition = katana_weapon_pos;
		goWeapon.transform.localEulerAngles = katana_weapon_rot;
		goRHand.transform.localPosition = katana_rhand_pos;
		goRHand.transform.localEulerAngles = katana_rhand_rot;
		goLHand.transform.localPosition = katana_lhand_pos;
		goLHand.transform.localEulerAngles = katana_lhand_rot;
		goWeapon.transform.parent = goWeaponMain.transform;
		if (shovel_attack == 0f)
		{
			return;
		}
		float num = Time.time - shovel_attack;
		if (num > 0.5f)
		{
			shovel_attack = 0f;
			return;
		}
		float num2 = num / 0.5f;
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		if (num2 <= 0.1f)
		{
			zero = katana_rhand_pos;
			zero2 = katana_rhand_rot1_att;
		}
		else if (num2 <= 0.2f)
		{
			num2 = (num2 - 0.1f) * 10f;
			zero = katana_rhand_pos;
			zero2.x = Mathf.LerpAngle(katana_rhand_rot1_att.x, katana_rhand_rot2_att.x, num2);
			zero2.y = Mathf.LerpAngle(katana_rhand_rot1_att.y, katana_rhand_rot2_att.y, num2);
			zero2.z = Mathf.LerpAngle(katana_rhand_rot1_att.z, katana_rhand_rot2_att.z, num2);
		}
		else if (num2 <= 0.4f)
		{
			num2 = (num2 - 0.2f) * 5f;
			zero = katana_rhand_pos;
			zero2 = katana_rhand_rot2_att;
		}
		else
		{
			num2 = (num2 - 0.4f) * 1.7f;
			zero = katana_rhand_pos;
			zero2.x = Mathf.LerpAngle(katana_rhand_rot2_att.x, katana_rhand_rot.x, num2);
			zero2.y = Mathf.LerpAngle(katana_rhand_rot2_att.y, katana_rhand_rot.y, num2);
			zero2.z = Mathf.LerpAngle(katana_rhand_rot2_att.z, katana_rhand_rot.z, num2);
		}
		goWeapon.transform.parent = goRHand.transform;
		goLHand.transform.parent = goRHand.transform;
		goRHand.transform.localPosition = zero;
		goRHand.transform.localEulerAngles = zero2;
		goWeapon.transform.parent = goWeaponMain.transform;
		goLHand.transform.parent = goWeaponMain.transform;
	}

	public static void SetGrenade()
	{
		inGrenade = true;
		tGrenadeEnd = Time.time + 0.25f;
	}

	private static void UpdateGrenade()
	{
		if (!inGrenade)
		{
			return;
		}
		if (Time.time > tGrenadeEnd)
		{
			if (Controll.pl.currweapon.ani != null && Controll.pl.currweapon.wield != null)
			{
				Controll.pl.currweapon.ani.clip = Controll.pl.currweapon.wield;
				Controll.pl.currweapon.ani.Stop();
				Controll.pl.currweapon.ani.Play();
			}
			else
			{
				UpdateHands();
			}
			inGrenade = false;
		}
		else
		{
			float t = 1f - (tGrenadeEnd - Time.time) * 4f;
			float z = Mathf.LerpAngle(100f, 320f, t);
			goRHand.transform.localPosition = new Vector3(0.6f, -1.25f, 0.8f);
			goRHand.transform.localEulerAngles = new Vector3(342f, 242f, z);
			goLHand.transform.localPosition = new Vector3(0f, -10f, 0f);
			goWeapon.transform.localPosition = new Vector3(0f, -10f, 0f);
		}
	}

	public static void SetDrop()
	{
		inDrop = true;
		tDropEnd = Time.time + 0.4f;
	}

	private static void UpdateDrop()
	{
		if (!inDrop)
		{
			return;
		}
		if (Time.time > tDropEnd)
		{
			if (Controll.pl.currweapon.ani != null && Controll.pl.currweapon.wield != null)
			{
				Controll.pl.currweapon.ani.clip = Controll.pl.currweapon.wield;
				Controll.pl.currweapon.ani.Stop();
				Controll.pl.currweapon.ani.Play();
			}
			else
			{
				UpdateHands();
			}
			inDrop = false;
		}
		else
		{
			float t = 1f - (tDropEnd - Time.time) * 2.5f;
			float z = Mathf.LerpAngle(45f, 300f, t);
			goRHand.transform.localPosition = new Vector3(0.7f, -1.5f, 0.8f);
			goRHand.transform.localEulerAngles = new Vector3(7f, 270f, z);
			float z2 = Mathf.LerpAngle(315f, 60f, t);
			goLHand.transform.localPosition = new Vector3(-0.7f, -1.5f, 0.8f);
			goLHand.transform.localEulerAngles = new Vector3(7f, 90f, z2);
			goWeapon.transform.localPosition = new Vector3(0f, -10f, 0f);
		}
	}

	public static void ShakeCamera()
	{
		if (ShakeForce <= 0f)
		{
			if (ShakeOffset != Vector2.zero)
			{
				ShakeOffset = Vector2.zero;
			}
			return;
		}
		float num = UnityEngine.Random.Range(0f - ShakeForce, ShakeForce);
		if ((bool)Controll.trCamera)
		{
			Controll.trCamera.localPosition += new Vector3(num, 0f, num);
		}
		ShakeForce -= Time.deltaTime * ShakeSpeed;
	}

	public static void AddOffset(Vector3 v, float timestart, float time, bool reverse = false)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 0, reverse));
	}

	public static void SetOffset(Vector3 v, float timestart, float time)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 1, false));
	}

	public static void AddAngle(Vector3 v, float timestart, float time, bool reverse = false)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 2, reverse));
	}

	public static void SetAngle(Vector3 v, float timestart, float time)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 3, false));
	}

	public static void ClearOffset()
	{
		woffs.Clear();
	}

	public static void CameraAddOffset(Vector3 v, float timestart, float time, bool reverse = false)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 4, reverse));
	}

	public static void CameraSetOffset(Vector3 v, float timestart, float time)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 5, false));
	}

	public static void CameraAddAngle(Vector3 v, float timestart, float time, bool reverse = false)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 6, reverse));
	}

	public static void CameraSetAngle(Vector3 v, float timestart, float time)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 7, false));
	}

	public static void WeaponAddAngle(Vector3 v, float timestart, float time, bool reverse = false)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 8, reverse));
	}

	public static void WeaponSetAngle(Vector3 v, float timestart, float time)
	{
		woffs.Add(new WeaponOffset(v, timestart, timestart + time, 9, false));
	}

	public static void BoltAddOffset(Vector3 v0, Vector3 v1, float timestart, float time, bool reverse = false)
	{
		woffs.Add(new WeaponOffset(v0, v1, timestart, timestart + time, 10, reverse));
	}

	public static void SetCameraShake(float amount, float speed)
	{
		ShakeForce = amount;
		ShakeSpeed = speed;
	}
}
