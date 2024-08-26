using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidTurnBasedMultiplayerClient : ITurnBasedMultiplayerClient
	{
		private class TurnBasedMatchUpdateCallbackProxy : AndroidJavaProxy
		{
			private Action<TurnBasedMatch, bool> mMatchDelegate;

			public TurnBasedMatchUpdateCallbackProxy(Action<TurnBasedMatch, bool> matchDelegate)
				: base("com/google/games/bridge/TurnBasedMatchUpdateCallbackProxy$Callback")
			{
				mMatchDelegate = matchDelegate;
			}

			public void onTurnBasedMatchReceived(AndroidJavaObject turnBasedMatch)
			{
				mMatchDelegate(AndroidJavaConverter.ToTurnBasedMatch(turnBasedMatch), false);
			}

			public void onTurnBasedMatchRemoved(string invitationId)
			{
			}
		}

		private volatile AndroidJavaObject mClient;

		private volatile AndroidClient mAndroidClient;

		private volatile Action<TurnBasedMatch, bool> mMatchDelegate;

		public Action<TurnBasedMatch, bool> MatchDelegate
		{
			get
			{
				return mMatchDelegate;
			}
		}

		public AndroidTurnBasedMultiplayerClient(AndroidClient androidClient, AndroidJavaObject account)
		{
			mAndroidClient = androidClient;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.Games"))
			{
				mClient = androidJavaClass.CallStatic<AndroidJavaObject>("getTurnBasedMultiplayerClient", new object[2]
				{
					AndroidHelperFragment.GetActivity(),
					account
				});
			}
		}

		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, Action<bool, TurnBasedMatch> callback)
		{
			CreateQuickMatch(minOpponents, maxOpponents, variant, 0uL, callback);
		}

		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitmask, Action<bool, TurnBasedMatch> callback)
		{
			callback = ToOnGameThread(callback);
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.multiplayer.turnbased.TurnBasedMatchConfig"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("builder", Array.Empty<object>()))
				{
					using (AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("createAutoMatchCriteria", new object[3]
					{
						(int)minOpponents,
						(int)maxOpponents,
						(long)exclusiveBitmask
					}))
					{
						androidJavaObject.Call<AndroidJavaObject>("setAutoMatchCriteria", new object[1] { androidJavaObject2 });
						if (variant != 0)
						{
							androidJavaObject.Call<AndroidJavaObject>("setVariant", new object[1] { (int)variant });
						}
						using (AndroidJavaObject androidJavaObject3 = androidJavaObject.Call<AndroidJavaObject>("build", Array.Empty<object>()))
						{
							using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("createMatch", new object[1] { androidJavaObject3 }))
							{
								AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject turnBasedMatch)
								{
									callback(true, AndroidJavaConverter.ToTurnBasedMatch(turnBasedMatch));
								});
								AndroidTaskUtils.AddOnFailureListener(task, delegate
								{
									callback(false, null);
								});
							}
						}
					}
				}
			}
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<bool, TurnBasedMatch> callback)
		{
			CreateWithInvitationScreen(minOpponents, maxOpponents, variant, delegate(UIStatus status, TurnBasedMatch match)
			{
				callback(status == UIStatus.Valid, match);
			});
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<UIStatus, TurnBasedMatch> callback)
		{
			callback = ToOnGameThread(callback);
			AndroidHelperFragment.ShowTbmpSelectOpponentsUI(minOpponents, maxOpponents, delegate(UIStatus status, AndroidHelperFragment.InvitationResultHolder result)
			{
				if (status == UIStatus.NotAuthorized)
				{
					mAndroidClient.SignOut(delegate
					{
						callback(status, null);
					});
				}
				else
				{
					if (status == UIStatus.Valid)
					{
						using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.multiplayer.turnbased.TurnBasedMatchConfig"))
						{
							using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("builder", Array.Empty<object>()))
							{
								if (result.MinAutomatchingPlayers > 0)
								{
									using (AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("createAutoMatchCriteria", new object[3] { result.MinAutomatchingPlayers, result.MaxAutomatchingPlayers, 0L }))
									{
										using (androidJavaObject.Call<AndroidJavaObject>("setAutoMatchCriteria", new object[1] { androidJavaObject2 }))
										{
										}
									}
								}
								if (variant != 0)
								{
									using (androidJavaObject.Call<AndroidJavaObject>("setVariant", new object[1] { (int)variant }))
									{
									}
								}
								using (AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("java.util.ArrayList"))
								{
									for (int i = 0; i < result.PlayerIdsToInvite.Count; i++)
									{
										androidJavaObject4.Call<bool>("add", new object[1] { result.PlayerIdsToInvite[i] });
									}
									using (androidJavaObject.Call<AndroidJavaObject>("addInvitedPlayers", new object[1] { androidJavaObject4 }))
									{
									}
								}
								using (AndroidJavaObject androidJavaObject5 = androidJavaObject.Call<AndroidJavaObject>("build", Array.Empty<object>()))
								{
									using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("createMatch", new object[1] { androidJavaObject5 }))
									{
										AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject turnBasedMatch)
										{
											callback(UIStatus.Valid, AndroidJavaConverter.ToTurnBasedMatch(turnBasedMatch));
										});
										AndroidTaskUtils.AddOnFailureListener(task, delegate
										{
											callback(UIStatus.InternalError, null);
										});
										return;
									}
								}
							}
						}
					}
					callback(status, null);
				}
			});
		}

		private AndroidJavaObject StringListToAndroidJavaObject(List<string> list)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.ArrayList");
			for (int i = 0; i < list.Count; i++)
			{
				androidJavaObject.Call<bool>("add", new object[1] { list[i] });
			}
			return androidJavaObject;
		}

		public void GetAllInvitations(Action<Invitation[]> callback)
		{
			callback = ToOnGameThread(callback);
			using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("loadMatchesByStatus", new object[1] { new int[4] { 0, 1, 2, 3 } }))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						using (AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getInvitations", Array.Empty<object>()))
						{
							int num = androidJavaObject2.Call<int>("getCount", Array.Empty<object>());
							Invitation[] array = new Invitation[num];
							for (int i = 0; i < num; i++)
							{
								using (AndroidJavaObject invitation = androidJavaObject2.Call<AndroidJavaObject>("get", new object[1] { i }))
								{
									array[i] = AndroidJavaConverter.ToInvitation(invitation);
								}
							}
							callback(array);
						}
					}
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					callback(null);
				});
			}
		}

		public void GetAllMatches(Action<TurnBasedMatch[]> callback)
		{
			callback = ToOnGameThread(callback);
			using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("loadMatchesByStatus", new object[1] { new int[4] { 0, 1, 2, 3 } }))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						List<TurnBasedMatch> collection;
						using (AndroidJavaObject turnBasedMatchBuffer = androidJavaObject.Call<AndroidJavaObject>("getMyTurnMatches", Array.Empty<object>()))
						{
							collection = CreateTurnBasedMatchList(turnBasedMatchBuffer);
						}
						List<TurnBasedMatch> collection2;
						using (AndroidJavaObject turnBasedMatchBuffer2 = androidJavaObject.Call<AndroidJavaObject>("getTheirTurnMatches", Array.Empty<object>()))
						{
							collection2 = CreateTurnBasedMatchList(turnBasedMatchBuffer2);
						}
						List<TurnBasedMatch> collection3;
						using (AndroidJavaObject turnBasedMatchBuffer3 = androidJavaObject.Call<AndroidJavaObject>("getCompletedMatches", Array.Empty<object>()))
						{
							collection3 = CreateTurnBasedMatchList(turnBasedMatchBuffer3);
						}
						List<TurnBasedMatch> list = new List<TurnBasedMatch>(collection);
						list.AddRange(collection2);
						list.AddRange(collection3);
						callback(list.ToArray());
					}
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					callback(null);
				});
			}
		}

		public void GetMatch(string matchId, Action<bool, TurnBasedMatch> callback)
		{
			callback = ToOnGameThread(callback);
			using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("loadMatch", new object[1] { matchId }))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						if (androidJavaObject == null)
						{
							GooglePlayGames.OurUtils.Logger.e(string.Format("Could not find match {0}", matchId));
							callback(false, null);
						}
						else
						{
							callback(true, AndroidJavaConverter.ToTurnBasedMatch(androidJavaObject));
						}
					}
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					callback(false, null);
				});
			}
		}

		private void GetMatchAndroidJavaObject(string matchId, Action<bool, AndroidJavaObject> callback)
		{
			using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("loadMatch", new object[1] { matchId }))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						if (androidJavaObject == null)
						{
							GooglePlayGames.OurUtils.Logger.e(string.Format("Could not find match {0}", matchId));
							callback(false, null);
						}
						else
						{
							callback(true, androidJavaObject);
						}
					}
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					callback(false, null);
				});
			}
		}

		public void AcceptFromInbox(Action<bool, TurnBasedMatch> callback)
		{
			callback = ToOnGameThread(callback);
			AndroidHelperFragment.ShowInboxUI(delegate(UIStatus status, TurnBasedMatch turnBasedMatch)
			{
				switch (status)
				{
				case UIStatus.NotAuthorized:
					mAndroidClient.SignOut(delegate
					{
						callback(false, null);
					});
					break;
				default:
					callback(false, null);
					break;
				case UIStatus.Valid:
					GooglePlayGames.OurUtils.Logger.d("Passing converted match to user callback:" + turnBasedMatch);
					callback(true, turnBasedMatch);
					break;
				}
			});
		}

		public void AcceptInvitation(string invitationId, Action<bool, TurnBasedMatch> callback)
		{
			callback = ToOnGameThread(callback);
			FindInvitationWithId(invitationId, delegate(Invitation invitation)
			{
				if (invitation == null)
				{
					GooglePlayGames.OurUtils.Logger.e("Could not find invitation with id " + invitationId);
					callback(false, null);
					return;
				}
				using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("acceptInvitation", new object[1] { invitationId }))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject turnBasedMatch)
					{
						callback(true, AndroidJavaConverter.ToTurnBasedMatch(turnBasedMatch));
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						callback(false, null);
					});
				}
			});
		}

		public void RegisterMatchDelegate(MatchDelegate del)
		{
			if (del == null)
			{
				mMatchDelegate = null;
				return;
			}
			mMatchDelegate = ToOnGameThread(delegate(TurnBasedMatch turnBasedMatch, bool autoAccept)
			{
				del(turnBasedMatch, autoAccept);
			});
			TurnBasedMatchUpdateCallbackProxy turnBasedMatchUpdateCallbackProxy = new TurnBasedMatchUpdateCallbackProxy(mMatchDelegate);
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.games.bridge.TurnBasedMatchUpdateCallbackProxy", turnBasedMatchUpdateCallbackProxy);
			using (mClient.Call<AndroidJavaObject>("registerTurnBasedMatchUpdateCallback", new object[1] { androidJavaObject }))
			{
			}
		}

		public void TakeTurn(TurnBasedMatch match, byte[] data, string pendingParticipantId, Action<bool> callback)
		{
			callback = ToOnGameThread(callback);
			FindEqualVersionMatchWithParticipant(match, pendingParticipantId, callback, delegate(Participant pendingParticipant, TurnBasedMatch foundMatch)
			{
				using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("takeTurn", new object[3] { foundMatch.MatchId, data, pendingParticipantId }))
				{
					AndroidTaskUtils.AddOnSuccessListener<AndroidJavaObject>(task, delegate
					{
						callback(true);
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						GooglePlayGames.OurUtils.Logger.d("Taking turn failed");
						callback(false);
					});
				}
			});
		}

		public int GetMaxMatchDataSize()
		{
			return 134217728;
		}

		public void Finish(TurnBasedMatch match, byte[] data, MatchOutcome outcome, Action<bool> callback)
		{
			callback = ToOnGameThread(callback);
			GetMatchAndroidJavaObject(match.MatchId, delegate(bool status, AndroidJavaObject foundMatch)
			{
				if (!status)
				{
					callback(false);
					return;
				}
				using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.util.ArrayList"))
				{
					Dictionary<string, AndroidJavaObject> dictionary = new Dictionary<string, AndroidJavaObject>();
					using (AndroidJavaObject androidJavaObject = foundMatch.Call<AndroidJavaObject>("getParticipants", Array.Empty<object>()))
					{
						int num = androidJavaObject.Call<int>("size", Array.Empty<object>());
						for (int i = 0; i < num; i++)
						{
							try
							{
								using (AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { i }).Call<AndroidJavaObject>("getResult", Array.Empty<object>()))
								{
									string key = androidJavaObject2.Call<string>("getParticipantId", Array.Empty<object>());
									dictionary[key] = androidJavaObject2;
									androidJavaObject3.Call<AndroidJavaObject>("add", new object[1] { androidJavaObject2 });
								}
							}
							catch (Exception)
							{
							}
						}
					}
					foreach (string participantId in outcome.ParticipantIds)
					{
						MatchOutcome.ParticipantResult resultFor = outcome.GetResultFor(participantId);
						uint placementFor = outcome.GetPlacementFor(participantId);
						if (dictionary.ContainsKey(participantId))
						{
							int num2 = dictionary[participantId].Get<int>("result");
							uint num3 = (uint)dictionary[participantId].Get<int>("placing");
							if (resultFor != (MatchOutcome.ParticipantResult)num2 || placementFor != num3)
							{
								GooglePlayGames.OurUtils.Logger.e(string.Format("Attempted to override existing results for participant {0}: Placing {1}, Result {2}", participantId, num3, num2));
								callback(false);
								return;
							}
						}
						else
						{
							using (AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("com.google.android.gms.games.multiplayer.ParticipantResult", participantId, (int)resultFor, (int)placementFor))
							{
								androidJavaObject3.Call<bool>("add", new object[1] { androidJavaObject4 });
							}
						}
					}
					using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("finishMatch", new object[3] { match.MatchId, data, androidJavaObject3 }))
					{
						AndroidTaskUtils.AddOnSuccessListener<AndroidJavaObject>(task, delegate
						{
							callback(true);
						});
						AndroidTaskUtils.AddOnFailureListener(task, delegate
						{
							callback(false);
						});
					}
				}
			});
		}

		public void AcknowledgeFinished(TurnBasedMatch match, Action<bool> callback)
		{
			callback = ToOnGameThread(callback);
			FindEqualVersionMatch(match, delegate(bool success, TurnBasedMatch foundMatch)
			{
				if (!success)
				{
					callback(false);
					return;
				}
				using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("dismissMatch", new object[1] { foundMatch.MatchId }))
				{
					AndroidTaskUtils.AddOnSuccessListener<AndroidJavaObject>(task, delegate
					{
						callback(true);
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						callback(false);
					});
				}
			});
		}

		public void Leave(TurnBasedMatch match, Action<bool> callback)
		{
			callback = ToOnGameThread(callback);
			FindEqualVersionMatch(match, delegate(bool success, TurnBasedMatch foundMatch)
			{
				if (!success)
				{
					callback(false);
				}
				using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("leaveMatch", new object[1] { match.MatchId }))
				{
					AndroidTaskUtils.AddOnSuccessListener<AndroidJavaObject>(task, delegate
					{
						callback(true);
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						callback(false);
					});
				}
			});
		}

		public void LeaveDuringTurn(TurnBasedMatch match, string pendingParticipantId, Action<bool> callback)
		{
			callback = ToOnGameThread(callback);
			FindEqualVersionMatchWithParticipant(match, pendingParticipantId, callback, delegate
			{
				using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("leaveMatchDuringTurn", new object[2] { match.MatchId, pendingParticipantId }))
				{
					AndroidTaskUtils.AddOnSuccessListener<AndroidJavaObject>(task, delegate
					{
						callback(true);
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						callback(false);
					});
				}
			});
		}

		public void Cancel(TurnBasedMatch match, Action<bool> callback)
		{
			callback = ToOnGameThread(callback);
			FindEqualVersionMatch(match, delegate(bool success, TurnBasedMatch foundMatch)
			{
				if (!success)
				{
					callback(false);
					return;
				}
				using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("cancelMatch", new object[1] { match.MatchId }))
				{
					AndroidTaskUtils.AddOnSuccessListener<string>(task, delegate
					{
						callback(true);
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						callback(false);
					});
				}
			});
		}

		public void Dismiss(TurnBasedMatch match)
		{
			FindEqualVersionMatch(match, delegate(bool success, TurnBasedMatch foundMatch)
			{
				if (success)
				{
					using (mClient.Call<AndroidJavaObject>("dismissMatch", new object[1] { match.MatchId }))
					{
					}
				}
			});
		}

		public void Rematch(TurnBasedMatch match, Action<bool, TurnBasedMatch> callback)
		{
			callback = ToOnGameThread(callback);
			FindEqualVersionMatch(match, delegate(bool success, TurnBasedMatch foundMatch)
			{
				if (!success)
				{
					callback(false, null);
					return;
				}
				using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("rematch", new object[1] { match.MatchId }))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject turnBasedMatch)
					{
						callback(true, AndroidJavaConverter.ToTurnBasedMatch(turnBasedMatch));
					});
					AndroidTaskUtils.AddOnFailureListener(task, delegate
					{
						callback(false, null);
					});
				}
			});
		}

		public void DeclineInvitation(string invitationId)
		{
			FindInvitationWithId(invitationId, delegate(Invitation invitation)
			{
				if (invitation == null)
				{
					return;
				}
				using (mClient.Call<AndroidJavaObject>("declineInvitation", new object[1] { invitationId }))
				{
				}
			});
		}

		private void FindInvitationWithId(string invitationId, Action<Invitation> callback)
		{
			using (AndroidJavaObject task = mClient.Call<AndroidJavaObject>("loadMatchesByStatus", new object[1] { new int[4] { 0, 1, 2, 3 } }))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						using (AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getInvitations", Array.Empty<object>()))
						{
							int num = androidJavaObject2.Call<int>("getCount", Array.Empty<object>());
							for (int i = 0; i < num; i++)
							{
								Invitation invitation = AndroidJavaConverter.ToInvitation(androidJavaObject2.Call<AndroidJavaObject>("get", new object[1] { i }));
								if (invitation.InvitationId == invitationId)
								{
									callback(invitation);
									return;
								}
							}
						}
					}
					callback(null);
				});
				AndroidTaskUtils.AddOnFailureListener(task, delegate
				{
					callback(null);
				});
			}
		}

		private void FindEqualVersionMatch(TurnBasedMatch match, Action<bool, TurnBasedMatch> callback)
		{
			GetMatch(match.MatchId, delegate(bool success, TurnBasedMatch foundMatch)
			{
				if (!success)
				{
					callback(false, null);
				}
				else if (match.Version != foundMatch.Version)
				{
					GooglePlayGames.OurUtils.Logger.e(string.Format("Attempted to update a stale version of the match. Expected version was {0} but current version is {1}.", match.Version, foundMatch.Version));
					callback(false, null);
				}
				else
				{
					callback(true, foundMatch);
				}
			});
		}

		private void FindEqualVersionMatchWithParticipant(TurnBasedMatch match, string participantId, Action<bool> onFailure, Action<Participant, TurnBasedMatch> onFoundParticipantAndMatch)
		{
			FindEqualVersionMatch(match, delegate(bool success, TurnBasedMatch foundMatch)
			{
				if (!success)
				{
					onFailure(true);
				}
				if (participantId == null)
				{
					onFoundParticipantAndMatch(CreateAutomatchingSentinel(), foundMatch);
				}
				else
				{
					Participant participant = foundMatch.GetParticipant(participantId);
					if (participant == null)
					{
						GooglePlayGames.OurUtils.Logger.e(string.Format("Located match {0} but desired participant with ID {1} could not be found", match.MatchId, participantId));
						onFailure(false);
					}
					else
					{
						onFoundParticipantAndMatch(participant, foundMatch);
					}
				}
			});
		}

		private Participant CreateAutomatchingSentinel()
		{
			return new Participant("", "", Participant.ParticipantStatus.NotInvitedYet, new GooglePlayGames.BasicApi.Multiplayer.Player("", "", ""), false);
		}

		private List<TurnBasedMatch> CreateTurnBasedMatchList(AndroidJavaObject turnBasedMatchBuffer)
		{
			List<TurnBasedMatch> list = new List<TurnBasedMatch>();
			int num = turnBasedMatchBuffer.Call<int>("getCount", Array.Empty<object>());
			for (int i = 0; i < num; i++)
			{
				TurnBasedMatch item = AndroidJavaConverter.ToTurnBasedMatch(turnBasedMatchBuffer.Call<AndroidJavaObject>("get", new object[1] { i }));
				list.Add(item);
			}
			return list;
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
