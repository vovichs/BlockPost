using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidRealTimeMultiplayerClient : IRealTimeMultiplayerClient
	{
		private enum RoomStatus
		{
			NotCreated = -1,
			Inviting = 0,
			AutoMatching = 1,
			Connecting = 2,
			Active = 3,
			Deleted = 4
		}

		private class RoomStatusUpdateCallbackProxy : AndroidJavaProxy
		{
			private OnGameThreadForwardingListener mListener;

			private AndroidRealTimeMultiplayerClient mParent;

			public RoomStatusUpdateCallbackProxy(AndroidRealTimeMultiplayerClient parent, OnGameThreadForwardingListener listener)
				: base("com/google/games/bridge/RoomStatusUpdateCallbackProxy$Callback")
			{
				mListener = listener;
				mParent = parent;
			}

			public void onRoomConnecting(AndroidJavaObject room)
			{
				mParent.mRoom = room;
			}

			public void onRoomAutoMatching(AndroidJavaObject room)
			{
				mParent.mRoom = room;
			}

			public void onPeerInvitedToRoom(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				handleParticipantStatusChanged(room, participantIds);
			}

			public void onPeerDeclined(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				handleParticipantStatusChanged(room, participantIds);
			}

			public void onPeerJoined(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				handleParticipantStatusChanged(room, participantIds);
			}

			public void onPeerLeft(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				handleParticipantStatusChanged(room, participantIds);
			}

			private void handleParticipantStatusChanged(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				mParent.mRoom = room;
				int num = participantIds.Get<int>("size");
				for (int i = 0; i < num; i++)
				{
					string text = participantIds.Call<string>("get", new object[1] { i });
					Participant participant = AndroidJavaConverter.ToParticipant(mParent.mRoom.Call<AndroidJavaObject>("getParticipant", new object[1] { text }));
					if (participant.Status == Participant.ParticipantStatus.Declined || participant.Status == Participant.ParticipantStatus.Left)
					{
						mListener.OnParticipantLeft(participant);
						RoomStatus roomStatus = mParent.GetRoomStatus();
						if (roomStatus != RoomStatus.Connecting && roomStatus != RoomStatus.AutoMatching)
						{
							mParent.LeaveRoom();
						}
					}
				}
			}

			public void onConnectedToRoom(AndroidJavaObject room)
			{
				if (mParent.GetRoomStatus() == RoomStatus.Active)
				{
					mParent.mRoom = room;
				}
				else
				{
					handleConnectedSetChanged(room);
				}
			}

			public void onDisconnectedFromRoom(AndroidJavaObject room)
			{
				if (mParent.GetRoomStatus() == RoomStatus.Active)
				{
					mParent.mRoom = room;
					return;
				}
				handleConnectedSetChanged(room);
				mParent.CleanSession();
			}

			public void onPeersConnected(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				if (mParent.GetRoomStatus() == RoomStatus.Active)
				{
					mParent.mRoom = room;
					mParent.mListener.OnPeersConnected(AndroidJavaConverter.ToStringList(participantIds).ToArray());
				}
				else
				{
					handleConnectedSetChanged(room);
				}
			}

			public void onPeersDisconnected(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				if (mParent.GetRoomStatus() == RoomStatus.Active)
				{
					mParent.mRoom = room;
					mParent.mListener.OnPeersDisconnected(AndroidJavaConverter.ToStringList(participantIds).ToArray());
				}
				else
				{
					handleConnectedSetChanged(room);
				}
			}

			private void handleConnectedSetChanged(AndroidJavaObject room)
			{
				HashSet<string> hashSet = new HashSet<string>();
				foreach (Participant connectedParticipant in mParent.GetConnectedParticipants())
				{
					hashSet.Add(connectedParticipant.ParticipantId);
				}
				mParent.mRoom = room;
				HashSet<string> hashSet2 = new HashSet<string>();
				foreach (Participant connectedParticipant2 in mParent.GetConnectedParticipants())
				{
					hashSet2.Add(connectedParticipant2.ParticipantId);
				}
				if (hashSet.Equals(hashSet2))
				{
					GooglePlayGames.OurUtils.Logger.w("Received connected set callback with unchanged connected set!");
					return;
				}
				List<string> list = new List<string>();
				foreach (string item in hashSet)
				{
					if (!hashSet2.Contains(item))
					{
						list.Add(item);
					}
				}
				if (mParent.GetRoomStatus() == RoomStatus.Deleted)
				{
					GooglePlayGames.OurUtils.Logger.e("Participants disconnected during room setup, failing. Participants were: " + string.Join(",", list.ToArray()));
					mParent.mListener.OnRoomConnected(false);
					mParent.CleanSession();
				}
				else
				{
					mParent.mListener.OnRoomSetupProgress(mParent.GetPercentComplete());
				}
			}

			public void onP2PConnected(string participantId)
			{
			}

			public void onP2PDisconnected(string participantId)
			{
			}
		}

		private class MessageReceivedListenerProxy : AndroidJavaProxy
		{
			private OnGameThreadForwardingListener mListener;

			public MessageReceivedListenerProxy(OnGameThreadForwardingListener listener)
				: base("com/google/games/bridge/RealTimeMessageReceivedListenerProxy$Callback")
			{
				mListener = listener;
			}

			public void onRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
			{
				mListener.OnRealTimeMessageReceived(isReliable, senderId, data);
			}
		}

		private class RoomUpdateCallbackProxy : AndroidJavaProxy
		{
			private OnGameThreadForwardingListener mListener;

			private AndroidRealTimeMultiplayerClient mParent;

			public RoomUpdateCallbackProxy(AndroidRealTimeMultiplayerClient parent, OnGameThreadForwardingListener listener)
				: base("com/google/games/bridge/RoomUpdateCallbackProxy$Callback")
			{
				mListener = listener;
				mParent = parent;
			}

			public void onRoomCreated(int statusCode, AndroidJavaObject room)
			{
				if (room == null)
				{
					mListener.OnRoomConnected(false);
					return;
				}
				mParent.mRoom = room;
				mListener.OnRoomSetupProgress(mParent.GetPercentComplete());
			}

			public void onJoinedRoom(int statusCode, AndroidJavaObject room)
			{
				if (room == null)
				{
					mListener.OnRoomConnected(false);
					return;
				}
				mParent.mRoom = room;
				int num = 0;
				using (AndroidJavaObject androidJavaObject = room.Call<AndroidJavaObject>("getAutoMatchCriteria", Array.Empty<object>()))
				{
					if (androidJavaObject != null)
					{
						num += androidJavaObject.Call<int>("getInt", new object[2] { "min_automatch_players", 0 });
					}
				}
				using (AndroidJavaObject androidJavaObject2 = room.Call<AndroidJavaObject>("getParticipantIds", Array.Empty<object>()))
				{
					num += androidJavaObject2.Call<int>("size", Array.Empty<object>());
				}
				mParent.mMinPlayersToStart = num;
			}

			public void onLeftRoom(int statusCode, string roomId)
			{
				mListener.OnLeftRoom();
				mParent.CleanSession();
			}

			public void onRoomConnected(int statusCode, AndroidJavaObject room)
			{
				if (room == null)
				{
					mListener.OnRoomConnected(false);
					return;
				}
				mParent.mRoom = room;
				mListener.OnRoomConnected(true);
			}
		}

		private class OnGameThreadForwardingListener
		{
			private readonly RealTimeMultiplayerListener mListener;

			internal OnGameThreadForwardingListener(RealTimeMultiplayerListener listener)
			{
				mListener = Misc.CheckNotNull(listener);
			}

			public void OnRoomSetupProgress(float percent)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnRoomSetupProgress(percent);
				});
			}

			public void OnRoomConnected(bool success)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnRoomConnected(success);
				});
			}

			public void OnLeftRoom()
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnLeftRoom();
				});
			}

			public void OnPeersConnected(string[] participantIds)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnPeersConnected(participantIds);
				});
			}

			public void OnPeersDisconnected(string[] participantIds)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnPeersDisconnected(participantIds);
				});
			}

			public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnRealTimeMessageReceived(isReliable, senderId, data);
				});
			}

			public void OnParticipantLeft(Participant participant)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnParticipantLeft(participant);
				});
			}
		}

		private readonly object mSessionLock = new object();

		private int mMinPlayersToStart;

		private volatile AndroidClient mAndroidClient;

		private volatile AndroidJavaObject mRtmpClient;

		private volatile AndroidJavaObject mInvitationsClient;

		private AndroidJavaObject mRoom;

		private AndroidJavaObject mRoomConfig;

		private OnGameThreadForwardingListener mListener;

		private Invitation mInvitation;

		public AndroidRealTimeMultiplayerClient(AndroidClient androidClient, AndroidJavaObject account)
		{
			mAndroidClient = androidClient;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.Games"))
			{
				mRtmpClient = androidJavaClass.CallStatic<AndroidJavaObject>("getRealTimeMultiplayerClient", new object[2]
				{
					AndroidHelperFragment.GetActivity(),
					account
				});
				mInvitationsClient = androidJavaClass.CallStatic<AndroidJavaObject>("getInvitationsClient", new object[2]
				{
					AndroidHelperFragment.GetActivity(),
					account
				});
			}
		}

		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, RealTimeMultiplayerListener listener)
		{
			CreateQuickGame(minOpponents, maxOpponents, variant, 0uL, listener);
		}

		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, RealTimeMultiplayerListener listener)
		{
			OnGameThreadForwardingListener listenerOnGameThread = new OnGameThreadForwardingListener(listener);
			lock (mSessionLock)
			{
				if (GetRoomStatus() == RoomStatus.Active)
				{
					GooglePlayGames.OurUtils.Logger.e("Received attempt to create a new room without cleaning up the old one.");
					listenerOnGameThread.OnRoomConnected(false);
					return;
				}
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.multiplayer.realtime.RoomConfig"))
				{
					using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.games.bridge.RoomUpdateCallbackProxy", new RoomUpdateCallbackProxy(this, listenerOnGameThread)))
					{
						using (AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("builder", new object[1] { androidJavaObject }))
						{
							if (variant != 0)
							{
								androidJavaObject2.Call<AndroidJavaObject>("setVariant", new object[1] { (int)variant });
							}
							using (AndroidJavaObject androidJavaObject3 = androidJavaClass.CallStatic<AndroidJavaObject>("createAutoMatchCriteria", new object[3]
							{
								(int)minOpponents,
								(int)maxOpponents,
								(long)exclusiveBitMask
							}))
							{
								using (androidJavaObject2.Call<AndroidJavaObject>("setAutoMatchCriteria", new object[1] { androidJavaObject3 }))
								{
								}
							}
							using (AndroidJavaObject androidJavaObject5 = new AndroidJavaObject("com.google.games.bridge.RealTimeMessageReceivedListenerProxy", new MessageReceivedListenerProxy(listenerOnGameThread)))
							{
								using (androidJavaObject2.Call<AndroidJavaObject>("setOnMessageReceivedListener", new object[1] { androidJavaObject5 }))
								{
								}
							}
							using (AndroidJavaObject androidJavaObject6 = new AndroidJavaObject("com.google.games.bridge.RoomStatusUpdateCallbackProxy", new RoomStatusUpdateCallbackProxy(this, listenerOnGameThread)))
							{
								using (androidJavaObject2.Call<AndroidJavaObject>("setRoomStatusUpdateCallback", new object[1] { androidJavaObject6 }))
								{
								}
							}
							mRoomConfig = androidJavaObject2.Call<AndroidJavaObject>("build", Array.Empty<object>());
							mListener = listenerOnGameThread;
						}
					}
				}
				mMinPlayersToStart = (int)(minOpponents + 1);
				using (AndroidJavaObject task = mRtmpClient.Call<AndroidJavaObject>("create", new object[1] { mRoomConfig }))
				{
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						listenerOnGameThread.OnRoomConnected(false);
						CleanSession();
					});
				}
			}
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, RealTimeMultiplayerListener listener)
		{
			OnGameThreadForwardingListener listenerOnGameThread = new OnGameThreadForwardingListener(listener);
			lock (mSessionLock)
			{
				if (GetRoomStatus() == RoomStatus.Active)
				{
					GooglePlayGames.OurUtils.Logger.e("Received attempt to create a new room without cleaning up the old one.");
					listenerOnGameThread.OnRoomConnected(false);
					return;
				}
				AndroidHelperFragment.ShowRtmpSelectOpponentsUI(minOpponents, maxOpponents, delegate(UIStatus status, AndroidHelperFragment.InvitationResultHolder result)
				{
					switch (status)
					{
					case UIStatus.NotAuthorized:
						mAndroidClient.SignOut(delegate
						{
							listenerOnGameThread.OnRoomConnected(false);
							CleanSession();
						});
						break;
					default:
						listenerOnGameThread.OnRoomConnected(false);
						CleanSession();
						break;
					case UIStatus.Valid:
					{
						using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.multiplayer.realtime.RoomConfig"))
						{
							using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.games.bridge.RoomUpdateCallbackProxy", new RoomUpdateCallbackProxy(this, listenerOnGameThread)))
							{
								using (AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("builder", new object[1] { androidJavaObject }))
								{
									if (result.MinAutomatchingPlayers > 0)
									{
										using (AndroidJavaObject androidJavaObject3 = androidJavaClass.CallStatic<AndroidJavaObject>("createAutoMatchCriteria", new object[3] { result.MinAutomatchingPlayers, result.MaxAutomatchingPlayers, 0L }))
										{
											using (androidJavaObject2.Call<AndroidJavaObject>("setAutoMatchCriteria", new object[1] { androidJavaObject3 }))
											{
											}
										}
									}
									if (variant != 0)
									{
										using (androidJavaObject2.Call<AndroidJavaObject>("setVariant", new object[1] { (int)variant }))
										{
										}
									}
									using (AndroidJavaObject androidJavaObject5 = new AndroidJavaObject("com.google.games.bridge.RealTimeMessageReceivedListenerProxy", new MessageReceivedListenerProxy(listenerOnGameThread)))
									{
										using (androidJavaObject2.Call<AndroidJavaObject>("setOnMessageReceivedListener", new object[1] { androidJavaObject5 }))
										{
										}
									}
									using (AndroidJavaObject androidJavaObject6 = new AndroidJavaObject("com.google.games.bridge.RoomStatusUpdateCallbackProxy", new RoomStatusUpdateCallbackProxy(this, listenerOnGameThread)))
									{
										using (androidJavaObject2.Call<AndroidJavaObject>("setRoomStatusUpdateCallback", new object[1] { androidJavaObject6 }))
										{
										}
									}
									using (androidJavaObject2.Call<AndroidJavaObject>("addPlayersToInvite", new object[1] { AndroidJavaConverter.ToJavaStringList(result.PlayerIdsToInvite) }))
									{
										mRoomConfig = androidJavaObject2.Call<AndroidJavaObject>("build", Array.Empty<object>());
									}
									mListener = listenerOnGameThread;
								}
							}
						}
						mMinPlayersToStart = result.MinAutomatchingPlayers + result.PlayerIdsToInvite.Count + 1;
						using (AndroidJavaObject task = mRtmpClient.Call<AndroidJavaObject>("create", new object[1] { mRoomConfig }))
						{
							AndroidTaskUtils.AddOnFailureListener(task, delegate
							{
								listenerOnGameThread.OnRoomConnected(false);
								CleanSession();
							});
							break;
						}
					}
					}
				});
			}
		}

		private float GetPercentComplete()
		{
			int num = Math.Max(1, GetConnectedParticipants().Count);
			return Math.Min(100f * (float)num / (float)mMinPlayersToStart, 100f);
		}

		public void ShowWaitingRoomUI()
		{
			RoomStatus roomStatus = GetRoomStatus();
			if (roomStatus != RoomStatus.Connecting && roomStatus != RoomStatus.AutoMatching && roomStatus != 0)
			{
				return;
			}
			AndroidHelperFragment.ShowWaitingRoomUI(mRoom, mMinPlayersToStart, delegate(AndroidHelperFragment.WaitingRoomUIStatus response, AndroidJavaObject room)
			{
				switch (response)
				{
				case AndroidHelperFragment.WaitingRoomUIStatus.Valid:
					mRoom = room;
					if (GetRoomStatus() == RoomStatus.Active)
					{
						mListener.OnRoomConnected(true);
					}
					break;
				case AndroidHelperFragment.WaitingRoomUIStatus.LeftRoom:
					LeaveRoom();
					break;
				default:
					mListener.OnRoomSetupProgress(GetPercentComplete());
					break;
				}
			});
		}

		public void GetAllInvitations(Action<Invitation[]> callback)
		{
			callback = ToOnGameThread(callback);
			using (AndroidJavaObject task = mInvitationsClient.Call<AndroidJavaObject>("loadInvitations", Array.Empty<object>()))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						int num = androidJavaObject.Call<int>("getCount", Array.Empty<object>());
						Invitation[] array = new Invitation[num];
						for (int i = 0; i < num; i++)
						{
							using (AndroidJavaObject invitation = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { i }))
							{
								array[i] = AndroidJavaConverter.ToInvitation(invitation);
							}
						}
						callback(array);
					}
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					callback(null);
				});
			}
		}

		public void AcceptFromInbox(RealTimeMultiplayerListener listener)
		{
			OnGameThreadForwardingListener listenerOnGameThread = new OnGameThreadForwardingListener(listener);
			lock (mSessionLock)
			{
				if (GetRoomStatus() == RoomStatus.Active)
				{
					GooglePlayGames.OurUtils.Logger.e("Received attempt to accept invitation without cleaning up active session.");
					listenerOnGameThread.OnRoomConnected(false);
					return;
				}
				AndroidHelperFragment.ShowInvitationInboxUI(delegate(UIStatus status, Invitation invitation)
				{
					switch (status)
					{
					case UIStatus.NotAuthorized:
						mAndroidClient.SignOut(delegate
						{
							listenerOnGameThread.OnRoomConnected(false);
						});
						break;
					default:
						GooglePlayGames.OurUtils.Logger.d("User did not complete invitation screen.");
						listenerOnGameThread.OnRoomConnected(false);
						break;
					case UIStatus.Valid:
						mInvitation = invitation;
						AcceptInvitation(mInvitation.InvitationId, listener);
						break;
					}
				});
			}
		}

		public void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener)
		{
			OnGameThreadForwardingListener listenerOnGameThread = new OnGameThreadForwardingListener(listener);
			lock (mSessionLock)
			{
				if (GetRoomStatus() == RoomStatus.Active)
				{
					GooglePlayGames.OurUtils.Logger.e("Received attempt to accept invitation without cleaning up active session.");
					listenerOnGameThread.OnRoomConnected(false);
					return;
				}
				FindInvitation(invitationId, delegate
				{
					listenerOnGameThread.OnRoomConnected(false);
				}, delegate(Invitation invitation)
				{
					mInvitation = invitation;
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.multiplayer.realtime.RoomConfig"))
					{
						using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.games.bridge.RoomUpdateCallbackProxy", new RoomUpdateCallbackProxy(this, listenerOnGameThread)))
						{
							using (AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("builder", new object[1] { androidJavaObject }))
							{
								using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("com.google.games.bridge.RealTimeMessageReceivedListenerProxy", new MessageReceivedListenerProxy(listenerOnGameThread)))
								{
									using (androidJavaObject2.Call<AndroidJavaObject>("setOnMessageReceivedListener", new object[1] { androidJavaObject3 }))
									{
										using (AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("com.google.games.bridge.RoomStatusUpdateCallbackProxy", new RoomStatusUpdateCallbackProxy(this, listenerOnGameThread)))
										{
											using (androidJavaObject2.Call<AndroidJavaObject>("setRoomStatusUpdateCallback", new object[1] { androidJavaObject4 }))
											{
												using (androidJavaObject2.Call<AndroidJavaObject>("setInvitationIdToAccept", new object[1] { invitationId }))
												{
													mRoomConfig = androidJavaObject2.Call<AndroidJavaObject>("build", Array.Empty<object>());
													mListener = listenerOnGameThread;
													using (AndroidJavaObject task = mRtmpClient.Call<AndroidJavaObject>("join", new object[1] { mRoomConfig }))
													{
														AndroidTaskUtils.AddOnFailureListener(task, delegate
														{
															listenerOnGameThread.OnRoomConnected(false);
															CleanSession();
														});
													}
												}
											}
										}
									}
								}
							}
						}
					}
				});
			}
		}

		public void SendMessageToAll(bool reliable, byte[] data)
		{
			if (reliable)
			{
				foreach (Participant connectedParticipant in GetConnectedParticipants())
				{
					SendMessage(true, connectedParticipant.ParticipantId, data);
				}
				return;
			}
			RoomStatus roomStatus = GetRoomStatus();
			if (roomStatus != RoomStatus.Active && roomStatus != RoomStatus.Connecting)
			{
				GooglePlayGames.OurUtils.Logger.d("Sending message is not allowed in this state.");
				return;
			}
			string text = mRoom.Call<string>("getRoomId", Array.Empty<object>());
			using (mRtmpClient.Call<AndroidJavaObject>("sendUnreliableMessageToOthers", new object[2] { data, text }))
			{
			}
		}

		public void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
		{
			SendMessageToAll(reliable, Misc.GetSubsetBytes(data, offset, length));
		}

		public void SendMessage(bool reliable, string participantId, byte[] data)
		{
			RoomStatus roomStatus = GetRoomStatus();
			if (roomStatus != RoomStatus.Active && roomStatus != RoomStatus.Connecting)
			{
				GooglePlayGames.OurUtils.Logger.d("Sending message is not allowed in this state.");
				return;
			}
			if (GetParticipant(participantId) == null)
			{
				GooglePlayGames.OurUtils.Logger.e("Attempted to send message to unknown participant " + participantId);
				return;
			}
			string text = mRoom.Call<string>("getRoomId", Array.Empty<object>());
			if (reliable)
			{
				using (mRtmpClient.Call<AndroidJavaObject>("sendReliableMessage", new object[4] { data, text, participantId, null }))
				{
					return;
				}
			}
			using (mRtmpClient.Call<AndroidJavaObject>("sendUnreliableMessage", new object[3] { data, text, participantId }))
			{
			}
		}

		public void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
		{
			SendMessage(reliable, participantId, Misc.GetSubsetBytes(data, offset, length));
		}

		public List<Participant> GetConnectedParticipants()
		{
			List<Participant> list = new List<Participant>();
			foreach (Participant participant in GetParticipantList())
			{
				if (participant.IsConnectedToRoom)
				{
					list.Add(participant);
				}
			}
			return list;
		}

		private List<Participant> GetParticipantList()
		{
			if (mRoom == null)
			{
				return new List<Participant>();
			}
			List<Participant> list = new List<Participant>();
			using (AndroidJavaObject androidJavaObject = mRoom.Call<AndroidJavaObject>("getParticipants", Array.Empty<object>()))
			{
				int num = androidJavaObject.Call<int>("size", Array.Empty<object>());
				for (int i = 0; i < num; i++)
				{
					using (AndroidJavaObject participant = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { i }))
					{
						list.Add(AndroidJavaConverter.ToParticipant(participant));
					}
				}
				return list;
			}
		}

		public Participant GetSelf()
		{
			foreach (Participant participant in GetParticipantList())
			{
				if (participant.Player != null && participant.Player.id.Equals(mAndroidClient.GetUserId()))
				{
					return participant;
				}
			}
			return null;
		}

		public Participant GetParticipant(string participantId)
		{
			if (GetRoomStatus() != RoomStatus.Active)
			{
				return null;
			}
			try
			{
				using (AndroidJavaObject participant = mRoom.Call<AndroidJavaObject>("getParticipant", new object[1] { participantId }))
				{
					return AndroidJavaConverter.ToParticipant(participant);
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public Invitation GetInvitation()
		{
			return mInvitation;
		}

		public void LeaveRoom()
		{
			if (GetRoomStatus() != RoomStatus.NotCreated)
			{
				using (mRtmpClient.Call<AndroidJavaObject>("leave", new object[2]
				{
					mRoomConfig,
					mRoom.Call<string>("getRoomId", Array.Empty<object>())
				}))
				{
				}
				if (mListener != null)
				{
					mListener.OnRoomConnected(false);
				}
				CleanSession();
			}
		}

		public bool IsRoomConnected()
		{
			return GetRoomStatus() == RoomStatus.Active;
		}

		private RoomStatus GetRoomStatus()
		{
			if (mRoom == null)
			{
				return RoomStatus.NotCreated;
			}
			return (RoomStatus)mRoom.Call<int>("getStatus", Array.Empty<object>());
		}

		public void DeclineInvitation(string invitationId)
		{
			FindInvitation(invitationId, delegate
			{
			}, delegate
			{
				using (mRtmpClient.Call<AndroidJavaObject>("declineInvitation", new object[1] { invitationId }))
				{
				}
			});
		}

		private void FindInvitation(string invitationId, Action<bool> fail, Action<Invitation> callback)
		{
			using (AndroidJavaObject task = mInvitationsClient.Call<AndroidJavaObject>("loadInvitations", Array.Empty<object>()))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						int num = androidJavaObject.Call<int>("getCount", Array.Empty<object>());
						for (int i = 0; i < num; i++)
						{
							Invitation invitation2;
							using (AndroidJavaObject invitation = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { i }))
							{
								invitation2 = AndroidJavaConverter.ToInvitation(invitation);
							}
							if (invitation2.InvitationId == invitationId)
							{
								callback(invitation2);
								return;
							}
						}
						GooglePlayGames.OurUtils.Logger.e("Invitation with ID " + invitationId + " couldn't be found");
						fail(true);
					}
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					GooglePlayGames.OurUtils.Logger.e("Couldn't load invitations.");
					fail(true);
				});
			}
		}

		private void CleanSession()
		{
			lock (mSessionLock)
			{
				mRoom = null;
				mRoomConfig = null;
				mListener = null;
				mInvitation = null;
				mMinPlayersToStart = 0;
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
	}
}
