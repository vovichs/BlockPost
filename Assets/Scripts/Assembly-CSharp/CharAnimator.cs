using Player;
using UnityEngine;

public class CharAnimator : MonoBehaviour
{
	private static Vector3 crouch_pos = new Vector3(0f, -1.5f, -0.4f);

	private static Vector3 crouch_lleg = new Vector3(-0.2f, -1.5f, -0.3f);

	private static Vector3 crouch_rleg = new Vector3(0.2f, -1.5f, -0.3f);

	private static float crouch_angle = 30f;

	private static Vector3 crouchmove_pos = new Vector3(0f, -1.4f, -0.4f);

	private static Vector3 crouchmove_lleg = new Vector3(-0.2f, -1.4f, -0.3f);

	private static Vector3 crouchmove_rleg = new Vector3(0.2f, -1.4f, -0.3f);

	private static float crouchmove_angle = 30f;

	private static Vector3 default_pos = new Vector3(0f, -1f, 0f);

	private static Vector3 default_lleg = new Vector3(-0.2f, -1f, 0f);

	private static Vector3 default_rleg = new Vector3(0.2f, -1f, 0f);

	private static float default_angle = 0f;

	private static float head_angle = 0f;

	private static Vector3[] pos_idle = new Vector3[4]
	{
		new Vector3(3.58f, 348.5f, 292.76f),
		new Vector3(359f, 293f, 347.44f),
		new Vector3(338f, 1.15f, 80.9f),
		new Vector3(15f, 56f, 21f)
	};

	private static Vector3[] pos_idle_weapon = new Vector3[4]
	{
		new Vector3(336f, 346f, 298f),
		new Vector3(355f, 286f, 285f),
		new Vector3(342f, 293f, 84f),
		new Vector3(330f, 330f, 82f)
	};

	private static Vector3[] pos_weapon = new Vector3[4]
	{
		new Vector3(338f, 0f, 275f),
		new Vector3(0f, 272f, 317f),
		new Vector3(321f, 323f, 137f),
		new Vector3(346f, 328f, 39f)
	};

	private static Vector3 weapon_rotation = new Vector3(46.9f, 84.6f, 66f);

	private static float g_deltatime = 0f;

	public static CharAnimator cs = null;

	private void Awake()
	{
		cs = this;
	}

	public void _Update()
	{
		g_deltatime = Time.deltaTime;
		for (int i = 0; i < 40; i++)
		{
			if (PLH.player[i] != null && PLH.player[i].visible)
			{
				UpdateLegs(PLH.player[i]);
				UpdateBody(PLH.player[i]);
				UpdateWaterSplash(PLH.player[i]);
			}
		}
	}

