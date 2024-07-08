using System;

namespace Scripts.AD.AdsInterfaces
{
	public interface IAds
	{
		public void ShowAd(Action<AdsResult> resultAction);

		public bool IsLoaded
		{
			get;
		}

		public void LoadAd();
	}
}