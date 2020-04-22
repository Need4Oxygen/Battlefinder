using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] CinemachineBrain brainCam = null;
    [SerializeField] CinemachineVirtualCamera closeUpCam = null;
    [SerializeField] Animator boxAnimator = null;

    [Space(15)]
    [SerializeField] Transform orbitCameraFollow = null;
    [SerializeField] Transform orbitCameraTarget = null;
    [SerializeField] float orbitSpeed = 1f;

    void Update()
    {
        orbitCameraFollow.RotateAround(orbitCameraTarget.position, Vector3.up, orbitSpeed * Time.deltaTime);

        if (Input.GetMouseButton(0) || Input.anyKeyDown)
        {
            closeUpCam.Priority = 12;
            StartCoroutine(WaitForBlend(brainCam.m_DefaultBlend.m_Time));
        }
    }

    IEnumerator WaitForBlend(float t)
    {
        yield return new WaitForSeconds(t);
        boxAnimator.SetTrigger("Open");
    }

}
