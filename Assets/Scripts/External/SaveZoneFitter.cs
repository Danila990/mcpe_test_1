using GoogleMobileAds.Api;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SaveZoneFitter : MonoBehaviour
{
	private void Awake()
	{
		RectTransform rectTransform = GetComponent<RectTransform>();

		Rect safeArea = Screen.safeArea;
		Vector2 anchorMin = safeArea.position;
		Vector2 anchorMax = safeArea.position + safeArea.size;
		anchorMin.y += AdSize.Banner.Height * 2;
		anchorMin.x /= Screen.width;
		anchorMin.y /= Screen.height;
		anchorMax.x /= Screen.width;
		anchorMax.y /= Screen.height;
		rectTransform.anchorMin = anchorMin;
		rectTransform.anchorMax = anchorMax;
	}
}