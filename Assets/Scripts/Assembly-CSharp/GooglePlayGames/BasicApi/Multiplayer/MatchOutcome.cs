using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class MatchOutcome
	{
		public enum ParticipantResult
		{
			Unset = -1,
			None = 0,
			Win = 1,
			Loss = 2,
			Tie = 3
		}

		public const uint PlacementUnset = 0u;

		private List<string> mParticipantIds = new List<string>();

		private Dictionary<string, uint> mPlacements = new Dictionary<string, uint>();

		private Dictionary<string, ParticipantResult> mResults = new Dictionary<string, ParticipantResult>();

		public List<string> ParticipantIds
		{
			get
			{
				return mParticipantIds;
			}
		}

		public void SetParticipantResult(string participantId, ParticipantResult result, uint placement)
		{
			if (!mParticipantIds.Contains(participantId))
			{
				mParticipantIds.Add(participantId);
			}
			mPlacements[participantId] = placement;
			mResults[participantId] = result;
		}

		public void SetParticipantResult(string participantId, ParticipantResult result)
		{
			SetParticipantResult(participantId, result, 0u);
		}

		public void SetParticipantResult(string participantId, uint placement)
		{
			SetParticipantResult(participantId, ParticipantResult.Unset, placement);
		}

		public ParticipantResult GetResultFor(string participantId)
		{
			if (!mResults.ContainsKey(participantId))
			{
				return ParticipantResult.Unset;
			}
			return mResults[participantId];
		}

		public uint GetPlacementFor(string participantId)
		{
			if (!mPlacements.ContainsKey(participantId))
			{
				return 0u;
			}
			return mPlacements[participantId];
		}

		public override string ToString()
		{
			string text = "[MatchOutcome";
			foreach (string mParticipantId in mParticipantIds)
			{
				text += string.Format(" {0}->({1},{2})", mParticipantId, GetResultFor(mParticipantId), GetPlacementFor(mParticipantId));
			}
			return text + "]";
		}
	}
}
