using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    [SerializeField]
    List<Transform> pageList = new List<Transform>();

    //[SerializeField]
    //private int openEuler = 77;
    [SerializeField]
    private float turningRate = 100;

    [SerializeField]
    private Transform rightPosition = default;
    [SerializeField]
    private Transform leftPosition = default;

    private Transform rightPage;
    private Transform leftPage;
    private int currentPage;
    private bool moving;

    private enum ESides { Left, Right }

    private void Start()
    {
        rightPosition.GetChild(0).gameObject.SetActive(false);
        leftPosition.GetChild(0).gameObject.SetActive(false);
        foreach (Transform t in pageList) { t.gameObject.SetActive(false); } //hide all pages

        leftPage = pageList[0];
        SetPage(ESides.Left);

        rightPage = pageList[1];
        SetPage(ESides.Right);

        currentPage = 1; //right page is always the current.
    }

    public void FlipNext()
    {
        //called by right arrow
        if (!moving && pageList.Count > currentPage + 1)
            StartCoroutine(RotateRightPage());
        else
            Debug.Log("END OF PAGE LIST");
    }

    public void FlipPrevious()
    {
        if (!moving && currentPage - 1 > 0)
            StartCoroutine(RotateLeftPage());
        else
            Debug.Log("END OF PAGE LIST");
    }

    public void FlipTo(int pg)
    {
        //called by tab
        //if > < > < > <
    }

    private Quaternion GetTargetRot(Transform t, ESides side) //if works delet dis
    {
        switch (side)
        {
            case ESides.Right:
                return rightPosition.rotation;//Quaternion.Euler(openEuler, t.rotation.eulerAngles.y, t.rotation.eulerAngles.z);
            case ESides.Left:
                return leftPosition.rotation;//Quaternion.Euler(-openEuler, t.rotation.eulerAngles.y, t.rotation.eulerAngles.z);
            default:
                Debug.Log("No side param. failed target rotation");
                return Quaternion.identity;
        }
    }

    private void SetPage(ESides side)
    {
        switch (side)
        {
            case ESides.Right:
                rightPage.rotation = GetTargetRot(rightPage, ESides.Right);
                rightPage.position = rightPosition.position;
                rightPage.gameObject.SetActive(true);
                break;
            case ESides.Left:
                leftPage.rotation = GetTargetRot(leftPage, ESides.Left);
                leftPage.position = leftPosition.position;
                leftPage.gameObject.SetActive(true);
                break;
        }
    }

    private IEnumerator RotateRightPage()
    {
        moving = true;
        Transform rotatingPage = rightPage;
        currentPage += 1;
        rightPage = pageList[currentPage];
        SetPage(ESides.Right);
        Quaternion targetRotation = GetTargetRot(rotatingPage, ESides.Left);
        while (rotatingPage.rotation.eulerAngles.x != targetRotation.eulerAngles.x)
        {
            rotatingPage.rotation = Quaternion.RotateTowards(rotatingPage.rotation, targetRotation, turningRate * Time.deltaTime);

            if (Quaternion.Angle(rotatingPage.rotation, targetRotation) < 10f)
            {
                leftPage.gameObject.SetActive(false);
            }

            yield return null;
        }

        leftPage = rotatingPage;
        moving = false;
    }

    private IEnumerator RotateLeftPage()
    {
        moving = true;
        Transform rotatingPage = leftPage;
        currentPage -= 1;
        leftPage = pageList[currentPage - 1]; //right is always the currentpage, so left is -1
        SetPage(ESides.Left);
        Quaternion targetRotation = GetTargetRot(rotatingPage, ESides.Right);
        while (rotatingPage.rotation.eulerAngles.x != targetRotation.eulerAngles.x)
        {
            rotatingPage.rotation = Quaternion.RotateTowards(rotatingPage.rotation, targetRotation, turningRate * Time.deltaTime);

            if (Quaternion.Angle(rotatingPage.rotation, targetRotation) < 10f)
            {
                rightPage.gameObject.SetActive(false);
            }

            yield return null;
        }
        rightPage = rotatingPage;
        moving = false;
    }
}
