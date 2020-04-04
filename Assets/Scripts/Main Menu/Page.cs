using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public bool hasTab;
    public float gradFadeOut;
    public float gradFadeIn;

    [SerializeField]
    private GameObject tab = null;
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
        tab.SetActive(true);
    }

    public void HideTab()
    {
        tab.SetActive(false);
    }
}
