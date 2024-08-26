using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidSavedGameClient : ISavedGameClient
	{
		private class AndroidConflictResolver : IConflictResolver
		{
			private readonly AndroidJavaObject mSnapshotsClient;

			private readonly AndroidJavaObject mConflict;

			private readonly AndroidSnapshotMetadata mOriginal;

			private readonly AndroidSnapshotMetadata mUnmerged;

			private readonly Action<SavedGameRequestStatus, ISavedGameMetadata> mCompleteCallback;

			private readonly Action mRetryFileOpen;

			private readonly AndroidSavedGameClient mAndroidSavedGameClient;

			internal AndroidConflictResolver(AndroidSavedGameClient androidSavedGameClient, AndroidJavaObject snapshotClient, AndroidJavaObject conflict, AndroidSnapshotMetadata original, AndroidSnapshotMetadata unmerged, Action<SavedGameRequestStatus, ISavedGameMetadata> completeCallback, Action retryOpen)
			{
				mAndroidSavedGameClient = androidSavedGameClient;
				mSnapshotsClient = Misc.CheckNotNull(snapshotClient);
				mConflict = Misc.CheckNotNull(conflict);
				mOriginal = Misc.CheckNotNull(original);
				mUnmerged = Misc.CheckNotNull(unmerged);
				mCompleteCallback = Misc.CheckNotNull(completeCallback);
				mRetryFileOpen = Misc.CheckNotNull(retryOpen);
			}

			public void ResolveConflict(ISavedGameMetadata chosenMetadata, SavedGameMetadataUpdate metadataUpdate, byte[] updatedData)
			{
				AndroidSnapshotMetadata androidSnapshotMetadata = chosenMetadata as AndroidSnapshotMetadata;
				if (androidSnapshotMetadata != mOriginal && androidSnapshotMetadata != mUnmerged)
				{
					GooglePlayGames.OurUtils.Logger.e("Caller attempted to choose a version of the metadata that was not part of the conflict");
					mCompleteCallback(SavedGameRequestStatus.BadInputError, null);
					return;
				}
				using (AndroidJavaObject androidJavaObject = mConflict.Call<AndroidJavaObject>("getResolutionSnapshotContents", Array.Empty<object>()))
				{
					if (!androidJavaObject.Call<bool>("writeBytes", new object[1] { updatedData }))
					{
						GooglePlayGames.OurUtils.Logger.e("Can't update snapshot contents during conflict resolution.");
						mCompleteCallback(SavedGameRequestStatus.BadInputError, null);
					}
					using (AndroidJavaObject androidJavaObject2 = AsMetadataChange(metadataUpdate))
					{
						using (AndroidJavaObject task = mSnapshotsClient.Call<AndroidJavaObject>("resolveConflict", new object[4]
						{
							mConflict.Call<string>("getConflictId", Array.Empty<object>()),
							androidSnapshotMetadata.JavaMetadata.Call<string>("getSnapshotId", Array.Empty<object>()),
							androidJavaObject2,
							androidJavaObject
						}))
						{
							AndroidTaskUtils.AddOnSuccessListener<AndroidJavaObject>(task, delegate
							{
								mRetryFileOpen();
							});
							mAndroidSavedGameClient.AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject exception)
							{
								GooglePlayGames.OurUtils.Logger.d("ResolveConflict failed: " + exception.Call<string>("toString", Array.Empty<object>()));
								SavedGameRequestStatus arg = (mAndroidSavedGameClient.mAndroidClient.IsAuthenticated() ? SavedGameRequestStatus.InternalError : SavedGameRequestStatus.AuthenticationError);
								mCompleteCallback(arg, null);
							});
						}
					}
				}
			}

			public void ChooseMetadata(ISavedGameMetadata chosenMetadata)
			{
				AndroidSnapshotMetadata androidSnapshotMetadata = chosenMetadata as AndroidSnapshotMetadata;
				if (androidSnapshotMetadata != mOriginal && androidSnapshotMetadata != mUnmerged)
				{
					GooglePlayGames.OurUtils.Logger.e("Caller attempted to choose a version of the metadata that was not part of the conflict");
					mCompleteCallback(SavedGameRequestStatus.BadInputError, null);
					return;
				}
				using (AndroidJavaObject task = mSnapshotsClient.Call<AndroidJavaObject>("resolveConflict", new object[2]
				{
					mConflict.Call<string>("getConflictId", Array.Empty<object>()),
					androidSnapshotMetadata.JavaSnapshot
				}))
				{
					AndroidTaskUtils.AddOnSuccessListener<AndroidJavaObject>(task, delegate
					{
						mRetryFileOpen();
					});
					mAndroidSavedGameClient.AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject exception)
					{
						GooglePlayGames.OurUtils.Logger.d("ChooseMetadata failed: " + exception.Call<string>("toString", Array.Empty<object>()));
						SavedGameRequestStatus arg = (mAndroidSavedGameClient.mAndroidClient.IsAuthenticated() ? SavedGameRequestStatus.InternalError : SavedGameRequestStatus.AuthenticationError);
						mCompleteCallback(arg, null);
					});
				}
			}
		}

		private static readonly Regex ValidFilenameRegex = new Regex("\\A[a-zA-Z0-9-._~]{1,100}\\Z");

		private volatile AndroidJavaObject mSnapshotsClient;

		private volatile AndroidClient mAndroidClient;

		public AndroidSavedGameClient(AndroidClient androidClient, AndroidJavaObject account)
		{
			mAndroidClient = androidClient;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.games.Games"))
			{
				mSnapshotsClient = androidJavaClass.CallStatic<AndroidJavaObject>("getSnapshotsClient", new object[2]
				{
					AndroidHelperFragment.GetActivity(),
					account
				});
			}
		}

		public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			Misc.CheckNotNull(filename);
			Misc.CheckNotNull(completedCallback);
			bool prefetchDataOnConflict = false;
			completedCallback = ToOnGameThread(completedCallback);
			ConflictCallback conflictCallback = delegate(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				switch (resolutionStrategy)
				{
				case ConflictResolutionStrategy.UseOriginal:
					resolver.ChooseMetadata(original);
					break;
				case ConflictResolutionStrategy.UseUnmerged:
					resolver.ChooseMetadata(unmerged);
					break;
				case ConflictResolutionStrategy.UseLongestPlaytime:
					if (original.TotalTimePlayed >= unmerged.TotalTimePlayed)
					{
						resolver.ChooseMetadata(original);
					}
					else
					{
						resolver.ChooseMetadata(unmerged);
					}
					break;
				default:
					GooglePlayGames.OurUtils.Logger.e("Unhandled strategy " + resolutionStrategy);
					completedCallback(SavedGameRequestStatus.InternalError, null);
					break;
				}
			};
			conflictCallback = ToOnGameThread(conflictCallback);
			if (!IsValidFilename(filename))
			{
				GooglePlayGames.OurUtils.Logger.e("Received invalid filename: " + filename);
				completedCallback(SavedGameRequestStatus.BadInputError, null);
			}
			else
			{
				InternalOpen(filename, source, resolutionStrategy, prefetchDataOnConflict, conflictCallback, completedCallback);
			}
		}

		public void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			Misc.CheckNotNull(filename);
			Misc.CheckNotNull(conflictCallback);
			Misc.CheckNotNull(completedCallback);
			conflictCallback = ToOnGameThread(conflictCallback);
			completedCallback = ToOnGameThread(completedCallback);
			if (!IsValidFilename(filename))
			{
				GooglePlayGames.OurUtils.Logger.e("Received invalid filename: " + filename);
				completedCallback(SavedGameRequestStatus.BadInputError, null);
			}
			else
			{
				InternalOpen(filename, source, ConflictResolutionStrategy.UseManual, prefetchDataOnConflict, conflictCallback, completedCallback);
			}
		}

		private void InternalOpen(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			int num;
			switch (resolutionStrategy)
			{
			case ConflictResolutionStrategy.UseLastKnownGood:
				num = 2;
				break;
			case ConflictResolutionStrategy.UseMostRecentlySaved:
				num = 3;
				break;
			case ConflictResolutionStrategy.UseLongestPlaytime:
				num = 1;
				break;
			case ConflictResolutionStrategy.UseManual:
				num = -1;
				break;
			default:
				num = 3;
				break;
			}
			using (AndroidJavaObject task = mSnapshotsClient.Call<AndroidJavaObject>("open", new object[3] { filename, true, num }))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject dataOrConflict)
				{
					if (dataOrConflict.Call<bool>("isConflict", Array.Empty<object>()))
					{
						AndroidJavaObject androidJavaObject = dataOrConflict.Call<AndroidJavaObject>("getConflict", Array.Empty<object>());
						AndroidSnapshotMetadata androidSnapshotMetadata = new AndroidSnapshotMetadata(androidJavaObject.Call<AndroidJavaObject>("getSnapshot", Array.Empty<object>()));
						AndroidSnapshotMetadata androidSnapshotMetadata2 = new AndroidSnapshotMetadata(androidJavaObject.Call<AndroidJavaObject>("getConflictingSnapshot", Array.Empty<object>()));
						AndroidConflictResolver resolver = new AndroidConflictResolver(this, mSnapshotsClient, androidJavaObject, androidSnapshotMetadata, androidSnapshotMetadata2, completedCallback, delegate
						{
							InternalOpen(filename, source, resolutionStrategy, prefetchDataOnConflict, conflictCallback, completedCallback);
						});
						byte[] originalData = androidSnapshotMetadata.JavaContents.Call<byte[]>("readFully", Array.Empty<object>());
						byte[] unmergedData = androidSnapshotMetadata2.JavaContents.Call<byte[]>("readFully", Array.Empty<object>());
						conflictCallback(resolver, androidSnapshotMetadata, originalData, androidSnapshotMetadata2, unmergedData);
						return;
					}
					using (AndroidJavaObject androidJavaObject2 = dataOrConflict.Call<AndroidJavaObject>("getData", Array.Empty<object>()))
					{
						AndroidJavaObject javaSnapshot = androidJavaObject2.Call<AndroidJavaObject>("freeze", Array.Empty<object>());
						completedCallback(SavedGameRequestStatus.Success, new AndroidSnapshotMetadata(javaSnapshot));
					}
				});
				AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject exception)
				{
					GooglePlayGames.OurUtils.Logger.d("InternalOpen has failed: " + exception.Call<string>("toString", Array.Empty<object>()));
					SavedGameRequestStatus arg = (mAndroidClient.IsAuthenticated() ? SavedGameRequestStatus.InternalError : SavedGameRequestStatus.AuthenticationError);
					completedCallback(arg, null);
				});
			}
		}

		public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
		{
			Misc.CheckNotNull(metadata);
			Misc.CheckNotNull(completedCallback);
			completedCallback = ToOnGameThread(completedCallback);
			AndroidSnapshotMetadata androidSnapshotMetadata = metadata as AndroidSnapshotMetadata;
			if (androidSnapshotMetadata == null)
			{
				GooglePlayGames.OurUtils.Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
				completedCallback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			if (!androidSnapshotMetadata.IsOpen)
			{
				GooglePlayGames.OurUtils.Logger.e("This method requires an open ISavedGameMetadata.");
				completedCallback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			byte[] array = androidSnapshotMetadata.JavaContents.Call<byte[]>("readFully", Array.Empty<object>());
			if (array == null)
			{
				completedCallback(SavedGameRequestStatus.BadInputError, null);
			}
			else
			{
				completedCallback(SavedGameRequestStatus.Success, array);
			}
		}

		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
		{
			Misc.CheckNotNull(uiTitle);
			Misc.CheckNotNull(callback);
			callback = ToOnGameThread(callback);
			if (maxDisplayedSavedGames == 0)
			{
				GooglePlayGames.OurUtils.Logger.e("maxDisplayedSavedGames must be greater than 0");
				callback(SelectUIStatus.BadInputError, null);
			}
			else
			{
				AndroidHelperFragment.ShowSelectSnapshotUI(showCreateSaveUI, showDeleteSaveUI, (int)maxDisplayedSavedGames, uiTitle, callback);
			}
		}

		public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			Misc.CheckNotNull(metadata);
			Misc.CheckNotNull(updatedBinaryData);
			Misc.CheckNotNull(callback);
			callback = ToOnGameThread(callback);
			AndroidSnapshotMetadata androidSnapshotMetadata = metadata as AndroidSnapshotMetadata;
			if (androidSnapshotMetadata == null)
			{
				GooglePlayGames.OurUtils.Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
				callback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			if (!androidSnapshotMetadata.IsOpen)
			{
				GooglePlayGames.OurUtils.Logger.e("This method requires an open ISavedGameMetadata.");
				callback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			if (!androidSnapshotMetadata.JavaContents.Call<bool>("writeBytes", new object[1] { updatedBinaryData }))
			{
				GooglePlayGames.OurUtils.Logger.e("This method requires an open ISavedGameMetadata.");
				callback(SavedGameRequestStatus.BadInputError, null);
			}
			using (AndroidJavaObject androidJavaObject = AsMetadataChange(updateForMetadata))
			{
				using (AndroidJavaObject task = mSnapshotsClient.Call<AndroidJavaObject>("commitAndClose", new object[2] { androidSnapshotMetadata.JavaSnapshot, androidJavaObject }))
				{
					AndroidTaskUtils.AddOnSuccessListener(task, false, delegate(AndroidJavaObject snapshotMetadata)
					{
						Debug.Log("commitAndClose.succeed");
						callback(SavedGameRequestStatus.Success, new AndroidSnapshotMetadata(snapshotMetadata, null));
					});
					AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject exception)
					{
						Debug.Log("commitAndClose.failed: " + exception.Call<string>("toString", Array.Empty<object>()));
						SavedGameRequestStatus arg = (mAndroidClient.IsAuthenticated() ? SavedGameRequestStatus.InternalError : SavedGameRequestStatus.AuthenticationError);
						callback(arg, null);
					});
				}
			}
		}

		public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
		{
			Misc.CheckNotNull(callback);
			callback = ToOnGameThread(callback);
			using (AndroidJavaObject task = mSnapshotsClient.Call<AndroidJavaObject>("load", new object[1] { source == DataSource.ReadNetworkOnly }))
			{
				AndroidTaskUtils.AddOnSuccessListener(task, delegate(AndroidJavaObject annotatedData)
				{
					using (AndroidJavaObject androidJavaObject = annotatedData.Call<AndroidJavaObject>("get", Array.Empty<object>()))
					{
						int num = androidJavaObject.Call<int>("getCount", Array.Empty<object>());
						List<ISavedGameMetadata> list = new List<ISavedGameMetadata>();
						for (int i = 0; i < num; i++)
						{
							using (AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { i }))
							{
								list.Add(new AndroidSnapshotMetadata(androidJavaObject2.Call<AndroidJavaObject>("freeze", Array.Empty<object>()), null));
							}
						}
						androidJavaObject.Call("release");
						callback(SavedGameRequestStatus.Success, list);
					}
				});
				AddOnFailureListenerWithSignOut(task, delegate(AndroidJavaObject exception)
				{
					GooglePlayGames.OurUtils.Logger.d("FetchAllSavedGames failed: " + exception.Call<string>("toString", Array.Empty<object>()));
					SavedGameRequestStatus arg = (mAndroidClient.IsAuthenticated() ? SavedGameRequestStatus.InternalError : SavedGameRequestStatus.AuthenticationError);
					callback(arg, new List<ISavedGameMetadata>());
				});
			}
		}

		public void Delete(ISavedGameMetadata metadata)
		{
			AndroidSnapshotMetadata androidSnapshotMetadata = metadata as AndroidSnapshotMetadata;
			Misc.CheckNotNull(androidSnapshotMetadata);
			using (mSnapshotsClient.Call<AndroidJavaObject>("delete", new object[1] { androidSnapshotMetadata.JavaMetadata }))
			{
			}
		}

		private void AddOnFailureListenerWithSignOut(AndroidJavaObject task, Action<AndroidJavaObject> callback)
		{
			AndroidTaskUtils.AddOnFailureListener(task, delegate(AndroidJavaObject exception)
			{
				int num = exception.Call<int>("getStatusCode", Array.Empty<object>());
				if (num == 4 || num == 26502)
				{
					mAndroidClient.SignOut();
				}
				callback(exception);
			});
		}

		private ConflictCallback ToOnGameThread(ConflictCallback conflictCallback)
		{
			return delegate(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				GooglePlayGames.OurUtils.Logger.d("Invoking conflict callback");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					conflictCallback(resolver, original, originalData, unmerged, unmergedData);
				});
			};
		}

		internal static bool IsValidFilename(string filename)
		{
			if (filename == null)
			{
				return false;
			}
			return ValidFilenameRegex.IsMatch(filename);
		}

		private static AndroidJavaObject AsMetadataChange(SavedGameMetadataUpdate update)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.google.android.gms.games.snapshot.SnapshotMetadataChange$Builder"))
			{
				if (update.IsCoverImageUpdated)
				{
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.graphics.BitmapFactory"))
					{
						using (AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("decodeByteArray", new object[3]
						{
							update.UpdatedPngCoverImage,
							0,
							update.UpdatedPngCoverImage.Length
						}))
						{
							using (androidJavaObject.Call<AndroidJavaObject>("setCoverImage", new object[1] { androidJavaObject2 }))
							{
							}
						}
					}
				}
				if (update.IsDescriptionUpdated)
				{
					using (androidJavaObject.Call<AndroidJavaObject>("setDescription", new object[1] { update.UpdatedDescription }))
					{
					}
				}
				if (update.IsPlayedTimeUpdated)
				{
					using (androidJavaObject.Call<AndroidJavaObject>("setPlayedTimeMillis", new object[1] { Convert.ToInt64(update.UpdatedPlayedTime.Value.TotalMilliseconds) }))
					{
					}
				}
				return androidJavaObject.Call<AndroidJavaObject>("build", Array.Empty<object>());
			}
		}

		private static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
		{
			return delegate(T1 val1, T2 val2)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val1, val2);
				});
			};
		}
	}
}
