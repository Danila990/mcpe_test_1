using Scripts.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class TextMeshProUGUIWithRebound : TextMeshProUGUI
	{
		private bool _needRecalculate = false;

		public override string text
		{
			set
			{
				base.text = value;
				SetDirty();
			}
		}

		private void LateUpdate()
		{
			if(_needRecalculate)
			{
				this.ReboundRectTransformHeightFitText();
				_needRecalculate = false;
                RectTransform rectTransform = GetComponent<RectTransform>();
                Vector3 pos = new Vector3(rectTransform.position.x, -(rectTransform.sizeDelta.y / 2), rectTransform.position.z);
                rectTransform.position = pos;
            }
		}

		public void SetDirty()
		{
			_needRecalculate = true;
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			SetDirty();
		}
	}
}
