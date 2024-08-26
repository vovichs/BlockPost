using Player;
using UnityEngine;

public class PLH : MonoBehaviour
{
	public static float pyofs = 2.08f;

	public static float cl_fps = 0.05f;

	public static int smooth_move = 1;

	public static PlayerData[] player = new PlayerData[40];

	private static float fs_time = 0f;

	private static AudioClip[] concrete = new AudioClip[5];

	private static AudioClip[] dirt = new AudioClip[6];

	private static AudioClip[] swamp = new AudioClip[7];

	private static AudioClip[] flame = new AudioClip[3];

	private static AudioClip sDraw = null;

	private static AudioClip sInfect = null;

	public static AudioClip sZombieHit = null;

	public static AudioClip sTaserHit = null;

	private static float g_realtimeSinceStartup = 0f;

	private static float g_time = 0f;

	public static float g_protecttime = 5f;

	public static float g_freezetime = 0f;

	public static int g_blocklimit = 50;

	private static Vector3 a;

	private static Vector3 b;

	private int pcount;

	private int vcount;

	private Bounds bound;

	private Vector3 minofs = new Vector3(1f, 0f, 1f);

	private Vector3 maxofs = new Vector3(1f, 3f, 1f);

	private bool vis;

	public static bool testcull = true;

	public static void Clear()
	{
		for (int i = 0; i < 40; i++)
		{
			if (player[i] != null)
			{
				ClearWeapon(player[i]);
				if (player[i].go != null)
				{
					Object.Destroy(player[i].go);
				}
				player[i] = null;
			}
		}
		g_freezetime = 0f;
		g_blocklimit = 50;
	}

	public static void Add(int id, string name, int team, int cp0, int cp1, int cp2, int cp3, int cp4, int cp5, int cp6, int cp7, int cp8, int cp9, int pexp, int cid, string cname)
	{
		if (player[id] == null)
		{
			if (DemoRec.isDemo())
			{
				DemoRec.stats_AddPlayer(name);
			}
			if (cp0 > 2)
			{
				cp0 = 0;
			}
			if (cp1 > 2)
			{
				cp1 = 0;
			}
			if (cp2 > 3)
			{
				cp2 = 0;
			}
			if (cp3 >= GUIInv.tHair.Length)
			{
				cp3 = 0;
			}
			if (cp4 >= GUIInv.tEye.Length)
			{
				cp4 = 0;
			}
			if (cp5 >= GUIInv.tBeard.Length)
			{
				cp5 = 0;
			}
			if (cp6 >= GUIInv.tHat.Length)
			{
				cp6 = 0;
			}
			if (cp7 >= GUIInv.tBody.Length)
			{
				cp7 = 0;
			}
			if (cp8 >= GUIInv.tPants.Length)
			{
				cp8 = 0;
			}
			if (cp9 >= GUIInv.tBoots.Length)
			{
				cp9 = 0;
			}
			if (id == Controll.pl.idx)
			{
				player[id] = Controll.pl;
			}
			else
			{
				player[id] = new PlayerData();
			}
			player[id].cp[0] = cp0;
			player[id].cp[1] = cp1;
			player[id].cp[2] = cp2;
			player[id].cp[3] = cp3;
			player[id].cp[4] = cp4;
			player[id].cp[5] = cp5;
			player[id].cp[6] = cp6;
			player[id].cp[7] = cp7;
			player[id].cp[8] = cp8;
			player[id].cp[9] = cp9;
			player[id].idx = id;
			player[id].name = name;
			player[id].team = team;
			player[id].pexp = pexp;
			player[id].sLVL = Main.CalcLevel(pexp).ToString();
			player[id].formatname = "<color=#F60>" + player[id].sLVL + "</color> " + name;
			player[id].clanid = cid;
			player[id].clanname = cname;
			player[id].go = VCGen.Build(player[id]);
			player[id].go.transform.position = new Vector3(-2000f, -2000f, -2000f);
			player[id].tr = player[id].go.transform;
			player[id].asFS = player[id].go.AddComponent<AudioSource>();
			player[id].asFS.spatialBlend = ((id != Controll.pl.idx) ? 1 : 0);
			player[id].asW = player[id].go.AddComponent<AudioSource>();
			player[id].asW.spatialBlend = ((id != Controll.pl.idx) ? 1 : 0);
			Controll.SetVolume(player[id]);
			if (GUIOptions.motion > 0)
			{
				player[id].go.AddComponent<AmplifyMotionObject>();
			}
		}
	}

	public static void Delete(int id)
	{
		for (int i = 0; i < 40; i++)
		{
			if (player[i] != null)
			{
				player[i].fragcount[id] = 0;
			}
		}
		if (player[id] == null)
		{
			return;
		}
		if (player[id].go != null)
		{
			if (Controll.specid == id)
			{
				Controll.specid = -1;
				Controll.trCamera.parent = null;
			}
			Object.Destroy(player[id].go);
		}
		for (int j = 0; j < player[id].mat.Length; j++)
		{
			Object.Destroy(player[id].mat[j]);
		}
		player[id] = null;
	}

	public static void UpdateTeam(int id, int team)
	{
		if (player[id] != null)
		{
			PlayerData obj = player[id];
			obj.team = team;
			VCGen.ReBuildSkin(obj);
			VCGen.UpdateTexturesNormal(obj);
		}
	}

