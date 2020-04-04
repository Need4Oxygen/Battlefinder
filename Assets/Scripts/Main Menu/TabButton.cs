using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    private TabGroup tabGroup = null;
    public Image background;
    public int targetPage;

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
        gameObject.SetActive(false);
    }

    public void SwitchTab()
    {
        if (transform.parent == tabGroup.tabsRight)
        {
            transform.SetParent(tabGroup.tabsLeft);
            ResetTransform();
        }
        else
        {
            transform.SetParent(tabGroup.tabsRight);
            ResetTransform();
        }
        gameObject.SetActive(true);
    }

    private void ResetTransform()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
