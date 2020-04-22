using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public bool hasTab;

    [HideInInspector] public BookScript bookScript;

    [SerializeField] private GameObject pageRotatingTab = null;
    [SerializeField] private Image[] gradients = null;

    private void OnEnable()
    {
        ShowGradient();
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
