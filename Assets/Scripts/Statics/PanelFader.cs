using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    public static IEnumerator Fade(CanvasGroup group, float alphaTarget, float duration)
    {
        float counter = 0;
        float completion = 0;
        float alphaInit = group.alpha;
        float alphaDiff = alphaInit - alphaTarget;

        if (alphaDiff != 0)
        {
            while (counter < duration)
            {
                counter += Time.unscaledDeltaTime;
                completion = counter / duration;

                group.alpha = -Mathf.Clamp(completion, 0f, 1.0f) * alphaDiff + alphaInit;
                yield return null;
            }
        }
        else
        {
            Logger.Log("PanelFader", $"Initial alpha equal to target!");
        }

        group.alpha = alphaTarget;
    }

    public static IEnumerator Rescale(Transform panel, float scaleTarget, float duration)
    {
        float counter = 0;
        float completion = 0;
        float scaleInit = panel.localScale.x;
        float scaleDiff = scaleInit - scaleTarget;

        if (scaleDiff != 0)
        {
            while (counter < duration)
            {
                counter += Time.unscaledDeltaTime;
                completion = counter / duration;

                panel.localScale = new Vector2(-Mathf.Clamp(completion, 0.1f, 1.0f) * scaleDiff + scaleInit, -Mathf.Clamp(completion, 0.1f, 1.0f) * scaleDiff + scaleInit);
                yield return null;
            }
        }
        else
        {
            Logger.Log("PanelFader", $"Initial scale equal to target!");
        }

        panel.localScale = new Vector2(scaleTarget, scaleTarget);
    }

    public static IEnumerator RescaleAndFadePanel(Transform panel, CanvasGroup group,
    float scaleTarget, float alphaTarget, float duration)
    {
        float scaleInit = panel.localScale.x;
        float alphaInit = group.alpha;
        yield return RescaleAndFadePanel(panel, group, scaleInit, scaleTarget, alphaInit, alphaTarget, duration, null, null);
    }
    public static IEnumerator RescaleAndFadePanel(Transform panel, CanvasGroup group,
    float scaleTarget, float alphaTarget, float duration,
    Action onFadeStart, Action onFadeEnd)
    {
        float scaleInit = panel.localScale.x;
        float alphaInit = group.alpha;
        yield return RescaleAndFadePanel(panel, group, scaleInit, scaleTarget, alphaInit, alphaTarget, duration, onFadeStart, onFadeEnd);
    }
    public static IEnumerator RescaleAndFadePanel(Transform panel, CanvasGroup group,
    float scaleInit, float scaleFinal,
    float alphaInit, float alphaFinal,
    float duration)
    {
        yield return RescaleAndFadePanel(panel, group, scaleInit, scaleFinal, alphaInit, alphaFinal, duration, null, null);
    }
    public static IEnumerator RescaleAndFadePanel(Transform panel, CanvasGroup group,
    float scaleInit, float scaleFinal,
    float alphaInit, float alphaFinal,
    float duration,
    Action onFadeStart, Action onFadeEnd)
    {
        if (duration > 0f)
        {
            if (panel.gameObject.activeSelf == false)
                panel.gameObject.SetActive(true);

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

                    group.alpha = -Mathf.Clamp(completion, 0f, 1.0f) * alphaDiff + alphaInit;
                    float localScale = -Mathf.Clamp(completion, 0f, 1.0f) * scaleDiff + scaleInit;
                    panel.localScale = new Vector2(localScale, localScale);

                    yield return null;
                }
            }
            else
            {
                Logger.Log("PanelFader", $"Initial scale or alpha equal to target!");
            }

            if (onFadeEnd != null)
                onFadeEnd.Invoke();
        }

        group.alpha = alphaFinal;
        panel.localScale = new Vector2(scaleFinal, scaleFinal);

        if (group.alpha == 0f)
            panel.gameObject.SetActive(false);
    }

    public static IEnumerator RescaleAndFadeWindow(WindowRIP window, float scaleTarget, float alphaTarget, float delay, float duration)
    {
        float scaleInit = window.transform.localScale.x;
        float alphaInit = window.group.alpha;
        yield return RescaleAndFadeWindow(window, scaleInit, scaleTarget, alphaInit, alphaTarget, delay, duration, null, null);
    }
    public static IEnumerator RescaleAndFadeWindow(WindowRIP window, float scaleTarget, float alphaTarget, float delay, float duration, Action onFadeStart, Action onFadeEnd)
    {
        float scaleInit = window.transform.localScale.x;
        float alphaInit = window.group.alpha;
        yield return RescaleAndFadeWindow(window, scaleInit, scaleTarget, alphaInit, alphaTarget, delay, duration, onFadeStart, onFadeEnd);
    }
    public static IEnumerator RescaleAndFadeWindow(WindowRIP window, float scaleInit, float scaleFinal, float alphaInit, float alphaFinal, float delay, float duration)
    {
        yield return RescaleAndFadeWindow(window, scaleInit, scaleFinal, alphaInit, alphaFinal, delay, duration, null, null);
    }
    public static IEnumerator RescaleAndFadeWindow(WindowRIP window, float scaleInit, float scaleFinal, float alphaInit, float alphaFinal, float delay, float duration, Action onFadeStart, Action onFadeEnd)
    {
        if (delay > 0f)
            yield return new WaitForSecondsRealtime(delay);

        if (window.canvas.enabled == false)
            window.canvas.enabled = true;

        if (onFadeStart != null)
            onFadeStart.Invoke();

        yield return null;

        if (duration > 0f)
        {

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

                    window.group.alpha = -Mathf.Clamp(completion, 0f, 1.0f) * alphaDiff + alphaInit;
                    float localScale = -Mathf.Clamp(completion, 0f, 1.0f) * scaleDiff + scaleInit;
                    window.transform.localScale = new Vector2(localScale, localScale);

                    yield return null;
                }
            }
            else
            {
                Logger.Log("PanelFader", $"Initial scale or alpha equal to target!");
            }

        }

        if (onFadeEnd != null)
            onFadeEnd.Invoke();

        window.group.alpha = alphaFinal;
        window.transform.localScale = new Vector2(scaleFinal, scaleFinal);

        if (window.group.alpha == 0f)
            window.canvas.enabled = false;
    }
}