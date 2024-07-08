#if UNITY_ADS_ENABLED

using Scripts.AD.Settings;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Scripts.AD.UnityAds
{
	[Serializable]
	public class UnityAdsController : IUnityAdsInitializationListener
	{
		public readonly UnityAdsParameters Parameters;

		public readonly BannerAd BannerAd;
		public readonly InterAd InterAd;
		public readonly RewardAd RewardAd;

		private readonly bool _isTestAd;

		public UnityAdsController(UnityAdsParameters parameters, bool isTest)
		{
			Parameters = parameters;
			_isTestAd = isTest;

			BannerAd = new BannerAd();
			InterAd = new InterAd();
			RewardAd = new RewardAd();
		}

		public event Action OnInitializeComplete;

		public void StartLoadingAds()
		{
			Advertisement.Initialize(Parameters.GameId, _isTestAd, this);
		}

		public void OnInitializationComplete()
		{
            BannerAd.LoadAd();
            InterAd.LoadAd();
            RewardAd.LoadAd();
			OnInitializeComplete?.Invoke();
		}

		public void OnInitializationFailed(UnityAdsInitializationError error, string message)
		{
			Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
		}
	}
}
#endif