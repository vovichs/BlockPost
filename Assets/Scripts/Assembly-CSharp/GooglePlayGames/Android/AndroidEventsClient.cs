using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidEventsClient : IEventsClient
	{
		private volatile AndroidJavaObject mEventsClient;

		public AndroidEventsClient(AndroidJavaObject account)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.Games"))
			{
				mEventsClient = androidJavaClass.CallStatic<AndroidJavaObject>("getEventsClient", new object[2]
				{
					AndroidHelperFragment.GetActivity(),
					account
				});
			}
		}

		public void FetchAllEvents(DataSource source, Action<ResponseStatus, List<IEvent>> callback)
		{
			callback = ToOnGameThread(callback);
			using (AndroidJavaObject task = mEventsClient.Call<AndroidJavaObject>("load", new object[1] { source == DataSource.ReadNetworkOnly }))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						int num = androidJavaObject.Call<int>("getCount", Array.Empty<object>());
						List<IEvent> list = new List<IEvent>();
						for (int i = 0; i < num; i++)
						{
							using (AndroidJavaObject eventJava = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { i }))
							{
								list.Add(CreateEvent(eventJava));
							}
						}
						androidJavaObject.Call("release");
						callback((!annotatedData.Call<bool>("isStale", Array.Empty<object>())) ? ResponseStatus.Success : ResponseStatus.SuccessWithStale, list);
					}
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					Debug.Log("FetchAllEvents failed");
					callback(ResponseStatus.InternalError, null);
				});
			}
		}

		public void FetchEvent(DataSource source, string eventId, Action<ResponseStatus, IEvent> callback)
		{
			callback = ToOnGameThread(callback);
			string[] array = new string[1] { eventId };
			using (AndroidJavaObject task = mEventsClient.Call<AndroidJavaObject>("loadByIds", new object[2]
			{
				source == DataSource.ReadNetworkOnly,
				array
			}))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						if (androidJavaObject.Call<int>("getCount", Array.Empty<object>()) > 0)
						{
							using (AndroidJavaObject eventJava = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { 0 }))
							{
								callback((!annotatedData.Call<bool>("isStale", Array.Empty<object>())) ? ResponseStatus.Success : ResponseStatus.SuccessWithStale, CreateEvent(eventJava));
							}
						}
						else
						{
							callback((!annotatedData.Call<bool>("isStale", Array.Empty<object>())) ? ResponseStatus.Success : ResponseStatus.SuccessWithStale, null);
						}
						androidJavaObject.Call("release");
					}
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					Debug.Log("FetchEvent failed");
					callback(ResponseStatus.InternalError, null);
				});
			}
		}

		public void IncrementEvent(string eventId, uint stepsToIncrement)
		{
			mEventsClient.Call("increment", eventId, (int)stepsToIncrement);
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

		private static GooglePlayGames.BasicApi.Events.Event CreateEvent(AndroidJavaObject eventJava)
		{
			string id = eventJava.Call<string>("getEventId", Array.Empty<object>());
			string name = eventJava.Call<string>("getName", Array.Empty<object>());
			string description = eventJava.Call<string>("getDescription", Array.Empty<object>());
			string imageUrl = eventJava.Call<string>("getIconImageUrl", Array.Empty<object>());
			ulong currentCount = (ulong)eventJava.Call<long>("getValue", Array.Empty<object>());
			EventVisibility visibility = ((!eventJava.Call<bool>("isVisible", Array.Empty<object>())) ? EventVisibility.Hidden : EventVisibility.Revealed);
			return new GooglePlayGames.BasicApi.Events.Event(id, name, description, imageUrl, currentCount, visibility);
		}
	}
}