	public static void Spawn(int id, int x, int y, int z, float ry, int weaponset)
	{
		if (player[id] == null)
		{
			return;
		}
		PlayerData playerData = player[id];
		if (weaponset != playerData.currset)
		{
			ClearWeapon(playerData);
		}
		playerData.currset = weaponset;
		BuildWeaponSet(playerData);
		for (int i = 1000; i < 1008; i++)
		{
			BuildWeaponExtra(playerData, i, 0);
		}
		playerData.currweapon = null;
		if (Controll.pl.idx == id)
		{
			Controll.forceweapon = false;
			HideWeapon(playerData);
			Controll.SetKillMode(false);
			Controll.SetPos(x, y, z);
			Controll.rx = ry;
			HUD.SetHealth(100);
			Controll.pl.bstate = 0;
			player[id].go.transform.position = new Vector3(-2000f, -2000f, -2000f);
			int num = -1;
			WeaponSet weaponSet = Controll.pl.wset[Controll.pl.currset];
			for (int j = 0; j < 5; j++)
			{
				if (weaponSet.w[j] == null)
				{
					continue;
				}
				if (!DemoRec.isDemo())
				{
					int id2 = weaponSet.w[j].wi.id;
					WeaponInfo weaponInfo = GUIInv.winfo[id2];
					weaponSet.w[j].ammo = weaponInfo.ammo;
					weaponSet.w[j].backpack = weaponInfo.backpack;
					if (j == 3)
					{
						weaponSet.w[j].backpack = g_blocklimit;
					}
				}
				if (num < 0)
				{
					num = j;
				}
			}
			if (Controll.pl.ew != null)
			{
				for (int k = 0; k < 8; k++)
				{
					if (Controll.pl.ew[k] != null)
					{
						Controll.pl.ew[k].ammo = Controll.pl.ew[k].wi.ammo;
						Controll.pl.ew[k].backpack = Controll.pl.ew[k].wi.backpack;
					}
				}
			}
			if (num >= 0)
			{
				Controll.ChangeWeapon(num);
			}
			HUD.ResetStreak();
			for (int l = 0; l < 40; l++)
			{
				if (player[l] != null && player[l] != Controll.pl)
				{
					if (Controll.gamemode == 2 || Controll.gamemode == 3 || Controll.gamemode == 5 || Controll.gamemode == 8)
					{
						player[l].goArrow.SetActive(false);
					}
					else if (player[l].team == Controll.pl.team)
					{
						player[l].goArrow.SetActive(true);
					}
					else
					{
						player[l].goArrow.SetActive(false);
					}
				}
			}
			HUD.SetFade(Color.black, 0.5f);
			Controll.UnZoom();
			if (g_freezetime != 0f)
			{
				Controll.SetFreeze(g_freezetime);
			}
			StaticBatchingUtility.Combine(Map.goMap);
			GC.Force();
			HUDKiller.SetActive(false);
			if (weaponSet != null && weaponSet.w[4] != null && weaponSet.w[4].wi.id == 110)
			{
				HUD.show_armor = true;
			}
			Map.goMap.AddComponent<CLC>();
		}
		else
		{
			player[id].Pos = new Vector3(x, (float)y + pyofs, z);
			player[id].prevPos = player[id].Pos;
			player[id].currPos = player[id].Pos;
			player[id].go.transform.position = player[id].Pos;
			player[id].health = 100;
			if (Controll.gamemode == 2 || Controll.gamemode == 3 || Controll.gamemode == 5)
			{
				player[id].goArrow.SetActive(false);
			}
			else
			{
				player[id].goArrow.SetActive(false);
			}
		}
		PlayerData playerData2 = player[id];
		if (playerData2.cjHead != null)
		{
			Object.Destroy(playerData2.cjHead);
		}
		if (playerData2.cjLArm != null)
		{
			Object.Destroy(playerData2.cjLArm);
		}
		if (playerData2.cjRArm != null)
		{
			Object.Destroy(playerData2.cjRArm);
		}
		if (playerData2.cjLLeg != null)
		{
			Object.Destroy(playerData2.cjLLeg);
		}
		if (playerData2.cjRLeg != null)
		{
			Object.Destroy(playerData2.cjRLeg);
		}
		if (playerData2.rbLArm[1] == null)
		{
			playerData2.rbLArm[1] = playerData2.goLArm[1].AddComponent<Rigidbody>();
		}
		if (playerData2.rbRArm[1] == null)
		{
			playerData2.rbRArm[1] = playerData2.goRArm[1].AddComponent<Rigidbody>();
		}
		if (playerData2.rbLLeg[1] == null)
		{
			playerData2.rbLLeg[1] = playerData2.goLLeg[1].AddComponent<Rigidbody>();
		}
		if (playerData2.rbLLeg[2] == null)
		{
			playerData2.rbLLeg[2] = playerData2.goLLeg[2].AddComponent<Rigidbody>();
		}
		if (playerData2.rbRLeg[1] == null)
		{
			playerData2.rbRLeg[1] = playerData2.goRLeg[1].AddComponent<Rigidbody>();
		}
		if (playerData2.rbRLeg[2] == null)
		{
			playerData2.rbRLeg[2] = playerData2.goRLeg[2].AddComponent<Rigidbody>();
		}
		playerData2.rbBody.isKinematic = true;
		playerData2.rbHead.isKinematic = true;
		playerData2.rbLArm[0].isKinematic = true;
		playerData2.rbLArm[1].isKinematic = true;
		playerData2.rbRArm[0].isKinematic = true;
		playerData2.rbRArm[1].isKinematic = true;
		playerData2.rbLLeg[0].isKinematic = true;
		playerData2.rbLLeg[1].isKinematic = true;
		playerData2.rbLLeg[2].isKinematic = true;
		playerData2.rbRLeg[0].isKinematic = true;
		playerData2.rbRLeg[1].isKinematic = true;
		playerData2.rbRLeg[2].isKinematic = true;
		SetLayer(playerData2, 0);
		if (playerData2.team == 0)
		{
			SetColor(id, Color.red);
		}
		else if (playerData2.team == 1)
		{
			SetColor(id, Color.blue);
		}
		playerData2.spawntime = Time.time;
		playerData2.spawnprotect = true;
		CharAnimator.RestoreBody(playerData2);
		playerData2.bstate = 0;
		playerData2.prevbstate = 5;
		SetNormal(playerData2.idx);
	}

	public static void RefillWeapons()
	{
		WeaponSet weaponSet = Controll.pl.wset[Controll.pl.currset];
		for (int i = 0; i < 5; i++)
		{
			if (weaponSet != null && weaponSet.w[i] != null)
			{
				int id = weaponSet.w[i].wi.id;
				WeaponInfo weaponInfo = GUIInv.winfo[id];
				weaponSet.w[i].ammo = weaponInfo.ammo;
				weaponSet.w[i].backpack = weaponInfo.backpack;
				if (i == 3)
				{
					weaponSet.w[i].backpack = g_blocklimit;
				}
			}
		}
		if (Controll.pl.ew == null)
		{
			return;
		}
		for (int j = 0; j < 8; j++)
		{
			if (Controll.pl.ew[j] != null)
			{
				Controll.pl.ew[j].ammo = Controll.pl.ew[j].wi.ammo;
				Controll.pl.ew[j].backpack = Controll.pl.ew[j].wi.backpack;
			}
		}
	}

	public static void SetNormal(int id)
	{
		if (player[id] != null)
		{
			PlayerData obj = player[id];
			VCGen.UpdateTexturesNormal(obj);
			obj.skinstate = 0;
		}
	}

	public static void SetZombie(int id)
	{
		if (player[id] != null)
		{
			PlayerData playerData = player[id];
			VCGen.BuildZombieSkin(playerData);
			VCGen.UpdateTexturesZombie(playerData);
			playerData.skinstate = 1;
			if (sInfect == null)
			{
				sInfect = Resources.Load("sounds/infect") as AudioClip;
			}
			playerData.asW.PlayOneShot(sInfect);
			for (int i = 0; i < 10; i++)
			{
				Vector3 vector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
				Controll.CreateBlood(playerData.currPos + vector, 1, 1);
			}
		}
	}

