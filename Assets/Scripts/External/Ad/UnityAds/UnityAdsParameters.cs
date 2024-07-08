using UnityEngine;

namespace Scripts.AD.Settings
{
	[CreateAssetMenu(fileName = "UnityAdsParameters", menuName = "Ads Settings/Unity Ads")]
	public class UnityAdsParameters : ScriptableObject
	{
		[field: SerializeField]
		public string GameId
		{
			get;
			private set;
		}
	}
}
