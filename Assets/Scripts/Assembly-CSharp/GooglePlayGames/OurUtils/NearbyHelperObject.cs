using System;
using GooglePlayGames.BasicApi.Nearby;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	public class NearbyHelperObject : MonoBehaviour
	{
		private static NearbyHelperObject instance;

		private static double mAdvertisingRemaining;

		private static double mDiscoveryRemaining;

		private static INearbyConnectionClient mClient;

		public static void CreateObject(INearbyConnectionClient client)
		{
			if (!(instance != null))
			{
				mClient = client;
				if (Application.isPlaying)
				{
					GameObject obj = new GameObject("PlayGames_NearbyHelper");
					UnityEngine.Object.DontDestroyOnLoad(obj);
					instance = obj.AddComponent<NearbyHelperObject>();
				}
				else
				{
					instance = new NearbyHelperObject();
				}
			}
		}

		private static double ToSeconds(TimeSpan? span)
		{
			if (!span.HasValue)
			{
				return 0.0;
			}
			if (span.Value.TotalSeconds < 0.0)
			{
				return 0.0;
			}
			return span.Value.TotalSeconds;
		}

		public static void StartAdvertisingTimer(TimeSpan? span)
		{
			mAdvertisingRemaining = ToSeconds(span);
		}

		public static void StartDiscoveryTimer(TimeSpan? span)
		{
			mDiscoveryRemaining = ToSeconds(span);
		}

		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public void OnDisable()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		public void Update()
		{
			if (mAdvertisingRemaining > 0.0)
			{
				mAdvertisingRemaining -= Time.deltaTime;
				if (mAdvertisingRemaining < 0.0)
				{
					mClient.StopAdvertising();
				}
			}
			if (mDiscoveryRemaining > 0.0)
			{
				mDiscoveryRemaining -= Time.deltaTime;
				if (mDiscoveryRemaining < 0.0)
				{
					mClient.StopDiscovery(mClient.GetServiceId());
				}
			}
		}
	}
}
