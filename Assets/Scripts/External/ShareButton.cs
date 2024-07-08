using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.UIPages.PagesCommonButtons
{
	[RequireComponent(typeof(Button))]
	public class ShareButton : MonoBehaviour
	{
		private void Awake()
		{
			var button = GetComponent<Button>();
			button.onClick.AddListener(ShowBoxShareWith);
		}

		private void ShowBoxShareWith()
		{
			var nativeShare = new NativeShare();
			nativeShare.SetText("https://play.google.com/store/apps/details?id=" + Application.identifier);
			nativeShare.Share();
		}
	}
}
