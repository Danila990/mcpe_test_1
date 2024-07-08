#if ADMOB_ENABLED

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AD.Applovin;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Scripts.AD.Native;
using UnityEngine;

namespace Scripts.AD.Admob
{
	public class NativeAdController
	{
		private readonly RequestRetryDelayer _retryDelayer = new RequestRetryDelayer(3);
		private readonly LinkedList<NativeComponentsContainer> _freeContainers = 
			new LinkedList<NativeComponentsContainer>();
		private readonly Queue<LoadedNative> _freeLoadedNative = new Queue<LoadedNative>();
		private readonly AdmobController _admobController;

		private bool _isLoading;
		private bool _isRecieveNoFill;

		public NativeAdController(AdmobController admobController)
		{
			_admobController = admobController;
		}

		public void AddFreeNative(NativeComponentsContainer nativeToRegister)
		{
			_freeContainers.AddLast(nativeToRegister);
			SetFill();
			LoadAd();
		}

		public void RemoveFreeNative(NativeComponentsContainer nativeToRegister)
		{
			_freeContainers.Remove(nativeToRegister);
		}

		public void LoadAd()
		{
			var adId = _admobController.Keys.NativeAdId;
			if(string.IsNullOrEmpty(adId) || !_admobController.IsInitialized ||
				_freeLoadedNative.Count >= _admobController.Parameters.NativePoolSize || 
				_isLoading || _isRecieveNoFill)
			{
				return;
			}

			_isLoading = true;
			AdLoader adLoader = new AdLoader.Builder(adId).ForNativeAd().Build();
			adLoader.OnNativeAdLoaded += OnLoaded;
			adLoader.OnAdFailedToLoad += OnFailedToLoad;
			adLoader.LoadAd(new AdRequest());
		}

		private void SetFill()
		{
			if(!_freeContainers.Any() || !_freeLoadedNative.Any())
			{
				return;
			}

			NativeComponentsContainer container = _freeContainers.First.Value;
			_freeContainers.RemoveFirst();
			LoadedNative loadedNative = _freeLoadedNative.Dequeue();
			container.SetFill(loadedNative.NativeAd);
			loadedNative.AdLoader.OnNativeAdClicked += (o, e) => OnNativeAdClicked(container);
		}

		// После загрузки нейтива, он добавляется в пул загруженных нейтивов и если есть gameObject, который
		// может отобразить этот нейтив, он будет отображён
		private void OnLoaded(object sender, NativeAdEventArgs e)
		{
			_isLoading = false;
			_retryDelayer.Reset();
			AdLoader adLoader = (AdLoader)sender;
            NativeAd nativeAd = e.nativeAd;
			var loadedNative = new LoadedNative(adLoader, nativeAd);
			_freeLoadedNative.Enqueue(loadedNative);
			MobileAdsEventExecutor.ExecuteInUpdate(() =>
			{
				SetFill();
				LoadAd();
			});
		}

		private void OnNativeAdClicked(NativeComponentsContainer container) 
		{
			if(_freeContainers.Contains(container))
			{
				return;
			}

			MobileAdsEventExecutor.ExecuteInUpdate(() =>
			{
				AddFreeNative(container);
			});
		}

		private async void OnFailedToLoad(object sender, AdFailedToLoadEventArgs e)
		{
			_isLoading = false;
			Debug.Log($"Admob native ad LoadAdError message is {e.LoadAdError.GetMessage()}");
			// 3 - это код No fill
			if(e.LoadAdError.GetCode() == 3)
			{
				_isRecieveNoFill = true;
				return;
			}

			await _retryDelayer.Wait();
			MobileAdsEventExecutor.ExecuteInUpdate(() =>
			{
				LoadAd();
			});
		}
	}
}
#endif