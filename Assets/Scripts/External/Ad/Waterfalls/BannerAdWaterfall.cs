using Scripts.AD.AdsInterfaces;
using System.Collections.Generic;

namespace Scripts.AD
{
	public class BannerAdWaterfall
	{
		private readonly LinkedList<IBanner> _bannerAds = new LinkedList<IBanner>();

		private IBanner _firstLoadedBanner;
		private bool _isShow = true;

		public void AddLastToWaterfall(IBanner banner)
		{
			_bannerAds.AddLast(banner);
			banner.OnLoadedEvent += InitializeFirstLoadedBanner;
		}

		public void Show()
		{
			_isShow = true;
			_firstLoadedBanner?.Show();
		}

		public void Hide()
		{
			_isShow = false;
			_firstLoadedBanner?.Hide();
		}

		private void InitializeFirstLoadedBanner(IBanner firstLoadedBanner)
		{
			firstLoadedBanner.OnLoadedEvent -= InitializeFirstLoadedBanner;
			if(_firstLoadedBanner != null)
			{
				firstLoadedBanner.Hide();
				return;
			}

			_firstLoadedBanner = firstLoadedBanner;
			if(_isShow)
			{
				firstLoadedBanner.Show();
			}
		}
	}
}