	public static void SetDamaged(int id)
	{
		if (player[id] != null)
		{
			PlayerData obj = player[id];
			VCGen.BuildZombieSkin(obj);
			VCGen.UpdateTexturesDamaged(obj);
			obj.skinstate = 2;
		}
	}

	public static void SetColor(int id, Color c)
	{
		if (player[id] == null)
		{
			return;
		}
		PlayerData playerData = player[id];
		int i = 0;
		for (int num = playerData.mat.Length; i < num; i++)
		{
			if (!(playerData.mat[i] == null))
			{
				playerData.mat[i].SetColor("_Color", c);
			}
		}
	}

	public static void Pos(int id, float x, float y, float z, float rx, float ry, int bitmask, int ipx, int ipy, int ipz, int irx, int iry)
	{
		if (player[id] == null)
		{
			return;
		}
		PlayerData playerData = player[id];
		playerData.prevPos = player[id].Pos;
		playerData.Pos.x = x;
		playerData.Pos.y = y + pyofs;
		playerData.Pos.z = z;
		playerData.prevRot[0] = playerData.Rot[0];
		playerData.prevRot[1] = playerData.Rot[1];
		playerData.Rot[0] = rx;
		playerData.Rot[1] = ry;
		float num = playerData.Rot[1] - playerData.prevRot[1];
		if (num > 180f)
		{
			playerData.prevRot[1] += 360f;
		}
		if (num < -180f)
		{
			playerData.prevRot[1] -= 360f;
		}
		playerData.nextFrame = g_realtimeSinceStartup + cl_fps;
		bool num2 = (bitmask & 2) != 0;
		bool flag = (bitmask & 1) != 0;
		bool flag2 = (bitmask & 8) != 0;
		bool flag3 = (bitmask & 4) != 0;
		if (!num2 && !flag && !flag2 && !flag3)
		{
			playerData.bstate = 0;
		}
		else
		{
			playerData.bstate = 1;
		}
		if (((uint)bitmask & 0x20u) != 0)
		{
			if (playerData.bstate == 0)
			{
				playerData.bstate = 2;
			}
			else if (playerData.bstate == 1)
			{
				playerData.bstate = 3;
			}
		}
		if (((uint)bitmask & 0x10u) != 0)
		{
			playerData.bstate = 4;
		}
		if (playerData.spawnprotect && g_time > playerData.spawntime + g_protecttime)
		{
			playerData.spawnprotect = false;
			SetColor(id, Color.white);
		}
		playerData.updatepos = true;
		if (playerData.ipx == ipx && playerData.ipy == ipy && playerData.ipz == ipz)
		{
			playerData.updatepos = false;
		}
		playerData.updaterotx = true;
		if (playerData.irx == irx)
		{
			playerData.updaterotx = false;
		}
		playerData.updateroty = true;
		if (playerData.iry == iry)
		{
			playerData.updateroty = false;
		}
		if (playerData.bstate != playerData.prevbstate)
		{
			playerData.updaterotx = true;
		}
		playerData.ipx = ipx;
		playerData.ipy = ipy;
		playerData.ipz = ipz;
		playerData.irx = irx;
		playerData.iry = iry;
		playerData.prevbstate = playerData.bstate;
		if (Vector3.Distance(playerData.prevPos, playerData.Pos) > 8f)
		{
			playerData.prevPos = playerData.Pos;
		}
		if (!playerData.visible && playerData.updatepos && playerData != Controll.pl && testcull)
		{
			playerData.go.transform.position = playerData.prevPos;
		}
	}

	public static void Attack(int id)
	{
		if (player[id] == null || Controll.pl.idx == id)
		{
			return;
		}
		PlayerData playerData = player[id];
		if (playerData.currweapon == null)
		{
			return;
		}
		if (playerData.currweapon.weaponname == "flamethrower")
		{
			Audio_Fire_Flame(playerData, 1);
			if (playerData.currweapon.goPMuzzle != null)
			{
				GOP.Create(22, playerData.currweapon.goPMuzzle.transform.position, new Vector3(playerData.Rot[0], playerData.Rot[1], 0f), Vector3.zero, Color.black);
			}
			return;
		}
		Audio_Fire(playerData);
		bool flag = true;
		WeaponInfo weaponInfo = GetWeaponInfo(playerData, playerData.currweapon.weaponname);
		if (weaponInfo != null && (weaponInfo.slot == 2 || weaponInfo.name == "icegun"))
		{
			flag = false;
		}
		if (flag)
		{
			Vector3 vector = playerData.goBody.transform.position + new Vector3(Random.Range(0f, 0.01f), Random.Range(0f, 0.01f), Random.Range(0f, 0.01f));
			GOP.Create(8, playerData.goBody.transform.position, new Vector3(playerData.Rot[0], playerData.Rot[1], 0f), Vector3.zero, Color.black);
		}
		if (playerData.currweapon.goPMuzzle != null && weaponInfo.slot != 2)
		{
			playerData.currweapon.goPMuzzle.transform.localEulerAngles = new Vector3(0f, 90f, Random.Range(0, 360));
			float num = Random.Range(30, 27);
			playerData.currweapon.goPMuzzle.transform.localScale = new Vector3(num, num, 0f);
			playerData.currweapon.goPMuzzle.SetActive(true);
			playerData.tM = Time.time + 0.03f;
		}
		if (DemoRec.isDemo())
		{
			DemoRec.stats_Attack(id, playerData.name, (weaponInfo == null) ? (-1) : weaponInfo.slot);
		}
	}

	public static void AttackDamage(int id, int vid, int vbox)
	{
		if (player[id] == null || player[vid] == null || Controll.pl.idx == id)
		{
			return;
		}
		PlayerData playerData = player[id];
		if (Controll.pl.idx == vid)
		{
			HUDIndicator.Set(playerData.Pos);
			if (Controll.pl.skinstate == 1)
			{
				Controll.zombieslowdown = Time.time + 0.5f;
				Controll.inSlow = true;
			}
		}
		int health = 100;
		if (vbox >= 128)
		{
			vbox -= 128;
		}
		else if (vbox >= 64)
		{
			vbox -= 64;
			health = 70;
		}
		else if (vbox >= 32)
		{
			vbox -= 32;
			health = 30;
		}
		else
		{
			health = 10;
		}
		PlayerData playerData2 = player[vid];
		playerData2.health = health;
		switch (vbox)
		{
		case 0:
			Controll.CreateBlood(playerData2.go.transform.position + new Vector3(0f, 0.5f, 0f), vbox, (playerData2.skinstate == 1) ? 1 : 0);
			break;
		case 1:
			Controll.CreateBlood(playerData2.goHead.transform.position + new Vector3(0f, 0.4f, 0f), vbox, (playerData2.skinstate == 1) ? 1 : 0);
			break;
		}
		if (playerData2.skinstate == 1)
		{
			if (sZombieHit == null)
			{
				sZombieHit = Resources.Load("sounds/zombiehit") as AudioClip;
			}
			playerData2.asW.PlayOneShot(sZombieHit);
		}
		if (DemoRec.isDemo())
		{
			WeaponInfo weaponInfo = null;
			if (playerData.currweapon != null)
			{
				weaponInfo = GetWeaponInfo(playerData, playerData.currweapon.weaponname);
			}
			DemoRec.stats_Damage(playerData.name, vbox, (weaponInfo == null) ? (-1) : weaponInfo.slot);
		}
	}