	public static void UpdateLegs(PlayerData p)
	{
		p.prevbstate = 999;
		if (p.goLLeg[0] == null)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		if (p.bstate == 0)
		{
			if (p.prevbstate != 0)
			{
				SetTransformNormal(p);
			}
			float num5 = g_deltatime * 200f;
			if (p.la > 0f)
			{
				p.la -= num5;
				if (p.la < 0f)
				{
					p.la = 0f;
				}
			}
			else if (p.la < 0f)
			{
				p.la += num5;
				if (p.la > 0f)
				{
					p.la = 0f;
				}
			}
		}
		if (p.bstate == 1)
		{
			if (p.prevbstate != 1)
			{
				SetTransformNormal(p);
			}
			float num6 = g_deltatime * 200f * (float)p.lf;
			p.la += num6;
			if (p.la > p.leg_limit)
			{
				p.la = p.leg_limit;
				p.lf = -1;
			}
			if (p.la < 0f - p.leg_limit)
			{
				p.la = 0f - p.leg_limit;
				p.lf = 1;
			}
		}
		if (p.bstate == 2)
		{
			if (p.prevbstate != 2)
			{
				SetTransformCrouch(p);
			}
			float num7 = g_deltatime * 200f;
			if (p.la > 0f)
			{
				p.la -= num7;
				if (p.la < 0f)
				{
					p.la = 0f;
				}
			}
			else if (p.la < 0f)
			{
				p.la += num7;
				if (p.la > 0f)
				{
					p.la = 0f;
				}
			}
			num = 80f;
			num2 = 0f;
			num3 = 90f;
			num4 = 110f;
		}
		if (p.bstate == 3)
		{
			if (p.prevbstate != 3)
			{
				SetTransformCrouchMove(p);
			}
			float num8 = g_deltatime * 200f * (float)p.lf;
			p.la += num8;
			if (p.la > 90f)
			{
				p.la = 90f;
				p.lf = -1;
			}
			if (p.la < -10f)
			{
				p.la = -10f;
				p.lf = 1;
			}
			float num9 = p.la - 80f;
			float num10 = 0f - p.la;
			p.goLLeg[0].transform.localEulerAngles = new Vector3(num9, 0f, 0f);
			p.goRLeg[0].transform.localEulerAngles = new Vector3(num10, 0f, 0f);
			p.goLLeg[1].transform.localEulerAngles = new Vector3(0f - num9 + 90f + num9, 0f, 0f);
			p.goRLeg[1].transform.localEulerAngles = new Vector3(0f - num10 + 90f + num10, 0f, 0f);
			return;
		}
		if (p.bstate == 4)
		{
			if (p.prevbstate != 4)
			{
				SetTransformNormal(p);
			}
			float num11 = Time.deltaTime * 200f;
			if (p.la > 0f || p.lf > 0)
			{
				p.la += num11;
				if (p.la > p.leg_limit)
				{
					p.la = p.leg_limit;
				}
			}
			else if (p.la < 0f)
			{
				p.la -= num11;
				if (p.la < 0f - p.leg_limit)
				{
					p.la = 0f - p.leg_limit;
				}
			}
		}
		if (p.bstate != 5)
		{
			p.goLLeg[0].transform.localEulerAngles = new Vector3(p.la - num, 0f, 0f);
			p.goRLeg[0].transform.localEulerAngles = new Vector3(0f - p.la - num2, 0f, 0f);
			float num12 = p.la;
			if (p.la > 0f)
			{
				num12 = p.la * 2f;
			}
			float num13 = 0f - p.la;
			if (0f - p.la > 0f)
			{
				num13 = (0f - p.la) * 2f;
			}
			p.goLLeg[1].transform.localEulerAngles = new Vector3(num12 - p.la + num3, 0f, 0f);
			p.goRLeg[1].transform.localEulerAngles = new Vector3(num13 + p.la + num4, 0f, 0f);
		}
	}

	public static void UpdateBody(PlayerData p)
	{
		if (p.goLLeg[0] == null || p.bstate == 5)
		{
			return;
		}
		p.wstate = 2;
		if (p.wstate == 0)
		{
			SetIdleNew(p);
		}
		else if (p.wstate == 1)
		{
			SetIdleWeapon(p);
		}
		else if (p.wstate == 2)
		{
			if (p.currweapon != null && p.currweapon.pBase != null)
			{
				p.goRArm[0].transform.localEulerAngles = p.currweapon.pBase.r[0];
				p.goRArm[1].transform.localEulerAngles = p.currweapon.pBase.r[1];
				p.goLArm[0].transform.localEulerAngles = p.currweapon.pBase.r[2];
				p.goLArm[1].transform.localEulerAngles = p.currweapon.pBase.r[3];
			}
			else
			{
				p.goRArm[0].transform.localEulerAngles = pos_weapon[0];
				p.goRArm[1].transform.localEulerAngles = pos_weapon[1];
				p.goLArm[0].transform.localEulerAngles = pos_weapon[2];
				p.goLArm[1].transform.localEulerAngles = pos_weapon[3];
			}
		}
		if (p.updaterotx)
		{
			float num = p.nextFrame - Time.realtimeSinceStartup;
			if (num < 0f)
			{
				num = 0f;
			}
			float t = (PLH.cl_fps - num) / PLH.cl_fps;
			p.currRot[0] = Mathf.Lerp((p.prevRot[0] >= 270f) ? (p.prevRot[0] - 360f) : p.prevRot[0], (p.Rot[0] >= 270f) ? (p.Rot[0] - 360f) : p.Rot[0], t);
			float num2 = p.currRot[0] + head_angle;
			num2 = num2 / 90f * 60f;
			if (p.bstate == 2 || p.bstate == 3)
			{
				num2 -= 7.5f;
			}
			p.goHead.transform.localEulerAngles = new Vector3(num2, 0f, 0f);
			float num3 = num2 / 2f;
			if (p.currweapon != null && p.currweapon.goPW != null && p.currweapon.wi != null && p.currweapon.wi.slot != 2)
			{
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(0f, 270f, 0f - num3);
			}
			p.goArmHelp.transform.localEulerAngles = new Vector3(num3, 0f, 0f);
		}
	}

	public static void SetIdle(PlayerData p)
	{
		p.goRArm[0].transform.localEulerAngles = pos_idle[0];
		p.goRArm[1].transform.localEulerAngles = pos_idle[1];
		p.goLArm[0].transform.localEulerAngles = pos_idle[2];
		p.goLArm[1].transform.localEulerAngles = pos_idle[3];
	}

