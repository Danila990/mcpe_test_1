using System.Linq;
using UnityEngine;

namespace Code
{
    public class PagesCreator
    {
        private readonly BasePage[] _pagesPrefabs;
        private readonly Transform _parent;

        public PagesCreator(Transform parent, PageType startPage)
        {
            _parent = parent;
            _pagesPrefabs = Resources.LoadAll<BasePage>("Prefabs/UI/Pages");
            CreatePage(startPage);
        }

        public BasePage CreatePage(PageType pageType)
        {
            BasePage findPage = _pagesPrefabs.FirstOrDefault(page => page.PageType == pageType);
            if (findPage == null)
            {
                Debug.LogError("Find PageError: " + pageType);
                return null;
            }

            return Object.Instantiate(findPage, _parent);
        }
    }
}