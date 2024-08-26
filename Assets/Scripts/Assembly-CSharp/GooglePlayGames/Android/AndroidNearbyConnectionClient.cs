using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	public class AndroidNearbyConnectionClient : INearbyConnectionClient
	{
		private class AdvertisingConnectionLifecycleCallbackProxy : AndroidJavaProxy
		{
			private Action<AdvertisingResult> mResultCallback;

			private Action<ConnectionRequest> mConnectionRequestCallback;

			private AndroidNearbyConnectionClient mClient;

			private string mLocalEndpointName;

			public AdvertisingConnectionLifecycleCallbackProxy(Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> connectionRequestCallback, AndroidNearbyConnectionClient client)
				: base("com/google/games/bridge/ConnectionLifecycleCallbackProxy$Callback")
			{
				mResultCallback = resultCallback;
				mConnectionRequestCallback = connectionRequestCallback;
				mClient = client;
			}

			public void onConnectionInitiated(string endpointId, AndroidJavaObject connectionInfo)
			{
				mLocalEndpointName = connectionInfo.Call<string>("getEndpointName", Array.Empty<object>());
				mConnectionRequestCallback(new ConnectionRequest(endpointId, mLocalEndpointName, mClient.GetServiceId(), new byte[0]));
			}

			public void onConnectionResult(string endpointId, AndroidJavaObject connectionResolution)
			{
				int num;
				using (AndroidJavaObject androidJavaObject = connectionResolution.Call<AndroidJavaObject>("getStatus", Array.Empty<object>()))
				{
					num = androidJavaObject.Call<int>("getStatusCode", Array.Empty<object>());
				}
				switch (num)
				{
				case 0:
					mResultCallback(new AdvertisingResult(ResponseStatus.Success, mLocalEndpointName));
					break;
				case 8001:
					mResultCallback(new AdvertisingResult(ResponseStatus.NotAuthorized, mLocalEndpointName));
					break;
				default:
					mResultCallback(new AdvertisingResult(ResponseStatus.InternalError, mLocalEndpointName));
					break;
				}
			}

			public void onDisconnected(string endpointId)
			{
				if (mClient.mAdvertisingMessageListener != null)
				{
					mClient.mAdvertisingMessageListener.OnRemoteEndpointDisconnected(endpointId);
				}
			}
		}

		private class PayloadCallback : AndroidJavaProxy
		{
			private IMessageListener mListener;

			public PayloadCallback(IMessageListener listener)
				: base("com/google/games/bridge/PayloadCallbackProxy$Callback")
			{
				mListener = listener;
			}

			public void onPayloadReceived(string endpointId, AndroidJavaObject payload)
			{
				if (payload.Call<int>("getType", Array.Empty<object>()) == 1)
				{
					mListener.OnMessageReceived(endpointId, payload.Call<byte[]>("asBytes", Array.Empty<object>()), true);
				}
			}
		}

		private class DiscoveringConnectionLifecycleCallback : AndroidJavaProxy
		{
			private Action<ConnectionResponse> mResponseCallback;

			private IMessageListener mListener;

			private AndroidJavaObject mClient;

			private string mLocalEndpointName;

			public DiscoveringConnectionLifecycleCallback(Action<ConnectionResponse> responseCallback, IMessageListener listener, AndroidJavaObject client)
				: base("com/google/games/bridge/ConnectionLifecycleCallbackProxy$Callback")
			{
				mResponseCallback = responseCallback;
				mListener = listener;
				mClient = client;
			}

			public void onConnectionInitiated(string endpointId, AndroidJavaObject connectionInfo)
			{
				using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.games.bridge.PayloadCallbackProxy", new PayloadCallback(mListener)))
				{
					using (mClient.Call<AndroidJavaObject>("acceptConnection", new object[2] { endpointId, androidJavaObject }))
					{
					}
				}
			}

			public void onConnectionResult(string endpointId, AndroidJavaObject connectionResolution)
			{
				int num;
				using (AndroidJavaObject androidJavaObject = connectionResolution.Call<AndroidJavaObject>("getStatus", Array.Empty<object>()))
				{
					num = androidJavaObject.Call<int>("getStatusCode", Array.Empty<object>());
				}
				switch (num)
				{
				case 0:
					mResponseCallback(ConnectionResponse.Accepted(NearbyClientId, endpointId, new byte[0]));
					break;
				case 8002:
					mResponseCallback(ConnectionResponse.AlreadyConnected(NearbyClientId, endpointId));
					break;
				default:
					mResponseCallback(ConnectionResponse.Rejected(NearbyClientId, endpointId));
					break;
				}
			}

			public void onDisconnected(string endpointId)
			{
				mListener.OnRemoteEndpointDisconnected(endpointId);
			}
		}

		private class EndpointDiscoveryCallback : AndroidJavaProxy
		{
			private IDiscoveryListener mListener;

			public EndpointDiscoveryCallback(IDiscoveryListener listener)
				: base("com/google/games/bridge/EndpointDiscoveryCallbackProxy$Callback")
			{
				mListener = listener;
			}

			public void onEndpointFound(string endpointId, AndroidJavaObject endpointInfo)
			{
				mListener.OnEndpointFound(CreateEndPointDetails(endpointId, endpointInfo));
			}

			public void onEndpointLost(string endpointId)
			{
				mListener.OnEndpointLost(endpointId);
			}

			private EndpointDetails CreateEndPointDetails(string endpointId, AndroidJavaObject endpointInfo)
			{
				return new EndpointDetails(endpointId, endpointInfo.Call<string>("getEndpointName", Array.Empty<object>()), endpointInfo.Call<string>("getServiceId", Array.Empty<object>()));
			}
		}

		private class OnGameThreadMessageListener : IMessageListener
		{
			private readonly IMessageListener mListener;

			public OnGameThreadMessageListener(IMessageListener listener)
			{
				mListener = Misc.CheckNotNull(listener);
			}

			public void OnMessageReceived(string remoteEndpointId, byte[] data, bool isReliableMessage)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnMessageReceived(remoteEndpointId, data, isReliableMessage);
				});
			}

			public void OnRemoteEndpointDisconnected(string remoteEndpointId)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnRemoteEndpointDisconnected(remoteEndpointId);
				});
			}
		}

		private class OnGameThreadDiscoveryListener : IDiscoveryListener
		{
			private readonly IDiscoveryListener mListener;

			public OnGameThreadDiscoveryListener(IDiscoveryListener listener)
			{
				mListener = listener;
			}

			public void OnEndpointFound(EndpointDetails discoveredEndpoint)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnEndpointFound(discoveredEndpoint);
				});
			}

			public void OnEndpointLost(string lostEndpointId)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnEndpointLost(lostEndpointId);
				});
			}
		}

		private volatile AndroidJavaObject mClient;

		private static readonly long NearbyClientId = 0L;

		private static readonly int ApplicationInfoFlags = 128;

		private static readonly string ServiceId = ReadServiceId();

		protected IMessageListener mAdvertisingMessageListener;

		public AndroidNearbyConnectionClient()
		{
			PlayGamesHelperObject.CreateObject();
			NearbyHelperObject.CreateObject(this);
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.nearby.Nearby"))
			{
				mClient = androidJavaClass.CallStatic<AndroidJavaObject>("getConnectionsClient", new object[1] { AndroidHelperFragment.GetActivity() });
			}
		}

		public int MaxUnreliableMessagePayloadLength()
		{
			return 1168;
		}

		public int MaxReliableMessagePayloadLength()
		{
			return 4096;
		}

		public void SendReliable(List<string> recipientEndpointIds, byte[] payload)
		{
			InternalSend(recipientEndpointIds, payload);
		}

		public void SendUnreliable(List<string> recipientEndpointIds, byte[] payload)
		{
			InternalSend(recipientEndpointIds, payload);
		}

		private void InternalSend(List<string> recipientEndpointIds, byte[] payload)
		{
			Misc.CheckNotNull(recipientEndpointIds);
			Misc.CheckNotNull(payload);
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.nearby.connection.Payload"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("fromBytes", new object[1] { payload }))
				{
					using (mClient.Call<AndroidJavaObject>("sendPayload", new object[2]
					{
						AndroidJavaConverter.ToJavaStringList(recipientEndpointIds),
						androidJavaObject
					}))
					{
					}
				}
			}
		}

		public void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> connectionRequestCallback)
		{
			Misc.CheckNotNull(resultCallback, "resultCallback");
			Misc.CheckNotNull(connectionRequestCallback, "connectionRequestCallback");
			if (advertisingDuration.HasValue && advertisingDuration.Value.Ticks < 0)
			{
				throw new InvalidOperationException("advertisingDuration must be positive");
			}
			connectionRequestCallback = ToOnGameThread(connectionRequestCallback);
			resultCallback = ToOnGameThread(resultCallback);
			AdvertisingConnectionLifecycleCallbackProxy advertisingConnectionLifecycleCallbackProxy = new AdvertisingConnectionLifecycleCallbackProxy(resultCallback, connectionRequestCallback, this);
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.games.bridge.ConnectionLifecycleCallbackProxy", advertisingConnectionLifecycleCallbackProxy))
			{
				using (AndroidJavaObject androidJavaObject2 = CreateAdvertisingOptions())
				{
					using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("startAdvertising", new object[4]
					{
						name,
						GetServiceId(),
						androidJavaObject,
						androidJavaObject2
					}))
					{
						AndroidTaskUtils.AddOnSuccessListener<AndroidJavaObject>(task, delegate
						{
							NearbyHelperObject.StartAdvertisingTimer(advertisingDuration);
						});
					}
				}
			}
		}

		private AndroidJavaObject CreateAdvertisingOptions()
		{
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaClass("com.google.android.gms.nearby.connection.Strategy").GetStatic<AndroidJavaObject>("P2P_CLUSTER"))
			{
				using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.nearby.connection.AdvertisingOptions$Builder"))
				{
					using (androidJavaObject.Call<AndroidJavaObject>("setStrategy", new object[1] { androidJavaObject2 }))
					{
						return androidJavaObject.Call<AndroidJavaObject>("build", Array.Empty<object>());
					}
				}
			}
		}

		public void StopAdvertising()
		{
			mClient.Call("stopAdvertising");
			mAdvertisingMessageListener = null;
		}

		public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener)
		{
			Misc.CheckNotNull(listener, "listener");
			OnGameThreadMessageListener listener2 = new OnGameThreadMessageListener(listener);
			DiscoveringConnectionLifecycleCallback discoveringConnectionLifecycleCallback = new DiscoveringConnectionLifecycleCallback(responseCallback, listener2, mClient);
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.games.bridge.ConnectionLifecycleCallbackProxy", discoveringConnectionLifecycleCallback))
			{
				using (mClient.Call<AndroidJavaObject>("requestConnection", new object[3] { name, remoteEndpointId, androidJavaObject }))
				{
				}
			}
		}

		public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener)
		{
			Misc.CheckNotNull(listener, "listener");
			mAdvertisingMessageListener = new OnGameThreadMessageListener(listener);
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.games.bridge.PayloadCallbackProxy", new PayloadCallback(listener)))
			{
				using (mClient.Call<AndroidJavaObject>("acceptConnection", new object[2] { remoteEndpointId, androidJavaObject }))
				{
				}
			}
		}

		public void StartDiscovery(string serviceId, TimeSpan? advertisingDuration, IDiscoveryListener listener)
		{
			Misc.CheckNotNull(serviceId, "serviceId");
			Misc.CheckNotNull(listener, "listener");
			OnGameThreadDiscoveryListener listener2 = new OnGameThreadDiscoveryListener(listener);
			if (advertisingDuration.HasValue && advertisingDuration.Value.Ticks < 0)
			{
				throw new InvalidOperationException("advertisingDuration must be positive");
			}
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.games.bridge.EndpointDiscoveryCallbackProxy", new EndpointDiscoveryCallback(listener2)))
			{
				using (AndroidJavaObject androidJavaObject2 = CreateDiscoveryOptions())
				{
					using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("startDiscovery", new object[3] { serviceId, androidJavaObject, androidJavaObject2 }))
					{
						AndroidTaskUtils.AddOnSuccessListener<AndroidJavaObject>(task, delegate
						{
							NearbyHelperObject.StartDiscoveryTimer(advertisingDuration);
						});
					}
				}
			}
		}

		private AndroidJavaObject CreateDiscoveryOptions()
		{
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaClass("com.google.android.gms.nearby.connection.Strategy").GetStatic<AndroidJavaObject>("P2P_CLUSTER"))
			{
				using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.nearby.connection.DiscoveryOptions$Builder"))
				{
					using (androidJavaObject.Call<AndroidJavaObject>("setStrategy", new object[1] { androidJavaObject2 }))
					{
						return androidJavaObject.Call<AndroidJavaObject>("build", Array.Empty<object>());
					}
				}
			}
		}

		public void StopDiscovery(string serviceId)
		{
			mClient.Call("stopDiscovery");
		}

		public void RejectConnectionRequest(string requestingEndpointId)
		{
			Misc.CheckNotNull(requestingEndpointId, "requestingEndpointId");
			using (mClient.Call<AndroidJavaObject>("rejectConnection", new object[1] { requestingEndpointId }))
			{
			}
		}

		public void DisconnectFromEndpoint(string remoteEndpointId)
		{
			mClient.Call("disconnectFromEndpoint", remoteEndpointId);
		}

		public void StopAllConnections()
		{
			mClient.Call("stopAllEndpoints");
			mAdvertisingMessageListener = null;
		}

		public string GetAppBundleId()
		{
			using (AndroidJavaObject androidJavaObject = AndroidHelperFragment.GetActivity())
			{
				return androidJavaObject.Call<string>("getPackageName", Array.Empty<object>());
			}
		}

		public string GetServiceId()
		{
			return ServiceId;
		}

		private static string ReadServiceId()
		{
			using (AndroidJavaObject androidJavaObject = AndroidHelperFragment.GetActivity())
			{
				string text = androidJavaObject.Call<string>("getPackageName", Array.Empty<object>());
				using (AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageManager", Array.Empty<object>()))
				{
					using (AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getApplicationInfo", new object[2] { text, ApplicationInfoFlags }))
					{
						using (AndroidJavaObject androidJavaObject4 = androidJavaObject3.Get<AndroidJavaObject>("metaData"))
						{
							string text2 = androidJavaObject4.Call<string>("getString", new object[1] { "com.google.android.gms.nearby.connection.SERVICE_ID" });
							Debug.Log("SystemId from Manifest: " + text2);
							return text2;
						}
					}
				}
			}
		}

		private static Action<T> ToOnGameThread<T>(Action<T> toConvert)
		{
			return delegate(T val)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val);
				});
			};
		}

		private static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
		{
			return delegate(T1 val1, T2 val2)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val1, val2);
				});
			};
		}
	}
}
