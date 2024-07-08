#if APPLOVIN_ENABLED

using Scripts.AD.AdsInterfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.AD.Applovin
{
	public class BannerAd : IBanner
	{
		private readonly string _adId;
		private readonly RequestRetryDelayer _retryDeleyer = new RequestRetryDelayer();

		private bool _isLoading;

		public BannerAd(string adId)
		{
			_adId = adId;
			MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnLoaded;
			MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnFailedToLoad;
		}

		public event Action<IBanner> OnLoadedEvent;

		public void Hide()
		{
			if(string.IsNullOrEmpty(_adId))
			{
				return;
			}

			MaxSdk.HideBanner(_adId);
		}

		public void Show()
		{
			if(string.IsNullOrEmpty(_adId))
			{
				return;
			}

			MaxSdk.ShowBanner(_adId);
		}

		public void LoadAd()
		{
			if(string.IsNullOrEmpty(_adId) || _isLoading)
			{
				return;
			}

			_isLoading = true;
			MaxSdk.CreateBanner(_adId, MaxSdkBase.BannerPosition.BottomCenter);
			MaxSdk.SetBannerBackgroundColor(_adId, new Color(0, 0, 0, 1));
		}

		private void OnLoaded(object sender, MaxSdk.AdInfo adInfo)
		{
			_isLoading = false;
			MaxSdkCallbacks.Banner.OnAdLoadedEvent -= OnLoaded;
			Hide();
			OnLoadedEvent?.Invoke(this);
		}

		private async void OnFailedToLoad(object sender, MaxSdk.ErrorInfo errorInfo)
		{
			_isLoading = false;
			Debug.Log("Applovin banner ad failed to load: " + errorInfo.Code);
			await _retryDeleyer.Wait();
			LoadAd();
		}
	}
}
#endif