using System;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidSnapshotMetadata : ISavedGameMetadata
	{
		private AndroidJavaObject mJavaSnapshot;

		private AndroidJavaObject mJavaMetadata;

		private AndroidJavaObject mJavaContents;

		public AndroidJavaObject JavaSnapshot
		{
			get
			{
				return mJavaSnapshot;
			}
		}

		public AndroidJavaObject JavaMetadata
		{
			get
			{
				return mJavaMetadata;
			}
		}

		public AndroidJavaObject JavaContents
		{
			get
			{
				return mJavaContents;
			}
		}

		public bool IsOpen
		{
			get
			{
				if (mJavaContents == null)
				{
					return false;
				}
				return !mJavaContents.Call<bool>("isClosed", Array.Empty<object>());
			}
		}

		public string Filename
		{
			get
			{
				return mJavaMetadata.Call<string>("getUniqueName", Array.Empty<object>());
			}
		}

		public string Description
		{
			get
			{
				return mJavaMetadata.Call<string>("getDescription", Array.Empty<object>());
			}
		}

		public string CoverImageURL
		{
			get
			{
				return mJavaMetadata.Call<string>("getCoverImageUrl", Array.Empty<object>());
			}
		}

		public TimeSpan TotalTimePlayed
		{
			get
			{
				return TimeSpan.FromMilliseconds(mJavaMetadata.Call<long>("getPlayedTime", Array.Empty<object>()));
			}
		}

		public DateTime LastModifiedTimestamp
		{
			get
			{
				return AndroidJavaConverter.ToDateTime(mJavaMetadata.Call<long>("getLastModifiedTimestamp", Array.Empty<object>()));
			}
		}

		public AndroidSnapshotMetadata(AndroidJavaObject javaSnapshot)
		{
			mJavaSnapshot = javaSnapshot;
			mJavaMetadata = javaSnapshot.Call<AndroidJavaObject>("getMetadata", Array.Empty<object>());
			mJavaContents = javaSnapshot.Call<AndroidJavaObject>("getSnapshotContents", Array.Empty<object>());
		}

		public AndroidSnapshotMetadata(AndroidJavaObject javaMetadata, AndroidJavaObject javaContents)
		{
			mJavaSnapshot = null;
			mJavaMetadata = javaMetadata;
			mJavaContents = javaContents;
		}
	}
}
