using DG.Tweening;
using Scripts;
using UnityEngine;

namespace Code
{
    public class PageHorizontalAnimation : BasePageAnimation
    {
        [SerializeField] private float _moveDuration = 0.3f;

        private float _rightPoint;
        private float _leftPoint;
        private float _middlePoint;

        protected override void Awake()
        {
            base.Awake();

            Vector2 canvasSize = GetCanvasSize();
            _middlePoint = 0;
            _rightPoint = canvasSize.x / 150;
            _leftPoint = -_rightPoint;
        }

        public override void Show()
        {
            base.Show();

            transform.position = new Vector2(_rightPoint, transform.position.y);
            _nowAnimation = StartAnimation(_middlePoint);
            _nowAnimation.onKill += InvokeOnShowAnimationEnd;
        }

        public override void MoveOut()
        {
            base.MoveOut();

            _nowAnimation = StartAnimation(_leftPoint);
            _nowAnimation.onKill += () => gameObject.SetActive(false);
            _nowAnimation.onKill += InvokeOnMoveOutAnimationEnd;
        }

        public override void MoveIn()
        {
            base.MoveIn();

            transform.position = new Vector2(_leftPoint, transform.position.y);
            _nowAnimation = StartAnimation(_middlePoint);
            _nowAnimation.onKill += InvokeOnMoveInAnimationEnd;
        }

        public override void Hide()
        {
            base.Hide();

            _nowAnimation = StartAnimation(_rightPoint);
            _nowAnimation.onKill += InvokeOnHideAnimationEnd;

        }

        private Sequence StartAnimation(float movePoint)
        {
            Tween tween = transform.DOMoveX(movePoint, _moveDuration).
                SetEase(Ease.Linear);

            Sequence result = DOTween.Sequence();
            result.Append(tween);

            return result;
        }

        private Vector2 GetCanvasSize()
        {
            return ((RectTransform)transform.parent).GetSizeWithCurrentAnchors();
        }
    }
}