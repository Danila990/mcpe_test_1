#if UNITY_ADS_ENABLED

using System;
using Scripts.AD.AdsInterfaces;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Scripts.AD.UnityAds
{
	public class InterAd : IAds, IUnityAdsLoadListener, IUnityAdsShowListener
	{
		private const string PlacementId = "Interstitial_Android";

		private bool _isAdLoaded = false;
		private Action<AdsResult> _resultAction = null;

		public bool IsLoaded => _isAdLoaded;

		public void LoadAd()
		{
			if(!Advertisement.isInitialized)
			{
				return;
			}

			// IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
			_isAdLoaded = false;
			Advertisement.Load(PlacementId, this);
		}

		public void ShowAd(Action<AdsResult> resultAction)
		{
			_resultAction = resultAction;
			BlockScreen();
			if(IsLoaded)
			{
				Advertisement.Show(PlacementId, this);
			}
			else
			{
				OnAdResult(AdsResult.NotReady);
			}
		}

		public void OnUnityAdsAdLoaded(string placementId)
		{
			_isAdLoaded = true;
		}

		public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
		{
			_isAdLoaded = false;
		}

		public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
		{
			OnAdResult(AdsResult.Watched);
		}

		public void OnUnityAdsShowStart(string placementId)
		{
		}

		public void OnUnityAdsShowClick(string placementId)
		{
		}

		public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
		{
			Debug.Log($"UnityAds inter ad failed to show {error} {message}");
			OnAdResult(AdsResult.Error);
		}

		private void OnAdResult(AdsResult result)
		{
			UnblockScreen();
			_resultAction?.Invoke(result);
			_resultAction = null;
			LoadAd();
		}

		private void BlockScreen()
		{
			Time.timeScale = 0;
		}

		private void UnblockScreen()
		{
			Time.timeScale = 1;
		}
	}
}
#endif