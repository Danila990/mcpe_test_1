#if ADMOB_ENABLED

using Assets.Scripts.Extensions;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.AD.Native
{
	public class NativeComponentsContainer : MonoBehaviour
	{
		private readonly List<NativeAdBoxColliderDisabler> _disablers = new List<NativeAdBoxColliderDisabler>();

		[SerializeField] private GameObject _loadingState;
		[SerializeField] private TextMeshProUGUI _advertiserText;
		[SerializeField] private TextMeshProUGUI _bodyText;
		[SerializeField] private TextMeshProUGUI _headlineText;
		[SerializeField] private TextMeshProUGUI _buttonText;
		[SerializeField] private GameObject _button;
		[SerializeField] private Image _iconTexture;
		[SerializeField] private Image _imageTexture;

		private void Awake()
		{
			var nativeElements = new List<MonoBehaviour>
			{
				_advertiserText,
				_bodyText,
				_headlineText,
				_buttonText,
				_iconTexture,
				_imageTexture,
			};

			foreach(var nativeElement in nativeElements)
			{
				if(nativeElement != null)
				{
					_disablers.Add(CreateDisabler(nativeElement.gameObject));
				}
			}
		}

		private void Start()
		{
			AdsController.Instance.AdmobController.NativeAdController.AddFreeNative(this);
			SetLoadingState(true);
		}

		private void OnDestroy()
		{
			AdsController.Instance.AdmobController.NativeAdController.RemoveFreeNative(this);
		}

		public void SetFill(NativeAd nativeAd)
		{
			SetLoadingState(false);
			GameObject advertiserText = SetAdvertiserText(nativeAd.GetAdvertiserText());
			if(advertiserText != null)
			{
				nativeAd.RegisterAdvertiserTextGameObject(advertiserText);
			}

			GameObject bodyText = SetBodyText(nativeAd.GetBodyText());
			if(bodyText != null)
			{
				nativeAd.RegisterBodyTextGameObject(bodyText);
			}

			GameObject headlineText = SetHeadlineText(nativeAd.GetHeadlineText());
			if(headlineText != null)
			{
				nativeAd.RegisterHeadlineTextGameObject(headlineText);
			}

			GameObject buttonText = SetButtonText(nativeAd.GetCallToActionText());
			if(buttonText != null)
			{
				nativeAd.RegisterCallToActionGameObject(buttonText);
			}

			GameObject iconTexture = SetIconTexture(nativeAd.GetIconTexture());
			if(iconTexture != null)
			{
				nativeAd.RegisterIconImageGameObject(iconTexture);
			}

			List<GameObject> imageTexture = SetImageTexture(nativeAd.GetImageTextures());
			if(imageTexture != null && imageTexture.Any() && imageTexture[0] != null)
			{
				nativeAd.RegisterImageGameObjects(imageTexture);
			}
		}

		private static NativeAdBoxColliderDisabler CreateDisabler(GameObject toCreateDisabler)
		{
			GameObject disabler = new GameObject(toCreateDisabler.name + " BoxColliderDisabler");
			var disablerTransform = disabler.AddComponent<RectTransform>();
			disablerTransform.SetParent(toCreateDisabler.transform.parent, false);
			disablerTransform.CopySizeAndPosition((RectTransform)toCreateDisabler.transform);
			disabler.AddComponent<BoxCollider>();
			var disablerComponent = disabler.AddComponent<NativeAdBoxColliderDisabler>();
			disablerComponent.DependentCollider = toCreateDisabler.GetComponent<BoxCollider>();
			return disablerComponent;
		}

		private void SetLoadingState(bool isLoading)
		{
			_loadingState.gameObject.SetActive(isLoading);
		}

		private GameObject SetAdvertiserText(string text)
		{
			return SetText(_advertiserText, text);
		}

		private GameObject SetBodyText(string text)
		{
			return SetText(_bodyText, text);
		}

		private GameObject SetHeadlineText(string text)
		{
			return SetText(_headlineText, text);
		}

		private GameObject SetButtonText(string text)
		{
			return SetText(_buttonText, text);
		}

		private GameObject SetIconTexture(Texture2D texture)
		{
			GameObject componentGameObject = SetTexture(_iconTexture, texture);
			return componentGameObject;
		}

		private List<GameObject> SetImageTexture(List<Texture2D> textures)
		{
			if(textures == null || !textures.Any())
			{
				return null;
			}

			GameObject componentGameObject = SetTexture(_imageTexture, textures[0]);
			return new List<GameObject>() { componentGameObject };
		}

		private GameObject SetText(TextMeshProUGUI textMesh, string text)
		{
			if(textMesh == null)
			{
				return null;
			}

			GameObject componentGameObject = textMesh.gameObject;
			if(string.IsNullOrEmpty(text))
			{
				componentGameObject.SetActive(false);
				return null;
			}

			textMesh.enableAutoSizing = true;
			textMesh.text = text;
			textMesh.ForceMeshUpdate();
			textMesh.enableAutoSizing = false;
			componentGameObject.SetActive(true);
			return componentGameObject;
		}

		private GameObject SetTexture(Image image, Texture2D texture)
		{
			if(image == null)
			{
				return null;
			}

			GameObject componentGameObject = image.gameObject;
			if(texture == null)
			{
				componentGameObject.SetActive(false);
				return null;
			}

			image.sprite = texture.Convert();
			componentGameObject.SetActive(true);
			return componentGameObject;
		}
	}
}

#endif