	public static void AttackDamageRepeat(int id, int vid, int vbox, int bloodid, int soundid, int affectid)
	{
		if (player[id] == null || player[vid] == null)
		{
			return;
		}
		int health = 100;
		if (vbox >= 128)
		{
			vbox -= 128;
		}
		else if (vbox >= 64)
		{
			vbox -= 64;
			health = 70;
		}
		else if (vbox >= 32)
		{
			vbox -= 32;
			health = 30;
		}
		else
		{
			health = 10;
		}
		PlayerData playerData = player[vid];
		playerData.health = health;
		if (bloodid == 2)
		{
			Controll.CreateBlood(playerData.go.transform.position - Vector3.up * 0.5f, vbox, bloodid);
		}
		else
		{
			switch (vbox)
			{
			case 0:
				Controll.CreateBlood(playerData.go.transform.position + new Vector3(0f, 0.5f, 0f), vbox, bloodid);
				break;
			case 1:
				Controll.CreateBlood(playerData.goHead.transform.position + new Vector3(0f, 0.4f, 0f), vbox, bloodid);
				break;
			}
		}
		if (soundid == 1)
		{
			if (sTaserHit == null)
			{
				sTaserHit = Resources.Load("sounds/s_taser_active") as AudioClip;
			}
			playerData.asW.PlayOneShot(sTaserHit);
		}
		if (Controll.pl.idx == vid && affectid == 1)
		{
			Controll.zombieslowdown = Time.time + 0.5f;
			Controll.inSlow = true;
		}
	}

	public static void Kill(int id, int aid, int hitbox)
	{
		if (player[id] == null)
		{
			return;
		}
		PlayerData playerData = player[id];
		playerData.bstate = 5;
		playerData.prevbstate = 5;
		if (Controll.pl.idx == id)
		{
			Controll.pl.go.transform.position = Controll.Pos + new Vector3(0f, pyofs, 0f);
			Controll.pl.go.transform.eulerAngles = Controll.trControll.eulerAngles;
		}
		else
		{
			playerData.health = 0;
		}
		if (playerData.currweapon != null && playerData.currweapon.goPMuzzle != null)
		{
			playerData.tM = 0f;
			playerData.currweapon.goPMuzzle.SetActive(false);
		}
		playerData.rbBody.isKinematic = false;
		playerData.rbHead.isKinematic = false;
		playerData.cjHead = playerData.goHead.AddComponent<CharacterJoint>();
		playerData.cjHead.connectedBody = playerData.rbBody;
		playerData.cjHead.anchor = Vector3.zero;
		playerData.cjHead.connectedAnchor = new Vector3(0f, 14f, 0f);
		playerData.rbLArm[0].isKinematic = false;
		playerData.cjLArm = playerData.goLArm[0].AddComponent<CharacterJoint>();
		playerData.cjLArm.connectedBody = playerData.rbBody;
		playerData.cjLArm.anchor = Vector3.zero;
		playerData.cjLArm.connectedAnchor = new Vector3(-8f, 19f, 0f);
		playerData.rbRArm[0].isKinematic = false;
		playerData.cjRArm = playerData.goRArm[0].AddComponent<CharacterJoint>();
		playerData.cjRArm.connectedBody = playerData.rbBody;
		playerData.cjRArm.anchor = Vector3.zero;
		playerData.cjRArm.connectedAnchor = new Vector3(8f, 19f, 0f);
		playerData.rbLLeg[0].isKinematic = false;
		playerData.cjLLeg = playerData.goLLeg[0].AddComponent<CharacterJoint>();
		playerData.cjLLeg.connectedBody = playerData.rbBody;
		playerData.rbRLeg[0].isKinematic = false;
		playerData.cjRLeg = playerData.goRLeg[0].AddComponent<CharacterJoint>();
		playerData.cjRLeg.connectedBody = playerData.rbBody;
		Object.Destroy(playerData.rbLLeg[1]);
		Object.Destroy(playerData.rbLLeg[2]);
		Object.Destroy(playerData.rbRLeg[1]);
		Object.Destroy(playerData.rbRLeg[2]);
		Object.Destroy(playerData.rbLArm[1]);
		Object.Destroy(playerData.rbRArm[1]);
		SetLayer(playerData, 9);
		Vector3 vector = Vector3.zero;
		if (player[aid] != null)
		{
			vector = player[aid].Pos;
		}
		if (aid == Controll.pl.idx)
		{
			vector = Controll.Pos;
		}
		Vector3 vector2 = playerData.go.transform.position - vector;
		if (hitbox == 1)
		{
			playerData.rbHead.AddForce(vector2.normalized * 1000f);
			int num = Random.Range(0, 2);
			if (num == 0)
			{
				num = -1;
			}
			playerData.rbBody.maxAngularVelocity = 500f;
			playerData.rbBody.AddRelativeTorque(playerData.tr.up * 500f * num);
		}
		else
		{
			playerData.rbBody.AddForce(vector2.normalized * 1500f);
			int num2 = Random.Range(0, 2);
			if (num2 == 0)
			{
				num2 = -1;
			}
			int num3 = Random.Range(0, 3) - 1;
			playerData.rbBody.maxAngularVelocity = 500f;
			playerData.rbBody.AddRelativeTorque(playerData.tr.up * 250f * num2);
			playerData.rbBody.AddRelativeTorque(playerData.tr.forward * 50f * num3);
		}
		if (Controll.pl.idx == id)
		{
			Controll.UnZoom();
			Controll.SetKillMode(true);
			HUD.SetHealth(0);
			Controll.inReload = false;
		}
		if (playerData.currweapon != null && playerData.currweapon.goPW != null && playerData.visible)
		{
			if (playerData.skinstate == 1)
			{
				playerData.currweapon.goPW.SetActive(false);
				return;
			}
			GameObject obj = Object.Instantiate(playerData.currweapon.goPW);
			obj.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
			Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
			rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
			Vector3 localPosition = playerData.go.transform.localPosition;
			obj.transform.localPosition = localPosition + playerData.go.transform.forward * 1.5f;
			obj.AddComponent<BoxCollider>();
			rigidbody.AddForce(vector2.normalized * -500f);
			obj.layer = 9;
			obj.AddComponent<DelayDestroy>();
			playerData.currweapon.goPW.SetActive(false);
		}
	}

