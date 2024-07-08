using UnityEngine;

namespace Code
{
    public class OpenManualPageButton : PageBaseButton
    {
        [SerializeField] private ManualType _manualType;

        protected override void OnClick()
        {
            base.OnClick();

           _stackPages.PageShow<ShortManualPage>(PageType.ShortManual).SetupPage(_manualType);
        }
    }
}