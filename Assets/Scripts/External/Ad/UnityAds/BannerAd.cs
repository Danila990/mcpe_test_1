#if UNITY_ADS_ENABLED

using System;
using Scripts.AD.AdsInterfaces;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Scripts.AD.UnityAds
{
	public class BannerAd : IBanner
	{
		private const string PlacementId = "Banner_Android";

		private bool _isBannerLoaded;

		public event Action<IBanner> OnLoadedEvent;

		public void LoadAd()
		{
			if(!Advertisement.isInitialized)
			{
				return;
			}

			BannerLoadOptions options = new BannerLoadOptions
			{
				loadCallback = OnLoaded,
				errorCallback = OnFailedToLoad,
			};

			Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
			Advertisement.Banner.Load(PlacementId, options);
		}

		public void Show()
		{
			Advertisement.Banner.Show(PlacementId);
		}

		public void Hide()
		{
			Advertisement.Banner.Hide();
		}

		private void OnLoaded()
		{
			if(_isBannerLoaded)
			{
				return;
			}

			Hide();
			OnLoadedEvent?.Invoke(this);
			_isBannerLoaded = true;
		}

		private void OnFailedToLoad(string message)
		{
			Debug.Log("UnityAds banner ad failed to load: " + message);
		}
	}
}
#endif