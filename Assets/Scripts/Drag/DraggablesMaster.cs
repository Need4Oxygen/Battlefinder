using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggablesMaster : MonoBehaviour
{
    public static Draggable ItemBeingDragged;
    public static LayerMask Board;

    [SerializeField] private Camera cam = null;

    private bool isLeftAltPressed;
    private bool isSuprPressed;

    void Awake()
    {
        Board = LayerMask.GetMask("Table");
    }

    void Update()
    {
        // Left Click
        if (Input.GetMouseButtonDown(0))
            OnPointerDown();
        if (Input.GetMouseButtonUp(0))
            OnPointerUp();

        // Left Alt
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            isLeftAltPressed = true;
        if (Input.GetKeyUp(KeyCode.LeftAlt))
            isLeftAltPressed = false;

        // Supr
        if (Input.GetKeyDown(KeyCode.Delete))
            isSuprPressed = true;
        if (Input.GetKeyUp(KeyCode.Delete))
            isSuprPressed = false;
    }

    private void OnPointerDown()
    {
        Draggable draggable = SearchDraggableUnderMouse();
        if (draggable != null)
        {
            SelectWithModifiers(draggable);
        }
    }

    private void OnPointerUp()
    {
        if (ItemBeingDragged != null)
        {
            ItemBeingDragged.OnDeselect();
            ItemBeingDragged = null;
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
            Destroy(draggable);
        else if (isLeftAltPressed && draggable.canDuplicate)
            Select(draggable.Duplicate());
        else
            Select(draggable);
    }

    private void Select(Draggable draggable)
    {
        ItemBeingDragged = draggable;
        ItemBeingDragged.OnSelect();
    }

    private void Delete(Draggable draggable)
    {
        Destroy(draggable.gameObject);
    }
}
