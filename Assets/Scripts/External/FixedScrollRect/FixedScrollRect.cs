using Assets.Scripts.UI.Scrolls;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.ScrollRect;

namespace Scripts.UI.FixedScroll
{
	// Content pivot должен быть с x или y равным 0.5 для vertical и horizontal соответственно
	// Content child pivot должен быть равным (0.5, 0.5)
	[RequireComponent(typeof(ScrollRectWithEvents))]
	public class FixedScrollRect : MonoBehaviour
	{
		[SerializeField] private float _decelerationRate = 10f;
		[SerializeField] private float _minLerpedMagnitude = 0.25f;
		[Range(0, 1)]
		[SerializeField] private float _newChildDistanceInPercents = 0.25f;

        private Vector2? _lerpTo;
		private int _nowChild = 0;

		public event Action<int> OnNowChildChanged;

		public ScrollRectWithEvents ScrollRect
		{
			get;
			private set;
		}

		public int NowChild
		{
			get
			{
				return _nowChild;
			}

			private set
			{
				if(_nowChild == value || 
					value < 0 || 
					(value >= ScrollRect.content.childCount && value != 0))
				{
					return;
				}

                _nowChild = value;
				OnNowChildChanged?.Invoke(value);
			}
		}

		private void Awake()
		{
			ScrollRect = GetComponent<ScrollRectWithEvents>();
			ScrollRect.OnBeginDragEvent += OnBeginDrag;
			ScrollRect.OnDragEvent += UpdateNowChild;
			ScrollRect.OnEndDragEvent += OnEndDrag;
		}

        private void Start()
        {
			UpdateLerpToPosition();
        }

        private void LateUpdate()
		{
            if (_lerpTo == null || ScrollRect.content.childCount == 0)
			{
				return;
			}

            float decelerate = Mathf.Min(_decelerationRate * Time.deltaTime, 1f);
			Vector2 newPosition =  Vector2.Lerp(ScrollRect.content.localPosition, _lerpTo.Value, decelerate);
			if(Vector2.SqrMagnitude(newPosition - _lerpTo.Value) > _minLerpedMagnitude)
			{
				ScrollRect.content.localPosition = newPosition;
			}
			else
			{
				ScrollRect.content.localPosition = _lerpTo.Value;
				_lerpTo = null;
			}
		}

        public void UpdateLerpToPosition()
        {
            if (NowChild >= ScrollRect.content.childCount)
            {
                return;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(ScrollRect.content);
            Vector2 lerpTo = GetContentPositionLerpTo(NowChild);
            if (_lerpTo != null)
            {
                _lerpTo = lerpTo;
            }
            else
            {
                ScrollRect.content.localPosition = lerpTo;
            }
        }

        public void ToBaseState()
		{
			NowChild = 0;
			_lerpTo = null;
		}

		public void SetChildWithLerp(int index)
		{
			NowChild = index;
			_lerpTo = GetContentPositionLerpTo(NowChild);
		}

		public void SetPreviousChildWithLerp()
		{
			SetChildWithLerp(NowChild - 1);
		}

		public void SetNextChildWithLerp()
		{
			SetChildWithLerp(NowChild + 1);
		}

		public void SetChild(int index)
		{
			NowChild = index;
			ScrollRect.content.localPosition = GetContentPositionLerpTo(NowChild);
		}


		public Vector2 GetContentPositionLerpTo(int childIndex)
		{
			RectTransform child = ScrollRect.content.GetChild(childIndex).GetComponent<RectTransform>();
			Vector2 lerpTo = -ScrollRect.content.InverseTransformPoint(child.position);
			return lerpTo;
		}

		private void OnBeginDrag(PointerEventData eventData)
		{
			_lerpTo = null;
		}

		private void UpdateNowChild(PointerEventData eventData)
		{
			NowChild = GetClosestIndex();
		}

		private int GetClosestIndex()
		{
			Vector2 nowChildContentPosition = GetContentPositionLerpTo(NowChild);
			Vector2 nowChildDelta = (Vector2)ScrollRect.content.localPosition - nowChildContentPosition;
			float minDistance = nowChildDelta.magnitude * _newChildDistanceInPercents;
			int leftChildIndex = NowChild - 1;
			if(leftChildIndex >= 0)
			{
				Vector2 leftChildPosition = GetContentPositionLerpTo(leftChildIndex);
				Vector2 leftChildDelta = (Vector2)ScrollRect.content.localPosition - leftChildPosition;
				if(leftChildDelta.magnitude < minDistance)
				{
					return leftChildIndex;
				}
			}

			int rightChildIndex = NowChild + 1;
			if(rightChildIndex < ScrollRect.content.childCount)
			{
				Vector2 rightChildPosition = GetContentPositionLerpTo(rightChildIndex);
				Vector2 rightChildDelta = (Vector2)ScrollRect.content.localPosition - rightChildPosition;
				if(rightChildDelta.magnitude < minDistance)
				{
					return rightChildIndex;
				}
			}

			return NowChild;
		}

		private void OnEndDrag(PointerEventData eventData)
		{
			if(ScrollRect.content.childCount == 0)
			{
				return;
			}

			int moveDirection = GetMoveDirection(eventData);
			int newIndex;
			if(moveDirection == -1)
			{
				int previousIndex = NowChild - 1;
				newIndex = previousIndex < 0 ? NowChild : previousIndex;
			}
			else if(moveDirection == 1)
			{
				int nextIndex = NowChild + 1;
				newIndex = nextIndex >= ScrollRect.content.childCount ? NowChild : nextIndex;
			}
			else
			{
				newIndex = NowChild;
			}

			SetChildWithLerp(newIndex);
		}

		private int GetMoveDirection(PointerEventData eventData)
		{
			Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
			if(ScrollRect.horizontal && dragVectorDirection.x > 0 ||
				ScrollRect.vertical && dragVectorDirection.y < 0)
			{
				return -1;
			}
			else if(ScrollRect.horizontal && dragVectorDirection.x < 0 ||
				ScrollRect.vertical && dragVectorDirection.y > 0)
			{
				return 1;
			}

			return 0;
		}
    }
}