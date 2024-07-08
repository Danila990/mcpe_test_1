using Assets.Scripts.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Localization
{
	public class LanguageScrollImage : MonoBehaviour
	{
		[SerializeField] private Image _image;
		[SerializeField] private LocalizedSprite _sprites;

		public async Task SetLocale(Locale locale)
		{
			await LocalizationSettings.InitializationOperation.Task;
			AsyncOperationHandle<Sprite> spriteGetter = _sprites.GetLocalizedAssetAsync(locale);
			await spriteGetter.Task;
			_image.sprite = spriteGetter.Result.Clone();
        }
    }
}
