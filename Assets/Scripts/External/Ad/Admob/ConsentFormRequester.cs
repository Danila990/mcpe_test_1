using GoogleMobileAds.Ump.Api;
using System;
using UnityEngine;

namespace Assets.Scripts.AD.Admob
{
	public class ConsentFormRequester : MonoBehaviour
    {
        private bool _needInvokeCallback = false;

        private event Action _callback;

        public static ConsentFormRequester Instance 
        {
            get;
            private set;
        }

        private void Awake()
		{
            Instance = this;    
		}

        private void Update()
        {
            if(!_needInvokeCallback)
            {
                return;
            }

            _callback?.Invoke();
            _callback = null;
            _needInvokeCallback = false;
        }

        /// <param name="consentCallback">Callback будет вызван в любом из случаев</param>
        public void RequestConsent(Action consentCallback) 
        {
            _callback = consentCallback;
            // Set tag for under age of consent.
            // Here false means users are not under age of consent.
            var request = new ConsentRequestParameters();
            request.TagForUnderAgeOfConsent = false;
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

        private void OnConsentInfoUpdated(FormError formError)
        {
            if (formError != null)
            {
                SetDirtyNeedInvokeCallback();
                Debug.LogError(formError.Message);
                AppMetrica.Instance.ReportUnhandledException(new Exception(formError.Message));
                return;
            }

            // If the error is null, the consent information state was updated.
            // You are now ready to check if a form is available.
            ConsentForm.LoadAndShowConsentFormIfRequired(OnConsentFormDissmised);
        }

        private void OnConsentFormDissmised(FormError formError)
        {
            SetDirtyNeedInvokeCallback();
            if(formError != null)
            {
                // Consent gathering failed.
                Debug.LogError(formError.Message);
                AppMetrica.Instance.ReportError(formError.Message, Environment.StackTrace);
                return;
            }

            // Consent has been gathered.
            //if(ConsentInformation.CanRequestAds())
            //{
            //    SetDirtyInvokeCanRequestAdsCallback();
            //}
        }

        private void SetDirtyNeedInvokeCallback() 
        {
            _needInvokeCallback = true;
        }
    }
}
