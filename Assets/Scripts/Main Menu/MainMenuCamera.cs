using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenuCamera : MonoBehaviour
{

    [SerializeField] CinemachineBrain brainCam = null;
    [SerializeField] CinemachineVirtualCamera closeUpCam = null;

    [Space(15)]
    [SerializeField] Animator boxAnimator = null;
    [SerializeField] AudioClip boxOpenClip = null;
    [SerializeField] AudioSource musicPlayer = null;

    [Space(15)]
    [SerializeField] Transform orbitCameraFollow = null;
    [SerializeField] Transform orbitCameraTarget = null;
    [SerializeField] float orbitSpeed = 1f;

    void Update()
    {
        orbitCameraFollow.RotateAround(orbitCameraTarget.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }

    public void ChangeToCloseCamera()
    {
        closeUpCam.Priority = 12;
        StartCoroutine(WaitForBlend(brainCam.m_DefaultBlend.m_Time));
    }

    private IEnumerator WaitForBlend(float t)
    {
        yield return new WaitForSeconds(t);
        boxAnimator.SetTrigger("Open");
        musicPlayer.PlayOneShot(boxOpenClip, 1f);
    }

}