	public static void Audio_fs(PlayerData p, float speed, bool inAir, bool freeFly)
	{
		if (p.asFS == null)
		{
			return;
		}
		if (p == Controll.pl)
		{
			if (Controll.vel.magnitude * speed < 0.2f)
			{
				return;
			}
		}
		else if (speed == 0f)
		{
			return;
		}
		if (inAir || freeFly || Time.time < p.tFS)
		{
			return;
		}
		p.tFS = Time.time + (0.95f - speed);
		bool flag = false;
		if (p == Controll.pl)
		{
			flag = true;
		}
		int groundBlock = GetGroundBlock(flag ? Controll.currPos : p.currPos, flag);
		if (groundBlock < 1 || groundBlock > 4)
		{
			switch (groundBlock)
			{
			case 9:
				break;
			case -2:
				Audio_fs_water(p, flag);
				return;
			default:
				Audio_fs_concrete(p, flag);
				return;
			}
		}
		Audio_fs_dirt(p, flag);
	}

	private static int GetGroundBlock(Vector3 pos, bool self)
	{
		if (Map.WATER_LEVEL > 0f && pos.y - (self ? 0f : pyofs) <= Map.WATER_LEVEL)
		{
			return -2;
		}
		return Map.GetBlock((int)pos.x, (int)pos.y - 1, (int)pos.z);
	}

	private static void Audio_fs_dirt(PlayerData p, bool self)
	{
		int num = Random.Range(1, 6);
		if (dirt[num] == null)
		{
			dirt[num] = ContentLoader_.LoadAudio("dirt" + num);
		}
		if (self)
		{
			p.asFS.volume = GUIOptions.gamevolume * 0.3f;
		}
		else
		{
			p.asFS.volume = GUIOptions.gamevolume * 0.5f;
		}
		p.asFS.clip = dirt[num];
		p.asFS.pitch = Random.Range(0.9f, 1.05f);
		p.asFS.Play();
	}

	private static void Audio_fs_concrete(PlayerData p, bool self)
	{
		int num = Random.Range(1, 5);
		if (concrete[num] == null)
		{
			concrete[num] = Resources.Load("sounds/concrete" + num) as AudioClip;
		}
		if (self)
		{
			p.asFS.volume = GUIOptions.gamevolume * 0.3f;
		}
		else
		{
			p.asFS.volume = GUIOptions.gamevolume * 0.5f;
		}
		p.asFS.clip = concrete[num];
		p.asFS.pitch = Random.Range(0.8f, 1f);
		p.asFS.Play();
	}

	private static void Audio_fs_water(PlayerData p, bool self)
	{
		int num = Random.Range(1, 7);
		if (swamp[num] == null)
		{
			swamp[num] = Resources.Load("sounds/swamp" + num) as AudioClip;
		}
		if (self)
		{
			p.asFS.volume = GUIOptions.gamevolume * 0.3f;
		}
		else
		{
			p.asFS.volume = GUIOptions.gamevolume * 0.5f;
		}
		p.asFS.clip = swamp[num];
		p.asFS.pitch = Random.Range(0.8f, 1f);
		p.asFS.Play();
	}

	public static void Audio_Fire(PlayerData p)
	{
		if (!(p.asW == null) && p.currweapon != null)
		{
			p.asW.clip = p.currweapon.snd;
			p.asW.pitch = Random.Range(0.9f, 1.05f);
			p.asW.Play();
		}
	}

	public static void Audio_Fire_Flame(PlayerData p, int state)
	{
		if (p.asW == null || p.currweapon == null)
		{
			return;
		}
		if (flame[state] == null)
		{
			switch (state)
			{
			case 0:
				flame[state] = ContentLoader_.LoadAudio("s_flamethrower_start");
				break;
			case 1:
				flame[state] = ContentLoader_.LoadAudio("s_flamethrower_work");
				break;
			case 2:
				flame[state] = ContentLoader_.LoadAudio("s_flamethrower_end");
				break;
			}
		}
		if (state == 2)
		{
			p.asW.Stop();
		}
		p.asW.clip = flame[state];
		p.asW.pitch = Random.Range(0.95f, 1.05f);
		p.asW.Play();
	}

	public static void AddWeapon(PlayerData p, WeaponData wd)
	{
		p.weapon.Add(wd);
	}

	public static void HideWeapon(PlayerData p)
	{
		for (int i = 0; i < p.weapon.Count; i++)
		{
			if (p.weapon[i] != null)
			{
				if (p.weapon[i].go != null)
				{
					p.weapon[i].go.SetActive(false);
				}
				if (p.weapon[i].goMuzzle != null)
				{
					p.weapon[i].goMuzzle.SetActive(false);
				}
			}
		}
	}

	public static void ClearWeapon(PlayerData p)
	{
		for (int i = 0; i < p.weapon.Count; i++)
		{
			if (p.weapon[i] != null)
			{
				if (p.weapon[i].go != null)
				{
					p.weapon[i].go.transform.SetParent(null);
					Object.Destroy(p.weapon[i].go);
				}
				if (p.weapon[i].goPW != null)
				{
					p.weapon[i].goPW.transform.SetParent(null);
					Object.Destroy(p.weapon[i].goPW);
				}
				p.weapon[i] = null;
			}
		}
		p.weapon.Clear();
		p.ew = null;
	}

	public static WeaponData GetWeapon(PlayerData p, string weaponname)
	{
		for (int i = 0; i < p.weapon.Count; i++)
		{
			if (p.weapon[i] != null && p.weapon[i].weaponname == weaponname)
			{
				return p.weapon[i];
			}
		}
		return null;
	}

	public static WeaponInfo GetWeaponInfo(PlayerData p, string weaponname)
	{
		for (int i = 0; i < 5; i++)
		{
			if (p.wset[p.currset] != null && p.wset[p.currset].w[i] != null)
			{
				WeaponInfo wi = p.wset[p.currset].w[i].wi;
				if (wi != null && wi.name == weaponname)
				{
					return wi;
				}
			}
		}
		if (p.ew == null)
		{
			return null;
		}
		for (int j = 0; j < 8; j++)
		{
			if (p.ew[j] != null && p.ew[j].wi.name == weaponname)
			{
				return p.ew[j].wi;
			}
		}
		return null;
	}

