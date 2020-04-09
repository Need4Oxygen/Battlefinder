using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public Transform tabsRight = null;
    public Transform tabsLeft = null;

    public TabButton selectedTab;

    [SerializeField]
    private BookScript bookScript = null;
    [SerializeField]
    private List<TabButton> tabList = new List<TabButton>();
    [SerializeField]
    private Color tabIdle = default;
    [SerializeField]
    private Color tabHover = default;
    [SerializeField]
    private Color tabActive = default;


    public void Subscribe(TabButton button)
    {
        tabList.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.color = tabHover;
        }
    }
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = tabActive;
        bookScript.FlipTo(button.targetPage);
    }

    public void ResetTabs()
    {
        foreach (TabButton b in tabList)
        {
            if (selectedTab != null && b == selectedTab)
            {
                b.background.color = tabActive;
                continue;
            }
            b.background.color = tabIdle;
        }
    }

    public TabButton GetPageTab(int pg)
    {
        foreach (TabButton b in tabList)
        {
            if (b.targetPage == pg)
            {
                return b;
            }
        }
        return null;
    }

    public void SwitchPreviousTabs(int destinationPage, int currentPage)
    {
        if (destinationPage > currentPage) //if we are going forward in the book
        {
            foreach (TabButton b in tabList)
            {
                if (b.targetPage < destinationPage && b.targetPage >= currentPage) //switch all tabs between current page and destination
                {
                    b.SwitchTab();
                }
            }
        }
        else //if we are going backwards in the book
        {
            foreach (TabButton b in tabList)
            {
                if (b.targetPage >= destinationPage && b.targetPage < currentPage) //switch all tabs between current page and destination, including destination
                {
                    b.SwitchTab();
                }
            }
        }
    }
}
