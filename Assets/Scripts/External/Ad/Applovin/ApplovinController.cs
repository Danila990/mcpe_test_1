#if APPLOVIN_ENABLED

using Scripts.AD;
using System;
using UnityEngine;

namespace Assets.Scripts.AD.Applovin
{
	public class ApplovinController
	{
		public readonly ApplovinParameters Parameters;

		public readonly AppOpenAd AppOpenAd;
		public readonly BannerAd BannerAd;
		public readonly InterAd InterAd;
		public readonly RewardAd RewardAd;

		public ApplovinController(ApplovinParameters parameters)
		{
			Parameters = parameters;
			AppOpenAd = new AppOpenAd(Parameters.AppOpenAdUnitId);
			BannerAd = new BannerAd(Parameters.BannerAdUnitId);
			InterAd = new InterAd(Parameters.InterstitialAdUnitId);
			RewardAd = new RewardAd(Parameters.RewardedAdUnitId);
		}

		public void Initialize()
		{
			MaxSdkCallbacks.OnSdkInitializedEvent += (configuration) =>
			{
				AppOpenAd.LoadAd();
				BannerAd.LoadAd();
				InterAd.LoadAd();
				RewardAd.LoadAd();
			};

			MaxSdk.SetIsAgeRestrictedUser(false);
			MaxSdk.SetSdkKey(Parameters.SdkKey);
			MaxSdk.InitializeSdk();
		}
	}
}
#endif