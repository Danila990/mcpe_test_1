using Scripts.AD.Admob;
using UnityEngine;

namespace Scripts.AD.Settings
{
	[CreateAssetMenu(fileName = "AdmobParameters", menuName = "Ads Settings/Admob")]
	public class AdmobParameters : ScriptableObject
	{
		[field: SerializeField]
		public AdmobKeys Keys
		{
			get;
			private set;
		}

		[field: SerializeField]
		public int NativePoolSize 
		{
			get;
			private set;
		}

		[field: SerializeField]
		public LayerMask NativeDisablerLayer
		{
			get;
			private set;
		}
	}
}
