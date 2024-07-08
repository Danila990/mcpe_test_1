using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class LongManualPage : BasePage
    {
        [SerializeField] private GameObject _mcaddonContent;
        [SerializeField] private GameObject _mcworldContent;
        [SerializeField] private GameObject _cskinsContent;
        [SerializeField] private ScrollRect _scrollRect;

        public void SetupPage(ManualType manualType)
        {
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
    }
}