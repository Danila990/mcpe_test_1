using Scripts.UI.FixedScroll;
using TMPro;
using UnityEngine;


namespace Code
{
    public class SelectedModPage : BasePage
    {
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _nameMod;
        [SerializeField] private FixedScrollRect _scrollRect;
        [SerializeField] private ScreenshotMod _screenshotsPrefab;
        [SerializeField] private OpenPageButton _openVersionPageButton;

        public ModData ModData { get; private set; }

        public void Setup(ModData modData)
        {
            ModData = modData;
            try
            {
                _nameMod.text = modData.GetModName();
            }
            catch 
            {
                Debug.LogError("Null Name Localization");
                _nameMod.text = "Mod name";
            }

            try
            {
                _descriptionText.text = modData.GetDescription();
            }
            catch
            {
                _descriptionText.text = "Mod description";
                Debug.LogError("Null description Localization");
            }

            if (modData.Version.Length == 0)
            {
                _openVersionPageButton.gameObject.SetActive(false);
            }
            else
            {
                _openVersionPageButton.gameObject.SetActive(true);
            }

            CreateScreenshots(modData.Screenshots);
        }

        private void CreateScreenshots(Sprite[] screenshots)
        {
            foreach (var sprite in screenshots)
            {
                ScreenshotMod screenshot = Instantiate(_screenshotsPrefab);
                screenshot.transform.SetParent(_scrollRect.ScrollRect.content, false);
                screenshot.Icon.sprite = sprite;
            }
        }
    }
}