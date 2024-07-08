using UnityEngine;

namespace Scripts
{
	public static class RectTransformExtensions
	{
		public static Vector2 GetAnchoredPositionWithPivot(this RectTransform transform)
		{
			Vector2 sizeDelta = transform.sizeDelta;
			Vector2 pivot = transform.pivot;
			Vector2 anchoredPosition = transform.anchoredPosition;
			Vector2 anchoredPositionWithPivot = sizeDelta * pivot - anchoredPosition;
			return anchoredPositionWithPivot;
		}

		public static void SetPivotWithSamePosition(this RectTransform transform, Vector2 newPivot)
		{
			Vector2 anchoredPositionWithPivot = transform.GetAnchoredPositionWithPivot();
			Vector2 anchoredPositionWithNewPivot = transform.sizeDelta * newPivot - anchoredPositionWithPivot;
			transform.pivot = newPivot;
			transform.anchoredPosition = anchoredPositionWithNewPivot;
		}

		public static void CopySizeAndPosition(this RectTransform to, RectTransform from)
		{
			to.anchorMin = from.anchorMin;
			to.anchorMax = from.anchorMax;
			to.anchoredPosition = from.anchoredPosition;
			to.pivot = from.pivot;
			to.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, from.rect.width);
			to.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, from.rect.height);
		}

		public static Vector2 GetSizeWithCurrentAnchors(this RectTransform transform)
		{
			var rect = transform.rect;
			return new Vector2(rect.xMax - rect.xMin, rect.yMax - rect.yMin);
		}

		public static RectTransform GetChildRect(this RectTransform transform, int index)
		{
			return (RectTransform)transform.GetChild(index);
		}

		public static RectTransform[] GetChildsRect(this RectTransform transform)
		{
			int childCount = transform.childCount;
			var childs = new RectTransform[childCount];
			for(int i = 0; i < childCount; i++)
			{
				childs[i] = transform.GetChildRect(i);
			}

			return childs;
		}

		public static Vector3[] GetWorldCorners(this RectTransform transform)
		{
			Vector3[] worldCorners = new Vector3[4];
			transform.GetWorldCorners(worldCorners);
			return worldCorners;
		}
	}
}