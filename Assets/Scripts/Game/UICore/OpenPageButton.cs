using UnityEngine;

namespace Code
{
    public class OpenPageButton : PageBaseButton
    {
        [SerializeField] protected PageType _pageType;

        protected override void OnClick()
        {
            base.OnClick();

            _stackPages.PageShow(_pageType);
        }
    }
}