using System;
using System.Collections.Generic;
using UnityEngine;
using VK.Unity.Responses;

namespace VK.Unity.Example
{
	internal sealed class MainMenu : MenuBase
	{
		protected override string MenuName
		{
			get
			{
				return "VK Unity SDK Test Menu";
			}
		}

		protected override bool ShowBackButton()
		{
			return false;
		}

		protected override void GetGui()
		{
			bool flag = GUI.enabled;
			if (Button("VK.Init"))
			{
				try
				{
					VKSDK.Init(OnInitComplete);
					base.Status = "VK.Init() called with " + VKSDK.AppId;
				}
				catch (Exception ex)
				{
					base.Status = "VK.Init faled: " + ex.Message;
				}
				VKSDK.OnAccessTokenChanged = delegate
				{
					base.Status = base.Status + Environment.NewLine + " AccessToken changed!";
				};
			}
			GUI.enabled = flag && VKSDK.IsInitialized;
			if (Button("Login"))
			{
				CallLogin();
				base.Status = "Login called";
			}
			GUI.enabled = flag && VKSDK.IsLoggedIn;
			if (Button("Logout"))
			{
				CallLogout();
				base.Status = "Logout called";
			}
			GUI.enabled = flag && VKSDK.IsLoggedIn;
			if (Button("API call (user.get)"))
			{
				CallApi();
				base.Status = "API.user.get is called";
			}
			if (Button("API call (friends.get)"))
			{
				CallGetFriends();
				base.Status = "API.friends.get is called";
			}
			if (Button("Test validation"))
			{
				TestValidation();
				base.Status = "API.account.testValidation is called";
			}
			GUI.enabled = flag;
		}

		private void CallGetFriends()
		{
			VKSDK.API("friends.get", new Dictionary<string, string> { { "order", "hints" } }, delegate(APICallResponse result)
			{
				base.Status = "API.friends.get call completed: " + result.ToString();
			});
		}

		private void TestValidation()
		{
			VKSDK.API("account.testValidation", new Dictionary<string, string>(), delegate(APICallResponse result)
			{
				base.Status = "API.account.testValidation call completed: " + result.ToString();
			});
		}

		private void CallLogout()
		{
			VKSDK.Logout();
		}

		private void CallApi()
		{
			VKSDK.API("users.get", new Dictionary<string, string>(), delegate(APICallResponse result)
			{
				base.Status = "API.users.get call completed: " + result.ToString();
			});
		}

		private void CallLogin()
		{
			VKSDK.Login(new List<Scope>
			{
				Scope.Friends,
				Scope.Offline
			}, OnLoginCompleted);
		}

		private void OnLoginCompleted(AuthResponse response)
		{
			base.Status = "Login completed: " + JsonUtility.ToJson(response);
		}

		private void OnInitComplete()
		{
			base.Status = "Init completed: SDK version =  " + VKSDK.SDKVersion + ", appId = " + VKSDK.AppId + ", isLoggedIn = " + VKSDK.IsLoggedIn.ToString();
			base.Status = base.Status + ", cert. fingerprint= " + VKSDK.GetExtraData(VKSDK.EXTRA_ANDROID_CERTIFICATE_FINGERPRINT_KEY);
		}
	}
}
