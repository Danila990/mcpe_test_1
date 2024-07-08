#if APPLOVIN_ENABLED

using Scripts.AD;
using Scripts.AD.Admob;
using Scripts.AD.AdsInterfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.AD.Applovin
{
	public class RewardAd : IAds
	{
		private readonly string _adId;
		private readonly RequestRetryDelayer _retryDeleyer = new RequestRetryDelayer();

		private Action<AdsResult> _resultAction = null;
		private bool _isLoading;

		public RewardAd(string adId)
		{
			_adId = adId;
			MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnLoaded;
			MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnFailedToLoad;
			MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnFailedToDisplay;
			MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnClosed;
			MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnWatched;
		}

		public bool IsLoaded => !string.IsNullOrEmpty(_adId) && 
			MaxSdk.IsRewardedAdReady(_adId);

		public void ShowAd(Action<AdsResult> resultAction)
		{
			_resultAction = resultAction;
			// Фикс ArgumentOutOfRangeException(когда пользователь нажимает кнопку назад,
			// но реклама не кончилась и получается, что закрываются окна)
			Time.timeScale = 0;
			if(IsLoaded)
			{
				MaxSdk.ShowRewardedAd(_adId);
			}
			else
			{
				OnAdResult(AdsResult.NotReady);
			}
		}

		public void LoadAd()
		{
			if(string.IsNullOrEmpty(_adId) || _isLoading)
			{
				return;
			}

			_isLoading = true;
			MaxSdk.LoadRewardedAd(_adId);
		}

		private void OnLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			_isLoading = false;
			_retryDeleyer.Reset();
		}

		private async void OnFailedToLoad(object sender, MaxSdk.ErrorInfo args)
		{
			_isLoading = false;
			Debug.Log("Applovin reward ad failed to load: " + args.Code);
			await _retryDeleyer.Wait();
			LoadAd();
		}

		private async void OnFailedToDisplay(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
		{
			// Rewarded ad failed to display. We recommend loading the next ad
			Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
			OnAdResult(AdsResult.Error);
			await _retryDeleyer.Wait();
			LoadAd();
		}

		private void OnClosed(string s, MaxSdk.AdInfo e)
		{
			Time.timeScale = 1;
		}

		private void OnWatched(string s, MaxSdk.Reward reward, MaxSdk.AdInfo e)
		{
			OnAdResult(AdsResult.Watched);
			LoadAd();
		}

		private void OnAdResult(AdsResult result)
		{
			Time.timeScale = 1;
			_resultAction?.Invoke(result);
			_resultAction = null;
		}
	}
}
#endif