
namespace Code
{
    public abstract class PageBaseButton : BaseButton
    {
        protected StackPages _stackPages;

        protected override void Start()
        {
            base.Start();

            _stackPages = StackPages.Instance;
        }
    }
}