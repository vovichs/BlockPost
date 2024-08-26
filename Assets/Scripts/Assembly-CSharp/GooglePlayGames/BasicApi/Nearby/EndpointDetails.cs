using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	public struct EndpointDetails
	{
		private readonly string mEndpointId;

		private readonly string mName;

		private readonly string mServiceId;

		public string EndpointId
		{
			get
			{
				return mEndpointId;
			}
		}

		public string Name
		{
			get
			{
				return mName;
			}
		}

		public string ServiceId
		{
			get
			{
				return mServiceId;
			}
		}

		public EndpointDetails(string endpointId, string name, string serviceId)
		{
			mEndpointId = Misc.CheckNotNull(endpointId);
			mName = Misc.CheckNotNull(name);
			mServiceId = Misc.CheckNotNull(serviceId);
		}
	}
}
