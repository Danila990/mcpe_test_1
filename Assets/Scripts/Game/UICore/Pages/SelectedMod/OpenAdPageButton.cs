
namespace Code
{
    public class OpenAdPageButton : PageBaseButton
    {
        protected override void OnClick()
        {
            base.OnClick();

            bool IsShowRewardAdAfterOpenMod = _stackPages.GetPage<SelectedModPage>(PageType.SelectedMod).ModData.IsShowRewardAdAfterOpenMod;
            if (IsShowRewardAdAfterOpenMod)
            {
                _stackPages.PageShow(PageType.ShowAD);
            }
            else
            {
                _stackPages.PageShow(PageType.LoadMod);
            }
        }
    }
}