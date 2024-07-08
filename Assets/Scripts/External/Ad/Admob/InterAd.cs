#if ADMOB_ENABLED

using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Scripts.AD.AdsInterfaces;
using UnityEngine;

namespace Scripts.AD.Admob
{
	public class InterAd : IAds
	{
		/// <summary>
		/// Это костыль из-за которого реклама не сразу показывается и некоторое время после нажатия на рекламу 
		/// пользователь может взаимодействовать с приложением
		/// </summary>
		public CanvasGroup BlockingImage;

		private readonly Assets.Scripts.AD.Applovin.RequestRetryDelayer _retryDelayer =
			new Assets.Scripts.AD.Applovin.RequestRetryDelayer(3);
		private readonly AdmobController _admobController;

		private InterstitialAd _interstitialAd;
		private Action<AdsResult> _resultAction = null;
		private bool _isLoading;
		private bool _isRecieveNoFill;

		public InterAd(AdmobController admobController)
		{
			_admobController = admobController;
		}

		public bool IsLoaded => _interstitialAd != null && _interstitialAd.CanShowAd();

		public void ShowAd(Action<AdsResult> resultAction)
		{
			_resultAction = resultAction;
			BlockScreen();
			if(IsLoaded)
			{
				_interstitialAd.Show();
			}
			else
			{
				OnAdResult(AdsResult.NotReady);
			}
		}

		public void LoadAd()
		{
			if(_interstitialAd != null)
			{
				_interstitialAd.Destroy();
				_interstitialAd = null;
			}

			var adId = _admobController.Keys.InterstitialAdId;
			if(string.IsNullOrEmpty(adId) || !_admobController.IsInitialized || _isLoading || _isRecieveNoFill)
			{
				return;
			}

			_isLoading = true;
			InterstitialAd.Load(adId, new AdRequest(), AdCallback);
		}

		private void BlockScreen()
		{
			Time.timeScale = 0;
			BlockingImage.blocksRaycasts = true;
		}

		private void UnblockScreen()
		{
			Time.timeScale = 1;
			BlockingImage.blocksRaycasts = false;
		}

		private void OnAdResult(AdsResult result)
		{
			UnblockScreen();
			_resultAction?.Invoke(result);
			_resultAction = null;
		}

		private void AdCallback(InterstitialAd interstitialAd, LoadAdError error)
		{
			_isLoading = false;
			if(error != null || interstitialAd == null)
			{
				OnFailedToLoad(error);
			}
			else 
			{
				OnLoaded(interstitialAd);
			}			
		}

		private void OnLoaded(InterstitialAd interstitialAd)
		{
			_retryDelayer.Reset();
			_interstitialAd = interstitialAd;
			_interstitialAd.OnAdFullScreenContentClosed += OnClosed;
			_interstitialAd.OnAdFullScreenContentFailed += OnFailedToDisplay;
		}

		private async void OnFailedToLoad(LoadAdError error)
		{
			Debug.Log($"Admob inter ad LoadAdError message is {error.GetMessage()}");
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

		private void OnFailedToDisplay(AdError error) 
		{
			MobileAdsEventExecutor.ExecuteInUpdate(() =>
			{
				OnAdResult(AdsResult.Error);
				LoadAd();
			});
		}

		private void OnClosed()
		{
			MobileAdsEventExecutor.ExecuteInUpdate(() =>
			{
				OnAdResult(AdsResult.Watched);
				LoadAd();
			});
		}
	}
}
#endif