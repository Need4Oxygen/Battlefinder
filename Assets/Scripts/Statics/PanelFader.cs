using System;
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
            Debug.Log("[PanelFader] RescaleAndFade failed: initial scale equal to target");
        }

        panelTransform.localScale = new Vector2(scaleTarget, scaleTarget);
    }

    public static IEnumerator RescaleAndFade(Transform panelTransform, CanvasGroup canvasGroup,
    float scaleTarget, float alphaTarget, float duration)
    {
        float scaleInit = panelTransform.localScale.x;
        float alphaInit = canvasGroup.alpha;
        yield return RescaleAndFade(panelTransform, canvasGroup, scaleInit, scaleTarget, alphaInit, alphaTarget, duration, null, null);
    }
    public static IEnumerator RescaleAndFade(Transform panelTransform, CanvasGroup canvasGroup,
    float scaleTarget, float alphaTarget, float duration,
    Action onFadeStart, Action onFadeEnd)
    {
        float scaleInit = panelTransform.localScale.x;
        float alphaInit = canvasGroup.alpha;
        yield return RescaleAndFade(panelTransform, canvasGroup, scaleInit, scaleTarget, alphaInit, alphaTarget, duration, onFadeStart, onFadeEnd);
    }
    public static IEnumerator RescaleAndFade(Transform panelTransform, CanvasGroup canvasGroup,
    float scaleInit, float scaleFinal,
    float alphaInit, float alphaFinal,
    float duration)
    {
        yield return RescaleAndFade(panelTransform, canvasGroup, scaleInit, scaleFinal, alphaInit, alphaFinal, duration, null, null);
    }
    public static IEnumerator RescaleAndFade(Transform panelTransform, CanvasGroup canvasGroup,
    float scaleInit, float scaleFinal,
    float alphaInit, float alphaFinal,
    float duration,
    Action onFadeStart, Action onFadeEnd)
    {
        if (duration > 0f)
        {
            if (panelTransform.gameObject.activeSelf == false)
                panelTransform.gameObject.SetActive(true);

            yield return null;

            if (onFadeStart != null)
                onFadeStart.Invoke();

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
                Debug.Log("[PanelFader] RescaleAndFade failed: initial scale or fade equal to target");
            }

            if (onFadeEnd != null)
                onFadeEnd.Invoke();
        }

        canvasGroup.alpha = alphaFinal;
        panelTransform.localScale = new Vector2(scaleFinal, scaleFinal);

        if (canvasGroup.alpha == 0f)
            panelTransform.gameObject.SetActive(false);
    }
}