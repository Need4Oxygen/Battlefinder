using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{

    [SerializeField]
    CinemachineVirtualCamera closeUpCam = null;
    [SerializeField]
    CinemachineBrain brainCam = null;
    [SerializeField]
    Animator boxAnimator = null;
    [SerializeField]
    Transform tableTarget = null;
    [SerializeField]
    float orbitSpeed = 1f;




    // MAKE THE FCKING SPLASH PANEL


    void Update()
    {
        transform.RotateAround(tableTarget.position, Vector3.up, orbitSpeed * Time.deltaTime);


        if (Input.GetMouseButton(0)) //change for SplashClicked()
        {
            //hide Splash Panel
            closeUpCam.Priority = 12;
            StartCoroutine(WaitForBlend(brainCam.m_DefaultBlend.m_Time));
        }
    }
    
    /*
    public void SplashClicked()
    {
        //hide Splash Panel
        closeUpCam.Priority = 12;
        StartCoroutine(WaitForBlend(brainCam.m_DefaultBlend.m_Time));
    }
    */

    IEnumerator WaitForBlend(float t)
    {
        yield return new WaitForSeconds(t);
        boxAnimator.SetTrigger("Open");
    }
}
