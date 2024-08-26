namespace GooglePlayGames.BasicApi
{
	public class ScorePageToken
	{
		private string mId;

		private object mInternalObject;

		private LeaderboardCollection mCollection;

		private LeaderboardTimeSpan mTimespan;

		private ScorePageDirection mDirection;

		public LeaderboardCollection Collection
		{
			get
			{
				return mCollection;
			}
		}

		public LeaderboardTimeSpan TimeSpan
		{
			get
			{
				return mTimespan;
			}
		}

		public ScorePageDirection Direction
		{
			get
			{
				return mDirection;
			}
		}

		public string LeaderboardId
		{
			get
			{
				return mId;
			}
		}

		internal object InternalObject
		{
			get
			{
				return mInternalObject;
			}
		}

		internal ScorePageToken(object internalObject, string id, LeaderboardCollection collection, LeaderboardTimeSpan timespan, ScorePageDirection direction)
		{
			mInternalObject = internalObject;
			mId = id;
			mCollection = collection;
			mTimespan = timespan;
			mDirection = direction;
		}
	}
}
