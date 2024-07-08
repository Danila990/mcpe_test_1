using Scripts.AD;
using Scripts.AD.AdsInterfaces;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.AD.MediationsByAdType
{
	public class AppOpenAdWaterfall : AdWaterfallBaseClass
	{
		private readonly LinkedList<IAds> _appOpenAds = new LinkedList<IAds>();

		private bool _isAppOpenClosed = true;

		public override bool IsLoaded => GetLoadedAd() != null;

		public override void AddLastToWaterfall(IAds ads)
		{
			_appOpenAds.AddLast(ads);
		}

		public override void ShowAd(Action<AdsResult> resultAction)
		{
			if(!_isAppOpenClosed) 
			{
				return;
			}

			IAds loadedAd = GetLoadedAd();
			if(loadedAd != null)
			{
				_isAppOpenClosed = false;
				resultAction += adsResult => _isAppOpenClosed = true;
				loadedAd.ShowAd(resultAction);
			}
			else
			{
				resultAction?.Invoke(AdsResult.NotReady);
			}
		}

		private IAds GetLoadedAd()
		{
			foreach(IAds appOpenAd in _appOpenAds)
			{
				if(appOpenAd.IsLoaded)
				{
					return appOpenAd;
				}
			}

			return null;
		}
	}
}
