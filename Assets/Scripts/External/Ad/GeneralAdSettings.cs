using Assets.Scripts.AD.Applovin;
using UnityEngine;

namespace Scripts.AD.Settings
{
	[CreateAssetMenu(fileName = "GeneralAdSettings", menuName = "Ads Settings/General")]
	public class GeneralAdSettings : ScriptableObject
	{
		[field: SerializeField]
		public bool IsTestAds
		{
			get;
			private set;
		}

		[field: SerializeField, Min(0)]
		public float AdCoolDown
		{
			get;
			private set;
		} = 5;

		[field: SerializeField]
		public ApplovinParameters ApplovinSettings
		{
			get;
			private set;
		}

		[field: SerializeField]
		public AdmobParameters AdmobSettings
		{
			get;
			private set;
		}

		[field: SerializeField]
		public UnityAdsParameters UnityAdsSettings
		{
			get;
			private set;
		}
	}
}