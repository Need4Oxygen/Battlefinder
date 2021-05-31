using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Fader
{

    public static IEnumerator Fade(VisualElement element, float alphaTarget, float duration, DVoid OnFadeStart = null, DVoid OnFadeEnd = null)
    {

        float alphaInit = element.style.opacity.value;
        yield return Fade(element, alphaInit, alphaTarget, duration, OnFadeStart, OnFadeEnd);

    }

    public static IEnumerator Fade(VisualElement element, float alphaInit, float alphaFinal, float duration, DVoid OnFadeStart = null, DVoid OnFadeEnd = null)
    {

        yield return null;
        if (OnFadeStart != null) OnFadeStart.Invoke();

        if (duration > 0f)
        {

            float counter = 0;
            float completion = 0;
            float alphaDiff = alphaInit - alphaFinal;

            if (alphaDiff == 0) { Logger.Log("PanelFader", $"Initial scale or alpha equal to target!"); yield break; }

            while (counter < duration)
            {
                counter += Time.unscaledDeltaTime;
                completion = counter / duration;

                element.style.opacity = -Mathf.Clamp(completion, 0f, 1.0f) * alphaDiff + alphaInit;

                yield return null;
            }

        }

        yield return null;
        if (OnFadeEnd != null) OnFadeEnd.Invoke();

    }

}
