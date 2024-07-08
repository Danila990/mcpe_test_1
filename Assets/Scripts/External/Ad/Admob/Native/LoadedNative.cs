using GoogleMobileAds.Api;

namespace Scripts.AD.Native
{
	public class LoadedNative
	{
		public readonly AdLoader AdLoader;
		public readonly NativeAd NativeAd;

		public LoadedNative(AdLoader adLoader, NativeAd nativeAd)
		{
			AdLoader = adLoader;
			NativeAd = nativeAd;
		}
	}
}