using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    [SerializeField] private bool skipIntro = false;

    [Space(15)]
    [SerializeField] private MainMenuCamera cameraController = null;

    [Space(15)]
    [SerializeField] private Window window = null;
    [SerializeField] private Button clickablePanel = null;
    [SerializeField] private CanvasGroup blackPanel = null;
    [SerializeField] private CanvasGroup disclaimerPanel = null;
    [SerializeField] private CanvasGroup battleFinderPanel = null;
    [SerializeField] private CanvasGroup clickToPanel = null;

    private bool splashing;

    public static DVoid OnSplashEnd;

    private void Start()
    {
        if (!Application.isEditor)
            skipIntro = false;

        if (!skipIntro && Time.time < 10f)
        {
            clickablePanel.interactable = false;

            blackPanel.alpha = 1f;
            battleFinderPanel.alpha = 0f;
            disclaimerPanel.alpha = 0f;
            clickToPanel.alpha = 0f;

            window.OpenWindow();

            StartCoroutine(ShowDisclaimer());
        }
        else
        {
            Audio.Instance.Play_Music("The Red Fox Tavern", 0f, 0f);
            Audio.Instance.Play_Ambient("Bar Night Busy", 0f, 0f);
            cameraController.ChangeToCloseCamera(true);
            Close_Splash();

            window.CloseWindow();
        }
    }

    private void Open_Splash()
    {
        window.OpenWindow();

        clickablePanel.interactable = false;
        blackPanel.alpha = 1f;
        battleFinderPanel.alpha = 0f;
        disclaimerPanel.alpha = 0f;
        clickToPanel.alpha = 0f;

        StartCoroutine(ShowDisclaimer());
    }

    private void Close_Splash()
    {
        window.CloseWindow();

        if (OnSplashEnd != null)
            OnSplashEnd();
    }

    private IEnumerator ShowDisclaimer()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(PanelFader.Fade(disclaimerPanel, 1f, 1f));
        yield return new WaitForSeconds(4f);
        StartCoroutine(PanelFader.Fade(disclaimerPanel, 0f, 0.5f));
        yield return new WaitForSeconds(2f);

        StartCoroutine(PanelFader.Fade(battleFinderPanel, 1f, 1f));
        Audio.Instance.Play_Music("The Red Fox Tavern", 0f, 0f);
        Audio.Instance.Play_Ambient("Bar Night Busy", 0f, 0f);
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
        cameraController.ChangeToCloseCamera(false);
        splashing = false;
        StopAllCoroutines();
        StartCoroutine(PanelFader.Fade(clickToPanel, 0f, 0.5f));
        StartCoroutine(PanelFader.Fade(blackPanel, 0f, 0.5f));
        Invoke("Close_Splash", 0.6f);
    }

}
