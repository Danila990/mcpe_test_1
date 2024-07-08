using UnityEngine;

namespace Code
{
    public class ModsPage : BasePage
    {
        [SerializeField] private OpenSelectedModPageButton _openModPageButtonPrefab;
        [SerializeField] private Transform _parent;

        protected override void Awake()
        {
            base.Awake();

            CreateModButtons();
        }

        private void CreateModButtons()
        {
            foreach (var modData in Resources.Load<ModsContainer>("ModsData/ModsContainer").GetModDatas())
            {
                OpenSelectedModPageButton spawnButton = Instantiate(_openModPageButtonPrefab, _parent);
                spawnButton.Setup(modData);
            }
        }
    }
}