using TMPro;
using UnityEngine;

namespace Code
{
    public class VersionsPage : BasePage
    {
        [SerializeField] private TMP_Text _versionPrefab;
        [SerializeField] private Transform _parent;

        protected override void Awake()
        {
            base.Awake();

            CreateVersions();
        }

        private void CreateVersions()
        {
            string versionsText = _stackPages.GetPage<SelectedModPage>(PageType.SelectedMod).ModData.Version;
            string version = versionsText.Replace(" ", "");
            string[] splitVesions = version.Split(',');
            foreach (var resultVersion in splitVesions)
            {
                TMP_Text spawnText = Instantiate(_versionPrefab, _parent);
                spawnText.text = resultVersion;
            }
        }
    }
}