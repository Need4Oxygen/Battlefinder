using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Page : MonoBehaviour
{
    public bool hasTab;

    [HideInInspector] public BookScript bookScript;

    [SerializeField] private int pageNum = 0;
    [SerializeField] private TMP_Text[] pageNumTexts = null;
    [SerializeField] private GameObject pageRotatingTab = null;
    [SerializeField] private Image[] gradients = null;

    private void OnEnable()
    {
        ShowGradient();
        SetPageNums();
    }

    private void SetPageNums()
    {
        foreach (var tmpText in pageNumTexts)
        {
            tmpText.text = pageNum.ToString();
        }
    }

    public void HideGradient()
    {
        foreach (Image grad in gradients)
        {
            grad.CrossFadeAlpha(1f, 0f, false);
            grad.CrossFadeAlpha(0f, bookScript.gradFadeInTime, false);
        }
    }

    public void ShowGradient()
    {
        foreach (Image grad in gradients)
        {
            grad.CrossFadeAlpha(0f, 0f, false);
            grad.CrossFadeAlpha(1f, bookScript.gradFadeOutTime, false);
        }
    }

    public void ShowTab()
    {
        pageRotatingTab.SetActive(true);
    }

    public void HideTab()
    {
        pageRotatingTab.SetActive(false);
    }
}