	public static void SetIdleNew(PlayerData p)
	{
		p.goRArm[0].transform.localEulerAngles = new Vector3(1.67502f, 358.618f, 281.9428f);
		p.goRArm[1].transform.localEulerAngles = new Vector3(2.66901f, 350.1923f, 347.5605f);
		p.goLArm[0].transform.localEulerAngles = new Vector3(356.9198f, 358.1182f, 81.9467f);
		p.goLArm[1].transform.localEulerAngles = new Vector3(358.6052f, 10.77333f, 12.44588f);
		p.goRLeg[0].transform.localEulerAngles = new Vector3(4.39091f, 6.96573f, 2.32498f);
		p.goRLeg[1].transform.localEulerAngles = new Vector3(357.4252f, 0f, 0f);
		p.goLLeg[0].transform.localEulerAngles = new Vector3(357.1478f, 346.0201f, 358.097f);
		p.goLLeg[1].transform.localEulerAngles = new Vector3(3.94814f, 2.31111f, 0f);
	}

	public static void SetIdleWeapon(PlayerData p)
	{
		p.goRArm[0].transform.localEulerAngles = pos_idle_weapon[0];
		p.goRArm[1].transform.localEulerAngles = pos_idle_weapon[1];
		p.goLArm[0].transform.localEulerAngles = pos_idle_weapon[2];
		p.goLArm[1].transform.localEulerAngles = pos_idle_weapon[3];
	}

	public static void SetTransformNormal(PlayerData p)
	{
		p.goBody.transform.localPosition = default_pos;
		p.goBody.transform.localEulerAngles = new Vector3(default_angle, 0f, 0f);
		p.goLLeg[0].transform.localPosition = default_lleg;
		p.goRLeg[0].transform.localPosition = default_rleg;
		head_angle = default_angle;
	}

	public static void SetTransformCrouch(PlayerData p)
	{
		p.goBody.transform.localPosition = crouch_pos;
		p.goBody.transform.localEulerAngles = new Vector3(crouch_angle, 0f, 0f);
		p.goLLeg[0].transform.localPosition = crouch_lleg;
		p.goRLeg[0].transform.localPosition = crouch_rleg;
		head_angle = 0f - crouch_angle;
	}

	public static void SetTransformCrouchMove(PlayerData p)
	{
		p.goBody.transform.localPosition = crouchmove_pos;
		p.goBody.transform.localEulerAngles = new Vector3(crouchmove_angle, 0f, 0f);
		p.goLLeg[0].transform.localPosition = crouchmove_lleg;
		p.goRLeg[0].transform.localPosition = crouchmove_rleg;
		head_angle = 0f - crouchmove_angle;
	}

	public static void UpdateWaterSplash(PlayerData p)
	{
		if (GUIOptions.globalpreset == 0 || !p.updatepos)
		{
			return;
		}
		if (p.currPos.y - PLH.pyofs > Map.WATER_LEVEL)
		{
			if (p.goWaterSplash.activeSelf)
			{
				p.goWaterSplash.SetActive(false);
			}
			return;
		}
		if (p.tWaterSplash > Time.time)
		{
			if (p.goWaterSplash.activeSelf)
			{
				p.goWaterSplash.transform.eulerAngles = Vector3.zero;
			}
			return;
		}
		p.tWaterSplash = Time.time + ((p.bstate == 0) ? 0.2f : 0.1f);
		p.frameWaterSplash++;
		if (p.frameWaterSplash >= 5)
		{
			p.frameWaterSplash = 0;
		}
		if (!p.goWaterSplash.activeSelf)
		{
			p.goWaterSplash.SetActive(true);
		}
		p.goWaterSplash.transform.position = new Vector3(p.currPos.x, Map.WATER_LEVEL, p.currPos.z);
		p.goWaterSplash.transform.eulerAngles = Vector3.zero;
		p.matWaterSplash.SetTextureScale("_MainTex", new Vector2(0.125f, 1f));
		p.matWaterSplash.SetTextureOffset("_MainTex", new Vector2(0.125f * (float)p.frameWaterSplash, 0f));
	}

	public static void RestoreBody(PlayerData p)
	{
		p.goHead.transform.localPosition = p.restoreHead;
		p.goLArm[0].transform.localPosition = p.restoreLArm;
		p.goRArm[0].transform.localPosition = p.restoreRArm;
	}
}
