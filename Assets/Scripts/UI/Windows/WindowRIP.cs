using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowRIP : MonoBehaviour
{
    public string windowName = "";

    [Space(15)]
    public Canvas canvas = null;
    public CanvasGroup group = null;
    public GraphicRaycaster raycaster = null;

    [Space(15)]
    public bool startClosed = true;
    [Tooltip("Isolate raycast on self and children when opened.")]
    public bool raycastTarget = true;
    [Tooltip("If open, camera will stop rendering until it closes again.")]
    public bool stopCameraRender = true;

    [Space(15)]
    [Tooltip("Don't go back in raycast stack.")]
    public bool isSubpanel = false;
    [Tooltip("Activates raycast when opening, deactivates raycast when closing.")]
    public bool selfHandleRaycast = false;
    [Tooltip("When parent raycast activates, this window's raycast will activate too.")]
    public bool syncRaycastWithParent = false;


    [Space(15)]
    public float openScaleTarget = 1f;
    public float openAlphaTarget = 1f;

    [Space(15)]
    public float closeScaleTarget = 0.85f;
    public float closeAlphaTarget = 0f;

    [Space(15)]
    public float openDelay = 0f;
    public float closeDelay = 0f;
    [Tooltip("Delay if this is the last opened window in the hierarchy, so camera have time to turn back on.")]
    public float closeDelayIfLast = 0.1f;
    public float openCloseAnimDuration = 0.1f;

    [Space(15)]
    public List<WindowRIP> children = new List<WindowRIP>();

    [HideInInspector] public bool isOpen { get { return _isOpen; } set { } }
    private bool _isOpen = true;

    void Awake()
    {
        if (raycaster != null)
            raycaster.enabled = false;

        if (startClosed)
        {
            _isOpen = false;
            StartCoroutine(PanelFader.RescaleAndFadeWindow(this, closeScaleTarget, closeAlphaTarget, 0f, 0f));
        }
    }

    public void OpenWindow()
    {
        if (_isOpen != false) { canvas.enabled = true; return; }
        _isOpen = true;

        if (selfHandleRaycast)
            raycaster.enabled = true;

        WindowManagerRIP.WindowOpened(this);
        StartCoroutine(PanelFader.RescaleAndFadeWindow(this, openScaleTarget, openAlphaTarget, openDelay, openCloseAnimDuration));
    }

    public void CloseWindow()
    {
        if (_isOpen != true) return;
        _isOpen = false;

        if (selfHandleRaycast)
            raycaster.enabled = false;

        WindowManagerRIP.WindowClosed(this);
        StartCoroutine(PanelFader.RescaleAndFadeWindow(this, closeScaleTarget, closeAlphaTarget, WindowManagerRIP.OpenWindows.Count > 0 ? closeDelay : closeDelayIfLast, openCloseAnimDuration));
    }
}
