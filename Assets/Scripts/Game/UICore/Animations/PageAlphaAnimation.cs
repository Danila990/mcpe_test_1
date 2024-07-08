using DG.Tweening;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PageAlphaAnimation : BasePageAnimation
    {
        [SerializeField, Range(0, 1)] private float _startAlpha = 0f;
        [SerializeField] private float _closeDuration = 0.2f;
        [SerializeField] private float _openDuration = 1f;

        private CanvasGroup _canvasGroup;

        protected override void Awake()
        {
            base.Awake();

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Show()
        {
            base.Show();

            _canvasGroup.alpha = _startAlpha;
            _nowAnimation = StartAnimation(1, _openDuration);
            _nowAnimation.onKill += InvokeOnShowAnimationEnd;
        }

        public override void MoveOut()
        {
            base.MoveOut();

            _nowAnimation = StartAnimation(0, _closeDuration);
            _nowAnimation.onKill += () => gameObject.SetActive(false);
            _nowAnimation.onKill += InvokeOnMoveOutAnimationEnd;
        }

        public override void MoveIn()
        {
            base.MoveIn();

            _canvasGroup.alpha = _startAlpha;
            _nowAnimation = StartAnimation(1, _openDuration);
            _nowAnimation.onKill += InvokeOnMoveInAnimationEnd;
        }

        public override void Hide()
        {
            base.Hide();

            _nowAnimation = StartAnimation(0, _closeDuration);
            _nowAnimation.onKill += InvokeOnHideAnimationEnd;

        }

        private Sequence StartAnimation(float alpha, float duration)
        {
            Tween tween = _canvasGroup.DOFade(alpha, duration).
                SetEase(Ease.Linear);

            Sequence result = DOTween.Sequence();
            result.Append(tween);

            return result;
        }
    }
}