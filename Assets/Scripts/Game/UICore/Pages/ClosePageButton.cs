
namespace Code
{
    public class ClosePageButton : PageBaseButton
    {
        protected override void OnClick()
        {
            base.OnClick();

            _stackPages.PageHide();
        }
    }
}