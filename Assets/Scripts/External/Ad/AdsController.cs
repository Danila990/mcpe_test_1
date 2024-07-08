using System;
using Scripts.AD.Settings;
using UnityEngine;
using Assets.Scripts.AD.MediationsByAdType;
using Assets.Scripts.AD;
using Assets.Scripts.AD.Admob;
#if APPLOVIN_ENABLED
using Assets.Scripts.AD.Applovin;
#endif
#if ADMOB_ENABLED
using Scripts.AD.Admob;
#endif
#if UNITY_ADS_ENABLED
using Scripts.AD.UnityAds;
#endif

namespace Scripts.AD
{
	public class AdsController : MonoBehaviour
	{
		[SerializeField] private GeneralAdSettings adSettings;
		[SerializeField] private CanvasGroup blockingImage;

		private AppOpenAdWaterfall _appOpenAd;
		private BannerAdWaterfall _bannerAd;
		private InterAdWaterfall _interAd;
		private RewardAdWaterfall _rewardAd;
		private AdsTimer _timer;
		private bool _isAdShowing;

		public static AdsController Instance
		{
			get;
			private set;
		}

#if APPLOVIN_ENABLED
		public ApplovinController ApplovinController
		{
			get;
			private set;
		}
#endif

#if ADMOB_ENABLED
		public AdmobController AdmobController
		{
			get;
			private set;
		}
#endif

#if UNITY_ADS_ENABLED
		public UnityAdsController UnityAdsController
		{
			get;
			private set;
		}
#endif

		private void Awake()
		{
			Instance = this;
			_timer = new AdsTimer(adSettings.AdCoolDown);
			_appOpenAd = new AppOpenAdWaterfall();
			_bannerAd = new BannerAdWaterfall();
			_interAd = new InterAdWaterfall();
			_rewardAd = new RewardAdWaterfall();

#if APPLOVIN_ENABLED
			ApplovinController = new ApplovinController(adSettings.ApplovinSettings);
#endif
#if ADMOB_ENABLED
			AdmobController = new AdmobController(adSettings.AdmobSettings, adSettings.IsTestAds);
#endif
#if UNITY_ADS_ENABLED
			UnityAdsController = new UnityAdsController(adSettings.UnityAdsSettings, adSettings.IsTestAds);
#endif
		}

		private void Start()
		{
			ConsentFormRequester.Instance.RequestConsent(StartLoadingAds);
		}

		public void StartLoadingAds()
		{
#if APPLOVIN_ENABLED
			AddApplovinToWaterfall();
			ApplovinController.Initialize();
#endif
#if ADMOB_ENABLED
			AddAdmobToWaterfall();
			AdmobController.Initialize();
#endif
#if UNITY_ADS_ENABLED
			AddUnityAdsToWaterfall();
			UnityAdsController.StartLoadingAds();
#endif
		}

		private void OnApplicationFocus(bool focus)
		{
			if(!focus)
			{
				ShowAppOpenAd();
			}
		}

		public void ShowBanner()
		{
			_bannerAd.Show();
		}

		public void HideBanner()
		{
			_bannerAd.Hide();
		}

		public void ShowAppOpenAd(Action<AdsResult> resultAction = null)
		{
			if(!_timer.IsTimerEnd || _isAdShowing)
			{
				resultAction?.Invoke(AdsResult.NotReady);
				return;
			}

			resultAction += StartTimerIfAdsWatched;
			resultAction += adsResult => SetAdNotShowing();
			SetAdShowing();
			_appOpenAd.ShowAd(resultAction);
		}

		public void ShowInterAd(Action<AdsResult> resultAction = null)
		{
			if(!_timer.IsTimerEnd || _isAdShowing)
			{
				resultAction?.Invoke(AdsResult.NotReady);
				return;
			}

			resultAction += StartTimerIfAdsWatched;
			resultAction += adsResult => SetAdNotShowing();
			SetAdShowing();
			_interAd.ShowAd(resultAction);
		}

		public void ShowRewardAd(Action<AdsResult> resultAction = null)
		{
			if(!_timer.IsTimerEnd || _isAdShowing)
			{
				resultAction?.Invoke(AdsResult.NotReady);
				return;
			}

			resultAction += StartTimerIfAdsWatched;
			resultAction += adsResult => SetAdNotShowing();
			SetAdShowing();
			_rewardAd.ShowAd(resultAction);
		}

		private void StartTimerIfAdsWatched(AdsResult adsResult)
		{
			if(adsResult == AdsResult.Watched)
			{
				_timer.StartTimer();
			}
		}

		private void SetAdShowing()
		{
			_isAdShowing = true;
		}

		private void SetAdNotShowing()
		{
			_isAdShowing = false;
		}

#if APPLOVIN_ENABLED
		private void AddApplovinToWaterfall()
		{
			_appOpenAd.AddLastToWaterfall(ApplovinController.AppOpenAd);
			_bannerAd.AddLastToWaterfall(ApplovinController.BannerAd);
			_interAd.AddLastToWaterfall(ApplovinController.InterAd);
			_rewardAd.AddLastToWaterfall(ApplovinController.RewardAd);
		}
#endif

#if ADMOB_ENABLED
		private void AddAdmobToWaterfall()
		{
			AdmobController.InterAd.BlockingImage = blockingImage;

			_appOpenAd.AddLastToWaterfall(AdmobController.AppOpenAd);
			_bannerAd.AddLastToWaterfall(AdmobController.BannerAd);
			_interAd.AddLastToWaterfall(AdmobController.InterAd);
			_rewardAd.AddLastToWaterfall(AdmobController.RewardAd);
		}
#endif

#if UNITY_ADS_ENABLED
		private void AddUnityAdsToWaterfall()
		{
			_bannerAd.AddLastToWaterfall(UnityAdsController.BannerAd);
			_interAd.AddLastToWaterfall(UnityAdsController.InterAd);
			_rewardAd.AddLastToWaterfall(UnityAdsController.RewardAd);
		}
#endif
	}
}