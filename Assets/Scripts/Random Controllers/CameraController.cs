using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float camZoomStrength = 0.5f;
    [SerializeField] float camZoomMax = 12f;
    [SerializeField] float camZoomMin = 5f;

    bool draggingCamera = false;
    Vector3 lastMousePos;

    void OnMouseCenterDown()
    {
        draggingCamera = true;
    }
    void OnMouseCenterUp()
    {
        draggingCamera = false;
    }

    public void Update()
    {
        if (Input.mouseScrollDelta.y > 0 && transform.position.y < camZoomMax)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + camZoomStrength, transform.position.z);
        }
        else if (Input.mouseScrollDelta.y < 0 && transform.position.y > camZoomMin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - camZoomStrength, transform.position.z);
        }

        if (Input.GetMouseButtonDown(1))
            OnMouseCenterDown();

        if (Input.GetMouseButtonUp(1))
            OnMouseCenterUp();

        if (draggingCamera)
        {
            Vector3 mousePosDelta = Input.mousePosition - lastMousePos;

            transform.position = new Vector3(
                transform.position.x - mousePosDelta.x / 50f,
                transform.position.y,
                transform.position.z - mousePosDelta.y / 50f);
        }

        lastMousePos = Input.mousePosition;
    }
}
