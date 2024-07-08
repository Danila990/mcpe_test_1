using Scripts.AD;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BasePage : MonoBehaviour
    {
        [SerializeField] private bool _showAdAfterHide = false;
        [SerializeField] private bool _escapeInputPage = true;

        [field: SerializeField] public PageType PageType { get; private set; }

        protected CanvasGroup _canvasGroup;
        protected BasePageAnimation _pageAnimation;
        protected StackPages _stackPages;

        protected virtual void Awake()
        {
            _stackPages = StackPages.Instance;
            PageAnimation();
            PageInteraction();
        }

        public virtual void Show()
        {
            _pageAnimation.Show();
        }

        public virtual void Hide()
        {
            if (_showAdAfterHide)
            {
                AdsController.Instance.ShowInterAd();
            }

            _pageAnimation.Hide();
        }

        public void MoveIn()
        {
            _pageAnimation.MoveIn();
        }

        public void MoveOut()
        {
            _pageAnimation.MoveOut();
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }

        public void EnableInteraction(bool isEnable)
        {
            _canvasGroup.blocksRaycasts = isEnable;
        }

        private void PageInteraction()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _pageAnimation.OnShowAnimationStart += () => EnableInteraction(true);
            _pageAnimation.OnHideAnimationStart += () => EnableInteraction(false);
            _pageAnimation.OnHideAnimationEnd += Dispose;
        }

        private void PageAnimation()
        {
            if (TryGetComponent(out BasePageAnimation basePageAnimation))
            {
                _pageAnimation = basePageAnimation;
            }
            else
            {
                _pageAnimation = gameObject.AddComponent<PageHorizontalAnimation>();
            }
        }
    }
}