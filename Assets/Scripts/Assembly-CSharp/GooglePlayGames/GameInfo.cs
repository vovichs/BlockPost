namespace GooglePlayGames
{
	public static class GameInfo
	{
		private const string UnescapedApplicationId = "APP_ID";

		private const string UnescapedIosClientId = "IOS_CLIENTID";

		private const string UnescapedWebClientId = "WEB_CLIENTID";

		private const string UnescapedNearbyServiceId = "NEARBY_SERVICE_ID";

		public const string ApplicationId = "1035200774300";

		public const string IosClientId = "__IOS_CLIENTID__";

		public const string WebClientId = "1035200774300-6qhvcak2te0q2n0kcc3on3g5it9a3v3j.apps.googleusercontent.com";

		public const string NearbyConnectionServiceId = "";

		public static bool ApplicationIdInitialized()
		{
			if (!string.IsNullOrEmpty("1035200774300"))
			{
				return !"1035200774300".Equals(ToEscapedToken("APP_ID"));
			}
			return false;
		}

		public static bool IosClientIdInitialized()
		{
			if (!string.IsNullOrEmpty("__IOS_CLIENTID__"))
			{
				return !"__IOS_CLIENTID__".Equals(ToEscapedToken("IOS_CLIENTID"));
			}
			return false;
		}

		public static bool WebClientIdInitialized()
		{
			if (!string.IsNullOrEmpty("1035200774300-6qhvcak2te0q2n0kcc3on3g5it9a3v3j.apps.googleusercontent.com"))
			{
				return !"1035200774300-6qhvcak2te0q2n0kcc3on3g5it9a3v3j.apps.googleusercontent.com".Equals(ToEscapedToken("WEB_CLIENTID"));
			}
			return false;
		}

		public static bool NearbyConnectionsInitialized()
		{
			if (!string.IsNullOrEmpty(""))
			{
				return !"".Equals(ToEscapedToken("NEARBY_SERVICE_ID"));
			}
			return false;
		}

		private static string ToEscapedToken(string token)
		{
			return string.Format("__{0}__", token);
		}
	}
}
