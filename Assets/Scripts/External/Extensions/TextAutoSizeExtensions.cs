using TMPro;
using UnityEngine;

namespace Scripts.UI
{
	public static class TextAutoSizeExtensions
	{
		public static void ReboundRectTransformHeightFitText(this TextMeshProUGUI toRebound)
		{
			ReboundRectTransformHeightFitText(toRebound, toRebound.text);
		}

		public static void ReboundRectTransformHeightFitText(this TextMeshProUGUI toRebound, string text)
		{
			if(toRebound.enableAutoSizing)
			{
				return;
			}

			Vector2 textSize = toRebound.GetPreferredValues(text, toRebound.rectTransform.rect.width, 0);
			toRebound.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textSize.y);
		}
	}
}
