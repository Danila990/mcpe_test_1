using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

namespace Code
{
    [CreateAssetMenu(menuName = "ModData", fileName = "ModData")]
    public class ModData : ScriptableObject
    {
        [field: SerializeField] public LocalizedString ModNameLocalized { get; private set; }
        [field: SerializeField] public LocalizedString DescriptionModLocalized { get; private set; }
        [field: SerializeField] public string ModPath { get; private set; }
        [field: SerializeField] public bool IsShowRewardAdAfterOpenMod { get; private set; } = false;
        [field: SerializeField, TextArea(1, 5)] public string Version { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Sprite[] Screenshots { get; private set; }

        public string GetModName()
        {
            return ModNameLocalized.GetLocalizedString();
        }

        public string GetDescription()
        {
            return  DescriptionModLocalized.GetLocalizedString();
        }

        public void SetIcon(Sprite icon)
        {
            Icon = icon;
        }

        public void SetScreenshots(Sprite[] screenshots)
        {
            Screenshots = screenshots;
        }

        public void SetVerison(string verison)
        {
            Version = verison;
        }

        public void ResetData()
        {
            Icon = null;
            Screenshots = null;
            Version = null;
        }
    }
}