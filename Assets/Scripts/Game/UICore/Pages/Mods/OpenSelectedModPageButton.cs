using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class OpenSelectedModPageButton : PageBaseButton
    {
        [SerializeField] private TMP_Text _nameMod;
        [SerializeField] private Image _iconMod;

        private  ModData _modData;

        private void OnEnable()
        {
            if(_modData != null)
            {
                UpdateData();
            }
        }

        public void Setup(ModData modData)
        {
            _modData = modData;
            UpdateData();
        }

        private void UpdateData()
        {
            _nameMod.text = _modData.GetModName();
            _iconMod.sprite = _modData.Icon;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _stackPages.PageShow<SelectedModPage>(PageType.SelectedMod).Setup(_modData);
        }
    }
}