using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerData
	{
		public int ac_offsetdata0;

		public int ac_offsetdata2;

		public int idx;

		public int team;

		public string name;

		public string formatname;

		public int clanid;

		public string clanname = "";

		public int health;

		public int skinstate;

		public int ac_offsetdata_00;

		public Vector3 prevPos = Vector3.zero;

		public int ac_offsetdata_01;

		public Vector3 currPos = Vector3.zero;

		public Vector3 Pos = Vector3.zero;

		public float[] prevRot = new float[2];

		public int ac_offsetdata_02;

		public float[] currRot = new float[2];

		public float[] Rot = new float[2];

		public float nextFrame;

		public AudioSource asFS;

		public float tFS;

		public AudioSource asW;

		public AudioSource asFX;

		public float tM;

		public WeaponData currweapon;

		public List<WeaponData> weapon = new List<WeaponData>();

		public WeaponSet[] wset = new WeaponSet[3];

		public int currset;

		public WeaponInv[] ew;

		public int[] rw = new int[4];

		public int ac_offsetdata1;

		public GameObject go;

		public int ac_offsetdata4;

		public int ac_offsetdata_03;

		public GameObject goHead;

		public GameObject goBody;

		public GameObject goDebug;

		public GameObject goBackPack;

		public int ac_offsetdata3;

		public int ac_offsetdata_04;

		public GameObject[] goLArm = new GameObject[2];

		public GameObject[] goRArm = new GameObject[2];

		public GameObject[] goLLeg = new GameObject[3];

		public GameObject[] goRLeg = new GameObject[3];

		public int ac_offsetdata5;

		public GameObject goArmHelp;

		public GameObject goWaterSplash;

		public GameObject goArrow;

		public GameObject goHeadHair;

		public int ac_offsetdata11;

		public int ac_offsetdata7;

		public int ac_offsetdata_05;

		public int ac_offsetdata_06;

		public Transform tr;

		public int ac_offsetdata12;

		public Rigidbody rbBody;

		public Rigidbody rbHead;

		public Rigidbody[] rbLArm = new Rigidbody[2];

		public Rigidbody[] rbRArm = new Rigidbody[2];

		public Rigidbody[] rbLLeg = new Rigidbody[3];

		public Rigidbody[] rbRLeg = new Rigidbody[3];

		public CharacterJoint cjHead;

		public CharacterJoint cjLArm;

		public CharacterJoint cjRArm;

		public CharacterJoint cjLLeg;

		public CharacterJoint cjRLeg;

		public BoxCollider bcBody;

		public BoxCollider bcHead;

		public BoxCollider[] bcLArm = new BoxCollider[2];

		public BoxCollider[] bcRArm = new BoxCollider[2];

		public BoxCollider[] bcLLeg = new BoxCollider[3];

		public BoxCollider[] bcRLeg = new BoxCollider[3];

		public int wstate;

		public int bstate;

		public int lf;

		public float la;

		public float leg_limit;

		public Material matWaterSplash;

		public int frameWaterSplash;

		public float tWaterSplash;

		public float yWaterSplash;

		public float speed;

		public float drawtime;

		public int f;

		public int d;

		public int a;

		public int exp;

		public string sFDA = "";

		public string sEXP = "";

		public string sLVL = "";

		public int pexp;

		public Material[] mat = new Material[5];

		public float spawntime;

		public bool spawnprotect;

		public int[] cp = new int[10];

		public Vector3 restoreHead;

		public Vector3 restoreLArm;

		public Vector3 restoreRArm;

		public int ipx;

		public int ipy;

		public int ipz;

		public int irx;

		public int iry;

		public int prevbstate = 5;

		public bool updatepos = true;

		public bool updaterotx = true;

		public bool updateroty = true;

		public bool visible;

		public int[] fragcount = new int[40];

		public Texture2D tAvatar;

		public Texture2D tAvatarZombie;

		public Texture2D[] skin = new Texture2D[6];

		public Texture2D[] skin_zombie;

		public Texture2D[] skin_damaged;

		public GameObject goFPSLArm;

		public GameObject goFPSRArm;

		public float tHeartBeat;

		public GameObject goAttach;

		public PlayerData()
		{
			idx = -1;
			team = 2;
			health = 0;
			nextFrame = 0f;
			weapon.Clear();
			lf = 1;
			la = 0f;
			leg_limit = 45f;
			currset = 0;
			drawtime = 0f;
			visible = false;
		}

		public WeaponInv GetWeaponInv(int set, int slot)
		{
			return wset[set].w[slot];
		}

		public WeaponInv GetWeaponInvExtra(int wid)
		{
			if (ew == null)
			{
				return null;
			}
			for (int i = 0; i < 8; i++)
			{
				if (ew[i].wi.id == wid)
				{
					return ew[i];
				}
			}
			return null;
		}
	}
}
