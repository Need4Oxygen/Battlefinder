using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggablesMaster : MonoBehaviour
{
    public static Draggable ObjectBeingDragged;
    public static LayerMask Board;

    [SerializeField] private Camera cam = null;

    [HideInInspector] public bool isLeftAltPressed;
    [HideInInspector] public bool isSuprPressed;

    void Awake()
    {
        Board = LayerMask.GetMask("Table");
    }

    public void OnPointerDown()
    {
        Draggable draggable = SearchDraggableUnderMouse();
        if (draggable != null)
        {
            SelectWithModifiers(draggable);
        }
    }

    public void OnPointerUp()
    {
        if (ObjectBeingDragged != null)
        {
            ObjectBeingDragged.OnDeselect();
            ObjectBeingDragged = null;
        }
    }

    private Draggable SearchDraggableUnderMouse()
    {
        cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Draggable draggable = hit.collider.GetComponent<Draggable>();
            if (draggable != null)
                return draggable;
        }
        return null;
    }

    private void SelectWithModifiers(Draggable draggable)
    {
        if (isSuprPressed)
            Destroy(draggable.gameObject);
        else if (isLeftAltPressed && draggable.canDuplicate)
            Select(draggable.Duplicate());
        else
            Select(draggable);
    }

    private void Select(Draggable draggable)
    {
        ObjectBeingDragged = draggable;
        ObjectBeingDragged.OnSelect();
    }

    private void Delete(Draggable draggable)
    {
        Destroy(draggable.gameObject);
    }
}
