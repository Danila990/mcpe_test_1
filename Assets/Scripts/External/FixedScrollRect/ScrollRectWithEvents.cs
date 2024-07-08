using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Scrolls
{
	public class ScrollRectWithEvents : ScrollRect
	{
		public event Action<PointerEventData> OnBeginDragEvent;
		public event Action<PointerEventData> OnDragEvent;
		public event Action<PointerEventData> OnEndDragEvent;

		public override void OnBeginDrag(PointerEventData eventData)
		{
			base.OnBeginDrag(eventData);
			OnBeginDragEvent?.Invoke(eventData);
		}

		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);
			OnDragEvent?.Invoke(eventData);
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			base.OnEndDrag(eventData);
			OnEndDragEvent?.Invoke(eventData);
		}
	}
}
