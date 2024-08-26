using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi.Video;
using GooglePlayGames.OurUtils;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesPlatform : ISocialPlatform
	{
		private static volatile PlayGamesPlatform sInstance;

		private static volatile bool sNearbyInitializePending;

		private static volatile INearbyConnectionClient sNearbyConnectionClient;

		private readonly PlayGamesClientConfiguration mConfiguration;

		private PlayGamesLocalUser mLocalUser;

		private IPlayGamesClient mClient;

		private string mDefaultLbUi;

		private Dictionary<string, string> mIdMap = new Dictionary<string, string>();

		public static bool DebugLogEnabled
		{
			get
			{
				return GooglePlayGames.OurUtils.Logger.DebugLogEnabled;
			}
			set
			{
				GooglePlayGames.OurUtils.Logger.DebugLogEnabled = value;
			}
		}

		public static PlayGamesPlatform Instance
		{
			get
			{
				if (sInstance == null)
				{
					GooglePlayGames.OurUtils.Logger.d("Instance was not initialized, using default configuration.");
					InitializeInstance(PlayGamesClientConfiguration.DefaultConfiguration);
				}
				return sInstance;
			}
		}

		public static INearbyConnectionClient Nearby
		{
			get
			{
				if (sNearbyConnectionClient == null && !sNearbyInitializePending)
				{
					sNearbyInitializePending = true;
					InitializeNearby(null);
				}
				return sNearbyConnectionClient;
			}
		}

		public IRealTimeMultiplayerClient RealTime
		{
			get
			{
				return mClient.GetRtmpClient();
			}
		}

		public ITurnBasedMultiplayerClient TurnBased
		{
			get
			{
				return mClient.GetTbmpClient();
			}
		}

		public ISavedGameClient SavedGame
		{
			get
			{
				return mClient.GetSavedGameClient();
			}
		}

		public IEventsClient Events
		{
			get
			{
				return mClient.GetEventsClient();
			}
		}

		public IVideoClient Video
		{
			get
			{
				return mClient.GetVideoClient();
			}
		}

		public ILocalUser localUser
		{
			get
			{
				return mLocalUser;
			}
		}

		internal PlayGamesPlatform(IPlayGamesClient client)
		{
			mClient = Misc.CheckNotNull(client);
			mLocalUser = new PlayGamesLocalUser(this);
			mConfiguration = PlayGamesClientConfiguration.DefaultConfiguration;
		}

		private PlayGamesPlatform(PlayGamesClientConfiguration configuration)
		{
			GooglePlayGames.OurUtils.Logger.w("Creating new PlayGamesPlatform");
			mLocalUser = new PlayGamesLocalUser(this);
			mConfiguration = configuration;
		}

		public static void InitializeInstance(PlayGamesClientConfiguration configuration)
		{
			if (sInstance == null || sInstance.mConfiguration != configuration)
			{
				sInstance = new PlayGamesPlatform(configuration);
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.w("PlayGamesPlatform already initialized. Ignoring this call.");
			}
		}

		public static void InitializeNearby(Action<INearbyConnectionClient> callback)
		{
			Debug.Log("Calling InitializeNearby!");
			if (sNearbyConnectionClient == null)
			{
				NearbyConnectionClientFactory.Create(delegate(INearbyConnectionClient client)
				{
					Debug.Log("Nearby Client Created!!");
					sNearbyConnectionClient = client;
					if (callback != null)
					{
						callback(client);
					}
					else
					{
						Debug.Log("Initialize Nearby callback is null");
					}
				});
			}
			else if (callback != null)
			{
				Debug.Log("Nearby Already initialized: calling callback directly");
				callback(sNearbyConnectionClient);
			}
			else
			{
				Debug.Log("Nearby Already initialized");
			}
		}

		public static PlayGamesPlatform Activate()
		{
			GooglePlayGames.OurUtils.Logger.d("Activating PlayGamesPlatform.");
			Social.Active = Instance;
			GooglePlayGames.OurUtils.Logger.d("PlayGamesPlatform activated: " + Social.Active);
			return Instance;
		}

		public void SetGravityForPopups(Gravity gravity)
		{
			mClient.SetGravityForPopups(gravity);
		}

		public void AddIdMapping(string fromId, string toId)
		{
			mIdMap[fromId] = toId;
		}

		public void Authenticate(Action<bool> callback)
		{
			Authenticate(callback, false);
		}

		public void Authenticate(Action<bool, string> callback)
		{
			Authenticate(callback, false);
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			Authenticate(delegate(bool success, string msg)
			{
				callback(success);
			}, silent);
		}

		public void Authenticate(Action<bool, string> callback, bool silent)
		{
			Authenticate((!silent) ? SignInInteractivity.CanPromptAlways : SignInInteractivity.NoPrompt, delegate(SignInStatus status)
			{
				switch (status)
				{
				case SignInStatus.Success:
					callback(true, "Authentication succeeded");
					break;
				case SignInStatus.Canceled:
					callback(false, "Authentication canceled");
					GooglePlayGames.OurUtils.Logger.d("Authentication canceled");
					break;
				case SignInStatus.DeveloperError:
					callback(false, "Authentication failed - developer error");
					GooglePlayGames.OurUtils.Logger.d("Authentication failed - developer error");
					break;
				default:
					callback(false, "Authentication failed");
					GooglePlayGames.OurUtils.Logger.d("Authentication failed");
					break;
				}
			});
		}

		public void Authenticate(SignInInteractivity signInInteractivity, Action<SignInStatus> callback)
		{
			if (mClient == null)
			{
				GooglePlayGames.OurUtils.Logger.d("Creating platform-specific Play Games client.");
				mClient = PlayGamesClientFactory.GetPlatformPlayGamesClient(mConfiguration);
			}
			if (callback == null)
			{
				callback = delegate
				{
				};
			}
			switch (signInInteractivity)
			{
			case SignInInteractivity.NoPrompt:
				mClient.Authenticate(true, delegate(SignInStatus code)
				{
					if (code == SignInStatus.UiSignInRequired && Application.internetReachability == NetworkReachability.NotReachable)
					{
						callback(SignInStatus.NetworkError);
					}
					else
					{
						callback(code);
					}
				});
				break;
			case SignInInteractivity.CanPromptAlways:
				mClient.Authenticate(false, delegate(SignInStatus code)
				{
					if (code == SignInStatus.Canceled && Application.internetReachability == NetworkReachability.NotReachable)
					{
						callback(SignInStatus.NetworkError);
					}
					else
					{
						callback(code);
					}
				});
				break;
			case SignInInteractivity.CanPromptOnce:
				mClient.Authenticate(true, delegate(SignInStatus silentSignInResultCode)
				{
					if (silentSignInResultCode == SignInStatus.Success)
					{
						GooglePlayGames.OurUtils.Logger.d("Successful, triggering callback");
						callback(silentSignInResultCode);
					}
					else if (!SignInHelper.ShouldPromptUiSignIn())
					{
						GooglePlayGames.OurUtils.Logger.d("User cancelled sign in attempt in the previous attempt. Triggering callback with silentSignInResultCode");
						callback(silentSignInResultCode);
					}
					else if (Application.internetReachability == NetworkReachability.NotReachable)
					{
						GooglePlayGames.OurUtils.Logger.d("No internet connection");
						callback(SignInStatus.NetworkError);
					}
					else
					{
						mClient.Authenticate(false, delegate(SignInStatus interactiveSignInResultCode)
						{
							if (interactiveSignInResultCode == SignInStatus.Canceled)
							{
								GooglePlayGames.OurUtils.Logger.d("Cancelled, saving this to a shared pref");
								SignInHelper.SetPromptUiSignIn(false);
							}
							callback(interactiveSignInResultCode);
						});
					}
				});
				break;
			default:
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(SignInStatus.Failed);
				});
				break;
			}
		}

		public void Authenticate(ILocalUser unused, Action<bool> callback)
		{
			Authenticate(callback, false);
		}

		public void Authenticate(ILocalUser unused, Action<bool, string> callback)
		{
			Authenticate(callback, false);
		}

		public bool IsAuthenticated()
		{
			if (mClient != null)
			{
				return mClient.IsAuthenticated();
			}
			return false;
		}

		public void SignOut()
		{
			if (mClient != null)
			{
				mClient.SignOut();
			}
			mLocalUser = new PlayGamesLocalUser(this);
		}

		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				callback(new IUserProfile[0]);
			}
			else
			{
				mClient.LoadUsers(userIds, callback);
			}
		}

		public string GetUserId()
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				return "0";
			}
			return mClient.GetUserId();
		}

		public string GetIdToken()
		{
			if (mClient != null)
			{
				return mClient.GetIdToken();
			}
			GooglePlayGames.OurUtils.Logger.e("No client available, returning null.");
			return null;
		}

		public string GetServerAuthCode()
		{
			if (mClient != null && mClient.IsAuthenticated())
			{
				return mClient.GetServerAuthCode();
			}
			return null;
		}

		public void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback)
		{
			if (mClient != null && mClient.IsAuthenticated())
			{
				mClient.GetAnotherServerAuthCode(reAuthenticateIfNeeded, callback);
			}
			else if (mClient != null && reAuthenticateIfNeeded)
			{
				mClient.Authenticate(false, delegate(SignInStatus status)
				{
					if (status == SignInStatus.Success)
					{
						callback(mClient.GetServerAuthCode());
					}
					else
					{
						GooglePlayGames.OurUtils.Logger.e("Re-authentication failed: " + status);
						callback(null);
					}
				});
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.e("Cannot call GetAnotherServerAuthCode: not authenticated");
				callback(null);
			}
		}

		public string GetUserEmail()
		{
			return mClient.GetUserEmail();
		}

		public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			if (mClient != null && mClient.IsAuthenticated())
			{
				mClient.GetPlayerStats(callback);
				return;
			}
			GooglePlayGames.OurUtils.Logger.e("GetPlayerStats can only be called after authentication.");
			callback(CommonStatusCodes.SignInRequired, new PlayerStats());
		}

		public string GetUserDisplayName()
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserDisplayName can only be called after authentication.");
				return string.Empty;
			}
			return mClient.GetUserDisplayName();
		}

		public string GetUserImageUrl()
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserImageUrl can only be called after authentication.");
				return null;
			}
			return mClient.GetUserImageUrl();
		}

		public void ReportProgress(string achievementID, double progress, Action<bool> callback)
		{
			callback = ToOnGameThread(callback);
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportProgress can only be called after authentication.");
				callback(false);
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("ReportProgress, " + achievementID + ", " + progress);
			achievementID = MapId(achievementID);
			if (progress < 1E-06)
			{
				GooglePlayGames.OurUtils.Logger.d("Progress 0.00 interpreted as request to reveal.");
				mClient.RevealAchievement(achievementID, callback);
				return;
			}
			mClient.LoadAchievements(delegate(Achievement[] ach)
			{
				for (int i = 0; i < ach.Length; i++)
				{
					if (ach[i].Id == achievementID)
					{
						if (ach[i].IsIncremental)
						{
							GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " interpreted as incremental target (approximate).");
							if (progress >= 0.0 && progress <= 1.0)
							{
								GooglePlayGames.OurUtils.Logger.w("Progress " + progress + " is less than or equal to 1. You might be trying to use values in the range of [0,1], while values are expected to be within the range [0,100]. If you are using the latter, you can safely ignore this message.");
							}
							int steps = (int)Math.Round(progress / 100.0 * (double)ach[i].TotalSteps);
							mClient.SetStepsAtLeast(achievementID, steps, callback);
						}
						else if (progress >= 100.0)
						{
							GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " interpreted as UNLOCK.");
							mClient.UnlockAchievement(achievementID, callback);
						}
						else
						{
							GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " not enough to unlock non-incremental achievement.");
							callback(false);
						}
						return;
					}
				}
				GooglePlayGames.OurUtils.Logger.e("Unable to locate achievement " + achievementID);
				callback(false);
			});
		}

		public void RevealAchievement(string achievementID, Action<bool> callback = null)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("RevealAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("RevealAchievement: " + achievementID);
				achievementID = MapId(achievementID);
				mClient.RevealAchievement(achievementID, callback);
			}
		}

		public void UnlockAchievement(string achievementID, Action<bool> callback = null)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("UnlockAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("UnlockAchievement: " + achievementID);
				achievementID = MapId(achievementID);
				mClient.UnlockAchievement(achievementID, callback);
			}
		}

		public void IncrementAchievement(string achievementID, int steps, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("IncrementAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("IncrementAchievement: " + achievementID + ", steps " + steps);
				achievementID = MapId(achievementID);
				mClient.IncrementAchievement(achievementID, steps, callback);
			}
		}

		public void SetStepsAtLeast(string achievementID, int steps, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("SetStepsAtLeast can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("SetStepsAtLeast: " + achievementID + ", steps " + steps);
				achievementID = MapId(achievementID);
				mClient.SetStepsAtLeast(achievementID, steps, callback);
			}
		}

		public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadAchievementDescriptions can only be called after authentication.");
				if (callback != null)
				{
					callback(null);
				}
				return;
			}
			mClient.LoadAchievements(delegate(Achievement[] ach)
			{
				IAchievementDescription[] array = new IAchievementDescription[ach.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new PlayGamesAchievement(ach[i]);
				}
				callback(array);
			});
		}

		public void LoadAchievements(Action<IAchievement[]> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadAchievements can only be called after authentication.");
				callback(null);
				return;
			}
			mClient.LoadAchievements(delegate(Achievement[] ach)
			{
				IAchievement[] array = new IAchievement[ach.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new PlayGamesAchievement(ach[i]);
				}
				callback(array);
			});
		}

		public IAchievement CreateAchievement()
		{
			return new PlayGamesAchievement();
		}

		public void ReportScore(long score, string board, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("ReportScore: score=" + score + ", board=" + board);
				string leaderboardId = MapId(board);
				mClient.SubmitScore(leaderboardId, score, callback);
			}
		}

		public void ReportScore(long score, string board, string metadata, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("ReportScore: score=" + score + ", board=" + board + " metadata=" + metadata);
			string leaderboardId = MapId(board);
			mClient.SubmitScore(leaderboardId, score, metadata, callback);
		}

		public void LoadScores(string leaderboardId, Action<IScore[]> callback)
		{
			LoadScores(leaderboardId, LeaderboardStart.PlayerCentered, mClient.LeaderboardMaxResults(), LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, delegate(LeaderboardScoreData scoreData)
			{
				callback(scoreData.Scores);
			});
		}

		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				callback(new LeaderboardScoreData(leaderboardId, ResponseStatus.NotAuthorized));
			}
			else
			{
				mClient.LoadScores(leaderboardId, start, rowCount, collection, timeSpan, callback);
			}
		}

		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadMoreScores can only be called after authentication.");
				callback(new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.NotAuthorized));
			}
			else
			{
				mClient.LoadMoreScores(token, rowCount, callback);
			}
		}

		public ILeaderboard CreateLeaderboard()
		{
			return new PlayGamesLeaderboard(mDefaultLbUi);
		}

		public void ShowAchievementsUI()
		{
			ShowAchievementsUI(null);
		}

		public void ShowAchievementsUI(Action<UIStatus> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ShowAchievementsUI can only be called after authentication.");
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("ShowAchievementsUI callback is " + callback);
			mClient.ShowAchievementsUI(callback);
		}

		public void ShowLeaderboardUI()
		{
			GooglePlayGames.OurUtils.Logger.d("ShowLeaderboardUI with default ID");
			ShowLeaderboardUI(MapId(mDefaultLbUi), null);
		}

		public void ShowLeaderboardUI(string leaderboardId)
		{
			if (leaderboardId != null)
			{
				leaderboardId = MapId(leaderboardId);
			}
			mClient.ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, null);
		}

		public void ShowLeaderboardUI(string leaderboardId, Action<UIStatus> callback)
		{
			ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, callback);
		}

		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ShowLeaderboardUI can only be called after authentication.");
				if (callback != null)
				{
					callback(UIStatus.NotAuthorized);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("ShowLeaderboardUI, lbId=" + leaderboardId + " callback is " + callback);
				mClient.ShowLeaderboardUI(leaderboardId, span, callback);
			}
		}

		public void SetDefaultLeaderboardForUI(string lbid)
		{
			GooglePlayGames.OurUtils.Logger.d("SetDefaultLeaderboardForUI: " + lbid);
			if (lbid != null)
			{
				lbid = MapId(lbid);
			}
			mDefaultLbUi = lbid;
		}

		public void LoadFriends(ILocalUser user, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
			}
			else
			{
				mClient.LoadFriends(callback);
			}
		}

		public void LoadScores(ILeaderboard board, Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			LeaderboardTimeSpan timeSpan;
			switch (board.timeScope)
			{
			case TimeScope.AllTime:
				timeSpan = LeaderboardTimeSpan.AllTime;
				break;
			case TimeScope.Week:
				timeSpan = LeaderboardTimeSpan.Weekly;
				break;
			case TimeScope.Today:
				timeSpan = LeaderboardTimeSpan.Daily;
				break;
			default:
				timeSpan = LeaderboardTimeSpan.AllTime;
				break;
			}
			((PlayGamesLeaderboard)board).loading = true;
			GooglePlayGames.OurUtils.Logger.d(string.Concat("LoadScores, board=", board, " callback is ", callback));
			mClient.LoadScores(board.id, LeaderboardStart.PlayerCentered, (board.range.count > 0) ? board.range.count : mClient.LeaderboardMaxResults(), (board.userScope != UserScope.FriendsOnly) ? LeaderboardCollection.Public : LeaderboardCollection.Social, timeSpan, delegate(LeaderboardScoreData scoreData)
			{
				HandleLoadingScores((PlayGamesLeaderboard)board, scoreData, callback);
			});
		}

		public void RequestPermission(string scope, Action<SignInStatus> callback)
		{
			RequestPermissions(new string[1] { scope }, callback);
		}

		public void RequestPermissions(string[] scopes, Action<SignInStatus> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("HasPermissions can only be called after authentication.");
				callback(SignInStatus.NotAuthenticated);
			}
			else
			{
				mClient.RequestPermissions(scopes, callback);
			}
		}

		public bool HasPermission(string scope)
		{
			return HasPermissions(new string[1] { scope });
		}

		public bool HasPermissions(string[] scopes)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("HasPermissions can only be called after authentication.");
				return false;
			}
			return mClient.HasPermissions(scopes);
		}

		public bool GetLoading(ILeaderboard board)
		{
			if (board != null)
			{
				return board.loading;
			}
			return false;
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate deleg)
		{
			mClient.RegisterInvitationDelegate(deleg);
		}

		internal void HandleLoadingScores(PlayGamesLeaderboard board, LeaderboardScoreData scoreData, Action<bool> callback)
		{
			bool flag = board.SetFromData(scoreData);
			if (flag && !board.HasAllScores() && scoreData.NextPageToken != null)
			{
				int rowCount = board.range.count - board.ScoreCount;
				mClient.LoadMoreScores(scoreData.NextPageToken, rowCount, delegate(LeaderboardScoreData nextScoreData)
				{
					HandleLoadingScores(board, nextScoreData, callback);
				});
			}
			else
			{
				callback(flag);
			}
		}

		internal IUserProfile[] GetFriends()
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.d("Cannot get friends when not authenticated!");
				return new IUserProfile[0];
			}
			return mClient.GetFriends();
		}

		private string MapId(string id)
		{
			if (id == null)
			{
				return null;
			}
			if (mIdMap.ContainsKey(id))
			{
				string text = mIdMap[id];
				GooglePlayGames.OurUtils.Logger.d("Mapping alias " + id + " to ID " + text);
				return text;
			}
			return id;
		}

		private static Action<T> ToOnGameThread<T>(Action<T> toConvert)
		{
			if (toConvert == null)
			{
				return delegate
				{
				};
			}
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
