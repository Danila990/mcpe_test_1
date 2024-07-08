using UnityEngine;

namespace Assets.Scripts.AD.Applovin
{
	[CreateAssetMenu(fileName = "ApplovinParameters", menuName = "Ads Settings/Applovin")]
	public class ApplovinParameters : ScriptableObject
	{
		[field: SerializeField]
		public string SdkKey
		{
			get;
			private set;
		}

		[field: SerializeField]
		public string AppOpenAdUnitId
		{
			get;
			private set;
		}

		[field: SerializeField]
		public string BannerAdUnitId
		{
			get;
			private set;
		}

		[field: SerializeField]
		public string InterstitialAdUnitId
		{
			get;
			private set;
		}

		[field: SerializeField]
		public string RewardedAdUnitId
		{
			get;
			private set;
		}
	}
}
