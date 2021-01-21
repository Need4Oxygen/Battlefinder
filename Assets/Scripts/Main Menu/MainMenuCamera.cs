using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera = null;
    [SerializeField] CinemachineBrain brainCam = null;
    [SerializeField] CinemachineVirtualCamera closeUpCam = null;
    [SerializeField] Animator boxAnimator = null;

    [Header("Orbit Stuff")]
    [SerializeField] Transform orbitCameraFollow = null;
    [SerializeField] Transform orbitCameraTarget = null;
    [SerializeField] float orbitSpeed = 1f;

    [Header("Pan Stuff")]
    [SerializeField] float panStrengthX = 0.001f;
    [SerializeField] float panStrengthY = 0.001f;
    [Range(0, 1)]
    [SerializeField] float smoothness = 0.2f;

    private Vector3 iPos = Vector3.zero;
    private Coroutine rorateCorou = null;

    void Awake()
    {
        QualitySettings.vSyncCount = 1;  // VSync must be disabled
        Application.targetFrameRate = 0;

        WindowManager.OnWindowOpens += WindowOpensListener;
        WindowManager.OnWindowCloses += WindowClosesListener;
    }

    void Start()
    {
        iPos = closeUpCam.transform.position;
        rorateCorou = StartCoroutine(Rotate());
    }

    /// <summary>Called by Splash Controller when input is detected. </summary>
    public void ChangeToCloseCamera(bool isInstant)
    {
        closeUpCam.Priority = 12;
        if (!isInstant)
            StartCoroutine(WaitForBlend(brainCam.m_DefaultBlend.m_Time));
        else
            StartCoroutine(WaitForBlend(0f));
    }

    private IEnumerator WaitForBlend(float t)
    {
        yield return new WaitForSeconds(t);
        StopCoroutine(rorateCorou);
        StartCoroutine(PanWithMouse());
        boxAnimator.SetTrigger("Open");
        Audio.Instance.Play_Effect("Open Box", 0f);
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            orbitCameraFollow.RotateAround(orbitCameraTarget.position, Vector3.up, orbitSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator PanWithMouse()
    {
        Transform cam = closeUpCam.transform;
        Resolution res = Screen.currentResolution;

        while (true)
        {
            Vector3 h = cam.right * (Input.mousePosition.x - res.width / 4) * panStrengthX;
            Vector3 v = cam.up * (Input.mousePosition.y - res.height / 4) * panStrengthY;
            Vector3 targetPos = iPos + h + v;
            Vector3 dir = targetPos - cam.position;

            cam.position = cam.position + (dir * smoothness);
            yield return null;
        }
    }

    private void WindowOpensListener(Window window)
    {
        StartCoroutine(WindowOpensCorou(window));
    }
    private IEnumerator WindowOpensCorou(Window window)
    {
        yield return new WaitForSecondsRealtime(0.15f);
        mainCamera.enabled = false;
    }

    private void WindowClosesListener(Window window)
    {
        if (WindowManager.OpenWindows.Count == 0)
            StartCoroutine(WindowClosesCorou(window));
    }
    private IEnumerator WindowClosesCorou(Window window)
    {
        mainCamera.enabled = true;
        yield return null;
    }

}
