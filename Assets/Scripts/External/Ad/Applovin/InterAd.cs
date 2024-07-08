#if APPLOVIN_ENABLED

using Scripts.AD;
using Scripts.AD.AdsInterfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.AD.Applovin
{
	public class InterAd : IAds
	{
		private readonly string _adId;
		private readonly RequestRetryDelayer _retryDeleyer = new RequestRetryDelayer();

		private Action<AdsResult> _resultAction = null;
		private bool _isLoading;

		public InterAd(string adId)
		{
			_adId = adId;
			MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnLoaded;
			MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnFailedToLoad;
			MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnFailedToDisplay;
			MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnClosed;
		}

		public bool IsLoaded => !string.IsNullOrEmpty(_adId) && 
			MaxSdk.IsInterstitialReady(_adId);

		public void LoadAd()
		{
			if(string.IsNullOrEmpty(_adId) || _isLoading)
			{
				return;
			}

			_isLoading = true;
			MaxSdk.LoadInterstitial(_adId);
		}

		public void ShowAd(Action<AdsResult> resultAction)
		{
			_resultAction = resultAction;
			BlockScreen();
			if(IsLoaded)
			{
				MaxSdk.ShowInterstitial(_adId);
			}
			else
			{
				OnAdResult(AdsResult.NotReady);
			}
		}

		private void OnAdResult(AdsResult result)
		{
			UnblockScreen();
			_resultAction?.Invoke(result);
			_resultAction = null;
		}

		private void OnLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			_isLoading = false;
			_retryDeleyer.Reset();
		}

		private async void OnFailedToLoad(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
		{
			_isLoading = false;
			Debug.Log("Applovin inter failed to load: " + errorInfo.Code);
			await _retryDeleyer.Wait();
			LoadAd();
		}

		private async void OnFailedToDisplay(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
		{
			// Interstitial ad failed to display. We recommend loading the next ad
			Debug.Log("Applovin inter failed to display: " + errorInfo.Code);
			OnAdResult(AdsResult.Error);
			await _retryDeleyer.Wait();
			LoadAd();
		}

		private void OnClosed(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			OnAdResult(AdsResult.Watched);
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