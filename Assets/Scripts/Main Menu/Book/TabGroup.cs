using System.Collections.Generic;
using UnityEngine;

namespace Book
{

    public class TabGroup : MonoBehaviour
    {
        public Transform tabsRight = null;
        public Transform tabsLeft = null;

        [SerializeField] private TabButton _selectedTab;
        public TabButton selectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                HighlightSelectedTab();
            }
        }

        [SerializeField] private BookScript bookScript = null;
        [SerializeField] private Color tabIdle = default;
        [SerializeField] private Color tabHover = default;
        [SerializeField] private Color tabActive = default;

        private List<TabButton> tabList = new List<TabButton>();

        private void Start()
        {
            HighlightSelectedTab();
        }

        public void Subscribe(TabButton button)
        {
            tabList.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            if (selectedTab == null || button != selectedTab)
                button.background.color = tabHover;
        }

        public void OnTabExit(TabButton button)
        {
            if (selectedTab == null || button != selectedTab)
                button.background.color = tabIdle;
        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;
            button.background.color = tabActive;
            bookScript.FlipTo(button.targetPage);
        }

        public void HighlightSelectedTab()
        {
            foreach (TabButton tabButton in tabList)
                tabButton.background.color = tabIdle;

            if (selectedTab != null)
                selectedTab.background.color = tabActive;
        }

        public TabButton GetPageTab(int page)
        {
            foreach (TabButton tabButton in tabList)
                if (tabButton.targetPage == page)
                    return tabButton;

            return null;
        }

        public void SwitchPreviousTabs(int destinationPage, int currentPage)
        {
            if (destinationPage > currentPage) // If we are going forward in the book
            {
                // Switch all tabs between current page and destination, including destination
                foreach (TabButton tabButton in tabList)
                    if (tabButton.targetPage < destinationPage && tabButton.targetPage >= currentPage)
                        tabButton.SwitchTab();
            }
            else // If we are going backwards in the book
            {
                foreach (TabButton tabButton in tabList)
                    if (tabButton.targetPage >= destinationPage && tabButton.targetPage < currentPage)
                        tabButton.SwitchTab();
            }
        }
    }

}