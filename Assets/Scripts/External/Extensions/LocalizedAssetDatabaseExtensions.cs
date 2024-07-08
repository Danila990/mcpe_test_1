using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Scripts.Extensions
{
	public static class LocalizedAssetDatabaseExtensions
	{
		public static AsyncOperationHandle<T> GetLocalizedAssetAsync<T>(this
			LocalizedAsset<T> localizedReference, Locale locale) where T : Object
		{
			TableReference tableReference = localizedReference.TableReference;
			TableEntryReference tableEntryReference = localizedReference.TableEntryReference;
			return LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<T>(tableReference, 
				tableEntryReference, locale);
		}
	}
}
