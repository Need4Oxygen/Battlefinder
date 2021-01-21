using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public Canvas canvas = null;
    public CanvasGroup group = null;

    [Space(15)]
    public bool setClosedOnAwake = true;

    [Space(15)]
    public float openScaleTarget = 1f;
    public float openAlphaTarget = 1f;

    [Space(15)]
    public float closeScaleTarget = 0.85f;
    public float closeAlphaTarget = 0f;

    [Space(15)]
    public float openDelay = 0f;
    public float closeDelay = 0f;
    public float closeDelayIfLast = 0.1f; // delay if this is the last opened window in the hierarchy, so camera have time to turn back on
    public float openCloseAnimDuration = 0.1f;

    void Awake()
    {
        if (setClosedOnAwake)
            StartCoroutine(PanelFader.RescaleAndFadeWindow(this, closeScaleTarget, closeAlphaTarget, 0f, 0f));
    }

    public void OpenWindow()
    {
        WindowManager.WindowOpened(this);
        StartCoroutine(PanelFader.RescaleAndFadeWindow(this, openScaleTarget, openAlphaTarget, openDelay, openCloseAnimDuration));
    }

    public void CloseWindow()
    {
        WindowManager.WindowClosed(this);
        StartCoroutine(PanelFader.RescaleAndFadeWindow(this, closeScaleTarget, closeAlphaTarget, WindowManager.OpenWindows.Count > 0 ? closeDelay : closeDelayIfLast, openCloseAnimDuration));
    }
}
