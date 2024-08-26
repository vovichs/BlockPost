using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidHelperFragment
	{
		public enum WaitingRoomUIStatus
		{
			Valid = 1,
			Cancelled = 2,
			LeftRoom = 3,
			InvalidRoom = 4,
			Busy = -1,
			InternalError = -2
		}

		public class InvitationResultHolder
		{
			public int MinAutomatchingPlayers;

			public int MaxAutomatchingPlayers;

			public List<string> PlayerIdsToInvite;

			public InvitationResultHolder(int MinAutomatchingPlayers, int MaxAutomatchingPlayers, List<string> PlayerIdsToInvite)
			{
				this.MinAutomatchingPlayers = MinAutomatchingPlayers;
				this.MaxAutomatchingPlayers = MaxAutomatchingPlayers;
				this.PlayerIdsToInvite = PlayerIdsToInvite;
			}
		}

		private const string HelperFragmentClass = "com.google.games.bridge.HelperFragment";

		public static AndroidJavaObject GetActivity()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				return androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
		}

		public static AndroidJavaObject GetDefaultPopupView()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject androidJavaObject = GetActivity())
				{
					return androidJavaClass.CallStatic<AndroidJavaObject>("getDecorView", new object[1] { androidJavaObject });
				}
			}
		}

		public static void ShowAchievementsUI(Action<UIStatus> cb)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>("showAchievementUi", new object[1] { GetActivity() }))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(int uiCode)
					{
						Debug.Log("ShowAchievementsUI result " + uiCode);
						cb((UIStatus)uiCode);
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						Debug.Log("ShowAchievementsUI failed with exception");
						cb(UIStatus.InternalError);
					});
				}
			}
		}

		public static void ShowCaptureOverlayUI()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				androidJavaClass.CallStatic("showCaptureOverlayUi", GetActivity());
			}
		}

		public static void ShowAllLeaderboardsUI(Action<UIStatus> cb)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>("showAllLeaderboardsUi", new object[1] { GetActivity() }))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(int uiCode)
					{
						Debug.Log("ShowAllLeaderboardsUI result " + uiCode);
						cb((UIStatus)uiCode);
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						Debug.Log("ShowAllLeaderboardsUI failed with exception");
						cb(UIStatus.InternalError);
					});
				}
			}
		}

		public static void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan timeSpan, Action<UIStatus> cb)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>("showLeaderboardUi", new object[3]
				{
					GetActivity(),
					leaderboardId,
					AndroidJavaConverter.ToLeaderboardVariantTimeSpan(timeSpan)
				}))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(int uiCode)
					{
						Debug.Log("ShowLeaderboardUI result " + uiCode);
						cb((UIStatus)uiCode);
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						Debug.Log("ShowLeaderboardUI failed with exception");
						cb(UIStatus.InternalError);
					});
				}
			}
		}

		public static void ShowSelectSnapshotUI(bool showCreateSaveUI, bool showDeleteSaveUI, int maxDisplayedSavedGames, string uiTitle, Action<SelectUIStatus, ISavedGameMetadata> cb)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>("showSelectSnapshotUi", new object[5]
				{
					GetActivity(),
					uiTitle,
					showCreateSaveUI,
					showDeleteSaveUI,
					maxDisplayedSavedGames
				}))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject result)
					{
						SelectUIStatus selectUIStatus = (SelectUIStatus)result.Get<int>("status");
						Debug.Log("ShowSelectSnapshotUI result " + selectUIStatus);
						AndroidJavaObject androidJavaObject = result.Get<AndroidJavaObject>("metadata");
						AndroidSnapshotMetadata arg = ((androidJavaObject == null) ? null : new AndroidSnapshotMetadata(androidJavaObject, null));
						cb(selectUIStatus, arg);
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						Debug.Log("ShowSelectSnapshotUI failed with exception");
						cb(SelectUIStatus.InternalError, null);
					});
				}
			}
		}

		public static void ShowRtmpSelectOpponentsUI(uint minOpponents, uint maxOpponents, Action<UIStatus, InvitationResultHolder> cb)
		{
			ShowSelectOpponentsUI(minOpponents, maxOpponents, true, cb);
		}

		public static void ShowTbmpSelectOpponentsUI(uint minOpponents, uint maxOpponents, Action<UIStatus, InvitationResultHolder> cb)
		{
			ShowSelectOpponentsUI(minOpponents, maxOpponents, false, cb);
		}

		private static void ShowSelectOpponentsUI(uint minOpponents, uint maxOpponents, bool isRealTime, Action<UIStatus, InvitationResultHolder> cb)
		{
			string methodName = (isRealTime ? "showRtmpSelectOpponentsUi" : "showTbmpSelectOpponentsUi");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>(methodName, new object[3]
				{
					GetActivity(),
					(int)minOpponents,
					(int)maxOpponents
				}))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject result)
					{
						int num = result.Get<int>("status");
						if (num != 1)
						{
							cb((UIStatus)num, null);
						}
						else
						{
							List<string> playerIdsToInvite;
							using (AndroidJavaObject playerIdsObject = result.Get<AndroidJavaObject>("playerIdsToInvite"))
							{
								playerIdsToInvite = CreatePlayerIdsToInvite(playerIdsObject);
							}
							InvitationResultHolder arg = new InvitationResultHolder(result.Get<int>("minAutomatchingPlayers"), result.Get<int>("maxAutomatchingPlayers"), playerIdsToInvite);
							cb((UIStatus)num, arg);
						}
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						Debug.Log("showSelectOpponentsUi failed with exception");
						cb(UIStatus.InternalError, null);
					});
				}
			}
		}

		public static void ShowWaitingRoomUI(AndroidJavaObject room, int minParticipantsToStart, Action<WaitingRoomUIStatus, AndroidJavaObject> cb)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>("showWaitingRoomUI", new object[3]
				{
					GetActivity(),
					room,
					minParticipantsToStart
				}))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject result)
					{
						cb((WaitingRoomUIStatus)result.Get<int>("status"), result.Get<AndroidJavaObject>("room"));
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						Debug.Log("ShowWaitingRoomUI failed with exception");
						cb(WaitingRoomUIStatus.InternalError, null);
					});
				}
			}
		}

		public static void ShowInboxUI(Action<UIStatus, TurnBasedMatch> cb)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>("showInboxUi", new object[1] { GetActivity() }))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject result)
					{
						int num = result.Get<int>("status");
						if (num != 1)
						{
							cb((UIStatus)num, null);
							return;
						}
						using (AndroidJavaObject turnBasedMatch = result.Get<AndroidJavaObject>("turnBasedMatch"))
						{
							cb((UIStatus)num, AndroidJavaConverter.ToTurnBasedMatch(turnBasedMatch));
						}
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						Debug.Log("showInboxUi failed with exception");
						cb(UIStatus.InternalError, null);
					});
				}
			}
		}

		public static void ShowInvitationInboxUI(Action<UIStatus, Invitation> cb)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>("showInvitationInboxUI", new object[1] { GetActivity() }))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject result)
					{
						int num = result.Get<int>("status");
						if (num != 1)
						{
							cb((UIStatus)num, null);
							return;
						}
						using (AndroidJavaObject invitation = result.Get<AndroidJavaObject>("invitation"))
						{
							cb((UIStatus)num, AndroidJavaConverter.ToInvitation(invitation));
						}
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						Debug.Log("ShowInvitationInboxUI failed with exception");
						cb(UIStatus.InternalError, null);
					});
				}
			}
		}

		private static List<string> CreatePlayerIdsToInvite(AndroidJavaObject playerIdsObject)
		{
			int num = playerIdsObject.Call<int>("size", Array.Empty<object>());
			List<string> list = new List<string>();
			for (int i = 0; i < num; i++)
			{
				list.Add(playerIdsObject.Call<string>("get", new object[1] { i }));
			}
			return list;
		}
	}
}
