using System;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Video;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidVideoClient : IVideoClient
	{
		private class OnCaptureOverlayStateListenerProxy : AndroidJavaProxy
		{
			private CaptureOverlayStateListener mListener;

			public OnCaptureOverlayStateListenerProxy(CaptureOverlayStateListener listener)
				: base("com/google/android/gms/games/VideosClient$OnCaptureOverlayStateListener")
			{
				mListener = listener;
			}

			public void onCaptureOverlayStateChanged(int overlayState)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnCaptureOverlayStateChanged(FromVideoCaptureOverlayState(overlayState));
				});
			}

			private static VideoCaptureOverlayState FromVideoCaptureOverlayState(int overlayState)
			{
				switch (overlayState)
				{
				case 1:
					return VideoCaptureOverlayState.Shown;
				case 2:
					return VideoCaptureOverlayState.Started;
				case 3:
					return VideoCaptureOverlayState.Stopped;
				case 4:
					return VideoCaptureOverlayState.Dismissed;
				default:
					return VideoCaptureOverlayState.Unknown;
				}
			}
		}

		private volatile AndroidJavaObject mVideosClient;

		private bool mIsCaptureSupported;

		private OnCaptureOverlayStateListenerProxy mOnCaptureOverlayStateListenerProxy;

		public AndroidVideoClient(bool isCaptureSupported, AndroidJavaObject account)
		{
			mIsCaptureSupported = isCaptureSupported;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.Games"))
			{
				mVideosClient = androidJavaClass.CallStatic<AndroidJavaObject>("getVideosClient", new object[2]
				{
					AndroidHelperFragment.GetActivity(),
					account
				});
			}
		}

		public void GetCaptureCapabilities(Action<ResponseStatus, VideoCapabilities> callback)
		{
			callback = ToOnGameThread(callback);
			using (AndroidJavaObject task = mVideosClient.Call<AndroidJavaObject>("getCaptureCapabilities", Array.Empty<object>()))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject videoCapabilities)
				{
					callback(ResponseStatus.Success, CreateVideoCapabilities(videoCapabilities));
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					callback(ResponseStatus.InternalError, null);
				});
			}
		}

		public void ShowCaptureOverlay()
		{
			AndroidHelperFragment.ShowCaptureOverlayUI();
		}

		public void GetCaptureState(Action<ResponseStatus, VideoCaptureState> callback)
		{
			callback = ToOnGameThread(callback);
			using (AndroidJavaObject task = mVideosClient.Call<AndroidJavaObject>("getCaptureState", Array.Empty<object>()))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject captureState)
				{
					callback(ResponseStatus.Success, CreateVideoCaptureState(captureState));
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					callback(ResponseStatus.InternalError, null);
				});
			}
		}

		public void IsCaptureAvailable(VideoCaptureMode captureMode, Action<ResponseStatus, bool> callback)
		{
			callback = ToOnGameThread(callback);
			using (AndroidJavaObject task = mVideosClient.Call<AndroidJavaObject>("isCaptureAvailable", new object[1] { ToVideoCaptureMode(captureMode) }))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(bool isCaptureAvailable)
				{
					callback(ResponseStatus.Success, isCaptureAvailable);
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					callback(ResponseStatus.InternalError, false);
				});
			}
		}

		public bool IsCaptureSupported()
		{
			return mIsCaptureSupported;
		}

		public void RegisterCaptureOverlayStateChangedListener(CaptureOverlayStateListener listener)
		{
			if (mOnCaptureOverlayStateListenerProxy != null)
			{
				UnregisterCaptureOverlayStateChangedListener();
			}
			mOnCaptureOverlayStateListenerProxy = new OnCaptureOverlayStateListenerProxy(listener);
			using (mVideosClient.Call<AndroidJavaObject>("registerOnCaptureOverlayStateChangedListener", new object[1] { mOnCaptureOverlayStateListenerProxy }))
			{
			}
		}

		public void UnregisterCaptureOverlayStateChangedListener()
		{
			if (mOnCaptureOverlayStateListenerProxy != null)
			{
				using (mVideosClient.Call<AndroidJavaObject>("unregisterOnCaptureOverlayStateChangedListener", new object[1] { mOnCaptureOverlayStateListenerProxy }))
				{
				}
				mOnCaptureOverlayStateListenerProxy = null;
			}
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

		private static VideoQualityLevel FromVideoQualityLevel(int captureQualityJava)
		{
			switch (captureQualityJava)
			{
			case 0:
				return VideoQualityLevel.SD;
			case 1:
				return VideoQualityLevel.HD;
			case 2:
				return VideoQualityLevel.XHD;
			case 3:
				return VideoQualityLevel.FullHD;
			default:
				return VideoQualityLevel.Unknown;
			}
		}

		private static VideoCaptureMode FromVideoCaptureMode(int captureMode)
		{
			switch (captureMode)
			{
			case 0:
				return VideoCaptureMode.File;
			case 1:
				return VideoCaptureMode.Stream;
			default:
				return VideoCaptureMode.Unknown;
			}
		}

		private static int ToVideoCaptureMode(VideoCaptureMode captureMode)
		{
			switch (captureMode)
			{
			case VideoCaptureMode.File:
				return 0;
			case VideoCaptureMode.Stream:
				return 1;
			default:
				return -1;
			}
		}

		private static VideoCaptureState CreateVideoCaptureState(AndroidJavaObject videoCaptureState)
		{
			bool isCapturing = videoCaptureState.Call<bool>("isCapturing", Array.Empty<object>());
			VideoCaptureMode captureMode = FromVideoCaptureMode(videoCaptureState.Call<int>("getCaptureMode", Array.Empty<object>()));
			VideoQualityLevel qualityLevel = FromVideoQualityLevel(videoCaptureState.Call<int>("getCaptureQuality", Array.Empty<object>()));
			bool isOverlayVisible = videoCaptureState.Call<bool>("isOverlayVisible", Array.Empty<object>());
			bool isPaused = videoCaptureState.Call<bool>("isPaused", Array.Empty<object>());
			return new VideoCaptureState(isCapturing, captureMode, qualityLevel, isOverlayVisible, isPaused);
		}

		private static VideoCapabilities CreateVideoCapabilities(AndroidJavaObject videoCapabilities)
		{
			bool isCameraSupported = videoCapabilities.Call<bool>("isCameraSupported", Array.Empty<object>());
			bool isMicSupported = videoCapabilities.Call<bool>("isMicSupported", Array.Empty<object>());
			bool isWriteStorageSupported = videoCapabilities.Call<bool>("isWriteStorageSupported", Array.Empty<object>());
			bool[] captureModesSupported = videoCapabilities.Call<bool[]>("getSupportedCaptureModes", Array.Empty<object>());
			bool[] qualityLevelsSupported = videoCapabilities.Call<bool[]>("getSupportedQualityLevels", Array.Empty<object>());
			return new VideoCapabilities(isCameraSupported, isMicSupported, isWriteStorageSupported, captureModesSupported, qualityLevelsSupported);
		}
	}
}
