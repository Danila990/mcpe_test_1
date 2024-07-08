using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code
{
    public class StackPages : Singleton<StackPages>
    {
        private LinkedList<BasePage> _pageStack = new LinkedList<BasePage>();
        private PagesCreator _pagesCreator;

        protected override void Awake()
        {
            base.Awake();

            _pagesCreator = new PagesCreator(transform, PageType.StartLoad);
        }

        public BasePage GetCurrentPage()
        {
            if (_pageStack.Count != 0)
            {
                return _pageStack.LastOrDefault();
            }

            Debug.LogError("Current Page Error");
            return null;
        }

        public T GetCurrentPage<T>() where T : BasePage
        {
            return GetCurrentPage() as T;
        }

        public BasePage GetPage(PageType pageType)
        {
            return _pageStack.FirstOrDefault(page => page.PageType == pageType);
        }

        public T GetPage<T>(PageType pageType) where T : BasePage
        {
            return GetPage(pageType) as T;
        }

        public void RemovePage(PageType pageType)
        {
            BasePage findPage = _pageStack.FirstOrDefault(page => page.PageType == pageType);
            if (findPage != null)
            {
                _pageStack.Remove(findPage);
            }
        }

        public BasePage PageShow(PageType pageType)
        {
            BasePage previousPage = _pageStack.LastOrDefault();
            previousPage?.MoveOut();
            BasePage newPage = _pagesCreator.CreatePage(pageType);
            _pageStack.AddLast(newPage);
            newPage.Show();
            return newPage;
        }

        public T PageShow<T>(PageType pageType) where T : BasePage
        {
            return PageShow(pageType) as T;
        }

        public void PageHide()
        {
            if (GetCurrentPage()?.PageType == PageType.Main || _pageStack.Count <= 1)
            {
                return;
            }

            BasePage pageToHide = _pageStack.Last();
            _pageStack.RemoveLast();
            pageToHide.Hide();
            BasePage previousPage = _pageStack.LastOrDefault();
            previousPage?.MoveIn();
        }
    }
}