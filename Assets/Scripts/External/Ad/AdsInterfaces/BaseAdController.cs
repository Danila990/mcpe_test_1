using System;

namespace Scripts.AD
{
	[Serializable]
	public abstract class BaseAdController
	{
		public readonly AdsMediator AdsMediator;

		protected bool _isPersonalizeAds = false;

		protected BaseAdController(AdsMediator adsMediator)
		{
			AdsMediator = adsMediator;
		}

		public event Action<bool> OnPersonalizeAdsChange;

		public virtual bool IsPersonalizeAds
		{
			get
			{
				return _isPersonalizeAds;
			}

			set
			{
				_isPersonalizeAds = value;
				OnPersonalizeAdsChange?.Invoke(value);
			}
		}

		public abstract bool NeedShowConsentPage
		{
			get;
		}

		public abstract string Name
		{
			get;
		}

		public abstract string PrivacyPolicyLink
		{
			get;
		}
	}
}