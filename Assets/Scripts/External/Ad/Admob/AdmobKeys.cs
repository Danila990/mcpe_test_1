using System;

namespace Scripts.AD.Admob
{
	[Serializable]
	public class AdmobKeys
	{
		public static AdmobKeys TestKeys = new AdmobKeys()
		{
			AppOpenAdId = "ca-app-pub-3940256099942544/9257395921",
			BannerAdId = "ca-app-pub-3940256099942544/6300978111",
			InterstitialAdId = "ca-app-pub-3940256099942544/1033173712",
			RewardedAdId = "ca-app-pub-3940256099942544/5224354917",
			NativeAdId = "ca-app-pub-3940256099942544/2247696110",
		};

		public string AppOpenAdId;
		public string BannerAdId;
		public string InterstitialAdId;
		public string RewardedAdId;
		public string NativeAdId;		
	}
}