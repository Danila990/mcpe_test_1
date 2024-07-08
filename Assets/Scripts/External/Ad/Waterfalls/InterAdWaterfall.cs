using System;
using System.Collections.Generic;
using Scripts.AD.AdsInterfaces;

namespace Scripts.AD
{
	public class InterAdWaterfall : AdWaterfallBaseClass
	{
		private readonly LinkedList<IAds> _interAds = new LinkedList<IAds>();

		public override bool IsLoaded => GetLoadedAd() != null;

		public override void ShowAd(Action<AdsResult> resultAction = null)
		{
			IAds loadedAd = GetLoadedAd();
			if(loadedAd != null)
			{
				loadedAd.ShowAd(resultAction);
			}
			else
			{
				resultAction?.Invoke(AdsResult.NotReady);
			}
		}

		public override void AddLastToWaterfall(IAds ads)
		{
			_interAds.AddLast(ads);
		}

		private IAds GetLoadedAd()
		{
			foreach(IAds interAd in _interAds)
			{
				if(interAd.IsLoaded)
				{
					return interAd;
				}
			}

			return null;
		}
	}
}