	public static WeaponInv GetWeaponInv(PlayerData p, string weaponname)
	{
		for (int i = 0; i < 5; i++)
		{
			if (p.wset[p.currset] != null && p.wset[p.currset].w[i] != null && p.wset[p.currset].w[i].wi.name == weaponname)
			{
				return p.wset[p.currset].w[i];
			}
		}
		if (p.ew == null)
		{
			return null;
		}
		for (int j = 0; j < 8; j++)
		{
			if (p.ew[j] != null && p.ew[j].wi.name == weaponname)
			{
				return p.ew[j];
			}
		}
		return null;
	}

	public static void ForceSelectWeapon(int id, int wid)
	{
		PlayerData playerData = player[id];
		if (playerData == null)
		{
			return;
		}
		WeaponInfo weaponInfo = GUIInv.winfo[wid];
		if (weaponInfo != null)
		{
			SelectWeapon(playerData, weaponInfo.name, true);
			if (Controll.pl == playerData)
			{
				Controll.UnZoom();
				Controll.forceunzoom = 0f;
				Controll.forceweapon = true;
			}
		}
	}

	public static void SelectWeapon(PlayerData p, string weaponname, bool force = false)
	{
		if (!force && ((p.currweapon != null && p.currweapon.weaponname == weaponname) || Time.time < p.drawtime + 0.1f))
		{
			return;
		}
		if (p == Controll.pl)
		{
			HideWeapon(p);
		}
		ResetMuzzleFlash(p);
		p.currweapon = null;
		WeaponData weapon = GetWeapon(p, weaponname);
		if (weapon != null && GetWeaponInv(p, weaponname) != null)
		{
			p.currweapon = weapon;
			if (p == Controll.pl)
			{
				SelectWeaponLocal(p, weaponname);
			}
			UpdateWeaponSpine(p);
		}
	}

	public static void SelectWeaponLocal(PlayerData p, string weaponname)
	{
		WeaponInv weaponInv = GetWeaponInv(p, weaponname);
		if (weaponInv == null)
		{
			return;
		}
		WeaponData weapon = GetWeapon(p, weaponname);
		if (weapon == null)
		{
			return;
		}
		if (weapon.go != null)
		{
			weapon.go.SetActive(true);
		}
		if (weapon.ani != null && weapon.ani != null && weapon.wield != null)
		{
			weapon.ani.Stop();
			weapon.ani.clip = weapon.wield;
			weapon.ani.Play();
		}
		WeaponInfo weaponInfo = GUIInv.GetWeaponInfo(weaponname);
		Controll.inReload = false;
		Controll.reload_result = 0f;
		int num = 100;
		if (!DemoRec.isDemo())
		{
			Client.cs.send_weaponselect(weaponInfo.slot);
			HUD.SetAmmo(weaponInv.ammo, weaponInfo.ammo);
			HUD.SetBackPack(weaponInv.backpack);
			if (weaponInfo.firerate > num)
			{
				num = weaponInfo.firerate;
			}
		}
		Controll.pl.drawtime = Time.time;
		HUD.fx = true;
		VWIK.ClearOffset();
		VWIK.AddOffset(new Vector3(0.2f, -0.1f, 0f), Time.time, (float)num / 1000f);
		VWIK.AddAngle(new Vector3(0f, -60f, 0f), Time.time, (float)num / 1000f);
		Controll.iattacktime = (uint)((int)Controll.GetClientTime() + num + 50);
		Controll.HideCursor();
		if (sDraw == null)
		{
			sDraw = ContentLoader_.LoadAudio("draw");
		}
		Controll.pl.asFX.PlayOneShot(sDraw);
		if (GUIOptions.motion > 0 && p.currweapon.go.GetComponent<AmplifyMotionObject>() == null)
		{
			p.currweapon.go.AddComponent<AmplifyMotionObject>();
		}
		if (weaponname == "panzerfaust")
		{
			GameObject gameObject = GameObject.Find(p.currweapon.go.name + "/" + p.currweapon.goWeapon.name + "/drop/Model");
			GameObject gameObject2 = GameObject.Find(p.currweapon.go.name + "/" + p.currweapon.goWeapon.name + "/model/Model");
			if (gameObject != null && gameObject2 != null)
			{
				gameObject.GetComponent<MeshRenderer>().enabled = false;
				gameObject2.GetComponent<MeshRenderer>().enabled = true;
			}
		}
	}

