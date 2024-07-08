using System;
using System.Collections.Generic;
using Scripts.AD.AdsInterfaces;

namespace Scripts.AD
{
	public class RewardAdWaterfall : AdWaterfallBaseClass
	{
		private readonly LinkedList<IAds> _rewardedAds = new LinkedList<IAds>();

		public override bool IsLoaded => GetLoadedAd() != null;

		public override void ShowAd(Action<AdsResult> resultAction)
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
			_rewardedAds.AddLast(ads);
		}

		private IAds GetLoadedAd()
		{
			foreach(IAds rewardedAd in _rewardedAds)
			{
				if(rewardedAd.IsLoaded)
				{
					return rewardedAd;
				}
			}

			return null;
		}
	}
}