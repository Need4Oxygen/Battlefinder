using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    private TabGroup tabGroup = null;
    [SerializeField]
    private GameObject paper = null;

    public Image background;
    public int targetPage;
    public Transform fakeTab;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    private void Start()
    {
        tabGroup.Subscribe(this);
    }

    public void HideTab()
    {
        paper.SetActive(false);
    }

    public void SwitchTab()
    {
        Transform targetParent = fakeTab.parent;
        Transform currentParent = transform.parent;
        int realSibling = transform.GetSiblingIndex();
        int fakeSibling = fakeTab.GetSiblingIndex();
        /*int lastSibling = transform.parent.childCount - 1;
        int targetSibling = lastSibling - siblingIndex;*/

        fakeTab.SetParent(currentParent, false);
        fakeTab.SetSiblingIndex(realSibling);

        transform.SetParent(targetParent, false);
        transform.SetSiblingIndex(fakeSibling);

        paper.SetActive(true);


        /*
        if (transform.parent == tabGroup.tabsRight)
        {
            transform.SetParent(tabGroup.tabsLeft, false);
            SortChildren(tabGroup.tabsLeft);
        }
        else
        {
            transform.SetParent(tabGroup.tabsRight, false);
            SortChildren(tabGroup.tabsRight);
        }*/

        //gameObject.SetActive(true);
    }

    private void SortChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            children.Add(child);
            child.SetParent(null, false);
        }

        children.Sort((Transform t1, Transform t2) =>
        { return t1.GetComponent<TabButton>().targetPage.CompareTo(t2.GetComponent<TabButton>().targetPage); });

        foreach (Transform child in children)
        {
            child.SetParent(parent, false);
        }
    }
}