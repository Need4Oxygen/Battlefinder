using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    [SerializeField] private bool skipIntro = false;

    [Space(15)]
    [SerializeField] private MainMenuCamera cameraController = null;
    [SerializeField] private AudioSource musicPlayer = null;
    [SerializeField] private AudioSource ambiencePlayer = null;

    [Space(15)]
    [SerializeField] private Button clickablePanel = null;
    [SerializeField] private GameObject splashPanel = null;
    [SerializeField] private CanvasGroup blackPanel = null;
    [SerializeField] private CanvasGroup disclaimerPanel = null;
    [SerializeField] private CanvasGroup battleFinderPanel = null;
    [SerializeField] private CanvasGroup clickToPanel = null;

    private bool splashing;

    public static DVoid OnSplashEnd;

    void Start()
    {
        if (!Application.isEditor)
            skipIntro = false;

        if (!skipIntro && Time.time < 10f)
        {
            clickablePanel.interactable = false;
            splashPanel.SetActive(true);
            blackPanel.alpha = 1f;
            battleFinderPanel.alpha = 0f;
            disclaimerPanel.alpha = 0f;
            clickToPanel.alpha = 0f;

            StartCoroutine(ShowDisclaimer());
        }
        else
        {
            splashPanel.SetActive(false);
            musicPlayer.Play();
            ambiencePlayer.Play();
            cameraController.ChangeToCloseCamera(true);
            DeactivateSplash();
        }
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
        ambiencePlayer.Play();
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

    //called by clickable panel
    public void StopSplashing()
    {
        cameraController.ChangeToCloseCamera(false);
        splashing = false;
        StopAllCoroutines();
        StartCoroutine(PanelFader.Fade(clickToPanel, 0f, 0.5f));
        StartCoroutine(PanelFader.Fade(blackPanel, 0f, 0.5f));
        Invoke("DeactivateSplash", 0.6f);
    }

    private void DeactivateSplash()
    {
        splashPanel.SetActive(false);
        if (OnSplashEnd != null)
            OnSplashEnd();
    }
}
