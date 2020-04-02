using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggablesMaster : MonoBehaviour
{
    public static Draggable ObjectBeingDragged;
    public static LayerMask Draggables;

    [SerializeField] private Camera cam = null;

    private E_Tools currentTool;

    void Awake()
    {
        Draggables = LayerMask.GetMask("Draggables");
    }

    void Update()
    {
        if (currentTool == E_Tools.None)
        {
            if (Input.GetMouseButtonDown(0))
                OnPointerDown();
            if (Input.GetMouseButtonUp(0))
                OnPointerUp();
        }
    }

    private void OnToolChange(E_Tools tool)
    {
        currentTool = tool;
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

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100f, Draggables))
        {
            Draggable draggable = hit.collider.GetComponent<Draggable>();
            if (draggable != null)
                return draggable;
        }
        return null;
    }

    private void SelectWithModifiers(Draggable draggable)
    {
        if (Input.GetKey(KeyCode.Delete))
            Destroy(draggable.gameObject);
        else if (Input.GetKey(KeyCode.LeftAlt) && draggable.canDuplicate)
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
