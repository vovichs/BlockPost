using System;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames.BasicApi;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidTokenClient : TokenClient
	{
		private class ResultCallbackProxy : AndroidJavaProxy
		{
			private Action<AndroidJavaObject> mCallback;

			public ResultCallbackProxy(Action<AndroidJavaObject> callback)
				: base("com/google/android/gms/common/api/ResultCallback")
			{
				mCallback = callback;
			}

			public void onResult(AndroidJavaObject tokenResult)
			{
				mCallback(tokenResult);
			}
		}

		private const string HelperFragmentClass = "com.google.games.bridge.HelperFragment";

		private bool requestEmail;

		private bool requestAuthCode;

		private bool requestIdToken;

		private List<string> oauthScopes;

		private string webClientId;

		private bool forceRefresh;

		private bool hidePopups;

		private string accountName;

		private AndroidJavaObject account;

		private string email;

		private string authCode;

		private string idToken;

		public void SetRequestAuthCode(bool flag, bool forceRefresh)
		{
			requestAuthCode = flag;
			this.forceRefresh = forceRefresh;
		}

		public void SetRequestEmail(bool flag)
		{
			requestEmail = flag;
		}

		public void SetRequestIdToken(bool flag)
		{
			requestIdToken = flag;
		}

		public void SetWebClientId(string webClientId)
		{
			this.webClientId = webClientId;
		}

		public void SetHidePopups(bool flag)
		{
			hidePopups = flag;
		}

		public void SetAccountName(string accountName)
		{
			this.accountName = accountName;
		}

		public void AddOauthScopes(params string[] scopes)
		{
			if (scopes != null)
			{
				if (oauthScopes == null)
				{
					oauthScopes = new List<string>();
				}
				oauthScopes.AddRange(scopes);
			}
		}

		public void Signout()
		{
			account = null;
			authCode = null;
			email = null;
			idToken = null;
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				Debug.Log("Calling Signout in token client");
				new AndroidJavaClass("com.google.games.bridge.HelperFragment").CallStatic("signOut", AndroidHelperFragment.GetActivity());
			});
		}

		public string GetEmail()
		{
			return email;
		}

		public string GetAuthCode()
		{
			return authCode;
		}

		public string GetIdToken()
		{
			return idToken;
		}

		public void FetchTokens(bool silent, Action<int> callback)
		{
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				DoFetchToken(silent, callback);
			});
		}

		public void RequestPermissions(string[] scopes, Action<SignInStatus> callback)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject androidJavaObject = AndroidHelperFragment.GetActivity())
				{
					using (AndroidJavaObject task = androidJavaClass.CallStatic<AndroidJavaObject>("showRequestPermissionsUi", new object[2]
					{
						androidJavaObject,
						Enumerable.ToArray(Enumerable.Union(oauthScopes, scopes))
					}))
					{
						AndroidTaskUtils.AddOnSuccessListener(task, false, delegate(AndroidJavaObject accountWithNewScopes)
						{
							if (accountWithNewScopes == null)
							{
								callback(SignInStatus.InternalError);
							}
							else
							{
								account = accountWithNewScopes;
								email = account.Call<string>("getEmail", Array.Empty<object>());
								idToken = account.Call<string>("getIdToken", Array.Empty<object>());
								authCode = account.Call<string>("getServerAuthCode", Array.Empty<object>());
								oauthScopes = Enumerable.ToList(Enumerable.Union(oauthScopes, scopes));
								callback(SignInStatus.Success);
							}
						});
						AndroidTaskUtils.AddOnFailureListener(task, delegate(AndroidJavaObject e)
						{
							SignInStatus signInStatus = SignInHelper.ToSignInStatus(e.Call<int>("getStatusCode", Array.Empty<object>()));
							GooglePlayGames.OurUtils.Logger.e("Exception requesting new permissions: " + signInStatus);
							callback(signInStatus);
						});
					}
				}
			}
		}

		public bool HasPermissions(string[] scopes)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
			{
				using (AndroidJavaObject androidJavaObject = AndroidHelperFragment.GetActivity())
				{
					return androidJavaClass.CallStatic<bool>("hasPermissions", new object[2] { androidJavaObject, scopes });
				}
			}
		}

		private void DoFetchToken(bool silent, Action<int> callback)
		{
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
				{
					using (AndroidJavaObject androidJavaObject = AndroidHelperFragment.GetActivity())
					{
						using (AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("fetchToken", new object[10]
						{
							androidJavaObject,
							silent,
							requestAuthCode,
							requestEmail,
							requestIdToken,
							webClientId,
							forceRefresh,
							oauthScopes.ToArray(),
							hidePopups,
							accountName
						}))
						{
							androidJavaObject2.Call("setResultCallback", new ResultCallbackProxy(delegate(AndroidJavaObject tokenResult)
							{
								account = tokenResult.Call<AndroidJavaObject>("getAccount", Array.Empty<object>());
								authCode = tokenResult.Call<string>("getAuthCode", Array.Empty<object>());
								email = tokenResult.Call<string>("getEmail", Array.Empty<object>());
								idToken = tokenResult.Call<string>("getIdToken", Array.Empty<object>());
								callback(tokenResult.Call<int>("getStatusCode", Array.Empty<object>()));
							}));
						}
					}
				}
			}
			catch (Exception ex)
			{
				GooglePlayGames.OurUtils.Logger.e("Exception launching token request: " + ex.Message);
				GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
		}

		public AndroidJavaObject GetAccount()
		{
			return account;
		}

		public void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback)
		{
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				DoGetAnotherServerAuthCode(reAuthenticateIfNeeded, callback);
			});
		}

		private void DoGetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback)
		{
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.HelperFragment"))
				{
					using (AndroidJavaObject androidJavaObject = AndroidHelperFragment.GetActivity())
					{
						using (AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("fetchToken", new object[10]
						{
							androidJavaObject,
							reAuthenticateIfNeeded,
							true,
							false,
							false,
							webClientId,
							false,
							oauthScopes.ToArray(),
							true,
							""
						}))
						{
							androidJavaObject2.Call("setResultCallback", new ResultCallbackProxy(delegate(AndroidJavaObject tokenResult)
							{
								callback(tokenResult.Call<string>("getAuthCode", Array.Empty<object>()));
							}));
						}
					}
				}
			}
			catch (Exception ex)
			{
				GooglePlayGames.OurUtils.Logger.e("Exception launching token request: " + ex.Message);
				GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
		}
	}
}
