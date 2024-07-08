using Scripts.AD;
using System.Threading.Tasks;
using UnityEngine;

namespace Code
{
    public class StartLoadPage : BasePage
    {
        [Header("LoadPage")]
        [SerializeField] private float _loadDuration = 1f;
        [SerializeField] private PageType _openPageType = PageType.Main;

        protected override void Awake()
        {
            base.Awake();

            AwaitLoad();
        }

        private async void AwaitLoad()
        {
            await Task.Delay((int)(_loadDuration * 1000));
            _stackPages.PageShow(_openPageType);
            Hide();
        }
    }
}