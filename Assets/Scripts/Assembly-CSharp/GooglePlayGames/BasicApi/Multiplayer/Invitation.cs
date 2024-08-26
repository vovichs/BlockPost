using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class Invitation
	{
		public enum InvType
		{
			RealTime = 0,
			TurnBased = 1,
			Unknown = 2
		}

		private InvType mInvitationType;

		private string mInvitationId;

		private Participant mInviter;

		private int mVariant;

		private DateTime mCreationTime;

		public InvType InvitationType
		{
			get
			{
				return mInvitationType;
			}
		}

		public string InvitationId
		{
			get
			{
				return mInvitationId;
			}
		}

		public Participant Inviter
		{
			get
			{
				return mInviter;
			}
		}

		public int Variant
		{
			get
			{
				return mVariant;
			}
		}

		public DateTime CreationTime
		{
			get
			{
				return mCreationTime;
			}
		}

		internal Invitation(InvType invType, string invId, Participant inviter, int variant, DateTime creationTime)
		{
			mInvitationType = invType;
			mInvitationId = invId;
			mInviter = inviter;
			mVariant = variant;
			mCreationTime = creationTime;
		}

		public override string ToString()
		{
			return string.Format("[Invitation: InvitationType={0}, InvitationId={1}, Inviter={2}, Variant={3}, CreationTime={4}]", InvitationType, InvitationId, Inviter, Variant, CreationTime);
		}
	}
}
