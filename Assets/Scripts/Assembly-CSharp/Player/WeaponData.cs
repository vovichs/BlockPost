using UnityEngine;

namespace Player
{
	public class WeaponData
	{
		public string weaponname;

		public GameObject go;

		public GameObject goWeapon;

		public GameObject goMuzzle;

		public GameObject goPW;

		public GameObject goLHand;

		public GameObject goRHand;

		public GameObject goLBone;

		public GameObject goRBone;

		public GameObject goBoneRHand;

		public GameObject goBoneLHand;

		public GameObject goBoneMuzzle;

		public GameObject goBoneScope;

		public GameObject goScope;

		public GameObject goWeaponDouble;

		public GameObject goBolt0;

		public GameObject goPMuzzle;

		public AudioClip snd;

		public Texture2D hudicon;

		public Vector2 vtc;

		public int shelltype;

		public Vector3 vPosition = Vector3.zero;

		public Vector3 vLRotation = Vector3.zero;

		public ScopeData[] scope;

		public Material mat;

		public Texture2D tex;

		public WeaponInfo wi;

		public Animation ani;

		public AnimationClip wield;

		public AnimationClip shoot;

		public AnimationClip shoot2;

		public WeaponPosHands pBase;

		public WeaponPosHands pMenu;

		public WeaponPos pBackpack;

		public WeaponPos pWeapon;

		public bool extra;
	}
}
