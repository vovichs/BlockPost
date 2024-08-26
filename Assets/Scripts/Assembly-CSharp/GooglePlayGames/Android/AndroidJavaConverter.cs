using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidJavaConverter
	{
		internal static DateTime ToDateTime(long milliseconds)
		{
			DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			result.AddMilliseconds(milliseconds);
			return result;
		}

		internal static int ToLeaderboardVariantTimeSpan(LeaderboardTimeSpan span)
		{
			switch (span)
			{
			case LeaderboardTimeSpan.Daily:
				return 0;
			case LeaderboardTimeSpan.Weekly:
				return 1;
			default:
				return 2;
			}
		}

		internal static int ToLeaderboardVariantCollection(LeaderboardCollection collection)
		{
			if (collection != LeaderboardCollection.Public && collection == LeaderboardCollection.Social)
			{
				return 1;
			}
			return 0;
		}

		internal static int ToPageDirection(ScorePageDirection direction)
		{
			switch (direction)
			{
			case ScorePageDirection.Forward:
				return 0;
			case ScorePageDirection.Backward:
				return 1;
			default:
				return -1;
			}
		}

		internal static Invitation.InvType FromInvitationType(int invitationTypeJava)
		{
			switch (invitationTypeJava)
			{
			case 0:
				return Invitation.InvType.RealTime;
			case 1:
				return Invitation.InvType.TurnBased;
			default:
				return Invitation.InvType.Unknown;
			}
		}

		internal static Participant.ParticipantStatus FromParticipantStatus(int participantStatusJava)
		{
			switch (participantStatusJava)
			{
			case 0:
				return Participant.ParticipantStatus.NotInvitedYet;
			case 1:
				return Participant.ParticipantStatus.Invited;
			case 2:
				return Participant.ParticipantStatus.Joined;
			case 3:
				return Participant.ParticipantStatus.Declined;
			case 4:
				return Participant.ParticipantStatus.Left;
			case 5:
				return Participant.ParticipantStatus.Finished;
			case 6:
				return Participant.ParticipantStatus.Unresponsive;
			default:
				return Participant.ParticipantStatus.Unknown;
			}
		}

		internal static Participant ToParticipant(AndroidJavaObject participant)
		{
			string displayName = participant.Call<string>("getDisplayName", Array.Empty<object>());
			string participantId = participant.Call<string>("getParticipantId", Array.Empty<object>());
			Participant.ParticipantStatus status = FromParticipantStatus(participant.Call<int>("getStatus", Array.Empty<object>()));
			bool connectedToRoom = participant.Call<bool>("isConnectedToRoom", Array.Empty<object>());
			GooglePlayGames.BasicApi.Multiplayer.Player player = null;
			try
			{
				using (AndroidJavaObject player2 = participant.Call<AndroidJavaObject>("getPlayer", Array.Empty<object>()))
				{
					player = ToPlayer(player2);
				}
			}
			catch (Exception)
			{
			}
			return new Participant(displayName, participantId, status, player, connectedToRoom);
		}

		internal static GooglePlayGames.BasicApi.Multiplayer.Player ToPlayer(AndroidJavaObject player)
		{
			if (player == null)
			{
				return null;
			}
			string displayName = player.Call<string>("getDisplayName", Array.Empty<object>());
			string playerId = player.Call<string>("getPlayerId", Array.Empty<object>());
			string avatarUrl = player.Call<string>("getIconImageUrl", Array.Empty<object>());
			return new GooglePlayGames.BasicApi.Multiplayer.Player(displayName, playerId, avatarUrl);
		}

		internal static Invitation ToInvitation(AndroidJavaObject invitation)
		{
			string invId = invitation.Call<string>("getInvitationId", Array.Empty<object>());
			int invitationTypeJava = invitation.Call<int>("getInvitationType", Array.Empty<object>());
			int variant = invitation.Call<int>("getVariant", Array.Empty<object>());
			DateTime creationTime = ToDateTime(invitation.Call<long>("getCreationTimestamp", Array.Empty<object>()));
			using (AndroidJavaObject participant = invitation.Call<AndroidJavaObject>("getInviter", Array.Empty<object>()))
			{
				return new Invitation(FromInvitationType(invitationTypeJava), invId, ToParticipant(participant), variant, creationTime);
			}
		}

		internal static TurnBasedMatch ToTurnBasedMatch(AndroidJavaObject turnBasedMatch)
		{
			if (turnBasedMatch == null)
			{
				return null;
			}
			string matchId = turnBasedMatch.Call<string>("getMatchId", Array.Empty<object>());
			byte[] data = turnBasedMatch.Call<byte[]>("getData", Array.Empty<object>());
			bool canRematch = turnBasedMatch.Call<bool>("canRematch", Array.Empty<object>());
			uint availableAutomatchSlots = (uint)turnBasedMatch.Call<int>("getAvailableAutoMatchSlots", Array.Empty<object>());
			string selfParticipantId = turnBasedMatch.Call<string>("getCreatorId", Array.Empty<object>());
			List<Participant> participants = ToParticipantList(turnBasedMatch);
			string pendingParticipantId = turnBasedMatch.Call<string>("getPendingParticipantId", Array.Empty<object>());
			TurnBasedMatch.MatchStatus matchStatus = ToMatchStatus(turnBasedMatch.Call<int>("getStatus", Array.Empty<object>()));
			TurnBasedMatch.MatchTurnStatus turnStatus = ToMatchTurnStatus(turnBasedMatch.Call<int>("getTurnStatus", Array.Empty<object>()));
			uint variant = (uint)turnBasedMatch.Call<int>("getVariant", Array.Empty<object>());
			uint version = (uint)turnBasedMatch.Call<int>("getVersion", Array.Empty<object>());
			DateTime creationTime = ToDateTime(turnBasedMatch.Call<long>("getCreationTimestamp", Array.Empty<object>()));
			DateTime lastUpdateTime = ToDateTime(turnBasedMatch.Call<long>("getLastUpdatedTimestamp", Array.Empty<object>()));
			return new TurnBasedMatch(matchId, data, canRematch, selfParticipantId, participants, availableAutomatchSlots, pendingParticipantId, turnStatus, matchStatus, variant, version, creationTime, lastUpdateTime);
		}

		internal static List<Participant> ToParticipantList(AndroidJavaObject turnBasedMatch)
		{
			using (AndroidJavaObject androidJavaObject = turnBasedMatch.Call<AndroidJavaObject>("getParticipantIds", Array.Empty<object>()))
			{
				List<Participant> list = new List<Participant>();
				int num = androidJavaObject.Call<int>("size", Array.Empty<object>());
				for (int i = 0; i < num; i++)
				{
					string text = androidJavaObject.Call<string>("get", new object[1] { i });
					using (AndroidJavaObject participant = turnBasedMatch.Call<AndroidJavaObject>("getParticipant", new object[1] { text }))
					{
						list.Add(ToParticipant(participant));
					}
				}
				return list;
			}
		}

		internal static List<string> ToStringList(AndroidJavaObject stringList)
		{
			if (stringList == null)
			{
				return new List<string>();
			}
			int num = stringList.Call<int>("size", Array.Empty<object>());
			List<string> list = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(stringList.Call<string>("get", new object[1] { i }));
			}
			return list;
		}

		internal static AndroidJavaObject ToJavaStringList(List<string> list)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.ArrayList");
			for (int i = 0; i < list.Count; i++)
			{
				androidJavaObject.Call<bool>("add", new object[1] { list[i] });
			}
			return androidJavaObject;
		}

		internal static TurnBasedMatch.MatchStatus ToMatchStatus(int matchStatus)
		{
			switch (matchStatus)
			{
			case 0:
				return TurnBasedMatch.MatchStatus.AutoMatching;
			case 1:
				return TurnBasedMatch.MatchStatus.Active;
			case 2:
				return TurnBasedMatch.MatchStatus.Complete;
			case 3:
				return TurnBasedMatch.MatchStatus.Expired;
			case 4:
				return TurnBasedMatch.MatchStatus.Cancelled;
			case 5:
				return TurnBasedMatch.MatchStatus.Deleted;
			default:
				return TurnBasedMatch.MatchStatus.Unknown;
			}
		}

		internal static TurnBasedMatch.MatchTurnStatus ToMatchTurnStatus(int matchTurnStatus)
		{
			switch (matchTurnStatus)
			{
			case 0:
				return TurnBasedMatch.MatchTurnStatus.Invited;
			case 1:
				return TurnBasedMatch.MatchTurnStatus.MyTurn;
			case 2:
				return TurnBasedMatch.MatchTurnStatus.TheirTurn;
			case 3:
				return TurnBasedMatch.MatchTurnStatus.Complete;
			default:
				return TurnBasedMatch.MatchTurnStatus.Unknown;
			}
		}
	}
}
