using System.Collections.Generic;
using System.Linq;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi
{
	public struct PlayGamesClientConfiguration
	{
		public class Builder
		{
			private bool mEnableSaveGames;

			private List<string> mScopes;

			private bool mHidePopups;

			private bool mRequestAuthCode;

			private bool mForceRefresh;

			private bool mRequestEmail;

			private bool mRequestIdToken;

			private string mAccountName;

			private InvitationReceivedDelegate mInvitationDelegate = delegate
			{
			};

			private MatchDelegate mMatchDelegate = delegate
			{
			};

			public Builder EnableSavedGames()
			{
				mEnableSaveGames = true;
				return this;
			}

			public Builder EnableHidePopups()
			{
				mHidePopups = true;
				return this;
			}

			public Builder RequestServerAuthCode(bool forceRefresh)
			{
				mRequestAuthCode = true;
				mForceRefresh = forceRefresh;
				return this;
			}

			public Builder RequestEmail()
			{
				mRequestEmail = true;
				return this;
			}

			public Builder RequestIdToken()
			{
				mRequestIdToken = true;
				return this;
			}

			public Builder SetAccountName(string accountName)
			{
				mAccountName = accountName;
				return this;
			}

			public Builder AddOauthScope(string scope)
			{
				if (mScopes == null)
				{
					mScopes = new List<string>();
				}
				mScopes.Add(scope);
				return this;
			}

			public Builder WithInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
			{
				mInvitationDelegate = Misc.CheckNotNull(invitationDelegate);
				return this;
			}

			public Builder WithMatchDelegate(MatchDelegate matchDelegate)
			{
				mMatchDelegate = Misc.CheckNotNull(matchDelegate);
				return this;
			}

			public PlayGamesClientConfiguration Build()
			{
				return new PlayGamesClientConfiguration(this);
			}

			internal bool HasEnableSaveGames()
			{
				return mEnableSaveGames;
			}

			internal bool IsRequestingAuthCode()
			{
				return mRequestAuthCode;
			}

			internal bool IsHidingPopups()
			{
				return mHidePopups;
			}

			internal bool IsForcingRefresh()
			{
				return mForceRefresh;
			}

			internal bool IsRequestingEmail()
			{
				return mRequestEmail;
			}

			internal bool IsRequestingIdToken()
			{
				return mRequestIdToken;
			}

			internal string GetAccountName()
			{
				return mAccountName;
			}

			internal string[] getScopes()
			{
				if (mScopes != null)
				{
					return mScopes.ToArray();
				}
				return new string[0];
			}

			internal MatchDelegate GetMatchDelegate()
			{
				return mMatchDelegate;
			}

			internal InvitationReceivedDelegate GetInvitationDelegate()
			{
				return mInvitationDelegate;
			}
		}

		public static readonly PlayGamesClientConfiguration DefaultConfiguration = new Builder().Build();

		private readonly bool mEnableSavedGames;

		private readonly string[] mScopes;

		private readonly bool mRequestAuthCode;

		private readonly bool mForceRefresh;

		private readonly bool mHidePopups;

		private readonly bool mRequestEmail;

		private readonly bool mRequestIdToken;

		private readonly string mAccountName;

		private readonly InvitationReceivedDelegate mInvitationDelegate;

		private readonly MatchDelegate mMatchDelegate;

		public bool EnableSavedGames
		{
			get
			{
				return mEnableSavedGames;
			}
		}

		public bool IsHidingPopups
		{
			get
			{
				return mHidePopups;
			}
		}

		public bool IsRequestingAuthCode
		{
			get
			{
				return mRequestAuthCode;
			}
		}

		public bool IsForcingRefresh
		{
			get
			{
				return mForceRefresh;
			}
		}

		public bool IsRequestingEmail
		{
			get
			{
				return mRequestEmail;
			}
		}

		public bool IsRequestingIdToken
		{
			get
			{
				return mRequestIdToken;
			}
		}

		public string AccountName
		{
			get
			{
				return mAccountName;
			}
		}

		public string[] Scopes
		{
			get
			{
				return mScopes;
			}
		}

		public InvitationReceivedDelegate InvitationDelegate
		{
			get
			{
				return mInvitationDelegate;
			}
		}

		public MatchDelegate MatchDelegate
		{
			get
			{
				return mMatchDelegate;
			}
		}

		private PlayGamesClientConfiguration(Builder builder)
		{
			mEnableSavedGames = builder.HasEnableSaveGames();
			mInvitationDelegate = builder.GetInvitationDelegate();
			mMatchDelegate = builder.GetMatchDelegate();
			mScopes = builder.getScopes();
			mHidePopups = builder.IsHidingPopups();
			mRequestAuthCode = builder.IsRequestingAuthCode();
			mForceRefresh = builder.IsForcingRefresh();
			mRequestEmail = builder.IsRequestingEmail();
			mRequestIdToken = builder.IsRequestingIdToken();
			mAccountName = builder.GetAccountName();
		}

		public static bool operator ==(PlayGamesClientConfiguration c1, PlayGamesClientConfiguration c2)
		{
			if (c1.EnableSavedGames != c2.EnableSavedGames || c1.IsForcingRefresh != c2.IsForcingRefresh || c1.IsHidingPopups != c2.IsHidingPopups || c1.IsRequestingEmail != c2.IsRequestingEmail || c1.IsRequestingAuthCode != c2.IsRequestingAuthCode || !Enumerable.SequenceEqual(c1.Scopes, c2.Scopes) || c1.AccountName != c2.AccountName)
			{
				return false;
			}
			return true;
		}

		public static bool operator !=(PlayGamesClientConfiguration c1, PlayGamesClientConfiguration c2)
		{
			return !(c1 == c2);
		}

		public override int GetHashCode()
		{
			return ((((((((17 * 31 + EnableSavedGames.GetHashCode()) * 31 + IsForcingRefresh.GetHashCode()) * 31 + IsHidingPopups.GetHashCode()) * 31 + IsRequestingEmail.GetHashCode()) * 31 + IsRequestingAuthCode.GetHashCode()) * 31 + Scopes.GetHashCode()) * 31 + AccountName.GetHashCode()) * 31 + InvitationDelegate.GetHashCode()) * 31 + MatchDelegate.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return this == (PlayGamesClientConfiguration)obj;
		}
	}
}
