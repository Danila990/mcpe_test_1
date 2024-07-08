using DG.Tweening;
using System;
using UnityEngine;

namespace Code
{
    public abstract class BasePageAnimation : MonoBehaviour
    {
        protected Sequence _nowAnimation;

        public event Action OnShowAnimationStart;
        public event Action OnShowAnimationEnd;
        public event Action OnHideAnimationStart;
        public event Action OnHideAnimationEnd;
        public event Action OnMoveOutAnimationStart;
        public event Action OnMoveOutAnimationEnd;
        public event Action OnMoveInAnimationStart;
        public event Action OnMoveInAnimationEnd;

        public bool IsNowPlayingAnyAnimation => _nowAnimation.IsPlaying();

        protected virtual void Awake()
        {
            _nowAnimation = DOTween.Sequence();
            _nowAnimation.Kill(true);
        }

        protected virtual void OnDestroy()
        {
            _nowAnimation?.Kill(true);
        }

        public virtual void Show()
        {
            _nowAnimation.Kill();
            gameObject.SetActive(true);
            OnShowAnimationStart?.Invoke();
        }

        public virtual void Hide()
        {
            _nowAnimation.Kill();
            OnHideAnimationStart?.Invoke();
        }
        public virtual void MoveOut()
        {
            _nowAnimation.Kill();
            OnMoveOutAnimationStart?.Invoke();
        }

        public virtual void MoveIn()
        {
            _nowAnimation.Kill();
            gameObject.SetActive(true);
            OnMoveInAnimationStart?.Invoke();
        }

        protected void InvokeOnMoveOutAnimationEnd()
        {
            OnMoveOutAnimationEnd?.Invoke();
        }

        protected void InvokeOnMoveInAnimationEnd()
        {
            OnMoveInAnimationEnd?.Invoke();
        }

        protected void InvokeOnShowAnimationEnd()
        {
            OnShowAnimationEnd?.Invoke();
        }

        protected void InvokeOnHideAnimationEnd()
        {
            OnHideAnimationEnd?.Invoke();
        }
    }
}