using System;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidTaskUtils
	{
		private class TaskOnCompleteProxy<T> : AndroidJavaProxy
		{
			private Action<T> mCallback;

			public TaskOnCompleteProxy(Action<T> callback)
				: base("com/google/android/gms/tasks/OnCompleteListener")
			{
				mCallback = callback;
			}

			public void onComplete(T result)
			{
				if (result is IDisposable)
				{
					using ((IDisposable)(object)result)
					{
						mCallback(result);
						return;
					}
				}
				mCallback(result);
			}
		}

		private class TaskOnSuccessProxy<T> : AndroidJavaProxy
		{
			private Action<T> mCallback;

			private Action<AndroidJavaObject> mCallback2;

			private bool mDisposeResult;

			public TaskOnSuccessProxy(Action<T> callback, bool disposeResult)
				: base("com/google/android/gms/tasks/OnSuccessListener")
			{
				mCallback = callback;
				mDisposeResult = disposeResult;
			}

			public void onSuccess(T result)
			{
				if (result is IDisposable && mDisposeResult)
				{
					using ((IDisposable)(object)result)
					{
						mCallback(result);
						return;
					}
				}
				mCallback(result);
			}
		}

		private class TaskOnFailedProxy : AndroidJavaProxy
		{
			private Action<AndroidJavaObject> mCallback;

			public TaskOnFailedProxy(Action<AndroidJavaObject> callback)
				: base("com/google/android/gms/tasks/OnFailureListener")
			{
				mCallback = callback;
			}

			public void onFailure(AndroidJavaObject exception)
			{
				using (exception)
				{
					mCallback(exception);
				}
			}
		}

		private AndroidTaskUtils()
		{
		}

		public static void AddOnSuccessListener<T>(AndroidJavaObject task, Action<T> callback)
		{
			using (task.Call<AndroidJavaObject>("addOnSuccessListener", new object[1]
			{
				new TaskOnSuccessProxy<T>(callback, true)
			}))
			{
			}
		}

		public static void AddOnSuccessListener<T>(AndroidJavaObject task, bool disposeResult, Action<T> callback)
		{
			using (task.Call<AndroidJavaObject>("addOnSuccessListener", new object[1]
			{
				new TaskOnSuccessProxy<T>(callback, disposeResult)
			}))
			{
			}
		}

		public static void AddOnFailureListener(AndroidJavaObject task, Action<AndroidJavaObject> callback)
		{
			using (task.Call<AndroidJavaObject>("addOnFailureListener", new object[1]
			{
				new TaskOnFailedProxy(callback)
			}))
			{
			}
		}

		public static void AddOnCompleteListener<T>(AndroidJavaObject task, Action<T> callback)
		{
			using (task.Call<AndroidJavaObject>("addOnCompleteListener", new object[1]
			{
				new TaskOnCompleteProxy<T>(callback)
			}))
			{
			}
		}
	}
}
