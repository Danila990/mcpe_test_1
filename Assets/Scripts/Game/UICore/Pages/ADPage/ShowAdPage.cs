
namespace Code
{
    public class ShowAdPage : BasePage
    {

        protected override void Awake()
        {
            base.Awake();

            _pageAnimation.OnMoveOutAnimationEnd += () =>
            {
                _stackPages.RemovePage(PageType.ShowAD);
                Dispose();
            };
        }
    }
}