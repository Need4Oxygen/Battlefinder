using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Book
{

    public class BookScript : MonoBehaviour
    {
        [SerializeField] private List<Page> pageList = new List<Page>();

        [Header("Page Turn Settings")]
        [SerializeField] private float turningRate = 200;
        public float gradFadeInTime = 0.3f;
        public float gradFadeOutTime = 2.5f;

        [Space(15)]
        [SerializeField] private Transform rightPosition = null;
        [SerializeField] private Transform leftPosition = null;
        [SerializeField] private Transform[] previewPapers = null;

        [SerializeField] private TabGroup tabGroup = null;

        private Page rightPage;
        private Page leftPage;
        private int currentPage;
        private bool moving;

        void Start()
        {
            foreach (var item in previewPapers)
                item.gameObject.SetActive(false);

            foreach (Page page in pageList)
            {
                page.bookScript = this;
                page.gameObject.SetActive(false);
            }

            leftPage = pageList[0];
            rightPage = pageList[1];

            SetPages();

            currentPage = 1; // Right page is always the current.
        }

        /// <summary> Called by Right arrows in book to flip right page to the left. </summary>
        public void FlipNext()
        {
            if (moving)
            {
                Logger.Log("BookScript", "Tried to flip page while moving");
                return;
            }

            if (currentPage + 1 < pageList.Count)
            {
                if (pageList[currentPage + 1].hasTab) // If next page has tab, select it
                    tabGroup.selectedTab = tabGroup.GetPageTab(currentPage + 1);
                else
                    tabGroup.selectedTab = null;

                tabGroup.HighlightSelectedTab();

                if (pageList[currentPage].hasTab) // If the current page has tab, set for animation
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
            {
                Logger.Log("BookScript", "Reached right limit of pageList");
            }
        }

        /// <summary> Called by Left arrows in book to flip left page to the right. </summary>
        public void FlipPrevious()
        {
            if (moving)
            {
                Logger.Log("BookScript", "Tried to flip page while moving");
                return;
            }

            if (currentPage - 1 > 0)
            {
                if (pageList[currentPage - 1].hasTab)
                {
                    tabGroup.selectedTab = tabGroup.GetPageTab(currentPage - 1);
                    tabGroup.GetPageTab(currentPage - 1).HideTab();
                    pageList[currentPage - 1].ShowTab();
                    StartCoroutine(RotateLeftPage(true));
                }
                else
                {
                    tabGroup.selectedTab = null;
                    StartCoroutine(RotateLeftPage(false));
                }

                tabGroup.HighlightSelectedTab();
            }
            else
            {
                Logger.Log("BookScript", "Reached left limit of pageList");
            }
        }

        public void FlipTo(int pg)
        {
            // Called by tab
            if (!moving && currentPage != pg)
            {
                if (pg < currentPage)
                {
                    // Debug.Log("animate to the right");
                }
                else
                {
                    // Debug.Log("animate to the left");
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

            leftPage.HideGradient();
            Page rotatingPage = rightPage;
            currentPage += 1;
            rightPage = pageList[currentPage];
            SetPages();
            bool once = false;
            while (Quaternion.Angle(rotatingPage.transform.rotation, leftPosition.rotation) > 0.01f)
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

            rightPage.HideGradient();
            Page rotatingPage = leftPage;
            currentPage -= 1;
            leftPage = pageList[currentPage - 1]; //right is always the currentpage, so left is -1
            SetPages();
            bool once = false;
            while (Quaternion.Angle(rotatingPage.transform.rotation, rightPosition.rotation) > 0.01f)
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

}