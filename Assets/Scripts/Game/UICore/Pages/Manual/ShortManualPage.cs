using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class ShortManualPage : BasePage
    {
        [SerializeField] private Button _openLongManualButton;
        [SerializeField] private GameObject _mcaddonContent;
        [SerializeField] private GameObject _mcworldContent;
        [SerializeField] private GameObject _cskinsContent;
        [SerializeField] private ScrollRect _scrollRect;

        private ManualType _currentType;

        protected override void Awake()
        {
            base.Awake();

            _openLongManualButton.onClick.AddListener(ShowLongInstruction);
        }

        public void SetupPage(ManualType manualType)
        {
            _currentType = manualType;
            switch (manualType)
            {
                case ManualType.Mods:
                    _scrollRect.content = _mcaddonContent.GetComponent<RectTransform>();
                    _mcaddonContent.gameObject.SetActive(true);
                    break;
                case ManualType.World:
                    _scrollRect.content = _mcworldContent.GetComponent<RectTransform>();
                    _mcworldContent.gameObject.SetActive(true);
                    break;
                case ManualType.Skins:
                    _scrollRect.content = _cskinsContent.GetComponent<RectTransform>();
                    _cskinsContent.gameObject.SetActive(true);
                    break;
            }
        }

        private void ShowLongInstruction()
        {
            _stackPages.PageShow<LongManualPage>(PageType.LongManual).SetupPage(_currentType);
        }
    }
}