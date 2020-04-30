using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    [SerializeField] Button clickablePanel;
    [SerializeField] GameObject splashPanel;
    [SerializeField] CanvasGroup blackPanel;
    [SerializeField] CanvasGroup disclaimerPanel;
    [SerializeField] CanvasGroup battleFinderPanel;
    [SerializeField] CanvasGroup clickToPanel;

    [SerializeField] AudioSource musicPlayer;

    private bool splashing;

    void Start()
    {
        clickablePanel.interactable = false;
        splashPanel.SetActive(true);
        blackPanel.alpha = 1f;
        battleFinderPanel.alpha = 0f;
        disclaimerPanel.alpha = 0f;
        clickToPanel.alpha = 0f;

        StartCoroutine(ShowDisclaimer());
    }

    private IEnumerator ShowDisclaimer()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(PanelFader.Fade(disclaimerPanel, 1f, 1f));
        yield return new WaitForSeconds(4f);
        StartCoroutine(PanelFader.Fade(disclaimerPanel, 0f, 0.5f));
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(PanelFader.Fade(battleFinderPanel, 1f, 1f));
        musicPlayer.Play();
        yield return new WaitForSeconds(3f);
        StartCoroutine(PanelFader.Fade(battleFinderPanel, 0f, 2f));
        yield return new WaitForSeconds(2f);
        StartCoroutine(PanelFader.Fade(blackPanel, 0.3f, 6f));

        yield return new WaitForSeconds(2f);
        StartCoroutine(PanelFader.Fade(clickToPanel, 0.3f, 3f));
        clickablePanel.interactable = true;
        StartCoroutine(FadeLoop());
    }

    private IEnumerator FadeLoop()
    {
        splashing = true;
        while (splashing)
        {
            StartCoroutine(PanelFader.Fade(clickToPanel, 0.3f, 3f));
            yield return new WaitForSeconds(3f);
            StartCoroutine(PanelFader.Fade(clickToPanel, 0f, 3f));
            yield return new WaitForSeconds(3f);
        }
    }

    public void StopSplashing()
    {
        splashing = false;
        StopAllCoroutines();
        StartCoroutine(PanelFader.Fade(clickToPanel, 0f, 0.5f));
        StartCoroutine(PanelFader.Fade(blackPanel, 0f, 0.5f));
        Invoke("DeactivateSplash", 0.6f);
    }

    private void DeactivateSplash() { splashPanel.SetActive(false); }
}
