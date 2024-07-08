#if APPLOVIN_ENABLED

using Scripts.AD;
using Scripts.AD.AdsInterfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.AD.Applovin
{
	public class AppOpenAd : IAds
	{
		private readonly string _adId;
		private readonly RequestRetryDelayer _retryDeleyer = new RequestRetryDelayer();

		private Action<AdsResult> _resultAction = null;
		private bool _isLoading;

		public AppOpenAd(string adId)
		{
			_adId = adId;
			MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnLoaded;
			MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnFailedToLoad;
			MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnFailedToDisplay;
			MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnClosed;
		}

		public bool IsLoaded => !string.IsNullOrEmpty(_adId) && MaxSdk.IsAppOpenAdReady(_adId);

		public void LoadAd()
		{
			if(string.IsNullOrEmpty(_adId) || _isLoading)
			{
				return;
			}

			_isLoading = true;
			MaxSdk.LoadAppOpenAd(_adId);
		}

		public void ShowAd(Action<AdsResult> resultAction)
		{
			_resultAction = resultAction;
			BlockScreen();
			if(IsLoaded)
			{
				MaxSdk.ShowAppOpenAd(_adId);
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
			Debug.Log("Applovin appOpen failed to load: " + errorInfo.Code);
			await _retryDeleyer.Wait();
			LoadAd();
		}

		private async void OnFailedToDisplay(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
		{
			Debug.Log("Applovin appOpen failed to display: " + errorInfo.Code);
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