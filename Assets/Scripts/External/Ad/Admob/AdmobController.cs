#if ADMOB_ENABLED
using System;
using System.Security.Cryptography;
using Assets.Scripts.AD.Admob;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Scripts.AD.Settings;

namespace Scripts.AD.Admob
{
	[Serializable]
	public class AdmobController
	{
		public readonly AdmobParameters Parameters;
		public readonly AdmobKeys Keys;
		public readonly AppOpenAdController AppOpenAd;
		public readonly BannerAd BannerAd;
		public readonly InterAd InterAd;
		public readonly RewardAd RewardAd;
		public readonly NativeAdController NativeAdController;

		public AdmobController(AdmobParameters parameters, bool isTest)
		{
			Parameters = parameters;
			Keys = isTest ? AdmobKeys.TestKeys : parameters.Keys;
			AppOpenAd = new AppOpenAdController(this);
			BannerAd = new BannerAd(this);
			InterAd = new InterAd(this);
			RewardAd = new RewardAd(this);
			NativeAdController = new NativeAdController(this);
		}

		public bool IsInitialized 
		{
			get;
			private set;
		}

		public void Initialize()
		{
			MobileAds.Initialize(OnInitializationComplete);
		}

		private void OnInitializationComplete(InitializationStatus status) 
		{
			IsInitialized = true;
			MobileAdsEventExecutor.ExecuteInUpdate(() =>
			{
				LoadAd();
			});
		}

		private void LoadAd() 
		{
			AppOpenAd.LoadAd();
			BannerAd.LoadAd();
			InterAd.LoadAd();
			RewardAd.LoadAd();
			NativeAdController.LoadAd();
		}
	}
}
#endif