#if ADMOB_ENABLED

using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Scripts.AD.AdsInterfaces;
using UnityEngine;

namespace Scripts.AD.Admob
{
	public class RewardAd : IAds
	{
		private readonly Assets.Scripts.AD.Applovin.RequestRetryDelayer _retryDelayer =
			new Assets.Scripts.AD.Applovin.RequestRetryDelayer(3);
		private readonly AdmobController _admobController;

		private RewardedAd _rewardedAd;
		private Action<AdsResult> _resultAction = null;
		private bool _isLoading;
		private bool _isRecieveNoFill;

		public RewardAd(AdmobController admobController)
		{
			_admobController = admobController;
		}

		public bool IsLoaded => _rewardedAd != null && _rewardedAd.CanShowAd();

		public void ShowAd(Action<AdsResult> resultAction)
		{
			_resultAction = resultAction;
			if(IsLoaded)
			{
				_rewardedAd.Show(HandleUserEarnedReward);
			}
			else
			{
				OnAdResult(AdsResult.NotReady);
			}
		}

		public void LoadAd()
		{
			if(_rewardedAd != null)
			{
				_rewardedAd.Destroy();
				_rewardedAd = null;
			}

			var adId = _admobController.Keys.RewardedAdId;
			if(string.IsNullOrEmpty(adId) || !_admobController.IsInitialized || _isLoading || _isRecieveNoFill)
			{
				return;
			}

			_isLoading = true;
			RewardedAd.Load(adId, new AdRequest(), AdCallback);
		}

		private void AdCallback(RewardedAd rewardedAd, LoadAdError error)
		{
			_isLoading = false;
			if(error != null || rewardedAd == null)
			{
				OnFailedToLoad(error);
			}
			else
			{
				OnLoaded(rewardedAd);
			}
		}

		private void OnLoaded(RewardedAd rewardedAd)
		{
			_retryDelayer.Reset();
			_rewardedAd = rewardedAd;
			_rewardedAd.OnAdFullScreenContentFailed += OnFailedToDisplay;
		}

		private async void OnFailedToLoad(LoadAdError error)
		{
			Debug.Log($"Admob reward ad LoadAdError message is {error.GetMessage()}");
			// 3 - это код No fill
			if(error.GetCode() == 3)
			{
				_isRecieveNoFill = true;
				return;
			}

			await _retryDelayer.Wait();
			MobileAdsEventExecutor.ExecuteInUpdate(() =>
			{
				LoadAd();
			});
		}

		private void HandleUserEarnedReward(Reward reward)
		{
			MobileAdsEventExecutor.ExecuteInUpdate(() =>
			{
				OnAdResult(AdsResult.Watched);
				LoadAd();
			});
		}

		private void OnFailedToDisplay(AdError error)
		{
			MobileAdsEventExecutor.ExecuteInUpdate(() =>
			{
				OnAdResult(AdsResult.Error);
				LoadAd();
			});
		}

		private void OnAdResult(AdsResult result)
		{
			_resultAction?.Invoke(result);
			_resultAction = null;
		}
	}
}
#endif