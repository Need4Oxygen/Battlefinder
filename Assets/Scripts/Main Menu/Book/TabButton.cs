using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup tabGroup = null;
    [SerializeField] private GameObject content = null;

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
        content.SetActive(false);
    }

    public void SwitchTab()
    {
        Transform targetParent = fakeTab.parent;
        Transform currentParent = transform.parent;
        int realSibling = transform.GetSiblingIndex();
        int fakeSibling = fakeTab.GetSiblingIndex();

        fakeTab.SetParent(currentParent, false);
        fakeTab.SetSiblingIndex(realSibling);

        transform.SetParent(targetParent, false);
        transform.SetSiblingIndex(fakeSibling);

        content.SetActive(true);
    }
}