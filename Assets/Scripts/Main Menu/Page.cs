using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public bool hasTab;
    
    [HideInInspector]
    public float gradFadeOut;
    [HideInInspector]
    public float gradFadeIn;

    [SerializeField]
    private GameObject pageRotatingTab = null;
    [SerializeField]
    private Image[] gradients = null;

    private void OnEnable()
    {
        FadeGrad(false);
    }

    public void FadeGrad(bool hide)
    {
        if (hide)
        {
            foreach (Image grad in gradients)
            {
                grad.CrossFadeAlpha(1f, 0f, false);
                grad.CrossFadeAlpha(0f, gradFadeOut, false);
            }
        }
        else
        {
            foreach (Image grad in gradients)
            {
                grad.CrossFadeAlpha(0f, 0f, false);
                grad.CrossFadeAlpha(1f, gradFadeIn, false);
            }
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
