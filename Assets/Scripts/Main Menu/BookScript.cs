using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    [SerializeField]
    private List<Page> pageList = new List<Page>();

    [Header("Page Turn Settings")]
    [SerializeField]
    private float turningRate = 200;
    [SerializeField]
    private float gradFadeInTime = 0.3f;
    [SerializeField]
    private float gradFadeOutTime = 2.5f;
    [Space(20)]

    [SerializeField]
    private Transform rightPosition = default;
    [SerializeField]
    private Transform leftPosition = default;

    [SerializeField]
    private TabGroup tabGroup = null;

    private Page rightPage;
    private Page leftPage;
    private int currentPage;
    private bool moving;

    private void Start()
    {
        rightPosition.GetChild(0).gameObject.SetActive(false);
        leftPosition.GetChild(0).gameObject.SetActive(false);

        foreach (Page p in pageList)
        {
            p.gradFadeIn = gradFadeInTime;
            p.gradFadeOut = gradFadeOutTime;
            p.gameObject.SetActive(false);
        }

        leftPage = pageList[0];
        rightPage = pageList[1];

        SetPages();


        currentPage = 1; //right page is always the current.
        
    }

    public void FlipNext()
    {
        //called by RIGHT arrow
        if (!moving && pageList.Count > currentPage + 1)
        {
            if (pageList[currentPage + 1].hasTab) //if next page has tab, select it
            {
                tabGroup.selectedTab = tabGroup.GetPageTab(currentPage + 1);
            }
            else
            {
                tabGroup.selectedTab = null;
            }
            tabGroup.ResetTabs();

            if (pageList[currentPage].hasTab) //if the current page has tab, set for animation
            {
                tabGroup.GetPageTab(currentPage).HideTab();
                pageList[currentPage].ShowTab();
                StartCoroutine(RotateRightPage(true));
            }
            else
            {
                StartCoroutine(RotateRightPage(false));
            }
        }
        else
            Debug.Log("END OF PAGE LIST");
    }

    public void FlipPrevious()
    {
        //called by LEFT arrow
        if (!moving && currentPage - 1 > 0)
        {


            if (pageList[currentPage - 1].hasTab)
            {
                tabGroup.selectedTab = tabGroup.GetPageTab(currentPage - 1);
                tabGroup.ResetTabs();
                tabGroup.GetPageTab(currentPage - 1).HideTab();
                pageList[currentPage - 1].ShowTab();
                StartCoroutine(RotateLeftPage(true));
            }
            else
            {
                tabGroup.selectedTab = null;
                tabGroup.ResetTabs();
                StartCoroutine(RotateLeftPage(false));
            }
        }
        else
        {
            Debug.Log("END OF PAGE LIST");
        }
    }

    public void FlipTo(int pg)
    {
        //called by tab
        if (!moving && currentPage != pg)
        {
            if (pg < currentPage)
            {
                Debug.Log("animate to the right");
            }
            else
            {
                Debug.Log("animate to the left");
            }
            tabGroup.SwitchPreviousTabs(pg, currentPage);
            currentPage = pg;
            rightPage.gameObject.SetActive(false);
            leftPage.gameObject.SetActive(false);
            rightPage = pageList[currentPage];
            leftPage = pageList[currentPage - 1];
            SetPages();
        }
    }

    private void SetPages()
    {
        rightPage.transform.rotation = rightPosition.rotation;
        rightPage.transform.position = rightPosition.position;
        rightPage.gameObject.SetActive(true);

        leftPage.transform.rotation = leftPosition.rotation;
        leftPage.transform.position = leftPosition.position;
        leftPage.gameObject.SetActive(true);
    }

    private IEnumerator RotateRightPage(bool tabbed)
    {
        moving = true;
        leftPage.FadeGrad(true);
        Page rotatingPage = rightPage;
        currentPage += 1;
        rightPage = pageList[currentPage];
        SetPages();
        bool once = false;
        while (rotatingPage.transform.rotation.eulerAngles.x != leftPosition.rotation.eulerAngles.x)
        {
            rotatingPage.transform.rotation = Quaternion.RotateTowards(rotatingPage.transform.rotation, leftPosition.rotation, turningRate * Time.deltaTime);

            if (Quaternion.Angle(rotatingPage.transform.rotation, leftPosition.rotation) < 10f && !once)
            {
                leftPage.gameObject.SetActive(false);
                leftPage = rotatingPage;
                once = true;
            }
            yield return null;
        }
        if (tabbed)
        {
            tabGroup.GetPageTab(currentPage - 1).SwitchTab();
            pageList[currentPage - 1].HideTab();
        }
        SetPages();
        moving = false;
    }

    private IEnumerator RotateLeftPage(bool tabbed)
    {
        moving = true;
        rightPage.FadeGrad(true);
        Page rotatingPage = leftPage;
        currentPage -= 1;
        leftPage = pageList[currentPage - 1]; //right is always the currentpage, so left is -1
        SetPages();
        bool once = false;
        while (rotatingPage.transform.rotation.eulerAngles.x != rightPosition.rotation.eulerAngles.x)
        {
            rotatingPage.transform.rotation = Quaternion.RotateTowards(rotatingPage.transform.rotation, rightPosition.rotation, turningRate * Time.deltaTime);

            if (Quaternion.Angle(rotatingPage.transform.rotation, rightPosition.rotation) < 10f && !once)
            {
                rightPage.gameObject.SetActive(false);
                rightPage = rotatingPage;
                once = true;
            }
            yield return null;
        }
        if (tabbed)
        {
            tabGroup.GetPageTab(currentPage).SwitchTab();
            pageList[currentPage].HideTab();
        }
        SetPages();
        moving = false;
    }
}
