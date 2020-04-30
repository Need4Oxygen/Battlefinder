using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    public static IEnumerator Fade(CanvasGroup cg, float alphaTarget, float duration)
    {
        float counter = 0;
        float completion = 0;
        float alphaInit = cg.alpha;
        float alphaDiff = alphaInit - alphaTarget;

        if (alphaDiff != 0)
        {
            while (counter < duration)
            {
                counter += Time.unscaledDeltaTime;
                completion = counter / duration;

                cg.alpha = -Mathf.Clamp(completion, 0f, 1.0f) * alphaDiff + alphaInit;
                yield return null;
            }
        }
        else
        {
            Debug.Log("Fade failed: initial alpha equal to target");
        }

        cg.alpha = alphaTarget;
    }

    public static IEnumerator Rescale(Transform panelTransform, float scaleTarget, float duration)
    {
        float counter = 0;
        float completion = 0;
        float scaleInit = panelTransform.localScale.x;
        float scaleDiff = scaleInit - scaleTarget;

        if (scaleDiff != 0)
        {
            while (counter < duration)
            {
                counter += Time.unscaledDeltaTime;
                completion = counter / duration;

                panelTransform.localScale = new Vector2(-Mathf.Clamp(completion, 0.1f, 1.0f) * scaleDiff + scaleInit, -Mathf.Clamp(completion, 0.1f, 1.0f) * scaleDiff + scaleInit);
                yield return null;
            }
        }
        else
        {
            Debug.Log("Rescale failed: initial scale equal to target");
        }

        panelTransform.localScale = new Vector2(scaleTarget, scaleTarget);
    }

    public static IEnumerator RescaleAndFade(Transform panelTransform, CanvasGroup cg, float scaleTarget, float alphaTarget, float duration)
    {
        float scaleInit = panelTransform.localScale.x;
        float alphaInit = cg.alpha;
        yield return RescaleAndFade(panelTransform, cg, scaleInit, scaleTarget, alphaInit, alphaTarget, duration);
    }
    public static IEnumerator RescaleAndFade(Transform panelTransform, CanvasGroup canvasGroup,
    float scaleInit, float scaleFinal,
    float alphaInit, float alphaFinal,
    float duration)
    {
        if (duration > 0f)
        {

            yield return null;
            yield return null;

            if (panelTransform.gameObject.activeSelf == false)
                panelTransform.gameObject.SetActive(true);

            float counter = 0;
            float completion = 0;
            float scaleDiff = scaleInit - scaleFinal;
            float alphaDiff = alphaInit - alphaFinal;

            if (scaleDiff != 0 || alphaDiff != 0)
            {
                while (counter < duration)
                {
                    counter += Time.unscaledDeltaTime;
                    completion = counter / duration;

                    canvasGroup.alpha = -Mathf.Clamp(completion, 0f, 1.0f) * alphaDiff + alphaInit;
                    panelTransform.localScale = new Vector2(-Mathf.Clamp(completion, 0f, 1.0f) * scaleDiff + scaleInit,
                                                            -Mathf.Clamp(completion, 0f, 1.0f) * scaleDiff + scaleInit);
                    yield return null;
                }
            }
            else
            {
                Debug.Log("RescaleAndFade failed: initial scale or fade equal to target");
            }
        }

        canvasGroup.alpha = alphaFinal;
        panelTransform.localScale = new Vector2(scaleFinal, scaleFinal);

        if (canvasGroup.alpha == 0f)
            panelTransform.gameObject.SetActive(false);
    }
}