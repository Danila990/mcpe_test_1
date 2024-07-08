using Assets.Scripts.UI.Scrolls;
using Scripts.UI.FixedScroll;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class ArrowPage : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _leftArrow;
        [SerializeField] private CanvasGroup _rightArrow;
        [SerializeField, Range(0, 1)] private float _alpha = 0.5f;

        private FixedScrollRect _scrollRect;
        private ScrollRectWithEvents _scrollRectWithEvents;

        private void Start()
        {
            _scrollRect = GetComponent<FixedScrollRect>();
            _scrollRectWithEvents = GetComponent<ScrollRectWithEvents>();
            UpdateArrow(_scrollRect.NowChild);
            _scrollRect.OnNowChildChanged += UpdateArrow;
            _leftArrow.GetComponent<Button>().onClick.AddListener(_scrollRect.SetPreviousChildWithLerp);
            _rightArrow.GetComponent<Button>().onClick.AddListener(_scrollRect.SetNextChildWithLerp);
        }

        private void UpdateArrow(int index)
        {
            if (index == 0)
            {
                _leftArrow.alpha = _alpha;
                _rightArrow.alpha = 1.0f;
                _leftArrow.interactable = false;
            }
            else if (index == _scrollRectWithEvents.content.childCount - 1)
            {
                _leftArrow.alpha = 1.0f;
                _rightArrow.alpha = _alpha;
                _rightArrow.interactable = false;
            }
            else
            {
                _leftArrow.alpha = 1.0f;
                _rightArrow.alpha = 1.0f;
                _leftArrow.interactable = true;
                _rightArrow.interactable = true;
            }
        }
    }
}