	public static void UpdateWeaponSpine(PlayerData p, bool local = false)
	{
		if (p.currweapon == null || p.currweapon.goBoneRHand == null)
		{
			return;
		}
		if (p.currweapon.goPW != null)
		{
			p.currweapon.goPW.SetActive(true);
			p.currweapon.goPW.transform.parent = p.goArmHelp.transform;
		}
		if (p.currweapon.goPMuzzle != null && p.currweapon.goPMuzzle.activeSelf)
		{
			p.currweapon.goPMuzzle.SetActive(false);
		}
		WeaponInfo weaponInfo = ((!local) ? GetWeaponInfo(p, p.currweapon.weaponname) : GUIInv.GetWeaponInfo(p.currweapon.weaponname));
		if (weaponInfo.slot == 0 || weaponInfo.slot == 1)
		{
			if (p.currweapon.pWeapon != null)
			{
				p.currweapon.goPW.transform.localPosition = p.currweapon.pWeapon.p;
				p.currweapon.goPW.transform.localEulerAngles = p.currweapon.pWeapon.r;
				p.currweapon.goPW.transform.localScale = p.currweapon.pWeapon.s;
			}
			else
			{
				Vector3 localPosition = p.currweapon.goBoneRHand.transform.localPosition;
				p.currweapon.goPW.transform.localPosition = localPosition + new Vector3(8f, -6f, 13f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
			}
		}
		else if (weaponInfo.slot == 2)
		{
			if (weaponInfo.id == 68 || weaponInfo.id == 171)
			{
				p.currweapon.goPW.transform.localPosition = new Vector3(2.8f, -6.8f, 10.4f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(342f, 164f, 94f);
			}
			else if (weaponInfo.id == 104)
			{
				p.currweapon.goPW.transform.localPosition = new Vector3(0.4f, -10.6f, 5.1f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(287f, 135f, 108f);
			}
			else if (weaponInfo.id == 105)
			{
				p.currweapon.goPW.transform.localPosition = new Vector3(6.3f, -14f, 3.4f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(358f, 78f, 87f);
			}
			else if (weaponInfo.id == 106)
			{
				p.currweapon.goPW.transform.localPosition = new Vector3(5.1f, -22.6f, 13.1f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(1.63f, 142f, 73f);
			}
			else if (weaponInfo.id == 124)
			{
				p.currweapon.goPW.transform.localPosition = new Vector3(6.45f, -17.5f, 13.4f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(16.8f, 229f, 64.1f);
			}
			else if (weaponInfo.id == 125)
			{
				p.currweapon.goPW.transform.localPosition = new Vector3(8.05f, -8.33f, 4.28f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(284.2f, 98.8f, 117.5f);
			}
			else if (weaponInfo.id == 123)
			{
				p.currweapon.goPW.transform.localPosition = new Vector3(5.1f, -22.6f, 13.1f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(1.63f, 142f, 73f);
			}
			else if (weaponInfo.id == 126)
			{
				p.currweapon.goPW.transform.localPosition = new Vector3(10.58f, -14.46f, 4.63f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(354.92f, 51.35f, 96.6f);
			}
			else if (weaponInfo.id == 127)
			{
				p.currweapon.goPW.transform.localPosition = new Vector3(8.66f, -14.83f, 7.08f);
				p.currweapon.goPW.transform.localEulerAngles = new Vector3(354.92f, 51.35f, 96.6f);
			}
			else if (p.currweapon.pWeapon != null)
			{
				p.currweapon.goPW.transform.parent = p.goRArm[1].transform;
				p.currweapon.goPW.transform.localPosition = p.currweapon.pWeapon.p;
				p.currweapon.goPW.transform.localEulerAngles = p.currweapon.pWeapon.r;
				p.currweapon.goPW.transform.localScale = p.currweapon.pWeapon.s;
			}
		}
		else if (weaponInfo.slot == 3)
		{
			p.currweapon.goPW.transform.localPosition = new Vector3(8f, -16f, 8f);
			p.currweapon.goPW.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
			p.currweapon.goPW.transform.localScale = new Vector3(16f, 16f, 16f);
		}
		else
		{
			int slot = weaponInfo.slot;
			int num = 4;
		}
		for (int i = 0; i < p.weapon.Count; i++)
		{
			if (p.weapon[i] == null || p.weapon[i] == p.currweapon)
			{
				continue;
			}
			if (p.weapon[i].goPMuzzle != null && p.weapon[i].goPMuzzle.activeSelf)
			{
				p.weapon[i].goPMuzzle.SetActive(false);
			}
			WeaponData weaponData = p.weapon[i];
			WeaponInfo weaponInfo2 = GetWeaponInfo(p, p.weapon[i].weaponname);
			GameObject goPW = p.weapon[i].goPW;
			if (weaponInfo2 == null)
			{
				continue;
			}
			if (weaponInfo2.slot == 0)
			{
				goPW.transform.parent = p.goBackPack.transform;
				goPW.transform.localPosition = new Vector3(1f, 12f, -6f);
				goPW.transform.localEulerAngles = new Vector3(0f, 340f, 300f);
			}
			else if (weaponInfo2.slot == 1)
			{
				if (weaponData.pBackpack != null)
				{
					goPW.transform.parent = p.goRLeg[0].transform;
					goPW.transform.localPosition = weaponData.pBackpack.p;
					goPW.transform.localEulerAngles = weaponData.pBackpack.r;
					goPW.transform.localScale = weaponData.pBackpack.s;
				}
				else
				{
					goPW.transform.parent = p.goRLeg[0].transform;
					goPW.transform.localPosition = new Vector3(5.35f, -7.5f, 0f);
					goPW.transform.localEulerAngles = new Vector3(0f, 270f, 270f);
				}
			}
			else if (weaponInfo2.slot == 2)
			{
				if (weaponInfo2.id == 68 || weaponInfo2.id == 171)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = new Vector3(-9f, 7.5f, -4f);
					goPW.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
					goPW.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				else if (weaponInfo2.id == 104)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = new Vector3(-8.45f, 20f, 1.3f);
					goPW.transform.localEulerAngles = new Vector3(0f, 90f, 270f);
					goPW.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				else if (weaponInfo2.id == 105)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = new Vector3(-8.45f, 25f, 3f);
					goPW.transform.localEulerAngles = new Vector3(0f, 90f, 270f);
					goPW.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				else if (weaponInfo2.id == 106)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = new Vector3(-8.3f, 3f, 1.35f);
					goPW.transform.localEulerAngles = new Vector3(0f, 270f, 90f);
					goPW.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				else if (weaponInfo2.id == 123)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = new Vector3(-8.3f, 3f, 1.35f);
					goPW.transform.localEulerAngles = new Vector3(0f, 270f, 90f);
					goPW.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				else if (weaponInfo2.id == 124)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = new Vector3(-8.38f, 17.37f, -6.99f);
					goPW.transform.localEulerAngles = new Vector3(0f, 270f, 270f);
					goPW.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				else if (weaponInfo2.id == 125)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = new Vector3(-8.16f, 24.78f, -0.42f);
					goPW.transform.localEulerAngles = new Vector3(0f, 90f, 270f);
					goPW.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				else if (weaponInfo2.id == 126)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = new Vector3(-8.16f, 19.26f, -7.84f);
					goPW.transform.localEulerAngles = new Vector3(0f, 270f, 270f);
					goPW.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				else if (weaponInfo2.id == 127)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = new Vector3(-11.25f, -3.86f, 0.02f);
					goPW.transform.localEulerAngles = new Vector3(2.88f, 216.1f, 89.99f);
					goPW.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				else if (weaponData.pBackpack != null)
				{
					goPW.transform.parent = p.goBackPack.transform;
					goPW.transform.localPosition = weaponData.pBackpack.p;
					goPW.transform.localEulerAngles = weaponData.pBackpack.r;
					goPW.transform.localScale = weaponData.pBackpack.s;
				}
			}
			else
			{
				if (weaponInfo2.slot == 3)
				{
					goPW.SetActive(false);
					continue;
				}
				if (weaponInfo2.slot == 4)
				{
					continue;
				}
			}
			if (p.weapon[i].extra)
			{
				goPW.SetActive(false);
			}
			else
			{
				goPW.SetActive(true);
			}
		}
	}

	private static void ResetMuzzleFlash(PlayerData p)
	{
		if (p.currweapon != null && !(p.currweapon.goPMuzzle == null))
		{
			p.tM = 0f;
			p.currweapon.goPMuzzle.SetActive(false);
		}
	}

	public static void UpdateMuzzle(PlayerData p)
	{
		if (p.currweapon == null)
		{
			return;
		}
		if (p.currweapon.goMuzzle != null)
		{
			if (p.tM != 0f && Time.time > p.tM)
			{
				p.tM = 0f;
				p.currweapon.goMuzzle.SetActive(false);
			}
		}
		else if (p.currweapon.goPMuzzle != null && p.tM != 0f && Time.time > p.tM)
		{
			p.tM = 0f;
			p.currweapon.goPMuzzle.SetActive(false);
		}
	}

	public static void DropWeapon(PlayerData p)
	{
	}

	public static void BuildWeaponSet(PlayerData p)
	{
		if (p.wset[p.currset] == null)
		{
			return;
		}
		WeaponSet weaponSet = p.wset[p.currset];
		if (weaponSet == null)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (weaponSet.w[i] == null)
			{
				continue;
			}
			WeaponInfo wi = weaponSet.w[i].wi;
			WeaponData weapon = GetWeapon(p, wi.name);
			if (weapon != null)
			{
				continue;
			}
			if (Controll.pl == p)
			{
				weapon = VWGen.BuildWeaponFPS(p, wi.name, wi, weaponSet.w[i].scopeid);
				if (PlayerPrefs.HasKey("lws_" + weaponSet.w[i].wi.id))
				{
					string @string = PlayerPrefs.GetString("lws_" + weaponSet.w[i].wi.id);
					Texture2D texture2D = TEX.LoadTexture("Skins/" + weaponSet.w[i].wi.name + "_" + @string + ".bpskn");
					if (texture2D != null)
					{
						weapon.mat.SetTexture("_MainTex", texture2D);
					}
				}
				AddWeapon(p, weapon);
			}
			else
			{
				weapon = VWGen2.BuildWeaponPlayer(wi.name, weaponSet.w[i].scopeid);
				AddWeapon(p, weapon);
			}
		}
	}

	public static void BuildWeaponExtra(PlayerData p, int wid, int scopeid)
	{
		WeaponInfo weaponInfo = GUIInv.winfo[wid];
		if (weaponInfo == null)
		{
			return;
		}
		if (p.ew == null)
		{
			p.ew = new WeaponInv[8];
		}
		int num = -1;
		for (int i = 0; i < 8; i++)
		{
			if (num < 0 && p.ew[i] == null)
			{
				num = i;
			}
			if (p.ew[i] != null && p.ew[i].wi.id == wid)
			{
				return;
			}
		}
		WeaponData weapon = GetWeapon(p, weaponInfo.name);
		if (weapon != null)
		{
			return;
		}
		WeaponInv weaponInv = new WeaponInv(0uL, weaponInfo);
		weaponInv.level = 1;
		weaponInv.mod[0] = 0;
		weaponInv.mod[1] = 0;
		weaponInv.mod[2] = 0;
		weaponInv.scopeid = scopeid;
		p.ew[num] = weaponInv;
		if (Controll.pl == p)
		{
			if (p.currweapon != null && p.currweapon.goPW != null)
			{
				p.currweapon.goPW.SetActive(false);
				p.currweapon.goPW.transform.parent = p.goBackPack.transform;
			}
			weapon = VWGen.BuildWeaponFPS(p, weaponInfo.name, weaponInfo, weaponInv.scopeid);
			weapon.extra = true;
			AddWeapon(p, weapon);
		}
		else
		{
			weapon = VWGen2.BuildWeaponPlayer(weaponInfo.name, weaponInv.scopeid);
			if (weapon != null)
			{
				weapon.extra = true;
				AddWeapon(p, weapon);
			}
		}
	}

	private void Update()
	{
		if (!(Controll.csCam == null))
		{
			UpdateVisible();
			_Update();
			CharAnimator.cs._Update();
		}
	}

	private void UpdateVisible()
	{
		if (!testcull)
		{
			for (int i = 0; i < 40; i++)
			{
				if (player[i] != null)
				{
					player[i].visible = true;
				}
			}
			return;
		}
		Util.CalculateFrustumPlanes(Controll.csCam.projectionMatrix * Controll.csCam.worldToCameraMatrix);
		vcount = 0;
		pcount = 0;
		for (int j = 0; j < 40; j++)
		{
			if (player[j] == null)
			{
				continue;
			}
			pcount++;
			vis = false;
			bound.SetMinMax(player[j].currPos - minofs, player[j].currPos + maxofs);
			if (GeometryUtility.TestPlanesAABB(Controll.camplanes, bound))
			{
				vis = true;
			}
			if (vis)
			{
				if (!player[j].visible)
				{
					player[j].updatepos = true;
				}
				vcount++;
			}
			player[j].visible = vis;
		}
	}

	private void _Update()
	{
		g_realtimeSinceStartup = Time.realtimeSinceStartup;
		for (int i = 0; i < 40; i++)
		{
			if (player[i] != null && Controll.pl.idx != i)
			{
				LerpMove(player[i]);
				Controll.UpdatePlayer(player[i]);
			}
		}
	}

	public static void CacheRealTime()
	{
		g_realtimeSinceStartup = Time.realtimeSinceStartup;
		g_time = Time.time;
	}

	private static void LerpMove(PlayerData pl)
	{
		float num = pl.nextFrame - g_realtimeSinceStartup;
		float num3 = 0f;
		float num2 = (cl_fps - num) / cl_fps;
		if (smooth_move == 2)
		{
			if (num2 > 2f)
			{
				num2 = 2f;
			}
			pl.currPos.x = Lerp(pl.prevPos.x, pl.Pos.x, num2);
			pl.currPos.y = Lerp(pl.prevPos.y, pl.Pos.y, num2);
			pl.currPos.z = Lerp(pl.prevPos.z, pl.Pos.z, num2);
		}
		else if (smooth_move == 1)
		{
			pl.currPos = Vector3.Lerp(pl.currPos, pl.Pos, Time.deltaTime * 10f);
		}
		else
		{
			pl.currPos = Vector3.Lerp(pl.prevPos, pl.Pos, num2);
		}
		if (!pl.visible)
		{
			return;
		}
		if (pl.updatepos)
		{
			pl.go.transform.position = pl.currPos;
		}
		if (pl.updateroty)
		{
			if (smooth_move == 1)
			{
				pl.currRot[1] = Mathf.LerpAngle(pl.currRot[1], pl.Rot[1], Time.deltaTime * 10f);
			}
			else
			{
				pl.currRot[1] = Mathf.Lerp(pl.prevRot[1], pl.Rot[1], num2);
			}
			pl.go.transform.eulerAngles = new Vector3(0f, pl.currRot[1], 0f);
		}
	}

	public static void SetLayer(PlayerData pl, int layer)
	{
		pl.goHead.layer = layer;
		pl.goBody.layer = layer;
		pl.goLArm[0].layer = layer;
		pl.goLArm[1].layer = layer;
		pl.goRArm[0].layer = layer;
		pl.goRArm[1].layer = layer;
		pl.goLLeg[0].layer = layer;
		pl.goLLeg[1].layer = layer;
		pl.goLLeg[2].layer = layer;
		pl.goRLeg[0].layer = layer;
		pl.goRLeg[1].layer = layer;
		pl.goRLeg[2].layer = layer;
	}

	public static float Lerp(float a, float b, float t)
	{
		return t * b + (1f - t) * a;
	}
}
