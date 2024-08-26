using System;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi.Video;
using GooglePlayGames.OurUtils;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.Android
{
	public class AndroidClient : IPlayGamesClient
	{
		private enum AuthState
		{
			Unauthenticated = 0,
			Authenticated = 1
		}

		private class InvitationCallbackProxy : AndroidJavaProxy
		{
			private Action<Invitation, bool> mInvitationDelegate;

			public InvitationCallbackProxy(Action<Invitation, bool> invitationDelegate)
				: base("com/google/games/bridge/InvitationCallbackProxy$Callback")
			{
				mInvitationDelegate = invitationDelegate;
			}

			public void onInvitationReceived(AndroidJavaObject invitation)
			{
				mInvitationDelegate(AndroidJavaConverter.ToInvitation(invitation), false);
			}

			public void onInvitationRemoved(string invitationId)
			{
			}
		}

		private readonly object GameServicesLock = new object();

		private readonly object AuthStateLock = new object();

		private readonly PlayGamesClientConfiguration mConfiguration;

		private volatile AndroidTurnBasedMultiplayerClient mTurnBasedClient;

		private volatile IRealTimeMultiplayerClient mRealTimeClient;

		private volatile ISavedGameClient mSavedGameClient;

		private volatile IEventsClient mEventsClient;

		private volatile IVideoClient mVideoClient;

		private volatile AndroidTokenClient mTokenClient;

		private volatile Action<Invitation, bool> mInvitationDelegate;

		private volatile GooglePlayGames.BasicApi.Multiplayer.Player mUser;

		private volatile AuthState mAuthState;

		private AndroidJavaClass mGamesClass = new AndroidJavaClass("com.google.android.gms.games.Games");

		private static string TasksClassName = "com.google.android.gms.tasks.Tasks";

		private AndroidJavaObject mInvitationCallback;

		private readonly int mLeaderboardMaxResults = 25;

		internal AndroidClient(PlayGamesClientConfiguration configuration)
		{
			PlayGamesHelperObject.CreateObject();
			mConfiguration = Misc.CheckNotNull(configuration);
			RegisterInvitationDelegate(configuration.InvitationDelegate);
		}

		public void Authenticate(bool silent, Action<SignInStatus> callback)
		{
			lock (AuthStateLock)
			{
				if (mAuthState == AuthState.Authenticated)
				{
					Debug.Log("Already authenticated.");
					InvokeCallbackOnGameThread(callback, SignInStatus.Success);
					return;
				}
			}
			InitializeTokenClient();
			Debug.Log("Starting Auth with token client.");
			mTokenClient.FetchTokens(silent, delegate(int result)
			{
				bool num = result == 0;
				InitializeGameServices();
				if (num)
				{
					using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.util.ArrayList"))
					{
						if (mInvitationDelegate != null)
						{
							mInvitationCallback = new AndroidJavaObject("com.google.games.bridge.InvitationCallbackProxy", new InvitationCallbackProxy(mInvitationDelegate));
							using (AndroidJavaObject androidJavaObject = getInvitationsClient())
							{
								using (AndroidJavaObject androidJavaObject3 = androidJavaObject.Call<AndroidJavaObject>("registerInvitationCallback", new object[1] { mInvitationCallback }))
								{
									androidJavaObject2.Call<bool>("add", new object[1] { androidJavaObject3 });
								}
							}
						}
						AndroidJavaObject taskGetPlayer = getPlayersClient().Call<AndroidJavaObject>("getCurrentPlayer", Array.Empty<object>());
						AndroidJavaObject taskGetActivationHint = getGamesClient().Call<AndroidJavaObject>("getActivationHint", Array.Empty<object>());
						AndroidJavaObject taskIsCaptureSupported = getVideosClient().Call<AndroidJavaObject>("isCaptureSupported", Array.Empty<object>());
						if (!mConfiguration.IsHidingPopups)
						{
							AndroidJavaObject androidJavaObject5;
							using (AndroidJavaObject androidJavaObject4 = AndroidHelperFragment.GetDefaultPopupView())
							{
								androidJavaObject5 = getGamesClient().Call<AndroidJavaObject>("setViewForPopups", new object[1] { androidJavaObject4 });
							}
							androidJavaObject2.Call<bool>("add", new object[1] { androidJavaObject5 });
						}
						androidJavaObject2.Call<bool>("add", new object[1] { taskGetPlayer });
						androidJavaObject2.Call<bool>("add", new object[1] { taskGetActivationHint });
						androidJavaObject2.Call<bool>("add", new object[1] { taskIsCaptureSupported });
						using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(TasksClassName))
						{
							using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>("whenAll", new object[1] { androidJavaObject2 }))
							{
								AndroidTaskUtils.AddOnCompleteListener(task, delegate(AndroidJavaObject completeTask)
								{
									if (completeTask.Call<bool>("isSuccessful", Array.Empty<object>()))
									{
										using (AndroidJavaObject player = taskGetPlayer.Call<AndroidJavaObject>("getResult", Array.Empty<object>()))
										{
											mUser = AndroidJavaConverter.ToPlayer(player);
										}
										AndroidJavaObject account = mTokenClient.GetAccount();
										lock (GameServicesLock)
										{
											mSavedGameClient = new AndroidSavedGameClient(this, account);
											mEventsClient = new AndroidEventsClient(account);
											bool isCaptureSupported;
											using (AndroidJavaObject androidJavaObject6 = taskIsCaptureSupported.Call<AndroidJavaObject>("getResult", Array.Empty<object>()))
											{
												isCaptureSupported = androidJavaObject6.Call<bool>("booleanValue", Array.Empty<object>());
											}
											mVideoClient = new AndroidVideoClient(isCaptureSupported, account);
											mRealTimeClient = new AndroidRealTimeMultiplayerClient(this, account);
											mTurnBasedClient = new AndroidTurnBasedMultiplayerClient(this, account);
											mTurnBasedClient.RegisterMatchDelegate(mConfiguration.MatchDelegate);
										}
										mAuthState = AuthState.Authenticated;
										InvokeCallbackOnGameThread(callback, SignInStatus.Success);
										GooglePlayGames.OurUtils.Logger.d("Authentication succeeded");
										try
										{
											using (AndroidJavaObject androidJavaObject7 = taskGetActivationHint.Call<AndroidJavaObject>("getResult", Array.Empty<object>()))
											{
												if (mInvitationDelegate != null)
												{
													try
													{
														using (AndroidJavaObject invitation = androidJavaObject7.Call<AndroidJavaObject>("getParcelable", new object[1] { "invitation" }))
														{
															Invitation arg = AndroidJavaConverter.ToInvitation(invitation);
															mInvitationDelegate(arg, true);
														}
													}
													catch (Exception)
													{
													}
												}
												if (mTurnBasedClient.MatchDelegate != null)
												{
													try
													{
														using (AndroidJavaObject turnBasedMatch = androidJavaObject7.Call<AndroidJavaObject>("getParcelable", new object[1] { "turn_based_match" }))
														{
															TurnBasedMatch arg2 = AndroidJavaConverter.ToTurnBasedMatch(turnBasedMatch);
															mTurnBasedClient.MatchDelegate(arg2, true);
														}
													}
													catch (Exception)
													{
													}
												}
											}
										}
										catch (Exception)
										{
										}
										LoadAchievements(delegate
										{
										});
									}
									else
									{
										SignOut();
										if (!completeTask.Call<bool>("isCanceled", Array.Empty<object>()))
										{
											using (AndroidJavaObject androidJavaObject8 = completeTask.Call<AndroidJavaObject>("getException", Array.Empty<object>()))
											{
												GooglePlayGames.OurUtils.Logger.e("Authentication failed - " + androidJavaObject8.Call<string>("toString", Array.Empty<object>()));
												InvokeCallbackOnGameThread(callback, SignInStatus.InternalError);
												return;
											}
										}
										InvokeCallbackOnGameThread(callback, SignInStatus.Canceled);
									}
								});
								return;
							}
						}
					}
				}
				lock (AuthStateLock)
				{
					Debug.Log("Returning an error code.");
					InvokeCallbackOnGameThread(callback, SignInHelper.ToSignInStatus(result));
				}
			});
		}

		private static Action<T> AsOnGameThreadCallback<T>(Action<T> callback)
		{
			if (callback == null)
			{
				return delegate
				{
				};
			}
			return delegate(T result)
			{
				InvokeCallbackOnGameThread(callback, result);
			};
		}

		private static void InvokeCallbackOnGameThread(Action callback)
		{
			if (callback != null)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback();
				});
			}
		}

		private static void InvokeCallbackOnGameThread<T>(Action<T> callback, T data)
		{
			if (callback != null)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(data);
				});
			}
		}

		private static Action<T1, T2> AsOnGameThreadCallback<T1, T2>(Action<T1, T2> toInvokeOnGameThread)
		{
			return delegate(T1 result1, T2 result2)
			{
				if (toInvokeOnGameThread != null)
				{
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						toInvokeOnGameThread(result1, result2);
					});
				}
			};
		}

		private static void InvokeCallbackOnGameThread<T1, T2>(Action<T1, T2> callback, T1 t1, T2 t2)
		{
			if (callback != null)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(t1, t2);
				});
			}
		}

		private void InitializeGameServices()
		{
			if (mTokenClient == null)
			{
				InitializeTokenClient();
			}
		}

		private void InitializeTokenClient()
		{
			if (mTokenClient == null)
			{
				mTokenClient = new AndroidTokenClient();
				if (!GameInfo.WebClientIdInitialized() && (mConfiguration.IsRequestingIdToken || mConfiguration.IsRequestingAuthCode))
				{
					GooglePlayGames.OurUtils.Logger.e("Server Auth Code and ID Token require web clientId to configured.");
				}
				string[] scopes = mConfiguration.Scopes;
				mTokenClient.SetWebClientId("1035200774300-6qhvcak2te0q2n0kcc3on3g5it9a3v3j.apps.googleusercontent.com");
				mTokenClient.SetRequestAuthCode(mConfiguration.IsRequestingAuthCode, mConfiguration.IsForcingRefresh);
				mTokenClient.SetRequestEmail(mConfiguration.IsRequestingEmail);
				mTokenClient.SetRequestIdToken(mConfiguration.IsRequestingIdToken);
				mTokenClient.SetHidePopups(mConfiguration.IsHidingPopups);
				mTokenClient.AddOauthScopes("https://www.googleapis.com/auth/games_lite");
				if (mConfiguration.EnableSavedGames)
				{
					mTokenClient.AddOauthScopes("https://www.googleapis.com/auth/drive.appdata");
				}
				mTokenClient.AddOauthScopes(scopes);
				mTokenClient.SetAccountName(mConfiguration.AccountName);
			}
		}

		public string GetUserEmail()
		{
			if (!IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			return mTokenClient.GetEmail();
		}

		public string GetIdToken()
		{
			if (!IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			return mTokenClient.GetIdToken();
		}

		public string GetServerAuthCode()
		{
			if (!IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			return mTokenClient.GetAuthCode();
		}

		public void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback)
		{
			mTokenClient.GetAnotherServerAuthCode(reAuthenticateIfNeeded, AsOnGameThreadCallback(callback));
		}

		public bool IsAuthenticated()
		{
			lock (AuthStateLock)
			{
				return mAuthState == AuthState.Authenticated;
			}
		}

		public void LoadFriends(Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.d("Cannot loadFriends when not authenticated");
				InvokeCallbackOnGameThread(callback, false);
			}
			else
			{
				InvokeCallbackOnGameThread(callback, true);
			}
		}

		public IUserProfile[] GetFriends()
		{
			return new IUserProfile[0];
		}

		public void SignOut()
		{
			SignOut(null);
		}

		public void SignOut(Action uiCallback)
		{
			if (mTokenClient == null)
			{
				InvokeCallbackOnGameThread(uiCallback);
				return;
			}
			if (mInvitationCallback != null)
			{
				using (AndroidJavaObject androidJavaObject = getInvitationsClient())
				{
					using (AndroidJavaObject task = androidJavaObject.Call<AndroidJavaObject>("unregisterInvitationCallback", new object[1] { mInvitationCallback }))
					{
						AndroidTaskUtils.AddOnCompleteListener<AndroidJavaObject>(task, delegate
						{
							mInvitationCallback = null;
							mTokenClient.Signout();
							mAuthState = AuthState.Unauthenticated;
							if (uiCallback != null)
							{
								InvokeCallbackOnGameThread(uiCallback);
							}
						});
					}
				}
			}
			else
			{
				mTokenClient.Signout();
				mAuthState = AuthState.Unauthenticated;
				if (uiCallback != null)
				{
					InvokeCallbackOnGameThread(uiCallback);
				}
			}
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				SignInHelper.SetPromptUiSignIn(true);
			});
		}

		public string GetUserId()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.id;
		}

		public string GetUserDisplayName()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.userName;
		}

		public string GetUserImageUrl()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.AvatarURL;
		}

		public void SetGravityForPopups(Gravity gravity)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.d("Cannot call SetGravityForPopups when not authenticated");
			}
			using (AndroidJavaObject androidJavaObject = getGamesClient())
			{
				using (androidJavaObject.Call<AndroidJavaObject>("setGravityForPopups", new object[1] { (int)(gravity | Gravity.CENTER_HORIZONTAL) }))
				{
				}
			}
		}

		public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			using (AndroidJavaObject androidJavaObject = getPlayerStatsClient())
			{
				using (AndroidJavaObject task = androidJavaObject.Call<AndroidJavaObject>("loadPlayerStats", new object[1] { false }))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
					{
						using (AndroidJavaObject androidJavaObject2 = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
						{
							int numberOfPurchases = androidJavaObject2.Call<int>("getNumberOfPurchases", Array.Empty<object>());
							float avgSessionLength = androidJavaObject2.Call<float>("getAverageSessionLength", Array.Empty<object>());
							int daysSinceLastPlayed = androidJavaObject2.Call<int>("getDaysSinceLastPlayed", Array.Empty<object>());
							int numberOfSessions = androidJavaObject2.Call<int>("getNumberOfSessions", Array.Empty<object>());
							float sessPercentile = androidJavaObject2.Call<float>("getSessionPercentile", Array.Empty<object>());
							float spendPercentile = androidJavaObject2.Call<float>("getSpendPercentile", Array.Empty<object>());
							float spendProbability = androidJavaObject2.Call<float>("getSpendProbability", Array.Empty<object>());
							float churnProbability = androidJavaObject2.Call<float>("getChurnProbability", Array.Empty<object>());
							float highSpenderProbability = androidJavaObject2.Call<float>("getHighSpenderProbability", Array.Empty<object>());
							float totalSpendNext28Days = androidJavaObject2.Call<float>("getTotalSpendNext28Days", Array.Empty<object>());
							PlayerStats t2 = new PlayerStats(numberOfPurchases, avgSessionLength, daysSinceLastPlayed, numberOfSessions, sessPercentile, spendPercentile, spendProbability, churnProbability, highSpenderProbability, totalSpendNext28Days);
							InvokeCallbackOnGameThread(callback, CommonStatusCodes.Success, t2);
						}
					});
					AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject e)
					{
						Debug.Log("GetPlayerStats failed: " + e.Call<string>("toString", Array.Empty<object>()));
						CommonStatusCodes t = (IsAuthenticated() ? CommonStatusCodes.InternalError : CommonStatusCodes.SignInRequired);
						InvokeCallbackOnGameThread(callback, t, new PlayerStats());
					});
				}
			}
		}

		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			if (!IsAuthenticated())
			{
				InvokeCallbackOnGameThread(callback, new IUserProfile[0]);
				return;
			}
			using (AndroidJavaObject androidJavaObject = getPlayersClient())
			{
				object countLock = new object();
				int count = userIds.Length;
				int resultCount = 0;
				IUserProfile[] users = new IUserProfile[count];
				int i = 0;
				while (i < count)
				{
					using (AndroidJavaObject task = androidJavaObject.Call<AndroidJavaObject>("loadPlayer", new object[1] { userIds[i] }))
					{
						AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
						{
							using (AndroidJavaObject androidJavaObject2 = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
							{
								string text = androidJavaObject2.Call<string>("getPlayerId", Array.Empty<object>());
								for (int j = 0; j < count; j++)
								{
									if (text == userIds[j])
									{
										users[j] = AndroidJavaConverter.ToPlayer(androidJavaObject2);
										break;
									}
								}
								lock (countLock)
								{
									int num3 = resultCount + 1;
									resultCount = num3;
									if (resultCount == count)
									{
										InvokeCallbackOnGameThread(callback, users);
									}
								}
							}
						});
						AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject exception)
						{
							Debug.Log("LoadUsers failed for index " + i + " with: " + exception.Call<string>("toString", Array.Empty<object>()));
							lock (countLock)
							{
								int num2 = resultCount + 1;
								resultCount = num2;
								if (resultCount == count)
								{
									InvokeCallbackOnGameThread(callback, users);
								}
							}
						});
					}
					int num = i + 1;
					i = num;
				}
			}
		}

		public void LoadAchievements(Action<Achievement[]> callback)
		{
			using (AndroidJavaObject androidJavaObject = getAchievementsClient())
			{
				using (AndroidJavaObject task = androidJavaObject.Call<AndroidJavaObject>("load", new object[1] { false }))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
					{
						using (AndroidJavaObject androidJavaObject2 = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
						{
							int num = androidJavaObject2.Call<int>("getCount", Array.Empty<object>());
							Achievement[] array = new Achievement[num];
							for (int i = 0; i < num; i++)
							{
								Achievement achievement = new Achievement();
								using (AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("get", new object[1] { i }))
								{
									achievement.Id = androidJavaObject3.Call<string>("getAchievementId", Array.Empty<object>());
									achievement.Description = androidJavaObject3.Call<string>("getDescription", Array.Empty<object>());
									achievement.Name = androidJavaObject3.Call<string>("getName", Array.Empty<object>());
									achievement.Points = androidJavaObject3.Call<ulong>("getXpValue", Array.Empty<object>());
									long milliseconds = androidJavaObject3.Call<long>("getLastUpdatedTimestamp", Array.Empty<object>());
									achievement.LastModifiedTime = AndroidJavaConverter.ToDateTime(milliseconds);
									achievement.RevealedImageUrl = androidJavaObject3.Call<string>("getRevealedImageUrl", Array.Empty<object>());
									achievement.UnlockedImageUrl = androidJavaObject3.Call<string>("getUnlockedImageUrl", Array.Empty<object>());
									achievement.IsIncremental = androidJavaObject3.Call<int>("getType", Array.Empty<object>()) == 1;
									if (achievement.IsIncremental)
									{
										achievement.CurrentSteps = androidJavaObject3.Call<int>("getCurrentSteps", Array.Empty<object>());
										achievement.TotalSteps = androidJavaObject3.Call<int>("getTotalSteps", Array.Empty<object>());
									}
									int num2 = androidJavaObject3.Call<int>("getState", Array.Empty<object>());
									achievement.IsUnlocked = num2 == 0;
									achievement.IsRevealed = num2 == 1;
								}
								array[i] = achievement;
							}
							androidJavaObject2.Call("release");
							InvokeCallbackOnGameThread(callback, array);
						}
					});
					AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject exception)
					{
						Debug.Log("LoadAchievements failed: " + exception.Call<string>("toString", Array.Empty<object>()));
						InvokeCallbackOnGameThread(callback, new Achievement[0]);
					});
				}
			}
		}

		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				InvokeCallbackOnGameThread(callback, false);
				return;
			}
			using (AndroidJavaObject androidJavaObject = getAchievementsClient())
			{
				androidJavaObject.Call("unlock", achId);
				InvokeCallbackOnGameThread(callback, true);
			}
		}

		public void RevealAchievement(string achId, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				InvokeCallbackOnGameThread(callback, false);
				return;
			}
			using (AndroidJavaObject androidJavaObject = getAchievementsClient())
			{
				androidJavaObject.Call("reveal", achId);
				InvokeCallbackOnGameThread(callback, true);
			}
		}

		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				InvokeCallbackOnGameThread(callback, false);
				return;
			}
			using (AndroidJavaObject androidJavaObject = getAchievementsClient())
			{
				androidJavaObject.Call("increment", achId, steps);
				InvokeCallbackOnGameThread(callback, true);
			}
		}

		public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				InvokeCallbackOnGameThread(callback, false);
				return;
			}
			using (AndroidJavaObject androidJavaObject = getAchievementsClient())
			{
				androidJavaObject.Call("setSteps", achId, steps);
				InvokeCallbackOnGameThread(callback, true);
			}
		}

		public void ShowAchievementsUI(Action<UIStatus> callback)
		{
			if (!IsAuthenticated())
			{
				InvokeCallbackOnGameThread(callback, UIStatus.NotAuthorized);
			}
			else
			{
				AndroidHelperFragment.ShowAchievementsUI(GetUiSignOutCallbackOnGameThread(callback));
			}
		}

		public int LeaderboardMaxResults()
		{
			return mLeaderboardMaxResults;
		}

		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
		{
			if (!IsAuthenticated())
			{
				InvokeCallbackOnGameThread(callback, UIStatus.NotAuthorized);
			}
			else if (leaderboardId == null)
			{
				AndroidHelperFragment.ShowAllLeaderboardsUI(GetUiSignOutCallbackOnGameThread(callback));
			}
			else
			{
				AndroidHelperFragment.ShowLeaderboardUI(leaderboardId, span, GetUiSignOutCallbackOnGameThread(callback));
			}
		}

		private void AddOnFailureListenerWithSignOut(AndroidJavaObject task, Action<AndroidJavaObject> callback)
		{
			AndroidTaskUtils.AddOnFailureListener(task, delegate(AndroidJavaObject exception)
			{
				int num = exception.Call<int>("getStatusCode", Array.Empty<object>());
				if (num == 4 || num == 26502)
				{
					SignOut();
				}
				callback(exception);
			});
		}

		private Action<UIStatus> GetUiSignOutCallbackOnGameThread(Action<UIStatus> callback)
		{
			return AsOnGameThreadCallback(delegate(UIStatus status)
			{
				if (status == UIStatus.NotAuthorized)
				{
					SignOut(delegate
					{
						if (callback != null)
						{
							callback(status);
						}
					});
				}
				else if (callback != null)
				{
					callback(status);
				}
			});
		}

		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			using (AndroidJavaObject androidJavaObject = getLeaderboardsClient())
			{
				string methodName = ((start == LeaderboardStart.TopScores) ? "loadTopScores" : "loadPlayerCenteredScores");
				using (AndroidJavaObject task = androidJavaObject.Call<AndroidJavaObject>(methodName, new object[4]
				{
					leaderboardId,
					AndroidJavaConverter.ToLeaderboardVariantTimeSpan(timeSpan),
					AndroidJavaConverter.ToLeaderboardVariantCollection(collection),
					rowCount
				}))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
					{
						using (AndroidJavaObject androidJavaObject2 = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
						{
							InvokeCallbackOnGameThread(callback, CreateLeaderboardScoreData(leaderboardId, collection, timeSpan, (!annotatedData.Call<bool>("isStale", Array.Empty<object>())) ? ResponseStatus.Success : ResponseStatus.SuccessWithStale, androidJavaObject2));
							androidJavaObject2.Call("release");
						}
					});
					AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject exception)
					{
						Debug.Log("LoadScores failed: " + exception.Call<string>("toString", Array.Empty<object>()));
						InvokeCallbackOnGameThread(callback, new LeaderboardScoreData(leaderboardId, ResponseStatus.InternalError));
					});
				}
			}
		}

		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			using (AndroidJavaObject androidJavaObject = getLeaderboardsClient())
			{
				using (AndroidJavaObject task = androidJavaObject.Call<AndroidJavaObject>("loadMoreScores", new object[3]
				{
					token.InternalObject,
					rowCount,
					AndroidJavaConverter.ToPageDirection(token.Direction)
				}))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
					{
						using (AndroidJavaObject androidJavaObject2 = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
						{
							InvokeCallbackOnGameThread(callback, CreateLeaderboardScoreData(token.LeaderboardId, token.Collection, token.TimeSpan, (!annotatedData.Call<bool>("isStale", Array.Empty<object>())) ? ResponseStatus.Success : ResponseStatus.SuccessWithStale, androidJavaObject2));
							androidJavaObject2.Call("release");
						}
					});
					AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject exception)
					{
						Debug.Log("LoadMoreScores failed: " + exception.Call<string>("toString", Array.Empty<object>()));
						InvokeCallbackOnGameThread(callback, new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.InternalError));
					});
				}
			}
		}

		private LeaderboardScoreData CreateLeaderboardScoreData(string leaderboardId, LeaderboardCollection collection, LeaderboardTimeSpan timespan, ResponseStatus status, AndroidJavaObject leaderboardScoresJava)
		{
			LeaderboardScoreData leaderboardScoreData = new LeaderboardScoreData(leaderboardId, status);
			AndroidJavaObject androidJavaObject = leaderboardScoresJava.Call<AndroidJavaObject>("getScores", Array.Empty<object>());
			int num = androidJavaObject.Call<int>("getCount", Array.Empty<object>());
			for (int i = 0; i < num; i++)
			{
				using (AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { i }))
				{
					DateTime date = AndroidJavaConverter.ToDateTime(androidJavaObject2.Call<long>("getTimestampMillis", Array.Empty<object>()));
					ulong rank = (ulong)androidJavaObject2.Call<long>("getRank", Array.Empty<object>());
					string playerId = "";
					using (AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getScoreHolder", Array.Empty<object>()))
					{
						playerId = androidJavaObject3.Call<string>("getPlayerId", Array.Empty<object>());
					}
					ulong value = (ulong)androidJavaObject2.Call<long>("getRawScore", Array.Empty<object>());
					string metadata = androidJavaObject2.Call<string>("getScoreTag", Array.Empty<object>());
					leaderboardScoreData.AddScore(new PlayGamesScore(date, leaderboardId, rank, playerId, value, metadata));
				}
			}
			leaderboardScoreData.NextPageToken = new ScorePageToken(androidJavaObject, leaderboardId, collection, timespan, ScorePageDirection.Forward);
			leaderboardScoreData.PrevPageToken = new ScorePageToken(androidJavaObject, leaderboardId, collection, timespan, ScorePageDirection.Backward);
			using (AndroidJavaObject androidJavaObject4 = leaderboardScoresJava.Call<AndroidJavaObject>("getLeaderboard", Array.Empty<object>()))
			{
				using (AndroidJavaObject androidJavaObject5 = androidJavaObject4.Call<AndroidJavaObject>("getVariants", Array.Empty<object>()))
				{
					using (AndroidJavaObject androidJavaObject6 = androidJavaObject5.Call<AndroidJavaObject>("get", new object[1] { 0 }))
					{
						leaderboardScoreData.Title = androidJavaObject4.Call<string>("getDisplayName", Array.Empty<object>());
						if (androidJavaObject6.Call<bool>("hasPlayerInfo", Array.Empty<object>()))
						{
							DateTime date2 = AndroidJavaConverter.ToDateTime(0L);
							ulong rank2 = (ulong)androidJavaObject6.Call<long>("getPlayerRank", Array.Empty<object>());
							ulong value2 = (ulong)androidJavaObject6.Call<long>("getRawPlayerScore", Array.Empty<object>());
							string metadata2 = androidJavaObject6.Call<string>("getPlayerScoreTag", Array.Empty<object>());
							leaderboardScoreData.PlayerScore = new PlayGamesScore(date2, leaderboardId, rank2, mUser.id, value2, metadata2);
						}
						leaderboardScoreData.ApproximateCount = (ulong)androidJavaObject6.Call<long>("getNumScores", Array.Empty<object>());
						return leaderboardScoreData;
					}
				}
			}
		}

		public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				InvokeCallbackOnGameThread(callback, false);
			}
			using (AndroidJavaObject androidJavaObject = getLeaderboardsClient())
			{
				androidJavaObject.Call("submitScore", leaderboardId, score);
				InvokeCallbackOnGameThread(callback, true);
			}
		}

		public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				InvokeCallbackOnGameThread(callback, false);
			}
			using (AndroidJavaObject androidJavaObject = getLeaderboardsClient())
			{
				androidJavaObject.Call("submitScore", leaderboardId, score, metadata);
				InvokeCallbackOnGameThread(callback, true);
			}
		}

		public void RequestPermissions(string[] scopes, Action<SignInStatus> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			mTokenClient.RequestPermissions(scopes, delegate(SignInStatus code)
			{
				UpdateClients();
				callback(code);
			});
		}

		private void UpdateClients()
		{
			lock (GameServicesLock)
			{
				AndroidJavaObject account = mTokenClient.GetAccount();
				mSavedGameClient = new AndroidSavedGameClient(this, account);
				mEventsClient = new AndroidEventsClient(account);
				mVideoClient = new AndroidVideoClient(mVideoClient.IsCaptureSupported(), account);
				mRealTimeClient = new AndroidRealTimeMultiplayerClient(this, account);
				mTurnBasedClient = new AndroidTurnBasedMultiplayerClient(this, account);
				mTurnBasedClient.RegisterMatchDelegate(mConfiguration.MatchDelegate);
			}
		}

		public bool HasPermissions(string[] scopes)
		{
			return mTokenClient.HasPermissions(scopes);
		}

		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			if (!IsAuthenticated())
			{
				return null;
			}
			lock (GameServicesLock)
			{
				return mRealTimeClient;
			}
		}

		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			lock (GameServicesLock)
			{
				return mTurnBasedClient;
			}
		}

		public ISavedGameClient GetSavedGameClient()
		{
			lock (GameServicesLock)
			{
				return mSavedGameClient;
			}
		}

		public IEventsClient GetEventsClient()
		{
			lock (GameServicesLock)
			{
				return mEventsClient;
			}
		}

		public IVideoClient GetVideoClient()
		{
			lock (GameServicesLock)
			{
				return mVideoClient;
			}
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
		{
			if (invitationDelegate == null)
			{
				mInvitationDelegate = null;
				return;
			}
			mInvitationDelegate = AsOnGameThreadCallback(delegate(Invitation invitation, bool autoAccept)
			{
				invitationDelegate(invitation, autoAccept);
			});
		}

		private AndroidJavaObject getAchievementsClient()
		{
			return mGamesClass.CallStatic<AndroidJavaObject>("getAchievementsClient", new object[2]
			{
				AndroidHelperFragment.GetActivity(),
				mTokenClient.GetAccount()
			});
		}

		private AndroidJavaObject getGamesClient()
		{
			return mGamesClass.CallStatic<AndroidJavaObject>("getGamesClient", new object[2]
			{
				AndroidHelperFragment.GetActivity(),
				mTokenClient.GetAccount()
			});
		}

		private AndroidJavaObject getInvitationsClient()
		{
			return mGamesClass.CallStatic<AndroidJavaObject>("getInvitationsClient", new object[2]
			{
				AndroidHelperFragment.GetActivity(),
				mTokenClient.GetAccount()
			});
		}

		private AndroidJavaObject getPlayersClient()
		{
			return mGamesClass.CallStatic<AndroidJavaObject>("getPlayersClient", new object[2]
			{
				AndroidHelperFragment.GetActivity(),
				mTokenClient.GetAccount()
			});
		}

		private AndroidJavaObject getLeaderboardsClient()
		{
			return mGamesClass.CallStatic<AndroidJavaObject>("getLeaderboardsClient", new object[2]
			{
				AndroidHelperFragment.GetActivity(),
				mTokenClient.GetAccount()
			});
		}

		private AndroidJavaObject getPlayerStatsClient()
		{
			return mGamesClass.CallStatic<AndroidJavaObject>("getPlayerStatsClient", new object[2]
			{
				AndroidHelperFragment.GetActivity(),
				mTokenClient.GetAccount()
			});
		}

		private AndroidJavaObject getVideosClient()
		{
			return mGamesClass.CallStatic<AndroidJavaObject>("getVideosClient", new object[2]
			{
				AndroidHelperFragment.GetActivity(),
				mTokenClient.GetAccount()
			});
		}
	}
}
