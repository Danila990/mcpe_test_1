using Scripts.AD.AdsInterfaces;
using System;

namespace Scripts.AD
{
	public abstract class AdWaterfallBaseClass
	{
		public abstract bool IsLoaded
		{
			get;
		}

		public abstract void ShowAd(Action<AdsResult> resultAction);

		public abstract void AddLastToWaterfall(IAds ads);
	}
}