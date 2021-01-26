using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public string windowName = "";

    [Space(15)]
    public Canvas canvas = null;
    public CanvasGroup group = null;
    public GraphicRaycaster raycaster = null;

    [Space(15)]
    public bool startClosed = true;
    public bool raycastTarget = true;             // isolate raycast on self and children when opened

    [Space(15)]
    public bool isSubpanel = false;               // don't go back in raycast stack
    public bool syncRaycastWithParent = false;    // when parent raycast activates, this window's raycast will activate too

    [Space(15)]
    public float openScaleTarget = 1f;
    public float openAlphaTarget = 1f;

    [Space(15)]
    public float closeScaleTarget = 0.85f;
    public float closeAlphaTarget = 0f;

    [Space(15)]
    public float openDelay = 0f;
    public float closeDelay = 0f;
    public float closeDelayIfLast = 0.1f;          // delay if this is the last opened window in the hierarchy, so camera have time to turn back on
    public float openCloseAnimDuration = 0.1f;

    [Space(15)]
    public List<Window> children = new List<Window>();

    void Awake()
    {
        if (raycaster != null)
            raycaster.enabled = false;

        if (startClosed)
            StartCoroutine(PanelFader.RescaleAndFadeWindow(this, closeScaleTarget, closeAlphaTarget, 0f, 0f));
    }

    public void OpenWindow()
    {
        if (raycaster != null)
            raycaster.enabled = true;

        WindowManager.WindowOpened(this);
        StartCoroutine(PanelFader.RescaleAndFadeWindow(this, openScaleTarget, openAlphaTarget, openDelay, openCloseAnimDuration));
    }

    public void CloseWindow()
    {
        if (raycaster != null)
            raycaster.enabled = false;

        WindowManager.WindowClosed(this);
        StartCoroutine(PanelFader.RescaleAndFadeWindow(this, closeScaleTarget, closeAlphaTarget, WindowManager.OpenWindows.Count > 0 ? closeDelay : closeDelayIfLast, openCloseAnimDuration));
    }
}
