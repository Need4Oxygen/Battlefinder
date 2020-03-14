using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public static Draggable ItemBeingDragged;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnPointerDown();

        if (Input.GetMouseButtonUp(0))
            OnPointerUp();
    }

    void OnPointerDown()
    {
        cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Draggable script = hit.collider.GetComponent<Draggable>();
            if (script != null)
            {
                ItemBeingDragged = script;
                ItemBeingDragged.OnPointerDown();
            }
        }
    }

    void OnPointerUp()
    {
        if (ItemBeingDragged != null)
        {
            ItemBeingDragged.OnPointerUp();
            ItemBeingDragged = null;
        }
    }
}
