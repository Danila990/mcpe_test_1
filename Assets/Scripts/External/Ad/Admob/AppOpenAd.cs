#if ADMOB_ENABLED

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Scripts.AD;
using Scripts.AD.Admob;
using Scripts.AD.AdsInterfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.AD.Admob
{
	public class AppOpenAdController : IAds
	{
		private readonly Applovin.RequestRetryDelayer _retryDelayer = new Applovin.RequestRetryDelayer(3);
		private readonly AdmobController _admobController;

		private AppOpenAd _appOpenAd;
		private Action<AdsResult> _resultAction = null;
		private bool _isLoading;
		private bool _isRecieveNoFill;

		public AppOpenAdController(AdmobController admobController)
		{
			_admobController = admobController;
		}

		public bool IsLoaded => _appOpenAd != null && _appOpenAd.CanShowAd();

		public void ShowAd(Action<AdsResult> resultAction)
		{
			_resultAction = resultAction;
			BlockScreen();
			if (IsLoaded)
			{
				_appOpenAd.Show();
			}
			else
			{
				OnAdResult(AdsResult.NotReady);
			}
		}

		public void LoadAd()
		{
			if (_appOpenAd != null)
			{
				_appOpenAd.Destroy();
				_appOpenAd = null;
			}

			var adId = _admobController.Keys.AppOpenAdId;
			if (string.IsNullOrEmpty(adId) || 
				!_admobController.IsInitialized || 
				_isLoading || 
				_isRecieveNoFill)
			{
				return;
			}

			_isLoading = true;
			AppOpenAd.Load(adId, new AdRequest(), AdCallback);
		}

		private void BlockScreen()
		{
			Time.timeScale = 0;
		}

		private void UnblockScreen()
		{
			Time.timeScale = 1;
		}

		private void OnAdResult(AdsResult result)
		{
			UnblockScreen();
			_resultAction?.Invoke(result);
			_resultAction = null;
		}

		private void AdCallback(AppOpenAd appOpenAd, LoadAdError error)
		{
			_isLoading = false;
			if(error != null || appOpenAd == null)
			{
				OnFailedToLoad(error);
			}
			else
			{
				OnLoaded(appOpenAd);
			}
		}

		private void OnLoaded(AppOpenAd appOpenAd)
		{
			_retryDelayer.Reset();
			_appOpenAd = appOpenAd;
			_appOpenAd.OnAdFullScreenContentClosed += OnClosed;
			_appOpenAd.OnAdFullScreenContentFailed += OnFailedToDisplay;
		}

		private async void OnFailedToLoad(LoadAdError error)
		{
			Debug.Log($"Admob app open ad LoadAdError message is {error.GetMessage()}");
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