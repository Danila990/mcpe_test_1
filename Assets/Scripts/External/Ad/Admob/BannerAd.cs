#if ADMOB_ENABLED

using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Scripts.AD.AdsInterfaces;
using UnityEngine;

namespace Scripts.AD.Admob
{
	public class BannerAd : IBanner
	{
		private readonly Assets.Scripts.AD.Applovin.RequestRetryDelayer _retryDelayer = 
			new Assets.Scripts.AD.Applovin.RequestRetryDelayer(3);
		private readonly AdmobController _admobController;

		private BannerView _bannerAd;
		private bool _isLoading;
		private bool _isRecieveNoFill;

		public BannerAd(AdmobController admobController)
		{
			_admobController = admobController;
		}

		public event Action<IBanner> OnLoadedEvent;

		public void Hide()
		{
			_bannerAd.Hide();
		}

		public void Show()
		{
			_bannerAd.Show();
		}

		public void LoadAd()
		{
			if(_bannerAd != null)
			{
				_bannerAd.Destroy();
				_bannerAd = null;
			}

			var adId = _admobController.Keys.BannerAdId;
			if(string.IsNullOrEmpty(adId) || !_admobController.IsInitialized || _isLoading || _isRecieveNoFill)
			{
				return;
			}

			_isLoading = true;
			_bannerAd = new BannerView(adId, AdSize.Banner, AdPosition.Bottom);
			_bannerAd.OnBannerAdLoaded += OnLoaded;
			_bannerAd.OnBannerAdLoadFailed += OnFailedToLoad;
			_bannerAd.LoadAd(new AdRequest());
		}

		private void OnLoaded()
		{
			_retryDelayer.Reset();
			Hide();
			OnLoadedEvent?.Invoke(this);
			_bannerAd.OnBannerAdLoaded -= OnLoaded;
		}

		private async void OnFailedToLoad(LoadAdError error)
		{
			Debug.Log($"Admob banner ad LoadAdError message is {error.GetMessage()}");
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
	}
}
#endif