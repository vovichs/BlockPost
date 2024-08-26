namespace GooglePlayGames.BasicApi
{
	public class PlayerStats
	{
		private static float UNSET_VALUE = -1f;

		private bool mValid;

		private int mNumberOfPurchases;

		private float mAvgSessionLength;

		private int mDaysSinceLastPlayed;

		private int mNumberOfSessions;

		private float mSessPercentile;

		private float mSpendPercentile;

		private float mSpendProbability;

		private float mChurnProbability;

		private float mHighSpenderProbability;

		private float mTotalSpendNext28Days;

		public bool Valid
		{
			get
			{
				return mValid;
			}
		}

		public int NumberOfPurchases
		{
			get
			{
				return mNumberOfPurchases;
			}
		}

		public float AvgSessionLength
		{
			get
			{
				return mAvgSessionLength;
			}
		}

		public int DaysSinceLastPlayed
		{
			get
			{
				return mDaysSinceLastPlayed;
			}
		}

		public int NumberOfSessions
		{
			get
			{
				return mNumberOfSessions;
			}
		}

		public float SessPercentile
		{
			get
			{
				return mSessPercentile;
			}
		}

		public float SpendPercentile
		{
			get
			{
				return mSpendPercentile;
			}
		}

		public float SpendProbability
		{
			get
			{
				return mSpendProbability;
			}
		}

		public float ChurnProbability
		{
			get
			{
				return mChurnProbability;
			}
		}

		public float HighSpenderProbability
		{
			get
			{
				return mHighSpenderProbability;
			}
		}

		public float TotalSpendNext28Days
		{
			get
			{
				return mTotalSpendNext28Days;
			}
		}

		public PlayerStats(int numberOfPurchases, float avgSessionLength, int daysSinceLastPlayed, int numberOfSessions, float sessPercentile, float spendPercentile, float spendProbability, float churnProbability, float highSpenderProbability, float totalSpendNext28Days)
		{
			mValid = true;
			mNumberOfPurchases = numberOfPurchases;
			mAvgSessionLength = avgSessionLength;
			mDaysSinceLastPlayed = daysSinceLastPlayed;
			mNumberOfSessions = numberOfSessions;
			mSessPercentile = sessPercentile;
			mSpendPercentile = spendPercentile;
			mSpendProbability = spendProbability;
			mChurnProbability = churnProbability;
			mHighSpenderProbability = highSpenderProbability;
			mTotalSpendNext28Days = totalSpendNext28Days;
		}

		public PlayerStats()
		{
			mValid = false;
		}

		public bool HasNumberOfPurchases()
		{
			return NumberOfPurchases != (int)UNSET_VALUE;
		}

		public bool HasAvgSessionLength()
		{
			return AvgSessionLength != UNSET_VALUE;
		}

		public bool HasDaysSinceLastPlayed()
		{
			return DaysSinceLastPlayed != (int)UNSET_VALUE;
		}

		public bool HasNumberOfSessions()
		{
			return NumberOfSessions != (int)UNSET_VALUE;
		}

		public bool HasSessPercentile()
		{
			return SessPercentile != UNSET_VALUE;
		}

		public bool HasSpendPercentile()
		{
			return SpendPercentile != UNSET_VALUE;
		}

		public bool HasChurnProbability()
		{
			return ChurnProbability != UNSET_VALUE;
		}

		public bool HasHighSpenderProbability()
		{
			return HighSpenderProbability != UNSET_VALUE;
		}

		public bool HasTotalSpendNext28Days()
		{
			return TotalSpendNext28Days != UNSET_VALUE;
		}
	}
}
